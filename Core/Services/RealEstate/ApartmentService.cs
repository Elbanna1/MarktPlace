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

public class ApartmentService : IApartmentService
{
    private readonly IApartmentRepository _repository;
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;
    private readonly INotificationService _notifications;

    public ApartmentService(
        IApartmentRepository repository,
        IFileService fileService,
        IMapper mapper,
        INotificationService notifications)
    {
        _repository = repository;
        _fileService = fileService;
        _mapper = mapper;
        _notifications = notifications;
    }

    public async Task<ApartmentDetailsDto> CreateAsync(
        string userId,
        CreateApartmentRequest request,
        IReadOnlyList<UploadImageModel> images,
        UploadImageModel? video,
        CancellationToken cancellationToken = default)
    {
        RealEstateListings.EnsureHasImages(images.Count);

        var entity = new Apartment
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Governorate = RealEstateListings.Governorate,
            CreatedAt = DateTime.UtcNow
        };

        ApplyEditableFields(entity, request);

        entity.Features = BuildFeatures(entity.Id, request.Features);
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
                    video, FileUploadConstants.ApartmentVideoFolder, cancellationToken);
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
            userId, ListingModuleType.Apartment, NotificationAction.Created, entity.Id, entity.Title);

        return await BuildDetailsAsync(entity.Id, cancellationToken, includeUnmoderated: true);
    }

    public async Task<ApartmentDetailsDto> UpdateAsync(
        string userId,
        bool isAdmin,
        Guid id,
        UpdateApartmentRequest request,
        IReadOnlyList<UploadImageModel> newImages,
        UploadImageModel? video,
        CancellationToken cancellationToken = default)
    {
        var entity = await LoadForWriteAsync(userId, isAdmin, id, cancellationToken);

        ApplyEditableFields(entity, request);

        await ReplaceFeaturesAsync(entity, request.Features);
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
                video, FileUploadConstants.ApartmentVideoFolder, cancellationToken);

            replacedVideoPath = entity.VideoPath;
            entity.VideoPath = storedVideo.RelativePath;
            entity.VideoUrl = storedVideo.Url;
        }
        else if (request.RemoveVideo && !string.IsNullOrWhiteSpace(entity.VideoPath))
        {
            throw new BadRequestException("فيديو الإعلان مطلوب. من فضلك ارفع فيديو بديل بدلًا من حذفه.");
        }

        entity.UpdatedAt = DateTime.UtcNow;

        _repository.Update(entity);
        await _repository.SaveChangesAsync();

        if (!string.IsNullOrWhiteSpace(replacedVideoPath))
            _fileService.Delete(replacedVideoPath);

        await _notifications.NotifyAsync(
            entity.UserId, ListingModuleType.Apartment,
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
            throw new NotFoundException("إعلان الشقة مش موجود.");

        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;

        _repository.Update(entity);
        await _repository.SaveChangesAsync();

        await _notifications.NotifyAsync(
            entity.UserId, ListingModuleType.Apartment,
            isAdmin && entity.UserId != userId ? NotificationAction.AdminDeleted : NotificationAction.Deleted,
            id, entity.Title);
    }

    public async Task<PaginatedResult<ApartmentListItemDto>> GetListAsync(
        ApartmentFilterParams filter, CancellationToken cancellationToken = default)
    {
        var (items, total) = await _repository.GetPagedAsync(filter, cancellationToken);
        var mapped = _mapper.Map<IReadOnlyList<ApartmentListItemDto>>(items);
        return new PaginatedResult<ApartmentListItemDto>(mapped, total, filter.PageIndex, filter.PageSize);
    }

    public async Task<ApartmentDetailsDto> GetByIdAsync(
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

    public async Task<IReadOnlyList<ApartmentListItemDto>> GetSimilarAsync(
        Guid id, int count, CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<ApartmentListItemDto>>(
            await _repository.GetSimilarAsync(id, RealEstateListings.StripSize(count), cancellationToken));

    public async Task<IReadOnlyList<ApartmentListItemDto>> GetRelatedAsync(
        Guid id, int count, CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<ApartmentListItemDto>>(
            await _repository.GetRelatedAsync(id, RealEstateListings.StripSize(count), cancellationToken));

    public async Task<IReadOnlyList<ApartmentListItemDto>> GetRecentlyAddedAsync(
        int count, CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<ApartmentListItemDto>>(
            await _repository.GetRecentlyAddedAsync(RealEstateListings.StripSize(count), cancellationToken));

    public Task<RealEstatePriceStatisticsDto> GetPriceStatisticsAsync(
        ApartmentFilterParams filter, CancellationToken cancellationToken = default) =>
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

    public async Task<ApartmentDetailsDto> ReorderImagesAsync(
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

    public async Task<ApartmentDetailsDto> SetPromotionAsync(
        Guid id, PromoteRealEstateRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, asNoTracking: false, cancellationToken, includeUnmoderated: true)
            ?? throw new NotFoundException("إعلان الشقة مش موجود.");

        entity.IsFeatured = request.IsFeatured;
        entity.IsPremium = request.IsPremium;
        entity.IsUrgent = request.IsUrgent;
        entity.UpdatedAt = DateTime.UtcNow;

        _repository.Update(entity);
        await _repository.SaveChangesAsync();

        await _notifications.NotifyAsync(
            entity.UserId, ListingModuleType.Apartment, NotificationAction.AdminUpdated, id, entity.Title);

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
            RealEstateLookupKeys.ApartmentTypes =>
                _repository.GetLookupAsync<ApartmentTypeLookup>(cancellationToken),
            RealEstateLookupKeys.OwnershipTypes =>
                _repository.GetLookupAsync<ApartmentOwnershipTypeLookup>(cancellationToken),
            RealEstateLookupKeys.ReceptionPieces =>
                _repository.GetLookupAsync<ApartmentReceptionPiecesLookup>(cancellationToken),
            RealEstateLookupKeys.FloorTypes =>
                _repository.GetLookupAsync<ApartmentFloorTypeLookup>(cancellationToken),
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
            RealEstateLookupKeys.SuitableFor =>
                _repository.GetLookupAsync<ApartmentSuitableForLookup>(cancellationToken),
            RealEstateLookupKeys.ExchangeTargets =>
                _repository.GetLookupAsync<ApartmentExchangeWithLookup>(cancellationToken),

            _ => throw new NotFoundException($"القائمة '{lookupKey}' مش معروفة.")
        };

    private async Task<Apartment> LoadForWriteAsync(
        string userId, bool isAdmin, Guid id, CancellationToken cancellationToken)
    {
        var entity = isAdmin
            ? await _repository.GetByIdAsync(id, asNoTracking: false, cancellationToken, includeUnmoderated: true)
            : await _repository.GetOwnedAsync(id, userId, cancellationToken);

        return entity ?? throw new NotFoundException("إعلان الشقة مش موجود.");
    }

    private static void ApplyEditableFields(Apartment entity, CreateApartmentRequest request)
    {
        var isSale = RealEstateListings.IsSale(request.ListingType);
        var isRent = RealEstateListings.IsRent(request.ListingType);
        var isExchange = RealEstateListings.IsExchange(request.ListingType);

        entity.Title = request.Title;
        entity.Description = request.Description;
        entity.AdvertiserName = request.AdvertiserName.Trim();

        entity.ListingType = request.ListingType;

        entity.ApartmentType = request.ApartmentType;
        entity.OtherApartmentType = RealEstateListings.Keep(
            request.ApartmentType == ApartmentType.Other, request.OtherApartmentType);
        entity.OwnershipType = request.OwnershipType;

        entity.Price = RealEstateListings.Keep(isSale, request.Price);
        entity.PricePerMeter = RealEstateListings.Keep(isSale, request.PricePerMeter);
        entity.Area = request.Area;
        entity.RoomsCount = request.RoomsCount;
        entity.BathroomsCount = request.BathroomsCount;
        entity.ReceptionPieces = request.ReceptionPieces;
        entity.FloorType = request.FloorType;

        var hasFloorDetails = request.FloorType is not null
            && request.FloorType != ApartmentFloorType.Basement;
        entity.FloorNumber = RealEstateListings.Keep(hasFloorDetails, request.FloorNumber);
        entity.TotalFloors = RealEstateListings.Keep(hasFloorDetails, request.TotalFloors);
        entity.ApartmentsPerFloor = RealEstateListings.Keep(hasFloorDetails, request.ApartmentsPerFloor);

        entity.HasElevator = request.HasElevator;
        entity.FurnishedStatus = request.FurnishedStatus;
        entity.FinishingType = request.FinishingType;
        entity.PropertyAge = request.PropertyAge;
        entity.Direction = request.Direction;
        entity.ViewType = request.ViewType;

        entity.LegalStatus = request.LegalStatus;

        var isLicensed = request.LegalStatus == ApartmentLegalStatus.Licensed;
        entity.LicenseNumber = RealEstateListings.Keep(isLicensed, request.LicenseNumber);
        entity.LicenseIssueDate = RealEstateListings.Keep(isLicensed, request.LicenseIssueDate);
        entity.LicenseExpiryDate = RealEstateListings.Keep(isLicensed, request.LicenseExpiryDate);
        entity.LicenseIssuer = RealEstateListings.Keep(isLicensed, request.LicenseIssuer);

        entity.ReconciliationForm = RealEstateListings.Keep(
            request.LegalStatus == ApartmentLegalStatus.Reconciliation, request.ReconciliationForm);

        entity.OwnershipDocument = request.OwnershipDocument;
        entity.IsRegistered = request.IsRegistered;
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

        entity.SuitableFor = null;
        entity.OwnerConditions = RealEstateListings.Keep(isRent, request.OwnerConditions);

        entity.ExchangeWith = RealEstateListings.Keep(isExchange, request.ExchangeWith);
        entity.AcceptsDifferencePayment = RealEstateListings.Keep(isExchange, request.AcceptsDifferencePayment);
        entity.DifferenceAmount = RealEstateListings.Keep(
            isExchange && request.AcceptsDifferencePayment == true, request.DifferenceAmount);
        entity.ExchangeDetails = RealEstateListings.Keep(isExchange, request.ExchangeDetails);
    }

    private static List<ApartmentFeatureSelection> BuildFeatures(
        Guid listingId, IEnumerable<ApartmentFeature> features) =>
        features.Distinct()
            .Select(feature => new ApartmentFeatureSelection { ApartmentId = listingId, Feature = feature })
            .ToList();

    private static List<ApartmentRentInclusionSelection> BuildRentInclusions(
        Guid listingId, IEnumerable<ApartmentRentInclusion> inclusions) =>
        inclusions.Distinct()
            .Select(inclusion => new ApartmentRentInclusionSelection
            {
                ApartmentId = listingId,
                Inclusion = inclusion
            })
            .ToList();

    private async Task ReplaceFeaturesAsync(Apartment entity, IEnumerable<ApartmentFeature> requested)
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
        Apartment entity, IEnumerable<ApartmentRentInclusion> requested)
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

    private async Task<ApartmentDetailsDto> BuildDetailsAsync(
        Guid id, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        var entity = await _repository.GetByIdAsync(id, asNoTracking: true, cancellationToken, includeUnmoderated)
            ?? throw new NotFoundException("إعلان الشقة مش موجود.");

        var details = _mapper.Map<ApartmentDetailsDto>(entity);

        details.ShareUrl = RealEstateListings.ShareUrl(_fileService, RealEstateRoutes.Apartments, id);

        return details;
    }

    private Task<List<ApartmentImage>> AttachImagesAsync(
        Apartment entity,
        IReadOnlyList<UploadImageModel> images,
        CancellationToken cancellationToken) =>
        ListingImages.AttachOrderedAsync(
            _fileService,
            entity.Images,
            images,
            ImageConstants.ApartmentFolder,
            _ => new ApartmentImage { ApartmentId = entity.Id },
            cancellationToken);
}
