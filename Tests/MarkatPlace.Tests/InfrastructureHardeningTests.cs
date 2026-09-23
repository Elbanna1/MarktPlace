using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Persistence.Configurations;
using Persistence.Data;
using Persistence.Listings;
using Persistence.Repositories;
using Shared.Constants;
using Xunit;

namespace MarkatPlace.Tests;

/// <summary>
/// Covers the parts of the hardening pass that live outside C# semantics: the shape of the SQL the
/// profile aggregates compile to, the listing sort indexes, and the committed Nginx site.
/// </summary>
public class InfrastructureHardeningTests
{
    private const string NginxSite = "deploy/nginx/api.shopiklopik.com.conf";

    private static AppDbContext ModelOnlyContext() =>
        new(new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer("Server=localhost;Database=ModelOnly;Trusted_Connection=True;")
            .Options);

    private static IModel Model()
    {
        using var context = ModelOnlyContext();

        return context.GetService<IDesignTimeModel>().Model;
    }

    private static UserListingRepository Repository(AppDbContext context)
    {
        var sources = typeof(AdvertisementListingSource).Assembly
            .GetTypes()
            .Where(type => type is { IsClass: true, IsAbstract: false } &&
                           typeof(IUserListingSource).IsAssignableFrom(type))
            .Select(type => (IUserListingSource)Activator.CreateInstance(type, context)!)
            .ToList();

        Assert.True(sources.Count > 40, $"Only {sources.Count} listing sources were discovered.");

        return new UserListingRepository(sources, context);
    }

    // The profile totals used to be counted in application memory. These assert the replacement is
    // genuinely a server-side aggregate: SQL Server translates the whole 49-way union, and the
    // statement it produces returns one scalar rather than a row per listing.
    [Fact]
    public async Task The_owners_totals_are_aggregated_by_SQL_Server_and_not_in_memory()
    {
        using var context = ModelOnlyContext();

        var repository = Repository(context);

        // A model-only context has no database to open, so the call must fail on the connection,
        // never on translation. Anything else means the query cannot be expressed in SQL.
        var failure = await Record.ExceptionAsync(() =>
            repository.GetOwnerTotalsAsync("some-user", DateTime.UtcNow));

        Assert.NotNull(failure);

        Assert.DoesNotContain("could not be translated", failure.ToString(), StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("client evaluation", failure.ToString(), StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task The_status_counts_are_grouped_by_SQL_Server_and_not_in_memory()
    {
        using var context = ModelOnlyContext();

        var repository = Repository(context);

        var failure = await Record.ExceptionAsync(() =>
            repository.GetStatusCountsAsync("some-user", DateTime.UtcNow));

        Assert.NotNull(failure);

        Assert.DoesNotContain("could not be translated", failure.ToString(), StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Every_module_that_offers_a_sort_has_an_index_shaped_like_that_sort()
    {
        var model = Model();

        var missing = new List<string>();

        foreach (var (clrType, sorts) in ListingSortIndexCatalog.ForEntity)
        {
            var entity = model.FindEntityType(clrType);

            Assert.NotNull(entity);

            foreach (var sort in sorts)
            {
                var match = entity!.GetIndexes().FirstOrDefault(index =>
                    index.Name?.EndsWith($"_Feed_{sort.Suffix}", StringComparison.Ordinal) == true);

                if (match is null)
                {
                    missing.Add($"{clrType.Name}.{sort.Column} ({sort.Suffix})");
                    continue;
                }

                Assert.Equal("ModerationStatus", match.Properties[0].Name);
                Assert.Equal("Id", match.Properties[^1].Name);

                var position = match.Properties
                    .Select((property, i) => (property.Name, i))
                    .First(pair => pair.Name == sort.Column).i;

                Assert.Equal(sort.Descending, match.IsDescending![position]);

                Assert.Contains(match.GetIncludeProperties() ?? [], name => name == "IsDeleted");
            }
        }

        Assert.True(missing.Count == 0, "No sort index for: " + string.Join(", ", missing));
    }

    [Fact]
    public void A_sort_index_is_only_added_where_the_module_really_offers_that_sort()
    {
        var model = Model();

        var stray = model.GetEntityTypes()
            .Where(entity => !ListingSortIndexCatalog.ForEntity.ContainsKey(entity.ClrType))
            .Where(entity => entity.ClrType.Name != "Advertisement")
            .SelectMany(entity => entity.GetIndexes()
                .Where(index => index.Name?.Contains("_Feed_", StringComparison.Ordinal) == true)
                .Select(index => $"{entity.ClrType.Name}: {index.Name}"))
            .ToList();

        Assert.True(stray.Count == 0, "Unexpected feed sort index on: " + string.Join(", ", stray));
    }

    [Fact]
    public void The_ascending_and_descending_price_sorts_get_an_index_each()
    {
        var model = Model();

        foreach (var clrType in ListingSortIndexCatalog.ForEntity
                     .Where(pair => pair.Value.Count(sort => sort.Column is "Price" or "TotalPrice") == 2)
                     .Select(pair => pair.Key))
        {
            var entity = model.FindEntityType(clrType)!;

            var priceIndexes = entity.GetIndexes()
                .Where(index => index.Properties.Any(property =>
                    property.Name is "Price" or "TotalPrice"))
                .ToList();

            Assert.True(priceIndexes.Count >= 2,
                $"{clrType.Name} offers both price directions but has {priceIndexes.Count} price index(es). " +
                "SQL Server can only read an index forwards or exactly backwards, so each direction " +
                "needs its own.");
        }
    }

    [Fact]
    public void The_committed_Nginx_site_is_balanced_and_every_directive_is_terminated()
    {
        var config = RepositoryRoot.ReadFile(NginxSite);

        var depth = 0;

        foreach (var (line, number) in Lines(config))
        {
            depth += line.Count(character => character == '{');
            depth -= line.Count(character => character == '}');

            Assert.True(depth >= 0, $"Nginx site closes a block that was never opened at line {number}.");
        }

        Assert.Equal(0, depth);

        foreach (var (line, number) in Lines(config))
        {
            if (line.EndsWith('{') || line.EndsWith('}') || line.EndsWith(';'))
                continue;

            Assert.Fail($"Nginx directive is not terminated at line {number}: {line}");
        }
    }

    [Fact]
    public void Kestrel_is_reached_through_a_keep_alive_upstream()
    {
        var config = RepositoryRoot.ReadFile(NginxSite);

        Assert.Matches(new Regex(@"upstream\s+markatplace\s*\{[^}]*keepalive\s+\d+;", RegexOptions.Singleline), config);

        Assert.DoesNotContain("proxy_pass http://127.0.0.1:5000;", config, StringComparison.Ordinal);
        Assert.Contains("proxy_pass http://markatplace;", config, StringComparison.Ordinal);

        // Upstream keep-alive only works over HTTP/1.1 with the hop-by-hop Connection header
        // cleared, which is exactly what the $connection_upgrade map does for a non-upgrade request.
        Assert.Contains("proxy_http_version 1.1;", config, StringComparison.Ordinal);
        Assert.Matches(new Regex(@"map\s+\$http_upgrade\s+\$connection_upgrade\s*\{[^}]*''\s+'';", RegexOptions.Singleline), config);
    }

    [Fact]
    public void The_realtime_hub_still_forwards_the_WebSocket_upgrade()
    {
        var config = RepositoryRoot.ReadFile(NginxSite);

        var hub = config[config.IndexOf("location /hubs/", StringComparison.Ordinal)..];

        Assert.Contains("proxy_set_header Upgrade           $http_upgrade;", hub, StringComparison.Ordinal);
        Assert.Contains("proxy_set_header Connection        $connection_upgrade;", hub, StringComparison.Ordinal);
        Assert.Contains("proxy_buffering off;", hub, StringComparison.Ordinal);
    }

    [Fact]
    public void Every_proxied_location_still_forwards_the_host_and_client_address()
    {
        var config = RepositoryRoot.ReadFile(NginxSite);

        foreach (var marker in new[] { "location / {\n        proxy_pass", "location /hubs/" })
        {
            var index = config.Replace("\r\n", "\n", StringComparison.Ordinal)
                .IndexOf(marker, StringComparison.Ordinal);

            Assert.True(index >= 0, $"Could not find {marker} in the Nginx site.");
        }

        foreach (var header in new[]
        {
            "X-Real-IP         $remote_addr",
            "X-Forwarded-For   $proxy_add_x_forwarded_for",
            "X-Forwarded-Proto $scheme",
            "X-Forwarded-Host  $host"
        })
        {
            Assert.Equal(2, Regex.Matches(config, Regex.Escape(header)).Count);
        }
    }

    [Fact]
    public void Nginx_serves_uploads_from_disk_for_exactly_the_formats_the_application_accepts()
    {
        var config = RepositoryRoot.ReadFile(NginxSite);

        var match = Regex.Match(config, @"location \~\*\s+\^/uploads/\.\+\\\.\(([a-z0-9|]+)\)\$");

        Assert.True(match.Success, "The Nginx site does not serve /uploads from disk.");

        var served = match.Groups[1].Value.Split('|').ToHashSet(StringComparer.Ordinal);

        var accepted = ImageFormatCatalog.AllExtensions
            .Concat(DocumentFormatCatalog.AllExtensions)
            .Concat(VideoFormatCatalog.AllExtensions)
            .Select(extension => extension.TrimStart('.'))
            .ToHashSet(StringComparer.Ordinal);

        Assert.True(served.SetEquals(accepted),
            "Nginx and the application disagree about which uploads are servable. " +
            $"Only Nginx: {string.Join(", ", served.Except(accepted))}. " +
            $"Only the application: {string.Join(", ", accepted.Except(served))}.");
    }

    [Fact]
    public void The_uploads_location_keeps_the_headers_the_application_used_to_send()
    {
        var config = RepositoryRoot.ReadFile(NginxSite);

        var uploads = config[config.IndexOf("location ~*", StringComparison.Ordinal)..];
        uploads = uploads[..uploads.IndexOf("location /uploads/", StringComparison.Ordinal)];

        Assert.Contains("public,max-age=31536000,immutable", uploads, StringComparison.Ordinal);
        Assert.Contains("Access-Control-Allow-Origin \"*\"", uploads, StringComparison.Ordinal);
        Assert.Contains("default-src 'none'; sandbox", uploads, StringComparison.Ordinal);
        Assert.Contains("try_files $uri =404;", uploads, StringComparison.Ordinal);

        // A `^~` prefix match would suppress the regex location above and 404 every image.
        Assert.DoesNotContain("location ^~ /uploads/", config, StringComparison.Ordinal);
        Assert.Contains("location /uploads/ {", config, StringComparison.Ordinal);
    }

    [Fact]
    public void Every_extension_Nginx_serves_is_given_an_explicit_content_type()
    {
        var config = RepositoryRoot.ReadFile(NginxSite);

        var types = config[config.IndexOf("types {", StringComparison.Ordinal)..];
        types = types[..types.IndexOf('}')];

        var mapped = types
            .Split(';', StringSplitOptions.RemoveEmptyEntries)
            .Select(entry => entry.Trim().Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries))
            .Where(parts => parts.Length > 1)
            .SelectMany(parts => parts.Skip(1))
            .ToHashSet(StringComparer.Ordinal);

        var accepted = ImageFormatCatalog.AllExtensions
            .Concat(DocumentFormatCatalog.AllExtensions)
            .Concat(VideoFormatCatalog.AllExtensions)
            .Select(extension => extension.TrimStart('.'))
            .ToHashSet(StringComparer.Ordinal);

        Assert.True(accepted.IsSubsetOf(mapped),
            "These uploads would be served as application/octet-stream: " +
            string.Join(", ", accepted.Except(mapped)));
    }

    private static IEnumerable<(string Line, int Number)> Lines(string config)
    {
        var number = 0;

        foreach (var raw in config.Replace("\r\n", "\n", StringComparison.Ordinal).Split('\n'))
        {
            number++;

            var line = raw.Trim();

            if (line.Length == 0 || line.StartsWith('#'))
                continue;

            yield return (line, number);
        }
    }
}
