using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Persistence.Configurations;

public class CommercialShopConfiguration : IEntityTypeConfiguration<CommercialShop>
{
    public void Configure(EntityTypeBuilder<CommercialShop> builder)
    {
        DirectoryListingMapping.Apply<CommercialShop, CommercialShopImage>(builder, "CommercialShops");

        builder.Property(x => x.ActivityType).HasConversion<int>();
        builder.Property(x => x.OtherActivityType).HasMaxLength(GovernorateGuideCatalog.MaxOtherActivityTypeLength);

        builder.HasIndex(x => new { x.ModerationStatus, x.ActivityType, x.CreatedAt })
            .IncludeProperties(x => new { x.IsDeleted, x.ExpireAt });
    }
}

public class CommercialShopImageConfiguration : IEntityTypeConfiguration<CommercialShopImage>
{
    public void Configure(EntityTypeBuilder<CommercialShopImage> builder)
    {
        DirectoryListingMapping.ApplyImage(builder, "CommercialShopImages");
        builder.HasOne(x => x.Listing).WithMany(x => x.Images).HasForeignKey(x => x.ListingId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasQueryFilter(x => !x.Listing.IsDeleted);
    }
}

public class GeneralServiceConfiguration : IEntityTypeConfiguration<GeneralService>
{
    public void Configure(EntityTypeBuilder<GeneralService> builder)
    {
        DirectoryListingMapping.Apply<GeneralService, GeneralServiceImage>(builder, "GeneralServices");

        builder.Property(x => x.ServiceType).HasConversion<int>();
        builder.Property(x => x.ServiceSubtype).HasConversion<int>();

        builder.HasIndex(x => new { x.ModerationStatus, x.ServiceType, x.ServiceSubtype, x.CreatedAt })
            .IncludeProperties(x => new { x.IsDeleted, x.ExpireAt });
    }
}

public class GeneralServiceImageConfiguration : IEntityTypeConfiguration<GeneralServiceImage>
{
    public void Configure(EntityTypeBuilder<GeneralServiceImage> builder)
    {
        DirectoryListingMapping.ApplyImage(builder, "GeneralServiceImages");
        builder.HasOne(x => x.Listing).WithMany(x => x.Images).HasForeignKey(x => x.ListingId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasQueryFilter(x => !x.Listing.IsDeleted);
    }
}

public class ResortHotelAmusementConfiguration : IEntityTypeConfiguration<ResortHotelAmusement>
{
    public void Configure(EntityTypeBuilder<ResortHotelAmusement> builder) =>
        DirectoryListingMapping.Apply<ResortHotelAmusement, ResortHotelAmusementImage>(builder, "ResortsHotelsAndAmusement");
}

public class ResortHotelAmusementImageConfiguration : IEntityTypeConfiguration<ResortHotelAmusementImage>
{
    public void Configure(EntityTypeBuilder<ResortHotelAmusementImage> builder)
    {
        DirectoryListingMapping.ApplyImage(builder, "ResortHotelAmusementImages");
        builder.HasOne(x => x.Listing).WithMany(x => x.Images).HasForeignKey(x => x.ListingId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasQueryFilter(x => !x.Listing.IsDeleted);
    }
}
