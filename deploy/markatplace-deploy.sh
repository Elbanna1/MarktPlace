#!/usr/bin/env bash
#
# MarkatPlace release tool.  See docs/DEPLOYMENT.md, "Release script".
#
#   sudo deploy/markatplace-deploy.sh diagnose           read-only health and consistency report
#   sudo deploy/markatplace-deploy.sh deploy             build the checked-out commit and release it
#   sudo deploy/markatplace-deploy.sh rollback <dir>     put a snapshot taken by 'deploy' back live
#
# Customer data (uploads, private payment files) is never written, moved or deleted: it is located,
# backed up and verified before anything changes, excluded from every copy, and re-verified after.
# Nothing here uses rsync --delete or removes anything outside RELEASES_DIR.

set -Eeuo pipefail

REPO_DIR="${REPO_DIR:-/home/MarktPlace}"
PROJECT="${PROJECT:-MarkatPlace/MarkatPlace.csproj}"
LIVE_DIR="${LIVE_DIR:-/var/www/backend}"
SERVICE="${SERVICE:-markatplace.service}"
RELEASES_DIR="${RELEASES_DIR:-/var/www/backend-releases}"
DATA_BACKUP_DIR="${DATA_BACKUP_DIR:-/var/backups/markatplace}"
LOCAL_URL="${LOCAL_URL:-http://127.0.0.1:5000}"
PUBLIC_URL="${PUBLIC_URL:-https://api.shopiklopik.com}"
KEEP_RELEASES="${KEEP_RELEASES:-5}"
ALLOW_CONFIG_CHANGES="${ALLOW_CONFIG_CHANGES:-0}"
KEEP_LIVE_CONFIG="${KEEP_LIVE_CONFIG:-0}"
SMOKE_USERNAME="${SMOKE_USERNAME:-}"

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
VERIFY_FSX="$SCRIPT_DIR/verify-assemblies.fsx"
ANON_PATH="/api/lookups/read-config/1"
AUTH_PATH="/api/profile"
TS="$(date +%Y%m%d-%H%M%S)"

say()  { printf '\n\033[1m== %s\033[0m\n' "$*"; }
ok()   { printf '   OK    %s\n' "$*"; }
warn() { printf '   WARN  %s\n' "$*"; }
die()  { printf '\n   FAIL  %s\n' "$*" >&2; exit 1; }

require_root() { [[ $EUID -eq 0 ]] || die "run with sudo (needs to read the service environment and restart it)"; }

SAFE_KEYS='ASPNETCORE_ENVIRONMENT|ASPNETCORE_URLS|DOTNET_[A-Z_]+|FileStorage__[A-Za-z]+'
redact_unit() { sed -E "/^Environment=\"?(${SAFE_KEYS})=/!s/^(Environment=\"?[^=]+)=.*/\1=<redacted>/"; }
redact_env()  { sed -E "/^(${SAFE_KEYS})=/!s/^([^=]+)=.*/\1=<redacted>/"; }

service_pid() { systemctl show -p MainPID --value "$SERVICE" 2>/dev/null || echo 0; }

service_env_value() {
    local key="$1" pid
    pid="$(service_pid)"
    if [[ "$pid" =~ ^[0-9]+$ && "$pid" -gt 0 && -r "/proc/$pid/environ" ]]; then
        tr '\0' '\n' < "/proc/$pid/environ" | sed -n "s/^${key}=//p" | tail -1
    else
        systemctl show -p Environment --value "$SERVICE" | tr ' ' '\n' | sed -n "s/^${key}=//p" | tail -1
    fi
}

content_root() {
    local pid wd
    pid="$(service_pid)"
    if [[ "$pid" =~ ^[0-9]+$ && "$pid" -gt 0 ]]; then
        readlink -f "/proc/$pid/cwd"
    else
        wd="$(systemctl show -p WorkingDirectory --value "$SERVICE")"
        echo "${wd:-$LIVE_DIR}"
    fi
}

json_setting() {
    local file="$1" section="$2" key="$3"
    [[ -f "$file" ]] || return 0
    python3 - "$file" "$section" "$key" <<'PY'
import json, sys
p, s, k = sys.argv[1:]
try:
    d = json.load(open(p, encoding='utf-8-sig'))
    v = (d.get(s) or {}).get(k)
    if isinstance(v, str) and v.strip():
        print(v.strip())
except Exception:
    pass
PY
}

resolve_setting() {
    local section="$1" key="$2" root="$3" v
    v="$(service_env_value "${section}__${key}")"
    [[ -n "$v" ]] || v="$(json_setting "$root/appsettings.Production.json" "$section" "$key")"
    [[ -n "$v" ]] || v="$(json_setting "$root/appsettings.json" "$section" "$key")"
    echo "$v"
}

resolve_data_dirs() {
    local root up pr logged
    root="$(content_root)"
    up="$(resolve_setting FileStorage UploadsRootPath "$root")"
    pr="$(resolve_setting FileStorage PrivateRootPath "$root")"
    [[ -n "$up" ]] || up="$root/wwwroot/uploads"
    [[ -n "$pr" ]] || pr="$root/App_Data/private-uploads"
    [[ "$up" = /* ]] || up="$root/$up"
    [[ "$pr" = /* ]] || pr="$root/$pr"
    UPLOADS_DIR="$(readlink -m "$up")"
    PRIVATE_DIR="$(readlink -m "$pr")"
    CONTENT_ROOT="$root"

    logged="$(journalctl -u "$SERVICE" --no-pager -o cat -n 20000 2>/dev/null \
              | sed -n 's/^ *Uploaded files are stored in \(.*\)\.$/\1/p' | tail -1)"
    if [[ -n "$logged" && "$(readlink -m "$logged")" != "$UPLOADS_DIR" ]]; then
        die "uploads path disagreement: computed $UPLOADS_DIR but the application last logged $logged. Inspect before continuing."
    fi
    check_path_layout
}

inside() { case "$(readlink -m "$1")/" in "$(readlink -m "$2")"/*) return 0 ;; esac; return 1; }

check_path_layout() {
    local d
    [[ "$RELEASES_DIR" = /* && "$DATA_BACKUP_DIR" = /* && "$LIVE_DIR" = /* ]] || die "LIVE_DIR, RELEASES_DIR and DATA_BACKUP_DIR must be absolute paths"
    [[ "$(readlink -m "$RELEASES_DIR")" != / && "$(readlink -m "$DATA_BACKUP_DIR")" != / ]] || die "RELEASES_DIR / DATA_BACKUP_DIR must not be /"
    for d in "$LIVE_DIR" "$DATA_BACKUP_DIR"; do
        inside "$d" "$RELEASES_DIR" || inside "$RELEASES_DIR" "$d" && die "$RELEASES_DIR overlaps $d (releases are pruned with rm -rf)"
    done
    inside "$DATA_BACKUP_DIR" "$LIVE_DIR" && die "DATA_BACKUP_DIR must not be inside $LIVE_DIR"
    for d in "$UPLOADS_DIR" "$PRIVATE_DIR"; do
        inside "$d" "$RELEASES_DIR" && die "customer data folder $d is inside $RELEASES_DIR (releases are pruned with rm -rf)"
        inside "$d" "$DATA_BACKUP_DIR" && die "customer data folder $d is inside $DATA_BACKUP_DIR"
    done
    return 0
}

data_relative_excludes() {
    EXCLUDES=(--exclude=/wwwroot/uploads --exclude=/App_Data)
    local d rel
    for d in "$UPLOADS_DIR" "$PRIVATE_DIR"; do
        case "$d/" in
            "$LIVE_DIR"/*) rel="${d#"$LIVE_DIR"}"; EXCLUDES+=("--exclude=${rel}") ;;
        esac
    done
}

manifest() {
    local dir="$1"
    if [[ -d "$dir" ]]; then (cd "$dir" && find . -type f -printf '%P\t%s\n' | LC_ALL=C sort); fi
}

assemblies_ok() {
    dotnet fsi "$VERIFY_FSX" "$1" 2>&1 | grep -v -e '^$' -e 'warning FS'
    return "${PIPESTATUS[0]}"
}

http_code() {
    curl -s -o /dev/null -w '%{http_code}' --max-time 15 "$@" || true
}

load_errors_since() {
    journalctl -u "$SERVICE" --since "$1" --no-pager -o cat 2>/dev/null | grep -c 'Could not load file or assembly' || true
}

smoke_test() {
    local base="$1" label="$2" code failures=0
    code="$(http_code "$base$ANON_PATH")"
    [[ "$code" == 200 ]] && ok "$label anonymous $ANON_PATH -> $code" || { warn "$label anonymous $ANON_PATH -> $code (want 200)"; failures=$((failures+1)); }
    code="$(http_code "$base$AUTH_PATH")"
    [[ "$code" == 401 ]] && ok "$label no token $AUTH_PATH -> $code" || { warn "$label no token $AUTH_PATH -> $code (want 401; 500 means the JWT stack failed to load)"; failures=$((failures+1)); }
    code="$(http_code -H 'Authorization: Bearer eyJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJ4In0.invalid' "$base$AUTH_PATH")"
    [[ "$code" == 401 ]] && ok "$label invalid token $AUTH_PATH -> $code" || { warn "$label invalid token $AUTH_PATH -> $code (want 401)"; failures=$((failures+1)); }
    return "$failures"
}

authenticated_test() {
    [[ -n "$SMOKE_USERNAME" ]] || { warn "SMOKE_USERNAME not set: skipped the valid-token test (set it to run a real authenticated request)"; return 0; }
    local pw="" body token code resp
    read -r -s -p "   password for $SMOKE_USERNAME (not echoed, not logged): " pw || true; echo
    body="$(SMOKE_U="$SMOKE_USERNAME" SMOKE_P="$pw" python3 -c 'import json,os; print(json.dumps({"username":os.environ["SMOKE_U"],"password":os.environ["SMOKE_P"]}))')"
    pw=""
    resp="$(curl -s --max-time 15 -w '\n%{http_code}' -H 'Content-Type: application/json' -d @- "$LOCAL_URL/api/auth/login" <<<"$body" || true)"
    body=""
    code="$(tail -n1 <<<"$resp")"
    if [[ "$code" != 200 ]]; then
        resp=""
        if [[ "$code" =~ ^5 || "$code" == 000 ]]; then warn "login -> $code (server error)"; return 1; fi
        warn "login as $SMOKE_USERNAME -> $code (credentials rejected; valid-token test not run, not counted as a release failure)"; return 0
    fi
    token="$(head -n -1 <<<"$resp" | python3 -c 'import json,sys
try: print(json.load(sys.stdin)["data"]["token"])
except Exception: pass')"
    resp=""
    [[ -n "$token" ]] || { warn "login returned 200 without a token"; return 1; }
    code="$(http_code -H "Authorization: Bearer $token" "$LOCAL_URL$AUTH_PATH")"
    token=""
    [[ "$code" == 200 ]] && ok "valid token $AUTH_PATH -> $code" || { warn "valid token $AUTH_PATH -> $code (want 200)"; return 1; }
}

cmd_diagnose() {
    require_root
    say "Service $SERVICE"
    systemctl is-active "$SERVICE" || true
    systemctl show "$SERVICE" -p MainPID -p NRestarts -p ActiveEnterTimestamp -p User -p WorkingDirectory -p ExecStart | sed 's/^/   /'
    say "Unit file (environment values redacted)"
    systemctl cat "$SERVICE" 2>/dev/null | redact_unit | sed 's/^/   /'
    say "Running process environment (names only, except paths)"
    local pid; pid="$(service_pid)"
    if [[ "$pid" -gt 0 && -r /proc/$pid/environ ]]; then tr '\0' '\n' < "/proc/$pid/environ" | grep -E '^(ASPNETCORE|DOTNET|FileStorage|ConnectionStrings|JwtSettings|App__)' | redact_env | sed 's/^/   /'; fi
    say ".NET on this host"
    dotnet --list-sdks | sed 's/^/   /'; dotnet --list-runtimes | grep -E 'NETCore|AspNetCore' | sed 's/^/   /'

    resolve_data_dirs
    say "Customer data locations"
    echo "   content root : $CONTENT_ROOT"
    local d
    for d in "$UPLOADS_DIR" "$PRIVATE_DIR"; do
        if [[ -d "$d" ]]; then
            printf '   %-60s files=%s size=%s owner=%s\n' "$d" "$(find "$d" -type f | wc -l)" "$(du -sh "$d" | cut -f1)" "$(stat -c %U:%G "$d")"
        else
            printf '   %-60s (does not exist)\n' "$d"
        fi
    done
    echo "   other non-release folders in $LIVE_DIR that may hold user files:"
    find "$LIVE_DIR" -mindepth 1 -maxdepth 2 -type d \( -iname '*upload*' -o -iname 'App_Data' -o -iname '*private*' -o -iname '*receipt*' -o -iname '*payment*' \) | sed 's/^/     /'

    say "IdentityModel assemblies in $LIVE_DIR"
    (cd "$LIVE_DIR" && for f in *IdentityModel*.dll; do printf '   %-55s %s  %s\n' "$f" "$(stat -c '%y' "$f" | cut -d. -f1)" "$(stat -c '%U:%G %a' "$f")"; done)
    grep -o '"Microsoft.IdentityModel[^"]*/[0-9.]*"\|"System.IdentityModel[^"]*/[0-9.]*"' "$LIVE_DIR/MarkatPlace.deps.json" | sort -u | sed 's/^/   deps.json: /'

    say "deps.json vs files on disk: $LIVE_DIR"
    assemblies_ok "$LIVE_DIR" | sed 's/^/   /' || true
    local b
    for b in /var/www/backend-backup-*; do
        [[ -d "$b" ]] || continue
        say "deps.json vs files on disk: $b"
        assemblies_ok "$b" | sed 's/^/   /' || true
    done

    say "Service user can read the application files"
    local user; user="$(systemctl show -p User --value "$SERVICE")"; user="${user:-root}"
    local unreadable; unreadable="$(find "$LIVE_DIR" -maxdepth 1 -type f \( -name '*.dll' -o -name '*.json' \) ! -perm -o+r ! -user "$user" | wc -l)"
    [[ "$unreadable" == 0 ]] && ok "every top-level dll/json is readable by $user" || warn "$unreadable top-level dll/json files may be unreadable by $user"

    say "Errors in the last 24h (counts)"
    local j; j="$(journalctl -u "$SERVICE" --since '-24h' --no-pager -o cat 2>/dev/null || true)"
    for pat in 'Could not load file or assembly' 'Unhandled exception' 'SqlException' 'PendingModelChangesWarning' 'Application started' 'Uploaded file not found on disk' 'stored inside the application directory'; do
        printf '   %6s  %s\n' "$(grep -c -- "$pat" <<<"$j" || true)" "$pat"
    done
    say "Distinct fail/crit messages in the last 24h"
    grep -A1 -E '^(fail|crit):' <<<"$j" | grep -v -E '^(fail|crit):|^--$' | sed 's/^ *//' | cut -c1-160 | sort | uniq -c | sort -rn | head -15 | sed 's/^/   /'

    say "Live HTTP checks"
    smoke_test "$LOCAL_URL" "local " || true
    smoke_test "$PUBLIC_URL" "public" || true

    say "Repository $REPO_DIR"
    git -C "$REPO_DIR" log --oneline -1 | sed 's/^/   HEAD /'
    git -C "$REPO_DIR" status --short | grep -v '^.. graphify-out/' | sed 's/^/   /' || true

    say "Disk"
    df -h "$LIVE_DIR" "$(dirname "$DATA_BACKUP_DIR")" | sed 's/^/   /'
}

backup_customer_data() {
    local dest="$DATA_BACKUP_DIR/$TS" d name need avail
    mkdir -p "$dest"
    need=0
    for d in "$UPLOADS_DIR" "$PRIVATE_DIR"; do [[ -d "$d" ]] && need=$((need + $(du -sk "$d" | cut -f1))); done
    avail="$(df -Pk "$dest" | awk 'NR==2{print $4}')"
    (( avail > need * 11 / 10 + 102400 )) || die "not enough space in $dest for a data backup (need ~$((need/1024)) MB, have $((avail/1024)) MB)"

    for d in "$UPLOADS_DIR" "$PRIVATE_DIR"; do
        [[ -d "$d" ]] || { warn "$d does not exist yet (nothing to back up)"; continue; }
        name="$(echo "$d" | tr '/' '_' | sed 's/^_//')"
        manifest "$d" > "$dest/$name.manifest"
        tar -C "$(dirname "$d")" -cpf "$dest/$name.tar" "$(basename "$d")"
        local in_tar in_dir
        in_tar="$(tar -tf "$dest/$name.tar" | grep -vc '/$' || true)"
        in_dir="$(wc -l < "$dest/$name.manifest")"
        [[ "$in_tar" == "$in_dir" ]] || die "backup of $d is incomplete ($in_tar files in tar, $in_dir on disk)"
        ok "backed up $d -> $dest/$name.tar ($in_dir files, verified)"
    done
    chmod -R go-rwx "$dest"
    BACKUP_PATH="$dest"
}

verify_customer_data_unchanged() {
    local d name before after missing
    for d in "$UPLOADS_DIR" "$PRIVATE_DIR"; do
        name="$(echo "$d" | tr '/' '_' | sed 's/^_//')"
        before="$BACKUP_PATH/$name.manifest"
        [[ -f "$before" ]] || continue
        after="$(mktemp)"
        manifest "$d" > "$after"
        missing="$(LC_ALL=C comm -23 "$before" "$after" | wc -l)"
        rm -f "$after"
        [[ "$missing" == 0 ]] && ok "$d: every file from before the release is present and unchanged" \
                              || die "$d: $missing files changed or disappeared during the release (the script never writes there, so check whether users deleted them; full copy: $BACKUP_PATH/$name.tar, list: $before)"
    done
}

sync_into_live() {
    local src="$1"
    rsync -a --checksum "${EXCLUDES[@]}" "$src"/ "$LIVE_DIR"/
}

config_differences() {
    local src="$1" f diffs=()
    for f in "$src"/appsettings*.json "$src"/web.config; do
        [[ -f "$f" ]] || continue
        if [[ -f "$LIVE_DIR/$(basename "$f")" ]] && ! cmp -s "$f" "$LIVE_DIR/$(basename "$f")"; then
            diffs+=("$(basename "$f")")
        fi
    done
    printf '%s\n' "${diffs[@]:-}"
}

restart_and_check() {
    local since="$1" i code restarts_before restarts_after failures=0
    restarts_before="$(systemctl show -p NRestarts --value "$SERVICE")"
    systemctl restart "$SERVICE"
    for i in $(seq 1 60); do
        code="$(http_code "$LOCAL_URL$ANON_PATH")"
        [[ "$code" == 200 ]] && break
        sleep 2
    done
    systemctl is-active --quiet "$SERVICE" || { warn "$SERVICE is not active"; return 1; }
    smoke_test "$LOCAL_URL" "local " || failures=$((failures+$?))
    authenticated_test || failures=$((failures+1))
    smoke_test "$PUBLIC_URL" "public" || warn "public checks failed while local checks decide the release; inspect Nginx/DNS if local passed"
    sleep 15
    restarts_after="$(systemctl show -p NRestarts --value "$SERVICE")"
    [[ "$restarts_after" == "$restarts_before" ]] && ok "service stayed up (NRestarts=$restarts_after)" || { warn "service restarted by systemd ($restarts_before -> $restarts_after)"; failures=$((failures+1)); }
    local n; n="$(load_errors_since "$since")"
    [[ "$n" == 0 ]] && ok "no assembly load errors since restart" || { warn "$n assembly load errors since restart"; failures=$((failures+1)); }
    return "$failures"
}

cmd_deploy() {
    require_root
    command -v rsync >/dev/null || die "rsync is not installed"
    command -v python3 >/dev/null || die "python3 is not installed"
    [[ -d "$LIVE_DIR" && -f "$LIVE_DIR/MarkatPlace.dll" ]] || die "$LIVE_DIR does not look like the live application"
    [[ -f "$REPO_DIR/$PROJECT" ]] || die "$REPO_DIR/$PROJECT not found"
    dotnet --list-sdks | grep -q '^10\.' || die "a .NET 10 SDK is required on this host"

    local commit release snapshot since
    commit="$(git -C "$REPO_DIR" rev-parse --short HEAD)"
    release="$RELEASES_DIR/$TS-$commit"
    snapshot="$RELEASES_DIR/$TS-previous-live"
    [[ ! -e "$release" ]] || die "$release already exists"

    say "1/8 Locate customer data"
    resolve_data_dirs
    data_relative_excludes
    echo "   uploads : $UPLOADS_DIR"
    echo "   private : $PRIVATE_DIR"
    echo "   rsync excludes: ${EXCLUDES[*]}"

    say "2/8 Back up customer data"
    backup_customer_data

    say "3/8 Build commit $commit into a fresh, empty release folder"
    if [[ -n "$(git -C "$REPO_DIR" status --porcelain -- . ':!graphify-out')" ]]; then warn "the repository has uncommitted changes; they are included in this build"; fi
    mkdir -p "$RELEASES_DIR"
    (cd "$REPO_DIR" && dotnet restore "$PROJECT" && dotnet build "$PROJECT" -c Release --no-restore \
        && dotnet publish "$PROJECT" -c Release --no-build -o "$release")
    git -C "$REPO_DIR" rev-parse HEAD > "$release/RELEASE_COMMIT"

    say "4/8 Verify the release"
    assemblies_ok "$release" | sed 's/^/   /' || die "release $release is inconsistent; production was not touched"
    [[ ! -d "$release/wwwroot/uploads" && ! -d "$release/App_Data" ]] || die "release contains data folders; production was not touched"
    local cfg f; cfg="$(config_differences "$release")"
    if [[ -n "$cfg" ]]; then
        echo "   configuration files that differ from the live copies (compare with: sudo diff $LIVE_DIR/<file> $release/<file>):"; sed 's/^/     /' <<<"$cfg"
        if [[ "$KEEP_LIVE_CONFIG" == 1 ]]; then
            while read -r f; do [[ -n "$f" ]] && EXCLUDES+=("--exclude=/$f"); done <<<"$cfg"
            ok "KEEP_LIVE_CONFIG=1: the live copies of those files are kept"
        elif [[ "$ALLOW_CONFIG_CHANGES" != 1 ]]; then
            die "review those differences (they may hold server-only edits), then re-run with KEEP_LIVE_CONFIG=1 (keep the live files) or ALLOW_CONFIG_CHANGES=1 (take the repository's); production was not touched"
        fi
    fi
    ok "release verified"

    say "5/8 Snapshot the current live application (for rollback)"
    rsync -a "${EXCLUDES[@]}" "$LIVE_DIR"/ "$snapshot"/
    ok "snapshot at $snapshot"

    say "6/8 Copy the release into $LIVE_DIR (no deletions, data folders excluded)"
    sync_into_live "$release"
    assemblies_ok "$LIVE_DIR" | sed 's/^/   /' || { warn "live folder inconsistent after copy; rolling back"; sync_into_live "$snapshot"; die "rolled back files; service was not restarted"; }

    say "7/8 Restart and verify"
    since="$(date '+%Y-%m-%d %H:%M:%S')"
    if ! restart_and_check "$since"; then
        warn "post-release checks failed; rolling back to $snapshot"
        sync_into_live "$snapshot"
        since="$(date '+%Y-%m-%d %H:%M:%S')"
        restart_and_check "$since" || true
        verify_customer_data_unchanged
        die "release $commit was rolled back. Customer data backup: $BACKUP_PATH"
    fi

    say "8/8 Customer data integrity"
    verify_customer_data_unchanged

    ls -1dt "$RELEASES_DIR"/*/ 2>/dev/null | tail -n +"$((KEEP_RELEASES * 2 + 1))" | while read -r old; do
        case "$old" in "$RELEASES_DIR"/*) rm -rf -- "$old" ;; esac
    done

    say "Released $commit"
    echo "   release folder : $release"
    echo "   rollback       : sudo $0 rollback $snapshot"
    echo "   data backup    : $BACKUP_PATH"
}

cmd_rollback() {
    require_root
    local snapshot="${1:-}" since
    [[ -n "$snapshot" && -f "$snapshot/MarkatPlace.dll" ]] || die "usage: $0 rollback <snapshot-or-release-dir from $RELEASES_DIR>"
    case "$(readlink -f "$snapshot")/" in "$RELEASES_DIR"/*|/var/www/backend-backup-*) ;; *) die "$snapshot is not under $RELEASES_DIR or a /var/www/backend-backup-* folder" ;; esac

    say "Verify the rollback source"
    assemblies_ok "$snapshot" | sed 's/^/   /' || die "$snapshot is itself inconsistent; refusing to roll back to it"
    [[ ! -d "$snapshot/wwwroot/uploads" || "$(find "$snapshot/wwwroot/uploads" -type f | wc -l)" == 0 ]] \
        || warn "$snapshot contains wwwroot/uploads files; they are excluded and will NOT be copied"

    resolve_data_dirs
    data_relative_excludes
    backup_customer_data

    say "Copy $snapshot into $LIVE_DIR (no deletions, data folders excluded)"
    rsync -a "${EXCLUDES[@]}" "$LIVE_DIR"/ "$RELEASES_DIR/$TS-before-rollback"/
    sync_into_live "$snapshot"
    assemblies_ok "$LIVE_DIR" | sed 's/^/   /' || die "live folder inconsistent after rollback copy; service NOT restarted. Previous state: $RELEASES_DIR/$TS-before-rollback"

    say "Restart and verify"
    since="$(date '+%Y-%m-%d %H:%M:%S')"
    restart_and_check "$since" || die "checks failed after rollback; previous state saved at $RELEASES_DIR/$TS-before-rollback"
    verify_customer_data_unchanged
    say "Rolled back to $snapshot"
}

case "${1:-}" in
    diagnose) cmd_diagnose ;;
    deploy)   cmd_deploy ;;
    rollback) shift; cmd_rollback "${1:-}" ;;
    *) echo "usage: sudo $0 {diagnose|deploy|rollback <dir>}"; exit 2 ;;
esac
