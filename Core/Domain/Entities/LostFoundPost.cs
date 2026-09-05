using System.ComponentModel.DataAnnotations.Schema;
using Shared.Enums;

namespace Domain.Entities;

public class LostFoundPost : IModeratedListing, IExpiringListing
{
    public Guid Id { get; set; }

    public string UserId { get; set; } = default!;
    public ApplicationUser User { get; set; } = default!;

    public PostType PostType { get; set; }
    public PostStatus Status { get; set; } = PostStatus.Active;

    public string Name { get; set; } = default!;

    public string ItemName { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;

    public string Governorate { get; set; } = default!;
    public string Center { get; set; } = default!;

    public DateOnly? LostDate { get; set; }

    public DateOnly? FoundDate { get; set; }

    public int LikesCount { get; set; }
    public int CommentsCount { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }

    public ICollection<LostFoundImage> Images { get; set; } = new List<LostFoundImage>();
    public ICollection<LostFoundLike> Likes { get; set; } = new List<LostFoundLike>();
    public ICollection<LostFoundComment> Comments { get; set; } = new List<LostFoundComment>();

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
    public string ListingTitle => ItemName;
}
