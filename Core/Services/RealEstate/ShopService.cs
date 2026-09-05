using AutoMapper;
using Domain.Entities;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Advertisements;
using Shared.DTOs.RealEstate;
using Shared.Enums;
using Shared.Exceptions;
using Shared.Responses;

namespace Services.RealEstate;

public class ShopService : IShopService
{
    private readonly IShopRepository _repository;
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;
    private readonly INotificationService _notifications;

    public ShopService(
        IShopRepository repository,
        IFileService fileService,
        IMapper mapper,
        INotificationService notifications)
    {
        _repository = repository;
        _fileService = fileService;
        _mapper = mapper;
        _notifications = notifications;
    }

    public async Task<ShopDetailsDto> CreateAsync(
        string userId,
        CreateShopRequest request,
        IReadOnlyList<UploadImageModel> images,
        UploadImageModel? video,
        CancellationToken cancellationToken = default)
    {
        RealEstateListings.EnsureHasImages(images.Count);

        var entity = new Shop
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Governorate = RealEstateListings.Governorate,
            CreatedAt = DateTime.UtcNow
        };

        ApplyEditableFields(entity, request);

        var isRent = RealEstateListings.IsRent(request.ListingType);

        entity.Utilities = BuildUtilities(entity.Id, request.Utilities);
        entity.RentInclusions = BuildRentInclusions(
            entity.Id, RealEstateListings.Keep(isRent, request.RentInclusions));

        entity.RentSuitableActivities = BuildRentSuitableActivities(
            entity.Id, Array.Empty<ShopRentSuitableActivity>());

        var stored = new List<string>();
        try
        {
            var added = await AttachImagesAsync(entity, images, cancellationToken);
            stored.AddRange(added.Select(image => image.ImagePath));

            if (video is not null)
            {
                var storedVideo = await _fileService.SaveVideoAsync(
                    video, FileUploadConstants.ShopVideoFolder, cancellationToken);
                stored.Add(storedVideo.RelativePath);

                entity.VideoPath = storedVideo.RelativePath;
                entity.VideoUrl = storedVideo.Url;
            }

            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();
        }
        catch
        {
            foreach (var path in stored)
                _fileService.Delete(path);

            throw;
        }

        await _notifications.NotifyAsync(
            userId, ListingModuleType.Shop, NotificationAction.Created, entity.Id, entity.Title);

        return await BuildDetailsAsync(entity.Id, cancellationToken, includeUnmoderated: true);
    }

    public async Task<ShopDetailsDto> UpdateAsync(
        string userId,
        bool isAdmin,
        Guid id,
        UpdateShopRequest request,
        IReadOnlyList<UploadImageModel> newImages,
        UploadImageModel? video,
        CancellationToken cancellationToken = default)
    {
        var entity = await LoadForWriteAsync(userId, isAdmin, id, cancellationToken);

        ApplyEditableFields(entity, request);

        var isRent = RealEstateListings.IsRent(request.ListingType);

        await ReplaceUtilitiesAsync(entity, request.Utilities);
        await ReplaceRentInclusionsAsync(entity, RealEstateListings.Keep(isRent, request.RentInclusions));

        await ReplaceRentSuitableActivitiesAsync(entity, Array.Empty<ShopRentSuitableActivity>());

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
            var added = await AttachImagesAsync(entity, newImages, cancellationToken);
            await _repository.AddImagesAsync(added);
        }

        RealEstateListings.EnsureHasImages(entity.Images.Count);
        ListingImages.NormalizeOrder(entity.Images);

        var replacedVideoPath = (string?)null;

        if (video is not null)
        {
            var storedVideo = await _fileService.SaveVideoAsync(
                video, FileUploadConstants.ShopVideoFolder, cancellationToken);

            replacedVideoPath = entity.VideoPath;
            entity.VideoPath = storedVideo.RelativePath;
            entity.VideoUrl = storedVideo.Url;
        }
        else if (request.RemoveVideo && !string.IsNullOrWhiteSpace(entity.VideoPath))
        {
            replacedVideoPath = entity.VideoPath;
            entity.VideoPath = null;
            entity.VideoUrl = null;
        }

        entity.UpdatedAt = DateTime.UtcNow;

        _repository.Update(entity);
        await _repository.SaveChangesAsync();

        if (!string.IsNullOrWhiteSpace(replacedVideoPath))
            _fileService.Delete(replacedVideoPath);

        await _notifications.NotifyAsync(
            entity.UserId, ListingModuleType.Shop,
            isAdmin && entity.UserId != userId ? NotificationAction.AdminUpdated : NotificationAction.Updated,
            id, entity.Title);

        return await BuildDetailsAsync(id, cancellationToken, includeUnmoderated: true);
    }

    public async Task DeleteAsync(string userId, Guid id, bool isAdmin)
    {
        var entity = isAdmin
            ? await _repository.GetByIdAsync(id, asNoTracking: false, includeUnmoderated: true)
            : await _repository.GetOwnedAsync(id, userId);

        if (entity is null)
            throw new NotFoundException("إعلان المحل مش موجود.");

        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;

        _repository.Update(entity);
        await _repository.SaveChangesAsync();

        await _notifications.NotifyAsync(
            entity.UserId, ListingModuleType.Shop,
            isAdmin && entity.UserId != userId ? NotificationAction.AdminDeleted : NotificationAction.Deleted,
            id, entity.Title);
    }

    public async Task<PaginatedResult<ShopListItemDto>> GetListAsync(
        ShopFilterParams filter, CancellationToken cancellationToken = default)
    {
        var (items, total) = await _repository.GetPagedAsync(filter, cancellationToken);
        var mapped = _mapper.Map<IReadOnlyList<ShopListItemDto>>(items);
        return new PaginatedResult<ShopListItemDto>(mapped, total, filter.PageIndex, filter.PageSize);
    }

    public async Task<ShopDetailsDto> GetByIdAsync(
        Guid id, bool countView = false, CancellationToken cancellationToken = default)
    {
        var details = await BuildDetailsAsync(id, cancellationToken);

        if (countView)
        {
            await _repository.IncrementViewCountAsync(id, cancellationToken);
            details.ViewCount++;
        }

        return details;
    }

    public async Task<IReadOnlyList<ShopListItemDto>> GetSimilarAsync(
        Guid id, int count, CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<ShopListItemDto>>(
            await _repository.GetSimilarAsync(id, RealEstateListings.StripSize(count), cancellationToken));

    public async Task<IReadOnlyList<ShopListItemDto>> GetRelatedAsync(
        Guid id, int count, CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<ShopListItemDto>>(
            await _repository.GetRelatedAsync(id, RealEstateListings.StripSize(count), cancellationToken));

    public async Task<IReadOnlyList<ShopListItemDto>> GetRecentlyAddedAsync(
        int count, CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<ShopListItemDto>>(
            await _repository.GetRecentlyAddedAsync(RealEstateListings.StripSize(count), cancellationToken));

    public Task<RealEstatePriceStatisticsDto> GetPriceStatisticsAsync(
        ShopFilterParams filter, CancellationToken cancellationToken = default) =>
        _repository.GetPriceStatisticsAsync(filter, cancellationToken);

    public async Task<IReadOnlyList<RealEstateSuggestionDto>> GetSuggestionsAsync(
        string? term, int count, CancellationToken cancellationToken = default)
    {
        var trimmed = term?.Trim();

        if (string.IsNullOrEmpty(trimmed) || trimmed.Length < RealEstateListings.MinSuggestionLength)
            return Array.Empty<RealEstateSuggestionDto>();

        return await _repository.GetSuggestionsAsync(
            trimmed, RealEstateListings.SuggestionCount(count), cancellationToken);
    }

    public async Task<ShopDetailsDto> ReorderImagesAsync(
        string userId, bool isAdmin, Guid id,
        ReorderRealEstateImagesRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await LoadForWriteAsync(userId, isAdmin, id, cancellationToken);

        if (!ListingImages.TryReorder(entity.Images, request.ImageIds))
            throw new BadRequestException(
                "ترتيب الصور لازم يشمل كل صورة في الإعلان مرة واحدة بالظبط.");

        entity.UpdatedAt = DateTime.UtcNow;

        _repository.Update(entity);
        await _repository.SaveChangesAsync();

        return await BuildDetailsAsync(id, cancellationToken, includeUnmoderated: true);
    }

    public async Task<ShopDetailsDto> SetPromotionAsync(
        Guid id, PromoteRealEstateRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, asNoTracking: false, cancellationToken, includeUnmoderated: true)
            ?? throw new NotFoundException("إعلان المحل مش موجود.");

        entity.IsFeatured = request.IsFeatured;
        entity.IsPremium = request.IsPremium;
        entity.IsUrgent = request.IsUrgent;
        entity.UpdatedAt = DateTime.UtcNow;

        _repository.Update(entity);
        await _repository.SaveChangesAsync();

        await _notifications.NotifyAsync(
            entity.UserId, ListingModuleType.Shop, NotificationAction.AdminUpdated, id, entity.Title);

        return await BuildDetailsAsync(id, cancellationToken, includeUnmoderated: true);
    }

    public Task<IReadOnlyList<RealEstateLookupItemDto>> GetLookupAsync(
        string lookupKey, CancellationToken cancellationToken = default) =>
        lookupKey switch
        {
            RealEstateLookupKeys.ListingTypes =>
                _repository.GetLookupAsync<RealEstateListingTypeLookup>(cancellationToken),
            RealEstateLookupKeys.Projects =>
                _repository.GetLookupAsync<RealEstateProjectLookup>(cancellationToken),
            RealEstateLookupKeys.SuitableActivities =>
                _repository.GetLookupAsync<ShopSuitableActivityLookup>(cancellationToken),
            RealEstateLookupKeys.FloorTypes =>
                _repository.GetLookupAsync<ShopFloorTypeLookup>(cancellationToken),
            RealEstateLookupKeys.FacadesCounts =>
                _repository.GetLookupAsync<ShopFacadesCountLookup>(cancellationToken),
            RealEstateLookupKeys.FacadeDirections =>
                _repository.GetLookupAsync<ShopFacadeDirectionLookup>(cancellationToken),
            RealEstateLookupKeys.FinishingTypes =>
                _repository.GetLookupAsync<ShopFinishingTypeLookup>(cancellationToken),
            RealEstateLookupKeys.PropertyAges =>
                _repository.GetLookupAsync<ShopPropertyAgeLookup>(cancellationToken),
            RealEstateLookupKeys.EntrancesCounts =>
                _repository.GetLookupAsync<ShopEntrancesCountLookup>(cancellationToken),
            RealEstateLookupKeys.LegalStatuses =>
                _repository.GetLookupAsync<ShopLegalStatusLookup>(cancellationToken),
            RealEstateLookupKeys.LicenseTypes =>
                _repository.GetLookupAsync<ShopLicenseTypeLookup>(cancellationToken),
            RealEstateLookupKeys.ReconciliationForms =>
                _repository.GetLookupAsync<ShopReconciliationFormLookup>(cancellationToken),
            RealEstateLookupKeys.OwnershipDocuments =>
                _repository.GetLookupAsync<ShopOwnershipDocumentLookup>(cancellationToken),
            RealEstateLookupKeys.Utilities =>
                _repository.GetLookupAsync<ShopUtilityLookup>(cancellationToken),
            RealEstateLookupKeys.PaymentMethods =>
                _repository.GetLookupAsync<ShopPaymentMethodLookup>(cancellationToken),
            RealEstateLookupKeys.InstallmentProviders =>
                _repository.GetLookupAsync<ShopInstallmentProviderLookup>(cancellationToken),
            RealEstateLookupKeys.RentTypes =>
                _repository.GetLookupAsync<ShopRentTypeLookup>(cancellationToken),
            RealEstateLookupKeys.RentInclusions =>
                _repository.GetLookupAsync<ShopRentInclusionLookup>(cancellationToken),
            RealEstateLookupKeys.RentSuitableActivities =>
                _repository.GetLookupAsync<ShopRentSuitableActivityLookup>(cancellationToken),
            RealEstateLookupKeys.ExchangeTargets =>
                _repository.GetLookupAsync<ShopExchangeWithLookup>(cancellationToken),

            _ => throw new NotFoundException($"القائمة '{lookupKey}' مش معروفة.")
        };

    private async Task<Shop> LoadForWriteAsync(
        string userId, bool isAdmin, Guid id, CancellationToken cancellationToken)
    {
        var entity = isAdmin
            ? await _repository.GetByIdAsync(id, asNoTracking: false, cancellationToken, includeUnmoderated: true)
            : await _repository.GetOwnedAsync(id, userId, cancellationToken);

        return entity ?? throw new NotFoundException("إعلان المحل مش موجود.");
    }

    private static void ApplyEditableFields(Shop entity, CreateShopRequest request)
    {
        var isSale = RealEstateListings.IsSale(request.ListingType);
        var isRent = RealEstateListings.IsRent(request.ListingType);
        var isExchange = RealEstateListings.IsExchange(request.ListingType);

        entity.Title = request.Title;
        entity.Description = request.Description;
        entity.AdvertiserName = request.AdvertiserName.Trim();

        entity.ListingType = request.ListingType;

        entity.SuitableActivity = request.SuitableActivity;
        entity.OtherSuitableActivity = RealEstateListings.Keep(
            request.SuitableActivity == ShopSuitableActivity.Other, request.OtherSuitableActivity);

        entity.Price = RealEstateListings.Keep(isSale, request.Price);
        entity.Area = request.Area;
        entity.FloorType = request.FloorType;
        entity.CeilingHeight = request.CeilingHeight;
        entity.FacadeWidth = request.FacadeWidth;
        entity.FacadesCount = request.FacadesCount;
        entity.FacadeDirection = request.FacadeDirection;
        entity.FinishingType = request.FinishingType;
        entity.PropertyAge = request.PropertyAge;

        entity.HasBathroom = request.HasBathroom;
        entity.BathroomsCount = RealEstateListings.Keep(request.HasBathroom == true, request.BathroomsCount);

        entity.HasStorage = request.HasStorage;
        entity.StorageArea = RealEstateListings.Keep(request.HasStorage == true, request.StorageArea);

        entity.HasGlassFacade = request.HasGlassFacade;
        entity.SuitableForRestaurantOrCafe = request.SuitableForRestaurantOrCafe;
        entity.HasExtractorFan = request.HasExtractorFan;
        entity.HasPrivateEntrance = request.HasPrivateEntrance;
        entity.EntrancesCount = request.EntrancesCount;

        entity.LegalStatus = request.LegalStatus;

        var isLicensed = request.LegalStatus == ShopLegalStatus.Licensed;
        entity.LicenseNumber = RealEstateListings.Keep(isLicensed, request.LicenseNumber);
        entity.LicenseType = RealEstateListings.Keep(isLicensed, request.LicenseType);
        entity.LicenseIssuer = RealEstateListings.Keep(isLicensed, request.LicenseIssuer);
        entity.LicenseIssueDate = RealEstateListings.Keep(isLicensed, request.LicenseIssueDate);
        entity.LicenseExpiryDate = RealEstateListings.Keep(isLicensed, request.LicenseExpiryDate);

        var isReconciliation = request.LegalStatus == ShopLegalStatus.Reconciliation;
        entity.ReconciliationForm = RealEstateListings.Keep(isReconciliation, request.ReconciliationForm);
        entity.OtherReconciliationForm = RealEstateListings.Keep(
            isReconciliation && request.ReconciliationForm == ShopReconciliationForm.Other,
            request.OtherReconciliationForm);

        entity.OwnershipDocument = request.OwnershipDocument;
        entity.OtherOwnershipDocument = RealEstateListings.Keep(
            request.OwnershipDocument == ShopOwnershipDocument.Other, request.OtherOwnershipDocument);
        entity.IsRegistered = request.IsRegistered;

        entity.WasPreviouslyOperating = request.WasPreviouslyOperating;

        var wasOperating = request.WasPreviouslyOperating == true;
        entity.PreviousActivity = RealEstateListings.Keep(wasOperating, request.PreviousActivity);
        entity.PreviousOperatingPeriod =
            RealEstateListings.Keep(wasOperating, request.PreviousOperatingPeriod);
        entity.VacancyReason = RealEstateListings.Keep(wasOperating, request.VacancyReason);

        entity.Center = request.Center;

        var allowsProject = RealEstateListings.AllowsProject(request.Center);
        entity.Project = RealEstateListings.Keep(allowsProject, request.Project);
        entity.OtherProject = RealEstateListings.Keep(
            allowsProject && request.Project == RealEstateProject.Other, request.OtherProject);

        entity.District = RealEstateListings.Trimmed(request.District);
        entity.Address = request.Address.Trim();
        entity.GoogleMaps = RealEstateListings.Trimmed(request.GoogleMaps);

        entity.Phone = request.Phone;
        entity.WhatsApp = RealEstateListings.Trimmed(request.WhatsApp);
        entity.Email = RealEstateListings.Trimmed(request.Email);

        entity.Negotiable = request.Negotiable ?? false;
        entity.Notes = RealEstateListings.Trimmed(request.Notes);

        entity.PaymentMethod = RealEstateListings.Keep(isSale, request.PaymentMethod);

        var isInstallments = isSale && request.PaymentMethod == ShopPaymentMethod.Installments;
        entity.DownPayment = RealEstateListings.Keep(isInstallments, request.DownPayment);
        entity.InstallmentPeriod = RealEstateListings.Keep(isInstallments, request.InstallmentPeriod);
        entity.InstallmentAmount = RealEstateListings.Keep(isInstallments, request.InstallmentAmount);
        entity.InstallmentProvider = RealEstateListings.Keep(isInstallments, request.InstallmentProvider);

        entity.RentType = RealEstateListings.Keep(isRent, request.RentType);
        entity.RentValue = RealEstateListings.Keep(isRent, request.RentValue);
        entity.SecurityDeposit = RealEstateListings.Keep(isRent, request.SecurityDeposit);
        entity.RentDownPayment = RealEstateListings.Keep(isRent, request.RentDownPayment);
        entity.MinimumRentPeriod = RealEstateListings.Keep(isRent, request.MinimumRentPeriod);
        entity.AvailableFrom = RealEstateListings.Keep(isRent, request.AvailableFrom);
        entity.AvailableTo = RealEstateListings.Keep(isRent, request.AvailableTo);
        entity.AllowsActivityChange = RealEstateListings.Keep(isRent, request.AllowsActivityChange);
        entity.OwnerConditions = RealEstateListings.Keep(isRent, request.OwnerConditions);

        entity.ExchangeWith = RealEstateListings.Keep(isExchange, request.ExchangeWith);
        entity.AcceptsDifferencePayment = RealEstateListings.Keep(isExchange, request.AcceptsDifferencePayment);
        entity.DifferenceAmount = RealEstateListings.Keep(
            isExchange && request.AcceptsDifferencePayment == true, request.DifferenceAmount);
        entity.ExchangeDetails = RealEstateListings.Keep(isExchange, request.ExchangeDetails);
    }

    private static List<ShopUtilitySelection> BuildUtilities(
        Guid listingId, IEnumerable<ShopUtility> utilities) =>
        utilities.Distinct()
            .Select(utility => new ShopUtilitySelection { ShopId = listingId, Utility = utility })
            .ToList();

    private static List<ShopRentInclusionSelection> BuildRentInclusions(
        Guid listingId, IEnumerable<ShopRentInclusion> inclusions) =>
        inclusions.Distinct()
            .Select(inclusion => new ShopRentInclusionSelection { ShopId = listingId, Inclusion = inclusion })
            .ToList();

    private static List<ShopRentSuitableActivitySelection> BuildRentSuitableActivities(
        Guid listingId, IEnumerable<ShopRentSuitableActivity> activities) =>
        activities.Distinct()
            .Select(activity => new ShopRentSuitableActivitySelection
            {
                ShopId = listingId,
                Activity = activity
            })
            .ToList();

    private async Task ReplaceUtilitiesAsync(Shop entity, IEnumerable<ShopUtility> requested)
    {
        var wanted = requested.Distinct().ToHashSet();

        var toRemove = entity.Utilities.Where(selection => !wanted.Contains(selection.Utility)).ToList();
        foreach (var selection in toRemove)
            entity.Utilities.Remove(selection);

        if (toRemove.Count > 0)
            _repository.RemoveUtilities(toRemove);

        var existing = entity.Utilities.Select(selection => selection.Utility).ToHashSet();
        var toAdd = BuildUtilities(entity.Id, wanted.Where(utility => !existing.Contains(utility)));

        if (toAdd.Count > 0)
        {
            await _repository.AddUtilitiesAsync(toAdd);
            foreach (var selection in toAdd)
                entity.Utilities.Add(selection);
        }
    }

    private async Task ReplaceRentInclusionsAsync(Shop entity, IEnumerable<ShopRentInclusion> requested)
    {
        var wanted = requested.Distinct().ToHashSet();

        var toRemove = entity.RentInclusions.Where(selection => !wanted.Contains(selection.Inclusion)).ToList();
        foreach (var selection in toRemove)
            entity.RentInclusions.Remove(selection);

        if (toRemove.Count > 0)
            _repository.RemoveRentInclusions(toRemove);

        var existing = entity.RentInclusions.Select(selection => selection.Inclusion).ToHashSet();
        var toAdd = BuildRentInclusions(entity.Id, wanted.Where(inclusion => !existing.Contains(inclusion)));

        if (toAdd.Count > 0)
        {
            await _repository.AddRentInclusionsAsync(toAdd);
            foreach (var selection in toAdd)
                entity.RentInclusions.Add(selection);
        }
    }

    private async Task ReplaceRentSuitableActivitiesAsync(
        Shop entity, IEnumerable<ShopRentSuitableActivity> requested)
    {
        var wanted = requested.Distinct().ToHashSet();

        var toRemove = entity.RentSuitableActivities
            .Where(selection => !wanted.Contains(selection.Activity))
            .ToList();

        foreach (var selection in toRemove)
            entity.RentSuitableActivities.Remove(selection);

        if (toRemove.Count > 0)
            _repository.RemoveRentSuitableActivities(toRemove);

        var existing = entity.RentSuitableActivities.Select(selection => selection.Activity).ToHashSet();
        var toAdd = BuildRentSuitableActivities(
            entity.Id, wanted.Where(activity => !existing.Contains(activity)));

        if (toAdd.Count > 0)
        {
            await _repository.AddRentSuitableActivitiesAsync(toAdd);
            foreach (var selection in toAdd)
                entity.RentSuitableActivities.Add(selection);
        }
    }

    private async Task<ShopDetailsDto> BuildDetailsAsync(
        Guid id, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        var entity = await _repository.GetByIdAsync(id, asNoTracking: true, cancellationToken, includeUnmoderated)
            ?? throw new NotFoundException("إعلان المحل مش موجود.");

        var details = _mapper.Map<ShopDetailsDto>(entity);

        details.ShareUrl = RealEstateListings.ShareUrl(_fileService, RealEstateRoutes.Shops, id);

        return details;
    }

    private Task<List<ShopImage>> AttachImagesAsync(
        Shop entity,
        IReadOnlyList<UploadImageModel> images,
        CancellationToken cancellationToken) =>
        ListingImages.AttachOrderedAsync(
            _fileService,
            entity.Images,
            images,
            ImageConstants.ShopFolder,
            _ => new ShopImage { ShopId = entity.Id },
            cancellationToken);
}
