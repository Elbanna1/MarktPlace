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

public class LandService : ILandService
{
    private readonly ILandRepository _repository;
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;
    private readonly INotificationService _notifications;

    public LandService(
        ILandRepository repository,
        IFileService fileService,
        IMapper mapper,
        INotificationService notifications)
    {
        _repository = repository;
        _fileService = fileService;
        _mapper = mapper;
        _notifications = notifications;
    }

    public async Task<LandDetailsDto> CreateAsync(
        string userId,
        CreateLandRequest request,
        IReadOnlyList<UploadImageModel> images,
        UploadImageModel? video,
        CancellationToken cancellationToken = default)
    {
        RealEstateListings.EnsureHasImages(images.Count);

        var entity = new Land
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Governorate = RealEstateListings.Governorate,
            CreatedAt = DateTime.UtcNow
        };

        ApplyEditableFields(entity, request);

        entity.Utilities = BuildUtilities(entity.Id, request.Utilities);
        entity.RentInclusions = BuildRentInclusions(
            entity.Id, RealEstateListings.Keep(RealEstateListings.IsRent(request.ListingType), request.RentInclusions));

        var stored = new List<string>();
        try
        {
            var added = await AttachImagesAsync(entity, images, cancellationToken);
            stored.AddRange(added.Select(image => image.ImagePath));

            if (video is not null)
            {
                var storedVideo = await _fileService.SaveVideoAsync(
                    video, FileUploadConstants.LandVideoFolder, cancellationToken);
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
            userId, ListingModuleType.Land, NotificationAction.Created, entity.Id, entity.Title);

        return await BuildDetailsAsync(entity.Id, cancellationToken, includeUnmoderated: true);
    }

    public async Task<LandDetailsDto> UpdateAsync(
        string userId,
        bool isAdmin,
        Guid id,
        UpdateLandRequest request,
        IReadOnlyList<UploadImageModel> newImages,
        UploadImageModel? video,
        CancellationToken cancellationToken = default)
    {
        var entity = await LoadForWriteAsync(userId, isAdmin, id, cancellationToken);

        ApplyEditableFields(entity, request);

        await ReplaceUtilitiesAsync(entity, request.Utilities);
        await ReplaceRentInclusionsAsync(
            entity,
            RealEstateListings.Keep(RealEstateListings.IsRent(request.ListingType), request.RentInclusions));

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
                video, FileUploadConstants.LandVideoFolder, cancellationToken);

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
            entity.UserId, ListingModuleType.Land,
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
            throw new NotFoundException("إعلان الأرض مش موجود.");

        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;

        _repository.Update(entity);
        await _repository.SaveChangesAsync();

        await _notifications.NotifyAsync(
            entity.UserId, ListingModuleType.Land,
            isAdmin && entity.UserId != userId ? NotificationAction.AdminDeleted : NotificationAction.Deleted,
            id, entity.Title);
    }

    public async Task<PaginatedResult<LandListItemDto>> GetListAsync(
        LandFilterParams filter, CancellationToken cancellationToken = default)
    {
        var (items, total) = await _repository.GetPagedAsync(filter, cancellationToken);
        var mapped = _mapper.Map<IReadOnlyList<LandListItemDto>>(items);
        return new PaginatedResult<LandListItemDto>(mapped, total, filter.PageIndex, filter.PageSize);
    }

    public async Task<LandDetailsDto> GetByIdAsync(
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

    public async Task<IReadOnlyList<LandListItemDto>> GetSimilarAsync(
        Guid id, int count, CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<LandListItemDto>>(
            await _repository.GetSimilarAsync(id, RealEstateListings.StripSize(count), cancellationToken));

    public async Task<IReadOnlyList<LandListItemDto>> GetRelatedAsync(
        Guid id, int count, CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<LandListItemDto>>(
            await _repository.GetRelatedAsync(id, RealEstateListings.StripSize(count), cancellationToken));

    public async Task<IReadOnlyList<LandListItemDto>> GetRecentlyAddedAsync(
        int count, CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<LandListItemDto>>(
            await _repository.GetRecentlyAddedAsync(RealEstateListings.StripSize(count), cancellationToken));

    public Task<RealEstatePriceStatisticsDto> GetPriceStatisticsAsync(
        LandFilterParams filter, CancellationToken cancellationToken = default) =>
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

    public async Task<LandDetailsDto> ReorderImagesAsync(
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

    public async Task<LandDetailsDto> SetPromotionAsync(
        Guid id, PromoteRealEstateRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, asNoTracking: false, cancellationToken, includeUnmoderated: true)
            ?? throw new NotFoundException("إعلان الأرض مش موجود.");

        entity.IsFeatured = request.IsFeatured;
        entity.IsPremium = request.IsPremium;
        entity.IsUrgent = request.IsUrgent;
        entity.UpdatedAt = DateTime.UtcNow;

        _repository.Update(entity);
        await _repository.SaveChangesAsync();

        await _notifications.NotifyAsync(
            entity.UserId, ListingModuleType.Land, NotificationAction.AdminUpdated, id, entity.Title);

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
            RealEstateLookupKeys.LandTypes =>
                _repository.GetLookupAsync<LandTypeLookup>(cancellationToken),
            RealEstateLookupKeys.AreaUnits =>
                _repository.GetLookupAsync<LandAreaUnitLookup>(cancellationToken),
            RealEstateLookupKeys.FacadesCounts =>
                _repository.GetLookupAsync<LandFacadesCountLookup>(cancellationToken),
            RealEstateLookupKeys.Directions =>
                _repository.GetLookupAsync<LandDirectionLookup>(cancellationToken),
            RealEstateLookupKeys.RoadTypes =>
                _repository.GetLookupAsync<LandRoadTypeLookup>(cancellationToken),
            RealEstateLookupKeys.LegalStatuses =>
                _repository.GetLookupAsync<LandLegalStatusLookup>(cancellationToken),
            RealEstateLookupKeys.ReconciliationForms =>
                _repository.GetLookupAsync<LandReconciliationFormLookup>(cancellationToken),
            RealEstateLookupKeys.OwnershipDocuments =>
                _repository.GetLookupAsync<LandOwnershipDocumentLookup>(cancellationToken),
            RealEstateLookupKeys.Utilities =>
                _repository.GetLookupAsync<LandUtilityLookup>(cancellationToken),
            RealEstateLookupKeys.RentTypes =>
                _repository.GetLookupAsync<LandRentTypeLookup>(cancellationToken),
            RealEstateLookupKeys.MinimumRentPeriods =>
                _repository.GetLookupAsync<LandMinimumRentPeriodLookup>(cancellationToken),
            RealEstateLookupKeys.RentInclusions =>
                _repository.GetLookupAsync<LandRentInclusionLookup>(cancellationToken),
            RealEstateLookupKeys.ContractDurations =>
                _repository.GetLookupAsync<LandContractDurationLookup>(cancellationToken),
            RealEstateLookupKeys.ExchangeTargets =>
                _repository.GetLookupAsync<LandExchangeWithLookup>(cancellationToken),
            RealEstateLookupKeys.HarvestSeasons =>
                _repository.GetLookupAsync<LandHarvestSeasonLookup>(cancellationToken),
            RealEstateLookupKeys.SoilTypes =>
                _repository.GetLookupAsync<LandSoilTypeLookup>(cancellationToken),
            RealEstateLookupKeys.IrrigationSources =>
                _repository.GetLookupAsync<LandIrrigationSourceLookup>(cancellationToken),
            RealEstateLookupKeys.QualityCertificates =>
                _repository.GetLookupAsync<LandQualityCertificateLookup>(cancellationToken),
            RealEstateLookupKeys.ExistingBuildingTypes =>
                _repository.GetLookupAsync<LandExistingBuildingTypeLookup>(cancellationToken),
            RealEstateLookupKeys.BuildingCompletionRatios =>
                _repository.GetLookupAsync<LandBuildingCompletionRatioLookup>(cancellationToken),

            _ => throw new NotFoundException($"القائمة '{lookupKey}' مش معروفة.")
        };

    private async Task<Land> LoadForWriteAsync(
        string userId, bool isAdmin, Guid id, CancellationToken cancellationToken)
    {
        var entity = isAdmin
            ? await _repository.GetByIdAsync(id, asNoTracking: false, cancellationToken, includeUnmoderated: true)
            : await _repository.GetOwnedAsync(id, userId, cancellationToken);

        return entity ?? throw new NotFoundException("إعلان الأرض مش موجود.");
    }

    private static void ApplyEditableFields(Land entity, CreateLandRequest request)
    {
        var isSale = RealEstateListings.IsSale(request.ListingType);
        var isRent = RealEstateListings.IsRent(request.ListingType);
        var isExchange = RealEstateListings.IsExchange(request.ListingType);
        var isAgricultural = LandTypeGroups.IsAgricultural(request.LandType);
        var isBuildingLand = LandTypeGroups.IsBuilding(request.LandType);

        entity.Title = request.Title;
        entity.Description = request.Description;
        entity.AdvertiserName = request.AdvertiserName.Trim();

        entity.ListingType = request.ListingType;

        entity.LandType = request.LandType;
        entity.OtherLandType = RealEstateListings.Keep(
            request.LandType == LandType.Other, request.OtherLandType);
        entity.AreaUnit = request.AreaUnit;
        entity.Area = request.Area;

        entity.PricePerMeter = RealEstateListings.Keep(isSale, request.PricePerMeter);
        entity.TotalPrice = RealEstateListings.Keep(isSale, request.TotalPrice);
        entity.Length = request.Length;
        entity.Width = request.Width;
        entity.FacadeLength = request.FacadeLength;
        entity.FacadesCount = request.FacadesCount;
        entity.Direction = request.Direction;
        entity.StreetWidth = request.StreetWidth;
        entity.RoadType = request.RoadType;
        entity.InsideBuildingCordon = request.InsideBuildingCordon;
        entity.IsBuildable = request.IsBuildable;

        var isBuildable = request.IsBuildable == true;
        entity.AllowedBuildingRatio = RealEstateListings.Keep(isBuildable, request.AllowedBuildingRatio);
        entity.AllowedFloorsCount = RealEstateListings.Keep(isBuildable, request.AllowedFloorsCount);

        entity.LegalStatus = request.LegalStatus;

        var isLicensed = request.LegalStatus == LandLegalStatus.Licensed;
        entity.LicenseNumber = RealEstateListings.Keep(isLicensed, request.LicenseNumber);
        entity.LicenseIssueDate = RealEstateListings.Keep(isLicensed, request.LicenseIssueDate);
        entity.LicenseExpiryDate = RealEstateListings.Keep(isLicensed, request.LicenseExpiryDate);
        entity.LicenseIssuer = RealEstateListings.Keep(isLicensed, request.LicenseIssuer);

        var isReconciliation = request.LegalStatus == LandLegalStatus.Reconciliation;
        entity.ReconciliationForm = RealEstateListings.Keep(isReconciliation, request.ReconciliationForm);
        entity.OtherReconciliationForm = RealEstateListings.Keep(
            isReconciliation && request.ReconciliationForm == LandReconciliationForm.Other,
            request.OtherReconciliationForm);

        entity.OwnershipDocument = request.OwnershipDocument;
        entity.OtherOwnershipDocument = RealEstateListings.Keep(
            request.OwnershipDocument == LandOwnershipDocument.Other, request.OtherOwnershipDocument);
        entity.HasSurveyPlan = request.HasSurveyPlan;
        entity.HasViolations = request.HasViolations;
        entity.ViolationDetails = RealEstateListings.Keep(
            request.HasViolations == true, request.ViolationDetails);

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

        entity.RentType = RealEstateListings.Keep(isRent, request.RentType);
        entity.RentValue = RealEstateListings.Keep(isRent, request.RentValue);
        entity.SecurityDeposit = RealEstateListings.Keep(isRent, request.SecurityDeposit);
        entity.DownPayment = RealEstateListings.Keep(isRent, request.DownPayment);
        entity.MinimumRentPeriod = RealEstateListings.Keep(isRent, request.MinimumRentPeriod);
        entity.AvailableFrom = RealEstateListings.Keep(isRent, request.AvailableFrom);
        entity.AvailableTo = RealEstateListings.Keep(isRent, request.AvailableTo);
        entity.ContractDuration = RealEstateListings.Keep(isRent, request.ContractDuration);
        entity.OwnerConditions = RealEstateListings.Keep(isRent, request.OwnerConditions);

        entity.ExchangeWith = RealEstateListings.Keep(isExchange, request.ExchangeWith);
        entity.OtherExchangeWith = RealEstateListings.Keep(
            isExchange && request.ExchangeWith == LandExchangeWith.Other, request.OtherExchangeWith);
        entity.AcceptsDifferencePayment = RealEstateListings.Keep(isExchange, request.AcceptsDifferencePayment);
        entity.DifferenceAmount = RealEstateListings.Keep(
            isExchange && request.AcceptsDifferencePayment == true, request.DifferenceAmount);
        entity.ExchangeInSameGovernorateOnly =
            RealEstateListings.Keep(isExchange, request.ExchangeInSameGovernorateOnly);
        entity.ExchangeDetails = RealEstateListings.Keep(isExchange, request.ExchangeDetails);

        entity.IsCurrentlyCultivated = RealEstateListings.Keep(isAgricultural, request.IsCurrentlyCultivated);

        var isCultivated = isAgricultural && request.IsCurrentlyCultivated == true;
        entity.CurrentCropType = RealEstateListings.Keep(isCultivated, request.CurrentCropType);
        entity.CultivatedFeddans = RealEstateListings.Keep(isCultivated, request.CultivatedFeddans);

        entity.HarvestSeason = RealEstateListings.Keep(isAgricultural, request.HarvestSeason);
        entity.SoilType = RealEstateListings.Keep(isAgricultural, request.SoilType);
        entity.IrrigationSource = RealEstateListings.Keep(isAgricultural, request.IrrigationSource);
        entity.OtherIrrigationSource = RealEstateListings.Keep(
            isAgricultural && request.IrrigationSource == LandIrrigationSource.Other,
            request.OtherIrrigationSource);
        entity.HasWell = RealEstateListings.Keep(isAgricultural, request.HasWell);
        entity.HasIrrigationMachine = RealEstateListings.Keep(isAgricultural, request.HasIrrigationMachine);
        entity.HasIrrigationNetwork = RealEstateListings.Keep(isAgricultural, request.HasIrrigationNetwork);
        entity.HasTrees = RealEstateListings.Keep(isAgricultural, request.HasTrees);

        var hasTrees = isAgricultural && request.HasTrees == true;
        entity.TreeType = RealEstateListings.Keep(hasTrees, request.TreeType);
        entity.TreesCount = RealEstateListings.Keep(hasTrees, request.TreesCount);
        entity.TreesAge = RealEstateListings.Keep(hasTrees, request.TreesAge);

        entity.HasFarmHouse = RealEstateListings.Keep(isAgricultural, request.HasFarmHouse);
        entity.HasRestHouse = RealEstateListings.Keep(isAgricultural, request.HasRestHouse);
        entity.HasStorage = RealEstateListings.Keep(isAgricultural, request.HasStorage);
        entity.HasResidentWorkers = RealEstateListings.Keep(isAgricultural, request.HasResidentWorkers);
        entity.IsOrganic = RealEstateListings.Keep(isAgricultural, request.IsOrganic);
        entity.UsesChemicalFertilizers =
            RealEstateListings.Keep(isAgricultural, request.UsesChemicalFertilizers);
        entity.HasQualityCertificate = RealEstateListings.Keep(isAgricultural, request.HasQualityCertificate);
        entity.QualityCertificate = RealEstateListings.Keep(
            isAgricultural && request.HasQualityCertificate == true, request.QualityCertificate);

        entity.HasFence = RealEstateListings.Keep(isAgricultural || isBuildingLand, request.HasFence);

        entity.HasGate = RealEstateListings.Keep(isBuildingLand, request.HasGate);
        entity.IsLeveledForBuilding = RealEstateListings.Keep(isBuildingLand, request.IsLeveledForBuilding);
        entity.HasFoundations = RealEstateListings.Keep(isBuildingLand, request.HasFoundations);
        entity.HasExistingBuilding = RealEstateListings.Keep(isBuildingLand, request.HasExistingBuilding);

        var hasExistingBuilding = isBuildingLand && request.HasExistingBuilding == true;
        entity.ExistingBuildingType =
            RealEstateListings.Keep(hasExistingBuilding, request.ExistingBuildingType);
        entity.BuildingCompletionRatio =
            RealEstateListings.Keep(hasExistingBuilding, request.BuildingCompletionRatio);
        entity.CurrentFloorsCount = RealEstateListings.Keep(hasExistingBuilding, request.CurrentFloorsCount);
        entity.CanAddFloors = RealEstateListings.Keep(hasExistingBuilding, request.CanAddFloors);
    }

    private static List<LandUtilitySelection> BuildUtilities(
        Guid listingId, IEnumerable<LandUtility> utilities) =>
        utilities.Distinct()
            .Select(utility => new LandUtilitySelection { LandId = listingId, Utility = utility })
            .ToList();

    private static List<LandRentInclusionSelection> BuildRentInclusions(
        Guid listingId, IEnumerable<LandRentInclusion> inclusions) =>
        inclusions.Distinct()
            .Select(inclusion => new LandRentInclusionSelection { LandId = listingId, Inclusion = inclusion })
            .ToList();

    private async Task ReplaceUtilitiesAsync(Land entity, IEnumerable<LandUtility> requested)
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

    private async Task ReplaceRentInclusionsAsync(Land entity, IEnumerable<LandRentInclusion> requested)
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

    private async Task<LandDetailsDto> BuildDetailsAsync(
        Guid id, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        var entity = await _repository.GetByIdAsync(id, asNoTracking: true, cancellationToken, includeUnmoderated)
            ?? throw new NotFoundException("إعلان الأرض مش موجود.");

        var details = _mapper.Map<LandDetailsDto>(entity);

        details.ShareUrl = RealEstateListings.ShareUrl(_fileService, RealEstateRoutes.Lands, id);

        return details;
    }

    private Task<List<LandImage>> AttachImagesAsync(
        Land entity,
        IReadOnlyList<UploadImageModel> images,
        CancellationToken cancellationToken) =>
        ListingImages.AttachOrderedAsync(
            _fileService,
            entity.Images,
            images,
            ImageConstants.LandFolder,
            _ => new LandImage { LandId = entity.Id },
            cancellationToken);
}
