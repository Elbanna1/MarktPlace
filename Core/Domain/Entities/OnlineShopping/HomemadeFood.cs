using System.ComponentModel.DataAnnotations.Schema;
using Shared.Enums;

namespace Domain.Entities;

public class HomemadeFood : IModeratedListing, IExpiringListing
{
    public Guid Id { get; set; }

    public string UserId { get; set; } = default!;
    public ApplicationUser User { get; set; } = default!;

    public string ProjectName { get; set; } = default!;

    public string Phone { get; set; } = default!;

    public string WhatsApp { get; set; } = default!;

    public HomemadeFoodSection Section { get; set; }

    public string? OtherSection { get; set; }

    public bool PreparedOnDemand { get; set; }

    public int MinimumOrderQuantity { get; set; }

    public string PreparationTime { get; set; } = default!;

    public bool DeliveryAvailable { get; set; }

    public decimal Price { get; set; }

    public string? Ingredients { get; set; }

    public string? WeightOrSize { get; set; }

    public string? StorageMethod { get; set; }

    public string? AvailableOrderingHours { get; set; }

    public string? AdditionalNotes { get; set; }

    public string? VideoPath { get; set; }

    public string? VideoUrl { get; set; }

    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }

    public ICollection<HomemadeFoodImage> Images { get; set; } = new List<HomemadeFoodImage>();

    public ICollection<HomemadeFoodDeliveryAreaSelection> DeliveryAreas { get; set; } =
        new List<HomemadeFoodDeliveryAreaSelection>();

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

public class HomemadeFoodImage : IListingImage
{
    public Guid Id { get; set; }

    public Guid HomemadeFoodId { get; set; }
    public HomemadeFood HomemadeFood { get; set; } = default!;

    public string FileName { get; set; } = default!;

    public string ImagePath { get; set; } = default!;

    public string ImageUrl { get; set; } = default!;

    public bool IsPrimary { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class HomemadeFoodDeliveryAreaSelection
{
    public Guid HomemadeFoodId { get; set; }
    public HomemadeFood HomemadeFood { get; set; } = default!;

    public HomemadeFoodDeliveryArea DeliveryArea { get; set; }
}
