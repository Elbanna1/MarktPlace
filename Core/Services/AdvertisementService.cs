using AutoMapper;
using Domain.Entities;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Advertisements;
using Shared.Enums;
using Shared.Exceptions;
using Shared.Responses;

namespace Services;

public class AdvertisementService : IAdvertisementService
{
    private readonly IAdvertisementRepository _repository;
    private readonly IFileService _fileService;
    private readonly INotificationService _notificationService;
    private readonly IListingInteractionService _interactions;
    private readonly ILookupService _lookups;
    private readonly IMapper _mapper;

    public AdvertisementService(
        IAdvertisementRepository repository,
        IFileService fileService,
        INotificationService notificationService,
        IListingInteractionService interactions,
        ILookupService lookups,
        IMapper mapper)
    {
        _repository = repository;
        _fileService = fileService;
        _notificationService = notificationService;
        _interactions = interactions;
        _lookups = lookups;
        _mapper = mapper;
    }

    public async Task<AdvertisementDetailsDto> CreateAsync(
        string ownerId,
        CreateAdvertisementRequest request,
        IReadOnlyList<UploadImageModel> images,
        UploadImageModel? video = null)
    {
        var categoryId = await ResolveCategoryIdAsync(request.SubCategoryId);

        var advertisement = _mapper.Map<Advertisement>(request);
        var now = DateTime.UtcNow;

        advertisement.Id = Guid.NewGuid();
        advertisement.OwnerId = ownerId;
        advertisement.CategoryId = categoryId;
        advertisement.Governorate = LocationConstants.Governorate;
        advertisement.Status = AdvertisementStatus.Active;
        advertisement.Views = 0;
        advertisement.CreatedAt = now;

        await AttachFeaturesAsync(advertisement, request.FeatureIds);
        await AttachImagesAsync(advertisement, images, startAsPrimary: true);

        if (video is not null)
        {
            var stored = await _fileService.SaveVideoAsync(video, FileUploadConstants.AdsVideoFolder);
            advertisement.VideoPath = stored.RelativePath;
            advertisement.VideoUrl = stored.Url;
        }

        await _repository.AddAsync(advertisement);
        await _repository.SaveChangesAsync();

        await _notificationService.NotifyAsync(
            ownerId, ListingModuleType.Advertisement, NotificationAction.Created,
            advertisement.Id, advertisement.Title);

        return await GetDetailsOrThrowAsync(
            advertisement.Id, ownerId, includeUnmoderated: true, newlyCreated: true);
    }

    public async Task<AdvertisementDetailsDto> UpdateAsync(string ownerId, Guid id, UpdateAdvertisementRequest request)
    {
        var advertisement = await _repository.GetOwnedAsync(id, ownerId)
            ?? throw new NotFoundException("الإعلان مش موجود.");

        advertisement.CategoryId = await ResolveCategoryIdAsync(request.SubCategoryId);

        _mapper.Map(request, advertisement);

        advertisement.AdvertisementFeatures.Clear();
        await AttachFeaturesAsync(advertisement, request.FeatureIds);

        _repository.Update(advertisement);
        await _repository.SaveChangesAsync();

        await _notificationService.NotifyAsync(
            ownerId, ListingModuleType.Advertisement, NotificationAction.Updated,
            advertisement.Id, advertisement.Title);

        return await GetDetailsOrThrowAsync(id, ownerId, includeUnmoderated: true);
    }

    public async Task DeleteAsync(string ownerId, Guid id)
    {
        var advertisement = await _repository.GetOwnedAsync(id, ownerId)
            ?? throw new NotFoundException("الإعلان مش موجود.");

        foreach (var image in advertisement.Images.ToList())
        {
            _fileService.Delete(image.ImagePath);
            _repository.RemoveImage(image);
        }

        if (advertisement.VideoPath is { Length: > 0 } videoPath)
        {
            _fileService.Delete(videoPath);
            advertisement.VideoPath = null;
            advertisement.VideoUrl = null;
        }

        advertisement.Status = AdvertisementStatus.Deleted;
        advertisement.DeletedAt = DateTime.UtcNow;

        _repository.Update(advertisement);
        await _repository.SaveChangesAsync();

        await _interactions.PurgeListingAsync(ListingModuleType.Advertisement, advertisement.Id);

        await _notificationService.NotifyAsync(
            ownerId, ListingModuleType.Advertisement, NotificationAction.Deleted,
            advertisement.Id, advertisement.Title);
    }

    public async Task<PaginatedResult<AdvertisementListItemDto>> GetAllAsync(
        AdvertisementFilterParams filter, string? viewerUserId = null,
        CancellationToken cancellationToken = default)
    {
        var (items, total) = await _repository.GetPagedAsync(
            filter, ownerId: null, includeNonActive: false, cancellationToken);

        return await ToPagedResultAsync(items, total, filter, viewerUserId, cancellationToken);
    }

    public async Task<AdvertisementDetailsDto> GetByIdAsync(
        Guid id, string? viewerUserId, string? ipAddress, CancellationToken cancellationToken = default)
    {
        var ownership = await _repository.GetOwnershipAsync(id, cancellationToken);

        if (ownership is null ||
            ownership.Status == AdvertisementStatus.Deleted ||
            ownership.DeletedAt is not null)
        {
            throw new NotFoundException("الإعلان مش موجود.");
        }

        var isOwner = viewerUserId is not null &&
                      string.Equals(ownership.OwnerId, viewerUserId, StringComparison.Ordinal);

        if (!isOwner && ownership.ModerationStatus != ModerationStatus.Approved)
            throw new NotFoundException("الإعلان مش موجود.");

        if (!isOwner &&
            ownership.Status != AdvertisementStatus.Pending &&
            ownership.ExpireAt is { } window && window <= DateTime.UtcNow)
        {
            throw new NotFoundException("الإعلان مش موجود.");
        }

        return await GetDetailsOrThrowAsync(id, viewerUserId, isOwner, cancellationToken);
    }

    public async Task<AdvertisementStatisticsDto> GetMyStatisticsAsync(
        string ownerId, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var counts = await _repository.GetOwnerStatusCountsAsync(ownerId, now, cancellationToken);

        var live = counts.Where(c => c.Status != AdvertisementStatus.Deleted).ToList();

        return new AdvertisementStatisticsDto
        {
            Total = live.Sum(c => c.Count),
            Active = SumOf(live, AdvertisementStatus.Active),
            Expired = SumOf(live, AdvertisementStatus.Expired),
            Pending = SumOf(live, AdvertisementStatus.Pending),
            Deleted = SumOf(counts, AdvertisementStatus.Deleted),
            TotalViews = live.Sum(c => c.Views),

            CanRepublish = SumOf(live, AdvertisementStatus.Expired),

            ByCategory = live
                .GroupBy(c => new { c.CategoryId, c.CategoryName, c.CategoryNameAr })
                .Select(g => new AdvertisementCategoryStatisticsDto
                {
                    CategoryId = g.Key.CategoryId,
                    CategoryName = g.Key.CategoryName,
                    CategoryNameAr = g.Key.CategoryNameAr,
                    Total = g.Sum(c => c.Count),
                    Active = SumOf(g, AdvertisementStatus.Active),
                    Expired = SumOf(g, AdvertisementStatus.Expired)
                })
                .OrderBy(c => c.CategoryId)
                .ToList(),

            BySubCategory = live
                .GroupBy(c => new { c.SubCategoryId, c.SubCategoryName, c.SubCategoryNameAr, c.CategoryId })
                .Select(g => new AdvertisementSubCategoryStatisticsDto
                {
                    SubCategoryId = g.Key.SubCategoryId,
                    SubCategoryName = g.Key.SubCategoryName,
                    SubCategoryNameAr = g.Key.SubCategoryNameAr,
                    CategoryId = g.Key.CategoryId,
                    Total = g.Sum(c => c.Count),
                    Active = SumOf(g, AdvertisementStatus.Active),
                    Expired = SumOf(g, AdvertisementStatus.Expired)
                })
                .OrderBy(s => s.SubCategoryId)
                .ToList()
        };
    }

    private static int SumOf(IEnumerable<OwnerAdvertisementCount> counts, AdvertisementStatus status) =>
        counts.Where(c => c.Status == status).Sum(c => c.Count);

    public async Task<PaginatedResult<AdvertisementListItemDto>> GetMyAdsAsync(
        string ownerId, AdvertisementFilterParams filter, CancellationToken cancellationToken = default)
    {
        var (items, total) = await _repository.GetPagedAsync(
            filter, ownerId, includeNonActive: true, cancellationToken);

        return await ToPagedResultAsync(items, total, filter, ownerId, cancellationToken);
    }

    public async Task<AdvertisementDetailsDto> RepublishAsync(
        string ownerId, Guid id, UpdateAdvertisementRequest? edits = null)
    {
        var advertisement = await _repository.GetOwnedAsync(id, ownerId)
            ?? throw new NotFoundException("الإعلان مش موجود.");

        var now = DateTime.UtcNow;

        if (advertisement.Status == AdvertisementStatus.Deleted)
            throw new BadRequestException("الإعلان المحذوف مش ممكن يتنشر تاني.");

        if (!advertisement.IsExpiredAt(now))
        {
            throw new BadRequestException(
                "الإعلان اللي لسه سارٍ مش محتاج إعادة نشر. " +
                $"لسه فاضل له {advertisement.RemainingDaysAt(now)} يوم.");
        }

        if (advertisement.ExpiredAt is { } expiredAt &&
            expiredAt.AddDays(AdvertisementConstants.ExpiredGracePeriodDays) < now)
        {
            throw new BadRequestException("معاد إعادة النشر للإعلان ده عدى.");
        }

        var freeUntil = (advertisement.FirstPublishedAt ?? advertisement.CreatedAt)
            .AddYears(AdvertisementConstants.FreeRepublishPeriodYears);
        if (now > freeUntil)
        {
            throw new PaymentRequiredException(
                "إعادة النشر مجانية في السنة الأولى بس. عشان تنشر الإعلان ده تاني لازم تدفع أونلاين.");
        }

        if (edits is not null)
        {
            await EnsureSubCategoryExistsAsync(edits.SubCategoryId);

            _mapper.Map(edits, advertisement);

            advertisement.AdvertisementFeatures.Clear();
            await AttachFeaturesAsync(advertisement, edits.FeatureIds);
        }

        advertisement.Status = AdvertisementStatus.Active;
        advertisement.CreatedAt = now;
        advertisement.ExpiredAt = null;
        advertisement.DeletedAt = null;
        advertisement.RepublishCount++;

        advertisement.PublishedAt = null;
        advertisement.ExpireAt = null;

        advertisement.ModerationStatus = ModerationStatus.Pending;
        advertisement.ModeratedAt = null;
        advertisement.ModeratedBy = null;
        advertisement.RejectionReason = null;
        advertisement.ModerationNotes = null;

        _repository.Update(advertisement);
        await _repository.SaveChangesAsync();

        await _notificationService.NotifyAsync(
            ownerId, ListingModuleType.Advertisement, NotificationAction.Republished,
            advertisement.Id, advertisement.Title);

        return await GetDetailsOrThrowAsync(id, ownerId, includeUnmoderated: true);
    }

    public async Task<IReadOnlyList<AdvertisementImageDto>> AddImagesAsync(
        string ownerId, Guid id, IReadOnlyList<UploadImageModel> images)
    {
        var advertisement = await _repository.GetOwnedAsync(id, ownerId)
            ?? throw new NotFoundException("الإعلان مش موجود.");

        var startAsPrimary = advertisement.Images.Count == 0;
        var added = await AttachImagesAsync(advertisement, images, startAsPrimary);

        await _repository.AddImagesAsync(added);
        await _repository.SaveChangesAsync();

        return _mapper.Map<IReadOnlyList<AdvertisementImageDto>>(added);
    }

    public async Task DeleteImageAsync(string ownerId, Guid imageId)
    {
        var image = await _repository.GetImageAsync(imageId);

        if (image is null || image.Advertisement is null || image.Advertisement.OwnerId != ownerId)
            throw new NotFoundException("الصورة مش موجودة.");

        var advertisementId = image.AdvertisementId;
        var wasPrimary = image.IsPrimary;

        _fileService.Delete(image.ImagePath);
        _repository.RemoveImage(image);
        await _repository.SaveChangesAsync();

        if (wasPrimary)
        {
            var advertisement = await _repository.GetByIdAsync(advertisementId, asNoTracking: false);
            if (advertisement is not null && advertisement.Images.Count > 0)
            {
                ListingImages.NormalizePrimary(advertisement.Images);
                await _repository.SaveChangesAsync();
            }
        }
    }

    private async Task EnsureSubCategoryExistsAsync(int subCategoryId)
    {
        if (!await _repository.SubCategoryExistsAsync(subCategoryId))
            throw new BadRequestException("القسم الفرعي اللي اخترته مش موجود.");
    }

    private async Task<int> ResolveCategoryIdAsync(int subCategoryId)
    {
        var tree = await _lookups.GetCategoriesTreeAsync();

        foreach (var category in tree)
        {
            foreach (var subCategory in category.SubCategories)
            {
                if (subCategory.Id == subCategoryId)
                    return category.Id;
            }
        }

        return await _repository.GetSubCategoryCategoryIdAsync(subCategoryId)
            ?? throw new BadRequestException("القسم الفرعي اللي اخترته مش موجود.");
    }

    private async Task AttachFeaturesAsync(Advertisement advertisement, IEnumerable<int> featureIds)
    {
        var ids = featureIds?.Distinct().ToList() ?? new List<int>();
        if (ids.Count == 0)
            return;

        var features = await _repository.GetFeaturesByIdsAsync(ids);
        foreach (var feature in features)
        {
            advertisement.AdvertisementFeatures.Add(new AdvertisementFeature
            {
                AdvertisementId = advertisement.Id,
                FeatureId = feature.Id
            });
        }
    }

    private Task<List<AdvertisementImage>> AttachImagesAsync(
        Advertisement advertisement, IReadOnlyList<UploadImageModel> images, bool startAsPrimary) =>
        ListingImages.AttachAsync(
            _fileService,
            advertisement.Images,
            images,
            ImageConstants.AdsFolder,
            startAsPrimary,
            _ => new AdvertisementImage { AdvertisementId = advertisement.Id });

    private async Task<AdvertisementDetailsDto> GetDetailsOrThrowAsync(
        Guid id, string? viewerUserId = null, bool includeUnmoderated = false,
        CancellationToken cancellationToken = default, bool newlyCreated = false)
    {
        var advertisement = await _repository.GetDetailsAsync(id, includeUnmoderated, cancellationToken)
            ?? throw new NotFoundException("الإعلان مش موجود.");

        var details = _mapper.Map<CarDetailsDto>(advertisement);

        if (!newlyCreated)
        {
            var counters = await _interactions.GetCountersAsync(
                ListingModuleType.Advertisement, id, viewerUserId, cancellationToken);

            details.Views = counters.Views;
            details.FavoriteCount = counters.FavoriteCount;
            details.AverageRating = counters.AverageRating;
            details.RatingsCount = counters.RatingsCount;
            details.IsFavorite = counters.IsFavorite;
        }

        details.CanReport = viewerUserId is not null && viewerUserId != advertisement.OwnerId;

        return details;
    }

    private async Task<PaginatedResult<AdvertisementListItemDto>> ToPagedResultAsync(
        IReadOnlyList<AdvertisementCardRow> items, int total, AdvertisementFilterParams filter,
        string? viewerUserId, CancellationToken cancellationToken)
    {
        var mapped = _mapper.Map<IReadOnlyList<AdvertisementListItemDto>>(items);

        if (mapped.Count > 0)
        {
            var keys = mapped
                .Select(item => (ListingModuleType.Advertisement, item.Id))
                .ToList();

            var counters = await _interactions.GetCountersAsync(keys, viewerUserId, cancellationToken);

            foreach (var item in mapped)
            {
                var counter = counters.GetValueOrDefault((ListingModuleType.Advertisement, item.Id));

                if (counter is null)
                    continue;

                item.Views = counter.Views;
                item.FavoriteCount = counter.FavoriteCount;
                item.AverageRating = counter.AverageRating;
                item.RatingsCount = counter.RatingsCount;
                item.IsFavorite = counter.IsFavorite;
            }
        }

        return new PaginatedResult<AdvertisementListItemDto>(mapped, total, filter.PageIndex, filter.PageSize);
    }
}
