using System.ComponentModel.DataAnnotations.Schema;
using Shared.Enums;

namespace Domain.Entities;

public class Accessory : IModeratedListing, IExpiringListing
{
    public Guid Id { get; set; }

    public string UserId { get; set; } = default!;
    public ApplicationUser User { get; set; } = default!;

    public string StoreName { get; set; } = default!;

    public string Phone { get; set; } = default!;

    public string WhatsApp { get; set; } = default!;

    public string? LogoPath { get; set; }

    public string? LogoUrl { get; set; }

    public AccessoryType AccessoryType { get; set; }

    public string? OtherAccessoryType { get; set; }

    public AccessoryCategory Category { get; set; }

    public AccessoryMaterial Material { get; set; }

    public string? OtherMaterial { get; set; }

    public string? OtherColor { get; set; }

    public decimal Price { get; set; }

    public bool ShippingAvailable { get; set; }

    public string? VideoPath { get; set; }

    public string? VideoUrl { get; set; }

    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }

    public ICollection<AccessoryImage> Images { get; set; } = new List<AccessoryImage>();

    public ICollection<AccessoryColorSelection> Colors { get; set; } = new List<AccessoryColorSelection>();

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

public class AccessoryImage : IListingImage
{
    public Guid Id { get; set; }

    public Guid AccessoryId { get; set; }
    public Accessory Accessory { get; set; } = default!;

    public string FileName { get; set; } = default!;

    public string ImagePath { get; set; } = default!;

    public string ImageUrl { get; set; } = default!;

    public bool IsPrimary { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class AccessoryColorSelection
{
    public Guid AccessoryId { get; set; }
    public Accessory Accessory { get; set; } = default!;

    public AccessoryColor Color { get; set; }
}
