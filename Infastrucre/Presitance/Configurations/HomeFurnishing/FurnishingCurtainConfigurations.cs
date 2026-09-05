using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;
using Shared.Enums;

namespace Persistence.Configurations;

public class FurnishingCurtainConfiguration : IEntityTypeConfiguration<FurnishingCurtain>
{
    public void Configure(EntityTypeBuilder<FurnishingCurtain> builder)
    {
        builder.ToTable("FurnishingCurtains");

        builder.HasKey(x => x.Id);

        HomeFurnishingListingMapping.ApplyCommonColumns(builder);

        builder.Property(x => x.OtherProductType).HasMaxLength(150);
        builder.Property(x => x.OtherSize).HasMaxLength(150);
        builder.Property(x => x.OtherMaterial).HasMaxLength(150);
        builder.Property(x => x.OtherColor).HasMaxLength(150);

        builder.Property(x => x.ProductType).HasConversion<int>();
        builder.Property(x => x.Size).HasConversion<int>();
        builder.Property(x => x.Material).HasConversion<int>();

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(x => x.Images)
            .WithOne(i => i.FurnishingCurtain)
            .HasForeignKey(i => i.FurnishingCurtainId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Colors)
            .WithOne(c => c.FurnishingCurtain)
            .HasForeignKey(c => c.FurnishingCurtainId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(QueryFilterNames.SoftDelete, x => !x.IsDeleted);

        HomeFurnishingListingMapping.ApplyCommonIndexes(builder);

        builder.HasIndex(x => x.ProductType);
        builder.HasIndex(x => x.Size);
        builder.HasIndex(x => x.Material);
        builder.HasIndex(x => x.DeliveryAvailable);
    }
}

public class FurnishingCurtainImageConfiguration : HomeFurnishingImageConfiguration<FurnishingCurtainImage>
{
    protected override string TableName => "FurnishingCurtainImages";

    protected override string ListingIdPropertyName => nameof(FurnishingCurtainImage.FurnishingCurtainId);

    protected override void ApplySoftDeleteFilter(EntityTypeBuilder<FurnishingCurtainImage> builder) =>
        builder.HasQueryFilter(i => !i.FurnishingCurtain.IsDeleted);
}

public class FurnishingCurtainColorSelectionConfiguration
    : IEntityTypeConfiguration<FurnishingCurtainColorSelection>
{
    public void Configure(EntityTypeBuilder<FurnishingCurtainColorSelection> builder)
    {
        builder.ToTable("FurnishingCurtainColorSelections");

        builder.HasKey(x => new { x.FurnishingCurtainId, x.Color });
        builder.Property(x => x.Color).HasConversion<int>();

        builder.HasIndex(x => x.Color);

        builder.HasQueryFilter(x => !x.FurnishingCurtain.IsDeleted);
    }
}

public class FurnishingCurtainProductTypeLookupConfiguration
    : HomeFurnishingLookupConfiguration<FurnishingCurtainProductTypeLookup, FurnishingCurtainProductType>
{
    protected override string TableName => "FurnishingCurtainProductTypes";
    protected override IReadOnlyList<HomeFurnishingLookupEntry<FurnishingCurtainProductType>> Entries =>
        FurnishingCurtainCatalog.ProductTypes;
}

public class FurnishingCurtainSizeLookupConfiguration
    : HomeFurnishingLookupConfiguration<FurnishingCurtainSizeLookup, FurnishingCurtainSize>
{
    protected override string TableName => "FurnishingCurtainSizes";
    protected override IReadOnlyList<HomeFurnishingLookupEntry<FurnishingCurtainSize>> Entries =>
        FurnishingCurtainCatalog.Sizes;
}

public class FurnishingCurtainMaterialLookupConfiguration
    : HomeFurnishingLookupConfiguration<FurnishingCurtainMaterialLookup, FurnishingCurtainMaterial>
{
    protected override string TableName => "FurnishingCurtainMaterials";
    protected override IReadOnlyList<HomeFurnishingLookupEntry<FurnishingCurtainMaterial>> Entries =>
        FurnishingCurtainCatalog.Materials;
}

public class FurnishingCurtainColorLookupConfiguration
    : HomeFurnishingLookupConfiguration<FurnishingCurtainColorLookup, FurnishingCurtainColor>
{
    protected override string TableName => "FurnishingCurtainColors";
    protected override IReadOnlyList<HomeFurnishingLookupEntry<FurnishingCurtainColor>> Entries =>
        FurnishingCurtainCatalog.Colors;
}
