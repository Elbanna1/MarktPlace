using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;
using Shared.Enums;

namespace Persistence.Configurations;

public class ApartmentConfiguration : IEntityTypeConfiguration<Apartment>
{
    public void Configure(EntityTypeBuilder<Apartment> builder)
    {
        builder.ToTable("Apartments");

        builder.HasKey(x => x.Id);

        RealEstateListingMapping.ApplyCommonColumns(builder);
        RealEstateListingMapping.ApplyLicenceColumns(builder);
        RealEstateListingMapping.ApplyRentColumns(builder);
        RealEstateListingMapping.ApplyExchangeColumns(builder);

        builder.Property(x => x.OtherApartmentType).HasMaxLength(150);

        builder.Property(x => x.ViolationDetails).HasMaxLength(4000);
        builder.Property(x => x.InstallmentPeriod).HasMaxLength(100);

        builder.Property(x => x.Price).HasColumnType("decimal(18,2)");
        builder.Property(x => x.PricePerMeter).HasColumnType("decimal(18,2)");
        builder.Property(x => x.DownPayment).HasColumnType("decimal(18,2)");
        builder.Property(x => x.InstallmentAmount).HasColumnType("decimal(18,2)");
        builder.Property(x => x.MaintenanceDepositAmount).HasColumnType("decimal(18,2)");
        builder.Property(x => x.MonthlyFees).HasColumnType("decimal(18,2)");
        builder.Property(x => x.RentDownPayment).HasColumnType("decimal(18,2)");

        builder.Property(x => x.Area).HasColumnType("decimal(18,2)");

        builder.Property(x => x.ApartmentType).HasConversion<int>();
        builder.Property(x => x.OwnershipType).HasConversion<int>();
        builder.Property(x => x.ReceptionPieces).HasConversion<int>();
        builder.Property(x => x.FloorType).HasConversion<int>();
        builder.Property(x => x.FurnishedStatus).HasConversion<int>();
        builder.Property(x => x.FinishingType).HasConversion<int>();
        builder.Property(x => x.PropertyAge).HasConversion<int>();
        builder.Property(x => x.Direction).HasConversion<int>();
        builder.Property(x => x.ViewType).HasConversion<int>();
        builder.Property(x => x.LegalStatus).HasConversion<int>();
        builder.Property(x => x.ReconciliationForm).HasConversion<int>();
        builder.Property(x => x.OwnershipDocument).HasConversion<int>();
        builder.Property(x => x.PaymentMethod).HasConversion<int>();
        builder.Property(x => x.InstallmentProvider).HasConversion<int>();
        builder.Property(x => x.RentType).HasConversion<int>();
        builder.Property(x => x.SuitableFor).HasConversion<int>();
        builder.Property(x => x.ExchangeWith).HasConversion<int>();

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(x => x.Images)
            .WithOne(i => i.Apartment)
            .HasForeignKey(i => i.ApartmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Features)
            .WithOne(f => f.Apartment)
            .HasForeignKey(f => f.ApartmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.RentInclusions)
            .WithOne(r => r.Apartment)
            .HasForeignKey(r => r.ApartmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(QueryFilterNames.SoftDelete, x => !x.IsDeleted);

        RealEstateListingMapping.ApplyCommonIndexes(builder);

        builder.HasIndex(x => x.ApartmentType);
        builder.HasIndex(x => x.Price);
        builder.HasIndex(x => x.Area);
        builder.HasIndex(x => x.RoomsCount);
        builder.HasIndex(x => x.BathroomsCount);
        builder.HasIndex(x => x.FloorType);
        builder.HasIndex(x => x.FinishingType);
        builder.HasIndex(x => x.FurnishedStatus);
        builder.HasIndex(x => x.OwnershipType);
        builder.HasIndex(x => x.LegalStatus);
        builder.HasIndex(x => x.OwnershipDocument);
    }
}

public class ApartmentImageConfiguration : RealEstateImageConfiguration<ApartmentImage>
{
    protected override string TableName => "ApartmentImages";

    protected override string ListingIdPropertyName => nameof(ApartmentImage.ApartmentId);

    protected override void ApplySoftDeleteFilter(EntityTypeBuilder<ApartmentImage> builder) =>
        builder.HasQueryFilter(i => !i.Apartment.IsDeleted);
}

public class ApartmentFeatureSelectionConfiguration : IEntityTypeConfiguration<ApartmentFeatureSelection>
{
    public void Configure(EntityTypeBuilder<ApartmentFeatureSelection> builder)
    {
        builder.ToTable("ApartmentFeatureSelections");

        builder.HasKey(x => new { x.ApartmentId, x.Feature });
        builder.Property(x => x.Feature).HasConversion<int>();

        builder.HasIndex(x => x.Feature);

        builder.HasQueryFilter(x => !x.Apartment.IsDeleted);
    }
}

public class ApartmentRentInclusionSelectionConfiguration
    : IEntityTypeConfiguration<ApartmentRentInclusionSelection>
{
    public void Configure(EntityTypeBuilder<ApartmentRentInclusionSelection> builder)
    {
        builder.ToTable("ApartmentRentInclusionSelections");

        builder.HasKey(x => new { x.ApartmentId, x.Inclusion });
        builder.Property(x => x.Inclusion).HasConversion<int>();

        builder.HasIndex(x => x.Inclusion);

        builder.HasQueryFilter(x => !x.Apartment.IsDeleted);
    }
}

public class ApartmentTypeLookupConfiguration : RealEstateLookupConfiguration<ApartmentTypeLookup, ApartmentType>
{
    protected override string TableName => "ApartmentTypes";
    protected override IReadOnlyList<RealEstateLookupEntry<ApartmentType>> Entries =>
        ApartmentCatalog.ApartmentTypes;
}

public class ApartmentOwnershipTypeLookupConfiguration
    : RealEstateLookupConfiguration<ApartmentOwnershipTypeLookup, ApartmentOwnershipType>
{
    protected override string TableName => "ApartmentOwnershipTypes";
    protected override IReadOnlyList<RealEstateLookupEntry<ApartmentOwnershipType>> Entries =>
        ApartmentCatalog.OwnershipTypes;
}

public class ApartmentReceptionPiecesLookupConfiguration
    : RealEstateLookupConfiguration<ApartmentReceptionPiecesLookup, ApartmentReceptionPieces>
{
    protected override string TableName => "ApartmentReceptionPieces";
    protected override IReadOnlyList<RealEstateLookupEntry<ApartmentReceptionPieces>> Entries =>
        ApartmentCatalog.ReceptionPieces;
}

public class ApartmentFloorTypeLookupConfiguration
    : RealEstateLookupConfiguration<ApartmentFloorTypeLookup, ApartmentFloorType>
{
    protected override string TableName => "ApartmentFloorTypes";
    protected override IReadOnlyList<RealEstateLookupEntry<ApartmentFloorType>> Entries =>
        ApartmentCatalog.FloorTypes;
}

public class ApartmentFurnishedStatusLookupConfiguration
    : RealEstateLookupConfiguration<ApartmentFurnishedStatusLookup, ApartmentFurnishedStatus>
{
    protected override string TableName => "ApartmentFurnishedStatuses";
    protected override IReadOnlyList<RealEstateLookupEntry<ApartmentFurnishedStatus>> Entries =>
        ApartmentCatalog.FurnishedStatuses;
}

public class ApartmentFinishingTypeLookupConfiguration
    : RealEstateLookupConfiguration<ApartmentFinishingTypeLookup, ApartmentFinishingType>
{
    protected override string TableName => "ApartmentFinishingTypes";
    protected override IReadOnlyList<RealEstateLookupEntry<ApartmentFinishingType>> Entries =>
        ApartmentCatalog.FinishingTypes;
}

public class ApartmentPropertyAgeLookupConfiguration
    : RealEstateLookupConfiguration<ApartmentPropertyAgeLookup, ApartmentPropertyAge>
{
    protected override string TableName => "ApartmentPropertyAges";
    protected override IReadOnlyList<RealEstateLookupEntry<ApartmentPropertyAge>> Entries =>
        ApartmentCatalog.PropertyAges;
}

public class ApartmentDirectionLookupConfiguration
    : RealEstateLookupConfiguration<ApartmentDirectionLookup, ApartmentDirection>
{
    protected override string TableName => "ApartmentDirections";
    protected override IReadOnlyList<RealEstateLookupEntry<ApartmentDirection>> Entries =>
        ApartmentCatalog.Directions;
}

public class ApartmentViewTypeLookupConfiguration
    : RealEstateLookupConfiguration<ApartmentViewTypeLookup, ApartmentViewType>
{
    protected override string TableName => "ApartmentViewTypes";
    protected override IReadOnlyList<RealEstateLookupEntry<ApartmentViewType>> Entries =>
        ApartmentCatalog.ViewTypes;
}

public class ApartmentLegalStatusLookupConfiguration
    : RealEstateLookupConfiguration<ApartmentLegalStatusLookup, ApartmentLegalStatus>
{
    protected override string TableName => "ApartmentLegalStatuses";
    protected override IReadOnlyList<RealEstateLookupEntry<ApartmentLegalStatus>> Entries =>
        ApartmentCatalog.LegalStatuses;
}

public class ApartmentReconciliationFormLookupConfiguration
    : RealEstateLookupConfiguration<ApartmentReconciliationFormLookup, ApartmentReconciliationForm>
{
    protected override string TableName => "ApartmentReconciliationForms";
    protected override IReadOnlyList<RealEstateLookupEntry<ApartmentReconciliationForm>> Entries =>
        ApartmentCatalog.ReconciliationForms;
}

public class ApartmentOwnershipDocumentLookupConfiguration
    : RealEstateLookupConfiguration<ApartmentOwnershipDocumentLookup, ApartmentOwnershipDocument>
{
    protected override string TableName => "ApartmentOwnershipDocuments";
    protected override IReadOnlyList<RealEstateLookupEntry<ApartmentOwnershipDocument>> Entries =>
        ApartmentCatalog.OwnershipDocuments;
}

public class ApartmentFeatureLookupConfiguration
    : RealEstateLookupConfiguration<ApartmentFeatureLookup, ApartmentFeature>
{
    protected override string TableName => "ApartmentFeatures";
    protected override IReadOnlyList<RealEstateLookupEntry<ApartmentFeature>> Entries =>
        ApartmentCatalog.Features;
}

public class ApartmentPaymentMethodLookupConfiguration
    : RealEstateLookupConfiguration<ApartmentPaymentMethodLookup, ApartmentPaymentMethod>
{
    protected override string TableName => "ApartmentPaymentMethods";
    protected override IReadOnlyList<RealEstateLookupEntry<ApartmentPaymentMethod>> Entries =>
        ApartmentCatalog.PaymentMethods;
}

public class ApartmentInstallmentProviderLookupConfiguration
    : RealEstateLookupConfiguration<ApartmentInstallmentProviderLookup, ApartmentInstallmentProvider>
{
    protected override string TableName => "ApartmentInstallmentProviders";
    protected override IReadOnlyList<RealEstateLookupEntry<ApartmentInstallmentProvider>> Entries =>
        ApartmentCatalog.InstallmentProviders;
}

public class ApartmentRentTypeLookupConfiguration
    : RealEstateLookupConfiguration<ApartmentRentTypeLookup, ApartmentRentType>
{
    protected override string TableName => "ApartmentRentTypes";
    protected override IReadOnlyList<RealEstateLookupEntry<ApartmentRentType>> Entries =>
        ApartmentCatalog.RentTypes;
}

public class ApartmentRentInclusionLookupConfiguration
    : RealEstateLookupConfiguration<ApartmentRentInclusionLookup, ApartmentRentInclusion>
{
    protected override string TableName => "ApartmentRentInclusions";
    protected override IReadOnlyList<RealEstateLookupEntry<ApartmentRentInclusion>> Entries =>
        ApartmentCatalog.RentInclusions;
}

public class ApartmentSuitableForLookupConfiguration
    : RealEstateLookupConfiguration<ApartmentSuitableForLookup, ApartmentSuitableFor>
{
    protected override string TableName => "ApartmentSuitableFor";
    protected override IReadOnlyList<RealEstateLookupEntry<ApartmentSuitableFor>> Entries =>
        ApartmentCatalog.SuitableFor;
}

public class ApartmentExchangeWithLookupConfiguration
    : RealEstateLookupConfiguration<ApartmentExchangeWithLookup, ApartmentExchangeWith>
{
    protected override string TableName => "ApartmentExchangeTargets";
    protected override IReadOnlyList<RealEstateLookupEntry<ApartmentExchangeWith>> Entries =>
        ApartmentCatalog.ExchangeTargets;
}
