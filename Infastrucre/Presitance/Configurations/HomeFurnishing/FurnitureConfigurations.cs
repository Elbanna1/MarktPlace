using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;
using Shared.Enums;

namespace Persistence.Configurations;

public class FurnitureConfiguration : IEntityTypeConfiguration<Furniture>
{
    public void Configure(EntityTypeBuilder<Furniture> builder)
    {
        builder.ToTable("Furnitures");

        builder.HasKey(x => x.Id);

        HomeFurnishingListingMapping.ApplyCommonColumns(builder);

        builder.Property(x => x.OtherFurnitureType).HasMaxLength(150);
        builder.Property(x => x.OtherMaterial).HasMaxLength(150);
        builder.Property(x => x.OtherColor).HasMaxLength(150);

        builder.Property(x => x.Length).HasColumnType("decimal(10,2)");
        builder.Property(x => x.Width).HasColumnType("decimal(10,2)");
        builder.Property(x => x.Height).HasColumnType("decimal(10,2)");

        builder.Property(x => x.FurnitureType).HasConversion<int>();
        builder.Property(x => x.Material).HasConversion<int>();
        builder.Property(x => x.Condition).HasConversion<int>();

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(x => x.Images)
            .WithOne(i => i.Furniture)
            .HasForeignKey(i => i.FurnitureId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Colors)
            .WithOne(c => c.Furniture)
            .HasForeignKey(c => c.FurnitureId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(QueryFilterNames.SoftDelete, x => !x.IsDeleted);

        HomeFurnishingListingMapping.ApplyCommonIndexes(builder);

        builder.HasIndex(x => x.FurnitureType);
        builder.HasIndex(x => x.Material);
        builder.HasIndex(x => x.Condition);
        builder.HasIndex(x => x.DeliveryAvailable);
    }
}

public class FurnitureImageConfiguration : HomeFurnishingImageConfiguration<FurnitureImage>
{
    protected override string TableName => "FurnitureImages";

    protected override string ListingIdPropertyName => nameof(FurnitureImage.FurnitureId);

    protected override void ApplySoftDeleteFilter(EntityTypeBuilder<FurnitureImage> builder) =>
        builder.HasQueryFilter(i => !i.Furniture.IsDeleted);
}

public class FurnitureColorSelectionConfiguration : IEntityTypeConfiguration<FurnitureColorSelection>
{
    public void Configure(EntityTypeBuilder<FurnitureColorSelection> builder)
    {
        builder.ToTable("FurnitureColorSelections");

        builder.HasKey(x => new { x.FurnitureId, x.Color });
        builder.Property(x => x.Color).HasConversion<int>();

        builder.HasIndex(x => x.Color);

        builder.HasQueryFilter(x => !x.Furniture.IsDeleted);
    }
}

public class FurnitureTypeLookupConfiguration
    : HomeFurnishingLookupConfiguration<FurnitureTypeLookup, FurnitureType>
{
    protected override string TableName => "FurnitureTypes";
    protected override IReadOnlyList<HomeFurnishingLookupEntry<FurnitureType>> Entries =>
        FurnitureCatalog.FurnitureTypes;
}

public class FurnitureMaterialLookupConfiguration
    : HomeFurnishingLookupConfiguration<FurnitureMaterialLookup, FurnitureMaterial>
{
    protected override string TableName => "FurnitureMaterials";
    protected override IReadOnlyList<HomeFurnishingLookupEntry<FurnitureMaterial>> Entries =>
        FurnitureCatalog.Materials;
}

public class FurnitureColorLookupConfiguration
    : HomeFurnishingLookupConfiguration<FurnitureColorLookup, FurnitureColor>
{
    protected override string TableName => "FurnitureColors";
    protected override IReadOnlyList<HomeFurnishingLookupEntry<FurnitureColor>> Entries =>
        FurnitureCatalog.Colors;
}

public class FurnitureConditionLookupConfiguration
    : HomeFurnishingLookupConfiguration<FurnitureConditionLookup, FurnitureCondition>
{
    protected override string TableName => "FurnitureConditions";
    protected override IReadOnlyList<HomeFurnishingLookupEntry<FurnitureCondition>> Entries =>
        FurnitureCatalog.Conditions;
}
