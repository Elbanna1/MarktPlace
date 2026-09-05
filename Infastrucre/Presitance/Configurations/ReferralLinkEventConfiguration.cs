using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Persistence.Configurations;

public class ReferralLinkEventConfiguration : IEntityTypeConfiguration<ReferralLinkEvent>
{
    public void Configure(EntityTypeBuilder<ReferralLinkEvent> builder)
    {
        builder.ToTable("ReferralLinkEvents");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.ReferrerUserId).IsRequired();

        builder.Property(e => e.ReferralCode)
            .IsRequired()
            .HasMaxLength(ReferralCatalog.CodeLength);

        builder.Property(e => e.EventType).HasConversion<int>();

        builder.HasIndex(e => new { e.ReferrerUserId, e.EventType, e.CreatedAt })
            .HasDatabaseName("IX_ReferralLinkEvents_Referrer_Type_CreatedAt");

        builder.HasIndex(e => e.ReferralCode)
            .HasDatabaseName("IX_ReferralLinkEvents_ReferralCode");

        builder.HasOne(e => e.Referrer)
            .WithMany()
            .HasForeignKey(e => e.ReferrerUserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
