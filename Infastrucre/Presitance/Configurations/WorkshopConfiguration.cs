using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class WorkshopConfiguration : IEntityTypeConfiguration<Workshop>
{
    public void Configure(EntityTypeBuilder<Workshop> builder)
    {
        builder.HasKey(w => w.Id);

        builder.Property(w => w.Name).IsRequired().HasMaxLength(150);
        builder.Property(w => w.OtherWorkshopType).HasMaxLength(150);
        builder.Property(w => w.Governorate).IsRequired().HasMaxLength(50);
        builder.Property(w => w.Center).IsRequired().HasMaxLength(50);
        builder.Property(w => w.Address).IsRequired().HasMaxLength(300);
        builder.Property(w => w.GoogleMapsUrl).HasMaxLength(1000);
        builder.Property(w => w.PhoneNumber).IsRequired().HasMaxLength(20);
        builder.Property(w => w.WhatsApp).IsRequired().HasMaxLength(20);
        builder.Property(w => w.Email).HasMaxLength(256);
        builder.Property(w => w.AdTitle).IsRequired().HasMaxLength(150);
        builder.Property(w => w.AdDescription).IsRequired().HasMaxLength(4000);
        builder.Property(w => w.UserId).IsRequired();

        builder.Property(w => w.WorkshopType).HasConversion<int>();

        builder.HasOne(w => w.User)
            .WithMany()
            .HasForeignKey(w => w.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(w => w.Images)
            .WithOne(i => i.Workshop)
            .HasForeignKey(i => i.WorkshopId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(QueryFilterNames.SoftDelete, w => !w.IsDeleted);

        builder.HasIndex(w => w.CreatedAt);
        builder.HasIndex(w => w.WorkshopType);
        builder.HasIndex(w => w.Center);
        builder.HasIndex(w => w.UserId);
    }
}
