using System.ComponentModel.DataAnnotations.Schema;
using Shared.Enums;

namespace Domain.Entities;

public class MenClothing : IModeratedListing, IExpiringListing
{
    public Guid Id { get; set; }

    public string UserId { get; set; } = default!;
    public ApplicationUser User { get; set; } = default!;

    public string StoreName { get; set; } = default!;

    public MenClothingSellingMethod SellingMethod { get; set; }

    public MenClothingType ClothingType { get; set; }

    public string? OtherClothingType { get; set; }

    public MenClothingBrand Brand { get; set; }

    public string? OtherBrand { get; set; }

    public string? OtherColor { get; set; }

    public MenClothingCondition Condition { get; set; }

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

    public ICollection<MenClothingImage> Images { get; set; } = new List<MenClothingImage>();

    public ICollection<MenClothingSizeSelection> Sizes { get; set; } = new List<MenClothingSizeSelection>();

    public ICollection<MenClothingColorSelection> Colors { get; set; } = new List<MenClothingColorSelection>();

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

public class MenClothingImage : IListingImage
{
    public Guid Id { get; set; }

    public Guid MenClothingId { get; set; }
    public MenClothing MenClothing { get; set; } = default!;

    public string FileName { get; set; } = default!;

    public string ImagePath { get; set; } = default!;

    public string ImageUrl { get; set; } = default!;

    public bool IsPrimary { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class MenClothingSizeSelection
{
    public Guid MenClothingId { get; set; }
    public MenClothing MenClothing { get; set; } = default!;

    public MenClothingSize Size { get; set; }
}

public class MenClothingColorSelection
{
    public Guid MenClothingId { get; set; }
    public MenClothing MenClothing { get; set; } = default!;

    public MenClothingColor Color { get; set; }
}
