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

public class LivestockService : ILivestockService
{
    private readonly ILivestockRepository _repository;
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;
    private readonly INotificationService _notifications;

    public LivestockService(
        ILivestockRepository repository,
        IFileService fileService,
        IMapper mapper,
        INotificationService notifications)
    {
        _repository = repository;
        _fileService = fileService;
        _mapper = mapper;
        _notifications = notifications;
    }

    public async Task<LivestockDetailsDto> CreateAsync(
        string userId,
        CreateLivestockRequest request,
        IReadOnlyList<UploadImageModel> images)
    {
        var entity = _mapper.Map<Livestock>(request);

        entity.Id = Guid.NewGuid();
        entity.UserId = userId;
        entity.CreatedAt = DateTime.UtcNow;

        await AttachImagesAsync(entity, images, startAsPrimary: true);

        await _repository.AddAsync(entity);
        await _repository.SaveChangesAsync();

        await _notifications.NotifyAsync(
            userId, ListingModuleType.Livestock, NotificationAction.Created, entity.Id, entity.Title);

        return await BuildDetailsAsync(entity.Id, includeUnmoderated: true);
    }

    public async Task<LivestockDetailsDto> UpdateAsync(
        string userId,
        bool isAdmin,
        Guid id,
        UpdateLivestockRequest request,
        IReadOnlyList<UploadImageModel> newImages)
    {
        var entity = isAdmin
            ? await _repository.GetByIdAsync(id, asNoTracking: false, includeUnmoderated: true)
            : await _repository.GetOwnedAsync(id, userId);

        if (entity is null)
            throw new NotFoundException("إعلان المواشي مش موجود.");

        entity.SellerName = request.SellerName;
        entity.Breed = request.Breed;
        entity.OtherBreed = request.OtherBreed?.Trim();
        entity.Purpose = request.Purpose;
        entity.Age = request.Age;
        entity.Gender = request.Gender;
        entity.HealthStatus = request.HealthStatus;
        entity.Vaccination = request.Vaccination;
        entity.Production = request.Production;
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
            entity.UserId, ListingModuleType.Livestock,
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
            throw new NotFoundException("إعلان المواشي مش موجود.");

        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;

        _repository.Update(entity);
        await _repository.SaveChangesAsync();

        await _notifications.NotifyAsync(
            entity.UserId, ListingModuleType.Livestock,
            isAdmin && entity.UserId != userId ? NotificationAction.AdminDeleted : NotificationAction.Deleted,
            id, entity.Title);
    }

    public async Task<PaginatedResult<LivestockListItemDto>> GetListAsync(
        LivestockFilterParams filter, CancellationToken cancellationToken = default)
    {
        var (items, total) = await _repository.GetPagedAsync(filter, cancellationToken);
        var mapped = _mapper.Map<IReadOnlyList<LivestockListItemDto>>(items);
        return new PaginatedResult<LivestockListItemDto>(
            mapped, total, filter.PageIndex, filter.PageSize);
    }

    public Task<LivestockDetailsDto> GetByIdAsync(
        Guid id, CancellationToken cancellationToken = default) =>
        BuildDetailsAsync(id, cancellationToken);

    public async Task<IReadOnlyList<AnimalLookupItemDto>> GetBreedsAsync(
        CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<AnimalLookupItemDto>>(
            await _repository.GetBreedsAsync(cancellationToken));

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

    public async Task<IReadOnlyList<AnimalLookupItemDto>> GetProductionsAsync(
        CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<AnimalLookupItemDto>>(
            await _repository.GetProductionsAsync(cancellationToken));

    private async Task<LivestockDetailsDto> BuildDetailsAsync(
        Guid id, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        var entity = await _repository.GetByIdAsync(id, asNoTracking: true, cancellationToken, includeUnmoderated)
            ?? throw new NotFoundException("إعلان المواشي مش موجود.");

        return _mapper.Map<LivestockDetailsDto>(entity);
    }

    private Task<List<LivestockImage>> AttachImagesAsync(
        Livestock entity, IReadOnlyList<UploadImageModel> images, bool startAsPrimary) =>
        ListingImages.AttachAsync(
            _fileService,
            entity.Images,
            images,
            ImageConstants.LivestockFolder,
            startAsPrimary,
            _ => new LivestockImage { LivestockId = entity.Id });
}
