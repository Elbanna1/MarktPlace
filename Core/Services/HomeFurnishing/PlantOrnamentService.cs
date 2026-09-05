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

public class PlantOrnamentService : IPlantOrnamentService
{
    private readonly IPlantOrnamentRepository _repository;
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;
    private readonly INotificationService _notifications;

    public PlantOrnamentService(
        IPlantOrnamentRepository repository,
        IFileService fileService,
        IMapper mapper,
        INotificationService notifications)
    {
        _repository = repository;
        _fileService = fileService;
        _mapper = mapper;
        _notifications = notifications;
    }

    public async Task<PlantOrnamentDetailsDto> CreateAsync(
        string userId,
        CreatePlantOrnamentRequest request,
        IReadOnlyList<UploadImageModel> images,
        UploadImageModel? video,
        CancellationToken cancellationToken = default)
    {
        HomeFurnishingListings.EnsureHasImages(images.Count);

        var entity = new PlantOrnament
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Governorate = HomeFurnishingListings.Governorate,
            CreatedAt = DateTime.UtcNow
        };

        ApplyEditableFields(entity, request);

        var stored = new List<string>();
        try
        {
            var added = await AttachImagesAsync(entity, images, cancellationToken);
            stored.AddRange(added.Select(image => image.ImagePath));

            if (video is not null)
            {
                var storedVideo = await _fileService.SaveVideoAsync(
                    video, FileUploadConstants.PlantOrnamentVideoFolder, cancellationToken);
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
            userId, ListingModuleType.PlantOrnament, NotificationAction.Created, entity.Id, entity.Title);

        return await BuildDetailsAsync(entity.Id, cancellationToken, includeUnmoderated: true);
    }

    public async Task<PlantOrnamentDetailsDto> UpdateAsync(
        string userId,
        bool isAdmin,
        Guid id,
        UpdatePlantOrnamentRequest request,
        IReadOnlyList<UploadImageModel> newImages,
        UploadImageModel? video,
        CancellationToken cancellationToken = default)
    {
        var entity = await LoadForWriteAsync(userId, isAdmin, id, cancellationToken);

        ApplyEditableFields(entity, request);

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
                video, FileUploadConstants.PlantOrnamentVideoFolder, cancellationToken);

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
            entity.UserId, ListingModuleType.PlantOrnament,
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
            throw new NotFoundException("إعلان النباتات والزينة مش موجود.");

        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;

        _repository.Update(entity);
        await _repository.SaveChangesAsync();

        await _notifications.NotifyAsync(
            entity.UserId, ListingModuleType.PlantOrnament,
            isAdmin && entity.UserId != userId ? NotificationAction.AdminDeleted : NotificationAction.Deleted,
            id, entity.Title);
    }

    public async Task<PaginatedResult<PlantOrnamentListItemDto>> GetListAsync(
        PlantOrnamentFilterParams filter, CancellationToken cancellationToken = default)
    {
        var (items, total) = await _repository.GetPagedAsync(filter, cancellationToken);
        var mapped = _mapper.Map<IReadOnlyList<PlantOrnamentListItemDto>>(items);
        return new PaginatedResult<PlantOrnamentListItemDto>(
            mapped, total, filter.PageIndex, filter.PageSize);
    }

    public async Task<PlantOrnamentDetailsDto> GetByIdAsync(
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

    public async Task<IReadOnlyList<PlantOrnamentListItemDto>> GetSimilarAsync(
        Guid id, int count, CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<PlantOrnamentListItemDto>>(
            await _repository.GetSimilarAsync(id, HomeFurnishingListings.StripSize(count), cancellationToken));

    public async Task<IReadOnlyList<PlantOrnamentListItemDto>> GetRelatedAsync(
        Guid id, int count, CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<PlantOrnamentListItemDto>>(
            await _repository.GetRelatedAsync(id, HomeFurnishingListings.StripSize(count), cancellationToken));

    public async Task<IReadOnlyList<PlantOrnamentListItemDto>> GetRecentlyAddedAsync(
        int count, CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<PlantOrnamentListItemDto>>(
            await _repository.GetRecentlyAddedAsync(HomeFurnishingListings.StripSize(count), cancellationToken));

    public Task<HomeFurnishingPriceStatisticsDto> GetPriceStatisticsAsync(
        PlantOrnamentFilterParams filter, CancellationToken cancellationToken = default) =>
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

    public async Task<PlantOrnamentDetailsDto> ReorderImagesAsync(
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

    public async Task<PlantOrnamentDetailsDto> SetPromotionAsync(
        Guid id, PromoteHomeFurnishingRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, asNoTracking: false, cancellationToken, includeUnmoderated: true)
            ?? throw new NotFoundException("إعلان النباتات والزينة مش موجود.");

        entity.IsFeatured = request.IsFeatured;
        entity.IsPremium = request.IsPremium;
        entity.IsUrgent = request.IsUrgent;
        entity.UpdatedAt = DateTime.UtcNow;

        _repository.Update(entity);
        await _repository.SaveChangesAsync();

        await _notifications.NotifyAsync(
            entity.UserId, ListingModuleType.PlantOrnament, NotificationAction.AdminUpdated, id, entity.Title);

        return await BuildDetailsAsync(id, cancellationToken, includeUnmoderated: true);
    }

    public async Task<IReadOnlyList<HomeFurnishingLookupItemDto>> GetProductTypesAsync(
        CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<HomeFurnishingLookupItemDto>>(
            await _repository.GetProductTypesAsync(cancellationToken));

    public async Task<IReadOnlyList<HomeFurnishingLookupItemDto>> GetSuitableForsAsync(
        CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<HomeFurnishingLookupItemDto>>(
            await _repository.GetSuitableForsAsync(cancellationToken));

    private async Task<PlantOrnament> LoadForWriteAsync(
        string userId, bool isAdmin, Guid id, CancellationToken cancellationToken)
    {
        var entity = isAdmin
            ? await _repository.GetByIdAsync(id, asNoTracking: false, cancellationToken, includeUnmoderated: true)
            : await _repository.GetOwnedAsync(id, userId, cancellationToken);

        return entity ?? throw new NotFoundException("إعلان النباتات والزينة مش موجود.");
    }

    private static void ApplyEditableFields(PlantOrnament entity, CreatePlantOrnamentRequest request)
    {
        entity.SellerName = request.SellerName;
        entity.Phone = request.Phone;
        entity.WhatsApp = HomeFurnishingListings.Trimmed(request.WhatsApp);
        entity.Email = HomeFurnishingListings.Trimmed(request.Email);

        entity.ProductName = request.ProductName;
        entity.ProductType = request.ProductType;
        entity.OtherProductType = HomeFurnishingListings.OtherWhen(
            request.ProductType == PlantOrnamentProductType.Other, request.OtherProductType);
        entity.SuitableFor = request.SuitableFor;
        entity.Height = request.Height;

        entity.Price = request.Price;
        entity.Negotiable = request.Negotiable;

        entity.Center = request.Center;
        entity.Address = request.Address;
        entity.GoogleMaps = HomeFurnishingListings.Trimmed(request.GoogleMaps);

        entity.Title = request.Title;
        entity.Description = request.Description;
    }

    private async Task<PlantOrnamentDetailsDto> BuildDetailsAsync(
        Guid id, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        var entity = await _repository.GetByIdAsync(id, asNoTracking: true, cancellationToken, includeUnmoderated)
            ?? throw new NotFoundException("إعلان النباتات والزينة مش موجود.");

        var details = _mapper.Map<PlantOrnamentDetailsDto>(entity);

        details.ShareUrl = HomeFurnishingListings.ShareUrl(
            _fileService, HomeFurnishingRoutes.PlantsOrnaments, id);

        return details;
    }

    private Task<List<PlantOrnamentImage>> AttachImagesAsync(
        PlantOrnament entity,
        IReadOnlyList<UploadImageModel> images,
        CancellationToken cancellationToken) =>
        ListingImages.AttachOrderedAsync(
            _fileService,
            entity.Images,
            images,
            ImageConstants.PlantOrnamentFolder,
            _ => new PlantOrnamentImage { PlantOrnamentId = entity.Id },
            cancellationToken);
}
