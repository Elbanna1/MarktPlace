using Shared.DTOs.RealEstate;
using Shared.Enums;

namespace Shared.Constants;

public static class LandCatalog
{
    public static readonly IReadOnlyList<RealEstateLookupEntry<LandType>> LandTypes =
        new List<RealEstateLookupEntry<LandType>>
        {
            new(LandType.Residential, "أرض سكنية", "Residential land"),
            new(LandType.Building, "أرض مباني", "Building land"),
            new(LandType.Commercial, "أرض تجارية", "Commercial land"),
            new(LandType.Administrative, "أرض إدارية", "Administrative land"),
            new(LandType.Industrial, "أرض صناعية", "Industrial land"),
            new(LandType.Agricultural, "أرض زراعية", "Agricultural land"),
            new(LandType.Reclamation, "أرض استصلاح", "Reclamation land"),
            new(LandType.Cemeteries, "مقابر", "Cemeteries"),
            new(LandType.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<RealEstateLookupEntry<LandAreaUnit>> AreaUnits =
        new List<RealEstateLookupEntry<LandAreaUnit>>
        {
            new(LandAreaUnit.SquareMeter, "متر مربع", "Square meter"),
            new(LandAreaUnit.Feddan, "فدان", "Feddan")
        };

    public static readonly IReadOnlyList<RealEstateLookupEntry<LandFacadesCount>> FacadesCounts =
        new List<RealEstateLookupEntry<LandFacadesCount>>
        {
            new(LandFacadesCount.One, "واجهة واحدة", "One facade"),
            new(LandFacadesCount.Two, "واجهتان", "Two facades"),
            new(LandFacadesCount.Three, "ثلاث واجهات", "Three facades"),
            new(LandFacadesCount.Four, "أربع واجهات", "Four facades")
        };

    public static readonly IReadOnlyList<RealEstateLookupEntry<LandDirection>> Directions =
        new List<RealEstateLookupEntry<LandDirection>>
        {
            new(LandDirection.North, "بحري", "North"),
            new(LandDirection.South, "قبلي", "South"),
            new(LandDirection.East, "شرقي", "East"),
            new(LandDirection.West, "غربي", "West"),
            new(LandDirection.Corner, "ناصية", "Corner")
        };

    public static readonly IReadOnlyList<RealEstateLookupEntry<LandRoadType>> RoadTypes =
        new List<RealEstateLookupEntry<LandRoadType>>
        {
            new(LandRoadType.Asphalt, "أسفلت", "Asphalt"),
            new(LandRoadType.Interlock, "إنترلوك", "Interlock"),
            new(LandRoadType.Dirt, "ترابي", "Dirt"),
            new(LandRoadType.Graded, "ممهد", "Graded")
        };

    public static readonly IReadOnlyList<RealEstateLookupEntry<LandLegalStatus>> LegalStatuses =
        new List<RealEstateLookupEntry<LandLegalStatus>>
        {
            new(LandLegalStatus.Licensed, "مرخصة", "Licensed"),
            new(LandLegalStatus.Reconciliation, "تصالح", "Reconciliation"),
            new(LandLegalStatus.Unlicensed, "غير مرخصة", "Unlicensed")
        };

    public static readonly IReadOnlyList<RealEstateLookupEntry<LandReconciliationForm>> ReconciliationForms =
        new List<RealEstateLookupEntry<LandReconciliationForm>>
        {
            new(LandReconciliationForm.Form1, "نموذج 1", "Form 1"),
            new(LandReconciliationForm.Form3, "نموذج 3", "Form 3"),
            new(LandReconciliationForm.Form8, "نموذج 8", "Form 8"),
            new(LandReconciliationForm.Form10, "نموذج 10", "Form 10"),
            new(LandReconciliationForm.FinalForm, "نموذج نهائي", "Final form"),
            new(LandReconciliationForm.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<RealEstateLookupEntry<LandOwnershipDocument>> OwnershipDocuments =
        new List<RealEstateLookupEntry<LandOwnershipDocument>>
        {
            new(LandOwnershipDocument.FinalContract, "عقد نهائي", "Final contract"),
            new(LandOwnershipDocument.GreenContract, "عقد أخضر", "Green contract"),
            new(LandOwnershipDocument.PreliminaryContract, "عقد ابتدائي", "Preliminary contract"),
            new(LandOwnershipDocument.PowerOfAttorney, "توكيل", "Power of attorney"),
            new(LandOwnershipDocument.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<RealEstateLookupEntry<LandUtility>> Utilities =
        new List<RealEstateLookupEntry<LandUtility>>
        {
            new(LandUtility.ElectricityMeter, "عداد كهرباء", "Electricity meter"),
            new(LandUtility.WaterMeter, "عداد مياه", "Water meter"),
            new(LandUtility.NaturalGas, "غاز طبيعي", "Natural gas"),
            new(LandUtility.Sewage, "صرف صحي", "Sewage"),
            new(LandUtility.Internet, "إنترنت", "Internet"),
            new(LandUtility.WaterWell, "بئر مياه", "Water well"),
            new(LandUtility.IrrigationNetwork, "شبكة ري", "Irrigation network"),
            new(LandUtility.Walled, "مسورة", "Walled"),
            new(LandUtility.Gate, "بوابة", "Gate"),
            new(LandUtility.StreetLighting, "إنارة شارع", "Street lighting"),
            new(LandUtility.AgriculturalDrainage, "صرف زراعي", "Agricultural drainage")
        };

    public static readonly IReadOnlyList<RealEstateLookupEntry<LandRentType>> RentTypes =
        new List<RealEstateLookupEntry<LandRentType>>
        {
            new(LandRentType.Daily, "يومي", "Daily"),
            new(LandRentType.Weekly, "أسبوعي", "Weekly"),
            new(LandRentType.Monthly, "شهري", "Monthly"),
            new(LandRentType.Yearly, "سنوي", "Yearly")
        };

    public static readonly IReadOnlyList<RealEstateLookupEntry<LandMinimumRentPeriod>> MinimumRentPeriods =
        new List<RealEstateLookupEntry<LandMinimumRentPeriod>>
        {
            new(LandMinimumRentPeriod.Day, "يوم", "Day"),
            new(LandMinimumRentPeriod.Week, "أسبوع", "Week"),
            new(LandMinimumRentPeriod.Month, "شهر", "Month"),
            new(LandMinimumRentPeriod.Year, "سنة", "Year")
        };

    public static readonly IReadOnlyList<RealEstateLookupEntry<LandRentInclusion>> RentInclusions =
        new List<RealEstateLookupEntry<LandRentInclusion>>
        {
            new(LandRentInclusion.Electricity, "كهرباء", "Electricity"),
            new(LandRentInclusion.Water, "مياه", "Water"),
            new(LandRentInclusion.Gas, "غاز", "Gas"),
            new(LandRentInclusion.Maintenance, "صيانة", "Maintenance"),
            new(LandRentInclusion.Security, "حراسة", "Security")
        };

    public static readonly IReadOnlyList<RealEstateLookupEntry<LandContractDuration>> ContractDurations =
        new List<RealEstateLookupEntry<LandContractDuration>>
        {
            new(LandContractDuration.OneYear, "سنة", "One year"),
            new(LandContractDuration.TwoYears, "سنتان", "Two years"),
            new(LandContractDuration.ThreeYears, "ثلاث سنوات", "Three years"),
            new(LandContractDuration.AsAgreed, "حسب الاتفاق", "As agreed")
        };

    public static readonly IReadOnlyList<RealEstateLookupEntry<LandExchangeWith>> ExchangeTargets =
        new List<RealEstateLookupEntry<LandExchangeWith>>
        {
            new(LandExchangeWith.Land, "أرض", "Land"),
            new(LandExchangeWith.Apartment, "شقة", "Apartment"),
            new(LandExchangeWith.Shop, "محل", "Shop"),
            new(LandExchangeWith.Villa, "فيلا", "Villa"),
            new(LandExchangeWith.Farm, "مزرعة", "Farm"),
            new(LandExchangeWith.Car, "سيارة", "Car"),
            new(LandExchangeWith.Factory, "مصنع", "Factory"),
            new(LandExchangeWith.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<RealEstateLookupEntry<LandHarvestSeason>> HarvestSeasons =
        new List<RealEstateLookupEntry<LandHarvestSeason>>
        {
            new(LandHarvestSeason.Summer, "صيفي", "Summer"),
            new(LandHarvestSeason.Winter, "شتوي", "Winter"),
            new(LandHarvestSeason.AllYear, "طوال العام", "All year")
        };

    public static readonly IReadOnlyList<RealEstateLookupEntry<LandSoilType>> SoilTypes =
        new List<RealEstateLookupEntry<LandSoilType>>
        {
            new(LandSoilType.Clay, "طينية", "Clay"),
            new(LandSoilType.Sandy, "رملية", "Sandy"),
            new(LandSoilType.Yellow, "صفراء", "Yellow"),
            new(LandSoilType.Limestone, "جيرية", "Limestone"),
            new(LandSoilType.Mixed, "مختلطة", "Mixed")
        };

    public static readonly IReadOnlyList<RealEstateLookupEntry<LandIrrigationSource>> IrrigationSources =
        new List<RealEstateLookupEntry<LandIrrigationSource>>
        {
            new(LandIrrigationSource.Canal, "ترعة", "Canal"),
            new(LandIrrigationSource.Well, "بئر", "Well"),
            new(LandIrrigationSource.Drip, "تنقيط", "Drip"),
            new(LandIrrigationSource.Sprinkler, "رش", "Sprinkler"),
            new(LandIrrigationSource.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<RealEstateLookupEntry<LandQualityCertificate>> QualityCertificates =
        new List<RealEstateLookupEntry<LandQualityCertificate>>
        {
            new(LandQualityCertificate.GlobalGap, "Global GAP", "Global GAP"),
            new(LandQualityCertificate.Organic, "Organic", "Organic"),
            new(LandQualityCertificate.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<RealEstateLookupEntry<LandExistingBuildingType>> ExistingBuildingTypes =
        new List<RealEstateLookupEntry<LandExistingBuildingType>>
        {
            new(LandExistingBuildingType.House, "منزل", "House"),
            new(LandExistingBuildingType.Storage, "مخزن", "Storage"),
            new(LandExistingBuildingType.Shop, "محل", "Shop"),
            new(LandExistingBuildingType.Factory, "مصنع", "Factory"),
            new(LandExistingBuildingType.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<RealEstateLookupEntry<LandBuildingCompletionRatio>> BuildingCompletionRatios =
        new List<RealEstateLookupEntry<LandBuildingCompletionRatio>>
        {
            new(LandBuildingCompletionRatio.TwentyFivePercent, "25%", "25%"),
            new(LandBuildingCompletionRatio.FiftyPercent, "50%", "50%"),
            new(LandBuildingCompletionRatio.SeventyFivePercent, "75%", "75%"),
            new(LandBuildingCompletionRatio.Completed, "مكتمل", "Completed")
        };

    public static readonly IReadOnlyList<RealEstateLookupItemDto> LandTypeOptions = RealEstateCatalog.ToOptions(LandTypes);
    public static readonly IReadOnlyList<RealEstateLookupItemDto> AreaUnitOptions = RealEstateCatalog.ToOptions(AreaUnits);
    public static readonly IReadOnlyList<RealEstateLookupItemDto> FacadesCountOptions = RealEstateCatalog.ToOptions(FacadesCounts);
    public static readonly IReadOnlyList<RealEstateLookupItemDto> DirectionOptions = RealEstateCatalog.ToOptions(Directions);
    public static readonly IReadOnlyList<RealEstateLookupItemDto> RoadTypeOptions = RealEstateCatalog.ToOptions(RoadTypes);
    public static readonly IReadOnlyList<RealEstateLookupItemDto> LegalStatusOptions = RealEstateCatalog.ToOptions(LegalStatuses);
    public static readonly IReadOnlyList<RealEstateLookupItemDto> ReconciliationFormOptions = RealEstateCatalog.ToOptions(ReconciliationForms);
    public static readonly IReadOnlyList<RealEstateLookupItemDto> OwnershipDocumentOptions = RealEstateCatalog.ToOptions(OwnershipDocuments);
    public static readonly IReadOnlyList<RealEstateLookupItemDto> UtilityOptions = RealEstateCatalog.ToOptions(Utilities);
    public static readonly IReadOnlyList<RealEstateLookupItemDto> RentTypeOptions = RealEstateCatalog.ToOptions(RentTypes);
    public static readonly IReadOnlyList<RealEstateLookupItemDto> MinimumRentPeriodOptions = RealEstateCatalog.ToOptions(MinimumRentPeriods);
    public static readonly IReadOnlyList<RealEstateLookupItemDto> RentInclusionOptions = RealEstateCatalog.ToOptions(RentInclusions);
    public static readonly IReadOnlyList<RealEstateLookupItemDto> ContractDurationOptions = RealEstateCatalog.ToOptions(ContractDurations);
    public static readonly IReadOnlyList<RealEstateLookupItemDto> ExchangeTargetOptions = RealEstateCatalog.ToOptions(ExchangeTargets);
    public static readonly IReadOnlyList<RealEstateLookupItemDto> HarvestSeasonOptions = RealEstateCatalog.ToOptions(HarvestSeasons);
    public static readonly IReadOnlyList<RealEstateLookupItemDto> SoilTypeOptions = RealEstateCatalog.ToOptions(SoilTypes);
    public static readonly IReadOnlyList<RealEstateLookupItemDto> IrrigationSourceOptions = RealEstateCatalog.ToOptions(IrrigationSources);
    public static readonly IReadOnlyList<RealEstateLookupItemDto> QualityCertificateOptions = RealEstateCatalog.ToOptions(QualityCertificates);
    public static readonly IReadOnlyList<RealEstateLookupItemDto> ExistingBuildingTypeOptions = RealEstateCatalog.ToOptions(ExistingBuildingTypes);
    public static readonly IReadOnlyList<RealEstateLookupItemDto> BuildingCompletionRatioOptions = RealEstateCatalog.ToOptions(BuildingCompletionRatios);

    public static string GetLandTypeName(LandType value) => RealEstateCatalog.GetName(LandTypes, value);
    public static string? GetAreaUnitName(LandAreaUnit? value) => RealEstateCatalog.GetNameOrNull(AreaUnits, value);
    public static string? GetFacadesCountName(LandFacadesCount? value) => RealEstateCatalog.GetNameOrNull(FacadesCounts, value);
    public static string? GetDirectionName(LandDirection? value) => RealEstateCatalog.GetNameOrNull(Directions, value);
    public static string? GetRoadTypeName(LandRoadType? value) => RealEstateCatalog.GetNameOrNull(RoadTypes, value);
    public static string? GetLegalStatusName(LandLegalStatus? value) => RealEstateCatalog.GetNameOrNull(LegalStatuses, value);
    public static string? GetReconciliationFormName(LandReconciliationForm? value) => RealEstateCatalog.GetNameOrNull(ReconciliationForms, value);
    public static string? GetOwnershipDocumentName(LandOwnershipDocument? value) => RealEstateCatalog.GetNameOrNull(OwnershipDocuments, value);
    public static string? GetRentTypeName(LandRentType? value) => RealEstateCatalog.GetNameOrNull(RentTypes, value);
    public static string? GetMinimumRentPeriodName(LandMinimumRentPeriod? value) => RealEstateCatalog.GetNameOrNull(MinimumRentPeriods, value);
    public static string? GetContractDurationName(LandContractDuration? value) => RealEstateCatalog.GetNameOrNull(ContractDurations, value);
    public static string? GetExchangeTargetName(LandExchangeWith? value) => RealEstateCatalog.GetNameOrNull(ExchangeTargets, value);
    public static string? GetHarvestSeasonName(LandHarvestSeason? value) => RealEstateCatalog.GetNameOrNull(HarvestSeasons, value);
    public static string? GetSoilTypeName(LandSoilType? value) => RealEstateCatalog.GetNameOrNull(SoilTypes, value);
    public static string? GetIrrigationSourceName(LandIrrigationSource? value) => RealEstateCatalog.GetNameOrNull(IrrigationSources, value);
    public static string? GetQualityCertificateName(LandQualityCertificate? value) => RealEstateCatalog.GetNameOrNull(QualityCertificates, value);
    public static string? GetExistingBuildingTypeName(LandExistingBuildingType? value) => RealEstateCatalog.GetNameOrNull(ExistingBuildingTypes, value);
    public static string? GetBuildingCompletionRatioName(LandBuildingCompletionRatio? value) => RealEstateCatalog.GetNameOrNull(BuildingCompletionRatios, value);

    public static List<RealEstateLookupItemDto> SelectedUtilities(IEnumerable<LandUtility> selected) =>
        RealEstateCatalog.SelectedOptions(Utilities, selected);

    public static List<RealEstateLookupItemDto> SelectedRentInclusions(IEnumerable<LandRentInclusion> selected) =>
        RealEstateCatalog.SelectedOptions(RentInclusions, selected);
}
