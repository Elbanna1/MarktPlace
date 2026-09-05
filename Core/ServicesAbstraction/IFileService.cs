using Shared.DTOs.Advertisements;

namespace ServicesAbstraction;

public interface IFileService
{
    Task<StoredFile> SaveAsync(UploadImageModel file, string folder, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<StoredFile>> SaveImagesAsync(
        IReadOnlyList<UploadImageModel> files,
        string folder,
        int existingCount = 0,
        CancellationToken cancellationToken = default);

    Task<StoredFile> SaveDocumentAsync(
        UploadImageModel file, string folder, CancellationToken cancellationToken = default);

    Task<StoredFile> SaveVideoAsync(
        UploadImageModel file, string folder, CancellationToken cancellationToken = default);

    void Delete(string relativePath);

    string BuildPublicUrl(string relativePath);
}
