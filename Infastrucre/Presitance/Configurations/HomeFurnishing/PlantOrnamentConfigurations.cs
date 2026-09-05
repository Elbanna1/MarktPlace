using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;
using Shared.Enums;

namespace Persistence.Configurations;

public class PlantOrnamentConfiguration : IEntityTypeConfiguration<PlantOrnament>
{
    public void Configure(EntityTypeBuilder<PlantOrnament> builder)
    {
        builder.ToTable("PlantOrnaments");

        builder.HasKey(x => x.Id);

        HomeFurnishingListingMapping.ApplyCommonColumns(builder);

        builder.Property(x => x.OtherProductType).HasMaxLength(150);

        builder.Property(x => x.Height).HasColumnType("decimal(10,2)");

        builder.Property(x => x.ProductType).HasConversion<int>();
        builder.Property(x => x.SuitableFor).HasConversion<int>();

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(x => x.Images)
            .WithOne(i => i.PlantOrnament)
            .HasForeignKey(i => i.PlantOrnamentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(QueryFilterNames.SoftDelete, x => !x.IsDeleted);

        HomeFurnishingListingMapping.ApplyCommonIndexes(builder);

        builder.HasIndex(x => x.ProductType);
        builder.HasIndex(x => x.SuitableFor);
    }
}

public class PlantOrnamentImageConfiguration : HomeFurnishingImageConfiguration<PlantOrnamentImage>
{
    protected override string TableName => "PlantOrnamentImages";

    protected override string ListingIdPropertyName => nameof(PlantOrnamentImage.PlantOrnamentId);

    protected override void ApplySoftDeleteFilter(EntityTypeBuilder<PlantOrnamentImage> builder) =>
        builder.HasQueryFilter(i => !i.PlantOrnament.IsDeleted);
}

public class PlantOrnamentProductTypeLookupConfiguration
    : HomeFurnishingLookupConfiguration<PlantOrnamentProductTypeLookup, PlantOrnamentProductType>
{
    protected override string TableName => "PlantOrnamentProductTypes";
    protected override IReadOnlyList<HomeFurnishingLookupEntry<PlantOrnamentProductType>> Entries =>
        PlantOrnamentCatalog.ProductTypes;
}

public class PlantOrnamentSuitableForLookupConfiguration
    : HomeFurnishingLookupConfiguration<PlantOrnamentSuitableForLookup, PlantOrnamentSuitableFor>
{
    protected override string TableName => "PlantOrnamentSuitableFors";
    protected override IReadOnlyList<HomeFurnishingLookupEntry<PlantOrnamentSuitableFor>> Entries =>
        PlantOrnamentCatalog.SuitableFors;
}
