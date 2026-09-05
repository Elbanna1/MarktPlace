using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;
using Shared.Enums;

namespace Persistence.Configurations;

public class ShopConfiguration : IEntityTypeConfiguration<Shop>
{
    public void Configure(EntityTypeBuilder<Shop> builder)
    {
        builder.ToTable("Shops");

        builder.HasKey(x => x.Id);

        RealEstateListingMapping.ApplyCommonColumns(builder);
        RealEstateListingMapping.ApplyLicenceColumns(builder);
        RealEstateListingMapping.ApplyRentColumns(builder);
        RealEstateListingMapping.ApplyExchangeColumns(builder);

        builder.Property(x => x.OtherSuitableActivity).HasMaxLength(150);
        builder.Property(x => x.OtherReconciliationForm).HasMaxLength(150);
        builder.Property(x => x.OtherOwnershipDocument).HasMaxLength(150);

        builder.Property(x => x.InstallmentPeriod).HasMaxLength(100);
        builder.Property(x => x.PreviousActivity).HasMaxLength(150);
        builder.Property(x => x.PreviousOperatingPeriod).HasMaxLength(100);
        builder.Property(x => x.VacancyReason).HasMaxLength(1000);

        builder.Property(x => x.Price).HasColumnType("decimal(18,2)");
        builder.Property(x => x.DownPayment).HasColumnType("decimal(18,2)");
        builder.Property(x => x.InstallmentAmount).HasColumnType("decimal(18,2)");
        builder.Property(x => x.RentDownPayment).HasColumnType("decimal(18,2)");

        builder.Property(x => x.Area).HasColumnType("decimal(18,2)");
        builder.Property(x => x.StorageArea).HasColumnType("decimal(18,2)");
        builder.Property(x => x.CeilingHeight).HasColumnType("decimal(10,2)");
        builder.Property(x => x.FacadeWidth).HasColumnType("decimal(10,2)");

        builder.Property(x => x.SuitableActivity).HasConversion<int>();
        builder.Property(x => x.FloorType).HasConversion<int>();
        builder.Property(x => x.FacadesCount).HasConversion<int>();
        builder.Property(x => x.FacadeDirection).HasConversion<int>();
        builder.Property(x => x.FinishingType).HasConversion<int>();
        builder.Property(x => x.PropertyAge).HasConversion<int>();
        builder.Property(x => x.EntrancesCount).HasConversion<int>();
        builder.Property(x => x.LegalStatus).HasConversion<int>();
        builder.Property(x => x.LicenseType).HasConversion<int>();
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
            .WithOne(i => i.Shop)
            .HasForeignKey(i => i.ShopId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Utilities)
            .WithOne(u => u.Shop)
            .HasForeignKey(u => u.ShopId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.RentInclusions)
            .WithOne(r => r.Shop)
            .HasForeignKey(r => r.ShopId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.RentSuitableActivities)
            .WithOne(a => a.Shop)
            .HasForeignKey(a => a.ShopId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(QueryFilterNames.SoftDelete, x => !x.IsDeleted);

        RealEstateListingMapping.ApplyCommonIndexes(builder);

        builder.HasIndex(x => x.Price);
        builder.HasIndex(x => x.SuitableActivity);
        builder.HasIndex(x => x.Area);
        builder.HasIndex(x => x.FloorType);
        builder.HasIndex(x => x.FinishingType);
        builder.HasIndex(x => x.FacadesCount);
        builder.HasIndex(x => x.HasStorage);
        builder.HasIndex(x => x.HasBathroom);
        builder.HasIndex(x => x.LegalStatus);
        builder.HasIndex(x => x.LicenseType);
        builder.HasIndex(x => x.OwnershipDocument);
    }
}

public class ShopImageConfiguration : RealEstateImageConfiguration<ShopImage>
{
    protected override string TableName => "ShopImages";

    protected override string ListingIdPropertyName => nameof(ShopImage.ShopId);

    protected override void ApplySoftDeleteFilter(EntityTypeBuilder<ShopImage> builder) =>
        builder.HasQueryFilter(i => !i.Shop.IsDeleted);
}

public class ShopUtilitySelectionConfiguration : IEntityTypeConfiguration<ShopUtilitySelection>
{
    public void Configure(EntityTypeBuilder<ShopUtilitySelection> builder)
    {
        builder.ToTable("ShopUtilitySelections");

        builder.HasKey(x => new { x.ShopId, x.Utility });
        builder.Property(x => x.Utility).HasConversion<int>();

        builder.HasIndex(x => x.Utility);

        builder.HasQueryFilter(x => !x.Shop.IsDeleted);
    }
}

public class ShopRentInclusionSelectionConfiguration : IEntityTypeConfiguration<ShopRentInclusionSelection>
{
    public void Configure(EntityTypeBuilder<ShopRentInclusionSelection> builder)
    {
        builder.ToTable("ShopRentInclusionSelections");

        builder.HasKey(x => new { x.ShopId, x.Inclusion });
        builder.Property(x => x.Inclusion).HasConversion<int>();

        builder.HasIndex(x => x.Inclusion);

        builder.HasQueryFilter(x => !x.Shop.IsDeleted);
    }
}

public class ShopRentSuitableActivitySelectionConfiguration
    : IEntityTypeConfiguration<ShopRentSuitableActivitySelection>
{
    public void Configure(EntityTypeBuilder<ShopRentSuitableActivitySelection> builder)
    {
        builder.ToTable("ShopRentSuitableActivitySelections");

        builder.HasKey(x => new { x.ShopId, x.Activity });
        builder.Property(x => x.Activity).HasConversion<int>();

        builder.HasIndex(x => x.Activity);

        builder.HasQueryFilter(x => !x.Shop.IsDeleted);
    }
}

public class ShopSuitableActivityLookupConfiguration
    : RealEstateLookupConfiguration<ShopSuitableActivityLookup, ShopSuitableActivity>
{
    protected override string TableName => "ShopSuitableActivities";
    protected override IReadOnlyList<RealEstateLookupEntry<ShopSuitableActivity>> Entries =>
        ShopCatalog.SuitableActivities;
}

public class ShopFloorTypeLookupConfiguration : RealEstateLookupConfiguration<ShopFloorTypeLookup, ShopFloorType>
{
    protected override string TableName => "ShopFloorTypes";
    protected override IReadOnlyList<RealEstateLookupEntry<ShopFloorType>> Entries => ShopCatalog.FloorTypes;
}

public class ShopFacadesCountLookupConfiguration
    : RealEstateLookupConfiguration<ShopFacadesCountLookup, ShopFacadesCount>
{
    protected override string TableName => "ShopFacadesCounts";
    protected override IReadOnlyList<RealEstateLookupEntry<ShopFacadesCount>> Entries => ShopCatalog.FacadesCounts;
}

public class ShopFacadeDirectionLookupConfiguration
    : RealEstateLookupConfiguration<ShopFacadeDirectionLookup, ShopFacadeDirection>
{
    protected override string TableName => "ShopFacadeDirections";
    protected override IReadOnlyList<RealEstateLookupEntry<ShopFacadeDirection>> Entries =>
        ShopCatalog.FacadeDirections;
}

public class ShopFinishingTypeLookupConfiguration
    : RealEstateLookupConfiguration<ShopFinishingTypeLookup, ShopFinishingType>
{
    protected override string TableName => "ShopFinishingTypes";
    protected override IReadOnlyList<RealEstateLookupEntry<ShopFinishingType>> Entries =>
        ShopCatalog.FinishingTypes;
}

public class ShopPropertyAgeLookupConfiguration
    : RealEstateLookupConfiguration<ShopPropertyAgeLookup, ShopPropertyAge>
{
    protected override string TableName => "ShopPropertyAges";
    protected override IReadOnlyList<RealEstateLookupEntry<ShopPropertyAge>> Entries => ShopCatalog.PropertyAges;
}

public class ShopEntrancesCountLookupConfiguration
    : RealEstateLookupConfiguration<ShopEntrancesCountLookup, ShopEntrancesCount>
{
    protected override string TableName => "ShopEntrancesCounts";
    protected override IReadOnlyList<RealEstateLookupEntry<ShopEntrancesCount>> Entries =>
        ShopCatalog.EntrancesCounts;
}

public class ShopLegalStatusLookupConfiguration
    : RealEstateLookupConfiguration<ShopLegalStatusLookup, ShopLegalStatus>
{
    protected override string TableName => "ShopLegalStatuses";
    protected override IReadOnlyList<RealEstateLookupEntry<ShopLegalStatus>> Entries => ShopCatalog.LegalStatuses;
}

public class ShopLicenseTypeLookupConfiguration
    : RealEstateLookupConfiguration<ShopLicenseTypeLookup, ShopLicenseType>
{
    protected override string TableName => "ShopLicenseTypes";
    protected override IReadOnlyList<RealEstateLookupEntry<ShopLicenseType>> Entries => ShopCatalog.LicenseTypes;
}

public class ShopReconciliationFormLookupConfiguration
    : RealEstateLookupConfiguration<ShopReconciliationFormLookup, ShopReconciliationForm>
{
    protected override string TableName => "ShopReconciliationForms";
    protected override IReadOnlyList<RealEstateLookupEntry<ShopReconciliationForm>> Entries =>
        ShopCatalog.ReconciliationForms;
}

public class ShopOwnershipDocumentLookupConfiguration
    : RealEstateLookupConfiguration<ShopOwnershipDocumentLookup, ShopOwnershipDocument>
{
    protected override string TableName => "ShopOwnershipDocuments";
    protected override IReadOnlyList<RealEstateLookupEntry<ShopOwnershipDocument>> Entries =>
        ShopCatalog.OwnershipDocuments;
}

public class ShopUtilityLookupConfiguration : RealEstateLookupConfiguration<ShopUtilityLookup, ShopUtility>
{
    protected override string TableName => "ShopUtilities";
    protected override IReadOnlyList<RealEstateLookupEntry<ShopUtility>> Entries => ShopCatalog.Utilities;
}

public class ShopPaymentMethodLookupConfiguration
    : RealEstateLookupConfiguration<ShopPaymentMethodLookup, ShopPaymentMethod>
{
    protected override string TableName => "ShopPaymentMethods";
    protected override IReadOnlyList<RealEstateLookupEntry<ShopPaymentMethod>> Entries =>
        ShopCatalog.PaymentMethods;
}

public class ShopInstallmentProviderLookupConfiguration
    : RealEstateLookupConfiguration<ShopInstallmentProviderLookup, ShopInstallmentProvider>
{
    protected override string TableName => "ShopInstallmentProviders";
    protected override IReadOnlyList<RealEstateLookupEntry<ShopInstallmentProvider>> Entries =>
        ShopCatalog.InstallmentProviders;
}

public class ShopRentTypeLookupConfiguration : RealEstateLookupConfiguration<ShopRentTypeLookup, ShopRentType>
{
    protected override string TableName => "ShopRentTypes";
    protected override IReadOnlyList<RealEstateLookupEntry<ShopRentType>> Entries => ShopCatalog.RentTypes;
}

public class ShopRentInclusionLookupConfiguration
    : RealEstateLookupConfiguration<ShopRentInclusionLookup, ShopRentInclusion>
{
    protected override string TableName => "ShopRentInclusions";
    protected override IReadOnlyList<RealEstateLookupEntry<ShopRentInclusion>> Entries =>
        ShopCatalog.RentInclusions;
}

public class ShopRentSuitableActivityLookupConfiguration
    : RealEstateLookupConfiguration<ShopRentSuitableActivityLookup, ShopRentSuitableActivity>
{
    protected override string TableName => "ShopRentSuitableActivities";
    protected override IReadOnlyList<RealEstateLookupEntry<ShopRentSuitableActivity>> Entries =>
        ShopCatalog.RentSuitableActivities;
}

public class ShopExchangeWithLookupConfiguration
    : RealEstateLookupConfiguration<ShopExchangeWithLookup, ShopExchangeWith>
{
    protected override string TableName => "ShopExchangeTargets";
    protected override IReadOnlyList<RealEstateLookupEntry<ShopExchangeWith>> Entries =>
        ShopCatalog.ExchangeTargets;
}
