using AutoMapper;
using Domain.Entities;
using ServicesAbstraction;
using Shared.Enums;
using Shared.Constants;
using Shared.DTOs.Advertisements;
using Shared.DTOs.Suppliers;
using Shared.Exceptions;
using Shared.Responses;

namespace Services;

public class SupplierService : ISupplierService
{
    private readonly ISupplierRepository _repository;
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;
    private readonly INotificationService _notifications;

    public SupplierService(
        ISupplierRepository repository,
        IFileService fileService,
        IMapper mapper,
        INotificationService notifications)
    {
        _repository = repository;
        _fileService = fileService;
        _mapper = mapper;
        _notifications = notifications;
    }

    public async Task<SupplierDetailsDto> CreateAsync(
        string userId,
        CreateSupplierRequest request,
        IReadOnlyList<UploadImageModel> images)
    {
        var supplier = _mapper.Map<Supplier>(request);

        supplier.Id = Guid.NewGuid();
        supplier.UserId = userId;
        supplier.CreatedAt = DateTime.UtcNow;

        await AttachImagesAsync(supplier, images, startAsPrimary: true);

        await _repository.AddAsync(supplier);
        await _repository.SaveChangesAsync();

        await _notifications.NotifyAsync(
            userId, ListingModuleType.Supplier, NotificationAction.Created, supplier.Id, supplier.Title);

        return await BuildDetailsAsync(supplier.Id, includeUnmoderated: true);
    }

    public async Task<SupplierDetailsDto> UpdateAsync(
        string userId,
        bool isAdmin,
        Guid id,
        UpdateSupplierRequest request,
        IReadOnlyList<UploadImageModel> newImages)
    {
        var supplier = isAdmin
            ? await _repository.GetByIdAsync(id, asNoTracking: false, includeUnmoderated: true)
            : await _repository.GetOwnedAsync(id, userId);

        if (supplier is null)
            throw new NotFoundException("المورد مش موجود.");

        supplier.SupplierName = request.SupplierName;
        supplier.SupplierType = request.SupplierType;
        supplier.OtherSupplierType = request.OtherSupplierType?.Trim();
        supplier.SuppliedProduct = request.SuppliedProduct;
        supplier.SupplyDetails = request.SupplyDetails;
        supplier.Address = request.Address;
        supplier.GoogleMaps = request.GoogleMaps?.Trim();
        supplier.Phone = request.Phone;
        supplier.WhatsApp = request.WhatsApp;
        supplier.Email = request.Email?.Trim();
        supplier.Title = request.Title;
        supplier.Description = request.Description;

        if (request.RemoveImageIds.Count > 0)
        {
            var toRemove = supplier.Images.Where(i => request.RemoveImageIds.Contains(i.Id)).ToList();
            foreach (var image in toRemove)
            {
                _fileService.Delete(image.ImagePath);
                _repository.RemoveImage(image);
                supplier.Images.Remove(image);
            }
        }

        if (newImages.Count > 0)
        {
            var startAsPrimary = supplier.Images.Count == 0;
            var added = await AttachImagesAsync(supplier, newImages, startAsPrimary);
            await _repository.AddImagesAsync(added);
        }

        ListingImages.NormalizePrimary(supplier.Images);

        supplier.UpdatedAt = DateTime.UtcNow;

        _repository.Update(supplier);
        await _repository.SaveChangesAsync();

        await _notifications.NotifyAsync(
            supplier.UserId, ListingModuleType.Supplier,
            isAdmin && supplier.UserId != userId ? NotificationAction.AdminUpdated : NotificationAction.Updated,
            id, supplier.Title);

        return await BuildDetailsAsync(id, includeUnmoderated: true);
    }

    public async Task DeleteAsync(string userId, Guid id, bool isAdmin)
    {
        var supplier = isAdmin
            ? await _repository.GetByIdAsync(id, asNoTracking: false, includeUnmoderated: true)
            : await _repository.GetOwnedAsync(id, userId);

        if (supplier is null)
            throw new NotFoundException("المورد مش موجود.");

        supplier.IsDeleted = true;
        supplier.DeletedAt = DateTime.UtcNow;

        _repository.Update(supplier);
        await _repository.SaveChangesAsync();

        await _notifications.NotifyAsync(
            supplier.UserId, ListingModuleType.Supplier,
            isAdmin && supplier.UserId != userId ? NotificationAction.AdminDeleted : NotificationAction.Deleted,
            id, supplier.Title);
    }

    public async Task<PaginatedResult<SupplierListItemDto>> GetListAsync(
        SupplierFilterParams filter, CancellationToken cancellationToken = default)
    {
        var (items, total) = await _repository.GetPagedAsync(filter, cancellationToken);
        var mapped = _mapper.Map<IReadOnlyList<SupplierListItemDto>>(items);
        return new PaginatedResult<SupplierListItemDto>(mapped, total, filter.PageIndex, filter.PageSize);
    }

    public Task<SupplierDetailsDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        BuildDetailsAsync(id, cancellationToken);

    public async Task<IReadOnlyList<SupplierSpecializationDto>> GetTypesAsync(
        CancellationToken cancellationToken = default)
    {
        var rows = await _repository.GetSpecializationsAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<SupplierSpecializationDto>>(rows);
    }

    private async Task<SupplierDetailsDto> BuildDetailsAsync(
        Guid id, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        var supplier = await _repository.GetByIdAsync(id, asNoTracking: true, cancellationToken, includeUnmoderated)
            ?? throw new NotFoundException("المورد مش موجود.");

        return _mapper.Map<SupplierDetailsDto>(supplier);
    }

    private Task<List<SupplierImage>> AttachImagesAsync(
        Supplier supplier, IReadOnlyList<UploadImageModel> images, bool startAsPrimary) =>
        ListingImages.AttachAsync(
            _fileService,
            supplier.Images,
            images,
            ImageConstants.SuppliersFolder,
            startAsPrimary,
            _ => new SupplierImage { SupplierId = supplier.Id });
}
