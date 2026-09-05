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

public class AccessoryService : IAccessoryService
{
    private readonly IAccessoryRepository _repository;
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;
    private readonly INotificationService _notifications;

    public AccessoryService(
        IAccessoryRepository repository,
        IFileService fileService,
        IMapper mapper,
        INotificationService notifications)
    {
        _repository = repository;
        _fileService = fileService;
        _mapper = mapper;
        _notifications = notifications;
    }

    public async Task<AccessoryDetailsDto> CreateAsync(
        string userId,
        CreateAccessoryRequest request,
        IReadOnlyList<UploadImageModel> images,
        UploadImageModel? logo,
        UploadImageModel? video,
        CancellationToken cancellationToken = default)
    {
        if (images.Count == 0)
            throw new BadRequestException("لازم ترفع صورة واحدة على الأقل.");

        var entity = _mapper.Map<Accessory>(request);

        entity.Id = Guid.NewGuid();
        entity.UserId = userId;
        entity.CreatedAt = DateTime.UtcNow;
        entity.OtherAccessoryType = NormalizeOther(request.AccessoryType == AccessoryType.Other, request.OtherAccessoryType);
        entity.OtherMaterial = NormalizeOther(request.Material == AccessoryMaterial.Other, request.OtherMaterial);
        entity.OtherColor = NormalizeOtherColor(request.Colors, request.OtherColor);

        entity.Colors = BuildColors(entity.Id, request.Colors);

        var stored = new List<string>();
        try
        {
            var added = await AttachImagesAsync(entity, images, startAsPrimary: true, cancellationToken);
            stored.AddRange(added.Select(image => image.ImagePath));

            if (logo is not null)
            {
                var storedLogo = await _fileService.SaveAsync(
                    logo, ImageConstants.AccessoryFolder, cancellationToken);
                stored.Add(storedLogo.RelativePath);

                entity.LogoPath = storedLogo.RelativePath;
                entity.LogoUrl = storedLogo.Url;
            }

            if (video is not null)
            {
                var storedVideo = await _fileService.SaveVideoAsync(
                    video, FileUploadConstants.AccessoryVideoFolder, cancellationToken);
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
            userId, ListingModuleType.Accessory, NotificationAction.Created, entity.Id, entity.Title);

        return await BuildDetailsAsync(entity.Id, cancellationToken, includeUnmoderated: true);
    }

    public async Task<AccessoryDetailsDto> UpdateAsync(
        string userId,
        bool isAdmin,
        Guid id,
        UpdateAccessoryRequest request,
        IReadOnlyList<UploadImageModel> newImages,
        UploadImageModel? logo,
        UploadImageModel? video,
        CancellationToken cancellationToken = default)
    {
        var entity = isAdmin
            ? await _repository.GetByIdAsync(id, asNoTracking: false, cancellationToken, includeUnmoderated: true)
            : await _repository.GetOwnedAsync(id, userId, cancellationToken);

        if (entity is null)
            throw new NotFoundException("إعلان الإكسسوار مش موجود.");

        entity.StoreName = request.StoreName;
        entity.Phone = request.Phone;
        entity.WhatsApp = request.WhatsApp;
        entity.AccessoryType = request.AccessoryType;
        entity.OtherAccessoryType = NormalizeOther(request.AccessoryType == AccessoryType.Other, request.OtherAccessoryType);
        entity.Category = request.Category;
        entity.Material = request.Material;
        entity.OtherMaterial = NormalizeOther(request.Material == AccessoryMaterial.Other, request.OtherMaterial);
        entity.OtherColor = NormalizeOtherColor(request.Colors, request.OtherColor);
        entity.Price = request.Price;
        entity.ShippingAvailable = request.ShippingAvailable;
        entity.Title = request.Title;
        entity.Description = request.Description;

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

        var replacedLogoPath = (string?)null;

        if (logo is not null)
        {
            var storedLogo = await _fileService.SaveAsync(
                logo, ImageConstants.AccessoryFolder, cancellationToken);

            replacedLogoPath = entity.LogoPath;
            entity.LogoPath = storedLogo.RelativePath;
            entity.LogoUrl = storedLogo.Url;
        }
        else if (request.RemoveLogo && !string.IsNullOrWhiteSpace(entity.LogoPath))
        {
            replacedLogoPath = entity.LogoPath;
            entity.LogoPath = null;
            entity.LogoUrl = null;
        }

        var replacedVideoPath = (string?)null;

        if (video is not null)
        {
            var storedVideo = await _fileService.SaveVideoAsync(
                video, FileUploadConstants.AccessoryVideoFolder, cancellationToken);

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

        if (!string.IsNullOrWhiteSpace(replacedLogoPath))
            _fileService.Delete(replacedLogoPath);

        if (!string.IsNullOrWhiteSpace(replacedVideoPath))
            _fileService.Delete(replacedVideoPath);

        await _notifications.NotifyAsync(
            entity.UserId, ListingModuleType.Accessory,
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
            throw new NotFoundException("إعلان الإكسسوار مش موجود.");

        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;

        _repository.Update(entity);
        await _repository.SaveChangesAsync();

        await _notifications.NotifyAsync(
            entity.UserId, ListingModuleType.Accessory,
            isAdmin && entity.UserId != userId ? NotificationAction.AdminDeleted : NotificationAction.Deleted,
            id, entity.Title);
    }

    public async Task<PaginatedResult<AccessoryListItemDto>> GetListAsync(
        AccessoryFilterParams filter, CancellationToken cancellationToken = default)
    {
        var (items, total) = await _repository.GetPagedAsync(filter, cancellationToken);
        var mapped = _mapper.Map<IReadOnlyList<AccessoryListItemDto>>(items);
        return new PaginatedResult<AccessoryListItemDto>(mapped, total, filter.PageIndex, filter.PageSize);
    }

    public Task<AccessoryDetailsDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        BuildDetailsAsync(id, cancellationToken);

    public async Task<IReadOnlyList<OnlineShoppingLookupItemDto>> GetAccessoryTypesAsync(
        CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<OnlineShoppingLookupItemDto>>(
            await _repository.GetAccessoryTypesAsync(cancellationToken));

    public async Task<IReadOnlyList<OnlineShoppingLookupItemDto>> GetCategoriesAsync(
        CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<OnlineShoppingLookupItemDto>>(
            await _repository.GetCategoriesAsync(cancellationToken));

    public async Task<IReadOnlyList<OnlineShoppingLookupItemDto>> GetMaterialsAsync(
        CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<OnlineShoppingLookupItemDto>>(
            await _repository.GetMaterialsAsync(cancellationToken));

    public async Task<IReadOnlyList<OnlineShoppingLookupItemDto>> GetColorsAsync(
        CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<OnlineShoppingLookupItemDto>>(
            await _repository.GetColorsAsync(cancellationToken));

    private static string? NormalizeOther(bool isOtherSelected, string? value) =>
        isOtherSelected && !string.IsNullOrWhiteSpace(value) ? value.Trim() : null;

    private static string? NormalizeOtherColor(IEnumerable<AccessoryColor> colors, string? otherColor) =>
        NormalizeOther(colors.Contains(AccessoryColor.Other), otherColor);

    private static List<AccessoryColorSelection> BuildColors(Guid listingId, IEnumerable<AccessoryColor> colors) =>
        colors.Distinct()
            .Select(color => new AccessoryColorSelection { AccessoryId = listingId, Color = color })
            .ToList();

    private async Task ReplaceColorsAsync(Accessory entity, IEnumerable<AccessoryColor> requested)
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

    private async Task<AccessoryDetailsDto> BuildDetailsAsync(
        Guid id, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        var entity = await _repository.GetByIdAsync(id, asNoTracking: true, cancellationToken, includeUnmoderated)
            ?? throw new NotFoundException("إعلان الإكسسوار مش موجود.");

        return _mapper.Map<AccessoryDetailsDto>(entity);
    }

    private Task<List<AccessoryImage>> AttachImagesAsync(
        Accessory entity,
        IReadOnlyList<UploadImageModel> images,
        bool startAsPrimary,
        CancellationToken cancellationToken) =>
        ListingImages.AttachAsync(
            _fileService,
            entity.Images,
            images,
            ImageConstants.AccessoryFolder,
            startAsPrimary,
            _ => new AccessoryImage { AccessoryId = entity.Id },
            cancellationToken);
}
