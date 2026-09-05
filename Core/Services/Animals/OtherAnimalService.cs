using AutoMapper;
using Domain.Entities;
using ServicesAbstraction;
using Shared.Enums;
using Shared.Constants;
using Shared.DTOs.Advertisements;
using Shared.DTOs.Animals;
using Shared.Exceptions;
using Shared.Responses;

namespace Services.Animals;

public class OtherAnimalService : IOtherAnimalService
{
    private readonly IOtherAnimalRepository _repository;
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;
    private readonly INotificationService _notifications;

    public OtherAnimalService(
        IOtherAnimalRepository repository,
        IFileService fileService,
        IMapper mapper,
        INotificationService notifications)
    {
        _repository = repository;
        _fileService = fileService;
        _mapper = mapper;
        _notifications = notifications;
    }

    public async Task<OtherAnimalDetailsDto> CreateAsync(
        string userId,
        CreateOtherAnimalRequest request,
        IReadOnlyList<UploadImageModel> images)
    {
        var entity = _mapper.Map<OtherAnimal>(request);

        entity.Id = Guid.NewGuid();
        entity.UserId = userId;
        entity.CreatedAt = DateTime.UtcNow;

        await AttachImagesAsync(entity, images, startAsPrimary: true);

        await _repository.AddAsync(entity);
        await _repository.SaveChangesAsync();

        await _notifications.NotifyAsync(
            userId, ListingModuleType.OtherAnimal, NotificationAction.Created, entity.Id, entity.Title);

        return await BuildDetailsAsync(entity.Id, includeUnmoderated: true);
    }

    public async Task<OtherAnimalDetailsDto> UpdateAsync(
        string userId,
        bool isAdmin,
        Guid id,
        UpdateOtherAnimalRequest request,
        IReadOnlyList<UploadImageModel> newImages)
    {
        var entity = isAdmin
            ? await _repository.GetByIdAsync(id, asNoTracking: false, includeUnmoderated: true)
            : await _repository.GetOwnedAsync(id, userId);

        if (entity is null)
            throw new NotFoundException("إعلان الحيوان مش موجود.");

        entity.SellerName = request.SellerName;
        entity.AnimalType = request.AnimalType;
        entity.OtherType = request.OtherType?.Trim();
        entity.Purpose = request.Purpose;
        entity.Age = request.Age;
        entity.Gender = request.Gender;
        entity.HealthStatus = request.HealthStatus;
        entity.Vaccination = request.Vaccination;
        entity.Quantity = request.Quantity;
        entity.Price = request.Price;
        entity.Negotiable = request.Negotiable;
        entity.Address = request.Address;
        entity.GoogleMaps = request.GoogleMaps?.Trim();
        entity.Phone = request.Phone;
        entity.WhatsApp = request.WhatsApp?.Trim();
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

        entity.UpdatedAt = DateTime.UtcNow;

        _repository.Update(entity);
        await _repository.SaveChangesAsync();

        await _notifications.NotifyAsync(
            entity.UserId, ListingModuleType.OtherAnimal,
            isAdmin && entity.UserId != userId ? NotificationAction.AdminUpdated : NotificationAction.Updated,
            id, entity.Title);

        return await BuildDetailsAsync(id, includeUnmoderated: true);
    }

    public async Task DeleteAsync(string userId, Guid id, bool isAdmin)
    {
        var entity = isAdmin
            ? await _repository.GetByIdAsync(id, asNoTracking: false, includeUnmoderated: true)
            : await _repository.GetOwnedAsync(id, userId);

        if (entity is null)
            throw new NotFoundException("إعلان الحيوان مش موجود.");

        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;

        _repository.Update(entity);
        await _repository.SaveChangesAsync();

        await _notifications.NotifyAsync(
            entity.UserId, ListingModuleType.OtherAnimal,
            isAdmin && entity.UserId != userId ? NotificationAction.AdminDeleted : NotificationAction.Deleted,
            id, entity.Title);
    }

    public async Task<PaginatedResult<OtherAnimalListItemDto>> GetListAsync(
        OtherAnimalFilterParams filter, CancellationToken cancellationToken = default)
    {
        var (items, total) = await _repository.GetPagedAsync(filter, cancellationToken);
        var mapped = _mapper.Map<IReadOnlyList<OtherAnimalListItemDto>>(items);
        return new PaginatedResult<OtherAnimalListItemDto>(
            mapped, total, filter.PageIndex, filter.PageSize);
    }

    public Task<OtherAnimalDetailsDto> GetByIdAsync(
        Guid id, CancellationToken cancellationToken = default) =>
        BuildDetailsAsync(id, cancellationToken);

    public async Task<IReadOnlyList<AnimalLookupItemDto>> GetTypesAsync(
        CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<AnimalLookupItemDto>>(
            await _repository.GetTypesAsync(cancellationToken));

    public async Task<IReadOnlyList<AnimalLookupItemDto>> GetPurposesAsync(
        CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<AnimalLookupItemDto>>(
            await _repository.GetPurposesAsync(cancellationToken));

    public async Task<IReadOnlyList<AnimalLookupItemDto>> GetAgesAsync(
        CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<AnimalLookupItemDto>>(
            await _repository.GetAgesAsync(cancellationToken));

    public async Task<IReadOnlyList<AnimalLookupItemDto>> GetGendersAsync(
        CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<AnimalLookupItemDto>>(
            await _repository.GetGendersAsync(cancellationToken));

    public async Task<IReadOnlyList<AnimalLookupItemDto>> GetHealthStatusesAsync(
        CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<AnimalLookupItemDto>>(
            await _repository.GetHealthStatusesAsync(cancellationToken));

    public async Task<IReadOnlyList<AnimalLookupItemDto>> GetVaccinationsAsync(
        CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<AnimalLookupItemDto>>(
            await _repository.GetVaccinationsAsync(cancellationToken));

    private async Task<OtherAnimalDetailsDto> BuildDetailsAsync(
        Guid id, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        var entity = await _repository.GetByIdAsync(id, asNoTracking: true, cancellationToken, includeUnmoderated)
            ?? throw new NotFoundException("إعلان الحيوان مش موجود.");

        return _mapper.Map<OtherAnimalDetailsDto>(entity);
    }

    private Task<List<OtherAnimalImage>> AttachImagesAsync(
        OtherAnimal entity, IReadOnlyList<UploadImageModel> images, bool startAsPrimary) =>
        ListingImages.AttachAsync(
            _fileService,
            entity.Images,
            images,
            ImageConstants.OtherAnimalFolder,
            startAsPrimary,
            _ => new OtherAnimalImage { OtherAnimalId = entity.Id });
}
