open System
open System.IO
open System.Reflection
open System.Text.Json
open System.Diagnostics

let args = fsi.CommandLineArgs |> Array.skip 1
if args.Length <> 1 then
    eprintfn "usage: dotnet fsi verify-assemblies.fsx <publish-or-live-dir>"
    exit 2

let dir = Path.GetFullPath args.[0]
let depsPath = Path.Combine(dir, "MarkatPlace.deps.json")
if not (File.Exists depsPath) then
    eprintfn "FAIL: %s not found" depsPath
    exit 2

let doc = JsonDocument.Parse(File.ReadAllText depsPath)
let root = doc.RootElement
let targetName = root.GetProperty("runtimeTarget").GetProperty("name").GetString()
let target = root.GetProperty("targets").GetProperty(targetName)

let mutable checkedCount = 0
let problems = Collections.Generic.List<string>()

for lib in target.EnumerateObject() do
    match lib.Value.TryGetProperty "runtime" with
    | true, runtime ->
        for asset in runtime.EnumerateObject() do
            let file = Path.Combine(dir, Path.GetFileName asset.Name)
            checkedCount <- checkedCount + 1
            if not (File.Exists file) then
                problems.Add(sprintf "MISSING  %-55s expected by %s" (Path.GetFileName file) lib.Name)
            else
                match asset.Value.TryGetProperty "assemblyVersion" with
                | true, expected ->
                    let actual = AssemblyName.GetAssemblyName(file).Version.ToString()
                    if actual <> expected.GetString() then
                        problems.Add(sprintf "VERSION  %-55s deps.json=%s on-disk=%s" (Path.GetFileName file) (expected.GetString()) actual)
                    else
                        match asset.Value.TryGetProperty "fileVersion" with
                        | true, fv ->
                            let actualFv = FileVersionInfo.GetVersionInfo(file).FileVersion
                            if actualFv <> fv.GetString() then
                                problems.Add(sprintf "BUILD    %-55s deps.json=%s on-disk=%s" (Path.GetFileName file) (fv.GetString()) actualFv)
                        | _ -> ()
                | _ -> ()
    | _ -> ()

for required in [ "MarkatPlace.runtimeconfig.json" ] do
    if not (File.Exists(Path.Combine(dir, required))) then
        problems.Add(sprintf "MISSING  %-55s required to start the application" required)

let runtimeFiles =
    target.EnumerateObject()
    |> Seq.collect (fun lib ->
        match lib.Value.TryGetProperty "runtime" with
        | true, r -> r.EnumerateObject() |> Seq.map (fun a -> Path.GetFileName a.Name)
        | _ -> Seq.empty)
    |> Set.ofSeq

let requiredAssemblies =
    [ "MarkatPlace.dll"; "Domain.dll"; "Services.dll"; "ServicesAbstraction.dll"; "Presitance.dll"; "Presantion.dll"; "Shared.dll"
      "Microsoft.IdentityModel.Tokens.dll"; "Microsoft.IdentityModel.JsonWebTokens.dll"
      "Microsoft.IdentityModel.Protocols.dll"; "Microsoft.IdentityModel.Protocols.OpenIdConnect.dll"
      "System.IdentityModel.Tokens.Jwt.dll"; "Microsoft.AspNetCore.Authentication.JwtBearer.dll"
      "Microsoft.EntityFrameworkCore.dll"; "Microsoft.EntityFrameworkCore.SqlServer.dll"; "Microsoft.EntityFrameworkCore.Relational.dll"
      "Swashbuckle.AspNetCore.Swagger.dll"; "Swashbuckle.AspNetCore.SwaggerGen.dll"; "Swashbuckle.AspNetCore.SwaggerUI.dll" ]

for name in requiredAssemblies do
    if not (runtimeFiles.Contains name) then
        problems.Add(sprintf "DEPS     %-55s not listed in MarkatPlace.deps.json (single-file or broken publish?)" name)
    elif not (File.Exists(Path.Combine(dir, name))) && not (problems |> Seq.exists (fun p -> p.Contains name)) then
        problems.Add(sprintf "MISSING  %-55s required assembly" name)

if problems.Count = 0 then
    printfn "OK: %d runtime assemblies in %s match MarkatPlace.deps.json" checkedCount dir
    exit 0
else
    for p in problems do eprintfn "%s" p
    eprintfn "FAIL: %d problems (%d runtime assemblies listed in MarkatPlace.deps.json) in %s" problems.Count checkedCount dir
    exit 1
