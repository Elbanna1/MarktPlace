using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class RestaurantAndCafeConfiguration : IEntityTypeConfiguration<RestaurantAndCafe>
{
    public void Configure(EntityTypeBuilder<RestaurantAndCafe> builder) =>
        DirectoryListingMapping.Apply<RestaurantAndCafe, RestaurantAndCafeImage>(builder, "RestaurantsAndCafes");
}

public class RestaurantAndCafeImageConfiguration : IEntityTypeConfiguration<RestaurantAndCafeImage>
{
    public void Configure(EntityTypeBuilder<RestaurantAndCafeImage> builder)
    {
        DirectoryListingMapping.ApplyImage(builder, "RestaurantAndCafeImages");
        builder.HasOne(x => x.Listing).WithMany(x => x.Images).HasForeignKey(x => x.ListingId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasQueryFilter(x => !x.Listing.IsDeleted);
    }
}
