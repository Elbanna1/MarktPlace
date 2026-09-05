using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;
using Shared.Enums;

namespace Persistence.Configurations;

public class KitchenToolConfiguration : IEntityTypeConfiguration<KitchenTool>
{
    public void Configure(EntityTypeBuilder<KitchenTool> builder)
    {
        builder.ToTable("KitchenTools");

        builder.HasKey(x => x.Id);

        HomeFurnishingListingMapping.ApplyCommonColumns(builder);

        builder.Property(x => x.OtherProductType).HasMaxLength(150);
        builder.Property(x => x.OtherMaterial).HasMaxLength(150);
        builder.Property(x => x.OtherColor).HasMaxLength(150);

        builder.Property(x => x.ProductType).HasConversion<int>();
        builder.Property(x => x.Material).HasConversion<int>();

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(x => x.Images)
            .WithOne(i => i.KitchenTool)
            .HasForeignKey(i => i.KitchenToolId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Colors)
            .WithOne(c => c.KitchenTool)
            .HasForeignKey(c => c.KitchenToolId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(QueryFilterNames.SoftDelete, x => !x.IsDeleted);

        HomeFurnishingListingMapping.ApplyCommonIndexes(builder);

        builder.HasIndex(x => x.ProductType);
        builder.HasIndex(x => x.Material);
    }
}

public class KitchenToolImageConfiguration : HomeFurnishingImageConfiguration<KitchenToolImage>
{
    protected override string TableName => "KitchenToolImages";

    protected override string ListingIdPropertyName => nameof(KitchenToolImage.KitchenToolId);

    protected override void ApplySoftDeleteFilter(EntityTypeBuilder<KitchenToolImage> builder) =>
        builder.HasQueryFilter(i => !i.KitchenTool.IsDeleted);
}

public class KitchenToolColorSelectionConfiguration : IEntityTypeConfiguration<KitchenToolColorSelection>
{
    public void Configure(EntityTypeBuilder<KitchenToolColorSelection> builder)
    {
        builder.ToTable("KitchenToolColorSelections");

        builder.HasKey(x => new { x.KitchenToolId, x.Color });
        builder.Property(x => x.Color).HasConversion<int>();

        builder.HasIndex(x => x.Color);

        builder.HasQueryFilter(x => !x.KitchenTool.IsDeleted);
    }
}

public class KitchenToolProductTypeLookupConfiguration
    : HomeFurnishingLookupConfiguration<KitchenToolProductTypeLookup, KitchenToolProductType>
{
    protected override string TableName => "KitchenToolProductTypes";
    protected override IReadOnlyList<HomeFurnishingLookupEntry<KitchenToolProductType>> Entries =>
        KitchenToolCatalog.ProductTypes;
}

public class KitchenToolMaterialLookupConfiguration
    : HomeFurnishingLookupConfiguration<KitchenToolMaterialLookup, KitchenToolMaterial>
{
    protected override string TableName => "KitchenToolMaterials";
    protected override IReadOnlyList<HomeFurnishingLookupEntry<KitchenToolMaterial>> Entries =>
        KitchenToolCatalog.Materials;
}

public class KitchenToolColorLookupConfiguration
    : HomeFurnishingLookupConfiguration<KitchenToolColorLookup, KitchenToolColor>
{
    protected override string TableName => "KitchenToolColors";
    protected override IReadOnlyList<HomeFurnishingLookupEntry<KitchenToolColor>> Entries =>
        KitchenToolCatalog.Colors;
}
