using Shared.Enums;

namespace Domain.Entities;

public class ReferralLinkEvent
{
    public Guid Id { get; set; }

    public string ReferrerUserId { get; set; } = default!;
    public ApplicationUser Referrer { get; set; } = default!;

    public string ReferralCode { get; set; } = default!;

    public ReferralLinkEventType EventType { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
