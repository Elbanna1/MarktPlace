using Domain.Entities;
using Microsoft.Extensions.Logging;
using ServicesAbstraction;
using Services.AdForms;
using Services.Listings;
using Shared.Constants;
using Shared.DTOs.Admin;
using Shared.DTOs.Listings;
using Shared.DTOs.Lookups.Forms;
using Shared.Enums;
using Shared.Exceptions;
using Shared.Responses;

namespace Services.Admin;

public class AdminAdService : IAdminAdService
{
    private readonly IAdminAdRepository _repository;
    private readonly IListingInteractionService _interactions;
    private readonly ILookupService _lookups;
    private readonly INotificationService _notifications;
    private readonly IListingInterestNotifier _interestNotifier;
    private readonly IAdminAuditService _audit;
    private readonly ILogger<AdminAdService> _logger;

    public AdminAdService(
        IAdminAdRepository repository,
        IListingInteractionService interactions,
        ILookupService lookups,
        INotificationService notifications,
        IListingInterestNotifier interestNotifier,
        IAdminAuditService audit,
        ILogger<AdminAdService> logger)
    {
        _repository = repository;
        _interactions = interactions;
        _lookups = lookups;
        _notifications = notifications;
        _interestNotifier = interestNotifier;
        _audit = audit;
        _logger = logger;
    }

    public Task<PaginatedResult<AdminAdListItemDto>> GetAdsAsync(
        AdminAdFilterParams filter, CancellationToken cancellationToken = default) =>
        GetAdsAsync(filter, includeTotal: true, cancellationToken);

    public async Task<IReadOnlyList<AdminAdListItemDto>> GetLatestAdsAsync(
        int count, CancellationToken cancellationToken = default)
    {
        var page = await GetAdsAsync(
            new AdminAdFilterParams { PageIndex = 1, PageSize = count },
            includeTotal: false,
            cancellationToken);

        return page.Items;
    }

    private async Task<PaginatedResult<AdminAdListItemDto>> GetAdsAsync(
        AdminAdFilterParams filter, bool includeTotal, CancellationToken cancellationToken)
    {
        var utcNow = DateTime.UtcNow;

        var (rows, totalCount) = await _repository.GetPagedAsync(
            filter, utcNow, cancellationToken, includeTotal);

        if (rows.Count == 0)
            return new PaginatedResult<AdminAdListItemDto>(
                Array.Empty<AdminAdListItemDto>(), totalCount, filter.PageIndex, filter.PageSize);

        var owners = await _repository.GetOwnersAsync(
            rows.Select(row => row.OwnerId).Distinct().ToList(), cancellationToken);

        var tree = await _lookups.GetCategoriesTreeAsync(cancellationToken);

        var items = rows.Select(row =>
        {
            var (categoryId, subCategoryId) = ListingInteractionService.ResolveCategory(row);
            var (categoryName, subCategoryName) =
                ListingInteractionService.ResolveNames(tree, categoryId, subCategoryId);

            var owner = owners.GetValueOrDefault(row.OwnerId);
            var route = ListingModuleCatalog.RouteOf(row.Type);

            return new AdminAdListItemDto
            {
                Id = row.Id,
                Type = row.Type.ToString(),
                TypeId = row.Type,
                Route = route,
                DetailsEndpoint = DetailsEndpointOf(route, row.Id),
                Title = row.Title,
                MainImageUrl = row.MainImageUrl,
                Price = row.Price,
                CategoryId = categoryId,
                CategoryName = categoryName,
                SubCategoryId = subCategoryId,
                SubCategoryName = subCategoryName,
                OwnerId = row.OwnerId,
                OwnerName = string.IsNullOrWhiteSpace(owner?.Name) ? null : owner!.Name,
                OwnerPhone = owner?.PhoneNumber,
                Status = row.Status.ToString(),

                Moderation = ListingModerationDto.From(row),
                CreatedAt = row.CreatedAt,
                StartDate = row.PublishedAt,
                EndDate = row.ExpireAt,
                ExpireAt = row.ExpireAt,

                RemainingDays = row.Status == ListingStatus.Expired
                    ? 0
                    : ListingLifecycle.RemainingDays(row.ExpireAt, utcNow)
            };
        }).ToList();

        return new PaginatedResult<AdminAdListItemDto>(
            items, totalCount, filter.PageIndex, filter.PageSize);
    }

    public Task<int> GetPendingCountAsync(CancellationToken cancellationToken = default) =>
        _repository.CountByStatusAsync(ModerationStatus.Pending, DateTime.UtcNow, cancellationToken);

    public async Task<AdminAdDetailsDto> GetAdDetailsAsync(
        ListingModuleType type, Guid id, CancellationToken cancellationToken = default)
    {
        var snapshot = await _repository.LoadListingAsync(type, id, cancellationToken)
            ?? throw new NotFoundException("الإعلان غير موجود.");

        var listing = snapshot.Listing;
        var values = snapshot.Values;

        var subCategoryId = ReadInt(values, "SubCategoryId")
            ?? (int?)ListingModuleCatalog.SubCategoryOf(type)
            ?? 0;

        var categoryId = ReadInt(values, "CategoryId")
            ?? (int?)ListingModuleCatalog.CategoryOf(type)
            ?? 0;

        var tree = await _lookups.GetCategoriesTreeAsync(cancellationToken);
        var (categoryName, subCategoryName) =
            ListingInteractionService.ResolveNames(tree, categoryId, subCategoryId);

        var owners = await _repository.GetOwnersAsync(new[] { listing.OwnerUserId }, cancellationToken);
        var owner = owners.GetValueOrDefault(listing.OwnerUserId);

        var route = ListingModuleCatalog.RouteOf(type);

        var schema = AdFormSchemaCatalog.GetSchema(categoryId, subCategoryId);
        var fields = BuildFields(values, schema?.Fields);

        var expireAt = ReadDate(values, "ExpireAt");
        var publishedAt = ReadDate(values, "PublishedAt");
        var createdAt = ReadDate(values, "CreatedAt") ?? DateTime.UtcNow;

        return new AdminAdDetailsDto
        {
            Id = id,
            Type = type.ToString(),
            TypeId = type,
            TypeName = ListingModuleCatalog.NameOf(type),
            Route = route,
            DetailsEndpoint = DetailsEndpointOf(route, id),

            Title = listing.ListingTitle,
            Price = ReadDecimal(values, "Price"),
            CategoryId = categoryId,
            CategoryName = categoryName,
            SubCategoryId = subCategoryId,
            SubCategoryName = subCategoryName,

            OwnerId = listing.OwnerUserId,
            OwnerName = string.IsNullOrWhiteSpace(owner?.Name) ? null : owner!.Name,
            OwnerPhone = owner?.PhoneNumber,

            Status = ResolveStatus(listing.ModerationStatus, expireAt).ToString(),
            Moderation = new ListingModerationDto
            {
                Status = listing.ModerationStatus.ToString(),
                StatusName = ModerationCatalog.NameOf(listing.ModerationStatus),
                Reason = listing.RejectionReason,
                ReasonName = listing.RejectionReason is { } reason
                    ? ModerationCatalog.NameOf(reason)
                    : null,
                Notes = listing.ModerationNotes,
                DecidedAt = listing.ModeratedAt
            },

            CreatedAt = createdAt,
            StartDate = publishedAt,
            EndDate = expireAt,
            ExpireAt = expireAt,
            RemainingDays = ListingLifecycle.RemainingDays(expireAt, DateTime.UtcNow),

            ImageUrls = snapshot.ImageUrls,
            VideoUrls = snapshot.VideoUrls,

            Fields = fields,
            Raw = values
        };
    }

    public async Task DeleteAsync(
        ListingModuleType type, Guid id, string adminUserId, CancellationToken cancellationToken = default)
    {
        var listing = await LoadAsync(type, id, cancellationToken);

        var ownerId = listing.OwnerUserId;
        var title = listing.ListingTitle;
        var previousStatus = listing.ModerationStatus;

        var deleted = await _repository.DeleteListingAsync(type, id, cancellationToken);

        if (!deleted)
            throw new NotFoundException("الإعلان غير موجود.");

        await _interactions.PurgeListingAsync(type, id, cancellationToken);

        await _audit.LogAsync(
            AdminAuditAction.DeleteAd, AdminAuditCatalog.Targets.Advertisement, id.ToString(),
            $"حذف إعلان: {title}",
            oldValue: ModerationCatalog.NameOf(previousStatus), newValue: "محذوف",
            adminUserId: adminUserId, cancellationToken: cancellationToken);

        await NotifyAsync(
            ownerId,
            NotificationCatalog.Moderation.Suspended(type, id, title, "تم حذف الإعلان بواسطة الإدارة."),
            NotificationAction.Deleted,
            id,
            type);
    }

    public async Task<AdminModerationResultDto> ApproveAsync(
        ListingModuleType type, Guid id, string adminUserId,
        ApproveListingRequest request, CancellationToken cancellationToken = default)
    {
        var listing = await LoadAsync(type, id, cancellationToken);

        var previousStatus = listing.ModerationStatus;

        Decide(listing, ModerationStatus.Approved, adminUserId, reason: null, request.Notes);

        await _repository.SaveModerationAsync(type, cancellationToken);

        await _audit.LogAsync(
            AdminAuditAction.ApproveAd, AdminAuditCatalog.Targets.Advertisement, id.ToString(),
            $"قبول إعلان: {listing.ListingTitle}",
            oldValue: ModerationCatalog.NameOf(previousStatus),
            newValue: ModerationCatalog.NameOf(ModerationStatus.Approved),
            adminUserId: adminUserId, cancellationToken: cancellationToken);

        await NotifyAsync(
            listing.OwnerUserId,
            NotificationCatalog.Moderation.Approved(type, id, listing.ListingTitle),
            NotificationAction.Approved,
            id,
            type);

        try
        {
            await _interestNotifier.AnnounceAsync(type, id, cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception,
                "The approval of {Module} {ListingId} succeeded but the interest fan-out failed.",
                type, id);
        }

        return ToResult(type, id, listing);
    }

    public async Task<AdminModerationResultDto> RejectAsync(
        ListingModuleType type, Guid id, string adminUserId,
        RejectListingRequest request, CancellationToken cancellationToken = default)
    {
        if (ModerationCatalog.RequiresNotes(request.Reason) && string.IsNullOrWhiteSpace(request.Notes))
            throw new BadRequestException("يجب كتابة سبب الرفض عند اختيار \"سبب آخر\".");

        var listing = await LoadAsync(type, id, cancellationToken);

        var previousStatus = listing.ModerationStatus;

        Decide(listing, ModerationStatus.Rejected, adminUserId, request.Reason, request.Notes);

        await _repository.SaveModerationAsync(type, cancellationToken);

        await _audit.LogAsync(
            AdminAuditAction.RejectAd, AdminAuditCatalog.Targets.Advertisement, id.ToString(),
            $"رفض إعلان: {listing.ListingTitle} — {ModerationCatalog.NameOf(request.Reason)}",
            oldValue: ModerationCatalog.NameOf(previousStatus),
            newValue: ModerationCatalog.NameOf(ModerationStatus.Rejected),
            adminUserId: adminUserId, cancellationToken: cancellationToken);

        await NotifyAsync(
            listing.OwnerUserId,
            NotificationCatalog.Moderation.Rejected(type, id, listing.ListingTitle, request.Reason, request.Notes),
            NotificationAction.Rejected,
            id,
            type);

        return ToResult(type, id, listing);
    }

    public async Task<AdminModerationResultDto> SuspendAsync(
        ListingModuleType type, Guid id, string adminUserId,
        SuspendListingRequest request, CancellationToken cancellationToken = default)
    {
        var listing = await LoadAsync(type, id, cancellationToken);

        var previousStatus = listing.ModerationStatus;

        Decide(listing, ModerationStatus.Suspended, adminUserId, reason: null, request.Notes);

        await _repository.SaveModerationAsync(type, cancellationToken);

        await _audit.LogAsync(
            AdminAuditAction.SuspendAd, AdminAuditCatalog.Targets.Advertisement, id.ToString(),
            $"إيقاف إعلان: {listing.ListingTitle}",
            oldValue: ModerationCatalog.NameOf(previousStatus),
            newValue: ModerationCatalog.NameOf(ModerationStatus.Suspended),
            adminUserId: adminUserId, cancellationToken: cancellationToken);

        await NotifyAsync(
            listing.OwnerUserId,
            NotificationCatalog.Moderation.Suspended(type, id, listing.ListingTitle, request.Notes),
            NotificationAction.Suspended,
            id,
            type);

        return ToResult(type, id, listing);
    }

    public async Task<AdminAdMetadataDto> GetMetadataAsync(CancellationToken cancellationToken = default)
    {
        var tree = await _lookups.GetCategoriesTreeAsync(cancellationToken);

        return new AdminAdMetadataDto
        {
            ModerationStatuses = ModerationCatalog.AllStatuses
                .Select(status => new AdminOptionDto
                {
                    Id = (int)status,
                    Name = ModerationCatalog.NameOf(status)
                })
                .ToList(),

            RejectionReasons = ModerationCatalog.AllRejectionReasons
                .Select(reason => new AdminOptionDto
                {
                    Id = (int)reason,
                    Name = ModerationCatalog.NameOf(reason),
                    RequiresNotes = ModerationCatalog.RequiresNotes(reason)
                })
                .ToList(),

            Modules = ListingModuleCatalog.All
                .Select(module =>
                {
                    var subCategory = ListingModuleCatalog.SubCategoryOf(module);
                    var category = subCategory is { } sub ? ListingModuleCatalog.CategoryOf(sub) : null;

                    var categoryId = (int)(category ?? 0);
                    var subCategoryId = (int)(subCategory ?? 0);

                    var (categoryName, subCategoryName) =
                        ListingInteractionService.ResolveNames(tree, categoryId, subCategoryId);

                    return new AdminModuleOptionDto
                    {
                        Id = module,
                        Name = module.ToString(),
                        ArabicName = ListingModuleCatalog.NameOf(module),
                        Route = ListingModuleCatalog.RouteOf(module),
                        CategoryId = categoryId,
                        CategoryName = categoryName,
                        SubCategoryId = subCategoryId,
                        SubCategoryName = subCategoryName
                    };
                })
                .ToList()
        };
    }

    private static IReadOnlyList<AdminAdFieldValueDto> BuildFields(
        IReadOnlyDictionary<string, object?> values,
        IReadOnlyList<FormFieldDto>? formFields)
    {
        var fields = new List<AdminAdFieldValueDto>(values.Count);
        var claimed = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var formField in formFields ?? Array.Empty<FormFieldDto>())
        {
            if (!values.TryGetValue(formField.Name, out var value))
                continue;

            claimed.Add(formField.Name);

            fields.Add(new AdminAdFieldValueDto
            {
                Name = formField.Name,
                Label = formField.Label,
                Type = formField.Type,
                Value = value,
                DisplayValue = ResolveDisplayValue(formField, value),
                InForm = true
            });
        }

        foreach (var (name, value) in values)
        {
            if (claimed.Contains(name))
                continue;

            fields.Add(new AdminAdFieldValueDto
            {
                Name = name,
                Label = name,
                Type = null,
                Value = value,
                InForm = false
            });
        }

        return fields;
    }

    private static string? ResolveDisplayValue(FormFieldDto field, object? value)
    {
        if (value is null || field.Options is not { Count: > 0 })
            return null;

        var text = Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture);

        var match = field.Options.FirstOrDefault(option =>
            string.Equals(
                Convert.ToString(option.Value, System.Globalization.CultureInfo.InvariantCulture),
                text,
                StringComparison.OrdinalIgnoreCase));

        return match?.Label;
    }

    private static ListingStatus ResolveStatus(ModerationStatus moderation, DateTime? expireAt) =>
        moderation switch
        {
            ModerationStatus.Pending => ListingStatus.Pending,
            ModerationStatus.Rejected => ListingStatus.Rejected,
            ModerationStatus.Suspended => ListingStatus.Suspended,
            _ => expireAt is { } expiry && expiry <= DateTime.UtcNow
                ? ListingStatus.Expired
                : ListingStatus.Active
        };

    private static int? ReadInt(IReadOnlyDictionary<string, object?> values, string name) =>
        values.TryGetValue(name, out var value) && value is int number ? number : null;

    private static decimal? ReadDecimal(IReadOnlyDictionary<string, object?> values, string name) =>
        values.TryGetValue(name, out var value) && value is decimal number ? number : null;

    private static DateTime? ReadDate(IReadOnlyDictionary<string, object?> values, string name) =>
        values.TryGetValue(name, out var value) && value is DateTime date ? date : null;

    private async Task<IModeratedListing> LoadAsync(
        ListingModuleType type, Guid id, CancellationToken cancellationToken) =>
        await _repository.FindForModerationAsync(type, id, cancellationToken)
        ?? throw new NotFoundException("الإعلان غير موجود.");

    private static void Decide(
        IModeratedListing listing,
        ModerationStatus status,
        string adminUserId,
        ListingRejectionReason? reason,
        string? notes)
    {
        var now = DateTime.UtcNow;

        listing.ModerationStatus = status;
        listing.ModeratedAt = now;
        listing.ModeratedBy = adminUserId;
        listing.RejectionReason = status == ModerationStatus.Rejected ? reason : null;
        listing.ModerationNotes = string.IsNullOrWhiteSpace(notes) ? null : notes.Trim();

        if (status == ModerationStatus.Approved)
            OpenWindow(listing, now);
    }

    private static void OpenWindow(IModeratedListing listing, DateTime now)
    {
        switch (listing)
        {
            case IExpiringListing expiring:
                if (expiring.ExpireAt is { } current && current > now)
                    return;

                expiring.PublishedAt = now;
                expiring.ExpireAt = ListingLifecycle.EndOf(now);
                expiring.FirstPublishedAt ??= now;
                break;

            case Advertisement advertisement:
                if (advertisement.ExpireAt is { } running && running > now)
                    return;

                advertisement.PublishedAt = now;
                advertisement.ExpireAt = ListingLifecycle.EndOf(now);
                advertisement.FirstPublishedAt ??= now;
                break;
        }
    }

    private static AdminModerationResultDto ToResult(
        ListingModuleType type, Guid id, IModeratedListing listing) => new()
    {
        Id = id,
        TypeId = type,
        Type = type.ToString(),
        Title = listing.ListingTitle,
        OwnerId = listing.OwnerUserId,
        Moderation = new ListingModerationDto
        {
            Status = listing.ModerationStatus.ToString(),
            StatusName = ModerationCatalog.NameOf(listing.ModerationStatus),
            Reason = listing.RejectionReason,
            ReasonName = listing.RejectionReason is { } reason
                ? ModerationCatalog.NameOf(reason)
                : null,
            Notes = listing.ModerationNotes,
            DecidedAt = listing.ModeratedAt
        }
    };

    private static string DetailsEndpointOf(string route, Guid id)
    {
        if (string.IsNullOrEmpty(route))
            return string.Empty;

        var firstSegment = route.Split('/', StringSplitOptions.RemoveEmptyEntries)[0];

        return $"/api/{firstSegment}/{id}";
    }

    private Task NotifyAsync(
        string userId, NotificationContent content, NotificationAction action, Guid listingId,
        ListingModuleType type) =>
        _notifications.CreateAsync(
            userId,
            content.WithListing(
                listingType: type,
                categoryId: (int?)ListingModuleCatalog.CategoryOf(type),
                subCategoryId: (int?)ListingModuleCatalog.SubCategoryOf(type)),
            listingId,
            action);
}
