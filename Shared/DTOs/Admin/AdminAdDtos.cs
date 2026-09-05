using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using Shared.Constants;
using Shared.DTOs.Common;
using Shared.DTOs.Listings;
using Shared.Enums;

namespace Shared.DTOs.Admin;

public class AdminAdListItemDto : IListingStats
{
    public Guid Id { get; set; }

    public string Type { get; set; } = default!;

    public ListingModuleType TypeId { get; set; }

    public string Route { get; set; } = default!;

    public string DetailsEndpoint { get; set; } = default!;

    public string Title { get; set; } = default!;

    public string? MainImageUrl { get; set; }

    public decimal? Price { get; set; }

    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = default!;

    public int SubCategoryId { get; set; }

    public string SubCategoryName { get; set; } = default!;

    public string OwnerId { get; set; } = default!;

    public string? OwnerName { get; set; }

    public string? OwnerPhone { get; set; }

    public string Status { get; set; } = default!;

    public ListingModerationDto Moderation { get; set; } = default!;

    public DateTime CreatedAt { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public DateTime? ExpireAt { get; set; }

    public int? RemainingDays { get; set; }

    [JsonIgnore]
    public ListingModuleType StatsListingType => TypeId;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Views { get; set; }

    public decimal? AverageRating { get; set; }

    public int RatingsCount { get; set; }

    public bool IsFavorite { get; set; }
}

public class AdminAdFilterParams : PaginationParams
{
    public string? Search { get; set; }

    public int? CategoryId { get; set; }

    public int? SubCategoryId { get; set; }

    public ListingModuleType? Type { get; set; }

    public ModerationStatus? ModerationStatus { get; set; }

    public ListingStatus? Status { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public string? OwnerId { get; set; }
}

public class AdminAdDetailsDto : IListingStats
{
    public Guid Id { get; set; }

    public string Type { get; set; } = default!;

    public ListingModuleType TypeId { get; set; }

    public string TypeName { get; set; } = default!;

    public string Route { get; set; } = default!;

    public string DetailsEndpoint { get; set; } = default!;

    public string Title { get; set; } = default!;

    public decimal? Price { get; set; }

    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = default!;

    public int SubCategoryId { get; set; }

    public string SubCategoryName { get; set; } = default!;

    public string OwnerId { get; set; } = default!;

    public string? OwnerName { get; set; }

    public string? OwnerPhone { get; set; }

    public string? OwnerEmail { get; set; }

    public string Status { get; set; } = default!;

    public ListingModerationDto Moderation { get; set; } = default!;

    public DateTime CreatedAt { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public DateTime? ExpireAt { get; set; }

    public int? RemainingDays { get; set; }

    public IReadOnlyList<string> ImageUrls { get; set; } = Array.Empty<string>();

    public IReadOnlyList<string> VideoUrls { get; set; } = Array.Empty<string>();

    public IReadOnlyList<AdminAdFieldValueDto> Fields { get; set; } = Array.Empty<AdminAdFieldValueDto>();

    public IReadOnlyDictionary<string, object?> Raw { get; set; } =
        new Dictionary<string, object?>();

    [JsonIgnore]
    public ListingModuleType StatsListingType => TypeId;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Views { get; set; }

    public decimal? AverageRating { get; set; }

    public int RatingsCount { get; set; }

    public bool IsFavorite { get; set; }
}

public class AdminAdFieldValueDto
{
    public string Name { get; set; } = default!;

    public string Label { get; set; } = default!;

    public string? Type { get; set; }

    public object? Value { get; set; }

    public string? DisplayValue { get; set; }

    public bool InForm { get; set; }
}

public class RejectListingRequest
{
    [Required(ErrorMessage = "سبب الرفض مطلوب.")]
    [EnumDataType(typeof(ListingRejectionReason), ErrorMessage = "سبب الرفض غير صحيح.")]
    public ListingRejectionReason Reason { get; set; }

    [MaxLength(ModerationCatalog.MaxNotesLength,
        ErrorMessage = "لا يمكن أن تتجاوز الملاحظات 500 حرف.")]
    public string? Notes { get; set; }
}

public class SuspendListingRequest
{
    [MaxLength(ModerationCatalog.MaxNotesLength,
        ErrorMessage = "لا يمكن أن تتجاوز الملاحظات 500 حرف.")]
    public string? Notes { get; set; }
}

public class ApproveListingRequest
{
    [MaxLength(ModerationCatalog.MaxNotesLength,
        ErrorMessage = "لا يمكن أن تتجاوز الملاحظات 500 حرف.")]
    public string? Notes { get; set; }
}

public class AdminModerationResultDto
{
    public Guid Id { get; set; }

    public ListingModuleType TypeId { get; set; }

    public string Type { get; set; } = default!;

    public string Title { get; set; } = default!;

    public string OwnerId { get; set; } = default!;

    public ListingModerationDto Moderation { get; set; } = default!;
}

public class AdminAdMetadataDto
{
    public IReadOnlyList<AdminOptionDto> ModerationStatuses { get; set; } = Array.Empty<AdminOptionDto>();

    public IReadOnlyList<AdminOptionDto> RejectionReasons { get; set; } = Array.Empty<AdminOptionDto>();

    public IReadOnlyList<AdminModuleOptionDto> Modules { get; set; } = Array.Empty<AdminModuleOptionDto>();
}

public class AdminOptionDto
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public bool? RequiresNotes { get; set; }
}

public class AdminModuleOptionDto
{
    public ListingModuleType Id { get; set; }

    public string Name { get; set; } = default!;

    public string ArabicName { get; set; } = default!;

    public string Route { get; set; } = default!;

    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = default!;

    public int SubCategoryId { get; set; }

    public string SubCategoryName { get; set; } = default!;
}
