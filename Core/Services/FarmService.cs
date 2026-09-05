using AutoMapper;
using Domain.Entities;
using ServicesAbstraction;
using Shared.Enums;
using Shared.Constants;
using Shared.DTOs.Advertisements;
using Shared.DTOs.Farms;
using Shared.Exceptions;
using Shared.Responses;

namespace Services;

public class FarmService : IFarmService
{
    private readonly IFarmRepository _repository;
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;
    private readonly INotificationService _notifications;

    public FarmService(
        IFarmRepository repository,
        IFileService fileService,
        IMapper mapper,
        INotificationService notifications)
    {
        _repository = repository;
        _fileService = fileService;
        _mapper = mapper;
        _notifications = notifications;
    }

    public async Task<FarmDetailsDto> CreateAsync(
        string userId, CreateFarmRequest request, IReadOnlyList<UploadImageModel> images)
    {
        var farm = _mapper.Map<Farm>(request);

        farm.Id = Guid.NewGuid();
        farm.UserId = userId;
        farm.CreatedAt = DateTime.UtcNow;

        await AttachImagesAsync(farm, images, startAsPrimary: true);

        await _repository.AddAsync(farm);
        await _repository.SaveChangesAsync();

        await _notifications.NotifyAsync(
            userId, ListingModuleType.Farm, NotificationAction.Created, farm.Id, farm.Title);

        return await BuildDetailsAsync(farm.Id, includeUnmoderated: true);
    }

    public async Task<FarmDetailsDto> UpdateAsync(
        string userId, bool isAdmin, Guid id, UpdateFarmRequest request,
        IReadOnlyList<UploadImageModel> newImages)
    {
        var farm = isAdmin
            ? await _repository.GetByIdAsync(id, asNoTracking: false, includeUnmoderated: true)
            : await _repository.GetOwnedAsync(id, userId);

        if (farm is null)
            throw new NotFoundException("المزرعة مش موجودة.");

        farm.FarmName = request.FarmName;
        farm.FarmType = request.FarmType;
        farm.OtherFarmType = request.OtherFarmType?.Trim();
        farm.Address = request.Address;
        farm.GoogleMaps = request.GoogleMaps?.Trim();
        farm.Phone = request.Phone;
        farm.WhatsApp = request.WhatsApp;
        farm.Email = request.Email?.Trim();
        farm.AreaInFeddan = request.AreaInFeddan;
        farm.AvailableQuantity = request.AvailableQuantity?.Trim();
        farm.AvailabilitySeason = request.AvailabilitySeason;
        farm.FarmingMethod = request.FarmingMethod;
        farm.Title = request.Title;
        farm.Description = request.Description;

        if (request.RemoveImageIds.Count > 0)
        {
            var toRemove = farm.Images.Where(i => request.RemoveImageIds.Contains(i.Id)).ToList();
            foreach (var image in toRemove)
            {
                _fileService.Delete(image.ImagePath);
                _repository.RemoveImage(image);
                farm.Images.Remove(image);
            }
        }

        if (newImages.Count > 0)
        {
            var startAsPrimary = farm.Images.Count == 0;
            var added = await AttachImagesAsync(farm, newImages, startAsPrimary);
            await _repository.AddImagesAsync(added);
        }

        ListingImages.NormalizePrimary(farm.Images);

        farm.UpdatedAt = DateTime.UtcNow;

        _repository.Update(farm);
        await _repository.SaveChangesAsync();

        await _notifications.NotifyAsync(
            farm.UserId, ListingModuleType.Farm,
            isAdmin && farm.UserId != userId ? NotificationAction.AdminUpdated : NotificationAction.Updated,
            id, farm.Title);

        return await BuildDetailsAsync(id, includeUnmoderated: true);
    }

    public async Task DeleteAsync(string userId, Guid id, bool isAdmin)
    {
        var farm = isAdmin
            ? await _repository.GetByIdAsync(id, asNoTracking: false, includeUnmoderated: true)
            : await _repository.GetOwnedAsync(id, userId);

        if (farm is null)
            throw new NotFoundException("المزرعة مش موجودة.");

        farm.IsDeleted = true;
        farm.DeletedAt = DateTime.UtcNow;

        _repository.Update(farm);
        await _repository.SaveChangesAsync();

        await _notifications.NotifyAsync(
            farm.UserId, ListingModuleType.Farm,
            isAdmin && farm.UserId != userId ? NotificationAction.AdminDeleted : NotificationAction.Deleted,
            id, farm.Title);
    }

    public async Task<PaginatedResult<FarmListItemDto>> GetListAsync(
        FarmFilterParams filter, CancellationToken cancellationToken = default)
    {
        var (items, total) = await _repository.GetPagedAsync(filter, cancellationToken);
        var mapped = _mapper.Map<IReadOnlyList<FarmListItemDto>>(items);
        return new PaginatedResult<FarmListItemDto>(mapped, total, filter.PageIndex, filter.PageSize);
    }

    public Task<FarmDetailsDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        BuildDetailsAsync(id, cancellationToken);

    public async Task<IReadOnlyList<FarmTypeOptionDto>> GetFarmTypesAsync(
        CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<FarmTypeOptionDto>>(
            await _repository.GetFarmTypesAsync(cancellationToken));

    public async Task<IReadOnlyList<FarmingMethodOptionDto>> GetFarmingMethodsAsync(
        CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<FarmingMethodOptionDto>>(
            await _repository.GetFarmingMethodsAsync(cancellationToken));

    public async Task<IReadOnlyList<AvailabilitySeasonOptionDto>> GetAvailabilitySeasonsAsync(
        CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<AvailabilitySeasonOptionDto>>(
            await _repository.GetAvailabilitySeasonsAsync(cancellationToken));

    private async Task<FarmDetailsDto> BuildDetailsAsync(
        Guid id, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        var farm = await _repository.GetByIdAsync(id, asNoTracking: true, cancellationToken, includeUnmoderated)
            ?? throw new NotFoundException("المزرعة مش موجودة.");

        return _mapper.Map<FarmDetailsDto>(farm);
    }

    private Task<List<FarmImage>> AttachImagesAsync(
        Farm farm, IReadOnlyList<UploadImageModel> images, bool startAsPrimary) =>
        ListingImages.AttachAsync(
            _fileService,
            farm.Images,
            images,
            ImageConstants.FarmsFolder,
            startAsPrimary,
            _ => new FarmImage { FarmId = farm.Id });
}
