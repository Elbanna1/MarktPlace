using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class AdvertisementViewConfiguration : IEntityTypeConfiguration<AdvertisementView>
{
    public void Configure(EntityTypeBuilder<AdvertisementView> builder)
    {
        builder.HasKey(v => v.Id);

        builder.Property(v => v.ViewerUserId).HasMaxLength(450);
        builder.Property(v => v.IpAddress).HasMaxLength(64);

        builder.HasOne(v => v.Advertisement)
            .WithMany(a => a.AdvertisementViews)
            .HasForeignKey(v => v.AdvertisementId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(v => v.AdvertisementId);
    }
}
