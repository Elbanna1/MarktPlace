namespace Domain.Entities;

public interface IRealEstateLookup
{
    int Id { get; set; }

    string Name { get; set; }

    string NameEn { get; set; }
}

public class RealEstateListingTypeLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class RealEstateProjectLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class LandTypeLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class LandAreaUnitLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class LandFacadesCountLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class LandDirectionLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class LandRoadTypeLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class LandLegalStatusLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class LandReconciliationFormLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class LandOwnershipDocumentLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class LandUtilityLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class LandRentTypeLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class LandMinimumRentPeriodLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class LandRentInclusionLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class LandContractDurationLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class LandExchangeWithLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class LandHarvestSeasonLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class LandSoilTypeLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class LandIrrigationSourceLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class LandQualityCertificateLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class LandExistingBuildingTypeLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class LandBuildingCompletionRatioLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class ApartmentTypeLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class ApartmentOwnershipTypeLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class ApartmentReceptionPiecesLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class ApartmentFloorTypeLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class ApartmentFurnishedStatusLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class ApartmentFinishingTypeLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class ApartmentPropertyAgeLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class ApartmentDirectionLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class ApartmentViewTypeLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class ApartmentLegalStatusLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class ApartmentReconciliationFormLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class ApartmentOwnershipDocumentLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class ApartmentFeatureLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class ApartmentPaymentMethodLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class ApartmentInstallmentProviderLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class ApartmentRentTypeLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class ApartmentRentInclusionLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class ApartmentSuitableForLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class ApartmentExchangeWithLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class ShopSuitableActivityLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class ShopFloorTypeLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class ShopFacadesCountLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class ShopFacadeDirectionLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class ShopFinishingTypeLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class ShopPropertyAgeLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class ShopEntrancesCountLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class ShopLegalStatusLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class ShopLicenseTypeLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class ShopReconciliationFormLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class ShopOwnershipDocumentLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class ShopUtilityLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class ShopPaymentMethodLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class ShopInstallmentProviderLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class ShopRentTypeLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class ShopRentInclusionLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class ShopRentSuitableActivityLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class ShopExchangeWithLookup : IRealEstateLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}
