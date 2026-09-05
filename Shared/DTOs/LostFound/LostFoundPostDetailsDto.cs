using Shared.DTOs.Listings;
using System.Text.Json.Serialization;
using Shared.Enums;

namespace Shared.DTOs.LostFound;

public class LostFoundPostDetailsDto : IListingViews
{
    public Guid Id { get; set; }

    public PostType PostType { get; set; }
    public PostStatus Status { get; set; }

    public string Name { get; set; } = default!;

    public string ItemName { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;

    public string Governorate { get; set; } = default!;
    public string Center { get; set; } = default!;

    public DateOnly? LostDate { get; set; }

    public DateOnly? FoundDate { get; set; }

    public List<LostFoundImageDto> Images { get; set; } = new();

    public int LikesCount { get; set; }
    public int CommentsCount { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Views { get; set; }

    public int FavoriteCount { get; set; }

    public decimal? AverageRating { get; set; }

    public int RatingsCount { get; set; }

    public bool IsFavorite { get; set; }

    public bool IsLikedByCurrentUser { get; set; }

    public ListingModuleType TypeId { get; set; }

    public int CategoryId { get; set; }

    public int SubCategoryId { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    ListingModuleType IListingViews.ViewsListingType => TypeId;

    Guid IListingViews.ViewsListingId => Id;
}
