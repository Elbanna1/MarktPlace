using System.ComponentModel.DataAnnotations.Schema;
using Shared.Enums;

namespace Domain.Entities;

public class FruitVegetableMerchant : IModeratedListing, IExpiringListing
{
    public Guid Id { get; set; }

    public string UserId { get; set; } = default!;
    public ApplicationUser User { get; set; } = default!;

    public string StallName { get; set; } = default!;

    public string MerchantName { get; set; } = default!;

    public string Phone { get; set; } = default!;

    public string? WhatsApp { get; set; }

    public string Address { get; set; } = default!;

    public string? GoogleMaps { get; set; }

    public string ProductName { get; set; } = default!;

    public MerchantSaleType SaleType { get; set; }

    public string ProductDetails { get; set; } = default!;

    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }

    public ICollection<FruitVegetableMerchantImage> Images { get; set; } = new List<FruitVegetableMerchantImage>();

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
