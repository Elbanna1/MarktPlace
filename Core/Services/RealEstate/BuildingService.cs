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

public class BuildingService : IBuildingService
{
    private const string NotFoundMessage = "إعلان المبنى مش موجود.";

    private readonly IBuildingRepository _repository;
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;
    private readonly INotificationService _notifications;

    public BuildingService(
        IBuildingRepository repository,
        IFileService fileService,
        IMapper mapper,
        INotificationService notifications)
    {
        _repository = repository;
        _fileService = fileService;
        _mapper = mapper;
        _notifications = notifications;
    }

    public async Task<BuildingDetailsDto> CreateAsync(
        string userId,
        CreateBuildingRequest request,
        IReadOnlyList<UploadImageModel> images,
        UploadImageModel? video,
        CancellationToken cancellationToken = default)
    {
        RealEstateListings.EnsureHasImages(images.Count);

        var entity = new Building
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Governorate = RealEstateListings.Governorate,
            CreatedAt = DateTime.UtcNow
        };

        ApplyEditableFields(entity, request);

        entity.Features = BuildFeatures(entity.Id, RequestedFeatures(request));
        entity.RentInclusions = BuildRentInclusions(
            entity.Id,
            RealEstateListings.Keep(RealEstateListings.IsRent(request.ListingType), request.RentInclusions));

        var stored = new List<string>();
        try
        {
            var added = await AttachImagesAsync(entity, images, cancellationToken);
            stored.AddRange(added.Select(image => image.ImagePath));

            if (video is not null)
            {
                var storedVideo = await _fileService.SaveVideoAsync(
                    video, FileUploadConstants.BuildingVideoFolder, cancellationToken);
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
            userId, ListingModuleType.Building, NotificationAction.Created, entity.Id, entity.Title);

        return await BuildDetailsAsync(entity.Id, cancellationToken, includeUnmoderated: true);
    }

    public async Task<BuildingDetailsDto> UpdateAsync(
        string userId,
        bool isAdmin,
        Guid id,
        UpdateBuildingRequest request,
        IReadOnlyList<UploadImageModel> newImages,
        UploadImageModel? video,
        CancellationToken cancellationToken = default)
    {
        var entity = await LoadForWriteAsync(userId, isAdmin, id, cancellationToken);

        ApplyEditableFields(entity, request);

        await ReplaceFeaturesAsync(entity, RequestedFeatures(request));
        await ReplaceRentInclusionsAsync(
            entity,
            RealEstateListings.Keep(RealEstateListings.IsRent(request.ListingType), request.RentInclusions));

        var removedImagePaths = new List<string>();

        if (request.RemoveImageIds.Count > 0)
        {
            var toRemove = entity.Images
                .Where(image => request.RemoveImageIds.Contains(image.Id))
                .ToList();

            foreach (var image in toRemove)
            {
                removedImagePaths.Add(image.ImagePath);
                _repository.RemoveImage(image);
                entity.Images.Remove(image);
            }
        }

        var addedImages = new List<BuildingImage>();

        if (newImages.Count > 0)
        {
            addedImages = await AttachImagesAsync(entity, newImages, cancellationToken);
            await _repository.AddImagesAsync(addedImages);
        }

        RealEstateListings.EnsureHasImages(entity.Images.Count);
        ListingImages.NormalizeOrder(entity.Images);

        string? obsoleteVideoPath = null;
        string? newVideoPath = null;

        if (video is not null)
        {
            var storedVideo = await _fileService.SaveVideoAsync(
                video, FileUploadConstants.BuildingVideoFolder, cancellationToken);

            newVideoPath = storedVideo.RelativePath;
            obsoleteVideoPath = entity.VideoPath;
            entity.VideoPath = storedVideo.RelativePath;
            entity.VideoUrl = storedVideo.Url;
        }
        else if (request.RemoveVideo && !string.IsNullOrWhiteSpace(entity.VideoPath))
        {
            obsoleteVideoPath = entity.VideoPath;
            entity.VideoPath = null;
            entity.VideoUrl = null;
        }

        entity.UpdatedAt = DateTime.UtcNow;

        try
        {
            _repository.Update(entity);
            await _repository.SaveChangesAsync();
        }
        catch
        {
            foreach (var image in addedImages)
                _fileService.Delete(image.ImagePath);

            if (newVideoPath is not null)
                _fileService.Delete(newVideoPath);

            throw;
        }

        foreach (var path in removedImagePaths)
            _fileService.Delete(path);

        if (!string.IsNullOrWhiteSpace(obsoleteVideoPath))
            _fileService.Delete(obsoleteVideoPath);

        await _notifications.NotifyAsync(
            entity.UserId, ListingModuleType.Building,
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
            throw new NotFoundException(NotFoundMessage);

        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;

        _repository.Update(entity);
        await _repository.SaveChangesAsync();

        await _notifications.NotifyAsync(
            entity.UserId, ListingModuleType.Building,
            isAdmin && entity.UserId != userId ? NotificationAction.AdminDeleted : NotificationAction.Deleted,
            id, entity.Title);
    }

    public async Task<PaginatedResult<BuildingListItemDto>> GetListAsync(
        BuildingFilterParams filter, CancellationToken cancellationToken = default)
    {
        var (items, total) = await _repository.GetPagedAsync(filter, cancellationToken);
        var mapped = _mapper.Map<IReadOnlyList<BuildingListItemDto>>(items);
        return new PaginatedResult<BuildingListItemDto>(mapped, total, filter.PageIndex, filter.PageSize);
    }

    public async Task<BuildingDetailsDto> GetByIdAsync(
        Guid id, bool countView = false, CancellationToken cancellationToken = default)
    {
        var details = await BuildDetailsAsync(id, cancellationToken);

        if (countView)
            details.ViewCount++;

        return details;
    }

    public async Task<IReadOnlyList<BuildingListItemDto>> GetSimilarAsync(
        Guid id, int count, CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<BuildingListItemDto>>(
            await _repository.GetSimilarAsync(id, RealEstateListings.StripSize(count), cancellationToken));

    public async Task<IReadOnlyList<BuildingListItemDto>> GetRelatedAsync(
        Guid id, int count, CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<BuildingListItemDto>>(
            await _repository.GetRelatedAsync(id, RealEstateListings.StripSize(count), cancellationToken));

    public async Task<IReadOnlyList<BuildingListItemDto>> GetRecentlyAddedAsync(
        int count, CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<BuildingListItemDto>>(
            await _repository.GetRecentlyAddedAsync(RealEstateListings.StripSize(count), cancellationToken));

    public Task<RealEstatePriceStatisticsDto> GetPriceStatisticsAsync(
        BuildingFilterParams filter, CancellationToken cancellationToken = default) =>
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

    public async Task<BuildingDetailsDto> ReorderImagesAsync(
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

    public async Task<BuildingDetailsDto> SetPromotionAsync(
        Guid id, PromoteRealEstateRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, asNoTracking: false, cancellationToken, includeUnmoderated: true)
            ?? throw new NotFoundException(NotFoundMessage);

        entity.IsFeatured = request.IsFeatured;
        entity.IsPremium = request.IsPremium;
        entity.IsUrgent = request.IsUrgent;
        entity.UpdatedAt = DateTime.UtcNow;

        _repository.Update(entity);
        await _repository.SaveChangesAsync();

        await _notifications.NotifyAsync(
            entity.UserId, ListingModuleType.Building, NotificationAction.AdminUpdated, id, entity.Title);

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
            RealEstateLookupKeys.BuildingTypes =>
                _repository.GetLookupAsync<BuildingTypeLookup>(cancellationToken),
            RealEstateLookupKeys.ReceptionPieces =>
                _repository.GetLookupAsync<ApartmentReceptionPiecesLookup>(cancellationToken),
            RealEstateLookupKeys.FurnishedStatuses =>
                _repository.GetLookupAsync<ApartmentFurnishedStatusLookup>(cancellationToken),
            RealEstateLookupKeys.FinishingTypes =>
                _repository.GetLookupAsync<ApartmentFinishingTypeLookup>(cancellationToken),
            RealEstateLookupKeys.PropertyAges =>
                _repository.GetLookupAsync<ApartmentPropertyAgeLookup>(cancellationToken),
            RealEstateLookupKeys.Directions =>
                _repository.GetLookupAsync<ApartmentDirectionLookup>(cancellationToken),
            RealEstateLookupKeys.ViewTypes =>
                _repository.GetLookupAsync<ApartmentViewTypeLookup>(cancellationToken),
            RealEstateLookupKeys.LegalStatuses =>
                _repository.GetLookupAsync<ApartmentLegalStatusLookup>(cancellationToken),
            RealEstateLookupKeys.ReconciliationForms =>
                _repository.GetLookupAsync<ApartmentReconciliationFormLookup>(cancellationToken),
            RealEstateLookupKeys.OwnershipDocuments =>
                _repository.GetLookupAsync<ApartmentOwnershipDocumentLookup>(cancellationToken),
            RealEstateLookupKeys.Features =>
                _repository.GetLookupAsync<ApartmentFeatureLookup>(cancellationToken),
            RealEstateLookupKeys.PaymentMethods =>
                _repository.GetLookupAsync<ApartmentPaymentMethodLookup>(cancellationToken),
            RealEstateLookupKeys.InstallmentProviders =>
                _repository.GetLookupAsync<ApartmentInstallmentProviderLookup>(cancellationToken),
            RealEstateLookupKeys.RentTypes =>
                _repository.GetLookupAsync<ApartmentRentTypeLookup>(cancellationToken),
            RealEstateLookupKeys.RentInclusions =>
                _repository.GetLookupAsync<ApartmentRentInclusionLookup>(cancellationToken),
            RealEstateLookupKeys.ExchangeTargets =>
                _repository.GetLookupAsync<ApartmentExchangeWithLookup>(cancellationToken),

            _ => throw new NotFoundException($"القائمة '{lookupKey}' مش معروفة.")
        };

    private async Task<Building> LoadForWriteAsync(
        string userId, bool isAdmin, Guid id, CancellationToken cancellationToken)
    {
        var entity = isAdmin
            ? await _repository.GetByIdAsync(id, asNoTracking: false, cancellationToken, includeUnmoderated: true)
            : await _repository.GetOwnedAsync(id, userId, cancellationToken);

        return entity ?? throw new NotFoundException(NotFoundMessage);
    }

    public static void ApplyEditableFields(Building entity, CreateBuildingRequest request)
    {
        var isSale = RealEstateListings.IsSale(request.ListingType);
        var isRent = RealEstateListings.IsRent(request.ListingType);
        var isExchange = RealEstateListings.IsExchange(request.ListingType);

        var isVilla = BuildingCatalog.IsVilla(request.BuildingType);
        var isHouse = BuildingCatalog.IsHouse(request.BuildingType);
        var isApartmentBuilding = BuildingCatalog.IsApartmentBuilding(request.BuildingType);

        entity.Title = request.Title.Trim();
        entity.Description = request.Description.Trim();
        entity.AdvertiserName = request.AdvertiserName.Trim();

        entity.ListingType = request.ListingType;
        entity.BuildingType = request.BuildingType!.Value;

        entity.Price = RealEstateListings.Keep(isSale, request.Price);
        entity.PricePerMeter = RealEstateListings.Keep(isSale, request.PricePerMeter);
        entity.Area = request.Area!.Value;

        entity.FurnishedStatus = request.FurnishedStatus!.Value;
        entity.FinishingType = request.FinishingType!.Value;
        entity.PropertyAge = request.PropertyAge!.Value;
        entity.Direction = request.Direction!.Value;
        entity.ViewType = request.ViewType;

        entity.FloorsCount = request.FloorsCount;

        entity.GardenArea = RealEstateListings.Keep(isVilla, request.GardenArea);
        entity.BathroomsCount = RealEstateListings.Keep(isVilla, request.BathroomsCount);
        entity.ReceptionPieces = RealEstateListings.Keep(isVilla, request.ReceptionPieces);
        entity.RoomsCount = RealEstateListings.Keep(isVilla, request.RoomsCount);
        entity.HasSwimmingPool = RealEstateListings.Keep(isVilla, request.HasSwimmingPool);

        entity.ResidentialUnitsCount = RealEstateListings.Keep(isHouse, request.ResidentialUnitsCount);
        entity.HasIndependentEntrance = RealEstateListings.Keep(isHouse, request.HasIndependentEntrance);

        entity.ApartmentsCount = RealEstateListings.Keep(isApartmentBuilding, request.ApartmentsCount);
        entity.ShopsCount = RealEstateListings.Keep(isApartmentBuilding, request.ShopsCount);
        entity.HasElevator = RealEstateListings.Keep(isApartmentBuilding, request.HasElevator);
        entity.HasGarage = RealEstateListings.Keep(isApartmentBuilding, request.HasGarage);

        entity.LegalStatus = request.LegalStatus!.Value;

        var isLicensed = request.LegalStatus == ApartmentLegalStatus.Licensed;
        entity.LicenseNumber = RealEstateListings.Keep(isLicensed, request.LicenseNumber);
        entity.LicenseIssueDate = RealEstateListings.Keep(isLicensed, request.LicenseIssueDate);
        entity.LicenseExpiryDate = RealEstateListings.Keep(isLicensed, request.LicenseExpiryDate);
        entity.LicenseIssuer = RealEstateListings.Keep(isLicensed, request.LicenseIssuer);

        entity.ReconciliationForm = RealEstateListings.Keep(
            request.LegalStatus == ApartmentLegalStatus.Reconciliation, request.ReconciliationForm);

        entity.OwnershipDocument = request.OwnershipDocument!.Value;
        entity.IsRegistered = request.IsRegistered;
        entity.HasViolations = request.HasViolations;
        entity.ViolationDetails = RealEstateListings.Keep(
            request.HasViolations == true, request.ViolationDetails);

        entity.Center = request.Center.Trim();

        var allowsProject = RealEstateListings.AllowsProject(request.Center);
        entity.Project = RealEstateListings.Keep(allowsProject, request.Project);
        entity.OtherProject = RealEstateListings.Keep(
            allowsProject && request.Project == RealEstateProject.Other, request.OtherProject);

        entity.District = RealEstateListings.Trimmed(request.District);
        entity.Address = request.Address.Trim();
        entity.GoogleMaps = RealEstateListings.Trimmed(request.GoogleMaps);

        entity.Phone = request.Phone.Trim();
        entity.WhatsApp = RealEstateListings.Trimmed(request.WhatsApp);
        entity.Email = RealEstateListings.Trimmed(request.Email);

        entity.Negotiable = request.Negotiable ?? false;
        entity.Notes = RealEstateListings.Trimmed(request.Notes);

        entity.PaymentMethod = RealEstateListings.Keep(isSale, request.PaymentMethod);

        var isInstallments = isSale && request.PaymentMethod == ApartmentPaymentMethod.Installments;
        entity.DownPayment = RealEstateListings.Keep(isInstallments, request.DownPayment);
        entity.InstallmentAmount = RealEstateListings.Keep(isInstallments, request.InstallmentAmount);
        entity.InstallmentPeriod = RealEstateListings.Keep(isInstallments, request.InstallmentPeriod);
        entity.InstallmentProvider = RealEstateListings.Keep(isInstallments, request.InstallmentProvider);

        entity.HasMaintenanceDeposit = RealEstateListings.Keep(isSale, request.HasMaintenanceDeposit);
        entity.MaintenanceDepositAmount = RealEstateListings.Keep(
            isSale && request.HasMaintenanceDeposit == true, request.MaintenanceDepositAmount);
        entity.MonthlyFees = RealEstateListings.Keep(isSale, request.MonthlyFees);

        entity.RentType = RealEstateListings.Keep(isRent, request.RentType);
        entity.RentValue = RealEstateListings.Keep(isRent, request.RentValue);
        entity.SecurityDeposit = RealEstateListings.Keep(isRent, request.SecurityDeposit);
        entity.RentDownPayment = RealEstateListings.Keep(isRent, request.RentDownPayment);
        entity.MinimumRentPeriod = RealEstateListings.Keep(isRent, request.MinimumRentPeriod);
        entity.AvailableFrom = RealEstateListings.Keep(isRent, request.AvailableFrom);
        entity.AvailableTo = RealEstateListings.Keep(isRent, request.AvailableTo);
        entity.OwnerConditions = RealEstateListings.Keep(isRent, request.OwnerConditions);

        entity.ExchangeWith = RealEstateListings.Keep(isExchange, request.ExchangeWith);
        entity.AcceptsDifferencePayment = RealEstateListings.Keep(isExchange, request.AcceptsDifferencePayment);
        entity.DifferenceAmount = RealEstateListings.Keep(
            isExchange && request.AcceptsDifferencePayment == true, request.DifferenceAmount);
        entity.ExchangeDetails = RealEstateListings.Keep(isExchange, request.ExchangeDetails);
    }

    public static IEnumerable<ApartmentFeature> RequestedFeatures(CreateBuildingRequest request) =>
        request.Features
            .Concat(BuildingCatalog.ImpliedFeatures(
                request.BuildingType, request.HasSwimmingPool, request.HasIndependentEntrance,
                request.HasElevator, request.HasGarage))
            .Distinct();

    private static List<BuildingFeatureSelection> BuildFeatures(
        Guid listingId, IEnumerable<ApartmentFeature> features) =>
        features.Distinct()
            .Select(feature => new BuildingFeatureSelection { BuildingId = listingId, Feature = feature })
            .ToList();

    private static List<BuildingRentInclusionSelection> BuildRentInclusions(
        Guid listingId, IEnumerable<ApartmentRentInclusion> inclusions) =>
        inclusions.Distinct()
            .Select(inclusion => new BuildingRentInclusionSelection
            {
                BuildingId = listingId,
                Inclusion = inclusion
            })
            .ToList();

    private async Task ReplaceFeaturesAsync(Building entity, IEnumerable<ApartmentFeature> requested)
    {
        var wanted = requested.Distinct().ToHashSet();

        var toRemove = entity.Features.Where(selection => !wanted.Contains(selection.Feature)).ToList();
        foreach (var selection in toRemove)
            entity.Features.Remove(selection);

        if (toRemove.Count > 0)
            _repository.RemoveFeatures(toRemove);

        var existing = entity.Features.Select(selection => selection.Feature).ToHashSet();
        var toAdd = BuildFeatures(entity.Id, wanted.Where(feature => !existing.Contains(feature)));

        if (toAdd.Count > 0)
        {
            await _repository.AddFeaturesAsync(toAdd);
            foreach (var selection in toAdd)
                entity.Features.Add(selection);
        }
    }

    private async Task ReplaceRentInclusionsAsync(
        Building entity, IEnumerable<ApartmentRentInclusion> requested)
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

    private async Task<BuildingDetailsDto> BuildDetailsAsync(
        Guid id, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        var entity = await _repository.GetByIdAsync(id, asNoTracking: true, cancellationToken, includeUnmoderated)
            ?? throw new NotFoundException(NotFoundMessage);

        var details = _mapper.Map<BuildingDetailsDto>(entity);

        details.ShareUrl = RealEstateListings.ShareUrl(_fileService, RealEstateRoutes.Buildings, id);

        return details;
    }

    private Task<List<BuildingImage>> AttachImagesAsync(
        Building entity,
        IReadOnlyList<UploadImageModel> images,
        CancellationToken cancellationToken) =>
        ListingImages.AttachOrderedAsync(
            _fileService,
            entity.Images,
            images,
            ImageConstants.BuildingFolder,
            _ => new BuildingImage { BuildingId = entity.Id },
            cancellationToken);
}
