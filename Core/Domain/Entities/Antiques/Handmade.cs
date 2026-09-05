using System.ComponentModel.DataAnnotations.Schema;
using Shared.Enums;

namespace Domain.Entities;

public class Handmade : IModeratedListing, IExpiringListing
{
    public Guid Id { get; set; }

    public string UserId { get; set; } = default!;
    public ApplicationUser User { get; set; } = default!;

    public string SellerName { get; set; } = default!;

    public string Phone { get; set; } = default!;

    public string? WhatsApp { get; set; }

    public string ProductName { get; set; } = default!;

    public HandmadeType HandmadeType { get; set; }

    public string? OtherType { get; set; }

    public string Material { get; set; } = default!;

    public bool IsFullyHandmade { get; set; }

    public bool CustomOrder { get; set; }

    public string? ProductionTime { get; set; }

    public string? Size { get; set; }

    public string? OtherColor { get; set; }

    public decimal Price { get; set; }

    public bool Negotiable { get; set; }

    public string Governorate { get; set; } = default!;

    public string Center { get; set; } = default!;

    public string Address { get; set; } = default!;

    public string? GoogleMaps { get; set; }

    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }

    public ICollection<HandmadeColorSelection> Colors { get; set; } = new List<HandmadeColorSelection>();

    public ICollection<HandmadeImage> Images { get; set; } = new List<HandmadeImage>();

    public HandmadeVideo? Video { get; set; }

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

public class HandmadeColorSelection
{
    public Guid Id { get; set; }

    public Guid HandmadeId { get; set; }
    public Handmade Handmade { get; set; } = default!;

    public HandmadeColor Color { get; set; }
}
