using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Advertisements;
using Shared.Exceptions;
using Shared.Settings;

namespace Persistence.Services;

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _environment;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;
    private readonly FileStorageSettings _storage;

    public FileService(
        IWebHostEnvironment environment,
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration,
        IOptions<FileStorageSettings> storage)
    {
        _environment = environment;
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
        _storage = storage.Value;
    }

    public Task<StoredFile> SaveAsync(UploadImageModel file, string folder, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(file);

        var format = ValidateImage(file);
        return WriteAsync(file, format.CanonicalExtension, folder, cancellationToken);
    }

    public Task<StoredFile> SaveDocumentAsync(
        UploadImageModel file, string folder, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(file);

        var format = ValidateDocument(file);
        return WriteAsync(file, format.CanonicalExtension, folder, cancellationToken);
    }

    public Task<StoredFile> SaveVideoAsync(
        UploadImageModel file, string folder, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(file);

        var format = ValidateVideo(file);
        return WriteAsync(file, format.CanonicalExtension, folder, cancellationToken);
    }

    public async Task<IReadOnlyList<StoredFile>> SaveImagesAsync(
        IReadOnlyList<UploadImageModel> files,
        string folder,
        int existingCount = 0,
        CancellationToken cancellationToken = default)
    {
        if (files is null || files.Count == 0)
            return Array.Empty<StoredFile>();

        ValidateCount(files.Count, existingCount);
        var formats = files.Select(ValidateImage).ToList();

        var stored = new List<StoredFile>(files.Count);
        try
        {
            for (var index = 0; index < files.Count; index++)
                stored.Add(await WriteAsync(
                    files[index], formats[index].CanonicalExtension, folder, cancellationToken));
        }
        catch
        {
            foreach (var written in stored)
                Delete(written.RelativePath);
            throw;
        }

        return stored;
    }

    public void Delete(string relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
            return;

        var uploadsRoot = GetUploadsRootPath();

        var normalized = relativePath.TrimStart('/', '\\');
        if (normalized.StartsWith($"{ImageConstants.UploadsRootFolder}/", StringComparison.OrdinalIgnoreCase))
            normalized = normalized[(ImageConstants.UploadsRootFolder.Length + 1)..];

        var physicalPath = Path.GetFullPath(
            Path.Combine(uploadsRoot, normalized.Replace('/', Path.DirectorySeparatorChar)));

        if (!physicalPath.StartsWith(uploadsRoot + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
            return;

        if (File.Exists(physicalPath))
            File.Delete(physicalPath);
    }

    public string BuildPublicUrl(string relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
            return relativePath;

        var path = relativePath.StartsWith('/') ? relativePath : $"/{relativePath}";

        var request = _httpContextAccessor.HttpContext?.Request;
        if (request is not null)
        {
            var pathBase = request.PathBase.HasValue ? request.PathBase.Value!.TrimEnd('/') : string.Empty;
            return $"{request.Scheme}://{request.Host.Value}{pathBase}{path}";
        }

        var baseUrl = _configuration["App:BaseUrl"]?.TrimEnd('/');
        return string.IsNullOrWhiteSpace(baseUrl) ? path : $"{baseUrl}{path}";
    }

    private async Task<StoredFile> WriteAsync(
        UploadImageModel file, string extension, string folder, CancellationToken cancellationToken)
    {
        var safeFolder = NormalizeFolder(folder);

        var targetDirectory = Path.Combine(GetUploadsRootPath(), safeFolder);
        Directory.CreateDirectory(targetDirectory);

        string uniqueFileName;
        string physicalPath;
        while (true)
        {
            uniqueFileName = $"{Guid.NewGuid():N}{extension}";
            physicalPath = Path.Combine(targetDirectory, uniqueFileName);

            if (File.Exists(physicalPath))
                continue;

            try
            {
                var stream = new FileStream(
                    physicalPath, FileMode.CreateNew, FileAccess.Write, FileShare.None,
                    bufferSize: 81920, useAsync: true);

                await using (stream)
                {
                    await stream.WriteAsync(file.Content, cancellationToken);
                }

                break;
            }
            catch (IOException) when (File.Exists(physicalPath))
            {
            }
        }

        var relativePath = $"/{ImageConstants.UploadsRootFolder}/{safeFolder}/{uniqueFileName}";

        return new StoredFile
        {
            FileName = uniqueFileName,
            RelativePath = relativePath,
            Url = BuildPublicUrl(relativePath)
        };
    }

    private static void ValidateCount(int newCount, int existingCount)
    {
        var max = ImageConstants.MaxImagesPerItem;
        if (existingCount + newCount <= max)
            return;

        var remaining = Math.Max(0, max - existingCount);
        var reason = remaining == 0
            ? $"وصل بالفعل للحد الأقصى وهو {max}"
            : $"فيه بالفعل {existingCount}، يعني تقدر تضيف {remaining} كمان بحد أقصى";

        throw new BadRequestException($"مسموح بـ {max} صور بحد أقصى؛ {reason}.");
    }

    private static ImageFormatDescriptor ValidateImage(UploadImageModel file)
    {
        var size = Math.Max(file.Length, file.Content.LongLength);
        var name = string.IsNullOrWhiteSpace(file.FileName) ? "الملف المرفوع" : $"الملف '{file.FileName}'";

        if (size <= 0)
            throw new BadRequestException($"{name} فاضي.");

        if (size > ImageConstants.MaxFileSizeBytes)
            throw new BadRequestException(
                $"{name} أكبر من الحد الأقصى وهو {ImageConstants.MaxFileSizeBytes / (1024 * 1024)} ميجابايت.");

        if (ImageFormatCatalog.LooksExecutable(file.Content))
            throw new BadRequestException(
                $"{name} برنامج أو سكربت أو مستند، مش صورة. الصور بس هي اللي ممكن ترفعها.");

        return ImageFormatCatalog.Detect(file.Content)
            ?? throw new BadRequestException(
                $"{name} مش صورة صالحة. الصيغ المقبولة: {ImageFormatCatalog.DisplayNames}.");
    }

    private static UploadFormatDescriptor ValidateDocument(UploadImageModel file)
    {
        var size = Math.Max(file.Length, file.Content.LongLength);
        var name = string.IsNullOrWhiteSpace(file.FileName) ? "الملف المرفوع" : $"الملف '{file.FileName}'";

        if (size <= 0)
            throw new BadRequestException($"{name} فاضي.");

        if (size > FileUploadConstants.MaxDocumentSizeBytes)
            throw new BadRequestException(
                $"{name} أكبر من الحد الأقصى وهو {FileUploadConstants.MaxDocumentSizeBytes / (1024 * 1024)} ميجابايت.");

        return DocumentFormatCatalog.Detect(file.Content)
            ?? throw new BadRequestException(
                $"{name} مش ملف سيرة ذاتية صالح. الصيغ المقبولة: {DocumentFormatCatalog.DisplayNames}.");
    }

    private static UploadFormatDescriptor ValidateVideo(UploadImageModel file)
    {
        var size = Math.Max(file.Length, file.Content.LongLength);
        var name = string.IsNullOrWhiteSpace(file.FileName) ? "الملف المرفوع" : $"الملف '{file.FileName}'";

        if (size <= 0)
            throw new BadRequestException($"{name} فاضي.");

        if (size > FileUploadConstants.MaxVideoSizeBytes)
            throw new BadRequestException(
                $"{name} أكبر من الحد الأقصى وهو {FileUploadConstants.MaxVideoSizeBytes / (1024 * 1024)} ميجابايت.");

        return VideoFormatCatalog.Detect(file.Content)
            ?? throw new BadRequestException(
                $"{name} مش فيديو صالح. الصيغ المقبولة: {VideoFormatCatalog.DisplayNames}.");
    }

    private static string NormalizeFolder(string folder)
    {
        if (string.IsNullOrWhiteSpace(folder))
            throw new ArgumentException("An upload folder name is required.", nameof(folder));

        var trimmed = folder.Trim().Trim('/', '\\');

        if (trimmed.Length == 0 ||
            trimmed.Contains("..", StringComparison.Ordinal) ||
            Path.IsPathRooted(trimmed) ||
            trimmed.IndexOfAny(Path.GetInvalidFileNameChars().Where(c => c != '/' && c != '\\').ToArray()) >= 0)
        {
            throw new ArgumentException($"Invalid upload folder '{folder}'.", nameof(folder));
        }

        return trimmed.Replace('\\', '/');
    }

    private string GetUploadsRootPath()
    {
        var root = _storage.UploadsRootPath;

        if (string.IsNullOrWhiteSpace(root))
        {
            var webRoot = _environment.WebRootPath;
            if (string.IsNullOrWhiteSpace(webRoot))
                webRoot = Path.Combine(_environment.ContentRootPath, "wwwroot");

            root = Path.Combine(webRoot, ImageConstants.UploadsRootFolder);
        }

        Directory.CreateDirectory(root);
        return Path.GetFullPath(root).TrimEnd(Path.DirectorySeparatorChar);
    }
}
