using AutoMapper;
using Domain.Entities;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Advertisements;
using Shared.DTOs.HomeFurnishing;
using Shared.Enums;
using Shared.Exceptions;
using Shared.Responses;

namespace Services.HomeFurnishing;

public class HomeApplianceService : IHomeApplianceService
{
    private readonly IHomeApplianceRepository _repository;
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;
    private readonly INotificationService _notifications;

    public HomeApplianceService(
        IHomeApplianceRepository repository,
        IFileService fileService,
        IMapper mapper,
        INotificationService notifications)
    {
        _repository = repository;
        _fileService = fileService;
        _mapper = mapper;
        _notifications = notifications;
    }

    public async Task<HomeApplianceDetailsDto> CreateAsync(
        string userId,
        CreateHomeApplianceRequest request,
        IReadOnlyList<UploadImageModel> images,
        UploadImageModel? video,
        CancellationToken cancellationToken = default)
    {
        HomeFurnishingListings.EnsureHasImages(images.Count);

        var entity = new HomeAppliance
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Governorate = HomeFurnishingListings.Governorate,
            CreatedAt = DateTime.UtcNow
        };

        ApplyEditableFields(entity, request);

        entity.Colors = BuildColors(entity.Id, request.Colors);

        var stored = new List<string>();
        try
        {
            var added = await AttachImagesAsync(entity, images, cancellationToken);
            stored.AddRange(added.Select(image => image.ImagePath));

            if (video is not null)
            {
                var storedVideo = await _fileService.SaveVideoAsync(
                    video, FileUploadConstants.HomeApplianceVideoFolder, cancellationToken);
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
            userId, ListingModuleType.HomeAppliance, NotificationAction.Created, entity.Id, entity.Title);

        return await BuildDetailsAsync(entity.Id, cancellationToken, includeUnmoderated: true);
    }

    public async Task<HomeApplianceDetailsDto> UpdateAsync(
        string userId,
        bool isAdmin,
        Guid id,
        UpdateHomeApplianceRequest request,
        IReadOnlyList<UploadImageModel> newImages,
        UploadImageModel? video,
        CancellationToken cancellationToken = default)
    {
        var entity = await LoadForWriteAsync(userId, isAdmin, id, cancellationToken);

        ApplyEditableFields(entity, request);

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
            var added = await AttachImagesAsync(entity, newImages, cancellationToken);
            await _repository.AddImagesAsync(added);
        }

        HomeFurnishingListings.EnsureHasImages(entity.Images.Count);

        ListingImages.NormalizeOrder(entity.Images);

        var replacedVideoPath = (string?)null;

        if (video is not null)
        {
            var storedVideo = await _fileService.SaveVideoAsync(
                video, FileUploadConstants.HomeApplianceVideoFolder, cancellationToken);

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
            entity.UserId, ListingModuleType.HomeAppliance,
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
            throw new NotFoundException("إعلان الأجهزة الكهربائية مش موجود.");

        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;

        _repository.Update(entity);
        await _repository.SaveChangesAsync();

        await _notifications.NotifyAsync(
            entity.UserId, ListingModuleType.HomeAppliance,
            isAdmin && entity.UserId != userId ? NotificationAction.AdminDeleted : NotificationAction.Deleted,
            id, entity.Title);
    }

    public async Task<PaginatedResult<HomeApplianceListItemDto>> GetListAsync(
        HomeApplianceFilterParams filter, CancellationToken cancellationToken = default)
    {
        var (items, total) = await _repository.GetPagedAsync(filter, cancellationToken);
        var mapped = _mapper.Map<IReadOnlyList<HomeApplianceListItemDto>>(items);
        return new PaginatedResult<HomeApplianceListItemDto>(
            mapped, total, filter.PageIndex, filter.PageSize);
    }

    public async Task<HomeApplianceDetailsDto> GetByIdAsync(
        Guid id, bool countView = false, CancellationToken cancellationToken = default)
    {
        var details = await BuildDetailsAsync(id, cancellationToken);

        if (countView)
        {
            await _repository.IncrementViewCountAsync(id, cancellationToken);
            details.ViewCount++;
        }

        return details;
    }

    public async Task<IReadOnlyList<HomeApplianceListItemDto>> GetSimilarAsync(
        Guid id, int count, CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<HomeApplianceListItemDto>>(
            await _repository.GetSimilarAsync(id, HomeFurnishingListings.StripSize(count), cancellationToken));

    public async Task<IReadOnlyList<HomeApplianceListItemDto>> GetRelatedAsync(
        Guid id, int count, CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<HomeApplianceListItemDto>>(
            await _repository.GetRelatedAsync(id, HomeFurnishingListings.StripSize(count), cancellationToken));

    public async Task<IReadOnlyList<HomeApplianceListItemDto>> GetRecentlyAddedAsync(
        int count, CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<HomeApplianceListItemDto>>(
            await _repository.GetRecentlyAddedAsync(HomeFurnishingListings.StripSize(count), cancellationToken));

    public Task<HomeFurnishingPriceStatisticsDto> GetPriceStatisticsAsync(
        HomeApplianceFilterParams filter, CancellationToken cancellationToken = default) =>
        _repository.GetPriceStatisticsAsync(filter, cancellationToken);

    public async Task<IReadOnlyList<HomeFurnishingSuggestionDto>> GetSuggestionsAsync(
        string? term, int count, CancellationToken cancellationToken = default)
    {
        var trimmed = term?.Trim();

        if (string.IsNullOrEmpty(trimmed) || trimmed.Length < HomeFurnishingListings.MinSuggestionLength)
            return Array.Empty<HomeFurnishingSuggestionDto>();

        return await _repository.GetSuggestionsAsync(
            trimmed, HomeFurnishingListings.SuggestionCount(count), cancellationToken);
    }

    public async Task<HomeApplianceDetailsDto> ReorderImagesAsync(
        string userId, bool isAdmin, Guid id,
        ReorderHomeFurnishingImagesRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await LoadForWriteAsync(userId, isAdmin, id, cancellationToken);

        if (!ListingImages.TryReorder(entity.Images, request.ImageIds))
            throw new BadRequestException(
                "ترتيب الصور لازم يشمل كل صورة في الإعلان مرة واحدة بالظبط.");

        entity.UpdatedAt = DateTime.UtcNow;

        _repository.Update(entity);
        await _repository.SaveChangesAsync();

        return await BuildDetailsAsync(id, cancellationToken, includeUnmoderated: true);
    }

    public async Task<HomeApplianceDetailsDto> SetPromotionAsync(
        Guid id, PromoteHomeFurnishingRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, asNoTracking: false, cancellationToken, includeUnmoderated: true)
            ?? throw new NotFoundException("إعلان الأجهزة الكهربائية مش موجود.");

        entity.IsFeatured = request.IsFeatured;
        entity.IsPremium = request.IsPremium;
        entity.IsUrgent = request.IsUrgent;
        entity.UpdatedAt = DateTime.UtcNow;

        _repository.Update(entity);
        await _repository.SaveChangesAsync();

        await _notifications.NotifyAsync(
            entity.UserId, ListingModuleType.HomeAppliance, NotificationAction.AdminUpdated, id, entity.Title);

        return await BuildDetailsAsync(id, cancellationToken, includeUnmoderated: true);
    }

    public async Task<IReadOnlyList<HomeFurnishingLookupItemDto>> GetDeviceTypesAsync(
        CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<HomeFurnishingLookupItemDto>>(
            await _repository.GetDeviceTypesAsync(cancellationToken));

    public async Task<IReadOnlyList<HomeFurnishingLookupItemDto>> GetBrandsAsync(
        CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<HomeFurnishingLookupItemDto>>(
            await _repository.GetBrandsAsync(cancellationToken));

    public async Task<IReadOnlyList<HomeFurnishingLookupItemDto>> GetConditionsAsync(
        CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<HomeFurnishingLookupItemDto>>(
            await _repository.GetConditionsAsync(cancellationToken));

    public async Task<IReadOnlyList<HomeFurnishingLookupItemDto>> GetWarrantiesAsync(
        CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<HomeFurnishingLookupItemDto>>(
            await _repository.GetWarrantiesAsync(cancellationToken));

    public async Task<IReadOnlyList<HomeFurnishingLookupItemDto>> GetColorsAsync(
        CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<HomeFurnishingLookupItemDto>>(
            await _repository.GetColorsAsync(cancellationToken));

    private async Task<HomeAppliance> LoadForWriteAsync(
        string userId, bool isAdmin, Guid id, CancellationToken cancellationToken)
    {
        var entity = isAdmin
            ? await _repository.GetByIdAsync(id, asNoTracking: false, cancellationToken, includeUnmoderated: true)
            : await _repository.GetOwnedAsync(id, userId, cancellationToken);

        return entity ?? throw new NotFoundException("إعلان الأجهزة الكهربائية مش موجود.");
    }

    private static void ApplyEditableFields(HomeAppliance entity, CreateHomeApplianceRequest request)
    {
        entity.SellerName = request.SellerName;
        entity.Phone = request.Phone;
        entity.WhatsApp = HomeFurnishingListings.Trimmed(request.WhatsApp);
        entity.Email = HomeFurnishingListings.Trimmed(request.Email);

        entity.ProductName = request.ProductName;
        entity.DeviceType = request.DeviceType;
        entity.OtherDeviceType = HomeFurnishingListings.OtherWhen(
            request.DeviceType == HomeApplianceDeviceType.Other, request.OtherDeviceType);
        entity.Brand = request.Brand;
        entity.OtherBrand = HomeFurnishingListings.OtherWhen(
            request.Brand == HomeApplianceBrand.Other, request.OtherBrand);
        entity.OtherColor = HomeFurnishingListings.OtherWhen(
            request.Colors.Contains(HomeApplianceColor.Other), request.OtherColor);

        entity.Condition = request.Condition;
        entity.Warranty = request.Warranty;

        entity.WarrantyDuration = HomeFurnishingListings.OtherWhen(
            request.Warranty == HomeApplianceWarranty.Available, request.WarrantyDuration);

        entity.PowerRating = HomeFurnishingListings.Trimmed(request.PowerRating);
        entity.DeliveryAvailable = request.DeliveryAvailable;

        entity.Price = request.Price;
        entity.Negotiable = request.Negotiable;

        entity.Center = request.Center;
        entity.Address = request.Address;
        entity.GoogleMaps = HomeFurnishingListings.Trimmed(request.GoogleMaps);

        entity.Title = request.Title;
        entity.Description = request.Description;
    }

    private static List<HomeApplianceColorSelection> BuildColors(
        Guid listingId, IEnumerable<HomeApplianceColor> colors) =>
        colors.Distinct()
            .Select(color => new HomeApplianceColorSelection { HomeApplianceId = listingId, Color = color })
            .ToList();

    private async Task ReplaceColorsAsync(HomeAppliance entity, IEnumerable<HomeApplianceColor> requested)
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

    private async Task<HomeApplianceDetailsDto> BuildDetailsAsync(
        Guid id, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        var entity = await _repository.GetByIdAsync(id, asNoTracking: true, cancellationToken, includeUnmoderated)
            ?? throw new NotFoundException("إعلان الأجهزة الكهربائية مش موجود.");

        var details = _mapper.Map<HomeApplianceDetailsDto>(entity);

        details.ShareUrl = HomeFurnishingListings.ShareUrl(
            _fileService, HomeFurnishingRoutes.HomeAppliances, id);

        return details;
    }

    private Task<List<HomeApplianceImage>> AttachImagesAsync(
        HomeAppliance entity,
        IReadOnlyList<UploadImageModel> images,
        CancellationToken cancellationToken) =>
        ListingImages.AttachOrderedAsync(
            _fileService,
            entity.Images,
            images,
            ImageConstants.HomeApplianceFolder,
            _ => new HomeApplianceImage { HomeApplianceId = entity.Id },
            cancellationToken);
}
