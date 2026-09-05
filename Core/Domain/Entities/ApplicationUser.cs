using Microsoft.AspNetCore.Identity;
using Shared.Enums;

namespace Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = default!;

    public string SecondName { get; set; } = default!;

    public string Governorate { get; set; } = default!;

    public string Center { get; set; } = default!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public UserAccountStatus Status { get; set; } = UserAccountStatus.Active;

    public DateTime? StatusChangedAt { get; set; }

    public string? StatusChangedBy { get; set; }

    public string? StatusReason { get; set; }

    public string? ReferralCode { get; set; }

    public string? ProfileImagePath { get; set; }

    public string? ProfileImageUrl { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpiryTime { get; set; }

    public string? PasswordResetOtpHash { get; set; }

    public DateTime? PasswordResetOtpExpiry { get; set; }

    public string? PasswordResetTokenHash { get; set; }

    public bool PasswordResetVerified { get; set; }

    public DateTime? PasswordResetVerifiedExpiry { get; set; }
}
