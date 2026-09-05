using Shared.Constants;
using Shared.DTOs.Lookups.Forms;

namespace Services.AdForms;

internal static class RealEstateFormLookups
{
    public static bool TryApply(CreateAdFormLookupsDto lookups, string key)
    {
        switch (key)
        {
            case AdFormLookupKeys.RealEstateListingTypes:
                lookups.RealEstateListingTypes = RealEstateCatalog.ListingTypeOptions;
                return true;
            case AdFormLookupKeys.RealEstateProjects:
                lookups.RealEstateProjects = RealEstateCatalog.ProjectOptions;
                return true;

            case AdFormLookupKeys.LandTypes:
                lookups.LandTypes = LandCatalog.LandTypeOptions;
                return true;
            case AdFormLookupKeys.LandAreaUnits:
                lookups.LandAreaUnits = LandCatalog.AreaUnitOptions;
                return true;
            case AdFormLookupKeys.LandFacadesCounts:
                lookups.LandFacadesCounts = LandCatalog.FacadesCountOptions;
                return true;
            case AdFormLookupKeys.LandDirections:
                lookups.LandDirections = LandCatalog.DirectionOptions;
                return true;
            case AdFormLookupKeys.LandRoadTypes:
                lookups.LandRoadTypes = LandCatalog.RoadTypeOptions;
                return true;
            case AdFormLookupKeys.LandLegalStatuses:
                lookups.LandLegalStatuses = LandCatalog.LegalStatusOptions;
                return true;
            case AdFormLookupKeys.LandReconciliationForms:
                lookups.LandReconciliationForms = LandCatalog.ReconciliationFormOptions;
                return true;
            case AdFormLookupKeys.LandOwnershipDocuments:
                lookups.LandOwnershipDocuments = LandCatalog.OwnershipDocumentOptions;
                return true;
            case AdFormLookupKeys.LandUtilities:
                lookups.LandUtilities = LandCatalog.UtilityOptions;
                return true;
            case AdFormLookupKeys.LandRentTypes:
                lookups.LandRentTypes = LandCatalog.RentTypeOptions;
                return true;
            case AdFormLookupKeys.LandMinimumRentPeriods:
                lookups.LandMinimumRentPeriods = LandCatalog.MinimumRentPeriodOptions;
                return true;
            case AdFormLookupKeys.LandRentInclusions:
                lookups.LandRentInclusions = LandCatalog.RentInclusionOptions;
                return true;
            case AdFormLookupKeys.LandContractDurations:
                lookups.LandContractDurations = LandCatalog.ContractDurationOptions;
                return true;
            case AdFormLookupKeys.LandExchangeTargets:
                lookups.LandExchangeTargets = LandCatalog.ExchangeTargetOptions;
                return true;
            case AdFormLookupKeys.LandHarvestSeasons:
                lookups.LandHarvestSeasons = LandCatalog.HarvestSeasonOptions;
                return true;
            case AdFormLookupKeys.LandSoilTypes:
                lookups.LandSoilTypes = LandCatalog.SoilTypeOptions;
                return true;
            case AdFormLookupKeys.LandIrrigationSources:
                lookups.LandIrrigationSources = LandCatalog.IrrigationSourceOptions;
                return true;
            case AdFormLookupKeys.LandQualityCertificates:
                lookups.LandQualityCertificates = LandCatalog.QualityCertificateOptions;
                return true;
            case AdFormLookupKeys.LandExistingBuildingTypes:
                lookups.LandExistingBuildingTypes = LandCatalog.ExistingBuildingTypeOptions;
                return true;
            case AdFormLookupKeys.LandBuildingCompletionRatios:
                lookups.LandBuildingCompletionRatios = LandCatalog.BuildingCompletionRatioOptions;
                return true;

            case AdFormLookupKeys.ApartmentTypes:
                lookups.ApartmentTypes = ApartmentCatalog.ApartmentTypeOptions;
                return true;
            case AdFormLookupKeys.ApartmentOwnershipTypes:
                lookups.ApartmentOwnershipTypes = ApartmentCatalog.OwnershipTypeOptions;
                return true;
            case AdFormLookupKeys.ApartmentReceptionPieces:
                lookups.ApartmentReceptionPieces = ApartmentCatalog.ReceptionPieceOptions;
                return true;
            case AdFormLookupKeys.ApartmentFloorTypes:
                lookups.ApartmentFloorTypes = ApartmentCatalog.FloorTypeOptions;
                return true;
            case AdFormLookupKeys.ApartmentFurnishedStatuses:
                lookups.ApartmentFurnishedStatuses = ApartmentCatalog.FurnishedStatusOptions;
                return true;
            case AdFormLookupKeys.ApartmentFinishingTypes:
                lookups.ApartmentFinishingTypes = ApartmentCatalog.FinishingTypeOptions;
                return true;
            case AdFormLookupKeys.ApartmentPropertyAges:
                lookups.ApartmentPropertyAges = ApartmentCatalog.PropertyAgeOptions;
                return true;
            case AdFormLookupKeys.ApartmentDirections:
                lookups.ApartmentDirections = ApartmentCatalog.DirectionOptions;
                return true;
            case AdFormLookupKeys.ApartmentViewTypes:
                lookups.ApartmentViewTypes = ApartmentCatalog.ViewTypeOptions;
                return true;
            case AdFormLookupKeys.ApartmentLegalStatuses:
                lookups.ApartmentLegalStatuses = ApartmentCatalog.LegalStatusOptions;
                return true;
            case AdFormLookupKeys.ApartmentReconciliationForms:
                lookups.ApartmentReconciliationForms = ApartmentCatalog.ReconciliationFormOptions;
                return true;
            case AdFormLookupKeys.ApartmentOwnershipDocuments:
                lookups.ApartmentOwnershipDocuments = ApartmentCatalog.OwnershipDocumentOptions;
                return true;
            case AdFormLookupKeys.ApartmentFeatures:
                lookups.ApartmentFeatures = ApartmentCatalog.FeatureOptions;
                return true;
            case AdFormLookupKeys.ApartmentPaymentMethods:
                lookups.ApartmentPaymentMethods = ApartmentCatalog.PaymentMethodOptions;
                return true;
            case AdFormLookupKeys.ApartmentInstallmentProviders:
                lookups.ApartmentInstallmentProviders = ApartmentCatalog.InstallmentProviderOptions;
                return true;
            case AdFormLookupKeys.ApartmentRentTypes:
                lookups.ApartmentRentTypes = ApartmentCatalog.RentTypeOptions;
                return true;
            case AdFormLookupKeys.ApartmentRentInclusions:
                lookups.ApartmentRentInclusions = ApartmentCatalog.RentInclusionOptions;
                return true;
            case AdFormLookupKeys.ApartmentSuitableFor:
                lookups.ApartmentSuitableFor = ApartmentCatalog.SuitableForOptions;
                return true;
            case AdFormLookupKeys.ApartmentExchangeTargets:
                lookups.ApartmentExchangeTargets = ApartmentCatalog.ExchangeTargetOptions;
                return true;

            case AdFormLookupKeys.ShopSuitableActivities:
                lookups.ShopSuitableActivities = ShopCatalog.SuitableActivityOptions;
                return true;
            case AdFormLookupKeys.ShopFloorTypes:
                lookups.ShopFloorTypes = ShopCatalog.FloorTypeOptions;
                return true;
            case AdFormLookupKeys.ShopFacadesCounts:
                lookups.ShopFacadesCounts = ShopCatalog.FacadesCountOptions;
                return true;
            case AdFormLookupKeys.ShopFacadeDirections:
                lookups.ShopFacadeDirections = ShopCatalog.FacadeDirectionOptions;
                return true;
            case AdFormLookupKeys.ShopFinishingTypes:
                lookups.ShopFinishingTypes = ShopCatalog.FinishingTypeOptions;
                return true;
            case AdFormLookupKeys.ShopPropertyAges:
                lookups.ShopPropertyAges = ShopCatalog.PropertyAgeOptions;
                return true;
            case AdFormLookupKeys.ShopEntrancesCounts:
                lookups.ShopEntrancesCounts = ShopCatalog.EntrancesCountOptions;
                return true;
            case AdFormLookupKeys.ShopLegalStatuses:
                lookups.ShopLegalStatuses = ShopCatalog.LegalStatusOptions;
                return true;
            case AdFormLookupKeys.ShopLicenseTypes:
                lookups.ShopLicenseTypes = ShopCatalog.LicenseTypeOptions;
                return true;
            case AdFormLookupKeys.ShopReconciliationForms:
                lookups.ShopReconciliationForms = ShopCatalog.ReconciliationFormOptions;
                return true;
            case AdFormLookupKeys.ShopOwnershipDocuments:
                lookups.ShopOwnershipDocuments = ShopCatalog.OwnershipDocumentOptions;
                return true;
            case AdFormLookupKeys.ShopUtilities:
                lookups.ShopUtilities = ShopCatalog.UtilityOptions;
                return true;
            case AdFormLookupKeys.ShopPaymentMethods:
                lookups.ShopPaymentMethods = ShopCatalog.PaymentMethodOptions;
                return true;
            case AdFormLookupKeys.ShopInstallmentProviders:
                lookups.ShopInstallmentProviders = ShopCatalog.InstallmentProviderOptions;
                return true;
            case AdFormLookupKeys.ShopRentTypes:
                lookups.ShopRentTypes = ShopCatalog.RentTypeOptions;
                return true;
            case AdFormLookupKeys.ShopRentInclusions:
                lookups.ShopRentInclusions = ShopCatalog.RentInclusionOptions;
                return true;
            case AdFormLookupKeys.ShopRentSuitableActivities:
                lookups.ShopRentSuitableActivities = ShopCatalog.RentSuitableActivityOptions;
                return true;
            case AdFormLookupKeys.ShopExchangeTargets:
                lookups.ShopExchangeTargets = ShopCatalog.ExchangeTargetOptions;
                return true;

            default:
                return false;
        }
    }
}
