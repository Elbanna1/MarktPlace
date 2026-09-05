using Shared.DTOs.Advertisements;
using Shared.Enums;

namespace Shared.Constants;

public static class CarCatalog
{
    public const int MinManufacturingYear = 1980;

    public static int MaxManufacturingYear => DateTime.UtcNow.Year;

    public const string OtherOptionValue = "أخرى";

    public static string DetailsSectionOf(SubCategoryType subCategory) => subCategory switch
    {
        SubCategoryType.Private => "بيانات السيارة",
        SubCategoryType.Taxi => "بيانات المركبة",
        SubCategoryType.Motorcycles => "بيانات الموتوسيكل",
        SubCategoryType.HeavyEquipment => "بيانات المعدة",
        _ => "بيانات الإعلان"
    };

    public static readonly IReadOnlyDictionary<ListingType, string> ListingTypeNames =
        new Dictionary<ListingType, string>
        {
            [ListingType.Sale] = "بيع",
            [ListingType.Rent] = "إيجار",
            [ListingType.Exchange] = "بدل",
            [ListingType.Accident] = "حوادث"
        };

    public static readonly IReadOnlyDictionary<TransmissionType, string> TransmissionNames =
        new Dictionary<TransmissionType, string>
        {
            [TransmissionType.Automatic] = "أوتوماتيك",
            [TransmissionType.Manual] = "مانيوال"
        };

    public static readonly IReadOnlyDictionary<TransmissionType, string> MotorcycleTransmissionNames =
        new Dictionary<TransmissionType, string>
        {
            [TransmissionType.Manual] = "مانيوال",
            [TransmissionType.Automatic] = "أوتوماتيك",
            [TransmissionType.SemiAutomatic] = "نصف أوتوماتيك"
        };

    public static readonly IReadOnlyDictionary<FuelType, string> FuelTypeNames =
        new Dictionary<FuelType, string>
        {
            [FuelType.Gasoline] = "بنزين",
            [FuelType.Diesel] = "سولار",
            [FuelType.Electric] = "كهرباء",
            [FuelType.Hybrid] = "هجين",
            [FuelType.NaturalGas] = "غاز طبيعي"
        };

    public static readonly IReadOnlyDictionary<FuelType, string> TaxiFuelTypeNames =
        new Dictionary<FuelType, string>
        {
            [FuelType.Gasoline] = "بنزين",
            [FuelType.Diesel] = "سولار",
            [FuelType.NaturalGas] = "غاز طبيعي",
            [FuelType.Electric] = "كهرباء",
            [FuelType.Hybrid] = "هجين"
        };

    public static readonly IReadOnlyDictionary<FuelType, string> MotorcycleFuelTypeNames =
        new Dictionary<FuelType, string>
        {
            [FuelType.Gasoline] = "بنزين",
            [FuelType.Electric] = "كهرباء"
        };

    public static readonly IReadOnlyDictionary<FuelType, string> EquipmentFuelTypeNames =
        new Dictionary<FuelType, string>
        {
            [FuelType.Diesel] = "سولار",
            [FuelType.Gasoline] = "بنزين",
            [FuelType.Electric] = "كهرباء",
            [FuelType.Hybrid] = "هجين"
        };

    public static readonly IReadOnlyDictionary<VehicleCondition, string> ConditionNames =
        new Dictionary<VehicleCondition, string>
        {
            [VehicleCondition.New] = "جديدة",
            [VehicleCondition.Used] = "مستعملة"
        };

    public static readonly IReadOnlyDictionary<VehicleCondition, string> MotorcycleConditionNames =
        new Dictionary<VehicleCondition, string>
        {
            [VehicleCondition.New] = "جديد",
            [VehicleCondition.Used] = "مستعمل"
        };

    public static readonly IReadOnlyDictionary<TechnicalCondition, string> TechnicalConditionNames =
        new Dictionary<TechnicalCondition, string>
        {
            [TechnicalCondition.Excellent] = "ممتازة",
            [TechnicalCondition.VeryGood] = "جيدة جدًا",
            [TechnicalCondition.Good] = "جيدة",
            [TechnicalCondition.NeedsMaintenance] = "تحتاج صيانة"
        };

    public static readonly IReadOnlyDictionary<BodyType, string> BodyTypeNames =
        new Dictionary<BodyType, string>
        {
            [BodyType.Sedan] = "سيدان",
            [BodyType.SUV] = "SUV",
            [BodyType.Hatchback] = "هاتشباك",
            [BodyType.Coupe] = "كوبيه",
            [BodyType.Crossover] = "كروس أوفر",
            [BodyType.Pickup] = "بيك أب",
            [BodyType.Van] = "فان",
            [BodyType.Convertible] = "كشف"
        };

    public static readonly IReadOnlyDictionary<CoolingType, string> CoolingTypeNames =
        new Dictionary<CoolingType, string>
        {
            [CoolingType.Air] = "هوائي",
            [CoolingType.Water] = "مائي",
            [CoolingType.Oil] = "زيتي"
        };

    public static readonly IReadOnlyDictionary<DamageLevel, string> DamageLevelNames =
        new Dictionary<DamageLevel, string>
        {
            [DamageLevel.Light] = "خفيف",
            [DamageLevel.Medium] = "متوسط",
            [DamageLevel.Heavy] = "شديد"
        };

    public static readonly IReadOnlyDictionary<VehicleOriginCountry, string> OriginCountryNames =
        new Dictionary<VehicleOriginCountry, string>
        {
            [VehicleOriginCountry.Japan] = "ياباني",
            [VehicleOriginCountry.Korea] = "كوري",
            [VehicleOriginCountry.Germany] = "ألماني",
            [VehicleOriginCountry.America] = "أمريكي",
            [VehicleOriginCountry.France] = "فرنسي",
            [VehicleOriginCountry.Italy] = "إيطالي",
            [VehicleOriginCountry.China] = "صيني",
            [VehicleOriginCountry.India] = "هندي",
            [VehicleOriginCountry.Other] = "أخرى"
        };

    public static readonly IReadOnlyDictionary<VehicleOriginCountry, string> TaxiOriginCountryNames =
        new Dictionary<VehicleOriginCountry, string>
        {
            [VehicleOriginCountry.Japan] = "ياباني",
            [VehicleOriginCountry.Korea] = "كوري",
            [VehicleOriginCountry.Germany] = "ألماني",
            [VehicleOriginCountry.America] = "أمريكي",
            [VehicleOriginCountry.China] = "صيني",
            [VehicleOriginCountry.India] = "هندي",
            [VehicleOriginCountry.Other] = "أخرى"
        };

    public static readonly IReadOnlyDictionary<VehicleOriginCountry, string> MotorcycleOriginCountryNames =
        new Dictionary<VehicleOriginCountry, string>
        {
            [VehicleOriginCountry.Japan] = "ياباني",
            [VehicleOriginCountry.China] = "صيني",
            [VehicleOriginCountry.India] = "هندي",
            [VehicleOriginCountry.Germany] = "ألماني",
            [VehicleOriginCountry.Italy] = "إيطالي",
            [VehicleOriginCountry.America] = "أمريكي",
            [VehicleOriginCountry.Other] = "أخرى"
        };

    public static readonly IReadOnlyDictionary<VehicleOriginCountry, string> EquipmentOriginCountryNames =
        new Dictionary<VehicleOriginCountry, string>
        {
            [VehicleOriginCountry.Japan] = "اليابان",
            [VehicleOriginCountry.Korea] = "كوريا",
            [VehicleOriginCountry.Germany] = "ألمانيا",
            [VehicleOriginCountry.America] = "أمريكا",
            [VehicleOriginCountry.China] = "الصين",
            [VehicleOriginCountry.India] = "الهند",
            [VehicleOriginCountry.Other] = "أخرى"
        };

    public static readonly IReadOnlyDictionary<VehicleAssemblyCountry, string> AssemblyCountryNames =
        new Dictionary<VehicleAssemblyCountry, string>
        {
            [VehicleAssemblyCountry.Egypt] = "مصر",
            [VehicleAssemblyCountry.Japan] = "اليابان",
            [VehicleAssemblyCountry.Korea] = "كوريا",
            [VehicleAssemblyCountry.Germany] = "ألمانيا",
            [VehicleAssemblyCountry.China] = "الصين",
            [VehicleAssemblyCountry.India] = "الهند",
            [VehicleAssemblyCountry.America] = "أمريكا",
            [VehicleAssemblyCountry.Other] = "أخرى"
        };

    public static readonly IReadOnlyDictionary<PreviousOwnersCount, string> PreviousOwnersNames =
        new Dictionary<PreviousOwnersCount, string>
        {
            [PreviousOwnersCount.First] = "الأول",
            [PreviousOwnersCount.Second] = "الثاني",
            [PreviousOwnersCount.Third] = "الثالث",
            [PreviousOwnersCount.MoreThanThree] = "أكثر من 3"
        };

    public static readonly IReadOnlyDictionary<PreviousOwnersCount, string> EquipmentPreviousOwnersNames =
        new Dictionary<PreviousOwnersCount, string>
        {
            [PreviousOwnersCount.First] = "الأول",
            [PreviousOwnersCount.Second] = "الثاني",
            [PreviousOwnersCount.Third] = "الثالث",
            [PreviousOwnersCount.MoreThanThree] = "أكثر"
        };

    public static readonly IReadOnlyDictionary<VehicleUsageType, string> UsageTypeNames =
        new Dictionary<VehicleUsageType, string>
        {
            [VehicleUsageType.Personal] = "استخدام شخصي",
            [VehicleUsageType.Uber] = "أوبر",
            [VehicleUsageType.Careem] = "كريم",
            [VehicleUsageType.Company] = "شركة",
            [VehicleUsageType.Other] = "أخرى"
        };

    public static readonly IReadOnlyDictionary<AccidentsCount, string> AccidentsCountNames =
        new Dictionary<AccidentsCount, string>
        {
            [AccidentsCount.None] = "بدون حوادث",
            [AccidentsCount.One] = "حادث واحد",
            [AccidentsCount.Two] = "حادثان",
            [AccidentsCount.More] = "أكثر"
        };

    public static readonly IReadOnlyDictionary<AccidentsCount, string> MotorcycleAccidentsCountNames =
        new Dictionary<AccidentsCount, string>
        {
            [AccidentsCount.None] = "بدون",
            [AccidentsCount.One] = "حادث",
            [AccidentsCount.Two] = "حادثان",
            [AccidentsCount.More] = "أكثر"
        };

    public static readonly IReadOnlyDictionary<LicenseStatus, string> LicenseStatusNames =
        new Dictionary<LicenseStatus, string>
        {
            [LicenseStatus.Valid] = "سارية",
            [LicenseStatus.Expired] = "منتهية",
            [LicenseStatus.Unlicensed] = "غير مرخصة"
        };

    public static readonly IReadOnlyDictionary<LicenseStatus, string> MotorcycleLicenseStatusNames =
        new Dictionary<LicenseStatus, string>
        {
            [LicenseStatus.Valid] = "سارية",
            [LicenseStatus.Expired] = "منتهية",
            [LicenseStatus.Unlicensed] = "غير مرخص"
        };

    public static readonly IReadOnlyDictionary<LicenseStatus, string> EquipmentLicenseStatusNames =
        new Dictionary<LicenseStatus, string>
        {
            [LicenseStatus.Valid] = "سارية",
            [LicenseStatus.Expired] = "منتهية",
            [LicenseStatus.Unlicensed] = "غير مرخصة",
            [LicenseStatus.NotRequired] = "لا تحتاج رخصة"
        };

    public static readonly IReadOnlyDictionary<OperatingLicenseStatus, string> OperatingLicenseStatusNames =
        new Dictionary<OperatingLicenseStatus, string>
        {
            [OperatingLicenseStatus.Valid] = "سارية",
            [OperatingLicenseStatus.Expired] = "منتهية",
            [OperatingLicenseStatus.None] = "لا توجد"
        };

    public static readonly IReadOnlyDictionary<InsuranceType, string> InsuranceTypeNames =
        new Dictionary<InsuranceType, string>
        {
            [InsuranceType.Comprehensive] = "تأمين شامل",
            [InsuranceType.ThirdParty] = "ضد الغير"
        };

    public static readonly IReadOnlyDictionary<MotorcycleType, string> MotorcycleTypeNames =
        new Dictionary<MotorcycleType, string>
        {
            [MotorcycleType.Motorcycle] = "موتوسيكل",
            [MotorcycleType.Scooter] = "سكوتر",
            [MotorcycleType.Sport] = "رياضي",
            [MotorcycleType.Cruiser] = "كروز",
            [MotorcycleType.OffRoad] = "أوف رود",
            [MotorcycleType.Trail] = "تريلا",
            [MotorcycleType.ATV] = "ATV",
            [MotorcycleType.Other] = "أخرى"
        };

    public static readonly IReadOnlyDictionary<MotorcycleStartType, string> MotorcycleStartTypeNames =
        new Dictionary<MotorcycleStartType, string>
        {
            [MotorcycleStartType.Electric] = "كهرباء",
            [MotorcycleStartType.Kick] = "بدال",
            [MotorcycleStartType.Both] = "الاثنين"
        };

    public static readonly IReadOnlyDictionary<EquipmentMachineType, string> MachineTypeNames =
        new Dictionary<EquipmentMachineType, string>
        {
            [EquipmentMachineType.Loader] = "لودر",
            [EquipmentMachineType.Excavator] = "حفار",
            [EquipmentMachineType.Bulldozer] = "بلدوزر",
            [EquipmentMachineType.Grader] = "جريدر",
            [EquipmentMachineType.Crane] = "ونش",
            [EquipmentMachineType.Forklift] = "رافعة شوكية",
            [EquipmentMachineType.Backhoe] = "باكهو",
            [EquipmentMachineType.ConcreteMixer] = "خلاطة خرسانة",
            [EquipmentMachineType.ConcretePump] = "مضخة خرسانة",
            [EquipmentMachineType.Dumper] = "قلاب",
            [EquipmentMachineType.Tractor] = "جرار زراعي",
            [EquipmentMachineType.Roller] = "مدحلة",
            [EquipmentMachineType.TelescopicHandler] = "رافعة تلسكوبية",
            [EquipmentMachineType.Other] = "أخرى"
        };

    public static readonly IReadOnlyDictionary<EquipmentDriveSystem, string> DriveSystemNames =
        new Dictionary<EquipmentDriveSystem, string>
        {
            [EquipmentDriveSystem.Wheels] = "كاوتش",
            [EquipmentDriveSystem.Tracks] = "جنزير"
        };

    public static readonly IReadOnlyDictionary<PowerUnit, string> PowerUnitNames =
        new Dictionary<PowerUnit, string>
        {
            [PowerUnit.Horsepower] = "حصان (HP)",
            [PowerUnit.Kilowatt] = "كيلووات (kW)"
        };

    public static readonly IReadOnlyDictionary<TaxiVehicleType, string> TaxiVehicleTypeNames =
        new Dictionary<TaxiVehicleType, string>
        {
            [TaxiVehicleType.Taxi] = "تاكسي",
            [TaxiVehicleType.Microbus] = "ميكروباص",
            [TaxiVehicleType.TouristMicrobus] = "ميكروباص سياحي",
            [TaxiVehicleType.Service] = "سرفيس",
            [TaxiVehicleType.Limousine] = "ليموزين",
            [TaxiVehicleType.PassengerVan] = "فان ركاب",
            [TaxiVehicleType.MiniBus] = "ميني باص",
            [TaxiVehicleType.Other] = "أخرى"
        };

    public static readonly IReadOnlyDictionary<TaxiActivityType, string> TaxiActivityTypeNames =
        new Dictionary<TaxiActivityType, string>
        {
            [TaxiActivityType.Taxi] = "تاكسي",
            [TaxiActivityType.Service] = "سرفيس",
            [TaxiActivityType.Microbus] = "ميكروباص",
            [TaxiActivityType.Limousine] = "ليموزين",
            [TaxiActivityType.EmployeeTransport] = "نقل موظفين",
            [TaxiActivityType.SchoolTransport] = "نقل مدارس",
            [TaxiActivityType.Tourism] = "سياحي",
            [TaxiActivityType.Other] = "أخرى"
        };

    public static readonly IReadOnlyDictionary<ChangedVehiclePart, string> ChangedPartNames =
        new Dictionary<ChangedVehiclePart, string>
        {
            [ChangedVehiclePart.Engine] = "موتور",
            [ChangedVehiclePart.Gearbox] = "فتيس",
            [ChangedVehiclePart.Chassis] = "شاسيه",
            [ChangedVehiclePart.Hood] = "كبوت",
            [ChangedVehiclePart.Trunk] = "شنطة",
            [ChangedVehiclePart.Fender] = "رفرف",
            [ChangedVehiclePart.Door] = "باب",
            [ChangedVehiclePart.FrontBumper] = "أكصدام أمامي",
            [ChangedVehiclePart.RearBumper] = "أكصدام خلفي",
            [ChangedVehiclePart.Other] = "أخرى"
        };

    public static readonly IReadOnlyDictionary<ChangedVehiclePart, string> TaxiChangedPartNames =
        new Dictionary<ChangedVehiclePart, string>
        {
            [ChangedVehiclePart.Engine] = "موتور",
            [ChangedVehiclePart.Gearbox] = "فتيس",
            [ChangedVehiclePart.Chassis] = "شاسيه",
            [ChangedVehiclePart.Hood] = "كبوت",
            [ChangedVehiclePart.Trunk] = "شنطة",
            [ChangedVehiclePart.Door] = "باب",
            [ChangedVehiclePart.Fender] = "رفرف",
            [ChangedVehiclePart.Bumper] = "أكصدام",
            [ChangedVehiclePart.Other] = "أخرى"
        };

    public static readonly IReadOnlyDictionary<ChangedVehiclePart, string> AllChangedPartNames =
        new Dictionary<ChangedVehiclePart, string>
        {
            [ChangedVehiclePart.Engine] = "موتور",
            [ChangedVehiclePart.Gearbox] = "فتيس",
            [ChangedVehiclePart.Chassis] = "شاسيه",
            [ChangedVehiclePart.Hood] = "كبوت",
            [ChangedVehiclePart.Trunk] = "شنطة",
            [ChangedVehiclePart.Fender] = "رفرف",
            [ChangedVehiclePart.Door] = "باب",
            [ChangedVehiclePart.FrontBumper] = "أكصدام أمامي",
            [ChangedVehiclePart.RearBumper] = "أكصدام خلفي",
            [ChangedVehiclePart.Bumper] = "أكصدام",
            [ChangedVehiclePart.Other] = "أخرى"
        };

    public static readonly IReadOnlyDictionary<EquipmentUsageField, string> UsageFieldNames =
        new Dictionary<EquipmentUsageField, string>
        {
            [EquipmentUsageField.Construction] = "إنشاءات",
            [EquipmentUsageField.RoadsAndBridges] = "طرق وكباري",
            [EquipmentUsageField.Agriculture] = "زراعة",
            [EquipmentUsageField.Quarries] = "محاجر",
            [EquipmentUsageField.Factories] = "مصانع",
            [EquipmentUsageField.Transport] = "نقل",
            [EquipmentUsageField.Contracting] = "مقاولات",
            [EquipmentUsageField.Other] = "أخرى"
        };

    public static readonly IReadOnlyDictionary<RentSystem, string> RentSystemNames =
        new Dictionary<RentSystem, string>
        {
            [RentSystem.Daily] = "يوم",
            [RentSystem.Weekly] = "أسبوع",
            [RentSystem.Monthly] = "شهر"
        };

    public static readonly IReadOnlyDictionary<RentSystem, string> EquipmentRentSystemNames =
        new Dictionary<RentSystem, string>
        {
            [RentSystem.Hourly] = "ساعة",
            [RentSystem.Daily] = "يوم",
            [RentSystem.Weekly] = "أسبوع",
            [RentSystem.Monthly] = "شهر"
        };

    public static readonly IReadOnlyDictionary<RentSystem, string> AllRentSystemNames =
        EquipmentRentSystemNames;

    public static readonly IReadOnlyDictionary<InterestedIn, string> PrivateExchangeTargetNames =
        new Dictionary<InterestedIn, string>
        {
            [InterestedIn.PrivateCar] = "سيارة ملاكي",
            [InterestedIn.AnyCar] = "أي سيارة",
            [InterestedIn.Motorcycle] = "موتوسيكل",
            [InterestedIn.TruckCar] = "سيارة نقل",
            [InterestedIn.LoaderOrHeavyEquipment] = "لودر أو معدة ثقيلة"
        };

    public static readonly IReadOnlyDictionary<InterestedIn, string> TaxiExchangeTargetNames =
        new Dictionary<InterestedIn, string>
        {
            [InterestedIn.Taxi] = "تاكسي",
            [InterestedIn.Microbus] = "ميكروباص",
            [InterestedIn.Limousine] = "ليموزين",
            [InterestedIn.PrivateCar] = "سيارة ملاكي",
            [InterestedIn.TruckCar] = "سيارة نقل",
            [InterestedIn.Other] = "أخرى"
        };

    public static readonly IReadOnlyDictionary<InterestedIn, string> MotorcycleExchangeTargetNames =
        new Dictionary<InterestedIn, string>
        {
            [InterestedIn.Motorcycle] = "موتوسيكل",
            [InterestedIn.Scooter] = "سكوتر",
            [InterestedIn.Car] = "سيارة",
            [InterestedIn.Other] = "أخرى"
        };

    public static readonly IReadOnlyDictionary<InterestedIn, string> EquipmentExchangeTargetNames =
        new Dictionary<InterestedIn, string>
        {
            [InterestedIn.Loader] = "لودر",
            [InterestedIn.Excavator] = "حفار",
            [InterestedIn.Crane] = "ونش",
            [InterestedIn.Tractor] = "جرار",
            [InterestedIn.AnyHeavyEquipment] = "أي معدة ثقيلة",
            [InterestedIn.Car] = "سيارة",
            [InterestedIn.Other] = "أخرى"
        };

    public static readonly IReadOnlyDictionary<InterestedIn, string> AllExchangeTargetNames =
        new Dictionary<InterestedIn, string>
        {
            [InterestedIn.PrivateCar] = "سيارة ملاكي",
            [InterestedIn.AnyCar] = "أي سيارة",
            [InterestedIn.Motorcycle] = "موتوسيكل",
            [InterestedIn.Loader] = "لودر",
            [InterestedIn.TruckCar] = "سيارة نقل",
            [InterestedIn.LoaderOrHeavyEquipment] = "لودر أو معدة ثقيلة",
            [InterestedIn.Scooter] = "سكوتر",
            [InterestedIn.Car] = "سيارة",
            [InterestedIn.Excavator] = "حفار",
            [InterestedIn.Crane] = "ونش",
            [InterestedIn.Tractor] = "جرار",
            [InterestedIn.AnyHeavyEquipment] = "أي معدة ثقيلة",
            [InterestedIn.Taxi] = "تاكسي",
            [InterestedIn.Microbus] = "ميكروباص",
            [InterestedIn.Limousine] = "ليموزين",
            [InterestedIn.Other] = "أخرى"
        };

    public static readonly IReadOnlyList<CarNameOptionDto> CarBrands = BuildGrouped(
        ("ياباني", new[]
        {
            "Toyota", "Nissan", "Honda", "Mazda", "Mitsubishi", "Suzuki", "Subaru",
            "Lexus", "Infiniti", "Isuzu", "Daihatsu"
        }),
        ("كوري", new[] { "Hyundai", "Kia", "Genesis", "Daewoo", "SsangYong" }),
        ("ألماني", new[]
        {
            "Mercedes-Benz", "BMW", "Audi", "Volkswagen", "Porsche", "Opel", "MINI", "Smart"
        }),
        ("فرنسي", new[] { "Peugeot", "Renault", "Citroën", "DS" }),
        ("إيطالي", new[] { "Fiat", "Alfa Romeo", "Ferrari", "Lamborghini", "Maserati" }),
        ("أمريكي", new[]
        {
            "Chevrolet", "Ford", "Jeep", "Dodge", "Chrysler", "GMC", "Cadillac",
            "Lincoln", "Buick", "Tesla"
        }),
        ("بريطاني", new[]
        {
            "Land Rover", "Range Rover", "Jaguar", "Bentley", "Rolls-Royce",
            "Aston Martin", "MG", "Lotus"
        }),
        ("صيني", new[]
        {
            "Chery", "BYD", "Geely", "Haval", "Jetour", "GAC", "BAIC", "Dongfeng", "JAC",
            "Changan", "Foton", "Great Wall", "Exeed", "Hongqi", "NIO", "XPeng", "Li Auto", "Zeekr"
        }),
        ("هندي", new[] { "Tata", "Mahindra" }),
        ("أخرى", new[] { OtherOptionValue }));

    public static readonly IReadOnlyList<CarNameOptionDto> MotorcycleBrands = BuildFlat(
        "Honda", "Yamaha", "Suzuki", "Kawasaki", "Harley-Davidson", "Ducati", "BMW", "KTM",
        "Bajaj", "TVS", "Benelli", "SYM", "Kymco", "Haojue", "Dayun", "Loncin", "Lifan",
        "Voge", "Zontes", OtherOptionValue);

    public static readonly IReadOnlyList<CarNameOptionDto> EquipmentBrands = BuildFlat(
        "Caterpillar (CAT)", "Komatsu", "Volvo", "Hyundai", "Hitachi", "JCB", "Doosan", "Case",
        "Liebherr", "Bobcat", "New Holland", "John Deere", "XCMG", "SANY", "LiuGong", "SDLG",
        "Zoomlion", "Kobelco", "Kubota", OtherOptionValue);

    public static readonly IReadOnlyList<CarNameOptionDto> CarModels = BuildGrouped(
        ("Toyota", new[]
        {
            "Corolla", "Camry", "Yaris", "Avalon", "Prius", "C-HR", "Raize", "Rush",
            "Fortuner", "Prado", "Land Cruiser", "Hilux", "Hiace", "RAV4"
        }),
        ("Hyundai", new[]
        {
            "Accent", "Verna", "Elantra", "Avante", "Sonata", "Azera", "i10", "i20", "i30",
            "Venue", "Creta", "Tucson", "Santa Fe", "Palisade", "Kona", "Staria"
        }),
        ("Kia", new[]
        {
            "Picanto", "Rio", "Cerato", "K3", "K5", "K8", "Sportage", "Seltos", "Sonet",
            "Sorento", "Carnival"
        }),
        ("Nissan", new[]
        {
            "Sunny", "Sentra", "Altima", "Maxima", "Tiida", "Patrol", "X-Trail", "Qashqai",
            "Juke", "Kicks", "Pathfinder", "Navara"
        }));

    public static readonly IReadOnlyList<CarNameOptionDto> CarColors = BuildFlat(
        "أبيض", "أسود", "فضي", "رمادي", "بني", "بيج", "أحمر", "أزرق", "كحلي", "أخضر",
        "أصفر", "برتقالي", "ذهبي", OtherOptionValue);

    public static readonly IReadOnlyList<CarNameOptionDto> TaxiColors = BuildFlat(
        "أبيض", "أسود", "فضي", "رمادي", "أحمر", "أزرق", "أصفر", "أخضر", "برتقالي", OtherOptionValue);

    public static readonly IReadOnlyList<CarNameOptionDto> MotorcycleColors = BuildFlat(
        "أسود", "أبيض", "أحمر", "أزرق", "أصفر", "أخضر", "فضي", "برتقالي", OtherOptionValue);

    public static readonly IReadOnlyList<CarNameOptionDto> EquipmentColors = BuildFlat(
        "أصفر", "برتقالي", "أبيض", "أسود", "أزرق", "أخضر", "أحمر", OtherOptionValue);

    public static readonly int[] CarEngineCapacities =
        [800, 1000, 1200, 1300, 1400, 1500, 1600, 1800, 2000, 2500, 3000];

    public static readonly int[] TaxiEngineCapacities =
        [1000, 1300, 1500, 1600, 1800, 2000, 2500, 3000];

    public static readonly int[] MotorcycleEngineCapacities =
        [50, 70, 100, 125, 150, 180, 200, 250, 300, 400, 600, 750, 1000];

    public static readonly int[] DoorCounts = [2, 3, 4, 5];

    public static readonly int[] SeatCounts = [2, 4, 5, 7, 8];

    public static readonly int[] PassengerCounts = [4, 5, 7, 8, 10, 12, 14, 15, 26];

    public const string RoutePlaceholder = "مثال: الفيوم - القاهرة، الفيوم - سنورس، الفيوم - إطسا";

    public const string BrandPlaceholder = "مثال: تويوتا، BMW، BYD";

    public const string ModelPlaceholder = "مثال: كورولا، X5، Seal";

    public sealed record CarFeature(int Id, string Name, string Group, VehicleFeatureScope Scope);

    private const VehicleFeatureScope PrivateOnly = VehicleFeatureScope.Private;
    private const VehicleFeatureScope TaxiOnly = VehicleFeatureScope.Taxi;
    private const VehicleFeatureScope MotorcycleOnly = VehicleFeatureScope.Motorcycles;
    private const VehicleFeatureScope EquipmentOnly = VehicleFeatureScope.HeavyEquipment;
    private const VehicleFeatureScope PrivateAndTaxi = VehicleFeatureScope.Private | VehicleFeatureScope.Taxi;

    public const string GroupSafety = "أنظمة الأمان";
    public const string GroupComfort = "الراحة";
    public const string GroupTechnology = "التكنولوجيا";
    public const string GroupExterior = "التجهيزات الخارجية";
    public const string GroupOther = "أخرى";
    public const string GroupTaxiExtras = "تجهيزات إضافية";
    public const string GroupMotorcycleSafety = "الأمان";
    public const string GroupMotorcycleEquipment = "التجهيزات";
    public const string GroupEquipmentFittings = "التجهيزات";
    public const string GroupEquipmentSafety = "وسائل الأمان";

    public static readonly IReadOnlyList<CarFeature> Features =
    [

        new(1, "ABS", GroupSafety, PrivateAndTaxi | VehicleFeatureScope.Motorcycles),
        new(2, "EBD", GroupSafety, PrivateAndTaxi),
        new(3, "ESP", GroupSafety, PrivateAndTaxi),
        new(4, "TCS (مانع الانزلاق)", GroupSafety, PrivateOnly),
        new(5, "Airbags", GroupSafety, PrivateAndTaxi),
        new(6, "كاميرا خلفية", GroupTechnology, PrivateAndTaxi | VehicleFeatureScope.HeavyEquipment),
        new(7, "كاميرات 360°", GroupTechnology, PrivateAndTaxi),
        new(8, "حساسات ركن أمامية", GroupExterior, PrivateOnly),
        new(9, "حساسات ركن خلفية", GroupExterior, PrivateOnly),
        new(10, "شاشة", GroupTechnology, PrivateAndTaxi | VehicleFeatureScope.HeavyEquipment),
        new(11, "Apple CarPlay", GroupTechnology, PrivateAndTaxi),
        new(12, "Android Auto", GroupTechnology, PrivateAndTaxi),
        new(13, "بلوتوث", GroupTechnology, PrivateAndTaxi | VehicleFeatureScope.Motorcycles),
        new(14, "GPS", GroupTechnology, PrivateAndTaxi | VehicleFeatureScope.HeavyEquipment),
        new(15, "مثبت سرعة", GroupComfort, PrivateAndTaxi),
        new(16, "مثبت سرعة تكيفي", GroupComfort, PrivateOnly),
        new(17, "فتحة سقف", GroupComfort, PrivateOnly),
        new(18, "سقف بانوراما", GroupComfort, PrivateOnly),
        new(19, "تكييف", GroupComfort, PrivateAndTaxi | VehicleFeatureScope.HeavyEquipment),
        new(20, "تكييف أوتوماتيك", GroupComfort, PrivateAndTaxi),
        new(21, "مقاعد كهربائية", GroupComfort, PrivateOnly),
        new(22, "مقاعد مدفأة", GroupComfort, PrivateOnly),
        new(23, "مقاعد مبردة", GroupComfort, PrivateOnly),
        new(24, "فرش جلد", GroupComfort, PrivateOnly),
        new(25, "عجلة قيادة متعددة الوظائف", GroupComfort, PrivateOnly),
        new(26, "تشغيل بدون مفتاح", GroupComfort, PrivateOnly | VehicleFeatureScope.Motorcycles),
        new(27, "بصمة", GroupComfort, PrivateOnly),
        new(28, "ريموت", GroupComfort, PrivateAndTaxi),
        new(29, "مرايا كهربائية", GroupExterior, PrivateAndTaxi),
        new(30, "طي مرايا كهربائي", GroupExterior, PrivateOnly),
        new(31, "زجاج كهربائي", GroupExterior, PrivateAndTaxi),
        new(32, "إضاءة LED", GroupExterior, PrivateAndTaxi | VehicleFeatureScope.HeavyEquipment),
        new(33, "فوانيس ضباب", GroupExterior, PrivateAndTaxi),
        new(34, "جنوط", GroupTaxiExtras, TaxiOnly),
        new(35, "حساس إضاءة", GroupExterior, PrivateOnly),
        new(36, "حساس مطر", GroupExterior, PrivateOnly),
        new(37, "نظام مراقبة ضغط الإطارات TPMS", GroupSafety, PrivateAndTaxi),
        new(38, "مساعد صعود المرتفعات", GroupSafety, PrivateOnly),
        new(39, "مثبت نزول المنحدرات", GroupSafety, PrivateOnly),
        new(40, "إنذار ضد السرقة", GroupSafety, PrivateAndTaxi),
        new(41, "سنتر لوك", GroupTaxiExtras, TaxiOnly),

        new(42, "Immobilizer", GroupSafety, PrivateOnly),
        new(43, "ISOFIX", GroupSafety, PrivateOnly),
        new(44, "ستائر خلفية", GroupComfort, PrivateOnly),
        new(45, "زجاج فاميه", GroupComfort, PrivateOnly),
        new(46, "شاشة عدادات رقمية", GroupTechnology, PrivateOnly),
        new(47, "USB", GroupTechnology, PrivateAndTaxi),
        new(48, "AUX", GroupTechnology, PrivateOnly),
        new(49, "شاحن لاسلكي", GroupTechnology, PrivateOnly),
        new(50, "نظام صوت Premium", GroupTechnology, PrivateOnly),
        new(51, "جنوط سبور", GroupExterior, PrivateOnly | VehicleFeatureScope.Motorcycles),
        new(52, "إضاءة Xenon", GroupExterior, PrivateOnly),
        new(53, "إضاءة Laser", GroupExterior, PrivateOnly),
        new(54, "Spoiler", GroupExterior, PrivateOnly),
        new(55, "Roof Rails", GroupExterior, PrivateOnly),
        new(56, "استبن", GroupOther, PrivateOnly),
        new(57, "عدة السيارة", GroupOther, PrivateOnly),
        new(58, "مفتاح احتياطي", GroupOther, PrivateOnly),
        new(59, "كتيب السيارة الأصلي", GroupOther, PrivateOnly),

        new(60, "مقاعد جلد", GroupComfort, TaxiOnly),
        new(61, "مقاعد قماش", GroupComfort, TaxiOnly),
        new(62, "حساسات ركن", GroupTaxiExtras, TaxiOnly),

        new(63, "CBS", GroupMotorcycleSafety, MotorcycleOnly),
        new(64, "مانع سرقة", GroupMotorcycleSafety, MotorcycleOnly),
        new(65, "إنذار", GroupMotorcycleSafety, MotorcycleOnly | VehicleFeatureScope.HeavyEquipment),
        new(66, "فرامل ديسك أمامي", GroupMotorcycleSafety, MotorcycleOnly),
        new(67, "فرامل ديسك خلفي", GroupMotorcycleSafety, MotorcycleOnly),
        new(68, "تشغيل بالبصمة", GroupComfort, MotorcycleOnly),
        new(69, "شاشة رقمية", GroupComfort, MotorcycleOnly),
        new(70, "USB Charger", GroupComfort, MotorcycleOnly),
        new(71, "صندوق خلفي", GroupMotorcycleEquipment, MotorcycleOnly),
        new(72, "صندوق جانبي", GroupMotorcycleEquipment, MotorcycleOnly),
        new(73, "زجاج أمامي", GroupMotorcycleEquipment, MotorcycleOnly),
        new(74, "Hand Guards", GroupMotorcycleEquipment, MotorcycleOnly),
        new(75, "Crash Bar", GroupMotorcycleEquipment, MotorcycleOnly),
        new(76, "LED", GroupMotorcycleEquipment, MotorcycleOnly),

        new(77, "كابينة مغلقة", GroupEquipmentFittings, EquipmentOnly),
        new(78, "كرسي هوائي", GroupEquipmentFittings, EquipmentOnly),
        new(79, "نظام هيدروليك إضافي", GroupEquipmentFittings, EquipmentOnly),
        new(80, "وصلة كسارة", GroupEquipmentFittings, EquipmentOnly),
        new(81, "وصلة حفار", GroupEquipmentFittings, EquipmentOnly),
        new(82, "وصلة شوكة", GroupEquipmentFittings, EquipmentOnly),
        new(83, "وصلة جردل إضافي", GroupEquipmentFittings, EquipmentOnly),
        new(84, "ROPS", GroupEquipmentSafety, EquipmentOnly),
        new(85, "FOPS", GroupEquipmentSafety, EquipmentOnly),
        new(86, "طفاية حريق", GroupEquipmentSafety, EquipmentOnly),
        new(87, "إنذار رجوع للخلف", GroupEquipmentSafety, EquipmentOnly),
        new(88, "كاميرات", GroupEquipmentSafety, EquipmentOnly),
        new(89, "أحزمة أمان", GroupEquipmentSafety, EquipmentOnly)
    ];

    public static IReadOnlyList<CarFeature> FeaturesOf(SubCategoryType subCategory)
    {
        var scope = ScopeOf(subCategory);
        var groups = GroupOrderOf(subCategory);

        return Features
            .Where(feature => feature.Scope.HasFlag(scope))
            .Select(feature => subCategory switch
            {
                SubCategoryType.Motorcycles => feature with { Group = MotorcycleGroupOf(feature) },
                SubCategoryType.HeavyEquipment => feature with { Group = EquipmentGroupOf(feature) },
                _ => feature
            })
            .OrderBy(feature => Array.IndexOf(groups, feature.Group))
            .ThenBy(feature => feature.Id)
            .ToList();
    }

    public static VehicleFeatureScope ScopeOf(SubCategoryType subCategory) => subCategory switch
    {
        SubCategoryType.Private => VehicleFeatureScope.Private,
        SubCategoryType.Taxi => VehicleFeatureScope.Taxi,
        SubCategoryType.Motorcycles => VehicleFeatureScope.Motorcycles,
        SubCategoryType.HeavyEquipment => VehicleFeatureScope.HeavyEquipment,
        _ => 0
    };

    private static string[] GroupOrderOf(SubCategoryType subCategory) => subCategory switch
    {
        SubCategoryType.Private => [GroupSafety, GroupComfort, GroupTechnology, GroupExterior, GroupOther],
        SubCategoryType.Taxi => [GroupSafety, GroupComfort, GroupTechnology, GroupTaxiExtras],
        SubCategoryType.Motorcycles => [GroupMotorcycleSafety, GroupComfort, GroupMotorcycleEquipment],
        SubCategoryType.HeavyEquipment => [GroupEquipmentFittings, GroupEquipmentSafety],
        _ => []
    };

    private static string MotorcycleGroupOf(CarFeature feature) => feature.Id switch
    {
        1 => GroupMotorcycleSafety,
        13 or 26 => GroupComfort,
        51 => GroupMotorcycleEquipment,
        _ => feature.Group
    };

    private static string EquipmentGroupOf(CarFeature feature) => feature.Id switch
    {
        6 or 10 or 14 or 19 or 32 => GroupEquipmentFittings,
        65 => GroupEquipmentFittings,
        _ => feature.Group
    };

    public static int? Combine<TFlag>(IEnumerable<TFlag>? selected)
        where TFlag : struct, Enum
    {
        if (selected is null)
            return null;

        var combined = selected
            .Distinct()
            .Aggregate(0, (total, flag) => total | Convert.ToInt32(flag));

        return combined == 0 ? null : combined;
    }

    public static List<TFlag> Split<TFlag>(int? stored)
        where TFlag : struct, Enum
    {
        if (stored is not { } value || value == 0)
            return [];

        return Enum.GetValues<TFlag>()
            .Where(flag =>
            {
                var bit = Convert.ToInt32(flag);
                return bit != 0 && (value & bit) == bit;
            })
            .ToList();
    }

    public static List<string> NamesOf<TFlag>(
        IEnumerable<TFlag> values, IReadOnlyDictionary<TFlag, string> names)
        where TFlag : struct, Enum =>
        values
            .Select(value => names.TryGetValue(value, out var name) ? name : value.ToString())
            .ToList();

    private static IReadOnlyList<CarNameOptionDto> BuildFlat(params string[] names) =>
        names.Select(name => new CarNameOptionDto { Value = name, Name = name }).ToList();

    private static IReadOnlyList<CarNameOptionDto> BuildGrouped(
        params (string Group, string[] Names)[] groups) =>
        groups
            .SelectMany(group => group.Names.Select(name => new CarNameOptionDto
            {
                Value = name,
                Name = name,
                Group = group.Group
            }))
            .ToList();
}
