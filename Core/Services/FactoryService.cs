using AutoMapper;
using Domain.Entities;
using ServicesAbstraction;
using Shared.Enums;
using Shared.Constants;
using Shared.DTOs.Advertisements;
using Shared.DTOs.Factories;
using Shared.Exceptions;
using Shared.Responses;

namespace Services;

public class FactoryService : IFactoryService
{
    private readonly IFactoryRepository _repository;
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;
    private readonly INotificationService _notifications;

    public FactoryService(
        IFactoryRepository repository,
        IFileService fileService,
        IMapper mapper,
        INotificationService notifications)
    {
        _repository = repository;
        _fileService = fileService;
        _mapper = mapper;
        _notifications = notifications;
    }

    public async Task<FactoryDetailsDto> CreateAsync(
        string userId,
        CreateFactoryRequest request,
        IReadOnlyList<UploadImageModel> images)
    {
        var factory = _mapper.Map<Factory>(request);

        factory.Id = Guid.NewGuid();
        factory.UserId = userId;
        factory.CreatedAt = DateTime.UtcNow;

        await AttachImagesAsync(factory, images, startAsPrimary: true);

        await _repository.AddAsync(factory);
        await _repository.SaveChangesAsync();

        await _notifications.NotifyAsync(
            userId, ListingModuleType.Factory, NotificationAction.Created, factory.Id, factory.Title);

        return await BuildDetailsAsync(factory.Id, includeUnmoderated: true);
    }

    public async Task<FactoryDetailsDto> UpdateAsync(
        string userId,
        bool isAdmin,
        Guid id,
        UpdateFactoryRequest request,
        IReadOnlyList<UploadImageModel> newImages)
    {
        var factory = isAdmin
            ? await _repository.GetByIdAsync(id, asNoTracking: false, includeUnmoderated: true)
            : await _repository.GetOwnedAsync(id, userId);

        if (factory is null)
            throw new NotFoundException("المصنع مش موجود.");

        factory.FactoryName = request.FactoryName;
        factory.ProductionSpecialty = request.ProductionSpecialty;
        factory.OtherSpecialty = request.OtherSpecialty?.Trim();
        factory.Address = request.Address;
        factory.GoogleMaps = request.GoogleMaps?.Trim();
        factory.Phone = request.Phone;
        factory.WhatsApp = request.WhatsApp;
        factory.Email = request.Email?.Trim();
        factory.Title = request.Title;
        factory.Description = request.Description;

        if (request.RemoveImageIds.Count > 0)
        {
            var toRemove = factory.Images.Where(i => request.RemoveImageIds.Contains(i.Id)).ToList();
            foreach (var image in toRemove)
            {
                _fileService.Delete(image.ImagePath);
                _repository.RemoveImage(image);
                factory.Images.Remove(image);
            }
        }

        if (newImages.Count > 0)
        {
            var startAsPrimary = factory.Images.Count == 0;
            var added = await AttachImagesAsync(factory, newImages, startAsPrimary);
            await _repository.AddImagesAsync(added);
        }

        ListingImages.NormalizePrimary(factory.Images);

        factory.UpdatedAt = DateTime.UtcNow;

        _repository.Update(factory);
        await _repository.SaveChangesAsync();

        await _notifications.NotifyAsync(
            factory.UserId, ListingModuleType.Factory,
            isAdmin && factory.UserId != userId ? NotificationAction.AdminUpdated : NotificationAction.Updated,
            id, factory.Title);

        return await BuildDetailsAsync(id, includeUnmoderated: true);
    }

    public async Task DeleteAsync(string userId, Guid id, bool isAdmin)
    {
        var factory = isAdmin
            ? await _repository.GetByIdAsync(id, asNoTracking: false, includeUnmoderated: true)
            : await _repository.GetOwnedAsync(id, userId);

        if (factory is null)
            throw new NotFoundException("المصنع مش موجود.");

        factory.IsDeleted = true;
        factory.DeletedAt = DateTime.UtcNow;

        _repository.Update(factory);
        await _repository.SaveChangesAsync();

        await _notifications.NotifyAsync(
            factory.UserId, ListingModuleType.Factory,
            isAdmin && factory.UserId != userId ? NotificationAction.AdminDeleted : NotificationAction.Deleted,
            id, factory.Title);
    }

    public async Task<PaginatedResult<FactoryListItemDto>> GetListAsync(
        FactoryFilterParams filter, CancellationToken cancellationToken = default)
    {
        var (items, total) = await _repository.GetPagedAsync(filter, cancellationToken);
        var mapped = _mapper.Map<IReadOnlyList<FactoryListItemDto>>(items);
        return new PaginatedResult<FactoryListItemDto>(mapped, total, filter.PageIndex, filter.PageSize);
    }

    public Task<FactoryDetailsDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        BuildDetailsAsync(id, cancellationToken);

    public async Task<IReadOnlyList<ProductionSpecialtyOptionDto>> GetProductionSpecialtiesAsync(
        CancellationToken cancellationToken = default)
    {
        var rows = await _repository.GetProductionSpecialtiesAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<ProductionSpecialtyOptionDto>>(rows);
    }

    private async Task<FactoryDetailsDto> BuildDetailsAsync(
        Guid id, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        var factory = await _repository.GetByIdAsync(id, asNoTracking: true, cancellationToken, includeUnmoderated)
            ?? throw new NotFoundException("المصنع مش موجود.");

        return _mapper.Map<FactoryDetailsDto>(factory);
    }

    private Task<List<FactoryImage>> AttachImagesAsync(
        Factory factory, IReadOnlyList<UploadImageModel> images, bool startAsPrimary) =>
        ListingImages.AttachAsync(
            _fileService,
            factory.Images,
            images,
            ImageConstants.FactoriesFolder,
            startAsPrimary,
            _ => new FactoryImage { FactoryId = factory.Id });
}
