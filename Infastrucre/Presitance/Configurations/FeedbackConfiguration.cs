using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Persistence.Configurations;

public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
{
    public void Configure(EntityTypeBuilder<Feedback> builder)
    {
        builder.ToTable("Feedbacks");

        builder.HasKey(f => f.Id);

        builder.Property(f => f.UserId).IsRequired();
        builder.Property(f => f.Title).IsRequired().HasMaxLength(FeedbackCatalog.MaxTitleLength);
        builder.Property(f => f.Description).IsRequired().HasMaxLength(FeedbackCatalog.MaxDescriptionLength);
        builder.Property(f => f.AdminReply).HasMaxLength(FeedbackCatalog.MaxAdminReplyLength);

        builder.Property(f => f.Type).HasConversion<int>();
        builder.Property(f => f.Status).HasConversion<int>();

        builder.HasOne(f => f.User)
            .WithMany()
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(f => f.Reviewer)
            .WithMany()
            .HasForeignKey(f => f.ReviewedBy)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasIndex(f => new { f.UserId, f.CreatedAt });

        builder.HasIndex(f => new { f.Status, f.CreatedAt });
        builder.HasIndex(f => f.Type);
    }
}

public class FeedbackImageConfiguration : IEntityTypeConfiguration<FeedbackImage>
{
    public void Configure(EntityTypeBuilder<FeedbackImage> builder)
    {
        builder.ToTable("FeedbackImages");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.FileName).IsRequired().HasMaxLength(260);
        builder.Property(i => i.ImagePath).IsRequired().HasMaxLength(500);
        builder.Property(i => i.ImageUrl).IsRequired().HasMaxLength(1000);

        builder.HasOne(i => i.Feedback)
            .WithMany(f => f.Images)
            .HasForeignKey(i => i.FeedbackId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(i => i.FeedbackId);
    }
}
