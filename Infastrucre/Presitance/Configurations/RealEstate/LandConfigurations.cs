using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;
using Shared.Enums;

namespace Persistence.Configurations;

public class LandConfiguration : IEntityTypeConfiguration<Land>
{
    public void Configure(EntityTypeBuilder<Land> builder)
    {
        builder.ToTable("Lands");

        builder.HasKey(x => x.Id);

        RealEstateListingMapping.ApplyCommonColumns(builder);
        RealEstateListingMapping.ApplyLicenceColumns(builder);
        RealEstateListingMapping.ApplyRentColumns(builder);
        RealEstateListingMapping.ApplyExchangeColumns(builder);

        builder.Property(x => x.OtherLandType).HasMaxLength(150);
        builder.Property(x => x.OtherReconciliationForm).HasMaxLength(150);
        builder.Property(x => x.OtherOwnershipDocument).HasMaxLength(150);
        builder.Property(x => x.OtherExchangeWith).HasMaxLength(150);
        builder.Property(x => x.OtherIrrigationSource).HasMaxLength(150);

        builder.Property(x => x.ViolationDetails).HasMaxLength(4000);
        builder.Property(x => x.CurrentCropType).HasMaxLength(150);
        builder.Property(x => x.TreeType).HasMaxLength(150);
        builder.Property(x => x.TreesAge).HasMaxLength(100);

        builder.Property(x => x.PricePerMeter).HasColumnType("decimal(18,2)");
        builder.Property(x => x.TotalPrice).HasColumnType("decimal(18,2)");
        builder.Property(x => x.DownPayment).HasColumnType("decimal(18,2)");

        builder.Property(x => x.Area).HasColumnType("decimal(18,2)");
        builder.Property(x => x.Length).HasColumnType("decimal(10,2)");
        builder.Property(x => x.Width).HasColumnType("decimal(10,2)");
        builder.Property(x => x.FacadeLength).HasColumnType("decimal(10,2)");
        builder.Property(x => x.StreetWidth).HasColumnType("decimal(10,2)");
        builder.Property(x => x.AllowedBuildingRatio).HasColumnType("decimal(5,2)");
        builder.Property(x => x.CultivatedFeddans).HasColumnType("decimal(10,2)");

        builder.Property(x => x.LandType).HasConversion<int>();
        builder.Property(x => x.AreaUnit).HasConversion<int>();
        builder.Property(x => x.FacadesCount).HasConversion<int>();
        builder.Property(x => x.Direction).HasConversion<int>();
        builder.Property(x => x.RoadType).HasConversion<int>();
        builder.Property(x => x.LegalStatus).HasConversion<int>();
        builder.Property(x => x.ReconciliationForm).HasConversion<int>();
        builder.Property(x => x.OwnershipDocument).HasConversion<int>();
        builder.Property(x => x.RentType).HasConversion<int>();
        builder.Property(x => x.MinimumRentPeriod).HasConversion<int>();
        builder.Property(x => x.ContractDuration).HasConversion<int>();
        builder.Property(x => x.ExchangeWith).HasConversion<int>();
        builder.Property(x => x.HarvestSeason).HasConversion<int>();
        builder.Property(x => x.SoilType).HasConversion<int>();
        builder.Property(x => x.IrrigationSource).HasConversion<int>();
        builder.Property(x => x.QualityCertificate).HasConversion<int>();
        builder.Property(x => x.ExistingBuildingType).HasConversion<int>();
        builder.Property(x => x.BuildingCompletionRatio).HasConversion<int>();

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(x => x.Images)
            .WithOne(i => i.Land)
            .HasForeignKey(i => i.LandId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Utilities)
            .WithOne(u => u.Land)
            .HasForeignKey(u => u.LandId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.RentInclusions)
            .WithOne(r => r.Land)
            .HasForeignKey(r => r.LandId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(QueryFilterNames.SoftDelete, x => !x.IsDeleted);

        RealEstateListingMapping.ApplyCommonIndexes(builder);

        builder.HasIndex(x => x.LandType);
        builder.HasIndex(x => x.TotalPrice);
        builder.HasIndex(x => x.PricePerMeter);
        builder.HasIndex(x => x.Area);
        builder.HasIndex(x => x.AreaUnit);
        builder.HasIndex(x => x.InsideBuildingCordon);
        builder.HasIndex(x => x.IsBuildable);
        builder.HasIndex(x => x.LegalStatus);
        builder.HasIndex(x => x.OwnershipDocument);
        builder.HasIndex(x => x.RoadType);
        builder.HasIndex(x => x.FacadesCount);
        builder.HasIndex(x => x.Direction);
        builder.HasIndex(x => x.IsCurrentlyCultivated);
        builder.HasIndex(x => x.IrrigationSource);
        builder.HasIndex(x => x.IsOrganic);
        builder.HasIndex(x => x.HasWell);
        builder.HasIndex(x => x.HasIrrigationNetwork);
        builder.HasIndex(x => x.HasFence);

        builder.HasIndex(x => new { x.ModerationStatus, x.IsDeleted, x.ExpireAt })
            .HasDatabaseName("IX_Lands_Search")
            .IncludeProperties(x => new
            {
                x.Title,
                x.Description,
                x.District,
                x.Address,
                x.OtherLandType,
                x.OtherProject,
                x.CreatedAt,
                x.IsPremium,
                x.IsFeatured,
                x.TotalPrice
            });
    }
}

public class LandImageConfiguration : RealEstateImageConfiguration<LandImage>
{
    protected override string TableName => "LandImages";

    protected override string ListingIdPropertyName => nameof(LandImage.LandId);

    protected override void ApplySoftDeleteFilter(EntityTypeBuilder<LandImage> builder) =>
        builder.HasQueryFilter(i => !i.Land.IsDeleted);
}

public class LandUtilitySelectionConfiguration : IEntityTypeConfiguration<LandUtilitySelection>
{
    public void Configure(EntityTypeBuilder<LandUtilitySelection> builder)
    {
        builder.ToTable("LandUtilitySelections");

        builder.HasKey(x => new { x.LandId, x.Utility });
        builder.Property(x => x.Utility).HasConversion<int>();

        builder.HasIndex(x => x.Utility);

        builder.HasQueryFilter(x => !x.Land.IsDeleted);
    }
}

public class LandRentInclusionSelectionConfiguration : IEntityTypeConfiguration<LandRentInclusionSelection>
{
    public void Configure(EntityTypeBuilder<LandRentInclusionSelection> builder)
    {
        builder.ToTable("LandRentInclusionSelections");

        builder.HasKey(x => new { x.LandId, x.Inclusion });
        builder.Property(x => x.Inclusion).HasConversion<int>();

        builder.HasIndex(x => x.Inclusion);

        builder.HasQueryFilter(x => !x.Land.IsDeleted);
    }
}

public class LandTypeLookupConfiguration : RealEstateLookupConfiguration<LandTypeLookup, LandType>
{
    protected override string TableName => "LandTypes";
    protected override IReadOnlyList<RealEstateLookupEntry<LandType>> Entries => LandCatalog.LandTypes;
}

public class LandAreaUnitLookupConfiguration : RealEstateLookupConfiguration<LandAreaUnitLookup, LandAreaUnit>
{
    protected override string TableName => "LandAreaUnits";
    protected override IReadOnlyList<RealEstateLookupEntry<LandAreaUnit>> Entries => LandCatalog.AreaUnits;
}

public class LandFacadesCountLookupConfiguration
    : RealEstateLookupConfiguration<LandFacadesCountLookup, LandFacadesCount>
{
    protected override string TableName => "LandFacadesCounts";
    protected override IReadOnlyList<RealEstateLookupEntry<LandFacadesCount>> Entries => LandCatalog.FacadesCounts;
}

public class LandDirectionLookupConfiguration : RealEstateLookupConfiguration<LandDirectionLookup, LandDirection>
{
    protected override string TableName => "LandDirections";
    protected override IReadOnlyList<RealEstateLookupEntry<LandDirection>> Entries => LandCatalog.Directions;
}

public class LandRoadTypeLookupConfiguration : RealEstateLookupConfiguration<LandRoadTypeLookup, LandRoadType>
{
    protected override string TableName => "LandRoadTypes";
    protected override IReadOnlyList<RealEstateLookupEntry<LandRoadType>> Entries => LandCatalog.RoadTypes;
}

public class LandLegalStatusLookupConfiguration
    : RealEstateLookupConfiguration<LandLegalStatusLookup, LandLegalStatus>
{
    protected override string TableName => "LandLegalStatuses";
    protected override IReadOnlyList<RealEstateLookupEntry<LandLegalStatus>> Entries => LandCatalog.LegalStatuses;
}

public class LandReconciliationFormLookupConfiguration
    : RealEstateLookupConfiguration<LandReconciliationFormLookup, LandReconciliationForm>
{
    protected override string TableName => "LandReconciliationForms";
    protected override IReadOnlyList<RealEstateLookupEntry<LandReconciliationForm>> Entries =>
        LandCatalog.ReconciliationForms;
}

public class LandOwnershipDocumentLookupConfiguration
    : RealEstateLookupConfiguration<LandOwnershipDocumentLookup, LandOwnershipDocument>
{
    protected override string TableName => "LandOwnershipDocuments";
    protected override IReadOnlyList<RealEstateLookupEntry<LandOwnershipDocument>> Entries =>
        LandCatalog.OwnershipDocuments;
}

public class LandUtilityLookupConfiguration : RealEstateLookupConfiguration<LandUtilityLookup, LandUtility>
{
    protected override string TableName => "LandUtilities";
    protected override IReadOnlyList<RealEstateLookupEntry<LandUtility>> Entries => LandCatalog.Utilities;
}

public class LandRentTypeLookupConfiguration : RealEstateLookupConfiguration<LandRentTypeLookup, LandRentType>
{
    protected override string TableName => "LandRentTypes";
    protected override IReadOnlyList<RealEstateLookupEntry<LandRentType>> Entries => LandCatalog.RentTypes;
}

public class LandMinimumRentPeriodLookupConfiguration
    : RealEstateLookupConfiguration<LandMinimumRentPeriodLookup, LandMinimumRentPeriod>
{
    protected override string TableName => "LandMinimumRentPeriods";
    protected override IReadOnlyList<RealEstateLookupEntry<LandMinimumRentPeriod>> Entries =>
        LandCatalog.MinimumRentPeriods;
}

public class LandRentInclusionLookupConfiguration
    : RealEstateLookupConfiguration<LandRentInclusionLookup, LandRentInclusion>
{
    protected override string TableName => "LandRentInclusions";
    protected override IReadOnlyList<RealEstateLookupEntry<LandRentInclusion>> Entries =>
        LandCatalog.RentInclusions;
}

public class LandContractDurationLookupConfiguration
    : RealEstateLookupConfiguration<LandContractDurationLookup, LandContractDuration>
{
    protected override string TableName => "LandContractDurations";
    protected override IReadOnlyList<RealEstateLookupEntry<LandContractDuration>> Entries =>
        LandCatalog.ContractDurations;
}

public class LandExchangeWithLookupConfiguration
    : RealEstateLookupConfiguration<LandExchangeWithLookup, LandExchangeWith>
{
    protected override string TableName => "LandExchangeTargets";
    protected override IReadOnlyList<RealEstateLookupEntry<LandExchangeWith>> Entries =>
        LandCatalog.ExchangeTargets;
}

public class LandHarvestSeasonLookupConfiguration
    : RealEstateLookupConfiguration<LandHarvestSeasonLookup, LandHarvestSeason>
{
    protected override string TableName => "LandHarvestSeasons";
    protected override IReadOnlyList<RealEstateLookupEntry<LandHarvestSeason>> Entries =>
        LandCatalog.HarvestSeasons;
}

public class LandSoilTypeLookupConfiguration : RealEstateLookupConfiguration<LandSoilTypeLookup, LandSoilType>
{
    protected override string TableName => "LandSoilTypes";
    protected override IReadOnlyList<RealEstateLookupEntry<LandSoilType>> Entries => LandCatalog.SoilTypes;
}

public class LandIrrigationSourceLookupConfiguration
    : RealEstateLookupConfiguration<LandIrrigationSourceLookup, LandIrrigationSource>
{
    protected override string TableName => "LandIrrigationSources";
    protected override IReadOnlyList<RealEstateLookupEntry<LandIrrigationSource>> Entries =>
        LandCatalog.IrrigationSources;
}

public class LandQualityCertificateLookupConfiguration
    : RealEstateLookupConfiguration<LandQualityCertificateLookup, LandQualityCertificate>
{
    protected override string TableName => "LandQualityCertificates";
    protected override IReadOnlyList<RealEstateLookupEntry<LandQualityCertificate>> Entries =>
        LandCatalog.QualityCertificates;
}

public class LandExistingBuildingTypeLookupConfiguration
    : RealEstateLookupConfiguration<LandExistingBuildingTypeLookup, LandExistingBuildingType>
{
    protected override string TableName => "LandExistingBuildingTypes";
    protected override IReadOnlyList<RealEstateLookupEntry<LandExistingBuildingType>> Entries =>
        LandCatalog.ExistingBuildingTypes;
}

public class LandBuildingCompletionRatioLookupConfiguration
    : RealEstateLookupConfiguration<LandBuildingCompletionRatioLookup, LandBuildingCompletionRatio>
{
    protected override string TableName => "LandBuildingCompletionRatios";
    protected override IReadOnlyList<RealEstateLookupEntry<LandBuildingCompletionRatio>> Entries =>
        LandCatalog.BuildingCompletionRatios;
}
