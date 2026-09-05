using AutoMapper;
using Domain.Entities;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Advertisements;
using Shared.DTOs.Charity;
using Shared.Enums;
using Shared.Exceptions;
using Shared.Responses;

namespace Services.Charity;

public class RescueService : IRescueService
{
    private readonly IRescueRepository _repository;
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;
    private readonly INotificationService _notifications;

    public RescueService(
        IRescueRepository repository,
        IFileService fileService,
        IMapper mapper,
        INotificationService notifications)
    {
        _repository = repository;
        _fileService = fileService;
        _mapper = mapper;
        _notifications = notifications;
    }

    public async Task<RescueDetailsDto> CreateAsync(
        string userId, CreateRescueRequest request, IReadOnlyList<UploadImageModel> images,
        CancellationToken cancellationToken = default)
    {
        CharityListings.EnsureResponsibilityAccepted(request.IsResponsibilityAccepted);
        CharityListings.EnsureLocation(request.Latitude, request.Longitude);

        var entity = new Rescue
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        ApplyEditableFields(entity, request);

        var stored = new List<string>();
        try
        {
            var added = await AttachImagesAsync(entity, images, startAsPrimary: true, cancellationToken);
            stored.AddRange(added.Select(image => image.ImagePath));

            await _repository.AddAsync(entity, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            foreach (var path in stored)
                _fileService.Delete(path);

            throw;
        }

        await _notifications.NotifyAsync(
            userId, ListingModuleType.Rescue, NotificationAction.Created, entity.Id, entity.RescuerName);

        return await BuildDetailsAsync(entity.Id, cancellationToken, includeUnmoderated: true);
    }

    public async Task<RescueDetailsDto> UpdateAsync(
        string userId, bool isAdmin, Guid id, UpdateRescueRequest request,
        IReadOnlyList<UploadImageModel> newImages, CancellationToken cancellationToken = default)
    {
        CharityListings.EnsureResponsibilityAccepted(request.IsResponsibilityAccepted);
        CharityListings.EnsureLocation(request.Latitude, request.Longitude);

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
            var added = await AttachImagesAsync(
                entity, newImages, startAsPrimary: entity.Images.Count == 0, cancellationToken);
            await _repository.AddImagesAsync(added, cancellationToken);
        }

        ListingImages.NormalizeOrder(entity.Images);
        ListingImages.NormalizePrimary(entity.Images);

        entity.UpdatedAt = DateTime.UtcNow;

        _repository.Update(entity);
        await _repository.SaveChangesAsync(cancellationToken);

        await _notifications.NotifyAsync(
            entity.UserId, ListingModuleType.Rescue,
            isAdmin && entity.UserId != userId ? NotificationAction.AdminUpdated : NotificationAction.Updated,
            id, entity.RescuerName);

        return await BuildDetailsAsync(id, cancellationToken, includeUnmoderated: true);
    }

    public async Task DeleteAsync(
        string userId, Guid id, bool isAdmin, CancellationToken cancellationToken = default)
    {
        var entity = await LoadForWriteAsync(userId, isAdmin, id, cancellationToken);

        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;

        _repository.Update(entity);
        await _repository.SaveChangesAsync(cancellationToken);

        await _notifications.NotifyAsync(
            entity.UserId, ListingModuleType.Rescue,
            isAdmin && entity.UserId != userId ? NotificationAction.AdminDeleted : NotificationAction.Deleted,
            id, entity.RescuerName);
    }

    public async Task<PaginatedResult<RescueListItemDto>> GetListAsync(
        RescueFilterParams filter, CancellationToken cancellationToken = default)
    {
        var (items, total) = await _repository.GetPagedAsync(filter, cancellationToken);

        var mapped = items.Select(Map<RescueListItemDto>).ToList();

        return new PaginatedResult<RescueListItemDto>(mapped, total, filter.PageIndex, filter.PageSize);
    }

    public Task<RescueDetailsDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        BuildDetailsAsync(id, cancellationToken);

    private async Task<Rescue> LoadForWriteAsync(
        string userId, bool isAdmin, Guid id, CancellationToken cancellationToken)
    {
        var entity = isAdmin
            ? await _repository.GetByIdAsync(id, asNoTracking: false, cancellationToken, includeUnmoderated: true)
            : await _repository.GetOwnedAsync(id, userId, cancellationToken);

        return entity ?? throw new NotFoundException("الاستغاثة غير موجودة.");
    }

    private static void ApplyEditableFields(Rescue entity, CreateRescueRequest request)
    {
        entity.RescuerName = request.RescuerName.Trim();
        entity.Address = request.Address.Trim();
        entity.Details = request.Details.Trim();
        entity.Phone = request.Phone.Trim();

        entity.Latitude = request.Latitude;
        entity.Longitude = request.Longitude;

        entity.IsResponsibilityAccepted = request.IsResponsibilityAccepted;
        entity.ResponsibilityAcceptedAt ??= DateTime.UtcNow;
    }

    private async Task<RescueDetailsDto> BuildDetailsAsync(
        Guid id, CancellationToken cancellationToken, bool includeUnmoderated = false)
    {
        var entity = await _repository.GetByIdAsync(id, asNoTracking: true, cancellationToken, includeUnmoderated)
            ?? throw new NotFoundException("الاستغاثة غير موجودة.");

        return Map<RescueDetailsDto>(entity);
    }

    private TDto Map<TDto>(Rescue entity) where TDto : CharityDetailsDtoBase
    {
        var dto = _mapper.Map<TDto>(entity);

        CharityListings.ApplyClassification(
            dto, ListingModuleType.Rescue, _fileService, OwnerNameOf(entity));

        return dto;
    }

    private static string OwnerNameOf(Rescue entity) =>
        entity.User is null ? string.Empty : $"{entity.User.FirstName} {entity.User.SecondName}".Trim();

    private Task<List<RescueImage>> AttachImagesAsync(
        Rescue entity, IReadOnlyList<UploadImageModel> images, bool startAsPrimary,
        CancellationToken cancellationToken) =>
        ListingImages.AttachAsync(
            _fileService,
            entity.Images,
            images,
            FileUploadConstants.RescuesFolder,
            startAsPrimary,
            _ => new RescueImage { RescueId = entity.Id },
            cancellationToken);
}
