using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Shared.DTOs.Common;
using Shared.DTOs.Listings;
using Shared.Enums;

namespace Shared.DTOs.Directory;

public class DirectoryImageDto
{
    public Guid Id { get; set; }

    public string Url { get; set; } = default!;

    public bool IsPrimary { get; set; }

    public int SortOrder { get; set; }
}

public abstract class CreateDirectoryListingRequest
{
    public string Name { get; set; } = default!;

    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;

    public string Address { get; set; } = default!;

    public string? GoogleMapsUrl { get; set; }

    public string Phone { get; set; } = default!;

    public string? WhatsApp { get; set; }

    public string? Email { get; set; }

    public List<IFormFile> Images { get; set; } = new();
}

public interface IDirectoryGalleryEdit
{
    List<Guid> RemoveImageIds { get; set; }
}

public abstract class DirectoryListingDto : IListingStats
{
    public Guid Id { get; set; }

    public string OwnerId { get; set; } = default!;

    public string OwnerName { get; set; } = default!;

    public ListingModuleType ListingType { get; set; }

    public string ListingTypeName { get; set; } = default!;

    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = default!;

    public int SubCategoryId { get; set; }

    public string SubCategoryName { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;

    public string Address { get; set; } = default!;

    public string? GoogleMapsUrl { get; set; }

    public string Phone { get; set; } = default!;

    public string? WhatsApp { get; set; }

    public string? Email { get; set; }

    public List<DirectoryImageDto> Images { get; set; } = new();

    public string? PrimaryImageUrl { get; set; }

    public string ShareUrl { get; set; } = default!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? ApprovedAt
    {
        get => _approvedAt;

        set => _approvedAt = value is { } moment
            ? DateTime.SpecifyKind(moment, DateTimeKind.Utc)
            : null;
    }

    private DateTime? _approvedAt;

    [JsonIgnore]
    public abstract ListingModuleType StatsListingType { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Views { get; set; }

    public decimal? AverageRating { get; set; }

    public int RatingsCount { get; set; }

    public bool IsFavorite { get; set; }
}

public abstract class DirectoryFilterParams : PaginationParams
{
    public string? Search { get; set; }

    public string? Name { get; set; }

    public DirectorySortBy SortBy { get; set; } = DirectorySortBy.Newest;
}
