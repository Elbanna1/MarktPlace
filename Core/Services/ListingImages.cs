using Domain.Entities;
using ServicesAbstraction;
using Shared.DTOs.Advertisements;

namespace Services;

internal static class ListingImages
{
    public static async Task<List<TImage>> AttachAsync<TImage>(
        IFileService fileService,
        ICollection<TImage> images,
        IReadOnlyList<UploadImageModel> uploads,
        string folder,
        bool startAsPrimary,
        Func<StoredFile, TImage> createImage,
        CancellationToken cancellationToken = default)
        where TImage : IListingImage
    {
        var added = new List<TImage>();
        if (uploads.Count == 0)
            return added;

        var stored = await fileService.SaveImagesAsync(
            uploads, folder, existingCount: images.Count, cancellationToken);

        var isFirst = startAsPrimary;
        foreach (var file in stored)
        {
            var image = createImage(file);

            image.Id = Guid.NewGuid();
            image.FileName = file.FileName;
            image.ImagePath = file.RelativePath;
            image.ImageUrl = file.Url;
            image.IsPrimary = isFirst;
            image.CreatedAt = DateTime.UtcNow;

            images.Add(image);
            added.Add(image);
            isFirst = false;
        }

        return added;
    }

    public static async Task<List<TImage>> AttachOrderedAsync<TImage>(
        IFileService fileService,
        ICollection<TImage> images,
        IReadOnlyList<UploadImageModel> uploads,
        string folder,
        Func<StoredFile, TImage> createImage,
        CancellationToken cancellationToken = default)
        where TImage : IOrderedListingImage
    {
        var startAsPrimary = images.Count == 0;
        var nextPosition = images.Count == 0 ? 0 : images.Max(image => image.SortOrder) + 1;

        var added = await AttachAsync(
            fileService, images, uploads, folder, startAsPrimary, createImage, cancellationToken);

        foreach (var image in added)
            image.SortOrder = nextPosition++;

        NormalizeOrder(images);

        return added;
    }

    public static void NormalizeOrder<TImage>(ICollection<TImage> images)
        where TImage : IOrderedListingImage
    {
        if (images.Count == 0)
            return;

        var ordered = images
            .OrderBy(image => image.SortOrder)
            .ThenBy(image => image.CreatedAt)
            .ToList();

        for (var position = 0; position < ordered.Count; position++)
        {
            ordered[position].SortOrder = position;
            ordered[position].IsPrimary = position == 0;
        }
    }

    public static bool TryReorder<TImage>(ICollection<TImage> images, IReadOnlyList<Guid> wantedOrder)
        where TImage : IOrderedListingImage
    {
        var byId = images.ToDictionary(image => image.Id);

        if (wantedOrder.Count != byId.Count || wantedOrder.Distinct().Count() != wantedOrder.Count)
            return false;

        if (!wantedOrder.All(byId.ContainsKey))
            return false;

        for (var position = 0; position < wantedOrder.Count; position++)
        {
            var image = byId[wantedOrder[position]];
            image.SortOrder = position;
            image.IsPrimary = position == 0;
        }

        return true;
    }

    public static void NormalizePrimary<TImage>(ICollection<TImage> images)
        where TImage : IListingImage
    {
        if (images.Count == 0)
            return;

        var primaries = images.Where(image => image.IsPrimary).ToList();

        if (primaries.Count == 1)
            return;

        foreach (var image in primaries)
            image.IsPrimary = false;

        images.OrderBy(image => image.CreatedAt).First().IsPrimary = true;
    }
}
