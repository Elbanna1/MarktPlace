using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Persistence.Configurations;

public class BannerBookingConfiguration : IEntityTypeConfiguration<BannerBooking>
{
    public void Configure(EntityTypeBuilder<BannerBooking> builder)
    {
        builder.ToTable("BannerBookings");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Title).IsRequired().HasMaxLength(BannerBookingCatalog.MaxTitleLength);
        builder.Property(b => b.Description).HasMaxLength(BannerBookingCatalog.MaxDescriptionLength);
        builder.Property(b => b.ButtonText).IsRequired().HasMaxLength(BannerBookingCatalog.MaxButtonTextLength);
        builder.Property(b => b.TargetUrl).IsRequired().HasMaxLength(BannerBookingCatalog.MaxTargetUrlLength);

        builder.Property(b => b.DesktopImageFileName).IsRequired().HasMaxLength(255);
        builder.Property(b => b.DesktopImagePath).IsRequired().HasMaxLength(500);
        builder.Property(b => b.DesktopImageUrl).IsRequired().HasMaxLength(1000);

        builder.Property(b => b.MobileImageFileName).IsRequired().HasMaxLength(255);
        builder.Property(b => b.MobileImagePath).IsRequired().HasMaxLength(500);
        builder.Property(b => b.MobileImageUrl).IsRequired().HasMaxLength(1000);

        builder.Property(b => b.UserId).IsRequired();
        builder.Property(b => b.AdvertiserName).IsRequired().HasMaxLength(BannerBookingCatalog.MaxAdvertiserNameLength);
        builder.Property(b => b.PhoneNumber).IsRequired().HasMaxLength(BannerBookingCatalog.MaxPhoneLength);
        builder.Property(b => b.WhatsAppNumber).HasMaxLength(BannerBookingCatalog.MaxPhoneLength);
        builder.Property(b => b.Email).HasMaxLength(BannerBookingCatalog.MaxEmailLength);

        builder.Property(b => b.Price).HasColumnType("decimal(18,2)");
        builder.Property(b => b.Currency).IsRequired().HasMaxLength(3);

        builder.Property(b => b.PaymentMethodNameSnapshot).HasMaxLength(100);
        builder.Property(b => b.PaymentInfoSnapshot).HasMaxLength(200);
        builder.Property(b => b.AccountNameSnapshot).HasMaxLength(150);
        builder.Property(b => b.InstructionsSnapshot).HasMaxLength(1000);

        builder.Property(b => b.PaymentProofFileName).IsRequired().HasMaxLength(255);
        builder.Property(b => b.PaymentProofPath).IsRequired().HasMaxLength(500);
        builder.Property(b => b.PaymentProofUrl).IsRequired().HasMaxLength(1000);
        builder.Property(b => b.PaymentRejectionNotes).HasMaxLength(BannerBookingCatalog.MaxRejectionNotesLength);

        builder.Property(b => b.RejectionNotes).HasMaxLength(BannerBookingCatalog.MaxRejectionNotesLength);

        builder.Property(b => b.Location).HasConversion<int>();
        builder.Property(b => b.Status).HasConversion<int>();
        builder.Property(b => b.PaymentStatus).HasConversion<int>();
        builder.Property(b => b.RejectionReason).HasConversion<int>();

        builder.Ignore(b => b.OccupiesSlot);

        builder.HasOne(b => b.User)
            .WithMany()
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(b => b.Reviewer)
            .WithMany()
            .HasForeignKey(b => b.ReviewedBy)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(b => b.PlacementSetting)
            .WithMany(p => p.Bookings)
            .HasForeignKey(b => b.PlacementSettingId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.PaymentMethod)
            .WithMany()
            .HasForeignKey(b => b.PaymentMethodId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.Category)
            .WithMany()
            .HasForeignKey(b => b.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.SubCategory)
            .WithMany()
            .HasForeignKey(b => b.SubCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(b => new { b.Location, b.SlotNumber, b.Status, b.StartDate, b.EndDate })
            .HasDatabaseName("IX_BannerBookings_SlotWindow");

        builder.HasIndex(b => new { b.CategoryId, b.SubCategoryId, b.Status, b.StartDate, b.EndDate })
            .HasDatabaseName("IX_BannerBookings_SubCategoryWindow");

        builder.HasIndex(b => new { b.Status, b.SubmittedAt });

        builder.HasIndex(b => new { b.UserId, b.SubmittedAt });

        builder.HasIndex(b => new { b.Status, b.StartDate, b.EndDate });

        builder.HasIndex(b => b.PaymentStatus);
        builder.HasIndex(b => b.PaymentMethodId);
    }
}
