using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class AdvertisementImageConfiguration : IEntityTypeConfiguration<AdvertisementImage>
{
    public void Configure(EntityTypeBuilder<AdvertisementImage> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.FileName).IsRequired().HasMaxLength(256);
        builder.Property(i => i.ImagePath).IsRequired().HasMaxLength(512);
        builder.Property(i => i.ImageUrl).IsRequired().HasMaxLength(1024);

        builder.HasOne(i => i.Advertisement)
            .WithMany(a => a.Images)
            .HasForeignKey(i => i.AdvertisementId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(i => i.AdvertisementId);
    }
}
