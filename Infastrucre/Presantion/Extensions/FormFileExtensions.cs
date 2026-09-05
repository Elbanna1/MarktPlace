using Microsoft.AspNetCore.Http;
using Shared.DTOs.Advertisements;

namespace Presentation.Extensions;

public static class FormFileExtensions
{
    public static IFormFileCollection FormFilesOrEmpty(this HttpRequest request) =>
        request.HasFormContentType ? request.Form.Files : EmptyFormFileCollection.Instance;

    public static async Task<List<UploadImageModel>> ToUploadModelsAsync(
        this IEnumerable<IFormFile>? files, CancellationToken cancellationToken = default)
    {
        var uploads = new List<UploadImageModel>();
        if (files is null)
            return uploads;

        foreach (var file in files)
        {
            var upload = await file.ToUploadModelAsync(cancellationToken);
            if (upload is not null)
                uploads.Add(upload);
        }

        return uploads;
    }

    public static async Task<UploadImageModel?> ToUploadModelAsync(
        this IFormFile? file, CancellationToken cancellationToken = default)
    {
        if (file is null || file.Length == 0)
            return null;

        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream, cancellationToken);

        return new UploadImageModel
        {
            FileName = file.FileName,
            ContentType = file.ContentType,
            Length = file.Length,
            Content = memoryStream.ToArray()
        };
    }

    private sealed class EmptyFormFileCollection : List<IFormFile>, IFormFileCollection
    {
        public static readonly EmptyFormFileCollection Instance = new();

        public IFormFile? this[string name] => null;

        public IFormFile? GetFile(string name) => null;

        public IReadOnlyList<IFormFile> GetFiles(string name) => Array.Empty<IFormFile>();
    }
}
