using Shared.DTOs.RealEstate;
using Shared.Enums;

namespace Shared.Constants;

public static class ShopCatalog
{
    public static readonly IReadOnlyList<RealEstateLookupEntry<ShopSuitableActivity>> SuitableActivities =
        new List<RealEstateLookupEntry<ShopSuitableActivity>>
        {
            new(ShopSuitableActivity.Clothing, "ملابس", "Clothing"),
            new(ShopSuitableActivity.Shoes, "أحذية", "Shoes"),
            new(ShopSuitableActivity.Supermarket, "سوبر ماركت", "Supermarket"),
            new(ShopSuitableActivity.Restaurant, "مطعم", "Restaurant"),
            new(ShopSuitableActivity.Cafe, "كافيه", "Cafe"),
            new(ShopSuitableActivity.Bakery, "مخبز", "Bakery"),
            new(ShopSuitableActivity.Sweets, "حلويات", "Sweets"),
            new(ShopSuitableActivity.Pharmacy, "صيدلية", "Pharmacy"),
            new(ShopSuitableActivity.Office, "مكتب", "Office"),
            new(ShopSuitableActivity.Clinic, "عيادة", "Clinic"),
            new(ShopSuitableActivity.Showroom, "معرض", "Showroom"),
            new(ShopSuitableActivity.Workshop, "ورشة", "Workshop"),
            new(ShopSuitableActivity.Storage, "مخزن", "Storage"),
            new(ShopSuitableActivity.Bookstore, "مكتبة", "Bookstore"),
            new(ShopSuitableActivity.ElectricalTools, "أدوات كهربائية", "Electrical tools"),
            new(ShopSuitableActivity.Mobiles, "موبايلات", "Mobiles"),
            new(ShopSuitableActivity.Computers, "كمبيوتر", "Computers"),
            new(ShopSuitableActivity.GoldAndJewellery, "ذهب ومجوهرات", "Gold & jewellery"),
            new(ShopSuitableActivity.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<RealEstateLookupEntry<ShopFloorType>> FloorTypes =
        new List<RealEstateLookupEntry<ShopFloorType>>
        {
            new(ShopFloorType.Ground, "أرضي", "Ground"),
            new(ShopFloorType.Mezzanine, "ميزانين", "Mezzanine"),
            new(ShopFloorType.First, "أول", "First"),
            new(ShopFloorType.Basement, "بدروم", "Basement")
        };

    public static readonly IReadOnlyList<RealEstateLookupEntry<ShopFacadesCount>> FacadesCounts =
        new List<RealEstateLookupEntry<ShopFacadesCount>>
        {
            new(ShopFacadesCount.One, "واجهة واحدة", "One facade"),
            new(ShopFacadesCount.Two, "واجهتان", "Two facades"),
            new(ShopFacadesCount.Three, "ثلاث واجهات", "Three facades"),
            new(ShopFacadesCount.Four, "أربع واجهات", "Four facades")
        };

    public static readonly IReadOnlyList<RealEstateLookupEntry<ShopFacadeDirection>> FacadeDirections =
        new List<RealEstateLookupEntry<ShopFacadeDirection>>
        {
            new(ShopFacadeDirection.North, "بحري", "North"),
            new(ShopFacadeDirection.South, "قبلي", "South"),
            new(ShopFacadeDirection.East, "شرقي", "East"),
            new(ShopFacadeDirection.West, "غربي", "West"),
            new(ShopFacadeDirection.Corner, "ناصية", "Corner")
        };

    public static readonly IReadOnlyList<RealEstateLookupEntry<ShopFinishingType>> FinishingTypes =
        new List<RealEstateLookupEntry<ShopFinishingType>>
        {
            new(ShopFinishingType.SuperLux, "سوبر لوكس", "Super lux"),
            new(ShopFinishingType.Lux, "لوكس", "Lux"),
            new(ShopFinishingType.SemiFinished, "نصف تشطيب", "Semi-finished"),
            new(ShopFinishingType.RedBrick, "طوب أحمر", "Red brick"),
            new(ShopFinishingType.Unfinished, "بدون تشطيب", "Unfinished")
        };

    public static readonly IReadOnlyList<RealEstateLookupEntry<ShopPropertyAge>> PropertyAges =
        new List<RealEstateLookupEntry<ShopPropertyAge>>
        {
            new(ShopPropertyAge.New, "جديد", "New"),
            new(ShopPropertyAge.UnderFiveYears, "أقل من 5 سنوات", "Under 5 years"),
            new(ShopPropertyAge.FiveToTenYears, "5 - 10 سنوات", "5 - 10 years"),
            new(ShopPropertyAge.OverTenYears, "أكثر من 10 سنوات", "Over 10 years")
        };

    public static readonly IReadOnlyList<RealEstateLookupEntry<ShopEntrancesCount>> EntrancesCounts =
        new List<RealEstateLookupEntry<ShopEntrancesCount>>
        {
            new(ShopEntrancesCount.One, "1", "1"),
            new(ShopEntrancesCount.Two, "2", "2"),
            new(ShopEntrancesCount.Three, "3", "3"),
            new(ShopEntrancesCount.More, "أكثر", "More")
        };

    public static readonly IReadOnlyList<RealEstateLookupEntry<ShopLegalStatus>> LegalStatuses =
        new List<RealEstateLookupEntry<ShopLegalStatus>>
        {
            new(ShopLegalStatus.Licensed, "مرخص", "Licensed"),
            new(ShopLegalStatus.Reconciliation, "تصالح", "Reconciliation"),
            new(ShopLegalStatus.Unlicensed, "غير مرخص", "Unlicensed")
        };

    public static readonly IReadOnlyList<RealEstateLookupEntry<ShopLicenseType>> LicenseTypes =
        new List<RealEstateLookupEntry<ShopLicenseType>>
        {
            new(ShopLicenseType.Commercial, "تجارية", "Commercial"),
            new(ShopLicenseType.Administrative, "إدارية", "Administrative"),
            new(ShopLicenseType.Industrial, "صناعية", "Industrial"),
            new(ShopLicenseType.Service, "خدمية", "Service")
        };

    public static readonly IReadOnlyList<RealEstateLookupEntry<ShopReconciliationForm>> ReconciliationForms =
        new List<RealEstateLookupEntry<ShopReconciliationForm>>
        {
            new(ShopReconciliationForm.Form1, "نموذج 1", "Form 1"),
            new(ShopReconciliationForm.Form3, "نموذج 3", "Form 3"),
            new(ShopReconciliationForm.Form8, "نموذج 8", "Form 8"),
            new(ShopReconciliationForm.Form10, "نموذج 10", "Form 10"),
            new(ShopReconciliationForm.FinalForm, "نموذج نهائي", "Final form"),
            new(ShopReconciliationForm.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<RealEstateLookupEntry<ShopOwnershipDocument>> OwnershipDocuments =
        new List<RealEstateLookupEntry<ShopOwnershipDocument>>
        {
            new(ShopOwnershipDocument.FinalContract, "عقد نهائي", "Final contract"),
            new(ShopOwnershipDocument.GreenContract, "عقد أخضر", "Green contract"),
            new(ShopOwnershipDocument.PreliminaryContract, "عقد ابتدائي", "Preliminary contract"),
            new(ShopOwnershipDocument.PowerOfAttorney, "توكيل", "Power of attorney"),
            new(ShopOwnershipDocument.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<RealEstateLookupEntry<ShopUtility>> Utilities =
        new List<RealEstateLookupEntry<ShopUtility>>
        {
            new(ShopUtility.ElectricityMeter, "عداد كهرباء", "Electricity meter"),
            new(ShopUtility.WaterMeter, "عداد مياه", "Water meter"),
            new(ShopUtility.NaturalGas, "غاز طبيعي", "Natural gas"),
            new(ShopUtility.Sewage, "صرف صحي", "Sewage"),
            new(ShopUtility.Internet, "إنترنت", "Internet"),
            new(ShopUtility.AirConditioning, "تكييف", "Air conditioning"),
            new(ShopUtility.SurveillanceCameras, "كاميرات مراقبة", "Surveillance cameras"),
            new(ShopUtility.AlarmSystem, "نظام إنذار", "Alarm system"),
            new(ShopUtility.FireExtinguishingSystem, "نظام إطفاء حريق", "Fire extinguishing system"),
            new(ShopUtility.Security, "أمن", "Security"),
            new(ShopUtility.Elevator, "مصعد", "Elevator"),
            new(ShopUtility.Parking, "موقف سيارات", "Parking"),
            new(ShopUtility.Generator, "مولد كهرباء", "Generator"),
            new(ShopUtility.ReadySign, "لافتة جاهزة", "Ready sign"),
            new(ShopUtility.RestaurantEquipment, "تجهيزات مطعم", "Restaurant equipment"),
            new(ShopUtility.CafeEquipment, "تجهيزات كافيه", "Cafe equipment")
        };

    public static readonly IReadOnlyList<RealEstateLookupEntry<ShopPaymentMethod>> PaymentMethods =
        new List<RealEstateLookupEntry<ShopPaymentMethod>>
        {
            new(ShopPaymentMethod.Cash, "كاش", "Cash"),
            new(ShopPaymentMethod.Installments, "تقسيط", "Installments"),
            new(ShopPaymentMethod.CashOrInstallments, "كاش أو تقسيط", "Cash or installments")
        };

    public static readonly IReadOnlyList<RealEstateLookupEntry<ShopInstallmentProvider>> InstallmentProviders =
        new List<RealEstateLookupEntry<ShopInstallmentProvider>>
        {
            new(ShopInstallmentProvider.Owner, "المالك", "Owner"),
            new(ShopInstallmentProvider.Company, "شركة", "Company"),
            new(ShopInstallmentProvider.Bank, "بنك", "Bank")
        };

    public static readonly IReadOnlyList<RealEstateLookupEntry<ShopRentType>> RentTypes =
        new List<RealEstateLookupEntry<ShopRentType>>
        {
            new(ShopRentType.Daily, "يومي", "Daily"),
            new(ShopRentType.Weekly, "أسبوعي", "Weekly"),
            new(ShopRentType.Monthly, "شهري", "Monthly"),
            new(ShopRentType.Yearly, "سنوي", "Yearly")
        };

    public static readonly IReadOnlyList<RealEstateLookupEntry<ShopRentInclusion>> RentInclusions =
        new List<RealEstateLookupEntry<ShopRentInclusion>>
        {
            new(ShopRentInclusion.Electricity, "كهرباء", "Electricity"),
            new(ShopRentInclusion.Water, "مياه", "Water"),
            new(ShopRentInclusion.Gas, "غاز", "Gas"),
            new(ShopRentInclusion.Internet, "إنترنت", "Internet"),
            new(ShopRentInclusion.Maintenance, "صيانة", "Maintenance")
        };

    public static readonly IReadOnlyList<RealEstateLookupEntry<ShopRentSuitableActivity>> RentSuitableActivities =
        new List<RealEstateLookupEntry<ShopRentSuitableActivity>>
        {
            new(ShopRentSuitableActivity.Restaurant, "مطعم", "Restaurant"),
            new(ShopRentSuitableActivity.Cafe, "كافيه", "Cafe"),
            new(ShopRentSuitableActivity.Supermarket, "سوبر ماركت", "Supermarket"),
            new(ShopRentSuitableActivity.Clothing, "ملابس", "Clothing"),
            new(ShopRentSuitableActivity.Pharmacy, "صيدلية", "Pharmacy"),
            new(ShopRentSuitableActivity.Office, "مكتب", "Office"),
            new(ShopRentSuitableActivity.Clinic, "عيادة", "Clinic"),
            new(ShopRentSuitableActivity.Showroom, "معرض", "Showroom"),
            new(ShopRentSuitableActivity.Storage, "مخزن", "Storage"),
            new(ShopRentSuitableActivity.AnyActivity, "أي نشاط", "Any activity")
        };

    public static readonly IReadOnlyList<RealEstateLookupEntry<ShopExchangeWith>> ExchangeTargets =
        new List<RealEstateLookupEntry<ShopExchangeWith>>
        {
            new(ShopExchangeWith.Shop, "محل", "Shop"),
            new(ShopExchangeWith.Apartment, "شقة", "Apartment"),
            new(ShopExchangeWith.Land, "أرض", "Land"),
            new(ShopExchangeWith.Villa, "فيلا", "Villa"),
            new(ShopExchangeWith.Car, "سيارة", "Car"),
            new(ShopExchangeWith.Factory, "مصنع", "Factory"),
            new(ShopExchangeWith.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<RealEstateLookupItemDto> SuitableActivityOptions = RealEstateCatalog.ToOptions(SuitableActivities);
    public static readonly IReadOnlyList<RealEstateLookupItemDto> FloorTypeOptions = RealEstateCatalog.ToOptions(FloorTypes);
    public static readonly IReadOnlyList<RealEstateLookupItemDto> FacadesCountOptions = RealEstateCatalog.ToOptions(FacadesCounts);
    public static readonly IReadOnlyList<RealEstateLookupItemDto> FacadeDirectionOptions = RealEstateCatalog.ToOptions(FacadeDirections);
    public static readonly IReadOnlyList<RealEstateLookupItemDto> FinishingTypeOptions = RealEstateCatalog.ToOptions(FinishingTypes);
    public static readonly IReadOnlyList<RealEstateLookupItemDto> PropertyAgeOptions = RealEstateCatalog.ToOptions(PropertyAges);
    public static readonly IReadOnlyList<RealEstateLookupItemDto> EntrancesCountOptions = RealEstateCatalog.ToOptions(EntrancesCounts);
    public static readonly IReadOnlyList<RealEstateLookupItemDto> LegalStatusOptions = RealEstateCatalog.ToOptions(LegalStatuses);
    public static readonly IReadOnlyList<RealEstateLookupItemDto> LicenseTypeOptions = RealEstateCatalog.ToOptions(LicenseTypes);
    public static readonly IReadOnlyList<RealEstateLookupItemDto> ReconciliationFormOptions = RealEstateCatalog.ToOptions(ReconciliationForms);
    public static readonly IReadOnlyList<RealEstateLookupItemDto> OwnershipDocumentOptions = RealEstateCatalog.ToOptions(OwnershipDocuments);
    public static readonly IReadOnlyList<RealEstateLookupItemDto> UtilityOptions = RealEstateCatalog.ToOptions(Utilities);
    public static readonly IReadOnlyList<RealEstateLookupItemDto> PaymentMethodOptions = RealEstateCatalog.ToOptions(PaymentMethods);
    public static readonly IReadOnlyList<RealEstateLookupItemDto> InstallmentProviderOptions = RealEstateCatalog.ToOptions(InstallmentProviders);
    public static readonly IReadOnlyList<RealEstateLookupItemDto> RentTypeOptions = RealEstateCatalog.ToOptions(RentTypes);
    public static readonly IReadOnlyList<RealEstateLookupItemDto> RentInclusionOptions = RealEstateCatalog.ToOptions(RentInclusions);
    public static readonly IReadOnlyList<RealEstateLookupItemDto> RentSuitableActivityOptions = RealEstateCatalog.ToOptions(RentSuitableActivities);
    public static readonly IReadOnlyList<RealEstateLookupItemDto> ExchangeTargetOptions = RealEstateCatalog.ToOptions(ExchangeTargets);

    public static string? GetSuitableActivityName(ShopSuitableActivity? value) => RealEstateCatalog.GetNameOrNull(SuitableActivities, value);
    public static string? GetFloorTypeName(ShopFloorType? value) => RealEstateCatalog.GetNameOrNull(FloorTypes, value);
    public static string? GetFacadesCountName(ShopFacadesCount? value) => RealEstateCatalog.GetNameOrNull(FacadesCounts, value);
    public static string? GetFacadeDirectionName(ShopFacadeDirection? value) => RealEstateCatalog.GetNameOrNull(FacadeDirections, value);
    public static string? GetFinishingTypeName(ShopFinishingType? value) => RealEstateCatalog.GetNameOrNull(FinishingTypes, value);
    public static string? GetPropertyAgeName(ShopPropertyAge? value) => RealEstateCatalog.GetNameOrNull(PropertyAges, value);
    public static string? GetEntrancesCountName(ShopEntrancesCount? value) => RealEstateCatalog.GetNameOrNull(EntrancesCounts, value);
    public static string? GetLegalStatusName(ShopLegalStatus? value) => RealEstateCatalog.GetNameOrNull(LegalStatuses, value);
    public static string? GetLicenseTypeName(ShopLicenseType? value) => RealEstateCatalog.GetNameOrNull(LicenseTypes, value);
    public static string? GetReconciliationFormName(ShopReconciliationForm? value) => RealEstateCatalog.GetNameOrNull(ReconciliationForms, value);
    public static string? GetOwnershipDocumentName(ShopOwnershipDocument? value) => RealEstateCatalog.GetNameOrNull(OwnershipDocuments, value);
    public static string? GetPaymentMethodName(ShopPaymentMethod? value) => RealEstateCatalog.GetNameOrNull(PaymentMethods, value);
    public static string? GetInstallmentProviderName(ShopInstallmentProvider? value) => RealEstateCatalog.GetNameOrNull(InstallmentProviders, value);
    public static string? GetRentTypeName(ShopRentType? value) => RealEstateCatalog.GetNameOrNull(RentTypes, value);
    public static string? GetExchangeTargetName(ShopExchangeWith? value) => RealEstateCatalog.GetNameOrNull(ExchangeTargets, value);

    public static List<RealEstateLookupItemDto> SelectedUtilities(IEnumerable<ShopUtility> selected) =>
        RealEstateCatalog.SelectedOptions(Utilities, selected);

    public static List<RealEstateLookupItemDto> SelectedRentInclusions(IEnumerable<ShopRentInclusion> selected) =>
        RealEstateCatalog.SelectedOptions(RentInclusions, selected);

    public static List<RealEstateLookupItemDto> SelectedRentSuitableActivities(
        IEnumerable<ShopRentSuitableActivity> selected) =>
        RealEstateCatalog.SelectedOptions(RentSuitableActivities, selected);
}
