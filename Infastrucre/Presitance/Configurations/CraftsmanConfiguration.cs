using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class CraftsmanConfiguration : IEntityTypeConfiguration<Craftsman>
{
    public void Configure(EntityTypeBuilder<Craftsman> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name).IsRequired().HasMaxLength(150);
        builder.Property(c => c.OtherSpecialization).HasMaxLength(150);
        builder.Property(c => c.Governorate).IsRequired().HasMaxLength(50);
        builder.Property(c => c.Center).IsRequired().HasMaxLength(50);
        builder.Property(c => c.Address).IsRequired().HasMaxLength(300);
        builder.Property(c => c.GoogleMapsUrl).HasMaxLength(1000);
        builder.Property(c => c.PhoneNumber).IsRequired().HasMaxLength(20);
        builder.Property(c => c.WhatsApp).IsRequired().HasMaxLength(20);
        builder.Property(c => c.Email).HasMaxLength(256);
        builder.Property(c => c.AdTitle).IsRequired().HasMaxLength(150);
        builder.Property(c => c.AdDescription).IsRequired().HasMaxLength(4000);
        builder.Property(c => c.UserId).IsRequired();

        builder.Property(c => c.Specialization).HasConversion<int>();
        builder.Property(c => c.ExperienceLevel).HasConversion<int>();

        builder.HasOne(c => c.User)
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(c => c.Images)
            .WithOne(i => i.Craftsman)
            .HasForeignKey(i => i.CraftsmanId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(QueryFilterNames.SoftDelete, c => !c.IsDeleted);

        builder.HasIndex(c => c.CreatedAt);
        builder.HasIndex(c => c.Specialization);
        builder.HasIndex(c => c.ExperienceLevel);
        builder.HasIndex(c => c.Center);
        builder.HasIndex(c => c.UserId);
    }
}
