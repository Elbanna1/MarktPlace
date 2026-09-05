using System.ComponentModel.DataAnnotations.Schema;
using Shared.Enums;

namespace Domain.Entities;

public class KidsClothing : IModeratedListing, IExpiringListing
{
    public Guid Id { get; set; }

    public string UserId { get; set; } = default!;
    public ApplicationUser User { get; set; } = default!;

    public string StoreName { get; set; } = default!;

    public KidsClothingSellingMethod SellingMethod { get; set; }

    public KidsClothingType ClothingType { get; set; }

    public string? OtherClothingType { get; set; }

    public KidsClothingBrand Brand { get; set; }

    public string? OtherBrand { get; set; }

    public string? OtherColor { get; set; }

    public KidsClothingCondition Condition { get; set; }

    public decimal Price { get; set; }

    public bool DeliveryAvailable { get; set; }

    public string Governorate { get; set; } = default!;

    public string Center { get; set; } = default!;

    public string Address { get; set; } = default!;

    public string? GoogleMaps { get; set; }

    public string Phone { get; set; } = default!;

    public string WhatsApp { get; set; } = default!;

    public string? Email { get; set; }

    public string? VideoPath { get; set; }

    public string? VideoUrl { get; set; }

    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }

    public ICollection<KidsClothingImage> Images { get; set; } = new List<KidsClothingImage>();

    public ICollection<KidsClothingSizeSelection> Sizes { get; set; } = new List<KidsClothingSizeSelection>();

    public ICollection<KidsClothingColorSelection> Colors { get; set; } = new List<KidsClothingColorSelection>();

    public DateTime? PublishedAt { get; set; }

    public DateTime? ExpireAt { get; set; }

    public DateTime? FirstPublishedAt { get; set; }

    public int RepublishCount { get; set; }

    public ModerationStatus ModerationStatus { get; set; } = ModerationStatus.Pending;

    public DateTime? ModeratedAt { get; set; }

    public string? ModeratedBy { get; set; }

    public ListingRejectionReason? RejectionReason { get; set; }

    public string? ModerationNotes { get; set; }

    [NotMapped]
    public string OwnerUserId => UserId;

    [NotMapped]
    public string ListingTitle => Title;
}

public class KidsClothingImage : IListingImage
{
    public Guid Id { get; set; }

    public Guid KidsClothingId { get; set; }
    public KidsClothing KidsClothing { get; set; } = default!;

    public string FileName { get; set; } = default!;

    public string ImagePath { get; set; } = default!;

    public string ImageUrl { get; set; } = default!;

    public bool IsPrimary { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class KidsClothingSizeSelection
{
    public Guid KidsClothingId { get; set; }
    public KidsClothing KidsClothing { get; set; } = default!;

    public KidsClothingSize Size { get; set; }
}

public class KidsClothingColorSelection
{
    public Guid KidsClothingId { get; set; }
    public KidsClothing KidsClothing { get; set; } = default!;

    public KidsClothingColor Color { get; set; }
}
