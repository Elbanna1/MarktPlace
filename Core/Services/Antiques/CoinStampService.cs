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

public class CoinStampService : ICoinStampService
{
    private readonly ICoinStampRepository _repository;
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;
    private readonly INotificationService _notifications;

    public CoinStampService(
        ICoinStampRepository repository,
        IFileService fileService,
        IMapper mapper,
        INotificationService notifications)
    {
        _repository = repository;
        _fileService = fileService;
        _mapper = mapper;
        _notifications = notifications;
    }

    public async Task<CoinStampDetailsDto> CreateAsync(
        string userId,
        CreateCoinStampRequest request,
        IReadOnlyList<UploadImageModel> images,
        UploadImageModel? video,
        CancellationToken cancellationToken = default)
    {
        if (images.Count == 0)
            throw new BadRequestException("لازم ترفع صورة واحدة على الأقل.");

        var entity = _mapper.Map<CoinStamp>(request);

        entity.Id = Guid.NewGuid();
        entity.UserId = userId;
        entity.Governorate = LocationConstants.Governorate;
        entity.CreatedAt = DateTime.UtcNow;
        entity.OtherType = request.OtherType?.Trim();
        entity.OtherMetal = request.OtherMetal?.Trim();
        entity.Country = request.Country?.Trim();
        entity.Denomination = request.Denomination?.Trim();
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
                    _fileService, video, FileUploadConstants.CoinStampVideoFolder,
                    _ => new CoinStampVideo { CoinStampId = entity.Id }, cancellationToken);

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
            userId, ListingModuleType.CoinStamp, NotificationAction.Created, entity.Id, entity.Title);

        return await BuildDetailsAsync(entity.Id, cancellationToken, includeUnmoderated: true);
    }

    public async Task<CoinStampDetailsDto> UpdateAsync(
        string userId,
        bool isAdmin,
        Guid id,
        UpdateCoinStampRequest request,
        IReadOnlyList<UploadImageModel> newImages,
        UploadImageModel? video,
        CancellationToken cancellationToken = default)
    {
        var entity = isAdmin
            ? await _repository.GetByIdAsync(id, asNoTracking: false, cancellationToken, includeUnmoderated: true)
            : await _repository.GetOwnedAsync(id, userId, cancellationToken);

        if (entity is null)
            throw new NotFoundException("إعلان العملات والطوابع مش موجود.");

        entity.SellerName = request.SellerName;
        entity.Phone = request.Phone;
        entity.WhatsApp = request.WhatsApp?.Trim();
        entity.ItemName = request.ItemName;
        entity.ItemType = request.ItemType;
        entity.OtherType = request.OtherType?.Trim();
        entity.Country = request.Country?.Trim();
        entity.IssueYear = request.IssueYear;
        entity.Denomination = request.Denomination?.Trim();
        entity.Metal = request.Metal;
        entity.OtherMetal = request.OtherMetal?.Trim();
        entity.Condition = request.Condition;
        entity.IsOriginal = request.IsOriginal;
        entity.IsRare = request.IsRare;
        entity.HasCertificate = request.HasCertificate;
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
                _fileService, video, FileUploadConstants.CoinStampVideoFolder,
                _ => new CoinStampVideo { CoinStampId = entity.Id }, cancellationToken);

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
            entity.UserId, ListingModuleType.CoinStamp,
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
            throw new NotFoundException("إعلان العملات والطوابع مش موجود.");

        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;

        _repository.Update(entity);
        await _repository.SaveChangesAsync();

        await _notifications.NotifyAsync(
            entity.UserId, ListingModuleType.CoinStamp,
            isAdmin && entity.UserId != userId ? NotificationAction.AdminDeleted : NotificationAction.Deleted,
            id, entity.Title);
    }

    public async Task<PaginatedResult<CoinStampListItemDto>> GetListAsync(
        CoinStampFilterParams filter, CancellationToken cancellationToken = default)
    {
        var (items, total) = await _repository.GetPagedAsync(filter, cancellationToken);
        var mapped = _mapper.Map<IReadOnlyList<CoinStampListItemDto>>(items);
        return new PaginatedResult<CoinStampListItemDto>(
            mapped, total, filter.PageIndex, filter.PageSize);
    }

    public Task<CoinStampDetailsDto> GetByIdAsync(
        Guid id, CancellationToken cancellationToken = default) =>
        BuildDetailsAsync(id, cancellationToken);

    public async Task<IReadOnlyList<AntiqueLookupItemDto>> GetItemTypesAsync(
        CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<AntiqueLookupItemDto>>(
            await _repository.GetItemTypesAsync(cancellationToken));

    public async Task<IReadOnlyList<AntiqueLookupItemDto>> GetMetalsAsync(
        CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<AntiqueLookupItemDto>>(
            await _repository.GetMetalsAsync(cancellationToken));

    public async Task<IReadOnlyList<AntiqueLookupItemDto>> GetConditionsAsync(
        CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<AntiqueLookupItemDto>>(
            await _repository.GetConditionsAsync(cancellationToken));

    private async Task<CoinStampDetailsDto> BuildDetailsAsync(
        Guid id, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        var entity = await _repository.GetByIdAsync(id, asNoTracking: true, cancellationToken, includeUnmoderated)
            ?? throw new NotFoundException("إعلان العملات والطوابع مش موجود.");

        return _mapper.Map<CoinStampDetailsDto>(entity);
    }

    private Task<List<CoinStampImage>> AttachImagesAsync(
        CoinStamp entity, IReadOnlyList<UploadImageModel> images, bool startAsPrimary) =>
        ListingImages.AttachAsync(
            _fileService,
            entity.Images,
            images,
            ImageConstants.CoinStampFolder,
            startAsPrimary,
            _ => new CoinStampImage { CoinStampId = entity.Id });
}
