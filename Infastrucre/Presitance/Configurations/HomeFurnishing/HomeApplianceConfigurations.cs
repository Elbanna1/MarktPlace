using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;
using Shared.Enums;

namespace Persistence.Configurations;

public class HomeApplianceConfiguration : IEntityTypeConfiguration<HomeAppliance>
{
    public void Configure(EntityTypeBuilder<HomeAppliance> builder)
    {
        builder.ToTable("HomeAppliances");

        builder.HasKey(x => x.Id);

        HomeFurnishingListingMapping.ApplyCommonColumns(builder);

        builder.Property(x => x.OtherDeviceType).HasMaxLength(150);
        builder.Property(x => x.OtherBrand).HasMaxLength(150);
        builder.Property(x => x.OtherColor).HasMaxLength(150);

        builder.Property(x => x.WarrantyDuration).HasMaxLength(100);
        builder.Property(x => x.PowerRating).HasMaxLength(100);

        builder.Property(x => x.DeviceType).HasConversion<int>();
        builder.Property(x => x.Brand).HasConversion<int>();
        builder.Property(x => x.Condition).HasConversion<int>();
        builder.Property(x => x.Warranty).HasConversion<int>();

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(x => x.Images)
            .WithOne(i => i.HomeAppliance)
            .HasForeignKey(i => i.HomeApplianceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Colors)
            .WithOne(c => c.HomeAppliance)
            .HasForeignKey(c => c.HomeApplianceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(QueryFilterNames.SoftDelete, x => !x.IsDeleted);

        HomeFurnishingListingMapping.ApplyCommonIndexes(builder);

        builder.HasIndex(x => x.DeviceType);
        builder.HasIndex(x => x.Brand);
        builder.HasIndex(x => x.Condition);
        builder.HasIndex(x => x.Warranty);
        builder.HasIndex(x => x.DeliveryAvailable);
    }
}

public class HomeApplianceImageConfiguration : HomeFurnishingImageConfiguration<HomeApplianceImage>
{
    protected override string TableName => "HomeApplianceImages";

    protected override string ListingIdPropertyName => nameof(HomeApplianceImage.HomeApplianceId);

    protected override void ApplySoftDeleteFilter(EntityTypeBuilder<HomeApplianceImage> builder) =>
        builder.HasQueryFilter(i => !i.HomeAppliance.IsDeleted);
}

public class HomeApplianceColorSelectionConfiguration
    : IEntityTypeConfiguration<HomeApplianceColorSelection>
{
    public void Configure(EntityTypeBuilder<HomeApplianceColorSelection> builder)
    {
        builder.ToTable("HomeApplianceColorSelections");

        builder.HasKey(x => new { x.HomeApplianceId, x.Color });
        builder.Property(x => x.Color).HasConversion<int>();

        builder.HasIndex(x => x.Color);

        builder.HasQueryFilter(x => !x.HomeAppliance.IsDeleted);
    }
}

public class HomeApplianceDeviceTypeLookupConfiguration
    : HomeFurnishingLookupConfiguration<HomeApplianceDeviceTypeLookup, HomeApplianceDeviceType>
{
    protected override string TableName => "HomeApplianceDeviceTypes";
    protected override IReadOnlyList<HomeFurnishingLookupEntry<HomeApplianceDeviceType>> Entries =>
        HomeApplianceCatalog.DeviceTypes;
}

public class HomeApplianceBrandLookupConfiguration
    : HomeFurnishingLookupConfiguration<HomeApplianceBrandLookup, HomeApplianceBrand>
{
    protected override string TableName => "HomeApplianceBrands";
    protected override IReadOnlyList<HomeFurnishingLookupEntry<HomeApplianceBrand>> Entries =>
        HomeApplianceCatalog.Brands;
}

public class HomeApplianceConditionLookupConfiguration
    : HomeFurnishingLookupConfiguration<HomeApplianceConditionLookup, HomeApplianceCondition>
{
    protected override string TableName => "HomeApplianceConditions";
    protected override IReadOnlyList<HomeFurnishingLookupEntry<HomeApplianceCondition>> Entries =>
        HomeApplianceCatalog.Conditions;
}

public class HomeApplianceWarrantyLookupConfiguration
    : HomeFurnishingLookupConfiguration<HomeApplianceWarrantyLookup, HomeApplianceWarranty>
{
    protected override string TableName => "HomeApplianceWarranties";
    protected override IReadOnlyList<HomeFurnishingLookupEntry<HomeApplianceWarranty>> Entries =>
        HomeApplianceCatalog.Warranties;
}

public class HomeApplianceColorLookupConfiguration
    : HomeFurnishingLookupConfiguration<HomeApplianceColorLookup, HomeApplianceColor>
{
    protected override string TableName => "HomeApplianceColors";
    protected override IReadOnlyList<HomeFurnishingLookupEntry<HomeApplianceColor>> Entries =>
        HomeApplianceCatalog.Colors;
}
