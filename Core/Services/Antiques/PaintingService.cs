using AutoMapper;
using Domain.Entities;
using ServicesAbstraction;
using Shared.Enums;
using Shared.Constants;
using Shared.DTOs.Advertisements;
using Shared.DTOs.Antiques;
using Shared.Exceptions;
using Shared.Responses;

namespace Services.Antiques;

public class PaintingService : IPaintingService
{
    private readonly IPaintingRepository _repository;
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;
    private readonly INotificationService _notifications;

    public PaintingService(
        IPaintingRepository repository,
        IFileService fileService,
        IMapper mapper,
        INotificationService notifications)
    {
        _repository = repository;
        _fileService = fileService;
        _mapper = mapper;
        _notifications = notifications;
    }

    public async Task<PaintingDetailsDto> CreateAsync(
        string userId,
        CreatePaintingRequest request,
        IReadOnlyList<UploadImageModel> images,
        UploadImageModel? video,
        CancellationToken cancellationToken = default)
    {
        if (images.Count == 0)
            throw new BadRequestException("لازم ترفع صورة واحدة على الأقل.");

        var entity = _mapper.Map<Painting>(request);

        entity.Id = Guid.NewGuid();
        entity.UserId = userId;
        entity.Governorate = LocationConstants.Governorate;
        entity.CreatedAt = DateTime.UtcNow;
        entity.OtherType = request.OtherType?.Trim();
        entity.OtherMaterial = request.OtherMaterial?.Trim();
        entity.GoogleMaps = request.GoogleMaps?.Trim();
        entity.WhatsApp = request.WhatsApp?.Trim();

        var stored = new List<string>();

        try
        {
            var added = await AttachImagesAsync(entity, images, startAsPrimary: true);
            stored.AddRange(added.Select(image => image.ImagePath));

            if (video is not null)
            {
                entity.Video = await ListingVideos.SaveAsync(
                    _fileService, video, FileUploadConstants.PaintingVideoFolder,
                    _ => new PaintingVideo { PaintingId = entity.Id }, cancellationToken);

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
            userId, ListingModuleType.Painting, NotificationAction.Created, entity.Id, entity.Title);

        return await BuildDetailsAsync(entity.Id, cancellationToken, includeUnmoderated: true);
    }

    public async Task<PaintingDetailsDto> UpdateAsync(
        string userId,
        bool isAdmin,
        Guid id,
        UpdatePaintingRequest request,
        IReadOnlyList<UploadImageModel> newImages,
        UploadImageModel? video,
        CancellationToken cancellationToken = default)
    {
        var entity = isAdmin
            ? await _repository.GetByIdAsync(id, asNoTracking: false, cancellationToken, includeUnmoderated: true)
            : await _repository.GetOwnedAsync(id, userId, cancellationToken);

        if (entity is null)
            throw new NotFoundException("إعلان اللوحة مش موجود.");

        entity.SellerName = request.SellerName;
        entity.Phone = request.Phone;
        entity.WhatsApp = request.WhatsApp?.Trim();
        entity.PaintingName = request.PaintingName;
        entity.PaintingType = request.PaintingType;
        entity.OtherType = request.OtherType?.Trim();
        entity.ArtistName = request.ArtistName;
        entity.ExecutionYear = request.ExecutionYear;
        entity.Width = request.Width;
        entity.Height = request.Height;
        entity.Material = request.Material;
        entity.OtherMaterial = request.OtherMaterial?.Trim();
        entity.Framed = request.Framed;
        entity.Originality = request.Originality;
        entity.SignedByArtist = request.SignedByArtist;
        entity.Price = request.Price;
        entity.Negotiable = request.Negotiable;
        entity.Center = request.Center;
        entity.Address = request.Address;
        entity.GoogleMaps = request.GoogleMaps?.Trim();
        entity.Title = request.Title;
        entity.Description = request.Description;

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
                _fileService, video, FileUploadConstants.PaintingVideoFolder,
                _ => new PaintingVideo { PaintingId = entity.Id }, cancellationToken);

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
            entity.UserId, ListingModuleType.Painting,
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
            throw new NotFoundException("إعلان اللوحة مش موجود.");

        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;

        _repository.Update(entity);
        await _repository.SaveChangesAsync();

        await _notifications.NotifyAsync(
            entity.UserId, ListingModuleType.Painting,
            isAdmin && entity.UserId != userId ? NotificationAction.AdminDeleted : NotificationAction.Deleted,
            id, entity.Title);
    }

    public async Task<PaginatedResult<PaintingListItemDto>> GetListAsync(
        PaintingFilterParams filter, CancellationToken cancellationToken = default)
    {
        var (items, total) = await _repository.GetPagedAsync(filter, cancellationToken);
        var mapped = _mapper.Map<IReadOnlyList<PaintingListItemDto>>(items);
        return new PaginatedResult<PaintingListItemDto>(
            mapped, total, filter.PageIndex, filter.PageSize);
    }

    public Task<PaintingDetailsDto> GetByIdAsync(
        Guid id, CancellationToken cancellationToken = default) =>
        BuildDetailsAsync(id, cancellationToken);

    public async Task<IReadOnlyList<AntiqueLookupItemDto>> GetTypesAsync(
        CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<AntiqueLookupItemDto>>(
            await _repository.GetTypesAsync(cancellationToken));

    public async Task<IReadOnlyList<AntiqueLookupItemDto>> GetMaterialsAsync(
        CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<AntiqueLookupItemDto>>(
            await _repository.GetMaterialsAsync(cancellationToken));

    public async Task<IReadOnlyList<AntiqueLookupItemDto>> GetOriginalitiesAsync(
        CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<AntiqueLookupItemDto>>(
            await _repository.GetOriginalitiesAsync(cancellationToken));

    private async Task<PaintingDetailsDto> BuildDetailsAsync(
        Guid id, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        var entity = await _repository.GetByIdAsync(id, asNoTracking: true, cancellationToken, includeUnmoderated)
            ?? throw new NotFoundException("إعلان اللوحة مش موجود.");

        return _mapper.Map<PaintingDetailsDto>(entity);
    }

    private Task<List<PaintingImage>> AttachImagesAsync(
        Painting entity, IReadOnlyList<UploadImageModel> images, bool startAsPrimary) =>
        ListingImages.AttachAsync(
            _fileService,
            entity.Images,
            images,
            ImageConstants.PaintingFolder,
            startAsPrimary,
            _ => new PaintingImage { PaintingId = entity.Id });
}
