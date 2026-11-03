using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Advertisements;
using Shared.Settings;

namespace Persistence.Services;

public class PrivateFileStore : IPrivateFileStore
{
    public const string DefaultFolderName = "private-uploads";

    private readonly IWebHostEnvironment _environment;
    private readonly FileStorageSettings _storage;

    public PrivateFileStore(IWebHostEnvironment environment, IOptions<FileStorageSettings> storage)
    {
        _environment = environment;
        _storage = storage.Value;
    }

    public async Task<StoredFile> SaveImageAsync(
        UploadImageModel file, string folder, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(file);

        var format = FileService.ValidateImage(file);
        var safeFolder = NormalizeFolder(folder);

        var directory = Path.Combine(Root(), safeFolder);
        Directory.CreateDirectory(directory);

        var fileName = $"{Guid.NewGuid():N}{format.CanonicalExtension}";
        var physicalPath = Path.Combine(directory, fileName);

        await using (var stream = new FileStream(
                         physicalPath, FileMode.CreateNew, FileAccess.Write, FileShare.None,
                         bufferSize: 81920, useAsync: true))
        {
            await stream.WriteAsync(file.Content, cancellationToken);
        }

        return new StoredFile
        {
            FileName = fileName,
            RelativePath = $"{safeFolder}/{fileName}",
            Url = string.Empty
        };
    }

    public PrivateFileContent? Open(string relativePath)
    {
        var physicalPath = Resolve(relativePath);

        if (physicalPath is null || !File.Exists(physicalPath))
            return null;

        var extension = Path.GetExtension(physicalPath);

        var contentType = ImageFormatCatalog.All
            .FirstOrDefault(format => format.Extensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
            ?.CanonicalContentType ?? "application/octet-stream";

        var stream = new FileStream(
            physicalPath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 81920, useAsync: true);

        return new PrivateFileContent(stream, contentType, Path.GetFileName(physicalPath));
    }

    public void Delete(string relativePath)
    {
        var physicalPath = Resolve(relativePath);

        if (physicalPath is not null && File.Exists(physicalPath))
            File.Delete(physicalPath);
    }

    public static string ResolveRoot(string? configured, string contentRootPath) =>
        string.IsNullOrWhiteSpace(configured)
            ? Path.GetFullPath(Path.Combine(contentRootPath, "App_Data", DefaultFolderName))
            : Path.GetFullPath(configured.Trim());

    private string Root()
    {
        var root = string.IsNullOrWhiteSpace(_storage.PrivateRootPath)
            ? ResolveRoot(null, _environment.ContentRootPath)
            : _storage.PrivateRootPath;

        Directory.CreateDirectory(root);

        return Path.GetFullPath(root).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
    }

    private string? Resolve(string relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
            return null;

        var root = Root();
        var normalized = relativePath.Trim().TrimStart('/', '\\').Replace('/', Path.DirectorySeparatorChar);
        var physicalPath = Path.GetFullPath(Path.Combine(root, normalized));

        return physicalPath.StartsWith(root + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)
            ? physicalPath
            : null;
    }

    private static string NormalizeFolder(string folder)
    {
        var trimmed = (folder ?? string.Empty).Trim().Trim('/', '\\');

        if (trimmed.Length == 0 || trimmed.Contains("..", StringComparison.Ordinal) || Path.IsPathRooted(trimmed))
            throw new ArgumentException($"Invalid private folder '{folder}'.", nameof(folder));

        return trimmed.Replace('\\', '/');
    }
}
