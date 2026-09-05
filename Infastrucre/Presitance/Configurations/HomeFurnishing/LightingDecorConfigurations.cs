using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;
using Shared.Enums;

namespace Persistence.Configurations;

public class LightingDecorConfiguration : IEntityTypeConfiguration<LightingDecor>
{
    public void Configure(EntityTypeBuilder<LightingDecor> builder)
    {
        builder.ToTable("LightingDecors");

        builder.HasKey(x => x.Id);

        HomeFurnishingListingMapping.ApplyCommonColumns(builder);

        builder.Property(x => x.OtherProductType).HasMaxLength(150);
        builder.Property(x => x.OtherMaterial).HasMaxLength(150);
        builder.Property(x => x.OtherColor).HasMaxLength(150);

        builder.Property(x => x.ProductType).HasConversion<int>();
        builder.Property(x => x.Material).HasConversion<int>();

        builder.Property(x => x.LightType).HasConversion<int>();

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(x => x.Images)
            .WithOne(i => i.LightingDecor)
            .HasForeignKey(i => i.LightingDecorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Colors)
            .WithOne(c => c.LightingDecor)
            .HasForeignKey(c => c.LightingDecorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(QueryFilterNames.SoftDelete, x => !x.IsDeleted);

        HomeFurnishingListingMapping.ApplyCommonIndexes(builder);

        builder.HasIndex(x => x.ProductType);
        builder.HasIndex(x => x.Material);
        builder.HasIndex(x => x.LightType);
    }
}

public class LightingDecorImageConfiguration : HomeFurnishingImageConfiguration<LightingDecorImage>
{
    protected override string TableName => "LightingDecorImages";

    protected override string ListingIdPropertyName => nameof(LightingDecorImage.LightingDecorId);

    protected override void ApplySoftDeleteFilter(EntityTypeBuilder<LightingDecorImage> builder) =>
        builder.HasQueryFilter(i => !i.LightingDecor.IsDeleted);
}

public class LightingDecorColorSelectionConfiguration
    : IEntityTypeConfiguration<LightingDecorColorSelection>
{
    public void Configure(EntityTypeBuilder<LightingDecorColorSelection> builder)
    {
        builder.ToTable("LightingDecorColorSelections");

        builder.HasKey(x => new { x.LightingDecorId, x.Color });
        builder.Property(x => x.Color).HasConversion<int>();

        builder.HasIndex(x => x.Color);

        builder.HasQueryFilter(x => !x.LightingDecor.IsDeleted);
    }
}

public class LightingDecorProductTypeLookupConfiguration
    : HomeFurnishingLookupConfiguration<LightingDecorProductTypeLookup, LightingDecorProductType>
{
    protected override string TableName => "LightingDecorProductTypes";
    protected override IReadOnlyList<HomeFurnishingLookupEntry<LightingDecorProductType>> Entries =>
        LightingDecorCatalog.ProductTypes;
}

public class LightingDecorMaterialLookupConfiguration
    : HomeFurnishingLookupConfiguration<LightingDecorMaterialLookup, LightingDecorMaterial>
{
    protected override string TableName => "LightingDecorMaterials";
    protected override IReadOnlyList<HomeFurnishingLookupEntry<LightingDecorMaterial>> Entries =>
        LightingDecorCatalog.Materials;
}

public class LightingDecorColorLookupConfiguration
    : HomeFurnishingLookupConfiguration<LightingDecorColorLookup, LightingDecorColor>
{
    protected override string TableName => "LightingDecorColors";
    protected override IReadOnlyList<HomeFurnishingLookupEntry<LightingDecorColor>> Entries =>
        LightingDecorCatalog.Colors;
}

public class LightingDecorLightTypeLookupConfiguration
    : HomeFurnishingLookupConfiguration<LightingDecorLightTypeLookup, LightingDecorLightType>
{
    protected override string TableName => "LightingDecorLightTypes";
    protected override IReadOnlyList<HomeFurnishingLookupEntry<LightingDecorLightType>> Entries =>
        LightingDecorCatalog.LightTypes;
}
