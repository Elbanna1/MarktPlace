using System.ComponentModel.DataAnnotations.Schema;
using Shared.Enums;

namespace Domain.Entities;

public class Craftsman : IModeratedListing, IExpiringListing
{
    public Guid Id { get; set; }

    public string UserId { get; set; } = default!;
    public ApplicationUser User { get; set; } = default!;

    public string Name { get; set; } = default!;

    public CraftsmanSpecialization Specialization { get; set; }

    public string? OtherSpecialization { get; set; }

    public ExperienceLevel ExperienceLevel { get; set; }

    public string Governorate { get; set; } = default!;

    public string Center { get; set; } = default!;

    public string Address { get; set; } = default!;

    public string? GoogleMapsUrl { get; set; }

    public string PhoneNumber { get; set; } = default!;

    public string WhatsApp { get; set; } = default!;

    public string? Email { get; set; }

    public string AdTitle { get; set; } = default!;

    public string AdDescription { get; set; } = default!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }

    public ICollection<CraftsmanImage> Images { get; set; } = new List<CraftsmanImage>();

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
    public string ListingTitle => AdTitle;
}
