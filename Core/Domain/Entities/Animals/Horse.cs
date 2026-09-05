using System.ComponentModel.DataAnnotations.Schema;
using Shared.Enums;

namespace Domain.Entities;

public class Horse : IModeratedListing, IExpiringListing
{
    public Guid Id { get; set; }

    public string UserId { get; set; } = default!;
    public ApplicationUser User { get; set; } = default!;

    public string SellerName { get; set; } = default!;

    public HorseBreed Breed { get; set; }

    public string? OtherBreed { get; set; }

    public HorsePurpose Purpose { get; set; }

    public HorseAge Age { get; set; }

    public HorseGender Gender { get; set; }

    public HorseHealthStatus HealthStatus { get; set; }

    public HorseTrainingLevel? TrainingLevel { get; set; }

    public HorseVaccination? Vaccination { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public bool Negotiable { get; set; }

    public string Address { get; set; } = default!;

    public string? GoogleMaps { get; set; }

    public string Phone { get; set; } = default!;

    public string? WhatsApp { get; set; }

    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }

    public ICollection<HorseImage> Images { get; set; } = new List<HorseImage>();

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
