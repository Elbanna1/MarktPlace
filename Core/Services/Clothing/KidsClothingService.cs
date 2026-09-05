using AutoMapper;
using Domain.Entities;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Advertisements;
using Shared.DTOs.Clothing;
using Shared.Enums;
using Shared.Exceptions;
using Shared.Responses;

namespace Services.Clothing;

public class KidsClothingService : IKidsClothingService
{
    private readonly IKidsClothingRepository _repository;
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;
    private readonly INotificationService _notifications;

    public KidsClothingService(
        IKidsClothingRepository repository,
        IFileService fileService,
        IMapper mapper,
        INotificationService notifications)
    {
        _repository = repository;
        _fileService = fileService;
        _mapper = mapper;
        _notifications = notifications;
    }

    public async Task<KidsClothingDetailsDto> CreateAsync(
        string userId,
        CreateKidsClothingRequest request,
        IReadOnlyList<UploadImageModel> images,
        UploadImageModel? video,
        CancellationToken cancellationToken = default)
    {
        if (images.Count == 0)
            throw new BadRequestException("لازم ترفع صورة واحدة على الأقل.");

        var entity = _mapper.Map<KidsClothing>(request);

        entity.Id = Guid.NewGuid();
        entity.UserId = userId;
        entity.Governorate = LocationConstants.Governorate;
        entity.CreatedAt = DateTime.UtcNow;
        entity.OtherClothingType = request.OtherClothingType?.Trim();
        entity.OtherBrand = request.OtherBrand?.Trim();
        entity.OtherColor = NormalizeOtherColor(request.Colors, request.OtherColor);
        entity.GoogleMaps = NormalizeGoogleMaps(request.SellingMethod, request.GoogleMaps);
        entity.Email = request.Email?.Trim();

        entity.Sizes = BuildSizes(entity.Id, request.Sizes);
        entity.Colors = BuildColors(entity.Id, request.Colors);

        var stored = new List<string>();
        try
        {
            var added = await AttachImagesAsync(entity, images, startAsPrimary: true, cancellationToken);
            stored.AddRange(added.Select(image => image.ImagePath));

            if (video is not null)
            {
                var storedVideo = await _fileService.SaveVideoAsync(
                    video, FileUploadConstants.KidsClothingVideoFolder, cancellationToken);
                stored.Add(storedVideo.RelativePath);

                entity.VideoPath = storedVideo.RelativePath;
                entity.VideoUrl = storedVideo.Url;
            }

            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();
        }
        catch
        {
            foreach (var path in stored)
                _fileService.Delete(path);

            throw;
        }

        await _notifications.NotifyAsync(
            userId, ListingModuleType.KidsClothing, NotificationAction.Created, entity.Id, entity.Title);

        return await BuildDetailsAsync(entity.Id, cancellationToken, includeUnmoderated: true);
    }

    public async Task<KidsClothingDetailsDto> UpdateAsync(
        string userId,
        bool isAdmin,
        Guid id,
        UpdateKidsClothingRequest request,
        IReadOnlyList<UploadImageModel> newImages,
        UploadImageModel? video,
        CancellationToken cancellationToken = default)
    {
        var entity = isAdmin
            ? await _repository.GetByIdAsync(id, asNoTracking: false, cancellationToken, includeUnmoderated: true)
            : await _repository.GetOwnedAsync(id, userId, cancellationToken);

        if (entity is null)
            throw new NotFoundException("إعلان ملابس الأطفال مش موجود.");

        entity.StoreName = request.StoreName;
        entity.SellingMethod = request.SellingMethod;
        entity.ClothingType = request.ClothingType;
        entity.OtherClothingType = request.OtherClothingType?.Trim();
        entity.Brand = request.Brand;
        entity.OtherBrand = request.OtherBrand?.Trim();
        entity.OtherColor = NormalizeOtherColor(request.Colors, request.OtherColor);
        entity.Condition = request.Condition;
        entity.Price = request.Price;
        entity.DeliveryAvailable = request.DeliveryAvailable;
        entity.Center = request.Center;
        entity.Address = request.Address;
        entity.GoogleMaps = NormalizeGoogleMaps(request.SellingMethod, request.GoogleMaps);
        entity.Phone = request.Phone;
        entity.WhatsApp = request.WhatsApp;
        entity.Email = request.Email?.Trim();
        entity.Title = request.Title;
        entity.Description = request.Description;

        await ReplaceSizesAsync(entity, request.Sizes);
        await ReplaceColorsAsync(entity, request.Colors);

        if (request.RemoveImageIds.Count > 0)
        {
            var toRemove = entity.Images
                .Where(image => request.RemoveImageIds.Contains(image.Id))
                .ToList();

            foreach (var image in toRemove)
            {
                _fileService.Delete(image.ImagePath);
                _repository.RemoveImage(image);
                entity.Images.Remove(image);
            }
        }

        if (newImages.Count > 0)
        {
            var startAsPrimary = entity.Images.Count == 0;
            var added = await AttachImagesAsync(entity, newImages, startAsPrimary, cancellationToken);
            await _repository.AddImagesAsync(added);
        }

        if (entity.Images.Count == 0)
            throw new BadRequestException("لازم ترفع صورة واحدة على الأقل.");

        ListingImages.NormalizePrimary(entity.Images);

        var replacedVideoPath = (string?)null;

        if (video is not null)
        {
            var storedVideo = await _fileService.SaveVideoAsync(
                video, FileUploadConstants.KidsClothingVideoFolder, cancellationToken);

            replacedVideoPath = entity.VideoPath;
            entity.VideoPath = storedVideo.RelativePath;
            entity.VideoUrl = storedVideo.Url;
        }
        else if (request.RemoveVideo && !string.IsNullOrWhiteSpace(entity.VideoPath))
        {
            replacedVideoPath = entity.VideoPath;
            entity.VideoPath = null;
            entity.VideoUrl = null;
        }

        entity.UpdatedAt = DateTime.UtcNow;

        _repository.Update(entity);
        await _repository.SaveChangesAsync();

        if (!string.IsNullOrWhiteSpace(replacedVideoPath))
            _fileService.Delete(replacedVideoPath);

        await _notifications.NotifyAsync(
            entity.UserId, ListingModuleType.KidsClothing,
            isAdmin && entity.UserId != userId ? NotificationAction.AdminUpdated : NotificationAction.Updated,
            id, entity.Title);

        return await BuildDetailsAsync(id, cancellationToken, includeUnmoderated: true);
    }

    public async Task DeleteAsync(string userId, Guid id, bool isAdmin)
    {
        var entity = isAdmin
            ? await _repository.GetByIdAsync(id, asNoTracking: false, includeUnmoderated: true)
            : await _repository.GetOwnedAsync(id, userId);

        if (entity is null)
            throw new NotFoundException("إعلان ملابس الأطفال مش موجود.");

        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;

        _repository.Update(entity);
        await _repository.SaveChangesAsync();

        await _notifications.NotifyAsync(
            entity.UserId, ListingModuleType.KidsClothing,
            isAdmin && entity.UserId != userId ? NotificationAction.AdminDeleted : NotificationAction.Deleted,
            id, entity.Title);
    }

    public async Task<PaginatedResult<KidsClothingListItemDto>> GetListAsync(
        KidsClothingFilterParams filter, CancellationToken cancellationToken = default)
    {
        var (items, total) = await _repository.GetPagedAsync(filter, cancellationToken);
        var mapped = _mapper.Map<IReadOnlyList<KidsClothingListItemDto>>(items);
        return new PaginatedResult<KidsClothingListItemDto>(
            mapped, total, filter.PageIndex, filter.PageSize);
    }

    public Task<KidsClothingDetailsDto> GetByIdAsync(
        Guid id, CancellationToken cancellationToken = default) =>
        BuildDetailsAsync(id, cancellationToken);

    public async Task<IReadOnlyList<ClothingLookupItemDto>> GetClothingTypesAsync(
        CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<ClothingLookupItemDto>>(
            await _repository.GetClothingTypesAsync(cancellationToken));

    public async Task<IReadOnlyList<ClothingLookupItemDto>> GetBrandsAsync(
        CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<ClothingLookupItemDto>>(
            await _repository.GetBrandsAsync(cancellationToken));

    public async Task<IReadOnlyList<ClothingLookupItemDto>> GetSizesAsync(
        CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<ClothingLookupItemDto>>(
            await _repository.GetSizesAsync(cancellationToken));

    public async Task<IReadOnlyList<ClothingLookupItemDto>> GetColorsAsync(
        CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<ClothingLookupItemDto>>(
            await _repository.GetColorsAsync(cancellationToken));

    public async Task<IReadOnlyList<ClothingLookupItemDto>> GetConditionsAsync(
        CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<ClothingLookupItemDto>>(
            await _repository.GetConditionsAsync(cancellationToken));

    public async Task<IReadOnlyList<ClothingLookupItemDto>> GetSellingMethodsAsync(
        CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<ClothingLookupItemDto>>(
            await _repository.GetSellingMethodsAsync(cancellationToken));

    private static string? NormalizeGoogleMaps(KidsClothingSellingMethod sellingMethod, string? googleMaps) =>
        sellingMethod is KidsClothingSellingMethod.Store or KidsClothingSellingMethod.StoreAndOnline
            ? string.IsNullOrWhiteSpace(googleMaps) ? null : googleMaps.Trim()
            : null;

    private static string? NormalizeOtherColor(
        IEnumerable<KidsClothingColor> colors, string? otherColor) =>
        colors.Contains(KidsClothingColor.Other)
            ? string.IsNullOrWhiteSpace(otherColor) ? null : otherColor.Trim()
            : null;

    private static List<KidsClothingSizeSelection> BuildSizes(Guid listingId, IEnumerable<KidsClothingSize> sizes) =>
        sizes.Distinct()
            .Select(size => new KidsClothingSizeSelection { KidsClothingId = listingId, Size = size })
            .ToList();

    private static List<KidsClothingColorSelection> BuildColors(Guid listingId, IEnumerable<KidsClothingColor> colors) =>
        colors.Distinct()
            .Select(color => new KidsClothingColorSelection { KidsClothingId = listingId, Color = color })
            .ToList();

    private async Task ReplaceSizesAsync(KidsClothing entity, IEnumerable<KidsClothingSize> requested)
    {
        var wanted = requested.Distinct().ToHashSet();

        var toRemove = entity.Sizes.Where(selection => !wanted.Contains(selection.Size)).ToList();
        foreach (var selection in toRemove)
            entity.Sizes.Remove(selection);

        if (toRemove.Count > 0)
            _repository.RemoveSizes(toRemove);

        var existing = entity.Sizes.Select(selection => selection.Size).ToHashSet();
        var toAdd = BuildSizes(entity.Id, wanted.Where(size => !existing.Contains(size)));

        if (toAdd.Count > 0)
        {
            await _repository.AddSizesAsync(toAdd);
            foreach (var selection in toAdd)
                entity.Sizes.Add(selection);
        }
    }

    private async Task ReplaceColorsAsync(KidsClothing entity, IEnumerable<KidsClothingColor> requested)
    {
        var wanted = requested.Distinct().ToHashSet();

        var toRemove = entity.Colors.Where(selection => !wanted.Contains(selection.Color)).ToList();
        foreach (var selection in toRemove)
            entity.Colors.Remove(selection);

        if (toRemove.Count > 0)
            _repository.RemoveColors(toRemove);

        var existing = entity.Colors.Select(selection => selection.Color).ToHashSet();
        var toAdd = BuildColors(entity.Id, wanted.Where(color => !existing.Contains(color)));

        if (toAdd.Count > 0)
        {
            await _repository.AddColorsAsync(toAdd);
            foreach (var selection in toAdd)
                entity.Colors.Add(selection);
        }
    }

    private async Task<KidsClothingDetailsDto> BuildDetailsAsync(
        Guid id, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        var entity = await _repository.GetByIdAsync(id, asNoTracking: true, cancellationToken, includeUnmoderated)
            ?? throw new NotFoundException("إعلان ملابس الأطفال مش موجود.");

        return _mapper.Map<KidsClothingDetailsDto>(entity);
    }

    private Task<List<KidsClothingImage>> AttachImagesAsync(
        KidsClothing entity,
        IReadOnlyList<UploadImageModel> images,
        bool startAsPrimary,
        CancellationToken cancellationToken) =>
        ListingImages.AttachAsync(
            _fileService,
            entity.Images,
            images,
            ImageConstants.KidsClothingFolder,
            startAsPrimary,
            _ => new KidsClothingImage { KidsClothingId = entity.Id },
            cancellationToken);
}
