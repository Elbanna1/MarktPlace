using AutoMapper;
using Domain.Entities;
using ServicesAbstraction;
using Shared.Enums;
using Shared.Constants;
using Shared.DTOs.Advertisements;
using Shared.DTOs.Workshops;
using Shared.Exceptions;
using Shared.Responses;

namespace Services;

public class WorkshopService : IWorkshopService
{
    private readonly IWorkshopRepository _repository;
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;
    private readonly INotificationService _notifications;

    public WorkshopService(
        IWorkshopRepository repository,
        IFileService fileService,
        IMapper mapper,
        INotificationService notifications)
    {
        _repository = repository;
        _fileService = fileService;
        _mapper = mapper;
        _notifications = notifications;
    }

    public async Task<WorkshopDetailsDto> CreateAsync(
        string userId,
        CreateWorkshopRequest request,
        IReadOnlyList<UploadImageModel> images)
    {
        if (images.Count == 0)
            throw new BadRequestException("لازم ترفع صورة واحدة على الأقل.");

        var workshop = _mapper.Map<Workshop>(request);

        workshop.Id = Guid.NewGuid();
        workshop.UserId = userId;
        workshop.Governorate = LocationConstants.Governorate;
        workshop.CreatedAt = DateTime.UtcNow;

        await AttachImagesAsync(workshop, images, startAsPrimary: true);

        await _repository.AddAsync(workshop);
        await _repository.SaveChangesAsync();

        await _notifications.NotifyAsync(
            userId, ListingModuleType.Workshop, NotificationAction.Created, workshop.Id, workshop.AdTitle);

        return await BuildDetailsAsync(workshop.Id, includeUnmoderated: true);
    }

    public async Task<WorkshopDetailsDto> UpdateAsync(
        string userId,
        bool isAdmin,
        Guid id,
        UpdateWorkshopRequest request,
        IReadOnlyList<UploadImageModel> newImages)
    {
        var workshop = isAdmin
            ? await _repository.GetByIdAsync(id, asNoTracking: false, includeUnmoderated: true)
            : await _repository.GetOwnedAsync(id, userId);

        if (workshop is null)
            throw new NotFoundException("الورشة مش موجودة.");

        workshop.Name = request.Name;
        workshop.WorkshopType = request.WorkshopType;
        workshop.OtherWorkshopType = request.OtherWorkshopType?.Trim();
        workshop.Center = request.Center;
        workshop.Address = request.Address;
        workshop.GoogleMapsUrl = request.GoogleMapsUrl?.Trim();
        workshop.PhoneNumber = request.PhoneNumber;
        workshop.WhatsApp = request.WhatsApp;
        workshop.Email = request.Email?.Trim();
        workshop.AdTitle = request.AdTitle;
        workshop.AdDescription = request.AdDescription;

        if (request.RemoveImageIds.Count > 0)
        {
            var toRemove = workshop.Images.Where(i => request.RemoveImageIds.Contains(i.Id)).ToList();
            foreach (var image in toRemove)
            {
                _fileService.Delete(image.ImagePath);
                _repository.RemoveImage(image);
                workshop.Images.Remove(image);
            }
        }

        if (newImages.Count > 0)
        {
            var startAsPrimary = workshop.Images.Count == 0;
            var added = await AttachImagesAsync(workshop, newImages, startAsPrimary);
            await _repository.AddImagesAsync(added);
        }

        ListingImages.NormalizePrimary(workshop.Images);

        workshop.UpdatedAt = DateTime.UtcNow;

        _repository.Update(workshop);
        await _repository.SaveChangesAsync();

        await _notifications.NotifyAsync(
            workshop.UserId, ListingModuleType.Workshop,
            isAdmin && workshop.UserId != userId ? NotificationAction.AdminUpdated : NotificationAction.Updated,
            id, workshop.AdTitle);

        return await BuildDetailsAsync(id, includeUnmoderated: true);
    }

    public async Task DeleteAsync(string userId, Guid id, bool isAdmin)
    {
        var workshop = isAdmin
            ? await _repository.GetByIdAsync(id, asNoTracking: false, includeUnmoderated: true)
            : await _repository.GetOwnedAsync(id, userId);

        if (workshop is null)
            throw new NotFoundException("الورشة مش موجودة.");

        workshop.IsDeleted = true;
        workshop.DeletedAt = DateTime.UtcNow;

        _repository.Update(workshop);
        await _repository.SaveChangesAsync();

        await _notifications.NotifyAsync(
            workshop.UserId, ListingModuleType.Workshop,
            isAdmin && workshop.UserId != userId ? NotificationAction.AdminDeleted : NotificationAction.Deleted,
            id, workshop.AdTitle);
    }

    public async Task<PaginatedResult<WorkshopListItemDto>> GetListAsync(
        WorkshopFilterParams filter, CancellationToken cancellationToken = default)
    {
        var (items, total) = await _repository.GetPagedAsync(filter, cancellationToken);
        var mapped = _mapper.Map<IReadOnlyList<WorkshopListItemDto>>(items);
        return new PaginatedResult<WorkshopListItemDto>(mapped, total, filter.PageIndex, filter.PageSize);
    }

    public Task<WorkshopDetailsDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        BuildDetailsAsync(id, cancellationToken);

    private async Task<WorkshopDetailsDto> BuildDetailsAsync(
        Guid id, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        var workshop = await _repository.GetByIdAsync(id, asNoTracking: true, cancellationToken, includeUnmoderated)
            ?? throw new NotFoundException("الورشة مش موجودة.");

        return _mapper.Map<WorkshopDetailsDto>(workshop);
    }

    private Task<List<WorkshopImage>> AttachImagesAsync(
        Workshop workshop, IReadOnlyList<UploadImageModel> images, bool startAsPrimary) =>
        ListingImages.AttachAsync(
            _fileService,
            workshop.Images,
            images,
            ImageConstants.WorkshopsFolder,
            startAsPrimary,
            _ => new WorkshopImage { WorkshopId = workshop.Id });
}
