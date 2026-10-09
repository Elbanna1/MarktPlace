using System.Reflection;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Persistence.Data;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Admin;
using Shared.Settings;

namespace Persistence.Services;

public sealed class UploadsDiagnostics : IUploadsDiagnostics
{
    public const int SampleSize = 20;

    private static readonly MethodInfo ReadPathsMethod =
        typeof(UploadsDiagnostics).GetMethod(nameof(ReadPathsAsync), BindingFlags.NonPublic | BindingFlags.Static)!;

    private readonly AppDbContext _context;
    private readonly FileStorageSettings _storage;
    private readonly IHostEnvironment _environment;

    public UploadsDiagnostics(AppDbContext context, IOptions<FileStorageSettings> storage, IHostEnvironment environment)
    {
        _context = context;
        _storage = storage.Value;
        _environment = environment;
    }

    public async Task<UploadsDiagnosticsDto> ScanAsync(CancellationToken cancellationToken = default)
    {
        var root = Path.GetFullPath(string.IsNullOrWhiteSpace(_storage.UploadsRootPath)
            ? Path.Combine(_environment.ContentRootPath, "wwwroot", ImageConstants.UploadsRootFolder)
            : _storage.UploadsRootPath);
        var imageTypes = _context.Model.GetEntityTypes()
            .Select(entity => entity.ClrType)
            .Where(type => type is { IsAbstract: false } && typeof(IListingImage).IsAssignableFrom(type))
            .ToList();

        var folders = new Dictionary<string, UploadsFolderDiagnosticsDto>(StringComparer.OrdinalIgnoreCase);
        var result = new UploadsDiagnosticsDto
        {
            StoredInsideApplicationDirectory = IsInside(root, _environment.ContentRootPath)
        };

        foreach (var type in imageTypes)
        {
            var paths = await (Task<List<string>>)ReadPathsMethod
                .MakeGenericMethod(type)
                .Invoke(null, [_context, cancellationToken])!;

            foreach (var relative in paths)
            {
                var folder = FolderOf(relative);
                if (!folders.TryGetValue(folder, out var entry))
                    folders[folder] = entry = new UploadsFolderDiagnosticsDto { Folder = folder };

                entry.Total++;
                result.TotalImages++;

                if (Exists(root, relative))
                    continue;

                entry.Missing++;
                result.MissingImages++;

                if (result.SampleMissingPaths.Count < SampleSize)
                    result.SampleMissingPaths.Add(relative);
            }
        }

        result.Folders = folders.Values.OrderByDescending(folder => folder.Missing).ThenBy(folder => folder.Folder).ToList();
        return result;
    }

    private static Task<List<string>> ReadPathsAsync<TImage>(AppDbContext context, CancellationToken cancellationToken)
        where TImage : class =>
        context.Set<TImage>()
            .IgnoreQueryFilters()
            .AsNoTracking()
            .Select(image => EF.Property<string>(image, nameof(IListingImage.ImagePath)))
            .ToListAsync(cancellationToken);

    public static bool Exists(string root, string relativePath)
    {
        var normalized = relativePath.Replace('\\', '/').TrimStart('/');
        var prefix = ImageConstants.UploadsRootFolder + "/";

        if (normalized.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            normalized = normalized[prefix.Length..];

        var physical = Path.GetFullPath(Path.Combine(root, normalized.Replace('/', Path.DirectorySeparatorChar)));

        return physical.StartsWith(root.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)
               && File.Exists(physical);
    }

    private static string FolderOf(string relativePath)
    {
        var segments = relativePath.Replace('\\', '/').Split('/', StringSplitOptions.RemoveEmptyEntries);
        var index = Array.FindIndex(segments, segment => segment.Equals(ImageConstants.UploadsRootFolder, StringComparison.OrdinalIgnoreCase));

        return index >= 0 && index + 1 < segments.Length - 1 ? segments[index + 1] : "(unknown)";
    }

    private static bool IsInside(string path, string directory) =>
        Path.GetFullPath(path).StartsWith(
            Path.GetFullPath(directory).TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar,
            StringComparison.OrdinalIgnoreCase);
}
