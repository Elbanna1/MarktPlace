using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class AdvertisementFeatureConfiguration : IEntityTypeConfiguration<AdvertisementFeature>
{
    public void Configure(EntityTypeBuilder<AdvertisementFeature> builder)
    {
        builder.HasKey(af => new { af.AdvertisementId, af.FeatureId });

        builder.HasOne(af => af.Advertisement)
            .WithMany(a => a.AdvertisementFeatures)
            .HasForeignKey(af => af.AdvertisementId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(af => af.Feature)
            .WithMany(f => f.AdvertisementFeatures)
            .HasForeignKey(af => af.FeatureId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
