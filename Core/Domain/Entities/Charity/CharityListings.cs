using System.ComponentModel.DataAnnotations.Schema;
using Shared.Enums;

namespace Domain.Entities;

public abstract class CharityListing : IModeratedListing, IExpiringListing
{
    public Guid Id { get; set; }

    public string UserId { get; set; } = default!;
    public ApplicationUser User { get; set; } = default!;

    public string Phone { get; set; } = default!;

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public bool IsResponsibilityAccepted { get; set; }

    public DateTime? ResponsibilityAcceptedAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }

    public int ViewCount { get; set; }

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
    public abstract string ListingTitle { get; }
}

public class Rescue : CharityListing
{
    public string RescuerName { get; set; } = default!;

    public string Address { get; set; } = default!;

    public string Details { get; set; } = default!;

    public ICollection<RescueImage> Images { get; set; } = new List<RescueImage>();

    [NotMapped]
    public override string ListingTitle => RescuerName;
}

public class BloodRequest : CharityListing
{
    public string RequesterName { get; set; } = default!;

    public BloodGroup BloodGroup { get; set; }

    public string Governorate { get; set; } = default!;

    public string Center { get; set; } = default!;

    public string HospitalName { get; set; } = default!;

    public string Address { get; set; } = default!;

    public string Details { get; set; } = default!;

    public ICollection<BloodRequestImage> Images { get; set; } = new List<BloodRequestImage>();

    [NotMapped]
    public override string ListingTitle => RequesterName;
}

public class AskConsult : CharityListing
{
    public AskConsultCategory Category { get; set; }

    public string? OtherCategory { get; set; }

    public string AskerName { get; set; } = default!;

    public string Title { get; set; } = default!;

    public string Question { get; set; } = default!;

    public string? Governorate { get; set; }

    public string? Center { get; set; }

    public int LikesCount { get; set; }

    public int CommentsCount { get; set; }

    public ICollection<AskConsultImage> Images { get; set; } = new List<AskConsultImage>();

    public ICollection<AskConsultLike> Likes { get; set; } = new List<AskConsultLike>();

    public ICollection<AskConsultComment> Comments { get; set; } = new List<AskConsultComment>();

    [NotMapped]
    public override string ListingTitle => Title;
}

public class RescueImage : IOrderedListingImage
{
    public Guid Id { get; set; }

    public Guid RescueId { get; set; }
    public Rescue Rescue { get; set; } = default!;

    public string FileName { get; set; } = default!;
    public string ImagePath { get; set; } = default!;
    public string ImageUrl { get; set; } = default!;

    public bool IsPrimary { get; set; }

    public int SortOrder { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class BloodRequestImage : IOrderedListingImage
{
    public Guid Id { get; set; }

    public Guid BloodRequestId { get; set; }
    public BloodRequest BloodRequest { get; set; } = default!;

    public string FileName { get; set; } = default!;
    public string ImagePath { get; set; } = default!;
    public string ImageUrl { get; set; } = default!;

    public bool IsPrimary { get; set; }

    public int SortOrder { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class AskConsultImage : IOrderedListingImage
{
    public Guid Id { get; set; }

    public Guid AskConsultId { get; set; }
    public AskConsult AskConsult { get; set; } = default!;

    public string FileName { get; set; } = default!;
    public string ImagePath { get; set; } = default!;
    public string ImageUrl { get; set; } = default!;

    public bool IsPrimary { get; set; }

    public int SortOrder { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class AskConsultLike
{
    public long Id { get; set; }

    public Guid AskConsultId { get; set; }
    public AskConsult AskConsult { get; set; } = default!;

    public string UserId { get; set; } = default!;
    public ApplicationUser User { get; set; } = default!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class AskConsultComment
{
    public Guid Id { get; set; }

    public Guid AskConsultId { get; set; }
    public AskConsult AskConsult { get; set; } = default!;

    public string UserId { get; set; } = default!;
    public ApplicationUser User { get; set; } = default!;

    public string Comment { get; set; } = default!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}
