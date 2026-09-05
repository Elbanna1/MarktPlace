using AutoMapper;
using Domain.Entities;
using ServicesAbstraction;
using Shared.Enums;
using Shared.Constants;
using Shared.DTOs.Advertisements;
using Shared.DTOs.Craftsmen;
using Shared.Exceptions;
using Shared.Responses;

namespace Services;

public class CraftsmanService : ICraftsmanService
{
    private readonly ICraftsmanRepository _repository;
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;
    private readonly INotificationService _notifications;

    public CraftsmanService(
        ICraftsmanRepository repository,
        IFileService fileService,
        IMapper mapper,
        INotificationService notifications)
    {
        _repository = repository;
        _fileService = fileService;
        _mapper = mapper;
        _notifications = notifications;
    }

    public async Task<CraftsmanDetailsDto> CreateAsync(
        string userId,
        CreateCraftsmanRequest request,
        IReadOnlyList<UploadImageModel> images)
    {
        if (images.Count == 0)
            throw new BadRequestException("لازم ترفع صورة واحدة على الأقل.");

        var craftsman = _mapper.Map<Craftsman>(request);

        craftsman.Id = Guid.NewGuid();
        craftsman.UserId = userId;
        craftsman.Governorate = LocationConstants.Governorate;
        craftsman.CreatedAt = DateTime.UtcNow;

        await AttachImagesAsync(craftsman, images, startAsPrimary: true);

        await _repository.AddAsync(craftsman);
        await _repository.SaveChangesAsync();

        await _notifications.NotifyAsync(
            userId, ListingModuleType.Craftsman, NotificationAction.Created, craftsman.Id, craftsman.AdTitle);

        return await BuildDetailsAsync(craftsman.Id, includeUnmoderated: true);
    }

    public async Task<CraftsmanDetailsDto> UpdateAsync(
        string userId,
        bool isAdmin,
        Guid id,
        UpdateCraftsmanRequest request,
        IReadOnlyList<UploadImageModel> newImages)
    {
        var craftsman = isAdmin
            ? await _repository.GetByIdAsync(id, asNoTracking: false, includeUnmoderated: true)
            : await _repository.GetOwnedAsync(id, userId);

        if (craftsman is null)
            throw new NotFoundException("الحرفي مش موجود.");

        craftsman.Name = request.Name;
        craftsman.Specialization = request.Specialization;
        craftsman.OtherSpecialization = request.OtherSpecialization?.Trim();
        craftsman.ExperienceLevel = request.ExperienceLevel;
        craftsman.Center = request.Center;
        craftsman.Address = request.Address;
        craftsman.GoogleMapsUrl = request.GoogleMapsUrl?.Trim();
        craftsman.PhoneNumber = request.PhoneNumber;
        craftsman.WhatsApp = request.WhatsApp;
        craftsman.Email = request.Email?.Trim();
        craftsman.AdTitle = request.AdTitle;
        craftsman.AdDescription = request.AdDescription;

        if (request.RemoveImageIds.Count > 0)
        {
            var toRemove = craftsman.Images.Where(i => request.RemoveImageIds.Contains(i.Id)).ToList();
            foreach (var image in toRemove)
            {
                _fileService.Delete(image.ImagePath);
                _repository.RemoveImage(image);
                craftsman.Images.Remove(image);
            }
        }

        if (newImages.Count > 0)
        {
            var startAsPrimary = craftsman.Images.Count == 0;
            var added = await AttachImagesAsync(craftsman, newImages, startAsPrimary);
            await _repository.AddImagesAsync(added);
        }

        ListingImages.NormalizePrimary(craftsman.Images);

        craftsman.UpdatedAt = DateTime.UtcNow;

        _repository.Update(craftsman);
        await _repository.SaveChangesAsync();

        await _notifications.NotifyAsync(
            craftsman.UserId, ListingModuleType.Craftsman,
            isAdmin && craftsman.UserId != userId ? NotificationAction.AdminUpdated : NotificationAction.Updated,
            id, craftsman.AdTitle);

        return await BuildDetailsAsync(id, includeUnmoderated: true);
    }

    public async Task DeleteAsync(string userId, Guid id, bool isAdmin)
    {
        var craftsman = isAdmin
            ? await _repository.GetByIdAsync(id, asNoTracking: false, includeUnmoderated: true)
            : await _repository.GetOwnedAsync(id, userId);

        if (craftsman is null)
            throw new NotFoundException("الحرفي مش موجود.");

        craftsman.IsDeleted = true;
        craftsman.DeletedAt = DateTime.UtcNow;

        _repository.Update(craftsman);
        await _repository.SaveChangesAsync();

        await _notifications.NotifyAsync(
            craftsman.UserId, ListingModuleType.Craftsman,
            isAdmin && craftsman.UserId != userId ? NotificationAction.AdminDeleted : NotificationAction.Deleted,
            id, craftsman.AdTitle);
    }

    public async Task<PaginatedResult<CraftsmanListItemDto>> GetListAsync(
        CraftsmanFilterParams filter, CancellationToken cancellationToken = default)
    {
        var (items, total) = await _repository.GetPagedAsync(filter, cancellationToken);
        var mapped = _mapper.Map<IReadOnlyList<CraftsmanListItemDto>>(items);
        return new PaginatedResult<CraftsmanListItemDto>(mapped, total, filter.PageIndex, filter.PageSize);
    }

    public Task<CraftsmanDetailsDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        BuildDetailsAsync(id, cancellationToken);

    private async Task<CraftsmanDetailsDto> BuildDetailsAsync(
        Guid id, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        var craftsman = await _repository.GetByIdAsync(id, asNoTracking: true, cancellationToken, includeUnmoderated)
            ?? throw new NotFoundException("الحرفي مش موجود.");

        return _mapper.Map<CraftsmanDetailsDto>(craftsman);
    }

    private Task<List<CraftsmanImage>> AttachImagesAsync(
        Craftsman craftsman, IReadOnlyList<UploadImageModel> images, bool startAsPrimary) =>
        ListingImages.AttachAsync(
            _fileService,
            craftsman.Images,
            images,
            ImageConstants.CraftsmenFolder,
            startAsPrimary,
            _ => new CraftsmanImage { CraftsmanId = craftsman.Id });
}
