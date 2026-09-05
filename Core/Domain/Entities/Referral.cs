using Shared.Enums;

namespace Domain.Entities;

public class Referral
{
    public Guid Id { get; set; }

    public string ReferrerUserId { get; set; } = default!;
    public ApplicationUser Referrer { get; set; } = default!;

    public string ReferredUserId { get; set; } = default!;
    public ApplicationUser Referred { get; set; } = default!;

    public string ReferralCode { get; set; } = default!;

    public ReferralStatus Status { get; set; } = ReferralStatus.Completed;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? CompletedAt { get; set; }

    public bool ReferrerNotified { get; set; }
}
