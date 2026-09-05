using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;
using Shared.Enums;

namespace Persistence.Configurations;

public class BathroomSupplyConfiguration : IEntityTypeConfiguration<BathroomSupply>
{
    public void Configure(EntityTypeBuilder<BathroomSupply> builder)
    {
        builder.ToTable("BathroomSupplies");

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
            .WithOne(i => i.BathroomSupply)
            .HasForeignKey(i => i.BathroomSupplyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Colors)
            .WithOne(c => c.BathroomSupply)
            .HasForeignKey(c => c.BathroomSupplyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(QueryFilterNames.SoftDelete, x => !x.IsDeleted);

        HomeFurnishingListingMapping.ApplyCommonIndexes(builder);

        builder.HasIndex(x => x.ProductType);
        builder.HasIndex(x => x.Material);
    }
}

public class BathroomSupplyImageConfiguration : HomeFurnishingImageConfiguration<BathroomSupplyImage>
{
    protected override string TableName => "BathroomSupplyImages";

    protected override string ListingIdPropertyName => nameof(BathroomSupplyImage.BathroomSupplyId);

    protected override void ApplySoftDeleteFilter(EntityTypeBuilder<BathroomSupplyImage> builder) =>
        builder.HasQueryFilter(i => !i.BathroomSupply.IsDeleted);
}

public class BathroomSupplyColorSelectionConfiguration
    : IEntityTypeConfiguration<BathroomSupplyColorSelection>
{
    public void Configure(EntityTypeBuilder<BathroomSupplyColorSelection> builder)
    {
        builder.ToTable("BathroomSupplyColorSelections");

        builder.HasKey(x => new { x.BathroomSupplyId, x.Color });
        builder.Property(x => x.Color).HasConversion<int>();

        builder.HasIndex(x => x.Color);

        builder.HasQueryFilter(x => !x.BathroomSupply.IsDeleted);
    }
}

public class BathroomSupplyProductTypeLookupConfiguration
    : HomeFurnishingLookupConfiguration<BathroomSupplyProductTypeLookup, BathroomSupplyProductType>
{
    protected override string TableName => "BathroomSupplyProductTypes";
    protected override IReadOnlyList<HomeFurnishingLookupEntry<BathroomSupplyProductType>> Entries =>
        BathroomSupplyCatalog.ProductTypes;
}

public class BathroomSupplyMaterialLookupConfiguration
    : HomeFurnishingLookupConfiguration<BathroomSupplyMaterialLookup, BathroomSupplyMaterial>
{
    protected override string TableName => "BathroomSupplyMaterials";
    protected override IReadOnlyList<HomeFurnishingLookupEntry<BathroomSupplyMaterial>> Entries =>
        BathroomSupplyCatalog.Materials;
}

public class BathroomSupplyColorLookupConfiguration
    : HomeFurnishingLookupConfiguration<BathroomSupplyColorLookup, BathroomSupplyColor>
{
    protected override string TableName => "BathroomSupplyColors";
    protected override IReadOnlyList<HomeFurnishingLookupEntry<BathroomSupplyColor>> Entries =>
        BathroomSupplyCatalog.Colors;
}
