using Shared.Enums;

namespace Shared.DTOs.Advertisements;

public sealed class AdvertisementCardRow
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public decimal? Price { get; set; }
    public bool Negotiable { get; set; }

    public ListingType ListingType { get; set; }

    public AdvertisementStatus Status { get; set; }

    public DateTime? ExpireAt { get; set; }

    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = default!;
    public int SubCategoryId { get; set; }
    public string SubCategoryName { get; set; } = default!;

    public string? Brand { get; set; }
    public string? Model { get; set; }
    public int? ManufacturingYear { get; set; }
    public string? BusinessName { get; set; }

    public string Governorate { get; set; } = default!;
    public string Center { get; set; } = default!;

    public int Views { get; set; }

    public DateTime CreatedAt { get; set; }

    public string OwnerId { get; set; } = default!;
    public string? OwnerFirstName { get; set; }
    public string? OwnerSecondName { get; set; }
    public string? OwnerPhoneNumber { get; set; }
    public string? OwnerGovernorate { get; set; }
    public string? OwnerCenter { get; set; }
    public string? OwnerProfileImageUrl { get; set; }
    public DateTime OwnerCreatedAt { get; set; }

    public List<AdvertisementImageDto> Images { get; set; } = new();
    public List<FeatureDto> Features { get; set; } = new();
}
