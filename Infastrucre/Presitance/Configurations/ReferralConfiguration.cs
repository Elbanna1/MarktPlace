using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Persistence.Configurations;

public class ReferralConfiguration : IEntityTypeConfiguration<Referral>
{
    public void Configure(EntityTypeBuilder<Referral> builder)
    {
        builder.ToTable("Referrals");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.ReferrerUserId).IsRequired().HasMaxLength(450);
        builder.Property(r => r.ReferredUserId).IsRequired().HasMaxLength(450);

        builder.Property(r => r.ReferralCode)
            .IsRequired()
            .HasMaxLength(ReferralCatalog.MaxCodeLength);

        builder.Property(r => r.Status).HasConversion<int>();

        builder.Property(r => r.ReferrerNotified).HasDefaultValue(false);

        builder.HasIndex(r => r.ReferredUserId)
            .IsUnique()
            .HasDatabaseName("IX_Referrals_ReferredUser");

        builder.HasIndex(r => new { r.ReferrerUserId, r.ReferredUserId })
            .IsUnique()
            .HasDatabaseName("IX_Referrals_Referrer_ReferredUser");

        builder.HasIndex(r => new { r.ReferrerUserId, r.Status, r.CreatedAt })
            .HasDatabaseName("IX_Referrals_Referrer_Status_CreatedAt");

        builder.HasIndex(r => new { r.Status, r.CreatedAt })
            .HasDatabaseName("IX_Referrals_Status_CreatedAt");

        builder.HasIndex(r => r.ReferralCode)
            .HasDatabaseName("IX_Referrals_ReferralCode");

        builder.ToTable(table => table.HasCheckConstraint(
            "CK_Referrals_NoSelfReferral",
            "[ReferrerUserId] <> [ReferredUserId]"));

        builder.HasOne(r => r.Referrer)
            .WithMany()
            .HasForeignKey(r => r.ReferrerUserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(r => r.Referred)
            .WithMany()
            .HasForeignKey(r => r.ReferredUserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
