using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Persistence.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.UserId).IsRequired();
        builder.Property(p => p.Amount).HasColumnType("decimal(18,2)");
        builder.Property(p => p.Currency).IsRequired().HasMaxLength(3);
        builder.Property(p => p.ScreenshotUrl).IsRequired().HasMaxLength(1000);
        builder.Property(p => p.ScreenshotPath).IsRequired().HasMaxLength(500);
        builder.Property(p => p.Notes).HasMaxLength(PaymentCatalog.MaxNotesLength);
        builder.Property(p => p.RejectReason).HasMaxLength(PaymentCatalog.MaxRejectReasonLength);

        builder.Property(p => p.PaymentMethodNameSnapshot).HasMaxLength(100);
        builder.Property(p => p.PaymentInfoSnapshot).HasMaxLength(200);
        builder.Property(p => p.AccountNameSnapshot).HasMaxLength(150);
        builder.Property(p => p.InstructionsSnapshot).HasMaxLength(1000);

        builder.Property(p => p.Status).HasConversion<int>();
        builder.Property(p => p.Purpose).HasConversion<int>();

        builder.Property(p => p.TargetType).HasMaxLength(50);

        builder.HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(p => p.Reviewer)
            .WithMany()
            .HasForeignKey(p => p.ApprovedBy)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(p => p.PaymentMethod)
            .WithMany(m => m.Payments)
            .HasForeignKey(p => p.PaymentMethodId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(p => new { p.UserId, p.SubmittedAt });

        builder.HasIndex(p => new { p.Status, p.SubmittedAt });
        builder.HasIndex(p => p.PaymentMethodId);

        builder.HasIndex(p => new { p.TargetType, p.TargetId });
    }
}
