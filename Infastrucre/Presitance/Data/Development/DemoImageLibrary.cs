using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Advertisements;

namespace Persistence.Data.Development;

internal sealed class DemoImageLibrary
{
    internal sealed record DemoImage(string FileName, string RelativePath, string Url);

    private sealed class ManifestEntry
    {
        public string Folder { get; set; } = default!;
        public string FileName { get; set; } = default!;
        public string RelativePath { get; set; } = default!;
    }

    private static readonly JsonSerializerOptions ManifestOptions = new() { WriteIndented = true };

    private const int MaxParallelDownloads = 8;

    private readonly object _sync = new();

    private readonly IFileService _fileService;
    private readonly ILogger _logger;
    private readonly string _uploadsRoot;
    private readonly string _manifestPath;

    private readonly Dictionary<string, ManifestEntry> _manifest;

    private bool _manifestChanged;

    public int DownloadedCount { get; private set; }
    public int ReusedCount { get; private set; }
    public int FailedCount { get; private set; }

    private DemoImageLibrary(
        IFileService fileService,
        ILogger logger,
        string uploadsRoot,
        string manifestPath,
        Dictionary<string, ManifestEntry> manifest)
    {
        _fileService = fileService;
        _logger = logger;
        _uploadsRoot = uploadsRoot;
        _manifestPath = manifestPath;
        _manifest = manifest;
    }

    public static DemoImageLibrary Create(IFileService fileService, string uploadsRoot, ILogger logger)
    {
        foreach (var folder in ImageConstants.AllFolders)
            Directory.CreateDirectory(Path.Combine(uploadsRoot, folder));

        var manifestPath = Path.Combine(uploadsRoot, "demo-images.json");
        var manifest = new Dictionary<string, ManifestEntry>(StringComparer.OrdinalIgnoreCase);

        if (File.Exists(manifestPath))
        {
            try
            {
                var loaded = JsonSerializer.Deserialize<Dictionary<string, ManifestEntry>>(
                    File.ReadAllText(manifestPath));

                if (loaded is not null)
                {
                    foreach (var (sourceUrl, entry) in loaded)
                    {
                        var physicalPath = Path.Combine(uploadsRoot, entry.Folder, entry.FileName);
                        if (File.Exists(physicalPath))
                            manifest[sourceUrl] = entry;
                    }
                }
            }
            catch (Exception exception)
            {
                logger.LogWarning(exception,
                    "Demo image manifest at {ManifestPath} could not be read; it will be rebuilt.", manifestPath);
            }
        }

        return new DemoImageLibrary(fileService, logger, uploadsRoot, manifestPath, manifest);
    }

    public async Task<IReadOnlyList<DemoImage>> EnsureAsync(
        IReadOnlyList<string> sourceUrls,
        string folder,
        HttpClient httpClient,
        CancellationToken cancellationToken)
    {
        var slots = new DemoImage?[sourceUrls.Count];
        var pending = new List<int>();

        for (var index = 0; index < sourceUrls.Count; index++)
        {
            if (_manifest.TryGetValue(sourceUrls[index], out var existing) &&
                string.Equals(existing.Folder, folder, StringComparison.OrdinalIgnoreCase))
            {
                ReusedCount++;
                slots[index] = new DemoImage(
                    existing.FileName, existing.RelativePath, _fileService.BuildPublicUrl(existing.RelativePath));
                continue;
            }

            pending.Add(index);
        }

        if (pending.Count > 0)
        {
            using var concurrency = new SemaphoreSlim(MaxParallelDownloads);

            await Task.WhenAll(pending.Select(async index =>
            {
                await concurrency.WaitAsync(cancellationToken);
                try
                {
                    var downloaded = await TryDownloadAsync(
                        sourceUrls[index], folder, httpClient, cancellationToken);

                    lock (_sync)
                    {
                        if (downloaded is null)
                            FailedCount++;
                        else
                        {
                            DownloadedCount++;
                            slots[index] = downloaded;
                        }
                    }
                }
                finally
                {
                    concurrency.Release();
                }
            }));
        }

        return slots.Where(image => image is not null).Select(image => image!).ToList();
    }

    public void SaveManifest()
    {
        if (!_manifestChanged)
            return;

        try
        {
            File.WriteAllText(_manifestPath, JsonSerializer.Serialize(_manifest, ManifestOptions));
            _manifestChanged = false;
        }
        catch (Exception exception)
        {
            _logger.LogWarning(exception,
                "Demo image manifest could not be written to {ManifestPath}; images may be downloaded again.",
                _manifestPath);
        }
    }

    private async Task<DemoImage?> TryDownloadAsync(
        string sourceUrl, string folder, HttpClient httpClient, CancellationToken cancellationToken)
    {
        try
        {
            using var response = await httpClient.GetAsync(sourceUrl, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Demo image {SourceUrl} returned {StatusCode}; skipped.",
                    sourceUrl, (int)response.StatusCode);
                return null;
            }

            var contentType = NormalizeContentType(response.Content.Headers.ContentType);
            var extension = ExtensionFor(contentType);

            if (extension is null)
            {
                _logger.LogWarning("Demo image {SourceUrl} has unsupported content type {ContentType}; skipped.",
                    sourceUrl, contentType);
                return null;
            }

            var bytes = await response.Content.ReadAsByteArrayAsync(cancellationToken);

            if (bytes.Length == 0 || bytes.Length > ImageConstants.MaxFileSizeBytes)
            {
                _logger.LogWarning("Demo image {SourceUrl} is {Bytes} bytes, outside the accepted range; skipped.",
                    sourceUrl, bytes.Length);
                return null;
            }

            var stored = await _fileService.SaveAsync(
                new UploadImageModel
                {
                    FileName = $"demo{extension}",
                    ContentType = contentType,
                    Length = bytes.LongLength,
                    Content = bytes
                },
                folder,
                cancellationToken);

            lock (_sync)
            {
                _manifest[sourceUrl] = new ManifestEntry
                {
                    Folder = folder,
                    FileName = stored.FileName,
                    RelativePath = stored.RelativePath
                };
                _manifestChanged = true;
            }

            return new DemoImage(stored.FileName, stored.RelativePath, stored.Url);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception exception)
        {
            _logger.LogWarning(exception, "Demo image {SourceUrl} could not be downloaded; skipped.", sourceUrl);
            return null;
        }
    }

    private static string NormalizeContentType(MediaTypeHeaderValue? contentType) =>
        (contentType?.MediaType ?? string.Empty).ToLowerInvariant();

    private static string? ExtensionFor(string contentType) => contentType switch
    {
        "image/jpeg" or "image/jpg" => ".jpg",
        "image/png" => ".png",
        "image/webp" => ".webp",
        _ => null
    };

    public IReadOnlyDictionary<string, int> CountsByFolder() =>
        _manifest.Values
            .GroupBy(entry => entry.Folder, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(group => group.Key, group => group.Count(), StringComparer.OrdinalIgnoreCase);

    public string UploadsRoot => _uploadsRoot;
}
