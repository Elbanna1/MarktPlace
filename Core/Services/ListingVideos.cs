using Domain.Entities;
using ServicesAbstraction;
using Shared.DTOs.Advertisements;

namespace Services;

internal static class ListingVideos
{
    public static async Task<TVideo> SaveAsync<TVideo>(
        IFileService fileService,
        UploadImageModel upload,
        string folder,
        Func<StoredFile, TVideo> createVideo,
        CancellationToken cancellationToken = default)
        where TVideo : IListingVideo
    {
        var stored = await fileService.SaveVideoAsync(upload, folder, cancellationToken);

        var video = createVideo(stored);

        video.Id = Guid.NewGuid();
        video.FileName = stored.FileName;
        video.VideoPath = stored.RelativePath;
        video.VideoUrl = stored.Url;
        video.CreatedAt = DateTime.UtcNow;

        return video;
    }
}
