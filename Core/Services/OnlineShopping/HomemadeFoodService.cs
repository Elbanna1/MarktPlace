using AutoMapper;
using Domain.Entities;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Advertisements;
using Shared.DTOs.OnlineShopping;
using Shared.Enums;
using Shared.Exceptions;
using Shared.Responses;

namespace Services.OnlineShopping;

public class HomemadeFoodService : IHomemadeFoodService
{
    private readonly IHomemadeFoodRepository _repository;
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;
    private readonly INotificationService _notifications;

    public HomemadeFoodService(
        IHomemadeFoodRepository repository,
        IFileService fileService,
        IMapper mapper,
        INotificationService notifications)
    {
        _repository = repository;
        _fileService = fileService;
        _mapper = mapper;
        _notifications = notifications;
    }

    public async Task<HomemadeFoodDetailsDto> CreateAsync(
        string userId,
        CreateHomemadeFoodRequest request,
        IReadOnlyList<UploadImageModel> images,
        UploadImageModel? video,
        CancellationToken cancellationToken = default)
    {
        if (images.Count == 0)
            throw new BadRequestException("لازم ترفع صورة واحدة على الأقل.");

        var entity = _mapper.Map<HomemadeFood>(request);

        entity.Id = Guid.NewGuid();
        entity.UserId = userId;
        entity.CreatedAt = DateTime.UtcNow;
        entity.OtherSection = NormalizeOther(request.Section == HomemadeFoodSection.Other, request.OtherSection);

        entity.DeliveryAreas = BuildDeliveryAreas(
            entity.Id, NormalizeDeliveryAreas(request.DeliveryAvailable, request.DeliveryAreas));

        var stored = new List<string>();
        try
        {
            var added = await AttachImagesAsync(entity, images, startAsPrimary: true, cancellationToken);
            stored.AddRange(added.Select(image => image.ImagePath));

            if (video is not null)
            {
                var storedVideo = await _fileService.SaveVideoAsync(
                    video, FileUploadConstants.HomemadeFoodVideoFolder, cancellationToken);
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
            userId, ListingModuleType.HomemadeFood, NotificationAction.Created, entity.Id, entity.Title);

        return await BuildDetailsAsync(entity.Id, cancellationToken, includeUnmoderated: true);
    }

    public async Task<HomemadeFoodDetailsDto> UpdateAsync(
        string userId,
        bool isAdmin,
        Guid id,
        UpdateHomemadeFoodRequest request,
        IReadOnlyList<UploadImageModel> newImages,
        UploadImageModel? video,
        CancellationToken cancellationToken = default)
    {
        var entity = isAdmin
            ? await _repository.GetByIdAsync(id, asNoTracking: false, cancellationToken, includeUnmoderated: true)
            : await _repository.GetOwnedAsync(id, userId, cancellationToken);

        if (entity is null)
            throw new NotFoundException("إعلان الأكل المنزلي مش موجود.");

        entity.ProjectName = request.ProjectName;
        entity.Phone = request.Phone;
        entity.WhatsApp = request.WhatsApp;
        entity.Section = request.Section;
        entity.OtherSection = NormalizeOther(request.Section == HomemadeFoodSection.Other, request.OtherSection);
        entity.PreparedOnDemand = request.PreparedOnDemand;
        entity.MinimumOrderQuantity = request.MinimumOrderQuantity;
        entity.PreparationTime = request.PreparationTime;
        entity.DeliveryAvailable = request.DeliveryAvailable;
        entity.Price = request.Price;
        entity.Ingredients = Trimmed(request.Ingredients);
        entity.WeightOrSize = Trimmed(request.WeightOrSize);
        entity.StorageMethod = Trimmed(request.StorageMethod);
        entity.AvailableOrderingHours = Trimmed(request.AvailableOrderingHours);
        entity.AdditionalNotes = Trimmed(request.AdditionalNotes);
        entity.Title = request.Title;
        entity.Description = request.Description;

        await ReplaceDeliveryAreasAsync(
            entity, NormalizeDeliveryAreas(request.DeliveryAvailable, request.DeliveryAreas));

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
                video, FileUploadConstants.HomemadeFoodVideoFolder, cancellationToken);

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
            entity.UserId, ListingModuleType.HomemadeFood,
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
            throw new NotFoundException("إعلان الأكل المنزلي مش موجود.");

        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;

        _repository.Update(entity);
        await _repository.SaveChangesAsync();

        await _notifications.NotifyAsync(
            entity.UserId, ListingModuleType.HomemadeFood,
            isAdmin && entity.UserId != userId ? NotificationAction.AdminDeleted : NotificationAction.Deleted,
            id, entity.Title);
    }

    public async Task<PaginatedResult<HomemadeFoodListItemDto>> GetListAsync(
        HomemadeFoodFilterParams filter, CancellationToken cancellationToken = default)
    {
        var (items, total) = await _repository.GetPagedAsync(filter, cancellationToken);
        var mapped = _mapper.Map<IReadOnlyList<HomemadeFoodListItemDto>>(items);
        return new PaginatedResult<HomemadeFoodListItemDto>(mapped, total, filter.PageIndex, filter.PageSize);
    }

    public Task<HomemadeFoodDetailsDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        BuildDetailsAsync(id, cancellationToken);

    public async Task<IReadOnlyList<OnlineShoppingLookupItemDto>> GetSectionsAsync(
        CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<OnlineShoppingLookupItemDto>>(
            await _repository.GetSectionsAsync(cancellationToken));

    public async Task<IReadOnlyList<OnlineShoppingLookupItemDto>> GetDeliveryAreasAsync(
        CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<OnlineShoppingLookupItemDto>>(
            await _repository.GetDeliveryAreasAsync(cancellationToken));

    private static string? Trimmed(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static string? NormalizeOther(bool isOtherSelected, string? value) =>
        isOtherSelected && !string.IsNullOrWhiteSpace(value) ? value.Trim() : null;

    private static IEnumerable<HomemadeFoodDeliveryArea> NormalizeDeliveryAreas(
        bool deliveryAvailable, IEnumerable<HomemadeFoodDeliveryArea> areas) =>
        deliveryAvailable ? areas : Array.Empty<HomemadeFoodDeliveryArea>();

    private static List<HomemadeFoodDeliveryAreaSelection> BuildDeliveryAreas(
        Guid listingId, IEnumerable<HomemadeFoodDeliveryArea> areas) =>
        areas.Distinct()
            .Select(area => new HomemadeFoodDeliveryAreaSelection
            {
                HomemadeFoodId = listingId,
                DeliveryArea = area
            })
            .ToList();

    private async Task ReplaceDeliveryAreasAsync(
        HomemadeFood entity, IEnumerable<HomemadeFoodDeliveryArea> requested)
    {
        var wanted = requested.Distinct().ToHashSet();

        var toRemove = entity.DeliveryAreas
            .Where(selection => !wanted.Contains(selection.DeliveryArea))
            .ToList();

        foreach (var selection in toRemove)
            entity.DeliveryAreas.Remove(selection);

        if (toRemove.Count > 0)
            _repository.RemoveDeliveryAreas(toRemove);

        var existing = entity.DeliveryAreas.Select(selection => selection.DeliveryArea).ToHashSet();
        var toAdd = BuildDeliveryAreas(entity.Id, wanted.Where(area => !existing.Contains(area)));

        if (toAdd.Count > 0)
        {
            await _repository.AddDeliveryAreasAsync(toAdd);
            foreach (var selection in toAdd)
                entity.DeliveryAreas.Add(selection);
        }
    }

    private async Task<HomemadeFoodDetailsDto> BuildDetailsAsync(
        Guid id, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        var entity = await _repository.GetByIdAsync(id, asNoTracking: true, cancellationToken, includeUnmoderated)
            ?? throw new NotFoundException("إعلان الأكل المنزلي مش موجود.");

        return _mapper.Map<HomemadeFoodDetailsDto>(entity);
    }

    private Task<List<HomemadeFoodImage>> AttachImagesAsync(
        HomemadeFood entity,
        IReadOnlyList<UploadImageModel> images,
        bool startAsPrimary,
        CancellationToken cancellationToken) =>
        ListingImages.AttachAsync(
            _fileService,
            entity.Images,
            images,
            ImageConstants.HomemadeFoodFolder,
            startAsPrimary,
            _ => new HomemadeFoodImage { HomemadeFoodId = entity.Id },
            cancellationToken);
}
