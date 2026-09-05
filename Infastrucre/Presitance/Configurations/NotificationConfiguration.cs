using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.HasKey(n => n.Id);

        builder.Property(n => n.Title).IsRequired().HasMaxLength(150);
        builder.Property(n => n.Message).IsRequired().HasMaxLength(1000);
        builder.Property(n => n.Type).HasConversion<int>();
        builder.Property(n => n.Action).HasConversion<int>();
        builder.Property(n => n.ReferenceType).HasMaxLength(50);

        builder.Property(n => n.Icon).HasMaxLength(16);
        builder.Property(n => n.EntityName).HasMaxLength(150);
        builder.Property(n => n.DeepLink).HasMaxLength(500);

        builder.Property(n => n.ListingType).HasConversion<int?>();
        builder.Property(n => n.ImageUrl).HasMaxLength(500);

        builder.Property(n => n.CategoryName).HasMaxLength(150);
        builder.Property(n => n.SubCategoryName).HasMaxLength(150);
        builder.Property(n => n.ListingTitle).HasMaxLength(300);
        builder.Property(n => n.OwnerId).HasMaxLength(450);
        builder.Property(n => n.OwnerName).HasMaxLength(150);

        builder.HasOne(n => n.User)
            .WithMany()
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(n => new { n.UserId, n.CreatedAt });

        builder.HasIndex(n => new { n.UserId, n.IsRead });

        builder.HasIndex(n => new { n.UserId, n.Type, n.ReferenceId });

        builder.HasIndex(n => new { n.UserId, n.ReferenceType });
    }
}
