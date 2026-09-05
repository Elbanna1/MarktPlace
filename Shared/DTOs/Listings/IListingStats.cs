using System.Text.Json.Serialization;
using Shared.Enums;

namespace Shared.DTOs.Listings;

public interface IListingStats : IListingViews
{
    [JsonIgnore]
    ListingModuleType StatsListingType { get; }

    Guid Id { get; }

    decimal? AverageRating { get; set; }

    int RatingsCount { get; set; }

    bool IsFavorite { get; set; }

    ListingModuleType IListingViews.ViewsListingType => StatsListingType;

    Guid IListingViews.ViewsListingId => Id;
}
