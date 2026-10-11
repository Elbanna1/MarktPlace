using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;
using Shared.Enums;

namespace Persistence.Configurations;

public class BuildingConfiguration : IEntityTypeConfiguration<Building>
{
    public void Configure(EntityTypeBuilder<Building> builder)
    {
        builder.ToTable("Buildings");

        builder.HasKey(x => x.Id);

        RealEstateListingMapping.ApplyCommonColumns(builder);
        RealEstateListingMapping.ApplyLicenceColumns(builder);
        RealEstateListingMapping.ApplyRentColumns(builder);
        RealEstateListingMapping.ApplyExchangeColumns(builder);

        builder.Property(x => x.ViolationDetails).HasMaxLength(4000);
        builder.Property(x => x.InstallmentPeriod).HasMaxLength(100);

        builder.Property(x => x.Price).HasColumnType("decimal(18,2)");
        builder.Property(x => x.PricePerMeter).HasColumnType("decimal(18,2)");
        builder.Property(x => x.Area).HasColumnType("decimal(18,2)");
        builder.Property(x => x.GardenArea).HasColumnType("decimal(18,2)");
        builder.Property(x => x.DownPayment).HasColumnType("decimal(18,2)");
        builder.Property(x => x.InstallmentAmount).HasColumnType("decimal(18,2)");
        builder.Property(x => x.MaintenanceDepositAmount).HasColumnType("decimal(18,2)");
        builder.Property(x => x.MonthlyFees).HasColumnType("decimal(18,2)");
        builder.Property(x => x.RentDownPayment).HasColumnType("decimal(18,2)");

        builder.Property(x => x.BuildingType).HasConversion<int>();
        builder.Property(x => x.FurnishedStatus).HasConversion<int>();
        builder.Property(x => x.FinishingType).HasConversion<int>();
        builder.Property(x => x.PropertyAge).HasConversion<int>();
        builder.Property(x => x.Direction).HasConversion<int>();
        builder.Property(x => x.ViewType).HasConversion<int>();
        builder.Property(x => x.ReceptionPieces).HasConversion<int>();
        builder.Property(x => x.LegalStatus).HasConversion<int>();
        builder.Property(x => x.ReconciliationForm).HasConversion<int>();
        builder.Property(x => x.OwnershipDocument).HasConversion<int>();
        builder.Property(x => x.PaymentMethod).HasConversion<int>();
        builder.Property(x => x.InstallmentProvider).HasConversion<int>();
        builder.Property(x => x.RentType).HasConversion<int>();
        builder.Property(x => x.ExchangeWith).HasConversion<int>();

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(x => x.Images)
            .WithOne(i => i.Building)
            .HasForeignKey(i => i.BuildingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Features)
            .WithOne(f => f.Building)
            .HasForeignKey(f => f.BuildingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.RentInclusions)
            .WithOne(r => r.Building)
            .HasForeignKey(r => r.BuildingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(QueryFilterNames.SoftDelete, x => !x.IsDeleted);

        RealEstateListingMapping.ApplyCommonIndexes(builder);

        builder.HasIndex(x => x.BuildingType);
        builder.HasIndex(x => x.Price);
        builder.HasIndex(x => x.Area);
        builder.HasIndex(x => new { x.ModerationStatus, x.AdvertiserName }, "IX_Buildings_Advertisers")
            .IncludeProperties(x => new { x.IsDeleted, x.ExpireAt });
    }
}

public class BuildingImageConfiguration : RealEstateImageConfiguration<BuildingImage>
{
    protected override string TableName => "BuildingImages";

    protected override string ListingIdPropertyName => nameof(BuildingImage.BuildingId);

    protected override void ApplySoftDeleteFilter(EntityTypeBuilder<BuildingImage> builder) =>
        builder.HasQueryFilter(i => !i.Building.IsDeleted);
}

public class BuildingFeatureSelectionConfiguration : IEntityTypeConfiguration<BuildingFeatureSelection>
{
    public void Configure(EntityTypeBuilder<BuildingFeatureSelection> builder)
    {
        builder.ToTable("BuildingFeatureSelections");

        builder.HasKey(x => new { x.BuildingId, x.Feature });
        builder.Property(x => x.Feature).HasConversion<int>();

        builder.HasIndex(x => x.Feature);

        builder.HasQueryFilter(x => !x.Building.IsDeleted);
    }
}

public class BuildingRentInclusionSelectionConfiguration
    : IEntityTypeConfiguration<BuildingRentInclusionSelection>
{
    public void Configure(EntityTypeBuilder<BuildingRentInclusionSelection> builder)
    {
        builder.ToTable("BuildingRentInclusionSelections");

        builder.HasKey(x => new { x.BuildingId, x.Inclusion });
        builder.Property(x => x.Inclusion).HasConversion<int>();

        builder.HasQueryFilter(x => !x.Building.IsDeleted);
    }
}

public class BuildingTypeLookupConfiguration : RealEstateLookupConfiguration<BuildingTypeLookup, BuildingType>
{
    protected override string TableName => "BuildingTypes";
    protected override IReadOnlyList<RealEstateLookupEntry<BuildingType>> Entries =>
        BuildingCatalog.BuildingTypes;
}
