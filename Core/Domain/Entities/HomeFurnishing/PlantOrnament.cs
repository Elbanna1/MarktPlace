using System.ComponentModel.DataAnnotations.Schema;
using Shared.Enums;

namespace Domain.Entities;

public class PlantOrnament : IModeratedListing, IExpiringListing
{
    public Guid Id { get; set; }

    public string UserId { get; set; } = default!;
    public ApplicationUser User { get; set; } = default!;

    public string SellerName { get; set; } = default!;

    public string Phone { get; set; } = default!;

    public string? WhatsApp { get; set; }

    public string? Email { get; set; }

    public string ProductName { get; set; } = default!;

    public PlantOrnamentProductType ProductType { get; set; }

    public string? OtherProductType { get; set; }

    public PlantOrnamentSuitableFor SuitableFor { get; set; }

    public decimal? Height { get; set; }

    public decimal Price { get; set; }

    public bool Negotiable { get; set; }

    public string Governorate { get; set; } = default!;

    public string Center { get; set; } = default!;

    public string Address { get; set; } = default!;

    public string? GoogleMaps { get; set; }

    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;

    public bool IsFeatured { get; set; }
    public bool IsPremium { get; set; }
    public bool IsUrgent { get; set; }
    public int ViewCount { get; set; }

    public string? VideoPath { get; set; }
    public string? VideoUrl { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }

    public ICollection<PlantOrnamentImage> Images { get; set; } = new List<PlantOrnamentImage>();

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

public class PlantOrnamentImage : IOrderedListingImage
{
    public Guid Id { get; set; }

    public Guid PlantOrnamentId { get; set; }
    public PlantOrnament PlantOrnament { get; set; } = default!;

    public string FileName { get; set; } = default!;
    public string ImagePath { get; set; } = default!;
    public string ImageUrl { get; set; } = default!;
    public bool IsPrimary { get; set; }

    public int SortOrder { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
