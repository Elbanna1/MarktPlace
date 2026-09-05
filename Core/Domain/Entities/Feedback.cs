using Shared.Enums;

namespace Domain.Entities;

public class Feedback
{
    public Guid Id { get; set; }

    public string UserId { get; set; } = default!;
    public ApplicationUser User { get; set; } = default!;

    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;

    public FeedbackType Type { get; set; }

    public int? Rating { get; set; }

    public FeedbackStatus Status { get; set; } = FeedbackStatus.New;

    public string? AdminReply { get; set; }

    public string? ReviewedBy { get; set; }
    public ApplicationUser? Reviewer { get; set; }

    public DateTime? ReviewedAt { get; set; }

    public DateTime? ClosedAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public ICollection<FeedbackImage> Images { get; set; } = new List<FeedbackImage>();
}
