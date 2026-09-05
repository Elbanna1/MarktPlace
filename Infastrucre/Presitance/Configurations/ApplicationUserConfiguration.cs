using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.SecondName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.Governorate)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.Center)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.CreatedAt)
            .IsRequired();

        builder.Property(u => u.ProfileImagePath)
            .HasMaxLength(300);

        builder.Property(u => u.ProfileImageUrl)
            .HasMaxLength(500);

        builder.Property(u => u.RefreshToken)
            .HasMaxLength(256);

        builder.Property(u => u.PasswordResetOtpHash)
            .HasMaxLength(128);

        builder.Property(u => u.PasswordResetTokenHash)
            .HasMaxLength(128);

        builder.Property(u => u.Status)
            .IsRequired()
            .HasDefaultValue(Shared.Enums.UserAccountStatus.Active)
            .HasSentinel(default(Shared.Enums.UserAccountStatus));

        builder.Property(u => u.StatusChangedBy)
            .HasMaxLength(450);

        builder.Property(u => u.StatusReason)
            .HasMaxLength(500);

        builder.HasIndex(u => u.Status);

        builder.Property(u => u.ReferralCode)
            .HasMaxLength(Shared.Constants.ReferralCatalog.MaxCodeLength);

        builder.HasIndex(u => u.ReferralCode)
            .IsUnique()
            .HasFilter("[ReferralCode] IS NOT NULL")
            .HasDatabaseName("IX_AspNetUsers_ReferralCode");

        builder.HasIndex(u => u.PhoneNumber)
            .IsUnique()
            .HasFilter("[PhoneNumber] IS NOT NULL");

        builder.HasIndex(u => u.PasswordResetTokenHash)
            .HasFilter("[PasswordResetTokenHash] IS NOT NULL");

        builder.HasIndex(u => u.RefreshToken)
            .HasFilter("[RefreshToken] IS NOT NULL");
    }
}
