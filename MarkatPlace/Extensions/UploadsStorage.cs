using Microsoft.Extensions.Diagnostics.HealthChecks;
using Shared.Constants;

namespace MarkatPlace.Extensions;

public static class UploadsStorage
{
    public const string ConfigurationKey = "FileStorage:UploadsRootPath";

    public static string ResolveRoot(IConfiguration configuration, string contentRootPath)
    {
        var configured = configuration[ConfigurationKey];

        return string.IsNullOrWhiteSpace(configured)
            ? Path.GetFullPath(Path.Combine(contentRootPath, "wwwroot", ImageConstants.UploadsRootFolder))
            : Path.GetFullPath(configured.Trim());
    }

    public static bool IsInside(string path, string directory)
    {
        var root = Path.GetFullPath(directory).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                   + Path.DirectorySeparatorChar;

        return Path.GetFullPath(path).StartsWith(root, StringComparison.OrdinalIgnoreCase);
    }
}

public sealed class UploadsStorageHealthCheck : IHealthCheck
{
    private readonly string _root;

    public UploadsStorageHealthCheck(string root)
    {
        _root = root;
    }

    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        if (!Directory.Exists(_root))
            return Task.FromResult(HealthCheckResult.Unhealthy("The uploads folder does not exist."));

        var probe = Path.Combine(_root, $".health-{Guid.NewGuid():N}");

        try
        {
            File.WriteAllText(probe, string.Empty);
            File.Delete(probe);
            return Task.FromResult(HealthCheckResult.Healthy());
        }
        catch (Exception exception)
        {
            return Task.FromResult(HealthCheckResult.Unhealthy("The uploads folder is not writable.", exception));
        }
    }
}
