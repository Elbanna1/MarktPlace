using AutoMapper;
using Domain.Entities;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Advertisements;
using Shared.DTOs.Antiques;
using Shared.Enums;
using Shared.Exceptions;
using Shared.Responses;

namespace Services.Antiques;

public class HandmadeService : IHandmadeService
{
    private readonly IHandmadeRepository _repository;
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;
    private readonly INotificationService _notifications;

    public HandmadeService(
        IHandmadeRepository repository,
        IFileService fileService,
        IMapper mapper,
        INotificationService notifications)
    {
        _repository = repository;
        _fileService = fileService;
        _mapper = mapper;
        _notifications = notifications;
    }

    public async Task<HandmadeDetailsDto> CreateAsync(
        string userId,
        CreateHandmadeRequest request,
        IReadOnlyList<UploadImageModel> images,
        UploadImageModel? video,
        CancellationToken cancellationToken = default)
    {
        if (images.Count == 0)
            throw new BadRequestException("لازم ترفع صورة واحدة على الأقل.");

        var entity = _mapper.Map<Handmade>(request);

        entity.Id = Guid.NewGuid();
        entity.UserId = userId;
        entity.Governorate = LocationConstants.Governorate;
        entity.CreatedAt = DateTime.UtcNow;
        entity.OtherType = request.OtherType?.Trim();
        entity.OtherColor = request.OtherColor?.Trim();
        entity.ProductionTime = request.ProductionTime?.Trim();
        entity.Size = request.Size?.Trim();
        entity.GoogleMaps = request.GoogleMaps?.Trim();
        entity.WhatsApp = request.WhatsApp?.Trim();

        entity.Colors = BuildColors(entity.Id, request.Colors);

        var stored = new List<string>();

        try
        {
            var added = await AttachImagesAsync(entity, images, startAsPrimary: true);
            stored.AddRange(added.Select(image => image.ImagePath));

            if (video is not null)
            {
                entity.Video = await ListingVideos.SaveAsync(
                    _fileService, video, FileUploadConstants.HandmadeVideoFolder,
                    _ => new HandmadeVideo { HandmadeId = entity.Id }, cancellationToken);

                stored.Add(entity.Video.VideoPath);
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
            userId, ListingModuleType.Handmade, NotificationAction.Created, entity.Id, entity.Title);

        return await BuildDetailsAsync(entity.Id, cancellationToken, includeUnmoderated: true);
    }

    public async Task<HandmadeDetailsDto> UpdateAsync(
        string userId,
        bool isAdmin,
        Guid id,
        UpdateHandmadeRequest request,
        IReadOnlyList<UploadImageModel> newImages,
        UploadImageModel? video,
        CancellationToken cancellationToken = default)
    {
        var entity = isAdmin
            ? await _repository.GetByIdAsync(id, asNoTracking: false, cancellationToken, includeUnmoderated: true)
            : await _repository.GetOwnedAsync(id, userId, cancellationToken);

        if (entity is null)
            throw new NotFoundException("إعلان الشغل اليدوي مش موجود.");

        entity.SellerName = request.SellerName;
        entity.Phone = request.Phone;
        entity.WhatsApp = request.WhatsApp?.Trim();
        entity.ProductName = request.ProductName;
        entity.HandmadeType = request.HandmadeType;
        entity.OtherType = request.OtherType?.Trim();
        entity.Material = request.Material;
        entity.IsFullyHandmade = request.IsFullyHandmade;
        entity.CustomOrder = request.CustomOrder;
        entity.ProductionTime = request.ProductionTime?.Trim();
        entity.Size = request.Size?.Trim();
        entity.OtherColor = request.OtherColor?.Trim();
        entity.Price = request.Price;
        entity.Negotiable = request.Negotiable;
        entity.Center = request.Center;
        entity.Address = request.Address;
        entity.GoogleMaps = request.GoogleMaps?.Trim();
        entity.Title = request.Title;
        entity.Description = request.Description;

        var existingColors = entity.Colors.ToList();
        _repository.RemoveColors(existingColors);
        entity.Colors.Clear();

        var newColors = BuildColors(entity.Id, request.Colors);
        await _repository.AddColorsAsync(newColors);

        foreach (var color in newColors)
            entity.Colors.Add(color);

        if (request.RemoveImageIds.Count > 0)
        {
            var toRemove = entity.Images.Where(i => request.RemoveImageIds.Contains(i.Id)).ToList();
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
            var added = await AttachImagesAsync(entity, newImages, startAsPrimary);
            await _repository.AddImagesAsync(added);
        }

        ListingImages.NormalizePrimary(entity.Images);

        string? replacedVideoPath = null;

        if (video is not null)
        {
            var newVideo = await ListingVideos.SaveAsync(
                _fileService, video, FileUploadConstants.HandmadeVideoFolder,
                _ => new HandmadeVideo { HandmadeId = entity.Id }, cancellationToken);

            if (entity.Video is not null)
            {
                replacedVideoPath = entity.Video.VideoPath;
                _repository.RemoveVideo(entity.Video);
            }

            entity.Video = newVideo;
            await _repository.AddVideoAsync(newVideo);
        }
        else if (request.RemoveVideo && entity.Video is not null)
        {
            replacedVideoPath = entity.Video.VideoPath;
            _repository.RemoveVideo(entity.Video);
            entity.Video = null;
        }

        entity.UpdatedAt = DateTime.UtcNow;

        _repository.Update(entity);
        await _repository.SaveChangesAsync();

        if (replacedVideoPath is not null)
            _fileService.Delete(replacedVideoPath);

        await _notifications.NotifyAsync(
            entity.UserId, ListingModuleType.Handmade,
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
            throw new NotFoundException("إعلان الشغل اليدوي مش موجود.");

        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;

        _repository.Update(entity);
        await _repository.SaveChangesAsync();

        await _notifications.NotifyAsync(
            entity.UserId, ListingModuleType.Handmade,
            isAdmin && entity.UserId != userId ? NotificationAction.AdminDeleted : NotificationAction.Deleted,
            id, entity.Title);
    }

    public async Task<PaginatedResult<HandmadeListItemDto>> GetListAsync(
        HandmadeFilterParams filter, CancellationToken cancellationToken = default)
    {
        var (items, total) = await _repository.GetPagedAsync(filter, cancellationToken);
        var mapped = _mapper.Map<IReadOnlyList<HandmadeListItemDto>>(items);
        return new PaginatedResult<HandmadeListItemDto>(
            mapped, total, filter.PageIndex, filter.PageSize);
    }

    public Task<HandmadeDetailsDto> GetByIdAsync(
        Guid id, CancellationToken cancellationToken = default) =>
        BuildDetailsAsync(id, cancellationToken);

    public async Task<IReadOnlyList<AntiqueLookupItemDto>> GetTypesAsync(
        CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<AntiqueLookupItemDto>>(
            await _repository.GetTypesAsync(cancellationToken));

    public async Task<IReadOnlyList<AntiqueLookupItemDto>> GetColorsAsync(
        CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<AntiqueLookupItemDto>>(
            await _repository.GetColorsAsync(cancellationToken));

    private static List<HandmadeColorSelection> BuildColors(Guid handmadeId, IEnumerable<HandmadeColor> colors) =>
        colors
            .Distinct()
            .Select(color => new HandmadeColorSelection
            {
                Id = Guid.NewGuid(),
                HandmadeId = handmadeId,
                Color = color
            })
            .ToList();

    private async Task<HandmadeDetailsDto> BuildDetailsAsync(
        Guid id, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        var entity = await _repository.GetByIdAsync(id, asNoTracking: true, cancellationToken, includeUnmoderated)
            ?? throw new NotFoundException("إعلان الشغل اليدوي مش موجود.");

        return _mapper.Map<HandmadeDetailsDto>(entity);
    }

    private Task<List<HandmadeImage>> AttachImagesAsync(
        Handmade entity, IReadOnlyList<UploadImageModel> images, bool startAsPrimary) =>
        ListingImages.AttachAsync(
            _fileService,
            entity.Images,
            images,
            ImageConstants.HandmadeFolder,
            startAsPrimary,
            _ => new HandmadeImage { HandmadeId = entity.Id });
}
