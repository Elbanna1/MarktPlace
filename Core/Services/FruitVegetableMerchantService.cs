using AutoMapper;
using Domain.Entities;
using ServicesAbstraction;
using Shared.Enums;
using Shared.Constants;
using Shared.DTOs.Advertisements;
using Shared.DTOs.FruitVegetableMerchants;
using Shared.Exceptions;
using Shared.Responses;

namespace Services;

public class FruitVegetableMerchantService : IFruitVegetableMerchantService
{
    private readonly IFruitVegetableMerchantRepository _repository;
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;
    private readonly INotificationService _notifications;

    public FruitVegetableMerchantService(
        IFruitVegetableMerchantRepository repository,
        IFileService fileService,
        IMapper mapper,
        INotificationService notifications)
    {
        _repository = repository;
        _fileService = fileService;
        _mapper = mapper;
        _notifications = notifications;
    }

    public async Task<FruitVegetableMerchantDetailsDto> CreateAsync(
        string userId,
        CreateFruitVegetableMerchantRequest request,
        IReadOnlyList<UploadImageModel> images)
    {
        var merchant = _mapper.Map<FruitVegetableMerchant>(request);

        merchant.Id = Guid.NewGuid();
        merchant.UserId = userId;
        merchant.CreatedAt = DateTime.UtcNow;

        await AttachImagesAsync(merchant, images, startAsPrimary: true);

        await _repository.AddAsync(merchant);
        await _repository.SaveChangesAsync();

        await _notifications.NotifyAsync(
            userId, ListingModuleType.FruitVegetableMerchant, NotificationAction.Created, merchant.Id, merchant.Title);

        return await BuildDetailsAsync(merchant.Id, includeUnmoderated: true);
    }

    public async Task<FruitVegetableMerchantDetailsDto> UpdateAsync(
        string userId,
        bool isAdmin,
        Guid id,
        UpdateFruitVegetableMerchantRequest request,
        IReadOnlyList<UploadImageModel> newImages)
    {
        var merchant = isAdmin
            ? await _repository.GetByIdAsync(id, asNoTracking: false, includeUnmoderated: true)
            : await _repository.GetOwnedAsync(id, userId);

        if (merchant is null)
            throw new NotFoundException("التاجر مش موجود.");

        merchant.StallName = request.StallName;
        merchant.MerchantName = request.MerchantName;
        merchant.Phone = request.Phone;
        merchant.WhatsApp = request.WhatsApp?.Trim();
        merchant.Address = request.Address;
        merchant.GoogleMaps = request.GoogleMaps?.Trim();
        merchant.ProductName = request.ProductName;
        merchant.SaleType = request.SaleType;
        merchant.ProductDetails = request.ProductDetails;
        merchant.Title = request.Title;
        merchant.Description = request.Description;

        if (request.RemoveImageIds.Count > 0)
        {
            var toRemove = merchant.Images.Where(i => request.RemoveImageIds.Contains(i.Id)).ToList();
            foreach (var image in toRemove)
            {
                _fileService.Delete(image.ImagePath);
                _repository.RemoveImage(image);
                merchant.Images.Remove(image);
            }
        }

        if (newImages.Count > 0)
        {
            var startAsPrimary = merchant.Images.Count == 0;
            var added = await AttachImagesAsync(merchant, newImages, startAsPrimary);
            await _repository.AddImagesAsync(added);
        }

        ListingImages.NormalizePrimary(merchant.Images);

        merchant.UpdatedAt = DateTime.UtcNow;

        _repository.Update(merchant);
        await _repository.SaveChangesAsync();

        await _notifications.NotifyAsync(
            merchant.UserId, ListingModuleType.FruitVegetableMerchant,
            isAdmin && merchant.UserId != userId ? NotificationAction.AdminUpdated : NotificationAction.Updated,
            id, merchant.Title);

        return await BuildDetailsAsync(id, includeUnmoderated: true);
    }

    public async Task DeleteAsync(string userId, Guid id, bool isAdmin)
    {
        var merchant = isAdmin
            ? await _repository.GetByIdAsync(id, asNoTracking: false, includeUnmoderated: true)
            : await _repository.GetOwnedAsync(id, userId);

        if (merchant is null)
            throw new NotFoundException("التاجر مش موجود.");

        merchant.IsDeleted = true;
        merchant.DeletedAt = DateTime.UtcNow;

        _repository.Update(merchant);
        await _repository.SaveChangesAsync();

        await _notifications.NotifyAsync(
            merchant.UserId, ListingModuleType.FruitVegetableMerchant,
            isAdmin && merchant.UserId != userId ? NotificationAction.AdminDeleted : NotificationAction.Deleted,
            id, merchant.Title);
    }

    public async Task<PaginatedResult<FruitVegetableMerchantListItemDto>> GetListAsync(
        FruitVegetableMerchantFilterParams filter, CancellationToken cancellationToken = default)
    {
        var (items, total) = await _repository.GetPagedAsync(filter, cancellationToken);
        var mapped = _mapper.Map<IReadOnlyList<FruitVegetableMerchantListItemDto>>(items);
        return new PaginatedResult<FruitVegetableMerchantListItemDto>(
            mapped, total, filter.PageIndex, filter.PageSize);
    }

    public Task<FruitVegetableMerchantDetailsDto> GetByIdAsync(
        Guid id, CancellationToken cancellationToken = default) =>
        BuildDetailsAsync(id, cancellationToken);

    public async Task<IReadOnlyList<MerchantSaleTypeDto>> GetSaleTypesAsync(
        CancellationToken cancellationToken = default)
    {
        var rows = await _repository.GetSaleTypesAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<MerchantSaleTypeDto>>(rows);
    }

    private async Task<FruitVegetableMerchantDetailsDto> BuildDetailsAsync(
        Guid id, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        var merchant = await _repository.GetByIdAsync(id, asNoTracking: true, cancellationToken, includeUnmoderated)
            ?? throw new NotFoundException("التاجر مش موجود.");

        return _mapper.Map<FruitVegetableMerchantDetailsDto>(merchant);
    }

    private Task<List<FruitVegetableMerchantImage>> AttachImagesAsync(
        FruitVegetableMerchant merchant, IReadOnlyList<UploadImageModel> images, bool startAsPrimary) =>
        ListingImages.AttachAsync(
            _fileService,
            merchant.Images,
            images,
            ImageConstants.FruitVegetableMerchantsFolder,
            startAsPrimary,
            _ => new FruitVegetableMerchantImage { FruitVegetableMerchantId = merchant.Id });
}
