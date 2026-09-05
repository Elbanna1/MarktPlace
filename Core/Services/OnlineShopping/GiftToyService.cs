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

public class GiftToyService : IGiftToyService
{
    private readonly IGiftToyRepository _repository;
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;
    private readonly INotificationService _notifications;

    public GiftToyService(
        IGiftToyRepository repository,
        IFileService fileService,
        IMapper mapper,
        INotificationService notifications)
    {
        _repository = repository;
        _fileService = fileService;
        _mapper = mapper;
        _notifications = notifications;
    }

    public async Task<GiftToyDetailsDto> CreateAsync(
        string userId,
        CreateGiftToyRequest request,
        IReadOnlyList<UploadImageModel> images,
        UploadImageModel? video,
        CancellationToken cancellationToken = default)
    {
        if (images.Count == 0)
            throw new BadRequestException("لازم ترفع صورة واحدة على الأقل.");

        var entity = _mapper.Map<GiftToy>(request);

        entity.Id = Guid.NewGuid();
        entity.UserId = userId;
        entity.CreatedAt = DateTime.UtcNow;
        entity.OtherGiftType = NormalizeOther(request.GiftType == GiftToyType.Other, request.OtherGiftType);

        var stored = new List<string>();
        try
        {
            var added = await AttachImagesAsync(entity, images, startAsPrimary: true, cancellationToken);
            stored.AddRange(added.Select(image => image.ImagePath));

            if (video is not null)
            {
                var storedVideo = await _fileService.SaveVideoAsync(
                    video, FileUploadConstants.GiftToyVideoFolder, cancellationToken);
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
            userId, ListingModuleType.GiftToy, NotificationAction.Created, entity.Id, entity.Title);

        return await BuildDetailsAsync(entity.Id, cancellationToken, includeUnmoderated: true);
    }

    public async Task<GiftToyDetailsDto> UpdateAsync(
        string userId,
        bool isAdmin,
        Guid id,
        UpdateGiftToyRequest request,
        IReadOnlyList<UploadImageModel> newImages,
        UploadImageModel? video,
        CancellationToken cancellationToken = default)
    {
        var entity = isAdmin
            ? await _repository.GetByIdAsync(id, asNoTracking: false, cancellationToken, includeUnmoderated: true)
            : await _repository.GetOwnedAsync(id, userId, cancellationToken);

        if (entity is null)
            throw new NotFoundException("إعلان الهدايا والألعاب مش موجود.");

        entity.StoreName = request.StoreName;
        entity.Phone = request.Phone;
        entity.WhatsApp = request.WhatsApp;
        entity.GiftType = request.GiftType;
        entity.OtherGiftType = NormalizeOther(request.GiftType == GiftToyType.Other, request.OtherGiftType);
        entity.SuitableFor = request.SuitableFor;
        entity.GiftWrapping = request.GiftWrapping;
        entity.Price = request.Price;
        entity.DeliveryAvailable = request.DeliveryAvailable;
        entity.Title = request.Title;
        entity.Description = request.Description;

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
                video, FileUploadConstants.GiftToyVideoFolder, cancellationToken);

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
            entity.UserId, ListingModuleType.GiftToy,
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
            throw new NotFoundException("إعلان الهدايا والألعاب مش موجود.");

        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;

        _repository.Update(entity);
        await _repository.SaveChangesAsync();

        await _notifications.NotifyAsync(
            entity.UserId, ListingModuleType.GiftToy,
            isAdmin && entity.UserId != userId ? NotificationAction.AdminDeleted : NotificationAction.Deleted,
            id, entity.Title);
    }

    public async Task<PaginatedResult<GiftToyListItemDto>> GetListAsync(
        GiftToyFilterParams filter, CancellationToken cancellationToken = default)
    {
        var (items, total) = await _repository.GetPagedAsync(filter, cancellationToken);
        var mapped = _mapper.Map<IReadOnlyList<GiftToyListItemDto>>(items);
        return new PaginatedResult<GiftToyListItemDto>(mapped, total, filter.PageIndex, filter.PageSize);
    }

    public Task<GiftToyDetailsDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        BuildDetailsAsync(id, cancellationToken);

    public async Task<IReadOnlyList<OnlineShoppingLookupItemDto>> GetTypesAsync(
        CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<OnlineShoppingLookupItemDto>>(
            await _repository.GetTypesAsync(cancellationToken));

    public async Task<IReadOnlyList<OnlineShoppingLookupItemDto>> GetSuitableForAsync(
        CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<OnlineShoppingLookupItemDto>>(
            await _repository.GetSuitableForAsync(cancellationToken));

    private static string? NormalizeOther(bool isOtherSelected, string? value) =>
        isOtherSelected && !string.IsNullOrWhiteSpace(value) ? value.Trim() : null;

    private async Task<GiftToyDetailsDto> BuildDetailsAsync(
        Guid id, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        var entity = await _repository.GetByIdAsync(id, asNoTracking: true, cancellationToken, includeUnmoderated)
            ?? throw new NotFoundException("إعلان الهدايا والألعاب مش موجود.");

        return _mapper.Map<GiftToyDetailsDto>(entity);
    }

    private Task<List<GiftToyImage>> AttachImagesAsync(
        GiftToy entity,
        IReadOnlyList<UploadImageModel> images,
        bool startAsPrimary,
        CancellationToken cancellationToken) =>
        ListingImages.AttachAsync(
            _fileService,
            entity.Images,
            images,
            ImageConstants.GiftToyFolder,
            startAsPrimary,
            _ => new GiftToyImage { GiftToyId = entity.Id },
            cancellationToken);
}
