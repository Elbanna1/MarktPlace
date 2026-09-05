using Shared.DTOs.RealEstate;
using Shared.Enums;

namespace Shared.Constants;

public static class ApartmentCatalog
{
    public static readonly IReadOnlyList<RealEstateLookupEntry<ApartmentType>> ApartmentTypes =
        new List<RealEstateLookupEntry<ApartmentType>>
        {
            new(ApartmentType.Residential, "شقة سكنية", "Residential apartment"),
            new(ApartmentType.Administrative, "شقة إدارية", "Administrative apartment"),
            new(ApartmentType.Studio, "استوديو", "Studio"),
            new(ApartmentType.Duplex, "دوبلكس", "Duplex"),
            new(ApartmentType.Penthouse, "بنتهاوس", "Penthouse"),
            new(ApartmentType.Roof, "رووف", "Roof"),
            new(ApartmentType.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<RealEstateLookupEntry<ApartmentOwnershipType>> OwnershipTypes =
        new List<RealEstateLookupEntry<ApartmentOwnershipType>>
        {
            new(ApartmentOwnershipType.Freehold, "تمليك", "Freehold"),
            new(ApartmentOwnershipType.Government, "حكومي", "Government"),
            new(ApartmentOwnershipType.Private, "خاص", "Private"),
            new(ApartmentOwnershipType.OwnersAssociation, "اتحاد ملاك", "Owners association"),
            new(ApartmentOwnershipType.SocialHousing, "إسكان اجتماعي", "Social housing"),
            new(ApartmentOwnershipType.DistinguishedHousing, "إسكان متميز", "Distinguished housing"),
            new(ApartmentOwnershipType.Compound, "كمبوند", "Compound"),
            new(ApartmentOwnershipType.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<RealEstateLookupEntry<ApartmentReceptionPieces>> ReceptionPieces =
        new List<RealEstateLookupEntry<ApartmentReceptionPieces>>
        {
            new(ApartmentReceptionPieces.One, "1", "1"),
            new(ApartmentReceptionPieces.Two, "2", "2"),
            new(ApartmentReceptionPieces.Three, "3", "3"),
            new(ApartmentReceptionPieces.Four, "4", "4"),
            new(ApartmentReceptionPieces.Open, "مفتوح", "Open")
        };

    public static readonly IReadOnlyList<RealEstateLookupEntry<ApartmentFloorType>> FloorTypes =
        new List<RealEstateLookupEntry<ApartmentFloorType>>
        {
            new(ApartmentFloorType.Ground, "أرضي", "Ground"),
            new(ApartmentFloorType.Repeated, "متكرر", "Repeated"),
            new(ApartmentFloorType.Last, "أخير", "Last"),
            new(ApartmentFloorType.Roof, "رووف", "Roof"),
            new(ApartmentFloorType.Basement, "بدروم", "Basement"),
            new(ApartmentFloorType.Mezzanine, "ميزانين", "Mezzanine")
        };

    public static readonly IReadOnlyList<RealEstateLookupEntry<ApartmentFurnishedStatus>> FurnishedStatuses =
        new List<RealEstateLookupEntry<ApartmentFurnishedStatus>>
        {
            new(ApartmentFurnishedStatus.Yes, "نعم", "Yes"),
            new(ApartmentFurnishedStatus.No, "لا", "No"),
            new(ApartmentFurnishedStatus.SemiFurnished, "نصف مفروشة", "Semi-furnished")
        };

    public static readonly IReadOnlyList<RealEstateLookupEntry<ApartmentFinishingType>> FinishingTypes =
        new List<RealEstateLookupEntry<ApartmentFinishingType>>
        {
            new(ApartmentFinishingType.SuperLux, "سوبر لوكس", "Super lux"),
            new(ApartmentFinishingType.Lux, "لوكس", "Lux"),
            new(ApartmentFinishingType.Luxury, "تشطيب فاخر", "Luxury finishing"),
            new(ApartmentFinishingType.SemiFinished, "نصف تشطيب", "Semi-finished"),
            new(ApartmentFinishingType.RedBrick, "طوب أحمر", "Red brick"),
            new(ApartmentFinishingType.Unfinished, "بدون تشطيب", "Unfinished")
        };

    public static readonly IReadOnlyList<RealEstateLookupEntry<ApartmentPropertyAge>> PropertyAges =
        new List<RealEstateLookupEntry<ApartmentPropertyAge>>
        {
            new(ApartmentPropertyAge.New, "جديد", "New"),
            new(ApartmentPropertyAge.UnderFiveYears, "أقل من 5 سنوات", "Under 5 years"),
            new(ApartmentPropertyAge.FiveToTenYears, "5 - 10 سنوات", "5 - 10 years"),
            new(ApartmentPropertyAge.OverTenYears, "أكثر من 10 سنوات", "Over 10 years")
        };

    public static readonly IReadOnlyList<RealEstateLookupEntry<ApartmentDirection>> Directions =
        new List<RealEstateLookupEntry<ApartmentDirection>>
        {
            new(ApartmentDirection.North, "بحري", "North"),
            new(ApartmentDirection.South, "قبلي", "South"),
            new(ApartmentDirection.East, "شرقي", "East"),
            new(ApartmentDirection.West, "غربي", "West"),
            new(ApartmentDirection.Corner, "ناصية", "Corner")
        };

    public static readonly IReadOnlyList<RealEstateLookupEntry<ApartmentViewType>> ViewTypes =
        new List<RealEstateLookupEntry<ApartmentViewType>>
        {
            new(ApartmentViewType.MainStreet, "شارع رئيسي", "Main street"),
            new(ApartmentViewType.SideStreet, "شارع جانبي", "Side street"),
            new(ApartmentViewType.Garden, "حديقة", "Garden"),
            new(ApartmentViewType.Nile, "نيل", "Nile"),
            new(ApartmentViewType.Sea, "بحر", "Sea"),
            new(ApartmentViewType.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<RealEstateLookupEntry<ApartmentLegalStatus>> LegalStatuses =
        new List<RealEstateLookupEntry<ApartmentLegalStatus>>
        {
            new(ApartmentLegalStatus.Licensed, "مرخصة", "Licensed"),
            new(ApartmentLegalStatus.Reconciliation, "تصالح", "Reconciliation"),
            new(ApartmentLegalStatus.Unlicensed, "غير مرخصة", "Unlicensed")
        };

    public static readonly IReadOnlyList<RealEstateLookupEntry<ApartmentReconciliationForm>> ReconciliationForms =
        new List<RealEstateLookupEntry<ApartmentReconciliationForm>>
        {
            new(ApartmentReconciliationForm.Form1, "نموذج 1", "Form 1"),
            new(ApartmentReconciliationForm.Form3, "نموذج 3", "Form 3"),
            new(ApartmentReconciliationForm.Form8, "نموذج 8", "Form 8"),
            new(ApartmentReconciliationForm.Form10, "نموذج 10", "Form 10"),
            new(ApartmentReconciliationForm.FinalForm, "نموذج نهائي", "Final form"),
            new(ApartmentReconciliationForm.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<RealEstateLookupEntry<ApartmentOwnershipDocument>> OwnershipDocuments =
        new List<RealEstateLookupEntry<ApartmentOwnershipDocument>>
        {
            new(ApartmentOwnershipDocument.FinalContract, "عقد نهائي", "Final contract"),
            new(ApartmentOwnershipDocument.GreenContract, "عقد أخضر", "Green contract"),
            new(ApartmentOwnershipDocument.PreliminaryContract, "عقد ابتدائي", "Preliminary contract"),
            new(ApartmentOwnershipDocument.PowerOfAttorney, "توكيل", "Power of attorney"),
            new(ApartmentOwnershipDocument.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<RealEstateLookupEntry<ApartmentFeature>> Features =
        new List<RealEstateLookupEntry<ApartmentFeature>>
        {
            new(ApartmentFeature.Elevator, "مصعد", "Elevator"),
            new(ApartmentFeature.Garage, "جراج", "Garage"),
            new(ApartmentFeature.Security, "أمن", "Security"),
            new(ApartmentFeature.SurveillanceCameras, "كاميرات مراقبة", "Surveillance cameras"),
            new(ApartmentFeature.NaturalGas, "غاز طبيعي", "Natural gas"),
            new(ApartmentFeature.ElectricityMeter, "عداد كهرباء", "Electricity meter"),
            new(ApartmentFeature.WaterMeter, "عداد مياه", "Water meter"),
            new(ApartmentFeature.Internet, "إنترنت", "Internet"),
            new(ApartmentFeature.AirConditioning, "تكييف", "Air conditioning"),
            new(ApartmentFeature.Kitchen, "مطبخ", "Kitchen"),
            new(ApartmentFeature.Balcony, "بلكونة", "Balcony"),
            new(ApartmentFeature.DressingRoom, "غرفة ملابس", "Dressing room"),
            new(ApartmentFeature.LaundryRoom, "غرفة غسيل", "Laundry room"),
            new(ApartmentFeature.Storage, "مخزن", "Storage"),
            new(ApartmentFeature.Generator, "مولد كهرباء", "Generator"),
            new(ApartmentFeature.Garden, "حديقة", "Garden"),
            new(ApartmentFeature.SwimmingPool, "مسبح", "Swimming pool"),
            new(ApartmentFeature.Club, "نادي", "Club"),
            new(ApartmentFeature.Gym, "جيم", "Gym"),
            new(ApartmentFeature.PrivateEntrance, "مدخل خاص", "Private entrance"),
            new(ApartmentFeature.ArmoredDoor, "باب مصفح", "Armored door"),
            new(ApartmentFeature.Intercom, "إنتركم", "Intercom"),
            new(ApartmentFeature.CentralSatellite, "دش مركزي", "Central satellite"),
            new(ApartmentFeature.WaterTanks, "خزانات مياه", "Water tanks"),
            new(ApartmentFeature.SolarPanels, "ألواح طاقة شمسية", "Solar panels")
        };

    public static readonly IReadOnlyList<RealEstateLookupEntry<ApartmentPaymentMethod>> PaymentMethods =
        new List<RealEstateLookupEntry<ApartmentPaymentMethod>>
        {
            new(ApartmentPaymentMethod.Cash, "كاش", "Cash"),
            new(ApartmentPaymentMethod.Installments, "تقسيط", "Installments"),
            new(ApartmentPaymentMethod.CashOrInstallments, "كاش أو تقسيط", "Cash or installments")
        };

    public static readonly IReadOnlyList<RealEstateLookupEntry<ApartmentInstallmentProvider>> InstallmentProviders =
        new List<RealEstateLookupEntry<ApartmentInstallmentProvider>>
        {
            new(ApartmentInstallmentProvider.Owner, "مالك", "Owner"),
            new(ApartmentInstallmentProvider.Company, "شركة", "Company"),
            new(ApartmentInstallmentProvider.Bank, "بنك", "Bank")
        };

    public static readonly IReadOnlyList<RealEstateLookupEntry<ApartmentRentType>> RentTypes =
        new List<RealEstateLookupEntry<ApartmentRentType>>
        {
            new(ApartmentRentType.Daily, "يومي", "Daily"),
            new(ApartmentRentType.Weekly, "أسبوعي", "Weekly"),
            new(ApartmentRentType.Monthly, "شهري", "Monthly"),
            new(ApartmentRentType.Yearly, "سنوي", "Yearly")
        };

    public static readonly IReadOnlyList<RealEstateLookupEntry<ApartmentRentInclusion>> RentInclusions =
        new List<RealEstateLookupEntry<ApartmentRentInclusion>>
        {
            new(ApartmentRentInclusion.Electricity, "كهرباء", "Electricity"),
            new(ApartmentRentInclusion.Water, "مياه", "Water"),
            new(ApartmentRentInclusion.Gas, "غاز", "Gas"),
            new(ApartmentRentInclusion.Internet, "إنترنت", "Internet"),
            new(ApartmentRentInclusion.Maintenance, "صيانة", "Maintenance")
        };

    public static readonly IReadOnlyList<RealEstateLookupEntry<ApartmentSuitableFor>> SuitableFor =
        new List<RealEstateLookupEntry<ApartmentSuitableFor>>
        {
            new(ApartmentSuitableFor.Individuals, "أفراد", "Individuals"),
            new(ApartmentSuitableFor.Families, "عائلات", "Families"),
            new(ApartmentSuitableFor.Students, "طلاب", "Students"),
            new(ApartmentSuitableFor.Companies, "شركات", "Companies")
        };

    public static readonly IReadOnlyList<RealEstateLookupEntry<ApartmentExchangeWith>> ExchangeTargets =
        new List<RealEstateLookupEntry<ApartmentExchangeWith>>
        {
            new(ApartmentExchangeWith.Apartment, "شقة", "Apartment"),
            new(ApartmentExchangeWith.Villa, "فيلا", "Villa"),
            new(ApartmentExchangeWith.Land, "أرض", "Land"),
            new(ApartmentExchangeWith.Shop, "محل", "Shop"),
            new(ApartmentExchangeWith.Car, "سيارة", "Car"),
            new(ApartmentExchangeWith.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<RealEstateLookupItemDto> ApartmentTypeOptions = RealEstateCatalog.ToOptions(ApartmentTypes);
    public static readonly IReadOnlyList<RealEstateLookupItemDto> OwnershipTypeOptions = RealEstateCatalog.ToOptions(OwnershipTypes);
    public static readonly IReadOnlyList<RealEstateLookupItemDto> ReceptionPieceOptions = RealEstateCatalog.ToOptions(ReceptionPieces);
    public static readonly IReadOnlyList<RealEstateLookupItemDto> FloorTypeOptions = RealEstateCatalog.ToOptions(FloorTypes);
    public static readonly IReadOnlyList<RealEstateLookupItemDto> FurnishedStatusOptions = RealEstateCatalog.ToOptions(FurnishedStatuses);
    public static readonly IReadOnlyList<RealEstateLookupItemDto> FinishingTypeOptions = RealEstateCatalog.ToOptions(FinishingTypes);
    public static readonly IReadOnlyList<RealEstateLookupItemDto> PropertyAgeOptions = RealEstateCatalog.ToOptions(PropertyAges);
    public static readonly IReadOnlyList<RealEstateLookupItemDto> DirectionOptions = RealEstateCatalog.ToOptions(Directions);
    public static readonly IReadOnlyList<RealEstateLookupItemDto> ViewTypeOptions = RealEstateCatalog.ToOptions(ViewTypes);
    public static readonly IReadOnlyList<RealEstateLookupItemDto> LegalStatusOptions = RealEstateCatalog.ToOptions(LegalStatuses);
    public static readonly IReadOnlyList<RealEstateLookupItemDto> ReconciliationFormOptions = RealEstateCatalog.ToOptions(ReconciliationForms);
    public static readonly IReadOnlyList<RealEstateLookupItemDto> OwnershipDocumentOptions = RealEstateCatalog.ToOptions(OwnershipDocuments);
    public static readonly IReadOnlyList<RealEstateLookupItemDto> FeatureOptions = RealEstateCatalog.ToOptions(Features);
    public static readonly IReadOnlyList<RealEstateLookupItemDto> PaymentMethodOptions = RealEstateCatalog.ToOptions(PaymentMethods);
    public static readonly IReadOnlyList<RealEstateLookupItemDto> InstallmentProviderOptions = RealEstateCatalog.ToOptions(InstallmentProviders);
    public static readonly IReadOnlyList<RealEstateLookupItemDto> RentTypeOptions = RealEstateCatalog.ToOptions(RentTypes);
    public static readonly IReadOnlyList<RealEstateLookupItemDto> RentInclusionOptions = RealEstateCatalog.ToOptions(RentInclusions);
    public static readonly IReadOnlyList<RealEstateLookupItemDto> SuitableForOptions = RealEstateCatalog.ToOptions(SuitableFor);
    public static readonly IReadOnlyList<RealEstateLookupItemDto> ExchangeTargetOptions = RealEstateCatalog.ToOptions(ExchangeTargets);

    public static string? GetApartmentTypeName(ApartmentType? value) => RealEstateCatalog.GetNameOrNull(ApartmentTypes, value);
    public static string? GetOwnershipTypeName(ApartmentOwnershipType? value) => RealEstateCatalog.GetNameOrNull(OwnershipTypes, value);
    public static string? GetReceptionPiecesName(ApartmentReceptionPieces? value) => RealEstateCatalog.GetNameOrNull(ReceptionPieces, value);
    public static string? GetFloorTypeName(ApartmentFloorType? value) => RealEstateCatalog.GetNameOrNull(FloorTypes, value);
    public static string? GetFurnishedStatusName(ApartmentFurnishedStatus? value) => RealEstateCatalog.GetNameOrNull(FurnishedStatuses, value);
    public static string? GetFinishingTypeName(ApartmentFinishingType? value) => RealEstateCatalog.GetNameOrNull(FinishingTypes, value);
    public static string? GetPropertyAgeName(ApartmentPropertyAge? value) => RealEstateCatalog.GetNameOrNull(PropertyAges, value);
    public static string? GetDirectionName(ApartmentDirection? value) => RealEstateCatalog.GetNameOrNull(Directions, value);
    public static string? GetViewTypeName(ApartmentViewType? value) => RealEstateCatalog.GetNameOrNull(ViewTypes, value);
    public static string? GetLegalStatusName(ApartmentLegalStatus? value) => RealEstateCatalog.GetNameOrNull(LegalStatuses, value);
    public static string? GetReconciliationFormName(ApartmentReconciliationForm? value) => RealEstateCatalog.GetNameOrNull(ReconciliationForms, value);
    public static string? GetOwnershipDocumentName(ApartmentOwnershipDocument? value) => RealEstateCatalog.GetNameOrNull(OwnershipDocuments, value);
    public static string? GetPaymentMethodName(ApartmentPaymentMethod? value) => RealEstateCatalog.GetNameOrNull(PaymentMethods, value);
    public static string? GetInstallmentProviderName(ApartmentInstallmentProvider? value) => RealEstateCatalog.GetNameOrNull(InstallmentProviders, value);
    public static string? GetRentTypeName(ApartmentRentType? value) => RealEstateCatalog.GetNameOrNull(RentTypes, value);
    public static string? GetSuitableForName(ApartmentSuitableFor? value) => RealEstateCatalog.GetNameOrNull(SuitableFor, value);
    public static string? GetExchangeTargetName(ApartmentExchangeWith? value) => RealEstateCatalog.GetNameOrNull(ExchangeTargets, value);

    public static List<RealEstateLookupItemDto> SelectedFeatures(IEnumerable<ApartmentFeature> selected) =>
        RealEstateCatalog.SelectedOptions(Features, selected);

    public static List<RealEstateLookupItemDto> SelectedRentInclusions(IEnumerable<ApartmentRentInclusion> selected) =>
        RealEstateCatalog.SelectedOptions(RentInclusions, selected);
}
