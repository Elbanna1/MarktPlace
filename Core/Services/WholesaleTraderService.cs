using AutoMapper;
using Domain.Entities;
using ServicesAbstraction;
using Shared.Enums;
using Shared.Constants;
using Shared.DTOs.Advertisements;
using Shared.DTOs.WholesaleTraders;
using Shared.Exceptions;
using Shared.Responses;

namespace Services;

public class WholesaleTraderService : IWholesaleTraderService
{
    private readonly IWholesaleTraderRepository _repository;
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;
    private readonly INotificationService _notifications;

    public WholesaleTraderService(
        IWholesaleTraderRepository repository,
        IFileService fileService,
        IMapper mapper,
        INotificationService notifications)
    {
        _repository = repository;
        _fileService = fileService;
        _mapper = mapper;
        _notifications = notifications;
    }

    public async Task<WholesaleTraderDetailsDto> CreateAsync(
        string userId,
        CreateWholesaleTraderRequest request,
        IReadOnlyList<UploadImageModel> images)
    {
        var trader = _mapper.Map<WholesaleTrader>(request);

        trader.Id = Guid.NewGuid();
        trader.UserId = userId;
        trader.CreatedAt = DateTime.UtcNow;

        await AttachImagesAsync(trader, images, startAsPrimary: true);

        await _repository.AddAsync(trader);
        await _repository.SaveChangesAsync();

        await _notifications.NotifyAsync(
            userId, ListingModuleType.WholesaleTrader, NotificationAction.Created, trader.Id, trader.Title);

        return await BuildDetailsAsync(trader.Id, includeUnmoderated: true);
    }

    public async Task<WholesaleTraderDetailsDto> UpdateAsync(
        string userId,
        bool isAdmin,
        Guid id,
        UpdateWholesaleTraderRequest request,
        IReadOnlyList<UploadImageModel> newImages)
    {
        var trader = isAdmin
            ? await _repository.GetByIdAsync(id, asNoTracking: false, includeUnmoderated: true)
            : await _repository.GetOwnedAsync(id, userId);

        if (trader is null)
            throw new NotFoundException("تاجر الجملة مش موجود.");

        trader.TraderName = request.TraderName;
        trader.TradeType = request.TradeType;
        trader.OtherTradeType = request.OtherTradeType?.Trim();
        trader.ProductsName = request.ProductsName;
        trader.ProductDetails = request.ProductDetails;
        trader.SaleType = request.SaleType;
        trader.Address = request.Address;
        trader.GoogleMaps = request.GoogleMaps?.Trim();
        trader.Phone = request.Phone;
        trader.WhatsApp = request.WhatsApp;
        trader.Email = request.Email?.Trim();
        trader.Title = request.Title;
        trader.Description = request.Description;

        if (request.RemoveImageIds.Count > 0)
        {
            var toRemove = trader.Images.Where(i => request.RemoveImageIds.Contains(i.Id)).ToList();
            foreach (var image in toRemove)
            {
                _fileService.Delete(image.ImagePath);
                _repository.RemoveImage(image);
                trader.Images.Remove(image);
            }
        }

        if (newImages.Count > 0)
        {
            var startAsPrimary = trader.Images.Count == 0;
            var added = await AttachImagesAsync(trader, newImages, startAsPrimary);
            await _repository.AddImagesAsync(added);
        }

        ListingImages.NormalizePrimary(trader.Images);

        trader.UpdatedAt = DateTime.UtcNow;

        _repository.Update(trader);
        await _repository.SaveChangesAsync();

        await _notifications.NotifyAsync(
            trader.UserId, ListingModuleType.WholesaleTrader,
            isAdmin && trader.UserId != userId ? NotificationAction.AdminUpdated : NotificationAction.Updated,
            id, trader.Title);

        return await BuildDetailsAsync(id, includeUnmoderated: true);
    }

    public async Task DeleteAsync(string userId, Guid id, bool isAdmin)
    {
        var trader = isAdmin
            ? await _repository.GetByIdAsync(id, asNoTracking: false, includeUnmoderated: true)
            : await _repository.GetOwnedAsync(id, userId);

        if (trader is null)
            throw new NotFoundException("تاجر الجملة مش موجود.");

        trader.IsDeleted = true;
        trader.DeletedAt = DateTime.UtcNow;

        _repository.Update(trader);
        await _repository.SaveChangesAsync();

        await _notifications.NotifyAsync(
            trader.UserId, ListingModuleType.WholesaleTrader,
            isAdmin && trader.UserId != userId ? NotificationAction.AdminDeleted : NotificationAction.Deleted,
            id, trader.Title);
    }

    public async Task<PaginatedResult<WholesaleTraderListItemDto>> GetListAsync(
        WholesaleTraderFilterParams filter, CancellationToken cancellationToken = default)
    {
        var (items, total) = await _repository.GetPagedAsync(filter, cancellationToken);
        var mapped = _mapper.Map<IReadOnlyList<WholesaleTraderListItemDto>>(items);
        return new PaginatedResult<WholesaleTraderListItemDto>(mapped, total, filter.PageIndex, filter.PageSize);
    }

    public Task<WholesaleTraderDetailsDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        BuildDetailsAsync(id, cancellationToken);

    public async Task<IReadOnlyList<WholesaleTradeTypeDto>> GetTradeTypesAsync(
        CancellationToken cancellationToken = default)
    {
        var rows = await _repository.GetTradeTypesAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<WholesaleTradeTypeDto>>(rows);
    }

    public IReadOnlyList<WholesaleSaleTypeDto> GetSaleTypes() =>
        WholesaleTraderCatalog.SaleTypeNames
            .Select(entry => new WholesaleSaleTypeDto
            {
                Id = (int)entry.Key,
                Name = entry.Value
            })
            .ToList();

    private async Task<WholesaleTraderDetailsDto> BuildDetailsAsync(
        Guid id, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        var trader = await _repository.GetByIdAsync(id, asNoTracking: true, cancellationToken, includeUnmoderated)
            ?? throw new NotFoundException("تاجر الجملة مش موجود.");

        return _mapper.Map<WholesaleTraderDetailsDto>(trader);
    }

    private Task<List<WholesaleTraderImage>> AttachImagesAsync(
        WholesaleTrader trader, IReadOnlyList<UploadImageModel> images, bool startAsPrimary) =>
        ListingImages.AttachAsync(
            _fileService,
            trader.Images,
            images,
            ImageConstants.WholesaleTradersFolder,
            startAsPrimary,
            _ => new WholesaleTraderImage { WholesaleTraderId = trader.Id });
}
