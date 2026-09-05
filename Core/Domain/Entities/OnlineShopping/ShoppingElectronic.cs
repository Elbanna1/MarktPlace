using System.ComponentModel.DataAnnotations.Schema;
using Shared.Enums;

namespace Domain.Entities;

public class ShoppingElectronic : IModeratedListing, IExpiringListing
{
    public Guid Id { get; set; }

    public string UserId { get; set; } = default!;
    public ApplicationUser User { get; set; } = default!;

    public string StoreName { get; set; } = default!;

    public string Phone { get; set; } = default!;

    public string WhatsApp { get; set; } = default!;

    public ShoppingElectronicSection Section { get; set; }

    public string? OtherSection { get; set; }

    public string Brand { get; set; } = default!;

    public ShoppingElectronicCompatibility CompatibleWith { get; set; }

    public ShoppingElectronicCondition ProductCondition { get; set; }

    public ShoppingElectronicWarranty Warranty { get; set; }

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

    public ICollection<ShoppingElectronicImage> Images { get; set; } = new List<ShoppingElectronicImage>();

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

public class ShoppingElectronicImage : IListingImage
{
    public Guid Id { get; set; }

    public Guid ShoppingElectronicId { get; set; }
    public ShoppingElectronic ShoppingElectronic { get; set; } = default!;

    public string FileName { get; set; } = default!;

    public string ImagePath { get; set; } = default!;

    public string ImageUrl { get; set; } = default!;

    public bool IsPrimary { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
