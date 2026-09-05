using Shared.Constants;
using Shared.DTOs.Lookups.Forms;
using Shared.Enums;
using static Services.AdForms.AdFormFieldFactory;

namespace Services.AdForms;

public static class AdFormSchemaCatalog
{
    private static readonly CreateAdFormSubmitDto AdvertisementsSubmit = new("/api/ads");
    private static readonly CreateAdFormSubmitDto WorkshopsSubmit = new("/api/workshops");
    private static readonly CreateAdFormSubmitDto CraftsmenSubmit = new("/api/craftsmen");
    private static readonly CreateAdFormSubmitDto LostFoundSubmit = new("/api/lost-found");

    private static readonly CreateAdFormSubmitDto FactoriesSubmit = new("/api/factories");
    private static readonly CreateAdFormSubmitDto FarmsSubmit = new("/api/farms");
    private static readonly CreateAdFormSubmitDto CompaniesSubmit = new("/api/companies");
    private static readonly CreateAdFormSubmitDto SuppliersSubmit = new("/api/suppliers");
    private static readonly CreateAdFormSubmitDto WholesaleTradersSubmit = new("/api/wholesale-traders");
    private static readonly CreateAdFormSubmitDto FruitMerchantsSubmit = new("/api/fruit-vegetable-merchants");

    private static readonly CreateAdFormSubmitDto JobRequestsSubmit = new("/api/job-requests");
    private static readonly CreateAdFormSubmitDto JobOpportunitiesSubmit = new("/api/job-opportunities");

    private static readonly CreateAdFormSubmitDto LivestockSubmit = new("/api/livestock");
    private static readonly CreateAdFormSubmitDto SheepGoatSubmit = new("/api/sheep-goats");
    private static readonly CreateAdFormSubmitDto HorseSubmit = new("/api/horses");
    private static readonly CreateAdFormSubmitDto CamelSubmit = new("/api/camels");
    private static readonly CreateAdFormSubmitDto BirdSubmit = new("/api/birds");
    private static readonly CreateAdFormSubmitDto PetSubmit = new("/api/pets");
    private static readonly CreateAdFormSubmitDto FishSubmit = new("/api/fish");
    private static readonly CreateAdFormSubmitDto BeeSubmit = new("/api/bees");
    private static readonly CreateAdFormSubmitDto OtherAnimalSubmit = new("/api/other-animals");

    private static readonly CreateAdFormSubmitDto DecorAntiqueSubmit = new("/api/decor-antiques");
    private static readonly CreateAdFormSubmitDto AntiqueSubmit = new("/api/antiques");
    private static readonly CreateAdFormSubmitDto PaintingSubmit = new("/api/paintings");
    private static readonly CreateAdFormSubmitDto HandmadeSubmit = new("/api/handmade");
    private static readonly CreateAdFormSubmitDto CoinStampSubmit = new("/api/coins-stamps");

    private static readonly CreateAdFormSubmitDto MenClothingSubmit = new("/api/men-clothing");
    private static readonly CreateAdFormSubmitDto WomenClothingSubmit = new("/api/women-clothing");
    private static readonly CreateAdFormSubmitDto KidsClothingSubmit = new("/api/kids-clothing");

    private static readonly CreateAdFormSubmitDto AccessoriesSubmit = new($"/{OnlineShoppingRoutes.Accessories}");
    private static readonly CreateAdFormSubmitDto CosmeticsSubmit = new($"/{OnlineShoppingRoutes.Cosmetics}");
    private static readonly CreateAdFormSubmitDto HomeKitchenSubmit = new($"/{OnlineShoppingRoutes.HomeKitchen}");
    private static readonly CreateAdFormSubmitDto ShoppingElectronicsSubmit = new($"/{OnlineShoppingRoutes.ShoppingElectronics}");
    private static readonly CreateAdFormSubmitDto GiftsToysSubmit = new($"/{OnlineShoppingRoutes.GiftsToys}");
    private static readonly CreateAdFormSubmitDto HomemadeFoodSubmit = new($"/{OnlineShoppingRoutes.HomemadeFood}");

    private static readonly CreateAdFormSubmitDto LandsSubmit = new($"/{RealEstateRoutes.Lands}");
    private static readonly CreateAdFormSubmitDto ApartmentsSubmit = new($"/{RealEstateRoutes.Apartments}");
    private static readonly CreateAdFormSubmitDto ShopsSubmit = new($"/{RealEstateRoutes.Shops}");

    private static readonly CreateAdFormSubmitDto FurnitureSubmit = new($"/{HomeFurnishingRoutes.Furniture}");
    private static readonly CreateAdFormSubmitDto FurnishingCurtainSubmit = new($"/{HomeFurnishingRoutes.FurnishingCurtains}");
    private static readonly CreateAdFormSubmitDto LightingDecorSubmit = new($"/{HomeFurnishingRoutes.LightingDecor}");
    private static readonly CreateAdFormSubmitDto KitchenToolSubmit = new($"/{HomeFurnishingRoutes.KitchenTools}");
    private static readonly CreateAdFormSubmitDto HomeApplianceSubmit = new($"/{HomeFurnishingRoutes.HomeAppliances}");
    private static readonly CreateAdFormSubmitDto BathroomSupplySubmit = new($"/{HomeFurnishingRoutes.BathroomSupplies}");
    private static readonly CreateAdFormSubmitDto PlantOrnamentSubmit = new($"/{HomeFurnishingRoutes.PlantsOrnaments}");

    public static AdFormSchema? GetSchema(int categoryId, int? subCategoryId)
    {
        if (subCategoryId is { } id && Enum.IsDefined(typeof(SubCategoryType), id))
            return GetSubCategorySchema((SubCategoryType)id);

        return categoryId switch
        {
            _ => null
        };
    }

    private static AdFormSchema? GetSubCategorySchema(SubCategoryType subCategory) => subCategory switch
    {
        SubCategoryType.Private => Cars(SubCategoryType.Private),
        SubCategoryType.Taxi => Cars(SubCategoryType.Taxi),
        SubCategoryType.Motorcycles => Cars(SubCategoryType.Motorcycles),
        SubCategoryType.HeavyEquipment => Cars(SubCategoryType.HeavyEquipment),
        SubCategoryType.Workshops => Workshops(),
        SubCategoryType.Craftsmen => Craftsmen(),
        SubCategoryType.LostItems => LostFound(PostType.Lost),
        SubCategoryType.FoundItems => LostFound(PostType.Found),

        SubCategoryType.Rescues or SubCategoryType.BloodRequests or SubCategoryType.AskConsults =>
            CharityFormSchemas.For(subCategory),
        SubCategoryType.Factories => Factories(),
        SubCategoryType.Farms => Farms(),
        SubCategoryType.Companies => Companies(),
        SubCategoryType.Suppliers => Suppliers(),
        SubCategoryType.WholesaleTraders => WholesaleTraders(),
        SubCategoryType.FruitAndVegetableTraders => FruitMerchants(),

        SubCategoryType.JobRequests => JobRequests(),
        SubCategoryType.JobOpportunities => JobOpportunities(),
        SubCategoryType.Livestock => Livestock(),
        SubCategoryType.SheepAndGoats => SheepGoat(),
        SubCategoryType.Horses => Horse(),
        SubCategoryType.Camels => Camel(),
        SubCategoryType.Birds => Bird(),
        SubCategoryType.Pets => Pet(),
        SubCategoryType.Fish => Fish(),
        SubCategoryType.Bees => Bee(),
        SubCategoryType.OtherAnimals => OtherAnimal(),

        SubCategoryType.DecorAntiques => DecorAntiques(),
        SubCategoryType.Antiques => Antiques(),
        SubCategoryType.Paintings => Paintings(),
        SubCategoryType.Handmade => Handmade(),
        SubCategoryType.CoinsAndStamps => CoinsStamps(),

        SubCategoryType.MenClothing => MenClothing(),
        SubCategoryType.WomenClothing => WomenClothing(),
        SubCategoryType.KidsClothing => KidsClothing(),

        SubCategoryType.Accessories => Accessories(),
        SubCategoryType.Cosmetics => Cosmetics(),
        SubCategoryType.HomeAndKitchen => HomeKitchen(),
        SubCategoryType.ShoppingElectronics => ShoppingElectronics(),
        SubCategoryType.GiftsAndToys => GiftsToys(),
        SubCategoryType.HomemadeFood => HomemadeFood(),

        SubCategoryType.Furniture => Furniture(),
        SubCategoryType.FurnishingsAndCurtains => FurnishingCurtains(),
        SubCategoryType.LightingAndDecor => LightingDecor(),
        SubCategoryType.KitchenTools => KitchenTools(),
        SubCategoryType.HomeAppliances => HomeAppliances(),
        SubCategoryType.BathroomSupplies => BathroomSupplies(),
        SubCategoryType.PlantsAndOrnaments => PlantsOrnaments(),

        SubCategoryType.Lands => Lands(),
        SubCategoryType.Apartments => Apartments(),
        SubCategoryType.Shops => Shops(),

        _ => null
    };

    public static IReadOnlyList<FormFieldDto> SubCategorySelectionFields() =>
        Order(new List<FormFieldDto>
        {
            LookupSelect("SubCategoryId", "القسم الفرعي", "Sub category", SectionAd,
                AdFormLookupKeys.SubCategories, required: true)
        });

    private static AdFormSchema Cars(SubCategoryType subCategory)
    {
        var lookups = new List<string>
        {
            AdFormLookupKeys.ListingTypes,
            AdFormLookupKeys.Governorates,
            AdFormLookupKeys.Centers,
            BrandsKeyOf(subCategory),
            ColorsKeyOf(subCategory),
            FeaturesKeyOf(subCategory)
        };

        if (subCategory is SubCategoryType.Private or SubCategoryType.Taxi)
            lookups.Add(AdFormLookupKeys.CarModels);

        if (EngineCapacitiesKeyOf(subCategory) is { } capacities)
            lookups.Add(capacities);

        if (subCategory is SubCategoryType.Private)
        {
            lookups.Add(AdFormLookupKeys.CarDoorCounts);
            lookups.Add(AdFormLookupKeys.CarSeatCounts);
        }

        if (subCategory is SubCategoryType.Taxi)
            lookups.Add(AdFormLookupKeys.TaxiPassengerCounts);

        var fields = new List<FormFieldDto>();

        fields.AddRange(SelectionFields((int)CategoryType.Cars, (int)subCategory));

        fields.AddRange(CarAdFields());
        fields.AddRange(VehicleFieldsOf(subCategory));
        fields.AddRange(LicenseFields(subCategory));

        if (subCategory is SubCategoryType.Private)
        {
            fields.AddRange(InsuranceFields());
            fields.AddRange(InspectionFields());
            fields.AddRange(FinanceFields());
        }

        fields.AddRange(RentFields(subCategory));
        fields.AddRange(AccidentFields(subCategory));
        fields.AddRange(ExchangeFields(subCategory));

        fields.Add(FeatureIdsField(subCategory));

        fields.AddRange(LocationFields());
        fields.Add(DetailedAddress());
        fields.Add(PhoneNumber());

        fields.Add(RequiredImages());
        fields.Add(VideoUpload());

        return new AdFormSchema("advertisements", AdvertisementsSubmit, lookups, Order(fields));
    }

    private static IEnumerable<FormFieldDto> CarAdFields() =>
    [
        LookupSelect("ListingType", "نوع الإعلان", "Listing type", SectionAd,
            AdFormLookupKeys.ListingTypes, required: true),
        Title(),
        Description(),
        VehicleSalePrice(),
        Negotiable()
    ];

    private static FormFieldDto VehicleSalePrice() =>
        Number("Price", "السعر (جنيه)", "Price", SectionAd, required: false, min: 0.01m)
            .VisibleOnlyWhen("ListingType", (int)ListingType.Sale, (int)ListingType.Accident)
            .MandatoryWhen("ListingType", (int)ListingType.Sale, (int)ListingType.Accident);

    private static IEnumerable<FormFieldDto> VehicleFieldsOf(SubCategoryType subCategory) =>
        subCategory switch
        {
            SubCategoryType.Private => PrivateCarFields(),
            SubCategoryType.Taxi => TaxiFields(),
            SubCategoryType.Motorcycles => MotorcycleFields(),
            SubCategoryType.HeavyEquipment => HeavyEquipmentFields(),
            _ => []
        };

    private static IEnumerable<FormFieldDto> PrivateCarFields()
    {
        const SubCategoryType sub = SubCategoryType.Private;
        var section = CarCatalog.DetailsSectionOf(sub);

        yield return TypedBrandField(section);
        yield return TypedModelField(section);
        yield return ManufacturingYear(section);
        yield return ColorField(sub, section);
        yield return OtherColorField(section);
        yield return Kilometers(section);

        yield return EnumSelect("Transmission", "ناقل الحركة", "Transmission", section,
            CarCatalog.TransmissionNames, required: true);
        yield return EnumSelect("FuelType", "نوع الوقود", "Fuel type", section,
            CarCatalog.FuelTypeNames, required: true);
        yield return EnumSelect("Condition", "الحالة", "Condition", section,
            CarCatalog.ConditionNames, required: true);
        yield return EnumSelect("TechnicalCondition", "الحالة الفنية", "Technical condition", section,
            CarCatalog.TechnicalConditionNames, required: true);
        yield return EnumSelect("BodyType", "نوع الهيكل", "Body type", section,
            CarCatalog.BodyTypeNames, required: true);

        yield return SuggestedBy(
            Number("DoorsCount", "عدد الأبواب", "Doors count", section, required: true, min: 1, max: 10),
            AdFormLookupKeys.CarDoorCounts);
        yield return SuggestedBy(
            Number("SeatsCount", "عدد المقاعد", "Seats count", section, required: false, min: 1, max: 20),
            AdFormLookupKeys.CarSeatCounts);
        yield return EngineCCField(sub, section, required: true);

        yield return EnumSelect("OriginCountry", "بلد المنشأ", "Origin country", section,
            CarCatalog.OriginCountryNames, required: false);
        yield return EnumSelect("AssemblyCountry", "بلد التجميع", "Assembly country", section,
            CarCatalog.AssemblyCountryNames, required: false);

        yield return Checkbox("FirstOwner", "أول مالك", "First owner", section);
        yield return EnumSelect("PreviousOwners", "عدد الملاك السابقين", "Previous owners", section,
            CarCatalog.PreviousOwnersNames, required: false);
        yield return EnumSelect("UsageType", "نوع الاستخدام", "Usage type", section,
            CarCatalog.UsageTypeNames, required: false);

        yield return Checkbox("PartsChanged", "تم تغيير أي جزء من السيارة", "Parts changed", section);
        yield return ChangedPartsField(CarCatalog.ChangedPartNames, section);

        yield return EnumSelect("AccidentsCount", "عدد الحوادث السابقة", "Previous accidents", section,
            CarCatalog.AccidentsCountNames, required: false);
        yield return Checkbox("HasMaintenanceBook", "يوجد كتاب صيانة", "Has maintenance book", section);
        yield return Checkbox("AllMaintenanceAtDealer", "جميع الصيانات بالتوكيل",
            "All maintenance at dealer", section);
    }

    private static IEnumerable<FormFieldDto> TaxiFields()
    {
        const SubCategoryType sub = SubCategoryType.Taxi;
        var section = CarCatalog.DetailsSectionOf(sub);

        yield return EnumSelect("VehicleType", "نوع المركبة", "Vehicle type", section,
            CarCatalog.TaxiVehicleTypeNames, required: true);
        yield return OtherTextField("OtherVehicleType", "اسم النوع", "Vehicle type name", section,
            "VehicleType", (int)TaxiVehicleType.Other);

        yield return TypedBrandField(section);
        yield return TypedModelField(section);
        yield return ManufacturingYear(section);
        yield return ColorField(sub, section);
        yield return OtherColorField(section);
        yield return Kilometers(section);

        yield return EnumSelect("Transmission", "ناقل الحركة", "Transmission", section,
            CarCatalog.TransmissionNames, required: true);
        yield return EnumSelect("FuelType", "نوع الوقود", "Fuel type", section,
            CarCatalog.TaxiFuelTypeNames, required: true);
        yield return EnumSelect("Condition", "الحالة", "Condition", section,
            CarCatalog.ConditionNames, required: true);
        yield return EnumSelect("TechnicalCondition", "الحالة الفنية", "Technical condition", section,
            CarCatalog.TechnicalConditionNames, required: true);
        yield return EngineCCField(sub, section, required: true);

        yield return SuggestedBy(
            Number("PassengersCount", "عدد الركاب", "Passengers count", section,
                required: false, min: 1, max: 60),
            AdFormLookupKeys.TaxiPassengerCounts);
        yield return EnumSelect("ActivityType", "نوع النشاط", "Activity type", section,
            CarCatalog.TaxiActivityTypeNames, required: false);
        yield return Text("Route", "خط السير", "Route", section,
            required: false, maxLength: 300, placeholder: CarCatalog.RoutePlaceholder);

        yield return EnumSelect("OriginCountry", "بلد المنشأ", "Origin country", section,
            CarCatalog.TaxiOriginCountryNames, required: false);
        yield return EnumSelect("AssemblyCountry", "بلد التجميع", "Assembly country", section,
            CarCatalog.AssemblyCountryNames, required: false);

        yield return Checkbox("FirstOwner", "أول مالك", "First owner", section);
        yield return EnumSelect("PreviousOwners", "عدد الملاك السابقين", "Previous owners", section,
            CarCatalog.PreviousOwnersNames, required: false);

        yield return Checkbox("PartsChanged", "تم تغيير أي جزء من المركبة", "Parts changed", section);
        yield return ChangedPartsField(CarCatalog.TaxiChangedPartNames, section);

        yield return EnumSelect("AccidentsCount", "عدد الحوادث السابقة", "Previous accidents", section,
            CarCatalog.AccidentsCountNames, required: false);
        yield return Checkbox("HasMaintenanceBook", "يوجد كتاب صيانة", "Has maintenance book", section);
        yield return Checkbox("AllMaintenanceAtDealer", "جميع الصيانات بالتوكيل",
            "All maintenance at dealer", section);
    }

    private static IEnumerable<FormFieldDto> MotorcycleFields()
    {
        const SubCategoryType sub = SubCategoryType.Motorcycles;
        var section = CarCatalog.DetailsSectionOf(sub);

        yield return EnumSelect("MotorcycleType", "النوع", "Type", section,
            CarCatalog.MotorcycleTypeNames, required: true);

        yield return BrandField(sub, section);
        yield return OtherBrandField(section);

        yield return Text("Model", "الموديل", "Model", section, required: true, maxLength: 100);
        yield return ManufacturingYear(section);
        yield return ColorField(sub, section);
        yield return OtherColorField(section);
        yield return Kilometers(section);

        yield return EngineCCField(sub, section, required: true);
        yield return EnumSelect("Transmission", "ناقل الحركة", "Transmission", section,
            CarCatalog.MotorcycleTransmissionNames, required: true);
        yield return EnumSelect("FuelType", "نوع الوقود", "Fuel type", section,
            CarCatalog.MotorcycleFuelTypeNames, required: true);
        yield return EnumSelect("CoolingType", "نظام التبريد", "Cooling type", section,
            CarCatalog.CoolingTypeNames, required: true);
        yield return EnumSelect("StartType", "تشغيل", "Start type", section,
            CarCatalog.MotorcycleStartTypeNames, required: false);

        yield return EnumSelect("Condition", "الحالة", "Condition", section,
            CarCatalog.MotorcycleConditionNames, required: true);
        yield return EnumSelect("TechnicalCondition", "الحالة الفنية", "Technical condition", section,
            CarCatalog.TechnicalConditionNames, required: true);
        yield return EnumSelect("OriginCountry", "بلد المنشأ", "Origin country", section,
            CarCatalog.MotorcycleOriginCountryNames, required: false);

        yield return Checkbox("FirstOwner", "أول مالك", "First owner", section);
        yield return EnumSelect("PreviousOwners", "عدد الملاك السابقين", "Previous owners", section,
            CarCatalog.PreviousOwnersNames, required: false);
        yield return EnumSelect("AccidentsCount", "عدد الحوادث السابقة", "Previous accidents", section,
            CarCatalog.MotorcycleAccidentsCountNames, required: false);

        yield return Checkbox("EngineChanged", "تم تغيير موتور", "Engine changed", section);
        yield return Checkbox("ChassisChanged", "تم تغيير شاسيه", "Chassis changed", section);
    }

    private static IEnumerable<FormFieldDto> HeavyEquipmentFields()
    {
        const SubCategoryType sub = SubCategoryType.HeavyEquipment;
        var section = CarCatalog.DetailsSectionOf(sub);

        yield return EnumSelect("MachineType", "نوع المعدة", "Machine type", section,
            CarCatalog.MachineTypeNames, required: true);
        yield return OtherTextField("OtherMachineType", "اسم المعدة", "Machine name", section,
            "MachineType", (int)EquipmentMachineType.Other);

        yield return BrandField(sub, section);
        yield return OtherBrandField(section);
        yield return Text("Model", "الموديل", "Model", section, required: true, maxLength: 100);
        yield return ManufacturingYear(section);
        yield return ColorField(sub, section);
        yield return OtherColorField(section);

        yield return Number("WorkingHours", "عدد ساعات التشغيل", "Working hours", section,
            required: true, min: 0);
        yield return Number("PowerValue", "القدرة", "Power", section, required: false, min: 0);

        yield return EnumSelect("PowerUnit", "وحدة القدرة", "Power unit", section,
            CarCatalog.PowerUnitNames, required: false);
        yield return Number("OperatingWeightTons", "الوزن التشغيلي (طن)", "Operating weight (tons)",
            section, required: false, min: 0);
        yield return Text("BucketCapacity", "سعة الجرافة / الحمولة", "Bucket capacity", section,
            required: false, maxLength: 100);
        yield return EnumSelect("DriveSystem", "نظام الحركة", "Drive system", section,
            CarCatalog.DriveSystemNames, required: false);
        yield return EnumSelect("FuelType", "نوع الوقود", "Fuel type", section,
            CarCatalog.EquipmentFuelTypeNames, required: false);

        yield return EnumSelect("Condition", "الحالة", "Condition", section,
            CarCatalog.ConditionNames, required: true);
        yield return EnumSelect("TechnicalCondition", "الحالة الفنية", "Technical condition", section,
            CarCatalog.TechnicalConditionNames, required: true);
        yield return EnumSelect("OriginCountry", "بلد المنشأ", "Origin country", section,
            CarCatalog.EquipmentOriginCountryNames, required: false);

        yield return Checkbox("FirstOwner", "أول مالك", "First owner", section);
        yield return EnumSelect("PreviousOwners", "عدد الملاك السابقين", "Previous owners", section,
            CarCatalog.EquipmentPreviousOwnersNames, required: false);

        yield return Checkbox("EngineOverhauled", "تم عمل عمرة للمحرك", "Engine overhauled", section);
        yield return Checkbox("AllMaintenanceAtDealer", "جميع الصيانات بالتوكيل",
            "All maintenance at dealer", section);

        yield return MultiSelectField("UsageFields", "مجال الاستخدام", "Usage fields", section,
            CarCatalog.UsageFieldNames);

        yield return Checkbox("CurrentlyWorking", "تعمل حاليًا", "Currently working", section);
        yield return Checkbox("ReadyToWork", "جاهزة للعمل", "Ready to work", section);
    }

    private static IEnumerable<FormFieldDto> LicenseFields(SubCategoryType subCategory)
    {
        var (label, names) = subCategory switch
        {
            SubCategoryType.Motorcycles => ("الرخصة", CarCatalog.MotorcycleLicenseStatusNames),
            SubCategoryType.HeavyEquipment => ("الرخصة", CarCatalog.EquipmentLicenseStatusNames),
            _ => ("حالة الرخصة", CarCatalog.LicenseStatusNames)
        };

        yield return EnumSelect("LicenseStatus", label, "License status", SectionLicense,
            names, required: true);

        if (subCategory is SubCategoryType.Private)
        {
            yield return DatePicker("LicenseIssueDate", "تاريخ إصدار الرخصة", "License issue date",
                    SectionLicense, required: false)
                .VisibleOnlyWhen("LicenseStatus", (int)LicenseStatus.Valid);
        }

        yield return DatePicker("LicenseExpiryDate", "تاريخ انتهاء الرخصة", "License expiry date",
                SectionLicense, required: false)
            .VisibleOnlyWhen("LicenseStatus", (int)LicenseStatus.Valid)
            .MandatoryWhen("LicenseStatus", (int)LicenseStatus.Valid);

        if (subCategory is SubCategoryType.Private)
        {
            yield return Checkbox("LicenseInOwnerName", "الرخصة باسم المالك الحالي",
                "License in owner name", SectionLicense);
            yield return Checkbox("LicenseTransferable", "الرخصة قابلة للنقل",
                "License transferable", SectionLicense);
        }

        if (subCategory is SubCategoryType.Taxi)
        {
            yield return EnumSelect("OperatingLicenseStatus", "رخصة التشغيل", "Operating license",
                SectionLicense, CarCatalog.OperatingLicenseStatusNames, required: false);

            yield return DatePicker("OperatingLicenseExpiryDate", "تاريخ انتهاء رخصة التشغيل",
                    "Operating license expiry date", SectionLicense, required: false)
                .VisibleOnlyWhen("OperatingLicenseStatus", (int)OperatingLicenseStatus.Valid)
                .MandatoryWhen("OperatingLicenseStatus", (int)OperatingLicenseStatus.Valid);
        }
    }

    private static IEnumerable<FormFieldDto> InsuranceFields() =>
    [
        Checkbox("IsInsured", "السيارة مؤمنة", "Is insured", SectionInsurance),
        EnumSelect("InsuranceType", "نوع التأمين", "Insurance type", SectionInsurance,
                CarCatalog.InsuranceTypeNames, required: false)
            .VisibleOnlyWhen("IsInsured", true)
            .MandatoryWhen("IsInsured", true),
        DatePicker("InsuranceExpiryDate", "تاريخ انتهاء التأمين", "Insurance expiry date",
                SectionInsurance, required: false)
            .VisibleOnlyWhen("IsInsured", true)
    ];

    private static IEnumerable<FormFieldDto> InspectionFields() =>
    [
        Checkbox("InspectionAllowed", "يسمح بالفحص والمعاينة", "Inspection allowed", SectionInspection),
        Text("InspectionLocation", "مكان المعاينة", "Inspection location", SectionInspection,
                required: false, maxLength: 300)
            .VisibleOnlyWhen("InspectionAllowed", true),
        Checkbox("ServiceCenterInspection", "يقبل الفحص في مركز خدمة",
                "Service centre inspection", SectionInspection)
            .VisibleOnlyWhen("InspectionAllowed", true)
    ];

    private static IEnumerable<FormFieldDto> FinanceFields() =>
    [
        Checkbox("InstallmentsAccepted", "يقبل التقسيط", "Installments accepted", SectionFinance),
        Number("DownPayment", "المقدم (جنيه)", "Down payment", SectionFinance, required: false, min: 0)
            .VisibleOnlyWhen("InstallmentsAccepted", true)
            .MandatoryWhen("InstallmentsAccepted", true),
        Number("InstallmentMonths", "عدد شهور التقسيط", "Installment months", SectionFinance,
                required: false, min: 1, max: 120)
            .VisibleOnlyWhen("InstallmentsAccepted", true)
            .MandatoryWhen("InstallmentsAccepted", true),
        Number("MonthlyInstallment", "القسط الشهري (جنيه)", "Monthly installment", SectionFinance,
                required: false, min: 0)
            .VisibleOnlyWhen("InstallmentsAccepted", true)
    ];

    private static IEnumerable<FormFieldDto> RentFields(SubCategoryType subCategory)
    {
        var isEquipment = subCategory is SubCategoryType.HeavyEquipment;
        var rentSystems = isEquipment ? CarCatalog.EquipmentRentSystemNames : CarCatalog.RentSystemNames;

        yield return MultiSelectField("RentSystems", "نظام الإيجار", "Rent system", SectionRent, rentSystems)
            .VisibleOnlyWhen("ListingType", (int)ListingType.Rent)
            .MandatoryWhen("ListingType", (int)ListingType.Rent);

        if (isEquipment)
            yield return RentPrice("HourlyPrice", "سعر الساعة", "Hourly price", RentSystem.Hourly);

        yield return RentPrice("DailyPrice", "سعر اليوم", "Daily price", RentSystem.Daily);
        yield return RentPrice("WeeklyPrice", "سعر الأسبوع", "Weekly price", RentSystem.Weekly);
        yield return RentPrice("MonthlyPrice", "سعر الشهر", "Monthly price", RentSystem.Monthly);

        yield return RentOnly(Number("MinimumRentPeriod", "أقل مدة للإيجار", "Minimum rent period",
            SectionRent, required: false, min: 1));

        if (subCategory is SubCategoryType.Private)
        {
            yield return RentOnly(Number("MaximumRentPeriod", "الحد الأقصى لمدة الإيجار",
                "Maximum rent period", SectionRent, required: false, min: 1));
        }

        var driverLabel = isEquipment ? "يوجد مشغل (سائق)" : "يوجد سائق";
        yield return RentOnly(Checkbox("DriverIncluded", driverLabel, "Driver included", SectionRent));
        yield return RentOnly(Checkbox("FuelIncluded", "يشمل الوقود", "Fuel included", SectionRent));

        if (subCategory is SubCategoryType.Private)
        {
            yield return RentOnly(Checkbox("HasRefundableDeposit", "يوجد تأمين مسترد",
                "Has refundable deposit", SectionRent));
        }

        yield return RentOnly(Number("DepositAmount", "قيمة التأمين (جنيه)", "Deposit amount",
            SectionRent, required: false, min: 0));
        yield return RentOnly(Number("MaximumDistanceKm", "الحد الأقصى للمسافة (كم)",
            "Maximum distance (km)", SectionRent, required: false, min: 0));

        if (subCategory is SubCategoryType.Motorcycles)
        {
            yield return RentOnly(Checkbox("HasHelmet", "يوجد خوذة", "Helmet included", SectionRent));
            yield return RentOnly(Checkbox("HasInsurance", "يوجد تأمين", "Insurance included", SectionRent));
        }
    }

    private static IEnumerable<FormFieldDto> AccidentFields(SubCategoryType subCategory)
    {
        yield return EnumSelect("DamageLevel", "درجة التلف", "Damage level", SectionAccident,
                CarCatalog.DamageLevelNames, required: false)
            .VisibleOnlyWhen("ListingType", (int)ListingType.Accident)
            .MandatoryWhen("ListingType", (int)ListingType.Accident);

        var movingLabel = subCategory switch
        {
            SubCategoryType.Private => "السيارة تتحرك",
            SubCategoryType.Motorcycles => "الموتوسيكل يتحرك",
            SubCategoryType.HeavyEquipment => "المعدة تتحرك",
            _ => "المركبة تتحرك"
        };

        yield return AccidentOnly(Checkbox("IsMoving", movingLabel, "Is moving", SectionAccident));
        yield return AccidentOnly(Checkbox("EngineWorks", "الموتور يعمل", "Engine works", SectionAccident));
        yield return AccidentOnly(Checkbox("GearboxWorks", "الفتيس سليم", "Gearbox works", SectionAccident));
        yield return AccidentOnly(Checkbox("ChassisIntact", "الشاسيه سليم", "Chassis intact", SectionAccident));

        if (subCategory is SubCategoryType.Private)
        {
            yield return AccidentOnly(Checkbox("AirbagsDeployed", "الوسائد الهوائية خرجت",
                "Airbags deployed", SectionAccident));
        }

        if (subCategory is SubCategoryType.HeavyEquipment)
        {
            yield return AccidentOnly(Checkbox("HydraulicSystemWorks", "النظام الهيدروليكي يعمل",
                "Hydraulic system works", SectionAccident));
        }

        yield return AccidentOnly(Checkbox("SellAsParts", "تباع كقطع غيار", "Sell as parts", SectionAccident));
        yield return AccidentOnly(TextArea("ConditionReport", "تقرير عن الحالة", "Condition report",
            SectionAccident, required: false, maxLength: 4000));
    }

    private static IEnumerable<FormFieldDto> ExchangeFields(SubCategoryType subCategory)
    {
        var targets = subCategory switch
        {
            SubCategoryType.Taxi => CarCatalog.TaxiExchangeTargetNames,
            SubCategoryType.Motorcycles => CarCatalog.MotorcycleExchangeTargetNames,
            SubCategoryType.HeavyEquipment => CarCatalog.EquipmentExchangeTargetNames,
            _ => CarCatalog.PrivateExchangeTargetNames
        };

        yield return EnumSelect("InterestedIn", "أرغب بالبدل مع", "Interested in", SectionExchange,
                targets, required: false)
            .VisibleOnlyWhen("ListingType", (int)ListingType.Exchange)
            .MandatoryWhen("ListingType", (int)ListingType.Exchange);

        yield return ExchangeOnly(Checkbox("DifferencePayment", "مع دفع فرق", "Difference payment",
            SectionExchange));
        yield return Number("DifferenceAmount", "قيمة الفرق التقريبية (جنيه)", "Difference amount",
                SectionExchange, required: false, min: 0)
            .VisibleOnlyWhen("DifferencePayment", true);

        if (subCategory is SubCategoryType.Private)
        {
            yield return ExchangeOnly(Checkbox("AcceptsHigherPriced", "يقبل سيارات أعلى سعرًا",
                "Accepts higher priced", SectionExchange));
            yield return ExchangeOnly(Checkbox("AcceptsLowerPriced", "يقبل سيارات أقل سعرًا",
                "Accepts lower priced", SectionExchange));
        }

        yield return ExchangeOnly(TextArea("ExchangeDetails", "تفاصيل البدل", "Exchange details",
            SectionExchange, required: false, maxLength: 4000));
    }

    private static FormFieldDto RentPrice(string name, string label, string labelEn, RentSystem system) =>
        RentOnly(Number(name, $"{label} (جنيه)", labelEn, SectionRent, required: false, min: 0))
            .MandatoryWhen("RentSystems", (int)system);

    private static FormFieldDto RentOnly(FormFieldDto field) =>
        field.VisibleOnlyWhen("ListingType", (int)ListingType.Rent);

    private static FormFieldDto AccidentOnly(FormFieldDto field) =>
        field.VisibleOnlyWhen("ListingType", (int)ListingType.Accident);

    private static FormFieldDto ExchangeOnly(FormFieldDto field) =>
        field.VisibleOnlyWhen("ListingType", (int)ListingType.Exchange);

    private static FormFieldDto BrandField(SubCategoryType subCategory, string section)
    {
        var field = LookupSelect("Brand", "الماركة", "Brand", section,
            BrandsKeyOf(subCategory), required: true);

        return field.AsSearchable(
            grouped: subCategory is SubCategoryType.Private or SubCategoryType.Taxi);
    }

    private static FormFieldDto OtherBrandField(string section) =>
        Text("OtherBrand", "اسم الماركة", "Brand name", section, required: false, maxLength: 100)
            .VisibleOnlyWhen("Brand", CarCatalog.OtherOptionValue)
            .MandatoryWhen("Brand", CarCatalog.OtherOptionValue);

    private static FormFieldDto TypedBrandField(string section) =>
        Text("Brand", "الماركة", "Brand", section, required: true, maxLength: 100,
            placeholder: CarCatalog.BrandPlaceholder);

    private static FormFieldDto TypedModelField(string section) =>
        Text("Model", "الموديل", "Model", section, required: true, maxLength: 100,
            placeholder: CarCatalog.ModelPlaceholder);

    private static FormFieldDto ColorField(SubCategoryType subCategory, string section) =>
        LookupSelect("Color", "اللون", "Color", section, ColorsKeyOf(subCategory), required: true);

    private static FormFieldDto OtherColorField(string section) =>
        Text("OtherColor", "اسم اللون", "Color name", section, required: false, maxLength: 50)
            .VisibleOnlyWhen("Color", CarCatalog.OtherOptionValue)
            .MandatoryWhen("Color", CarCatalog.OtherOptionValue);

    private static FormFieldDto ManufacturingYear(string section) =>
        Number("ManufacturingYear", "سنة الصنع", "Manufacturing year", section, required: true,
            min: CarCatalog.MinManufacturingYear, max: CarCatalog.MaxManufacturingYear);

    private static FormFieldDto Kilometers(string section, bool required = false) =>
        Number("Kilometers", "عدد الكيلومترات", "Kilometers", section, required, min: 0);

    private static FormFieldDto EngineCCField(SubCategoryType subCategory, string section, bool required)
    {
        var field = Number("EngineCC", "سعة المحرك (CC)", "Engine CC", section, required, min: 1);

        return EngineCapacitiesKeyOf(subCategory) is { } key ? SuggestedBy(field, key) : field;
    }

    private static FormFieldDto OtherTextField(
        string name, string label, string labelEn, string section, string parentField, int otherValue) =>
        Text(name, label, labelEn, section, required: false, maxLength: 100)
            .VisibleOnlyWhen(parentField, otherValue)
            .MandatoryWhen(parentField, otherValue);

    private static FormFieldDto ChangedPartsField(
        IReadOnlyDictionary<ChangedVehiclePart, string> names, string section) =>
        MultiSelectField("ChangedParts", "الأجزاء التي تم تغييرها", "Changed parts", section, names)
            .VisibleOnlyWhen("PartsChanged", true)
            .MandatoryWhen("PartsChanged", true);

    private static FormFieldDto MultiSelectField<TEnum>(
        string name, string label, string labelEn, string section,
        IReadOnlyDictionary<TEnum, string> names)
        where TEnum : struct, Enum
    {
        var field = EnumSelect(name, label, labelEn, section, names,
            required: false, type: FormFieldTypes.MultiSelect);
        field.MaxSelections = names.Count;
        return field;
    }

    private static FormFieldDto SuggestedBy(FormFieldDto field, string lookupKey)
    {
        field.OptionsSource = lookupKey;
        field.HelpText = "اختر من القائمة أو اكتب القيمة مباشرة.";
        return field;
    }

    private static FormFieldDto DetailedAddress() =>
        Text("DetailedAddress", "العنوان التفصيلي", "Detailed address", SectionLocation,
            required: true, maxLength: 300, placeholder: "الشارع، العلامة المميزة…");

    private static FormFieldDto FeatureIdsField(SubCategoryType subCategory)
    {
        var field = LookupSelect("FeatureIds", "المميزات", "Features", SectionFeatures,
            FeaturesKeyOf(subCategory), required: false, type: FormFieldTypes.MultiSelect);
        field.Grouped = true;
        field.MaxSelections = 100;
        return field;
    }

    private static string BrandsKeyOf(SubCategoryType subCategory) => subCategory switch
    {
        SubCategoryType.Motorcycles => AdFormLookupKeys.MotorcycleBrands,
        SubCategoryType.HeavyEquipment => AdFormLookupKeys.EquipmentBrands,
        _ => AdFormLookupKeys.CarBrands
    };

    private static string ColorsKeyOf(SubCategoryType subCategory) => subCategory switch
    {
        SubCategoryType.Taxi => AdFormLookupKeys.TaxiColors,
        SubCategoryType.Motorcycles => AdFormLookupKeys.MotorcycleColors,
        SubCategoryType.HeavyEquipment => AdFormLookupKeys.EquipmentColors,
        _ => AdFormLookupKeys.CarColors
    };

    private static string FeaturesKeyOf(SubCategoryType subCategory) => subCategory switch
    {
        SubCategoryType.Taxi => AdFormLookupKeys.TaxiFeatures,
        SubCategoryType.Motorcycles => AdFormLookupKeys.MotorcycleFeatures,
        SubCategoryType.HeavyEquipment => AdFormLookupKeys.EquipmentFeatures,
        _ => AdFormLookupKeys.PrivateCarFeatures
    };

    private static string? EngineCapacitiesKeyOf(SubCategoryType subCategory) => subCategory switch
    {
        SubCategoryType.Private => AdFormLookupKeys.CarEngineCapacities,
        SubCategoryType.Taxi => AdFormLookupKeys.TaxiEngineCapacities,
        SubCategoryType.Motorcycles => AdFormLookupKeys.MotorcycleEngineCapacities,
        _ => null
    };

    private static AdFormSchema Workshops()
    {
        var fields = new List<FormFieldDto>
        {
            Text("Name", "اسم الورشة", "Workshop name", SectionWorkshop, required: true, maxLength: 150),
            LookupSelect("WorkshopType", "نوع الورشة", "Workshop type", SectionWorkshop,
                AdFormLookupKeys.WorkshopTypes, required: true),
            Text("OtherWorkshopType", "حدد نوع الورشة", "Other workshop type", SectionWorkshop,
                    required: false, maxLength: 150)
                .VisibleOnlyWhen("WorkshopType", (int)WorkshopType.Other)
                .MandatoryWhen("WorkshopType", (int)WorkshopType.Other),
            Title("AdTitle"),
            Description("AdDescription", "وصف الإعلان")
        };

        fields.AddRange(LocationFields(centerLabel: "المدينة (المركز)", includeAddress: true));
        fields.Add(PhoneNumber());
        fields.Add(WhatsApp());
        fields.Add(Email());
        fields.Add(RequiredImages());

        return new AdFormSchema("workshops", WorkshopsSubmit,
            new[]
            {
                AdFormLookupKeys.WorkshopTypes,
                AdFormLookupKeys.Governorates,
                AdFormLookupKeys.Centers
            },
            Order(fields));
    }

    private static AdFormSchema Craftsmen()
    {
        var fields = new List<FormFieldDto>
        {
            Text("Name", "اسم الحرفي", "Craftsman name", SectionCraftsman, required: true, maxLength: 150),
            LookupSelect("Specialization", "التخصص", "Specialization", SectionCraftsman,
                AdFormLookupKeys.Specializations, required: true),
            Text("OtherSpecialization", "حدد التخصص", "Other specialization", SectionCraftsman,
                    required: false, maxLength: 150)
                .VisibleOnlyWhen("Specialization", (int)CraftsmanSpecialization.Other)
                .MandatoryWhen("Specialization", (int)CraftsmanSpecialization.Other),
            LookupSelect("ExperienceLevel", "سنوات الخبرة", "Experience level", SectionCraftsman,
                AdFormLookupKeys.ExperienceLevels, required: true),
            Title("AdTitle"),
            Description("AdDescription", "وصف الإعلان")
        };

        fields.AddRange(LocationFields(centerLabel: "المدينة (المركز)", includeAddress: true));
        fields.Add(PhoneNumber());
        fields.Add(WhatsApp());
        fields.Add(Email());
        fields.Add(RequiredImages());

        return new AdFormSchema("craftsmen", CraftsmenSubmit,
            new[]
            {
                AdFormLookupKeys.Specializations,
                AdFormLookupKeys.ExperienceLevels,
                AdFormLookupKeys.Governorates,
                AdFormLookupKeys.Centers
            },
            Order(fields));
    }

    private static AdFormSchema LostFound(PostType postType)
    {
        var postTypeField = EnumSelect("PostType", "نوع المنشور", "Post type", SectionItem,
            AdvertisementCatalog.PostTypeNames, required: true);
        postTypeField.DefaultValue = (int)postType;
        postTypeField.ReadOnly = true;
        postTypeField.HelpText = "محدد تلقائيًا حسب القسم الفرعي المختار.";

        var dateField = postType == PostType.Lost
            ? DatePicker("LostDate", "تاريخ الفقدان", "Lost date", SectionItem, required: true)
            : DatePicker("FoundDate", "تاريخ العثور", "Found date", SectionItem, required: true);

        var fields = new List<FormFieldDto>
        {
            postTypeField,
            Text("Name", "اسم صاحب الإعلان", "Contact name", SectionItem, required: true, maxLength: 150),
            Text("ItemName", "عنوان الإعلان", "Advertisement title", SectionItem,
                required: true, maxLength: 150, placeholder: "مثال: محفظة سوداء"),
            TextArea("Description", "الوصف", "Description", SectionItem, required: true, maxLength: 4000),
            dateField
        };

        fields.AddRange(LocationFields());
        fields.Add(PhoneNumber());
        fields.Add(Images());

        return new AdFormSchema("lostFound", LostFoundSubmit,
            new[] { AdFormLookupKeys.Governorates, AdFormLookupKeys.Centers },
            Order(fields));
    }

    private static IEnumerable<FormFieldDto> BusinessContactFields(bool googleMapsRequired = false) =>
    [
        Address(),
        BusinessGoogleMaps(googleMapsRequired),
        PhoneNumber("Phone"),
        PhoneNumber("WhatsApp", "رقم الواتساب", "WhatsApp number"),
        Email()
    ];

    private static IEnumerable<FormFieldDto> BusinessAdFields(
        string descriptionPlaceholder, bool imagesRequired = true) =>
    [
        Title(),
        BusinessDescription(descriptionPlaceholder),
        imagesRequired ? RequiredImages() : Images()
    ];

    private static FormFieldDto BusinessGoogleMaps(bool required = false)
    {
        var field = GoogleMapsUrl();
        field.Name = "GoogleMaps";
        field.Required = required;
        return field;
    }

    private static FormFieldDto BusinessDescription(string placeholder)
    {
        var field = TextArea("Description", "وصف الإعلان", "Description", SectionAd,
            required: true, maxLength: 4000);
        field.Placeholder = placeholder;
        field.HelpText = placeholder;
        return field;
    }

    private static AdFormSchema BusinessSchema(
        string module, CreateAdFormSubmitDto submit,
        IEnumerable<FormFieldDto> specificFields, string descriptionPlaceholder,
        params string[] lookups) =>
        BusinessSchema(module, submit, specificFields, descriptionPlaceholder,
            imagesRequired: true, googleMapsRequired: false, lookups);

    private static AdFormSchema BusinessSchema(
        string module, CreateAdFormSubmitDto submit,
        IEnumerable<FormFieldDto> specificFields, string descriptionPlaceholder,
        bool imagesRequired, bool googleMapsRequired, params string[] lookups)
    {
        var fields = new List<FormFieldDto>();
        fields.AddRange(specificFields);
        fields.AddRange(BusinessContactFields(googleMapsRequired));
        fields.AddRange(BusinessAdFields(descriptionPlaceholder, imagesRequired));

        return new AdFormSchema(module, submit, lookups, Order(fields));
    }

    private static AdFormSchema Factories() =>
        BusinessSchema("factories", FactoriesSubmit,
        [
            Text("FactoryName", "اسم المصنع", "Factory name", SectionFactory,
                required: true, maxLength: 150, placeholder: "مثال: مصنع الفيوم للأغذية"),
            LookupSelect("ProductionSpecialty", "تخصص الإنتاج", "Production specialty", SectionFactory,
                    AdFormLookupKeys.ProductionSpecialties, required: true)
                .AsSearchable(),
            Text("OtherSpecialty", "حدد تخصص الإنتاج", "Other specialty", SectionFactory,
                    required: false, maxLength: 150)
                .VisibleOnlyWhen("ProductionSpecialty", (int)ProductionSpecialty.Other)
                .MandatoryWhen("ProductionSpecialty", (int)ProductionSpecialty.Other)
        ],
        "اكتب وصفًا كاملًا للمصنع ومنتجاته.",
        imagesRequired: true,

        googleMapsRequired: true,
        AdFormLookupKeys.ProductionSpecialties);

    private static AdFormSchema Farms() =>
        BusinessSchema("farms", FarmsSubmit,
        [
            Text("FarmName", "اسم المزرعة", "Farm name", SectionFarm,
                required: true, maxLength: 150, placeholder: "مثال: مزرعة الواحة"),
            LookupSelect("FarmType", "نوع المزرعة", "Farm type", SectionFarm,
                    AdFormLookupKeys.FarmTypes, required: true)
                .AsSearchable(grouped: true),
            Text("OtherFarmType", "حدد نوع المزرعة", "Other farm type", SectionFarm,
                    required: false, maxLength: 150)
                .VisibleOnlyWhen("FarmType", (int)FarmType.Other)
                .MandatoryWhen("FarmType", (int)FarmType.Other),
            Number("AreaInFeddan", "المساحة بالفدان", "Area in feddan", SectionFarm,
                required: false, min: 0.01m),
            Text("AvailableQuantity", "الكمية المتاحة", "Available quantity", SectionFarm,
                required: false, maxLength: 150, placeholder: "مثال: 3 طن يوميًا"),
            LookupSelect("AvailabilitySeason", "موسم التوفر", "Availability season", SectionFarm,
                AdFormLookupKeys.Seasons, required: false),
            LookupSelect("FarmingMethod", "أسلوب الزراعة", "Farming method", SectionFarm,
                AdFormLookupKeys.FarmingMethods, required: false)
        ],
        BusinessPlaceholders.FarmDescription,
        imagesRequired: true,

        googleMapsRequired: true,
        AdFormLookupKeys.FarmTypes, AdFormLookupKeys.Seasons, AdFormLookupKeys.FarmingMethods);

    private static AdFormSchema Companies()
    {
        var logo = new FormFieldDto
        {
            Name = "Logo",
            Label = "شعار الشركة",
            LabelEn = "Company logo",
            Type = FormFieldTypes.Image,
            Required = false,
            Section = SectionImages,
            Multiple = false,
            MaxFiles = 1,
            MaxSizeMb = (int)(ImageConstants.MaxFileSizeBytes / (1024 * 1024)),
            AllowedExtensions = ImageConstants.AllowedExtensions
                .Select(extension => extension.TrimStart('.')).ToList(),
            HelpText = "اختياري — يُرفع منفصلًا عن صور المعرض."
        };

        var fields = new List<FormFieldDto>
        {
            Text("CompanyName", "اسم الشركة", "Company name", SectionCompany,
                required: true, maxLength: 150, placeholder: "مثال: شركة النيل للمقاولات"),
            LookupSelect("CompanyField", "مجال الشركة", "Company field", SectionCompany,
                    AdFormLookupKeys.CompanyFields, required: true)
                .AsSearchable(grouped: true),
            Text("OtherCompanyField", "حدد مجال الشركة", "Other company field", SectionCompany,
                    required: false, maxLength: 150)
                .VisibleOnlyWhen("CompanyField", (int)CompanyField.Other)
                .MandatoryWhen("CompanyField", (int)CompanyField.Other)
        };

        fields.AddRange(BusinessContactFields(googleMapsRequired: true));
        fields.Add(Website());
        fields.Add(logo);
        fields.AddRange(BusinessAdFields(BusinessPlaceholders.CompanyDescription));

        return new AdFormSchema("companies", CompaniesSubmit,
            new[] { AdFormLookupKeys.CompanyFields }, Order(fields));
    }

    private static AdFormSchema Suppliers() =>
        BusinessSchema("suppliers", SuppliersSubmit,
        [
            Text("SupplierName", "اسم المورد", "Supplier name", SectionSupplier,
                required: true, maxLength: 150, placeholder: "مثال: مؤسسة الأمل للتوريدات"),
            LookupSelect("SupplierType", "تخصص التوريد", "Supplier type", SectionSupplier,
                    AdFormLookupKeys.SupplierTypes, required: true)
                .AsSearchable(grouped: true),
            Text("OtherSupplierType", "حدد تخصص التوريد", "Other supplier type", SectionSupplier,
                    required: false, maxLength: 150)
                .VisibleOnlyWhen("SupplierType", (int)SupplierSpecialization.Other)
                .MandatoryWhen("SupplierType", (int)SupplierSpecialization.Other),
            Text("SuppliedProduct", "المنتج الموّرد", "Supplied product", SectionSupplier,
                required: true, maxLength: 150, placeholder: "مثال: حبيبات بولي إيثيلين"),
            BusinessDetails("SupplyDetails", "تفاصيل التوريد", "Supply details", SectionSupplier,
                BusinessPlaceholders.SupplierSupplyDetails)
        ],
        "اكتب وصفًا موجزًا للنشاط.",

        imagesRequired: false,
        googleMapsRequired: false,
        AdFormLookupKeys.SupplierTypes);

    private static AdFormSchema WholesaleTraders() =>
        BusinessSchema("wholesaleTraders", WholesaleTradersSubmit,
        [
            Text("TraderName", "اسم التاجر / المحل", "Trader name", SectionWholesale,
                required: true, maxLength: 150, placeholder: "مثال: تجارة الفيوم للجملة"),
            LookupSelect("TradeType", "تخصص التجارة", "Trade type", SectionWholesale,
                    AdFormLookupKeys.TradeTypes, required: true)
                .AsSearchable(),
            Text("OtherTradeType", "حدد تخصص التجارة", "Other trade type", SectionWholesale,
                    required: false, maxLength: 150)
                .VisibleOnlyWhen("TradeType", (int)WholesaleTradeType.Other)
                .MandatoryWhen("TradeType", (int)WholesaleTradeType.Other),
            Text("ProductsName", "أسماء المنتجات", "Products name", SectionWholesale,
                required: true, maxLength: 150, placeholder: "مثال: زيوت وسمن ومعلبات"),
            BusinessDetails("ProductDetails", "تفاصيل المنتجات", "Product details", SectionWholesale,
                BusinessPlaceholders.WholesaleProductDetails),
            LookupSelect("SaleType", "نوع البيع", "Sale type", SectionWholesale,
                AdFormLookupKeys.WholesaleSaleTypes, required: true)
        ],
        "اكتب وصفًا موجزًا للنشاط.",
        AdFormLookupKeys.TradeTypes, AdFormLookupKeys.WholesaleSaleTypes);

    private static AdFormSchema FruitMerchants()
    {
        var fields = new List<FormFieldDto>
        {
            Text("StallName", "اسم المحل", "Stall name", SectionFruitTrader,
                required: true, maxLength: 150, placeholder: "مثال: محل الخير للخضار"),
            Text("MerchantName", "اسم التاجر", "Merchant name", SectionFruitTrader,
                required: true, maxLength: 150),
            Text("ProductName", "اسم المنتج", "Product name", SectionFruitTrader,
                required: true, maxLength: 150, placeholder: "مثال: خضروات طازجة"),
            LookupSelect("SaleType", "نوع البيع", "Sale type", SectionFruitTrader,
                AdFormLookupKeys.MerchantSaleTypes, required: true),
            BusinessDetails("ProductDetails", "تفاصيل المنتجات", "Product details", SectionFruitTrader,
                BusinessPlaceholders.MerchantProductDetails)
        };

        fields.AddRange(FruitMerchantContactFields());

        fields.AddRange(BusinessAdFields("اكتب وصفًا موجزًا للنشاط.", imagesRequired: false));

        return new AdFormSchema("fruitVegetableMerchants", FruitMerchantsSubmit,
            new[] { AdFormLookupKeys.MerchantSaleTypes }, Order(fields));
    }

    private static IEnumerable<FormFieldDto> FruitMerchantContactFields() =>
    [
        Address(),
        BusinessGoogleMaps(),
        PhoneNumber("Phone"),
        PhoneNumber("WhatsApp", "رقم الواتساب", "WhatsApp number", required: false)
    ];

    private static FormFieldDto BusinessDetails(
        string name, string label, string labelEn, string section, string placeholder)
    {
        var field = TextArea(name, label, labelEn, section, required: true, maxLength: 4000);
        field.Placeholder = placeholder;
        field.HelpText = placeholder;
        return field;
    }

    private static FormFieldDto Website() =>
        new()
        {
            Name = "Website",
            Label = "الموقع الإلكتروني",
            LabelEn = "Website",
            Type = FormFieldTypes.Text,
            Required = false,
            Section = SectionContact,
            MaxLength = 1000,
            Placeholder = "https://example.com"
        };

    private static AdFormSchema JobRequests()
    {
        var fields = new List<FormFieldDto>
        {
            Text("ApplicantName", "اسم المتقدم", "Applicant name", SectionApplicant,
                required: true, maxLength: 150),
            PhoneNumber("Phone"),
            PhoneNumber("WhatsApp", "رقم الواتساب", "WhatsApp number"),

            LookupSelect("JobField", "مجال العمل", "Job field", SectionRequiredJob,
                    AdFormLookupKeys.JobFields, required: true)
                .AsSearchable(grouped: true),
            Text("OtherJobField", "حدد مجال العمل", "Other job field", SectionRequiredJob,
                    required: false, maxLength: 150)
                .VisibleOnlyWhen("JobField", (int)JobField.Other)
                .MandatoryWhen("JobField", (int)JobField.Other),
            LookupSelect("Experience", "الخبرة", "Experience", SectionRequiredJob,
                AdFormLookupKeys.JobExperienceLevels, required: true),
            LookupSelect("Education", "المؤهل الدراسي", "Education", SectionRequiredJob,
                AdFormLookupKeys.EducationLevels, required: true),
            JobSkills()
        };

        fields.AddRange(LocationFields(includeAddress: false, centerRequired: false));
        fields.Add(Address());

        fields.Add(ProfileImageUpload());
        fields.Add(CvUpload());
        fields.Add(IntroVideoUpload());

        fields.Add(JobRequestTitle());
        fields.Add(JobDescriptionField("Description", "وصف الإعلان", "Description",
            JobPlaceholders.RequestDescription));

        return new AdFormSchema("jobRequests", JobRequestsSubmit,
            new[]
            {
                AdFormLookupKeys.JobFields,
                AdFormLookupKeys.JobExperienceLevels,
                AdFormLookupKeys.EducationLevels,
                AdFormLookupKeys.Governorates,
                AdFormLookupKeys.Centers
            },
            Order(fields));
    }

    private static AdFormSchema JobOpportunities()
    {
        var fields = new List<FormFieldDto>
        {
            Text("EmployerName", "اسم صاحب العمل", "Employer name", SectionEmployer,
                required: true, maxLength: 150),
            PhoneNumber("Phone"),
            PhoneNumber("WhatsApp", "رقم الواتساب", "WhatsApp number"),

            Text("JobTitle", "المسمى الوظيفي", "Job title", SectionJob,
                required: true, maxLength: 150, placeholder: JobPlaceholders.JobTitle),
            LookupSelect("JobField", "مجال العمل", "Job field", SectionJob,
                    AdFormLookupKeys.JobFields, required: true)
                .AsSearchable(grouped: true),
            Text("OtherJobField", "حدد مجال العمل", "Other job field", SectionJob,
                    required: false, maxLength: 150)
                .VisibleOnlyWhen("JobField", (int)JobField.Other)
                .MandatoryWhen("JobField", (int)JobField.Other),
            LookupSelect("RequiredExperience", "الخبرة المطلوبة", "Required experience", SectionJob,
                AdFormLookupKeys.JobExperienceLevels, required: true),
            LookupSelect("WorkType", "نوع الدوام", "Work type", SectionJob,
                AdFormLookupKeys.WorkTypes, required: true),
            LookupSelect("SalaryType", "الراتب", "Salary type", SectionJob,
                AdFormLookupKeys.SalaryTypes, required: true),

            Number("Salary", "قيمة الراتب (جنيه)", "Salary", SectionJob,
                    required: false, min: 0.01m)
                .VisibleOnlyWhen("SalaryType", (int)SalaryType.Specified)
                .MandatoryWhen("SalaryType", (int)SalaryType.Specified)
        };

        fields.AddRange(LocationFields(includeAddress: false, centerRequired: false));
        fields.Add(Address());
        fields.Add(BusinessGoogleMaps());

        fields.Add(LogoUpload());
        fields.Add(Images());

        fields.Add(Title());
        fields.Add(JobDescriptionField("Description", "وصف الوظيفة", "Job description",
            JobPlaceholders.JobDescription));

        return new AdFormSchema("jobOpportunities", JobOpportunitiesSubmit,
            new[]
            {
                AdFormLookupKeys.JobFields,
                AdFormLookupKeys.JobExperienceLevels,
                AdFormLookupKeys.WorkTypes,
                AdFormLookupKeys.SalaryTypes,
                AdFormLookupKeys.Governorates,
                AdFormLookupKeys.Centers
            },
            Order(fields));
    }

    private static FormFieldDto JobSkills()
    {
        var field = TextArea("Skills", "المهارات", "Skills", SectionRequiredJob,
            required: true, maxLength: 1000);

        field.Placeholder = JobPlaceholders.Skills;
        field.HelpText = JobPlaceholders.Skills;
        return field;
    }

    private static FormFieldDto JobRequestTitle()
    {
        var field = Title();
        field.Placeholder = JobPlaceholders.RequestTitle;
        return field;
    }

    private static FormFieldDto JobDescriptionField(
        string name, string label, string labelEn, string placeholder)
    {
        var field = TextArea(name, label, labelEn, SectionAd, required: true, maxLength: 4000);
        field.Placeholder = placeholder;
        field.HelpText = placeholder;
        return field;
    }

    private static AdFormSchema AnimalSchema(
        string module, CreateAdFormSubmitDto submit, string section,
        string quantityLabel, string quantityLabelEn, string titlePlaceholder,
        string descriptionPlaceholder, IEnumerable<FormFieldDto> moduleFields,
        params string[] lookups)
    {
        var fields = new List<FormFieldDto>
        {
            Text("SellerName", "اسم البائع", "Seller name", section, required: true, maxLength: 150)
        };

        fields.AddRange(moduleFields);

        fields.Add(Number("Quantity", quantityLabel, quantityLabelEn, section, required: true, min: 1));
        fields.Add(Price());
        fields.Add(Negotiable());
        fields.Add(Address());
        fields.Add(BusinessGoogleMaps());
        fields.Add(PhoneNumber("Phone"));
        fields.Add(PhoneNumber("WhatsApp", "رقم الواتساب", "WhatsApp number", required: false));
        fields.Add(AnimalTitle(titlePlaceholder));
        fields.Add(BusinessDescription(descriptionPlaceholder));

        fields.Add(RequiredImages());

        return new AdFormSchema(module, submit, lookups, Order(fields));
    }

    private static FormFieldDto AnimalTitle(string placeholder)
    {
        var field = Title();
        field.Placeholder = placeholder;
        return field;
    }

    private static AdFormSchema Livestock() =>
        AnimalSchema("livestock", LivestockSubmit, SectionLivestock,
            "عدد الرؤوس", "Head count", "مثال: عجول تسمين بلدي للبيع",
            "اذكر كل التفاصيل المهمة: الحالة، الوزن، مصدر الحيوان وأي ملاحظات أخرى.",
            [
                LookupSelect("Breed", "السلالة", "Breed", SectionLivestock,
                    AdFormLookupKeys.LivestockBreeds, required: true)
                    .AsSearchable(),
                Text("OtherBreed", "حدد السلالة", "Other breed", SectionLivestock,
                        required: false, maxLength: 150)
                    .VisibleOnlyWhen("Breed", (int)LivestockBreed.Other)
                    .MandatoryWhen("Breed", (int)LivestockBreed.Other),
                LookupSelect("Purpose", "الغرض", "Purpose", SectionLivestock,
                    AdFormLookupKeys.LivestockPurposes, required: true),
                LookupSelect("Age", "العمر", "Age", SectionLivestock,
                    AdFormLookupKeys.LivestockAges, required: true),
                LookupSelect("Gender", "الجنس", "Gender", SectionLivestock,
                    AdFormLookupKeys.LivestockGenders, required: true),
                LookupSelect("HealthStatus", "الحالة الصحية", "Health status", SectionLivestock,
                    AdFormLookupKeys.LivestockHealthStatuses, required: true),
                LookupSelect("Vaccination", "حالة التحصين", "Vaccination status", SectionLivestock,
                    AdFormLookupKeys.LivestockVaccinations, required: false),
                LookupSelect("Production", "الإنتاج", "Production", SectionLivestock,
                    AdFormLookupKeys.LivestockProductions, required: false)
            ],
            AdFormLookupKeys.LivestockBreeds, AdFormLookupKeys.LivestockPurposes,
            AdFormLookupKeys.LivestockAges, AdFormLookupKeys.LivestockGenders,
            AdFormLookupKeys.LivestockHealthStatuses, AdFormLookupKeys.LivestockVaccinations,
            AdFormLookupKeys.LivestockProductions);

    private static AdFormSchema SheepGoat() =>
        AnimalSchema("sheepGoats", SheepGoatSubmit, SectionSheepGoat,
            "عدد الرؤوس", "Head count", "مثال: خرفان برقي للبيع",
            "اذكر كل التفاصيل المهمة: الحالة، الوزن، مصدر الحيوان وأي ملاحظات أخرى.",
            [
                LookupSelect("Breed", "السلالة", "Breed", SectionSheepGoat,
                    AdFormLookupKeys.SheepGoatBreeds, required: true)
                    .AsSearchable(),
                Text("OtherBreed", "حدد السلالة", "Other breed", SectionSheepGoat,
                        required: false, maxLength: 150)
                    .VisibleOnlyWhen("Breed", (int)SheepGoatBreed.Other)
                    .MandatoryWhen("Breed", (int)SheepGoatBreed.Other),
                LookupSelect("Purpose", "الغرض", "Purpose", SectionSheepGoat,
                    AdFormLookupKeys.SheepGoatPurposes, required: true),
                LookupSelect("Age", "العمر", "Age", SectionSheepGoat,
                    AdFormLookupKeys.SheepGoatAges, required: true),
                LookupSelect("Gender", "الجنس", "Gender", SectionSheepGoat,
                    AdFormLookupKeys.SheepGoatGenders, required: true),
                LookupSelect("HealthStatus", "الحالة الصحية", "Health status", SectionSheepGoat,
                    AdFormLookupKeys.SheepGoatHealthStatuses, required: true),
                LookupSelect("Vaccination", "حالة التحصين", "Vaccination status", SectionSheepGoat,
                    AdFormLookupKeys.SheepGoatVaccinations, required: false)
            ],
            AdFormLookupKeys.SheepGoatBreeds, AdFormLookupKeys.SheepGoatPurposes,
            AdFormLookupKeys.SheepGoatAges, AdFormLookupKeys.SheepGoatGenders,
            AdFormLookupKeys.SheepGoatHealthStatuses, AdFormLookupKeys.SheepGoatVaccinations);

    private static AdFormSchema Horse() =>
        AnimalSchema("horses", HorseSubmit, SectionHorse,
            "العدد", "Quantity", "مثال: حصان عربي أصيل للبيع",
            "اذكر كل التفاصيل المهمة: الحالة، الوزن، مصدر الحيوان وأي ملاحظات أخرى.",
            [
                LookupSelect("Breed", "السلالة", "Breed", SectionHorse,
                    AdFormLookupKeys.HorseBreeds, required: true)
                    .AsSearchable(),
                Text("OtherBreed", "حدد السلالة", "Other breed", SectionHorse,
                        required: false, maxLength: 150)
                    .VisibleOnlyWhen("Breed", (int)HorseBreed.Other)
                    .MandatoryWhen("Breed", (int)HorseBreed.Other),
                LookupSelect("Purpose", "الغرض", "Purpose", SectionHorse,
                    AdFormLookupKeys.HorsePurposes, required: true),
                LookupSelect("Age", "العمر", "Age", SectionHorse,
                    AdFormLookupKeys.HorseAges, required: true),
                LookupSelect("Gender", "الجنس", "Gender", SectionHorse,
                    AdFormLookupKeys.HorseGenders, required: true),
                LookupSelect("HealthStatus", "الحالة الصحية", "Health status", SectionHorse,
                    AdFormLookupKeys.HorseHealthStatuses, required: true),
                LookupSelect("TrainingLevel", "مستوى التدريب", "Training level", SectionHorse,
                    AdFormLookupKeys.HorseTrainingLevels, required: false),
                LookupSelect("Vaccination", "حالة التحصين", "Vaccination status", SectionHorse,
                    AdFormLookupKeys.HorseVaccinations, required: false)
            ],
            AdFormLookupKeys.HorseBreeds, AdFormLookupKeys.HorsePurposes,
            AdFormLookupKeys.HorseAges, AdFormLookupKeys.HorseGenders,
            AdFormLookupKeys.HorseHealthStatuses, AdFormLookupKeys.HorseTrainingLevels,
            AdFormLookupKeys.HorseVaccinations);

    private static AdFormSchema Camel() =>
        AnimalSchema("camels", CamelSubmit, SectionCamel,
            "عدد الرؤوس", "Head count", "مثال: نوق مغربي للبيع",
            "اذكر كل التفاصيل المهمة: الحالة، الوزن، مصدر الحيوان وأي ملاحظات أخرى.",
            [
                LookupSelect("Breed", "السلالة", "Breed", SectionCamel,
                    AdFormLookupKeys.CamelBreeds, required: true)
                    .AsSearchable(),
                Text("OtherBreed", "حدد السلالة", "Other breed", SectionCamel,
                        required: false, maxLength: 150)
                    .VisibleOnlyWhen("Breed", (int)CamelBreed.Other)
                    .MandatoryWhen("Breed", (int)CamelBreed.Other),
                LookupSelect("Purpose", "الغرض", "Purpose", SectionCamel,
                    AdFormLookupKeys.CamelPurposes, required: true),
                LookupSelect("Age", "العمر", "Age", SectionCamel,
                    AdFormLookupKeys.CamelAges, required: true),
                LookupSelect("Gender", "الجنس", "Gender", SectionCamel,
                    AdFormLookupKeys.CamelGenders, required: true),
                LookupSelect("HealthStatus", "الحالة الصحية", "Health status", SectionCamel,
                    AdFormLookupKeys.CamelHealthStatuses, required: true),
                LookupSelect("Vaccination", "حالة التحصين", "Vaccination status", SectionCamel,
                    AdFormLookupKeys.CamelVaccinations, required: false)
            ],
            AdFormLookupKeys.CamelBreeds, AdFormLookupKeys.CamelPurposes,
            AdFormLookupKeys.CamelAges, AdFormLookupKeys.CamelGenders,
            AdFormLookupKeys.CamelHealthStatuses, AdFormLookupKeys.CamelVaccinations);

    private static AdFormSchema Bird() =>
        AnimalSchema("birds", BirdSubmit, SectionBird,
            "العدد", "Quantity", "مثال: حمام زاجل للبيع",
            "اذكر كل التفاصيل المهمة: الحالة، الوزن، مصدر الحيوان وأي ملاحظات أخرى.",
            [
                LookupSelect("AnimalType", "النوع", "Type", SectionBird,
                    AdFormLookupKeys.BirdTypes, required: true)
                    .AsSearchable(),
                Text("OtherType", "حدد النوع", "Other type", SectionBird,
                        required: false, maxLength: 150)
                    .VisibleOnlyWhen("AnimalType", (int)BirdType.Other)
                    .MandatoryWhen("AnimalType", (int)BirdType.Other),
                LookupSelect("Purpose", "الغرض", "Purpose", SectionBird,
                    AdFormLookupKeys.BirdPurposes, required: true),
                LookupSelect("Age", "العمر", "Age", SectionBird,
                    AdFormLookupKeys.BirdAges, required: true),
                LookupSelect("Gender", "الجنس", "Gender", SectionBird,
                    AdFormLookupKeys.BirdGenders, required: true),
                LookupSelect("HealthStatus", "الحالة الصحية", "Health status", SectionBird,
                    AdFormLookupKeys.BirdHealthStatuses, required: true),
                LookupSelect("Vaccination", "حالة التحصين", "Vaccination status", SectionBird,
                    AdFormLookupKeys.BirdVaccinations, required: false)
            ],
            AdFormLookupKeys.BirdTypes, AdFormLookupKeys.BirdPurposes, AdFormLookupKeys.BirdAges,
            AdFormLookupKeys.BirdGenders, AdFormLookupKeys.BirdHealthStatuses,
            AdFormLookupKeys.BirdVaccinations);

    private static AdFormSchema Pet() =>
        AnimalSchema("pets", PetSubmit, SectionPet,
            "العدد", "Quantity", "مثال: قط شيرازي صغير للبيع",
            "اذكر كل التفاصيل المهمة: الحالة، الوزن، مصدر الحيوان وأي ملاحظات أخرى.",
            [
                LookupSelect("Breed", "السلالة", "Breed", SectionPet,
                    AdFormLookupKeys.PetBreeds, required: true)
                    .AsSearchable(),
                Text("OtherBreed", "حدد السلالة", "Other breed", SectionPet,
                        required: false, maxLength: 150)
                    .VisibleOnlyWhen("Breed", (int)PetBreed.Other)
                    .MandatoryWhen("Breed", (int)PetBreed.Other),
                LookupSelect("Purpose", "الغرض", "Purpose", SectionPet,
                    AdFormLookupKeys.PetPurposes, required: true),
                LookupSelect("Age", "العمر", "Age", SectionPet,
                    AdFormLookupKeys.PetAges, required: true),
                LookupSelect("Gender", "الجنس", "Gender", SectionPet,
                    AdFormLookupKeys.PetGenders, required: true),
                LookupSelect("HealthStatus", "الحالة الصحية", "Health status", SectionPet,
                    AdFormLookupKeys.PetHealthStatuses, required: true),
                LookupSelect("TrainingLevel", "مستوى التدريب", "Training level", SectionPet,
                    AdFormLookupKeys.PetTrainingLevels, required: false),
                LookupSelect("Vaccination", "حالة التحصين", "Vaccination status", SectionPet,
                    AdFormLookupKeys.PetVaccinations, required: false)
            ],
            AdFormLookupKeys.PetBreeds, AdFormLookupKeys.PetPurposes, AdFormLookupKeys.PetAges,
            AdFormLookupKeys.PetGenders, AdFormLookupKeys.PetHealthStatuses,
            AdFormLookupKeys.PetTrainingLevels, AdFormLookupKeys.PetVaccinations);

    private static AdFormSchema Fish() =>
        AnimalSchema("fish", FishSubmit, SectionFish,
            "الكمية (كجم / عدد)", "Quantity", "مثال: زريعة بلطي للبيع",
            "اذكر كل التفاصيل المهمة: الحالة، الوزن، مصدر الحيوان وأي ملاحظات أخرى.",
            [
                LookupSelect("AnimalType", "النوع", "Type", SectionFish,
                    AdFormLookupKeys.FishTypes, required: true)
                    .AsSearchable(),
                Text("OtherType", "حدد النوع", "Other type", SectionFish,
                        required: false, maxLength: 150)
                    .VisibleOnlyWhen("AnimalType", (int)FishType.Other)
                    .MandatoryWhen("AnimalType", (int)FishType.Other),
                LookupSelect("Purpose", "الغرض", "Purpose", SectionFish,
                    AdFormLookupKeys.FishPurposes, required: true),
                LookupSelect("Age", "العمر", "Age", SectionFish,
                    AdFormLookupKeys.FishAges, required: true),
                LookupSelect("HealthStatus", "الحالة الصحية", "Health status", SectionFish,
                    AdFormLookupKeys.FishHealthStatuses, required: true)
            ],
            AdFormLookupKeys.FishTypes, AdFormLookupKeys.FishPurposes, AdFormLookupKeys.FishAges,
            AdFormLookupKeys.FishHealthStatuses);

    private static AdFormSchema Bee() =>
        AnimalSchema("bees", BeeSubmit, SectionBee,
            "عدد الخلايا", "Hive count", "مثال: خلايا نحل كرنيولي للبيع",
            "اذكر كل التفاصيل المهمة: الحالة، الوزن، مصدر الحيوان وأي ملاحظات أخرى.",
            [
                LookupSelect("AnimalType", "النوع", "Type", SectionBee,
                    AdFormLookupKeys.BeeTypes, required: true)
                    .AsSearchable(),
                Text("OtherType", "حدد النوع", "Other type", SectionBee,
                        required: false, maxLength: 150)
                    .VisibleOnlyWhen("AnimalType", (int)BeeType.Other)
                    .MandatoryWhen("AnimalType", (int)BeeType.Other),
                LookupSelect("Purpose", "الغرض", "Purpose", SectionBee,
                    AdFormLookupKeys.BeePurposes, required: true),
                LookupSelect("HealthStatus", "الحالة الصحية", "Health status", SectionBee,
                    AdFormLookupKeys.BeeHealthStatuses, required: true),
                LookupSelect("Production", "الإنتاج", "Production", SectionBee,
                    AdFormLookupKeys.BeeProductions, required: false)
            ],
            AdFormLookupKeys.BeeTypes, AdFormLookupKeys.BeePurposes,
            AdFormLookupKeys.BeeHealthStatuses, AdFormLookupKeys.BeeProductions);

    private static AdFormSchema OtherAnimal() =>
        AnimalSchema("otherAnimals", OtherAnimalSubmit, SectionOtherAnimal,
            "العدد", "Quantity", "مثال: حمار بلدي للبيع",
            "اذكر كل التفاصيل المهمة: الحالة، الوزن، مصدر الحيوان وأي ملاحظات أخرى.",
            [
                LookupSelect("AnimalType", "النوع", "Type", SectionOtherAnimal,
                    AdFormLookupKeys.OtherAnimalTypes, required: true)
                    .AsSearchable(),
                Text("OtherType", "حدد النوع", "Other type", SectionOtherAnimal,
                        required: false, maxLength: 150)
                    .VisibleOnlyWhen("AnimalType", (int)OtherAnimalType.Other)
                    .MandatoryWhen("AnimalType", (int)OtherAnimalType.Other),
                LookupSelect("Purpose", "الغرض", "Purpose", SectionOtherAnimal,
                    AdFormLookupKeys.OtherAnimalPurposes, required: true),
                LookupSelect("Age", "العمر", "Age", SectionOtherAnimal,
                    AdFormLookupKeys.OtherAnimalAges, required: true),
                LookupSelect("Gender", "الجنس", "Gender", SectionOtherAnimal,
                    AdFormLookupKeys.OtherAnimalGenders, required: true),
                LookupSelect("HealthStatus", "الحالة الصحية", "Health status", SectionOtherAnimal,
                    AdFormLookupKeys.OtherAnimalHealthStatuses, required: true),
                LookupSelect("Vaccination", "حالة التحصين", "Vaccination status", SectionOtherAnimal,
                    AdFormLookupKeys.OtherAnimalVaccinations, required: false)
            ],
            AdFormLookupKeys.OtherAnimalTypes, AdFormLookupKeys.OtherAnimalPurposes,
            AdFormLookupKeys.OtherAnimalAges, AdFormLookupKeys.OtherAnimalGenders,
            AdFormLookupKeys.OtherAnimalHealthStatuses, AdFormLookupKeys.OtherAnimalVaccinations);

    private static AdFormSchema AntiqueSchema(
        string module, CreateAdFormSubmitDto submit,
        string sellerNameLabel, string sellerNameLabelEn,
        string titlePlaceholder, string descriptionPlaceholder,
        IEnumerable<FormFieldDto> moduleFields,
        params string[] lookups)
    {
        var fields = new List<FormFieldDto>
        {
            Text("SellerName", sellerNameLabel, sellerNameLabelEn, SectionSeller,
                required: true, maxLength: 150),
            PhoneNumber("Phone"),
            PhoneNumber("WhatsApp", "رقم الواتساب", "WhatsApp number", required: false)
        };

        fields.AddRange(moduleFields);

        fields.Add(Price());
        fields.Add(Negotiable());

        fields.AddRange(LocationFields(includeAddress: false));
        fields.Add(Address());
        fields.Add(BusinessGoogleMaps());

        fields.Add(AntiqueTitle(titlePlaceholder));
        fields.Add(BusinessDescription(descriptionPlaceholder));

        fields.Add(RequiredImages());
        fields.Add(VideoUpload());

        return new AdFormSchema(module, submit, lookups, Order(fields));
    }

    private static FormFieldDto AntiqueTitle(string placeholder)
    {
        var field = Title();
        field.Placeholder = placeholder;
        return field;
    }

    private static FormFieldDto AntiqueMeasurement(string name, string label, string labelEn) =>
        Number(name, label, labelEn, SectionDimensions, required: false, min: 0.01m);

    private static FormFieldDto AntiqueYear(string name, string label, string labelEn, string section) =>
        Number(name, label, labelEn, section, required: false,
            min: AntiqueCatalog.MinYear, max: AntiqueCatalog.MaxYear);

    private static AdFormSchema DecorAntiques() =>
        AntiqueSchema("decorAntiques", DecorAntiqueSubmit,
            "اسم البائع", "Seller name",
            AntiquePlaceholders.DecorAntiqueTitle, AntiquePlaceholders.DecorAntiqueDescription,
            [
                Text("ItemName", "اسم القطعة", "Item name", SectionDecorAntique,
                    required: true, maxLength: 150),
                LookupSelect("ItemType", "نوع القطعة", "Item type", SectionDecorAntique,
                        AdFormLookupKeys.DecorAntiqueItemTypes, required: true)
                    .AsSearchable(),
                Text("OtherItemType", "حدد نوع القطعة", "Other item type", SectionDecorAntique,
                        required: false, maxLength: 150)
                    .VisibleOnlyWhen("ItemType", (int)DecorAntiqueItemType.Other)
                    .MandatoryWhen("ItemType", (int)DecorAntiqueItemType.Other),
                LookupSelect("Material", "الخامة", "Material", SectionDecorAntique,
                        AdFormLookupKeys.DecorAntiqueMaterials, required: true)
                    .AsSearchable(),
                Text("OtherMaterial", "حدد الخامة", "Other material", SectionDecorAntique,
                        required: false, maxLength: 150)
                    .VisibleOnlyWhen("Material", (int)DecorAntiqueMaterial.Other)
                    .MandatoryWhen("Material", (int)DecorAntiqueMaterial.Other),
                LookupSelect("Condition", "الحالة", "Condition", SectionDecorAntique,
                    AdFormLookupKeys.DecorAntiqueConditions, required: true),
                LookupSelect("Originality", "الأصالة", "Originality", SectionDecorAntique,
                    AdFormLookupKeys.DecorAntiqueOriginalities, required: true),

                AntiqueMeasurement("Length", "الطول (سم)", "Length (cm)"),
                AntiqueMeasurement("Width", "العرض (سم)", "Width (cm)"),
                AntiqueMeasurement("Height", "الارتفاع (سم)", "Height (cm)"),
                AntiqueMeasurement("Weight", "الوزن (كجم)", "Weight (kg)")
            ],
            AdFormLookupKeys.DecorAntiqueItemTypes, AdFormLookupKeys.DecorAntiqueMaterials,
            AdFormLookupKeys.DecorAntiqueConditions, AdFormLookupKeys.DecorAntiqueOriginalities,
            AdFormLookupKeys.Governorates, AdFormLookupKeys.Centers);

    private static AdFormSchema Antiques() =>
        AntiqueSchema("antiques", AntiqueSubmit,
            "اسم البائع", "Seller name",
            AntiquePlaceholders.AntiqueTitle, AntiquePlaceholders.AntiqueDescription,
            [
                Text("AntiqueName", "اسم الأنتيك", "Antique name", SectionAntique,
                    required: true, maxLength: 150),
                LookupSelect("AntiqueType", "نوع الأنتيك", "Antique type", SectionAntique,
                        AdFormLookupKeys.AntiqueTypes, required: true)
                    .AsSearchable(),
                Text("OtherType", "حدد النوع", "Other type", SectionAntique,
                        required: false, maxLength: 150)
                    .VisibleOnlyWhen("AntiqueType", (int)AntiqueType.Other)
                    .MandatoryWhen("AntiqueType", (int)AntiqueType.Other),
                AntiqueYear("ManufactureYear", "سنة الصنع", "Manufacture year", SectionAntique),
                Text("CountryOfOrigin", "بلد المنشأ", "Country of origin", SectionAntique,
                    required: false, maxLength: 100, placeholder: AntiquePlaceholders.Country),
                Text("Manufacturer", "الشركة المصنعة", "Manufacturer", SectionAntique,
                    required: false, maxLength: 150),
                LookupSelect("Material", "الخامة", "Material", SectionAntique,
                    AdFormLookupKeys.AntiqueMaterials, required: true),
                Text("OtherMaterial", "حدد الخامة", "Other material", SectionAntique,
                        required: false, maxLength: 150)
                    .VisibleOnlyWhen("Material", (int)AntiqueMaterial.Other)
                    .MandatoryWhen("Material", (int)AntiqueMaterial.Other),
                LookupSelect("Condition", "الحالة", "Condition", SectionAntique,
                    AdFormLookupKeys.AntiqueConditions, required: true),
                LookupSelect("WorkingStatus", "هل يعمل؟", "Working status", SectionAntique,
                    AdFormLookupKeys.AntiqueWorkingStatuses, required: true),
                LookupSelect("Originality", "الأصالة", "Originality", SectionAntique,
                    AdFormLookupKeys.AntiqueOriginalities, required: true)
            ],
            AdFormLookupKeys.AntiqueTypes, AdFormLookupKeys.AntiqueMaterials,
            AdFormLookupKeys.AntiqueConditions, AdFormLookupKeys.AntiqueWorkingStatuses,
            AdFormLookupKeys.AntiqueOriginalities,
            AdFormLookupKeys.Governorates, AdFormLookupKeys.Centers);

    private static AdFormSchema Paintings() =>
        AntiqueSchema("paintings", PaintingSubmit,
            "اسم الفنان / البائع", "Artist / seller name",
            AntiquePlaceholders.PaintingTitle, AntiquePlaceholders.PaintingDescription,
            [
                Text("PaintingName", "اسم اللوحة", "Painting name", SectionPainting,
                    required: true, maxLength: 150),
                LookupSelect("PaintingType", "نوع اللوحة", "Painting type", SectionPainting,
                        AdFormLookupKeys.PaintingTypes, required: true)
                    .AsSearchable(),
                Text("OtherType", "حدد النوع", "Other type", SectionPainting,
                        required: false, maxLength: 150)
                    .VisibleOnlyWhen("PaintingType", (int)PaintingType.Other)
                    .MandatoryWhen("PaintingType", (int)PaintingType.Other),
                Text("ArtistName", "اسم الفنان", "Artist name", SectionPainting,
                    required: true, maxLength: 150),
                AntiqueYear("ExecutionYear", "سنة التنفيذ", "Execution year", SectionPainting),
                LookupSelect("Material", "الخامة", "Material", SectionPainting,
                    AdFormLookupKeys.PaintingMaterials, required: true),
                Text("OtherMaterial", "حدد الخامة", "Other material", SectionPainting,
                        required: false, maxLength: 150)
                    .VisibleOnlyWhen("Material", (int)PaintingMaterial.Other)
                    .MandatoryWhen("Material", (int)PaintingMaterial.Other),
                Checkbox("Framed", "مؤطرة", "Framed", SectionPainting),
                LookupSelect("Originality", "الأصالة", "Originality", SectionPainting,
                    AdFormLookupKeys.PaintingOriginalities, required: true),
                Checkbox("SignedByArtist", "موقّعة من الفنان", "Signed by artist", SectionPainting),

                AntiqueMeasurement("Width", "العرض (سم)", "Width (cm)"),
                AntiqueMeasurement("Height", "الارتفاع (سم)", "Height (cm)")
            ],
            AdFormLookupKeys.PaintingTypes, AdFormLookupKeys.PaintingMaterials,
            AdFormLookupKeys.PaintingOriginalities,
            AdFormLookupKeys.Governorates, AdFormLookupKeys.Centers);

    private static AdFormSchema Handmade() =>
        AntiqueSchema("handmade", HandmadeSubmit,
            "اسم البائع", "Seller name",
            AntiquePlaceholders.HandmadeTitle, AntiquePlaceholders.HandmadeDescription,
            [
                Text("ProductName", "اسم المنتج", "Product name", SectionHandmade,
                    required: true, maxLength: 150),
                LookupSelect("HandmadeType", "نوع العمل اليدوي", "Handmade type", SectionHandmade,
                        AdFormLookupKeys.HandmadeTypes, required: true)
                    .AsSearchable(),
                Text("OtherType", "حدد النوع", "Other type", SectionHandmade,
                        required: false, maxLength: 150)
                    .VisibleOnlyWhen("HandmadeType", (int)HandmadeType.Other)
                    .MandatoryWhen("HandmadeType", (int)HandmadeType.Other),
                Text("Material", "الخامة", "Material", SectionHandmade,
                    required: true, maxLength: 250, placeholder: AntiquePlaceholders.HandmadeMaterial),
                Checkbox("IsFullyHandmade", "هاند ميد بالكامل", "Fully handmade", SectionHandmade),
                Checkbox("CustomOrder", "يقبل الطلب حسب المواصفات", "Custom order", SectionHandmade),
                Text("ProductionTime", "مدة التنفيذ", "Production time", SectionHandmade,
                    required: false, maxLength: 100, placeholder: AntiquePlaceholders.HandmadeProductionTime),
                Text("Size", "المقاس", "Size", SectionHandmade,
                    required: false, maxLength: 100, placeholder: AntiquePlaceholders.HandmadeSize),
                HandmadeColorsField(),
                Text("OtherColor", "حدد اللون", "Other color", SectionHandmade,
                        required: false, maxLength: 150)
                    .VisibleOnlyWhen("Colors", (int)HandmadeColor.Other)
                    .MandatoryWhen("Colors", (int)HandmadeColor.Other)
            ],
            AdFormLookupKeys.HandmadeTypes, AdFormLookupKeys.HandmadeColors,
            AdFormLookupKeys.Governorates, AdFormLookupKeys.Centers);

    private static FormFieldDto HandmadeColorsField()
    {
        var field = LookupSelect("Colors", "الألوان", "Colors", SectionHandmade,
            AdFormLookupKeys.HandmadeColors, required: true, type: FormFieldTypes.MultiSelect);

        field.MaxSelections = HandmadeCatalog.Colors.Count;
        field.HelpText = "يمكن اختيار أكثر من لون.";
        return field;
    }

    private static AdFormSchema CoinsStamps() =>
        AntiqueSchema("coinsStamps", CoinStampSubmit,
            "اسم البائع", "Seller name",
            AntiquePlaceholders.CoinStampTitle, AntiquePlaceholders.CoinStampDescription,
            [
                Text("ItemName", "اسم القطعة", "Item name", SectionCoinStamp,
                    required: true, maxLength: 150),
                LookupSelect("ItemType", "نوع القطعة", "Item type", SectionCoinStamp,
                        AdFormLookupKeys.CoinStampItemTypes, required: true)
                    .AsSearchable(),
                Text("OtherType", "حدد النوع", "Other type", SectionCoinStamp,
                        required: false, maxLength: 150)
                    .VisibleOnlyWhen("ItemType", (int)CoinStampItemType.Other)
                    .MandatoryWhen("ItemType", (int)CoinStampItemType.Other),
                Text("Country", "بلد الإصدار", "Country", SectionCoinStamp,
                    required: false, maxLength: 100, placeholder: AntiquePlaceholders.Country),
                AntiqueYear("IssueYear", "سنة الإصدار", "Issue year", SectionCoinStamp),
                Text("Denomination", "الفئة", "Denomination", SectionCoinStamp,
                    required: false, maxLength: 100, placeholder: AntiquePlaceholders.Denomination),
                LookupSelect("Metal", "المعدن", "Metal", SectionCoinStamp,
                    AdFormLookupKeys.CoinStampMetals, required: true),
                Text("OtherMetal", "حدد المعدن", "Other metal", SectionCoinStamp,
                        required: false, maxLength: 150)
                    .VisibleOnlyWhen("Metal", (int)CoinStampMetal.Other)
                    .MandatoryWhen("Metal", (int)CoinStampMetal.Other),
                LookupSelect("Condition", "الحالة", "Condition", SectionCoinStamp,
                    AdFormLookupKeys.CoinStampConditions, required: true),
                Checkbox("IsOriginal", "أصلية", "Original", SectionCoinStamp),
                Checkbox("IsRare", "نادرة", "Rare", SectionCoinStamp),
                Checkbox("HasCertificate", "لديها شهادة توثيق", "Has certificate", SectionCoinStamp)
            ],
            AdFormLookupKeys.CoinStampItemTypes, AdFormLookupKeys.CoinStampMetals,
            AdFormLookupKeys.CoinStampConditions,
            AdFormLookupKeys.Governorates, AdFormLookupKeys.Centers);

    private static AdFormSchema ClothingSchema(
        string module, CreateAdFormSubmitDto submit, string section,
        int sellingMethodStore, int sellingMethodStoreAndOnline,
        string sellingMethodsKey, string titlePlaceholder,
        IEnumerable<FormFieldDto> moduleFields, params string[] lookups)
    {
        var fields = new List<FormFieldDto>
        {
            Text("StoreName", "اسم المحل", "Store name", SectionStore,
                required: true, maxLength: 150, placeholder: "مثال: محل الأناقة للملابس"),
            LookupSelect("SellingMethod", "طريقة البيع", "Selling method", SectionStore,
                sellingMethodsKey, required: true)
        };

        fields.AddRange(moduleFields);

        fields.Add(Price());
        fields.Add(Checkbox("DeliveryAvailable", "التوصيل متاح", "Delivery available", SectionAd));

        fields.AddRange(LocationFields());
        fields.Add(Address());
        fields.Add(ClothingGoogleMaps(sellingMethodStore, sellingMethodStoreAndOnline));

        fields.Add(PhoneNumber("Phone"));
        fields.Add(PhoneNumber("WhatsApp", "رقم الواتساب", "WhatsApp number"));
        fields.Add(Email());

        fields.Add(RequiredImages());
        fields.Add(VideoUpload());

        fields.Add(ClothingTitle(titlePlaceholder));
        fields.Add(BusinessDescription(
            "اذكر كل التفاصيل المهمة: الخامة، المقاسات المتاحة، الألوان وأي ملاحظات أخرى."));

        return new AdFormSchema(module, submit, lookups, Order(fields));
    }

    private static FormFieldDto ClothingGoogleMaps(int store, int storeAndOnline)
    {
        var field = BusinessGoogleMaps();
        field.HelpText = "يظهر فقط عند اختيار البيع من محل أو محل + أونلاين.";
        return field.VisibleOnlyWhen("SellingMethod", store, storeAndOnline);
    }

    private static FormFieldDto ClothingTitle(string placeholder)
    {
        var field = Title();
        field.Placeholder = placeholder;
        return field;
    }

    private static FormFieldDto ClothingMultiSelect(
        string name, string label, string labelEn, string section, string optionsSource, int maxSelections)
    {
        var field = LookupSelect(name, label, labelEn, section, optionsSource,
            required: true, type: FormFieldTypes.MultiSelect);

        field.MaxSelections = maxSelections;
        return field;
    }

    private static AdFormSchema MenClothing() =>
        ClothingSchema("menClothing", MenClothingSubmit, SectionMenClothing,
            (int)MenClothingSellingMethod.Store, (int)MenClothingSellingMethod.StoreAndOnline,
            AdFormLookupKeys.MenClothingSellingMethods,
            "مثال: تيشيرتات رجالي قطن مستورد",
            [
                LookupSelect("ClothingType", "نوع الملابس", "Clothing type", SectionMenClothing,
                        AdFormLookupKeys.MenClothingTypes, required: true)
                    .AsSearchable(),
                Text("OtherClothingType", "حدد نوع الملابس", "Other clothing type", SectionMenClothing,
                        required: false, maxLength: 150)
                    .VisibleOnlyWhen("ClothingType", (int)MenClothingType.Other)
                    .MandatoryWhen("ClothingType", (int)MenClothingType.Other),
                LookupSelect("Brand", "الماركة", "Brand", SectionMenClothing,
                    AdFormLookupKeys.MenClothingBrands, required: true),
                Text("OtherBrand", "حدد الماركة", "Other brand", SectionMenClothing,
                        required: false, maxLength: 150)
                    .VisibleOnlyWhen("Brand", (int)MenClothingBrand.Other)
                    .MandatoryWhen("Brand", (int)MenClothingBrand.Other),
                ClothingMultiSelect("Sizes", "المقاسات", "Sizes", SectionMenClothing,
                    AdFormLookupKeys.MenClothingSizes, MenClothingCatalog.Sizes.Count),
                ClothingMultiSelect("Colors", "الألوان", "Colors", SectionMenClothing,
                    AdFormLookupKeys.MenClothingColors, MenClothingCatalog.Colors.Count),
                Text("OtherColor", "حدد اللون", "Other color", SectionMenClothing,
                        required: false, maxLength: 150)
                    .VisibleOnlyWhen("Colors", (int)MenClothingColor.Other)
                    .MandatoryWhen("Colors", (int)MenClothingColor.Other),
                LookupSelect("Condition", "الحالة", "Condition", SectionMenClothing,
                    AdFormLookupKeys.MenClothingConditions, required: true)
            ],
            AdFormLookupKeys.MenClothingTypes, AdFormLookupKeys.MenClothingBrands,
            AdFormLookupKeys.MenClothingSizes, AdFormLookupKeys.MenClothingColors,
            AdFormLookupKeys.MenClothingConditions, AdFormLookupKeys.MenClothingSellingMethods,
            AdFormLookupKeys.Governorates, AdFormLookupKeys.Centers);

    private static AdFormSchema WomenClothing() =>
        ClothingSchema("womenClothing", WomenClothingSubmit, SectionWomenClothing,
            (int)WomenClothingSellingMethod.Store, (int)WomenClothingSellingMethod.StoreAndOnline,
            AdFormLookupKeys.WomenClothingSellingMethods,
            "مثال: فساتين سواريه حريمي",
            [
                LookupSelect("ClothingType", "نوع الملابس", "Clothing type", SectionWomenClothing,
                        AdFormLookupKeys.WomenClothingTypes, required: true)
                    .AsSearchable(),
                Text("OtherClothingType", "حدد نوع الملابس", "Other clothing type", SectionWomenClothing,
                        required: false, maxLength: 150)
                    .VisibleOnlyWhen("ClothingType", (int)WomenClothingType.Other)
                    .MandatoryWhen("ClothingType", (int)WomenClothingType.Other),
                LookupSelect("Brand", "الماركة", "Brand", SectionWomenClothing,
                    AdFormLookupKeys.WomenClothingBrands, required: true),
                Text("OtherBrand", "حدد الماركة", "Other brand", SectionWomenClothing,
                        required: false, maxLength: 150)
                    .VisibleOnlyWhen("Brand", (int)WomenClothingBrand.Other)
                    .MandatoryWhen("Brand", (int)WomenClothingBrand.Other),
                ClothingMultiSelect("Sizes", "المقاسات", "Sizes", SectionWomenClothing,
                    AdFormLookupKeys.WomenClothingSizes, WomenClothingCatalog.Sizes.Count),
                ClothingMultiSelect("Colors", "الألوان", "Colors", SectionWomenClothing,
                    AdFormLookupKeys.WomenClothingColors, WomenClothingCatalog.Colors.Count),
                Text("OtherColor", "حدد اللون", "Other color", SectionWomenClothing,
                        required: false, maxLength: 150)
                    .VisibleOnlyWhen("Colors", (int)WomenClothingColor.Other)
                    .MandatoryWhen("Colors", (int)WomenClothingColor.Other),
                LookupSelect("Condition", "الحالة", "Condition", SectionWomenClothing,
                    AdFormLookupKeys.WomenClothingConditions, required: true)
            ],
            AdFormLookupKeys.WomenClothingTypes, AdFormLookupKeys.WomenClothingBrands,
            AdFormLookupKeys.WomenClothingSizes, AdFormLookupKeys.WomenClothingColors,
            AdFormLookupKeys.WomenClothingConditions, AdFormLookupKeys.WomenClothingSellingMethods,
            AdFormLookupKeys.Governorates, AdFormLookupKeys.Centers);

    private static AdFormSchema KidsClothing() =>
        ClothingSchema("kidsClothing", KidsClothingSubmit, SectionKidsClothing,
            (int)KidsClothingSellingMethod.Store, (int)KidsClothingSellingMethod.StoreAndOnline,
            AdFormLookupKeys.KidsClothingSellingMethods,
            "مثال: أطقم أطفال قطن مواليد",
            [
                LookupSelect("ClothingType", "نوع الملابس", "Clothing type", SectionKidsClothing,
                        AdFormLookupKeys.KidsClothingTypes, required: true)
                    .AsSearchable(),
                Text("OtherClothingType", "حدد نوع الملابس", "Other clothing type", SectionKidsClothing,
                        required: false, maxLength: 150)
                    .VisibleOnlyWhen("ClothingType", (int)KidsClothingType.Other)
                    .MandatoryWhen("ClothingType", (int)KidsClothingType.Other),
                LookupSelect("Brand", "الماركة", "Brand", SectionKidsClothing,
                    AdFormLookupKeys.KidsClothingBrands, required: true),
                Text("OtherBrand", "حدد الماركة", "Other brand", SectionKidsClothing,
                        required: false, maxLength: 150)
                    .VisibleOnlyWhen("Brand", (int)KidsClothingBrand.Other)
                    .MandatoryWhen("Brand", (int)KidsClothingBrand.Other),
                ClothingMultiSelect("Sizes", "المقاسات", "Sizes", SectionKidsClothing,
                    AdFormLookupKeys.KidsClothingSizes, KidsClothingCatalog.Sizes.Count),
                ClothingMultiSelect("Colors", "الألوان", "Colors", SectionKidsClothing,
                    AdFormLookupKeys.KidsClothingColors, KidsClothingCatalog.Colors.Count),
                Text("OtherColor", "حدد اللون", "Other color", SectionKidsClothing,
                        required: false, maxLength: 150)
                    .VisibleOnlyWhen("Colors", (int)KidsClothingColor.Other)
                    .MandatoryWhen("Colors", (int)KidsClothingColor.Other),
                LookupSelect("Condition", "الحالة", "Condition", SectionKidsClothing,
                    AdFormLookupKeys.KidsClothingConditions, required: true)
            ],
            AdFormLookupKeys.KidsClothingTypes, AdFormLookupKeys.KidsClothingBrands,
            AdFormLookupKeys.KidsClothingSizes, AdFormLookupKeys.KidsClothingColors,
            AdFormLookupKeys.KidsClothingConditions, AdFormLookupKeys.KidsClothingSellingMethods,
            AdFormLookupKeys.Governorates, AdFormLookupKeys.Centers);

    private static AdFormSchema ShoppingSchema(
        string module, CreateAdFormSubmitDto submit,
        IEnumerable<FormFieldDto> storeFields, IEnumerable<FormFieldDto> productFields,
        string titlePlaceholder, string descriptionGuidance, params string[] lookups)
    {
        var fields = new List<FormFieldDto>();

        fields.AddRange(storeFields);

        fields.AddRange(productFields);

        fields.Add(RequiredImages());
        fields.Add(VideoUpload());

        fields.Add(ShoppingTitle(titlePlaceholder));
        fields.Add(BusinessDescription(descriptionGuidance));

        return new AdFormSchema(module, submit, lookups, Order(fields));
    }

    private static IEnumerable<FormFieldDto> ShoppingStoreFields(string storeNamePlaceholder) =>
    [
        Text("StoreName", "اسم المحل", "Store name", SectionStore,
            required: true, maxLength: 150, placeholder: storeNamePlaceholder),
        PhoneNumber("Phone"),
        PhoneNumber("WhatsApp", "رقم الواتساب", "WhatsApp number")
    ];

    private static FormFieldDto ShoppingTitle(string placeholder)
    {
        var field = Title();
        field.Placeholder = placeholder;
        return field;
    }

    private static FormFieldDto ShoppingMultiSelect(
        string name, string label, string labelEn, string section, string optionsSource, int maxSelections)
    {
        var field = LookupSelect(name, label, labelEn, section, optionsSource,
            required: true, type: FormFieldTypes.MultiSelect);

        field.MaxSelections = maxSelections;
        field.MinItems = 1;
        return field;
    }

    private static FormFieldDto ShoppingOtherText(
        string name, string label, string labelEn, string section, string dependsOn, int otherValue) =>
        Text(name, label, labelEn, section, required: false, maxLength: 150)
            .VisibleOnlyWhen(dependsOn, otherValue)
            .MandatoryWhen(dependsOn, otherValue);

    private static IEnumerable<FormFieldDto> ShoppingPriceAndShipping() =>
    [
        Price(),
        Checkbox("ShippingAvailable", "الشحن متاح", "Shipping available", SectionProduct)
    ];

    private static IEnumerable<FormFieldDto> ShoppingPriceAndDelivery() =>
    [
        Price(),
        Checkbox("DeliveryAvailable", "التوصيل متاح", "Delivery available", SectionProduct)
    ];

    private static AdFormSchema Accessories()
    {
        var storeFields = new List<FormFieldDto>
        {
            Text("StoreName", "اسم المحل", "Store name", SectionStore,
                required: true, maxLength: 150, placeholder: "مثال: محل الأناقة للإكسسوارات"),
            PhoneNumber("Phone"),
            PhoneNumber("WhatsApp", "رقم الواتساب", "WhatsApp number"),
            StoreLogo()
        };

        var productFields = new List<FormFieldDto>
        {
            LookupSelect("AccessoryType", "نوع الإكسسوار", "Accessory type", SectionProduct,
                    AdFormLookupKeys.AccessoryTypes, required: true)
                .AsSearchable(),
            ShoppingOtherText("OtherAccessoryType", "حدد نوع الإكسسوار", "Other accessory type",
                SectionProduct, "AccessoryType", (int)AccessoryType.Other),
            LookupSelect("Category", "الفئة", "Category", SectionProduct,
                AdFormLookupKeys.AccessoryCategories, required: true),
            LookupSelect("Material", "الخامة", "Material", SectionProduct,
                AdFormLookupKeys.AccessoryMaterials, required: true),
            ShoppingOtherText("OtherMaterial", "حدد الخامة", "Other material",
                SectionProduct, "Material", (int)AccessoryMaterial.Other),
            ShoppingMultiSelect("Colors", "الألوان", "Colors", SectionProduct,
                AdFormLookupKeys.AccessoryColors, AccessoryCatalog.Colors.Count),
            ShoppingOtherText("OtherColor", "حدد اللون", "Other color",
                SectionProduct, "Colors", (int)AccessoryColor.Other)
        };
        productFields.AddRange(ShoppingPriceAndShipping());

        return ShoppingSchema("accessories", AccessoriesSubmit, storeFields, productFields,
            "مثال: ساعات رجالي ستانلس مستوردة",
            "اذكر كل التفاصيل المهمة: المقاس، الخامة، الألوان المتاحة وأي ملاحظات أخرى.",
            AdFormLookupKeys.AccessoryTypes, AdFormLookupKeys.AccessoryCategories,
            AdFormLookupKeys.AccessoryMaterials, AdFormLookupKeys.AccessoryColors);
    }

    private static AdFormSchema Cosmetics()
    {
        var productFields = new List<FormFieldDto>
        {
            LookupSelect("Section", "القسم", "Section", SectionProduct,
                    AdFormLookupKeys.CosmeticSections, required: true)
                .AsSearchable(),
            ShoppingOtherText("OtherSection", "حدد القسم", "Other section",
                SectionProduct, "Section", (int)CosmeticSection.Other),
            Text("Brand", "الماركة", "Brand", SectionProduct, required: true, maxLength: 150),
            LookupSelect("SuitableFor", "مناسب لـ", "Suitable for", SectionProduct,
                AdFormLookupKeys.CosmeticSuitableFor, required: true),
            Price(),
            Checkbox("DiscountAvailable", "يوجد خصم", "Discount available", SectionProduct),
            Checkbox("ShippingAvailable", "الشحن متاح", "Shipping available", SectionProduct)
        };

        return ShoppingSchema("cosmetics", CosmeticsSubmit,
            ShoppingStoreFields("مثال: محل الجمال لمستحضرات التجميل"), productFields,
            "مثال: كريم عناية بالبشرة مستورد",
            "اذكر كل التفاصيل المهمة: المكونات، طريقة الاستخدام، الحجم وأي ملاحظات أخرى.",
            AdFormLookupKeys.CosmeticSections, AdFormLookupKeys.CosmeticSuitableFor);
    }

    private static AdFormSchema HomeKitchen()
    {
        var productFields = new List<FormFieldDto>
        {
            LookupSelect("Section", "القسم", "Section", SectionProduct,
                    AdFormLookupKeys.HomeKitchenSections, required: true)
                .AsSearchable(),
            ShoppingOtherText("OtherSection", "حدد القسم", "Other section",
                SectionProduct, "Section", (int)HomeKitchenSection.Other),
            LookupSelect("Material", "الخامة", "Material", SectionProduct,
                AdFormLookupKeys.HomeKitchenMaterials, required: true),
            ShoppingOtherText("OtherMaterial", "حدد الخامة", "Other material",
                SectionProduct, "Material", (int)HomeKitchenMaterial.Other),
            ShoppingMultiSelect("Colors", "الألوان", "Colors", SectionProduct,
                AdFormLookupKeys.HomeKitchenColors, HomeKitchenCatalog.Colors.Count),
            ShoppingOtherText("OtherColor", "حدد اللون", "Other color",
                SectionProduct, "Colors", (int)HomeKitchenColor.Other)
        };
        productFields.AddRange(ShoppingPriceAndDelivery());

        return ShoppingSchema("homeKitchen", HomeKitchenSubmit,
            ShoppingStoreFields("مثال: بيت الأدوات المنزلية"), productFields,
            "مثال: طقم أواني ستانلس 12 قطعة",
            "اذكر كل التفاصيل المهمة: المقاسات، عدد القطع، الخامة وأي ملاحظات أخرى.",
            AdFormLookupKeys.HomeKitchenSections, AdFormLookupKeys.HomeKitchenMaterials,
            AdFormLookupKeys.HomeKitchenColors);
    }

    private static AdFormSchema ShoppingElectronics()
    {
        var productFields = new List<FormFieldDto>
        {
            LookupSelect("Section", "القسم", "Section", SectionProduct,
                    AdFormLookupKeys.ShoppingElectronicSections, required: true)
                .AsSearchable(),
            ShoppingOtherText("OtherSection", "حدد القسم", "Other section",
                SectionProduct, "Section", (int)ShoppingElectronicSection.Other),
            Text("Brand", "الماركة", "Brand", SectionProduct, required: true, maxLength: 150),
            LookupSelect("CompatibleWith", "متوافق مع", "Compatible with", SectionProduct,
                AdFormLookupKeys.ShoppingElectronicCompatibilities, required: true),
            LookupSelect("ProductCondition", "حالة المنتج", "Product condition", SectionProduct,
                AdFormLookupKeys.ShoppingElectronicConditions, required: true),

            LookupSelect("Warranty", "الضمان", "Warranty", SectionProduct,
                AdFormLookupKeys.ShoppingElectronicWarranties, required: true)
        };
        productFields.AddRange(ShoppingPriceAndShipping());

        return ShoppingSchema("shoppingElectronics", ShoppingElectronicsSubmit,
            ShoppingStoreFields("مثال: محل التقنية للإلكترونيات"), productFields,
            "مثال: سماعة بلوتوث أصلية بضمان",
            "اذكر كل التفاصيل المهمة: المواصفات، مدة الضمان، محتويات العلبة وأي ملاحظات أخرى.",
            AdFormLookupKeys.ShoppingElectronicSections,
            AdFormLookupKeys.ShoppingElectronicCompatibilities,
            AdFormLookupKeys.ShoppingElectronicConditions,
            AdFormLookupKeys.ShoppingElectronicWarranties);
    }

    private static AdFormSchema GiftsToys()
    {
        var productFields = new List<FormFieldDto>
        {
            LookupSelect("GiftType", "النوع", "Type", SectionProduct,
                    AdFormLookupKeys.GiftToyTypes, required: true)
                .AsSearchable(),
            ShoppingOtherText("OtherGiftType", "حدد النوع", "Other type",
                SectionProduct, "GiftType", (int)GiftToyType.Other),
            LookupSelect("SuitableFor", "مناسب لـ", "Suitable for", SectionProduct,
                AdFormLookupKeys.GiftToySuitableFor, required: true),

            Checkbox("GiftWrapping", "تغليف هدايا", "Gift wrapping", SectionProduct)
        };
        productFields.AddRange(ShoppingPriceAndDelivery());

        return ShoppingSchema("giftsToys", GiftsToysSubmit,
            ShoppingStoreFields("مثال: محل الفرحة للهدايا"), productFields,
            "مثال: بوكس هدايا فاخر لعيد الميلاد",
            "اذكر كل التفاصيل المهمة: محتويات الهدية، المقاس، خيارات التغليف وأي ملاحظات أخرى.",
            AdFormLookupKeys.GiftToyTypes, AdFormLookupKeys.GiftToySuitableFor);
    }

    private static AdFormSchema HomemadeFood()
    {
        var projectFields = new List<FormFieldDto>
        {
            Text("ProjectName", "اسم المشروع", "Project name", SectionProject,
                required: true, maxLength: 150, placeholder: "مثال: مطبخ أم أحمد"),
            PhoneNumber("Phone"),
            PhoneNumber("WhatsApp", "رقم الواتساب", "WhatsApp number")
        };

        var productFields = new List<FormFieldDto>
        {
            LookupSelect("Section", "القسم", "Section", SectionProduct,
                    AdFormLookupKeys.HomemadeFoodSections, required: true)
                .AsSearchable(),
            ShoppingOtherText("OtherSection", "حدد القسم", "Other section",
                SectionProduct, "Section", (int)HomemadeFoodSection.Other),
            Checkbox("PreparedOnDemand", "يتم التحضير عند الطلب", "Prepared on demand", SectionProduct),
            Number("MinimumOrderQuantity", "الحد الأدنى للطلب", "Minimum order quantity",
                SectionProduct, required: true, min: 1),
            PreparationTime(),
            Checkbox("DeliveryAvailable", "التوصيل متاح", "Delivery available", SectionProduct),

            DeliveryAreas(),

            Price(),

            TextArea("Ingredients", "المكونات", "Ingredients", SectionProductDetails,
                required: false, maxLength: 2000),
            Text("WeightOrSize", "الوزن أو الحجم", "Weight or size", SectionProductDetails,
                required: false, maxLength: 150, placeholder: "مثال: 1 كيلو"),
            TextArea("StorageMethod", "طريقة التخزين", "Storage method", SectionProductDetails,
                required: false, maxLength: 500),
            Text("AvailableOrderingHours", "مواعيد استقبال الطلبات", "Available ordering hours",
                SectionProductDetails, required: false, maxLength: 200,
                placeholder: "مثال: يوميًا من 10 ص إلى 8 م"),
            TextArea("AdditionalNotes", "ملاحظات إضافية", "Additional notes", SectionProductDetails,
                required: false, maxLength: 2000)
        };

        return ShoppingSchema("homemadeFood", HomemadeFoodSubmit, projectFields, productFields,
            "مثال: تورت شوكولاتة بالطلب",
            "اذكر كل التفاصيل المهمة: المكونات، الوزن، طريقة التخزين ومواعيد استقبال الطلبات.",
            AdFormLookupKeys.HomemadeFoodSections, AdFormLookupKeys.HomemadeFoodDeliveryAreas);
    }

    private static AdFormSchema HomeFurnishingSchema(
        string module, CreateAdFormSubmitDto submit, string section,
        string productNamePlaceholder, string titlePlaceholder, string descriptionPlaceholder,
        IEnumerable<FormFieldDto> moduleFields, params string[] lookups)
    {
        var fields = new List<FormFieldDto>
        {
            Text("SellerName", "اسم البائع", "Seller name", SectionSeller,
                required: true, maxLength: 150),
            PhoneNumber("Phone"),
            PhoneNumber("WhatsApp", "رقم الواتساب", "WhatsApp number", required: false),
            Email(),

            Text("ProductName", "اسم المنتج", "Product name", section,
                required: true, maxLength: 150, placeholder: productNamePlaceholder)
        };

        fields.AddRange(moduleFields);

        fields.Add(Price());
        fields.Add(Negotiable());

        fields.AddRange(LocationFields(includeAddress: true));

        fields.Add(RequiredImages());
        fields.Add(VideoUpload());

        fields.Add(ClothingTitle(titlePlaceholder));
        fields.Add(BusinessDescription(descriptionPlaceholder));

        return new AdFormSchema(module, submit, lookups, Order(fields));
    }

    private static FormFieldDto HomeFurnishingColors(string section, string optionsSource, int optionCount)
    {
        var field = LookupSelect("Colors", "الألوان", "Colors", section, optionsSource,
            required: true, type: FormFieldTypes.MultiSelect);

        field.MaxSelections = optionCount;
        field.MinItems = 1;
        return field;
    }

    private static AdFormSchema Furniture() =>
        HomeFurnishingSchema("furniture", FurnitureSubmit, SectionFurniture,
            "مثال: كنبة ثلاث مقاعد", "مثال: كنبة مودرن بحالة ممتازة",
            "اذكر كل التفاصيل المهمة: الخامة، المقاسات، الحالة وهل هو قابل للفك.",
            [
                LookupSelect("FurnitureType", "نوع الأثاث", "Furniture type", SectionFurniture,
                        AdFormLookupKeys.FurnitureTypes, required: true)
                    .AsSearchable(),
                ShoppingOtherText("OtherFurnitureType", "حدد نوع الأثاث", "Other furniture type",
                    SectionFurniture, "FurnitureType", (int)FurnitureType.Other),
                LookupSelect("Material", "الخامة", "Material", SectionFurniture,
                    AdFormLookupKeys.FurnitureMaterials, required: true),
                ShoppingOtherText("OtherMaterial", "حدد الخامة", "Other material",
                    SectionFurniture, "Material", (int)FurnitureMaterial.Other),
                HomeFurnishingColors(SectionFurniture, AdFormLookupKeys.FurnitureColors,
                    FurnitureCatalog.Colors.Count),
                ShoppingOtherText("OtherColor", "حدد اللون", "Other color",
                    SectionFurniture, "Colors", (int)FurnitureColor.Other),
                LookupSelect("Condition", "الحالة", "Condition", SectionFurniture,
                    AdFormLookupKeys.FurnitureConditions, required: true),

                Number("Length", "الطول (سم)", "Length (cm)", SectionDimensions, required: true, min: 0.01m),
                Number("Width", "العرض (سم)", "Width (cm)", SectionDimensions, required: true, min: 0.01m),
                Number("Height", "الارتفاع (سم)", "Height (cm)", SectionDimensions, required: true, min: 0.01m),

                Checkbox("CanBeDisassembled", "قابل للفك والتركيب", "Can be disassembled", SectionFurniture),
                Checkbox("DeliveryAvailable", "التوصيل متاح", "Delivery available", SectionFurniture)
            ],
            AdFormLookupKeys.FurnitureTypes, AdFormLookupKeys.FurnitureMaterials,
            AdFormLookupKeys.FurnitureColors, AdFormLookupKeys.FurnitureConditions,
            AdFormLookupKeys.Governorates, AdFormLookupKeys.Centers);

    private static AdFormSchema FurnishingCurtains() =>
        HomeFurnishingSchema("furnishingsCurtains", FurnishingCurtainSubmit, SectionFurnishingCurtain,
            "مثال: ستارة بلاك أوت", "مثال: ستائر بلاك أوت مقاس 3 متر",
            "اذكر كل التفاصيل المهمة: الخامة، المقاس، الألوان المتاحة وأي ملاحظات أخرى.",
            [
                LookupSelect("ProductType", "نوع المنتج", "Product type", SectionFurnishingCurtain,
                        AdFormLookupKeys.FurnishingCurtainProductTypes, required: true)
                    .AsSearchable(),
                ShoppingOtherText("OtherProductType", "حدد نوع المنتج", "Other product type",
                    SectionFurnishingCurtain, "ProductType", (int)FurnishingCurtainProductType.Other),
                LookupSelect("Size", "المقاس", "Size", SectionFurnishingCurtain,
                    AdFormLookupKeys.FurnishingCurtainSizes, required: true),
                ShoppingOtherText("OtherSize", "حدد المقاس", "Other size",
                    SectionFurnishingCurtain, "Size", (int)FurnishingCurtainSize.Other),
                LookupSelect("Material", "الخامة", "Material", SectionFurnishingCurtain,
                    AdFormLookupKeys.FurnishingCurtainMaterials, required: true),
                ShoppingOtherText("OtherMaterial", "حدد الخامة", "Other material",
                    SectionFurnishingCurtain, "Material", (int)FurnishingCurtainMaterial.Other),
                HomeFurnishingColors(SectionFurnishingCurtain, AdFormLookupKeys.FurnishingCurtainColors,
                    FurnishingCurtainCatalog.Colors.Count),
                ShoppingOtherText("OtherColor", "حدد اللون", "Other color",
                    SectionFurnishingCurtain, "Colors", (int)FurnishingCurtainColor.Other),
                Checkbox("DeliveryAvailable", "التوصيل متاح", "Delivery available", SectionFurnishingCurtain)
            ],
            AdFormLookupKeys.FurnishingCurtainProductTypes, AdFormLookupKeys.FurnishingCurtainSizes,
            AdFormLookupKeys.FurnishingCurtainMaterials, AdFormLookupKeys.FurnishingCurtainColors,
            AdFormLookupKeys.Governorates, AdFormLookupKeys.Centers);

    private static readonly object[] LightingProductTypeValues =
        LightingDecorProductTypes.Lighting.Select(type => (object)(int)type).ToArray();

    private static AdFormSchema LightingDecor() =>
        HomeFurnishingSchema("lightingDecor", LightingDecorSubmit, SectionLightingDecor,
            "مثال: نجفة كريستال", "مثال: نجفة كريستال 6 لمبات",
            "اذكر كل التفاصيل المهمة: الخامة، المقاس، نوع الإضاءة وأي ملاحظات أخرى.",
            [
                LookupSelect("ProductType", "نوع المنتج", "Product type", SectionLightingDecor,
                        AdFormLookupKeys.LightingDecorProductTypes, required: true)
                    .AsSearchable(),
                ShoppingOtherText("OtherProductType", "حدد نوع المنتج", "Other product type",
                    SectionLightingDecor, "ProductType", (int)LightingDecorProductType.Other),
                LookupSelect("Material", "الخامة", "Material", SectionLightingDecor,
                    AdFormLookupKeys.LightingDecorMaterials, required: true),
                ShoppingOtherText("OtherMaterial", "حدد الخامة", "Other material",
                    SectionLightingDecor, "Material", (int)LightingDecorMaterial.Other),
                HomeFurnishingColors(SectionLightingDecor, AdFormLookupKeys.LightingDecorColors,
                    LightingDecorCatalog.Colors.Count),
                ShoppingOtherText("OtherColor", "حدد اللون", "Other color",
                    SectionLightingDecor, "Colors", (int)LightingDecorColor.Other),

                LookupSelect("LightType", "نوع الإضاءة", "Light type", SectionLightingDecor,
                        AdFormLookupKeys.LightingDecorLightTypes, required: false)
                    .VisibleOnlyWhen("ProductType", LightingProductTypeValues)
                    .MandatoryWhen("ProductType", LightingProductTypeValues)
            ],
            AdFormLookupKeys.LightingDecorProductTypes, AdFormLookupKeys.LightingDecorMaterials,
            AdFormLookupKeys.LightingDecorColors, AdFormLookupKeys.LightingDecorLightTypes,
            AdFormLookupKeys.Governorates, AdFormLookupKeys.Centers);

    private static AdFormSchema KitchenTools() =>
        HomeFurnishingSchema("kitchenTools", KitchenToolSubmit, SectionKitchenTool,
            "مثال: طقم حلل استانلس", "مثال: طقم حلل استانلس 10 قطع",
            "اذكر كل التفاصيل المهمة: الخامة، عدد القطع، الألوان المتاحة وأي ملاحظات أخرى.",
            [
                LookupSelect("ProductType", "نوع المنتج", "Product type", SectionKitchenTool,
                        AdFormLookupKeys.KitchenToolProductTypes, required: true)
                    .AsSearchable(),
                ShoppingOtherText("OtherProductType", "حدد نوع المنتج", "Other product type",
                    SectionKitchenTool, "ProductType", (int)KitchenToolProductType.Other),
                LookupSelect("Material", "الخامة", "Material", SectionKitchenTool,
                    AdFormLookupKeys.KitchenToolMaterials, required: true),
                ShoppingOtherText("OtherMaterial", "حدد الخامة", "Other material",
                    SectionKitchenTool, "Material", (int)KitchenToolMaterial.Other),
                HomeFurnishingColors(SectionKitchenTool, AdFormLookupKeys.KitchenToolColors,
                    KitchenToolCatalog.Colors.Count),
                ShoppingOtherText("OtherColor", "حدد اللون", "Other color",
                    SectionKitchenTool, "Colors", (int)KitchenToolColor.Other)
            ],
            AdFormLookupKeys.KitchenToolProductTypes, AdFormLookupKeys.KitchenToolMaterials,
            AdFormLookupKeys.KitchenToolColors,
            AdFormLookupKeys.Governorates, AdFormLookupKeys.Centers);

    private static AdFormSchema HomeAppliances() =>
        HomeFurnishingSchema("homeAppliances", HomeApplianceSubmit, SectionHomeAppliance,
            "مثال: غسالة أوتوماتيك", "مثال: غسالة أوتوماتيك 8 كيلو بالضمان",
            "اذكر كل التفاصيل المهمة: الماركة، الحالة، الضمان والقدرة الكهربائية.",
            [
                LookupSelect("DeviceType", "نوع الجهاز", "Device type", SectionHomeAppliance,
                        AdFormLookupKeys.HomeApplianceDeviceTypes, required: true)
                    .AsSearchable(),
                ShoppingOtherText("OtherDeviceType", "حدد نوع الجهاز", "Other device type",
                    SectionHomeAppliance, "DeviceType", (int)HomeApplianceDeviceType.Other),
                LookupSelect("Brand", "الماركة", "Brand", SectionHomeAppliance,
                        AdFormLookupKeys.HomeApplianceBrands, required: true)
                    .AsSearchable(),
                ShoppingOtherText("OtherBrand", "حدد الماركة", "Other brand",
                    SectionHomeAppliance, "Brand", (int)HomeApplianceBrand.Other),
                HomeFurnishingColors(SectionHomeAppliance, AdFormLookupKeys.HomeApplianceColors,
                    HomeApplianceCatalog.Colors.Count),
                ShoppingOtherText("OtherColor", "حدد اللون", "Other color",
                    SectionHomeAppliance, "Colors", (int)HomeApplianceColor.Other),
                LookupSelect("Condition", "الحالة", "Condition", SectionHomeAppliance,
                    AdFormLookupKeys.HomeApplianceConditions, required: true),
                LookupSelect("Warranty", "الضمان", "Warranty", SectionHomeAppliance,
                    AdFormLookupKeys.HomeApplianceWarranties, required: true),

                Text("WarrantyDuration", "مدة الضمان", "Warranty duration", SectionHomeAppliance,
                        required: false, maxLength: 100, placeholder: "مثال: سنتان")
                    .VisibleOnlyWhen("Warranty", (int)HomeApplianceWarranty.Available)
                    .MandatoryWhen("Warranty", (int)HomeApplianceWarranty.Available),

                Text("PowerRating", "القدرة الكهربائية", "Power rating", SectionHomeAppliance,
                    required: false, maxLength: 100, placeholder: "مثال: 1500 وات"),
                Checkbox("DeliveryAvailable", "التوصيل متاح", "Delivery available", SectionHomeAppliance)
            ],
            AdFormLookupKeys.HomeApplianceDeviceTypes, AdFormLookupKeys.HomeApplianceBrands,
            AdFormLookupKeys.HomeApplianceColors, AdFormLookupKeys.HomeApplianceConditions,
            AdFormLookupKeys.HomeApplianceWarranties,
            AdFormLookupKeys.Governorates, AdFormLookupKeys.Centers);

    private static AdFormSchema BathroomSupplies() =>
        HomeFurnishingSchema("bathroomSupplies", BathroomSupplySubmit, SectionBathroomSupply,
            "مثال: طقم حمام كامل", "مثال: طقم حمام سيراميك كامل",
            "اذكر كل التفاصيل المهمة: الخامة، المقاسات، الألوان المتاحة وأي ملاحظات أخرى.",
            [
                LookupSelect("ProductType", "نوع المنتج", "Product type", SectionBathroomSupply,
                        AdFormLookupKeys.BathroomSupplyProductTypes, required: true)
                    .AsSearchable(),
                ShoppingOtherText("OtherProductType", "حدد نوع المنتج", "Other product type",
                    SectionBathroomSupply, "ProductType", (int)BathroomSupplyProductType.Other),
                LookupSelect("Material", "الخامة", "Material", SectionBathroomSupply,
                    AdFormLookupKeys.BathroomSupplyMaterials, required: true),
                ShoppingOtherText("OtherMaterial", "حدد الخامة", "Other material",
                    SectionBathroomSupply, "Material", (int)BathroomSupplyMaterial.Other),
                HomeFurnishingColors(SectionBathroomSupply, AdFormLookupKeys.BathroomSupplyColors,
                    BathroomSupplyCatalog.Colors.Count),
                ShoppingOtherText("OtherColor", "حدد اللون", "Other color",
                    SectionBathroomSupply, "Colors", (int)BathroomSupplyColor.Other)
            ],
            AdFormLookupKeys.BathroomSupplyProductTypes, AdFormLookupKeys.BathroomSupplyMaterials,
            AdFormLookupKeys.BathroomSupplyColors,
            AdFormLookupKeys.Governorates, AdFormLookupKeys.Centers);

    private static AdFormSchema PlantsOrnaments() =>
        HomeFurnishingSchema("plantsOrnaments", PlantOrnamentSubmit, SectionPlantOrnament,
            "مثال: نبات صبار", "مثال: نبات صبار للمكتب",
            "اذكر كل التفاصيل المهمة: نوع النبات، الارتفاع، مكان وضعه وطريقة العناية به.",
            [
                LookupSelect("ProductType", "نوع المنتج", "Product type", SectionPlantOrnament,
                        AdFormLookupKeys.PlantOrnamentProductTypes, required: true)
                    .AsSearchable(),
                ShoppingOtherText("OtherProductType", "حدد نوع المنتج", "Other product type",
                    SectionPlantOrnament, "ProductType", (int)PlantOrnamentProductType.Other),
                LookupSelect("SuitableFor", "مناسب لـ", "Suitable for", SectionPlantOrnament,
                    AdFormLookupKeys.PlantOrnamentSuitableFor, required: true),

                Number("Height", "الارتفاع (سم)", "Height (cm)", SectionPlantOrnament,
                    required: false, min: 0)
            ],
            AdFormLookupKeys.PlantOrnamentProductTypes, AdFormLookupKeys.PlantOrnamentSuitableFor,
            AdFormLookupKeys.Governorates, AdFormLookupKeys.Centers);

    private static FormFieldDto StoreLogo()
    {
        var field = LogoUpload("شعار المحل", "Store logo");
        field.Section = SectionStore;
        return field;
    }

    private static FormFieldDto PreparationTime()
    {
        var field = Text("PreparationTime", "مدة التحضير", "Preparation time", SectionProduct,
            required: true, maxLength: 100, placeholder: HomemadeFoodCatalog.PreparationTimeExamples[0]);
        field.HelpText = $"أمثلة: {string.Join("، ", HomemadeFoodCatalog.PreparationTimeExamples)}.";
        return field;
    }

    private static FormFieldDto DeliveryAreas()
    {
        var field = ShoppingMultiSelect("DeliveryAreas", "مناطق التوصيل", "Delivery areas",
            SectionProduct, AdFormLookupKeys.HomemadeFoodDeliveryAreas,
            HomemadeFoodCatalog.DeliveryAreas.Count);

        field.HelpText = "تظهر فقط عند تفعيل التوصيل.";
        return field
            .VisibleOnlyWhen("DeliveryAvailable", true)
            .MandatoryWhen("DeliveryAvailable", true);
    }

    private static IEnumerable<FormFieldDto> LocationFields(
        string centerLabel = "المركز", bool includeAddress = false, bool centerRequired = true)
    {
        yield return Governorate();
        yield return Center(centerLabel, required: centerRequired);

        if (includeAddress)
        {
            yield return Address();
            yield return GoogleMapsUrl();
        }
    }

    private static AdFormSchema RealEstateSchema(
        string module, CreateAdFormSubmitDto submit,
        IEnumerable<FormFieldDto> propertyFields,
        IEnumerable<FormFieldDto> conditionalFields,
        string titlePlaceholder, string descriptionGuidance,
        params string[] moduleLookups) =>
        RealEstateSchema(module, submit, propertyFields, conditionalFields,
            titlePlaceholder, descriptionGuidance, false, [], moduleLookups);

    private static AdFormSchema RealEstateSchema(
        string module, CreateAdFormSubmitDto submit,
        IEnumerable<FormFieldDto> propertyFields,
        IEnumerable<FormFieldDto> conditionalFields,
        string titlePlaceholder, string descriptionGuidance,
        bool everyFieldRequired,
        IReadOnlyCollection<string> optionalFields,
        params string[] moduleLookups)
    {
        var fields = new List<FormFieldDto>();

        fields.Add(RealEstateTitle(titlePlaceholder));
        fields.Add(TextArea("Description", "وصف الإعلان", "Advertisement description", SectionAd,
            required: true, maxLength: 4000, placeholder: descriptionGuidance));

        fields.Add(Text("AdvertiserName", "اسم المعلن", "Advertiser name", SectionAd,
            required: true, maxLength: 150, placeholder: "مثال: أحمد محمود"));

        fields.Add(RequiredImages());
        fields.Add(VideoUpload());

        fields.Add(LookupSelect("ListingType", "نوع الإعلان", "Listing type", SectionAd,
            AdFormLookupKeys.RealEstateListingTypes, required: true));

        fields.AddRange(propertyFields);

        fields.AddRange(conditionalFields);

        fields.AddRange(RealEstateLocationFields());

        fields.Add(PhoneNumber("Phone"));
        fields.Add(PhoneNumber("WhatsApp", "واتساب", "WhatsApp number", required: false));
        fields.Add(Email());

        fields.Add(Checkbox("Negotiable", "السعر قابل للتفاوض", "Negotiable", SectionExtraInformation));
        fields.Add(TextArea("Notes", "ملاحظات إضافية", "Additional notes", SectionExtraInformation,
            required: false, maxLength: 4000));

        var lookups = new List<string>
        {
            AdFormLookupKeys.RealEstateListingTypes,
            AdFormLookupKeys.RealEstateProjects,
            AdFormLookupKeys.Governorates,
            AdFormLookupKeys.Centers
        };
        lookups.AddRange(moduleLookups);

        if (everyFieldRequired)
            RequireEveryField(fields, optionalFields);

        return new AdFormSchema(module, submit, lookups, Order(fields));
    }

    private static void RequireEveryField(List<FormFieldDto> fields, IReadOnlyCollection<string> optionalFields)
    {
        foreach (var field in fields)
        {
            if (optionalFields.Contains(field.Name))
                continue;

            if (field.ReadOnly == true)
                continue;

            if (field.VisibleWhen is null)
            {
                field.Required = true;
                continue;
            }

            field.RequiredWhen ??= field.VisibleWhen;
        }
    }

    private static FormFieldDto RealEstateTitle(string placeholder)
    {
        var field = Title();
        field.Placeholder = placeholder;
        return field;
    }

    private static IEnumerable<FormFieldDto> RealEstateLocationFields()
    {
        yield return Governorate();
        yield return Center();

        yield return LookupSelect("Project", "اسم المشروع / الحي", "Project / district",
                SectionLocation, AdFormLookupKeys.RealEstateProjects, required: false)
            .AsSearchable()
            .VisibleOnlyWhen("Center", RealEstateCatalog.ProjectCenter)
            .MandatoryWhen("Center", RealEstateCatalog.ProjectCenter);

        yield return RealEstateOtherText("OtherProject", "اسم المشروع", "Project name",
            SectionLocation, "Project", (int)RealEstateProject.Other);

        yield return Text("District", "المنطقة / القرية", "District / village", SectionLocation,
            required: false, maxLength: 150);

        yield return Text("Address", "العنوان التفصيلي", "Detailed address", SectionLocation,
            required: true, maxLength: 300, placeholder: "الشارع، العلامة المميزة…");

        yield return RealEstateGoogleMaps();
    }

    private static FormFieldDto RealEstateSalePrice(
        string name = "Price", string label = "السعر (جنيه)", string labelEn = "Price",
        string section = SectionAd) =>
        Number(name, label, labelEn, section, required: false, min: 0.01m)
            .VisibleOnlyWhen("ListingType", (int)RealEstateListingType.Sale)
            .MandatoryWhen("ListingType", (int)RealEstateListingType.Sale);

    private static FormFieldDto RealEstateSaleOnlyMoney(
        string name, string label, string labelEn, string section) =>
        Number(name, label, labelEn, section, required: false, min: 0.01m)
            .VisibleOnlyWhen("ListingType", (int)RealEstateListingType.Sale);

    private static FormFieldDto RealEstateGoogleMaps() =>
        new()
        {
            Name = "GoogleMaps",
            Label = "رابط الموقع على خرائط جوجل",
            LabelEn = "Google Maps link",
            Type = FormFieldTypes.Text,
            Required = false,
            Section = SectionLocation,
            MaxLength = 1000,
            Placeholder = "https://maps.google.com/…"
        };

    private static FormFieldDto RealEstateOtherText(
        string name, string label, string labelEn, string section, string dependsOn, int otherValue) =>
        Text(name, label, labelEn, section, required: false, maxLength: 150)
            .VisibleOnlyWhen(dependsOn, otherValue)
            .MandatoryWhen(dependsOn, otherValue);

    private static FormFieldDto YesNo(string name, string label, string labelEn, string section) =>
        Checkbox(name, label, labelEn, section);

    private static FormFieldDto YesNoWhen(
        string name, string label, string labelEn, string section,
        string dependsOn, params object[] values) =>
        Checkbox(name, label, labelEn, section).VisibleOnlyWhen(dependsOn, values);

    private static FormFieldDto RealEstateMultiSelect(
        string name, string label, string labelEn, string section, string optionsSource, int maxSelections)
    {
        var field = LookupSelect(name, label, labelEn, section, optionsSource,
            required: false, type: FormFieldTypes.MultiSelect);

        field.MaxSelections = maxSelections;
        return field;
    }

    private static IEnumerable<FormFieldDto> RealEstateRentMoneyFields(
        string downPaymentName, string availableFromLabel, string availableToLabel)
    {
        yield return Number("RentValue", "قيمة الإيجار", "Rent value", SectionRentDetails,
                required: false, min: 0.01m)
            .VisibleOnlyWhen("ListingType", (int)RealEstateListingType.Rent)
            .MandatoryWhen("ListingType", (int)RealEstateListingType.Rent);

        yield return Number("SecurityDeposit", "قيمة التأمين", "Security deposit", SectionRentDetails,
                required: false, min: 0)
            .VisibleOnlyWhen("ListingType", (int)RealEstateListingType.Rent);

        yield return Number(downPaymentName, "مقدم", "Down payment", SectionRentDetails,
                required: false, min: 0)
            .VisibleOnlyWhen("ListingType", (int)RealEstateListingType.Rent);

        yield return DatePicker("AvailableFrom", availableFromLabel, "Available from",
                SectionRentDetails, required: false)
            .VisibleOnlyWhen("ListingType", (int)RealEstateListingType.Rent);

        yield return DatePicker("AvailableTo", availableToLabel, "Available to",
                SectionRentDetails, required: false)
            .VisibleOnlyWhen("ListingType", (int)RealEstateListingType.Rent);
    }

    private static IEnumerable<FormFieldDto> RealEstateExchangeFields(string exchangeTargetsKey)
    {
        yield return LookupSelect("ExchangeWith", "أرغب بالبدل مع", "Interested in exchanging with",
                SectionExchangeDetails, exchangeTargetsKey, required: false)
            .AsSearchable()
            .VisibleOnlyWhen("ListingType", (int)RealEstateListingType.Exchange)
            .MandatoryWhen("ListingType", (int)RealEstateListingType.Exchange);

        yield return YesNoWhen("AcceptsDifferencePayment", "يقبل دفع فرق", "Accepts difference payment",
            SectionExchangeDetails, "ListingType", (int)RealEstateListingType.Exchange);

        yield return Number("DifferenceAmount", "قيمة الفرق", "Difference amount",
                SectionExchangeDetails, required: false, min: 0.01m)
            .VisibleOnlyWhen("AcceptsDifferencePayment", true)
            .MandatoryWhen("AcceptsDifferencePayment", true);

        yield return TextArea("ExchangeDetails", "تفاصيل البدل", "Exchange details",
                SectionExchangeDetails, required: false, maxLength: 4000)
            .VisibleOnlyWhen("ListingType", (int)RealEstateListingType.Exchange);
    }

    private static IEnumerable<FormFieldDto> RealEstateLicenceFields(string section, int licensedValue)
    {
        yield return Text("LicenseNumber", "رقم الرخصة", "License number", section,
                required: false, maxLength: 100)
            .VisibleOnlyWhen("LegalStatus", licensedValue);

        yield return DatePicker("LicenseIssueDate", "تاريخ إصدار الرخصة", "License issue date",
                section, required: false)
            .VisibleOnlyWhen("LegalStatus", licensedValue);

        yield return DatePicker("LicenseExpiryDate", "تاريخ انتهاء الرخصة", "License expiry date",
                section, required: false)
            .VisibleOnlyWhen("LegalStatus", licensedValue);
    }

    private static AdFormSchema Lands()
    {
        var propertyFields = new List<FormFieldDto>
        {
            LookupSelect("LandType", "نوع الأرض", "Land type", SectionLand,
                    AdFormLookupKeys.LandTypes, required: true)
                .AsSearchable(),
            RealEstateOtherText("OtherLandType", "اسم نوع الأرض", "Other land type",
                SectionLand, "LandType", (int)LandType.Other),

            LookupSelect("AreaUnit", "وحدة المساحة", "Area unit", SectionLand,
                AdFormLookupKeys.LandAreaUnits, required: true),
            Number("Area", "المساحة", "Area", SectionLand, required: true, min: 0.01m),

            RealEstateSalePrice("PricePerMeter", "سعر المتر", "Price per meter", SectionLand),
            RealEstateSalePrice("TotalPrice", "إجمالي السعر", "Total price", SectionLand),

            Number("Length", "الطول (م)", "Length (m)", SectionLand, required: true, min: 0.01m),
            Number("Width", "العرض (م)", "Width (m)", SectionLand, required: true, min: 0.01m),
            Number("FacadeLength", "طول الواجهة (م)", "Facade length (m)", SectionLand,
                required: true, min: 0.01m),

            LookupSelect("FacadesCount", "عدد الواجهات", "Facades count", SectionLand,
                AdFormLookupKeys.LandFacadesCounts, required: false),
            LookupSelect("Direction", "اتجاه الأرض", "Land direction", SectionLand,
                AdFormLookupKeys.LandDirections, required: true),

            Number("StreetWidth", "عرض الشارع (م)", "Street width (m)", SectionLand,
                required: false, min: 0.01m),
            LookupSelect("RoadType", "نوع الطريق", "Road type", SectionLand,
                AdFormLookupKeys.LandRoadTypes, required: false),

            YesNo("InsideBuildingCordon", "داخل كردون المباني", "Inside building cordon", SectionLand),
            YesNo("IsBuildable", "صالحة للبناء", "Buildable", SectionLand),

            Number("AllowedBuildingRatio", "نسبة البناء المسموح بها", "Allowed building ratio",
                    SectionLand, required: false, min: 0, max: 100)
                .VisibleOnlyWhen("IsBuildable", true),
            Number("AllowedFloorsCount", "عدد الأدوار المسموح بها", "Allowed floors count",
                    SectionLand, required: false, min: 1)
                .VisibleOnlyWhen("IsBuildable", true),

            LookupSelect("LegalStatus", "حالة الأرض", "Legal status", SectionLicensing,
                AdFormLookupKeys.LandLegalStatuses, required: true)
        };

        propertyFields.AddRange(RealEstateLicenceFields(SectionLicensing, (int)LandLegalStatus.Licensed));

        propertyFields.Add(
            Text("LicenseIssuer", "الجهة المصدرة للرخصة", "License issuer", SectionLicensing,
                    required: false, maxLength: 150)
                .VisibleOnlyWhen("LegalStatus", (int)LandLegalStatus.Licensed));

        propertyFields.AddRange(new[]
        {
            LookupSelect("ReconciliationForm", "نموذج التصالح", "Reconciliation form", SectionLicensing,
                    AdFormLookupKeys.LandReconciliationForms, required: false)
                .AsSearchable()
                .VisibleOnlyWhen("LegalStatus", (int)LandLegalStatus.Reconciliation)
                .MandatoryWhen("LegalStatus", (int)LandLegalStatus.Reconciliation),
            RealEstateOtherText("OtherReconciliationForm", "اكتب رقم أو اسم النموذج",
                "Other reconciliation form", SectionLicensing,
                "ReconciliationForm", (int)LandReconciliationForm.Other),

            LookupSelect("OwnershipDocument", "نوع مستند الملكية", "Ownership document",
                    SectionOwnershipDocuments, AdFormLookupKeys.LandOwnershipDocuments, required: true)
                .AsSearchable(),
            RealEstateOtherText("OtherOwnershipDocument", "اسم المستند", "Other ownership document",
                SectionOwnershipDocuments, "OwnershipDocument", (int)LandOwnershipDocument.Other),

            YesNo("HasSurveyPlan", "يوجد رفع مساحي", "Has survey plan", SectionOwnershipDocuments),
            YesNo("HasViolations", "الأرض عليها مخالفات", "Has violations", SectionOwnershipDocuments),
            TextArea("ViolationDetails", "تفاصيل المخالفات", "Violation details",
                    SectionOwnershipDocuments, required: false, maxLength: 4000)
                .VisibleOnlyWhen("HasViolations", true)
                .MandatoryWhen("HasViolations", true),

            RealEstateMultiSelect("Utilities", "المرافق", "Utilities", SectionUtilities,
                AdFormLookupKeys.LandUtilities, LandCatalog.Utilities.Count)
        });

        var conditionalFields = new List<FormFieldDto>();

        conditionalFields.Add(
            LookupSelect("RentType", "نوع الإيجار", "Rent type", SectionRentDetails,
                    AdFormLookupKeys.LandRentTypes, required: false)
                .VisibleOnlyWhen("ListingType", (int)RealEstateListingType.Rent)
                .MandatoryWhen("ListingType", (int)RealEstateListingType.Rent));

        conditionalFields.AddRange(
            RealEstateRentMoneyFields("DownPayment", "متاحة من", "متاحة حتى"));

        conditionalFields.AddRange(new[]
        {
            LookupSelect("MinimumRentPeriod", "الحد الأدنى لمدة الإيجار", "Minimum rent period",
                    SectionRentDetails, AdFormLookupKeys.LandMinimumRentPeriods, required: false)
                .VisibleOnlyWhen("ListingType", (int)RealEstateListingType.Rent),

            RealEstateMultiSelect("RentInclusions", "يشمل الإيجار", "Rent includes", SectionRentDetails,
                    AdFormLookupKeys.LandRentInclusions, LandCatalog.RentInclusions.Count)
                .VisibleOnlyWhen("ListingType", (int)RealEstateListingType.Rent),

            LookupSelect("ContractDuration", "العقد", "Contract duration", SectionRentDetails,
                    AdFormLookupKeys.LandContractDurations, required: false)
                .VisibleOnlyWhen("ListingType", (int)RealEstateListingType.Rent),

            TextArea("OwnerConditions", "شروط المالك", "Owner conditions", SectionRentDetails,
                    required: false, maxLength: 4000,
                    placeholder: "مثال: يمنع البناء. يمنع التأجير من الباطن. النشاط الزراعي فقط.")
                .VisibleOnlyWhen("ListingType", (int)RealEstateListingType.Rent)
        });

        conditionalFields.Add(
            LookupSelect("ExchangeWith", "أرغب بالبدل مع", "Interested in exchanging with",
                    SectionExchangeDetails, AdFormLookupKeys.LandExchangeTargets, required: false)
                .AsSearchable()
                .VisibleOnlyWhen("ListingType", (int)RealEstateListingType.Exchange)
                .MandatoryWhen("ListingType", (int)RealEstateListingType.Exchange));

        conditionalFields.AddRange(new[]
        {
            RealEstateOtherText("OtherExchangeWith", "اكتب المطلوب", "Other exchange target",
                SectionExchangeDetails, "ExchangeWith", (int)LandExchangeWith.Other),

            YesNoWhen("AcceptsDifferencePayment", "يقبل دفع فرق", "Accepts difference payment",
                SectionExchangeDetails, "ListingType", (int)RealEstateListingType.Exchange),

            Number("DifferenceAmount", "قيمة الفرق التقريبية", "Approximate difference amount",
                    SectionExchangeDetails, required: false, min: 0.01m)
                .VisibleOnlyWhen("AcceptsDifferencePayment", true)
                .MandatoryWhen("AcceptsDifferencePayment", true),

            YesNoWhen("ExchangeInSameGovernorateOnly", "البدل داخل نفس المحافظة فقط",
                "Exchange within the same governorate only",
                SectionExchangeDetails, "ListingType", (int)RealEstateListingType.Exchange),

            TextArea("ExchangeDetails", "تفاصيل البدل", "Exchange details", SectionExchangeDetails,
                    required: false, maxLength: 4000)
                .VisibleOnlyWhen("ListingType", (int)RealEstateListingType.Exchange)
        });

        var agricultural = new object[] { (int)LandType.Agricultural, (int)LandType.Reclamation };

        conditionalFields.AddRange(new[]
        {
            YesNoWhen("IsCurrentlyCultivated", "الأرض مزروعة حالياً", "Currently cultivated",
                SectionAgriculturalLand, "LandType", agricultural),

            Text("CurrentCropType", "نوع المحصول الحالي", "Current crop type", SectionAgriculturalLand,
                    required: false, maxLength: 150)
                .VisibleOnlyWhen("IsCurrentlyCultivated", true)
                .MandatoryWhen("IsCurrentlyCultivated", true),
            Number("CultivatedFeddans", "عدد الأفدنة المزروعة", "Cultivated feddans",
                    SectionAgriculturalLand, required: false, min: 0.01m)
                .VisibleOnlyWhen("IsCurrentlyCultivated", true),

            LookupSelect("HarvestSeason", "موسم الحصاد", "Harvest season", SectionAgriculturalLand,
                    AdFormLookupKeys.LandHarvestSeasons, required: false)
                .VisibleOnlyWhen("LandType", agricultural),
            LookupSelect("SoilType", "نوع التربة", "Soil type", SectionAgriculturalLand,
                    AdFormLookupKeys.LandSoilTypes, required: false)
                .VisibleOnlyWhen("LandType", agricultural),
            LookupSelect("IrrigationSource", "مصدر الري", "Irrigation source", SectionAgriculturalLand,
                    AdFormLookupKeys.LandIrrigationSources, required: false)
                .VisibleOnlyWhen("LandType", agricultural),
            RealEstateOtherText("OtherIrrigationSource", "مصدر الري", "Other irrigation source",
                SectionAgriculturalLand, "IrrigationSource", (int)LandIrrigationSource.Other),

            YesNoWhen("HasWell", "يوجد بئر", "Has a well", SectionAgriculturalLand, "LandType", agricultural),
            YesNoWhen("HasIrrigationMachine", "يوجد ماكينة ري", "Has an irrigation machine",
                SectionAgriculturalLand, "LandType", agricultural),
            YesNoWhen("HasIrrigationNetwork", "يوجد شبكة ري", "Has an irrigation network",
                SectionAgriculturalLand, "LandType", agricultural),
            YesNoWhen("HasTrees", "يوجد أشجار", "Has trees", SectionAgriculturalLand, "LandType", agricultural),

            Text("TreeType", "نوع الأشجار", "Tree type", SectionAgriculturalLand,
                    required: false, maxLength: 150)
                .VisibleOnlyWhen("HasTrees", true)
                .MandatoryWhen("HasTrees", true),
            Number("TreesCount", "عدد الأشجار", "Trees count", SectionAgriculturalLand,
                    required: false, min: 1)
                .VisibleOnlyWhen("HasTrees", true),
            Text("TreesAge", "عمر الأشجار", "Trees age", SectionAgriculturalLand,
                    required: false, maxLength: 100)
                .VisibleOnlyWhen("HasTrees", true),

            YesNoWhen("HasFarmHouse", "يوجد منزل بالمزرعة", "Has a farm house",
                SectionAgriculturalLand, "LandType", agricultural),
            YesNoWhen("HasRestHouse", "يوجد استراحة", "Has a rest house",
                SectionAgriculturalLand, "LandType", agricultural),
            YesNoWhen("HasStorage", "يوجد مخزن", "Has storage",
                SectionAgriculturalLand, "LandType", agricultural),
            YesNoWhen("HasResidentWorkers", "يوجد عمالة مقيمة", "Has resident workers",
                SectionAgriculturalLand, "LandType", agricultural),
            YesNoWhen("IsOrganic", "الأرض عضوية (Organic)", "Organic land",
                SectionAgriculturalLand, "LandType", agricultural),
            YesNoWhen("UsesChemicalFertilizers", "تستخدم أسمدة كيميائية", "Uses chemical fertilizers",
                SectionAgriculturalLand, "LandType", agricultural),
            YesNoWhen("HasQualityCertificate", "شهادة جودة زراعية", "Has a quality certificate",
                SectionAgriculturalLand, "LandType", agricultural),

            LookupSelect("QualityCertificate", "نوع الشهادة", "Certificate type", SectionAgriculturalLand,
                    AdFormLookupKeys.LandQualityCertificates, required: false)
                .VisibleOnlyWhen("HasQualityCertificate", true)
                .MandatoryWhen("HasQualityCertificate", true)
        });

        var buildingLand = new object[]
        {
            (int)LandType.Building, (int)LandType.Residential,
            (int)LandType.Commercial, (int)LandType.Industrial
        };

        conditionalFields.AddRange(new[]
        {
            Checkbox("HasFence", "يوجد سور", "Has a fence", SectionBuilding)
                .VisibleOnlyWhen("LandType", agricultural.Concat(buildingLand).ToArray()),

            YesNoWhen("HasGate", "يوجد بوابة", "Has a gate", SectionBuilding, "LandType", buildingLand),
            YesNoWhen("IsLeveledForBuilding", "الأرض ممهدة للبناء", "Leveled for building",
                SectionBuilding, "LandType", buildingLand),
            YesNoWhen("HasFoundations", "يوجد أساسات", "Has foundations",
                SectionBuilding, "LandType", buildingLand),
            YesNoWhen("HasExistingBuilding", "يوجد مبنى قائم", "Has an existing building",
                SectionBuilding, "LandType", buildingLand),

            LookupSelect("ExistingBuildingType", "نوع المبنى", "Existing building type", SectionBuilding,
                    AdFormLookupKeys.LandExistingBuildingTypes, required: false)
                .VisibleOnlyWhen("HasExistingBuilding", true)
                .MandatoryWhen("HasExistingBuilding", true),
            LookupSelect("BuildingCompletionRatio", "نسبة تنفيذ المبنى", "Building completion ratio",
                    SectionBuilding, AdFormLookupKeys.LandBuildingCompletionRatios, required: false)
                .VisibleOnlyWhen("HasExistingBuilding", true)
                .MandatoryWhen("HasExistingBuilding", true),
            Number("CurrentFloorsCount", "عدد الأدوار الحالية", "Current floors count", SectionBuilding,
                    required: false, min: 1)
                .VisibleOnlyWhen("HasExistingBuilding", true),
            Checkbox("CanAddFloors", "إمكانية التعلية", "Can add floors", SectionBuilding)
                .VisibleOnlyWhen("HasExistingBuilding", true)
        });

        return RealEstateSchema("lands", LandsSubmit, propertyFields, conditionalFields,
            "مثال: أرض مباني للبيع بمدينة الفيوم الجديدة",
            "وصف الأرض، مميزاتها، الخدمات القريبة، سبب البيع (اختياري) وأي ملاحظات إضافية.",
            AdFormLookupKeys.LandTypes, AdFormLookupKeys.LandAreaUnits,
            AdFormLookupKeys.LandFacadesCounts, AdFormLookupKeys.LandDirections,
            AdFormLookupKeys.LandRoadTypes, AdFormLookupKeys.LandLegalStatuses,
            AdFormLookupKeys.LandReconciliationForms, AdFormLookupKeys.LandOwnershipDocuments,
            AdFormLookupKeys.LandUtilities, AdFormLookupKeys.LandRentTypes,
            AdFormLookupKeys.LandMinimumRentPeriods, AdFormLookupKeys.LandRentInclusions,
            AdFormLookupKeys.LandContractDurations, AdFormLookupKeys.LandExchangeTargets,
            AdFormLookupKeys.LandHarvestSeasons, AdFormLookupKeys.LandSoilTypes,
            AdFormLookupKeys.LandIrrigationSources, AdFormLookupKeys.LandQualityCertificates,
            AdFormLookupKeys.LandExistingBuildingTypes, AdFormLookupKeys.LandBuildingCompletionRatios);
    }

    private static AdFormSchema Apartments()
    {
        var propertyFields = new List<FormFieldDto>
        {
            LookupSelect("ApartmentType", "نوع الشقة", "Apartment type", SectionApartment,
                    AdFormLookupKeys.ApartmentTypes, required: false)
                .AsSearchable(),
            RealEstateOtherText("OtherApartmentType", "اسم النوع", "Other apartment type",
                SectionApartment, "ApartmentType", (int)ApartmentType.Other),

            LookupSelect("OwnershipType", "نوع الملكية", "Ownership type", SectionApartment,
                AdFormLookupKeys.ApartmentOwnershipTypes, required: false),

            RealEstateSalePrice(),

            RealEstateSalePrice("PricePerMeter", "سعر المتر (جنيه)", "Price per meter"),

            Number("Area", "المساحة (م²)", "Area (m²)", SectionApartment, required: true, min: 0.01m),
            Number("RoomsCount", "عدد الغرف", "Rooms count", SectionApartment, required: false, min: 1),
            Number("BathroomsCount", "عدد الحمامات", "Bathrooms count", SectionApartment,
                required: false, min: 1),

            LookupSelect("ReceptionPieces", "عدد قطع الريسبشن", "Reception pieces", SectionApartment,
                AdFormLookupKeys.ApartmentReceptionPieces, required: false),

            LookupSelect("FloorType", "الدور", "Floor", SectionApartment,
                AdFormLookupKeys.ApartmentFloorTypes, required: true),

            Number("FloorNumber", "رقم الدور", "Floor number", SectionApartment, required: false, min: 0)
                .VisibleOnlyWhen("FloorType", ApartmentFloorTypeValues),
            Number("TotalFloors", "إجمالي عدد الأدوار", "Total floors", SectionApartment,
                    required: false, min: 1)
                .VisibleOnlyWhen("FloorType", ApartmentFloorTypeValues),
            Number("ApartmentsPerFloor", "عدد الشقق في الدور", "Apartments per floor", SectionApartment,
                    required: false, min: 1)
                .VisibleOnlyWhen("FloorType", ApartmentFloorTypeValues),

            YesNo("HasElevator", "يوجد أسانسير", "Has an elevator", SectionApartment),

            LookupSelect("FurnishedStatus", "مفروشة", "Furnished", SectionApartment,
                AdFormLookupKeys.ApartmentFurnishedStatuses, required: false),
            LookupSelect("FinishingType", "التشطيب", "Finishing", SectionApartment,
                AdFormLookupKeys.ApartmentFinishingTypes, required: false),
            LookupSelect("PropertyAge", "عمر العقار", "Property age", SectionApartment,
                AdFormLookupKeys.ApartmentPropertyAges, required: false),
            LookupSelect("Direction", "الاتجاه", "Direction", SectionApartment,
                AdFormLookupKeys.ApartmentDirections, required: false),
            LookupSelect("ViewType", "تطل على", "View", SectionApartment,
                AdFormLookupKeys.ApartmentViewTypes, required: false),

            LookupSelect("LegalStatus", "حالة الشقة", "Legal status", SectionLicensingAndOwnership,
                AdFormLookupKeys.ApartmentLegalStatuses, required: true)
        };

        propertyFields.AddRange(
            RealEstateLicenceFields(SectionLicensingAndOwnership, (int)ApartmentLegalStatus.Licensed));

        propertyFields.AddRange(new[]
        {
            Text("LicenseIssuer", "الجهة المصدرة", "License issuer", SectionLicensingAndOwnership,
                    required: false, maxLength: 150)
                .VisibleOnlyWhen("LegalStatus", (int)ApartmentLegalStatus.Licensed),

            LookupSelect("ReconciliationForm", "نموذج التصالح", "Reconciliation form",
                    SectionLicensingAndOwnership, AdFormLookupKeys.ApartmentReconciliationForms,
                    required: false)
                .VisibleOnlyWhen("LegalStatus", (int)ApartmentLegalStatus.Reconciliation)
                .MandatoryWhen("LegalStatus", (int)ApartmentLegalStatus.Reconciliation),

            LookupSelect("OwnershipDocument", "مستند الملكية", "Ownership document",
                    SectionLicensingAndOwnership, AdFormLookupKeys.ApartmentOwnershipDocuments,
                    required: true)
                .AsSearchable(),

            YesNo("IsRegistered", "مسجلة بالشهر العقاري", "Registered", SectionLicensingAndOwnership),
            YesNo("HasViolations", "عليها مخالفات", "Has violations", SectionLicensingAndOwnership),
            TextArea("ViolationDetails", "تفاصيل المخالفات", "Violation details",
                    SectionLicensingAndOwnership, required: false, maxLength: 4000)
                .VisibleOnlyWhen("HasViolations", true)
                .MandatoryWhen("HasViolations", true),

            RealEstateMultiSelect("Features", "المرافق والمميزات", "Utilities and features",
                SectionUtilitiesAndFeatures, AdFormLookupKeys.ApartmentFeatures,
                ApartmentCatalog.Features.Count)
        });

        var conditionalFields = new List<FormFieldDto>
        {
            LookupSelect("PaymentMethod", "طريقة السداد", "Payment method", SectionSale,
                    AdFormLookupKeys.ApartmentPaymentMethods, required: false)
                .VisibleOnlyWhen("ListingType", (int)RealEstateListingType.Sale)
                .MandatoryWhen("ListingType", (int)RealEstateListingType.Sale),

            Number("DownPayment", "المقدم", "Down payment", SectionSale, required: false, min: 0.01m)
                .VisibleOnlyWhen("PaymentMethod", (int)ApartmentPaymentMethod.Installments)
                .MandatoryWhen("PaymentMethod", (int)ApartmentPaymentMethod.Installments),
            Number("InstallmentAmount", "قيمة القسط", "Installment amount", SectionSale,
                    required: false, min: 0.01m)
                .VisibleOnlyWhen("PaymentMethod", (int)ApartmentPaymentMethod.Installments)
                .MandatoryWhen("PaymentMethod", (int)ApartmentPaymentMethod.Installments),
            Text("InstallmentPeriod", "مدة التقسيط (بالسنوات أو الأشهر)", "Installment period",
                    SectionSale, required: false, maxLength: 100, placeholder: "مثال: 5 سنوات")
                .VisibleOnlyWhen("PaymentMethod", (int)ApartmentPaymentMethod.Installments)
                .MandatoryWhen("PaymentMethod", (int)ApartmentPaymentMethod.Installments),
            LookupSelect("InstallmentProvider", "جهة التقسيط", "Installment provider", SectionSale,
                    AdFormLookupKeys.ApartmentInstallmentProviders, required: false)
                .VisibleOnlyWhen("PaymentMethod", (int)ApartmentPaymentMethod.Installments)
                .MandatoryWhen("PaymentMethod", (int)ApartmentPaymentMethod.Installments),

            YesNoWhen("HasMaintenanceDeposit", "يوجد وديعة صيانة", "Has a maintenance deposit",
                SectionSale, "ListingType", (int)RealEstateListingType.Sale),
            Number("MaintenanceDepositAmount", "قيمة وديعة الصيانة", "Maintenance deposit amount",
                    SectionSale, required: false, min: 0.01m)
                .VisibleOnlyWhen("HasMaintenanceDeposit", true)
                .MandatoryWhen("HasMaintenanceDeposit", true),
            Number("MonthlyFees", "رسوم شهرية (صيانة / اتحاد ملاك)", "Monthly fees", SectionSale,
                    required: false, min: 0)
                .VisibleOnlyWhen("ListingType", (int)RealEstateListingType.Sale),

            LookupSelect("RentType", "نوع الإيجار", "Rent type", SectionRentDetails,
                    AdFormLookupKeys.ApartmentRentTypes, required: false)
                .VisibleOnlyWhen("ListingType", (int)RealEstateListingType.Rent)
                .MandatoryWhen("ListingType", (int)RealEstateListingType.Rent)
        };

        conditionalFields.AddRange(
            RealEstateRentMoneyFields("RentDownPayment", "متاحة من", "متاحة حتى"));

        conditionalFields.AddRange(new[]
        {
            Number("MinimumRentPeriod", "الحد الأدنى لمدة الإيجار", "Minimum rent period",
                    SectionRentDetails, required: false, min: 1)
                .VisibleOnlyWhen("ListingType", (int)RealEstateListingType.Rent),

            RealEstateMultiSelect("RentInclusions", "يشمل الإيجار", "Rent includes", SectionRentDetails,
                    AdFormLookupKeys.ApartmentRentInclusions, ApartmentCatalog.RentInclusions.Count)
                .VisibleOnlyWhen("ListingType", (int)RealEstateListingType.Rent),

            TextArea("OwnerConditions", "شروط المالك", "Owner conditions", SectionRentDetails,
                    required: false, maxLength: 4000)
                .VisibleOnlyWhen("ListingType", (int)RealEstateListingType.Rent)
        });

        conditionalFields.AddRange(
            RealEstateExchangeFields(AdFormLookupKeys.ApartmentExchangeTargets));

        return RealEstateSchema("apartments", ApartmentsSubmit, propertyFields, conditionalFields,
            "مثال: شقة للبيع ببرج حديث",
            "وصف الشقة، مميزاتها، الخدمات القريبة، سبب البيع أو الإيجار وأي ملاحظات إضافية.",

            everyFieldRequired: true, ApartmentOptionalFields,
            AdFormLookupKeys.ApartmentTypes, AdFormLookupKeys.ApartmentOwnershipTypes,
            AdFormLookupKeys.ApartmentReceptionPieces, AdFormLookupKeys.ApartmentFloorTypes,
            AdFormLookupKeys.ApartmentFurnishedStatuses, AdFormLookupKeys.ApartmentFinishingTypes,
            AdFormLookupKeys.ApartmentPropertyAges, AdFormLookupKeys.ApartmentDirections,
            AdFormLookupKeys.ApartmentViewTypes, AdFormLookupKeys.ApartmentLegalStatuses,
            AdFormLookupKeys.ApartmentReconciliationForms, AdFormLookupKeys.ApartmentOwnershipDocuments,
            AdFormLookupKeys.ApartmentFeatures, AdFormLookupKeys.ApartmentPaymentMethods,
            AdFormLookupKeys.ApartmentInstallmentProviders, AdFormLookupKeys.ApartmentRentTypes,
            AdFormLookupKeys.ApartmentRentInclusions,
            AdFormLookupKeys.ApartmentExchangeTargets);
    }

    private static readonly object[] ApartmentFloorTypeValues =
        Enum.GetValues<ApartmentFloorType>()
            .Where(value => value != ApartmentFloorType.Basement)
            .Select(value => (object)(int)value)
            .ToArray();

    private static readonly HashSet<string> ApartmentOptionalFields =
    [

        "Video",

        "ViewType",

        "LicenseNumber", "LicenseIssueDate", "LicenseExpiryDate", "LicenseIssuer",
        "IsRegistered", "HasViolations",

        "Negotiable",

        "Features", "District", "GoogleMaps", "WhatsApp", "Email", "Notes"
    ];

    private static AdFormSchema Shops()
    {
        var propertyFields = new List<FormFieldDto>
        {
            LookupSelect("SuitableActivity", "نوع النشاط المناسب", "Suitable activity", SectionShop,
                    AdFormLookupKeys.ShopSuitableActivities, required: false)
                .AsSearchable(),
            RealEstateOtherText("OtherSuitableActivity", "اسم النشاط", "Other activity",
                SectionShop, "SuitableActivity", (int)ShopSuitableActivity.Other),

            RealEstateSalePrice(),

            Number("Area", "المساحة (م²)", "Area (m²)", SectionShop, required: true, min: 0.01m),

            LookupSelect("FloorType", "الدور", "Floor", SectionShop,
                AdFormLookupKeys.ShopFloorTypes, required: true),

            Number("CeilingHeight", "ارتفاع السقف (م)", "Ceiling height (m)", SectionShop,
                required: false, min: 0.01m),
            Number("FacadeWidth", "عرض الواجهة (م)", "Facade width (m)", SectionShop,
                required: false, min: 0.01m),

            LookupSelect("FacadesCount", "عدد الواجهات", "Facades count", SectionShop,
                AdFormLookupKeys.ShopFacadesCounts, required: false),
            LookupSelect("FacadeDirection", "اتجاه الواجهة", "Facade direction", SectionShop,
                AdFormLookupKeys.ShopFacadeDirections, required: false),
            LookupSelect("FinishingType", "التشطيب", "Finishing", SectionShop,
                AdFormLookupKeys.ShopFinishingTypes, required: false),
            LookupSelect("PropertyAge", "عمر العقار", "Property age", SectionShop,
                AdFormLookupKeys.ShopPropertyAges, required: false),

            YesNo("HasBathroom", "يوجد حمام", "Has a bathroom", SectionShop),
            Number("BathroomsCount", "عدد الحمامات", "Bathrooms count", SectionShop,
                    required: false, min: 1)
                .VisibleOnlyWhen("HasBathroom", true)
                .MandatoryWhen("HasBathroom", true),

            YesNo("HasStorage", "يوجد مخزن", "Has storage", SectionShop),
            Number("StorageArea", "مساحة المخزن", "Storage area", SectionShop,
                    required: false, min: 0.01m)
                .VisibleOnlyWhen("HasStorage", true)
                .MandatoryWhen("HasStorage", true),

            YesNo("HasGlassFacade", "واجهة زجاج", "Glass facade", SectionShop),
            YesNo("SuitableForRestaurantOrCafe", "يصلح لمطعم أو كافيه",
                "Suitable for a restaurant or cafe", SectionShop),
            YesNo("HasExtractorFan", "يوجد شفاط", "Has an extractor fan", SectionShop),
            YesNo("HasPrivateEntrance", "يوجد مدخل خاص", "Has a private entrance", SectionShop),

            LookupSelect("EntrancesCount", "عدد المداخل", "Entrances count", SectionShop,
                AdFormLookupKeys.ShopEntrancesCounts, required: false),

            LookupSelect("LegalStatus", "حالة المحل", "Legal status", SectionLicensingAndOwnership,
                AdFormLookupKeys.ShopLegalStatuses, required: true),

            Text("LicenseNumber", "رقم الرخصة", "License number", SectionLicensingAndOwnership,
                    required: false, maxLength: 100)
                .VisibleOnlyWhen("LegalStatus", (int)ShopLegalStatus.Licensed),

            LookupSelect("LicenseType", "نوع الرخصة", "License type", SectionLicensingAndOwnership,
                    AdFormLookupKeys.ShopLicenseTypes, required: false)
                .VisibleOnlyWhen("LegalStatus", (int)ShopLegalStatus.Licensed)
                .MandatoryWhen("LegalStatus", (int)ShopLegalStatus.Licensed),

            Text("LicenseIssuer", "الجهة المصدرة", "License issuer", SectionLicensingAndOwnership,
                    required: false, maxLength: 150)
                .VisibleOnlyWhen("LegalStatus", (int)ShopLegalStatus.Licensed),

            DatePicker("LicenseIssueDate", "تاريخ إصدار الرخصة", "License issue date",
                    SectionLicensingAndOwnership, required: false)
                .VisibleOnlyWhen("LegalStatus", (int)ShopLegalStatus.Licensed),
            DatePicker("LicenseExpiryDate", "تاريخ انتهاء الرخصة", "License expiry date",
                    SectionLicensingAndOwnership, required: false)
                .VisibleOnlyWhen("LegalStatus", (int)ShopLegalStatus.Licensed),

            LookupSelect("ReconciliationForm", "نموذج التصالح", "Reconciliation form",
                    SectionLicensingAndOwnership, AdFormLookupKeys.ShopReconciliationForms,
                    required: false)
                .VisibleOnlyWhen("LegalStatus", (int)ShopLegalStatus.Reconciliation)
                .MandatoryWhen("LegalStatus", (int)ShopLegalStatus.Reconciliation),
            RealEstateOtherText("OtherReconciliationForm", "اسم النموذج", "Other reconciliation form",
                SectionLicensingAndOwnership, "ReconciliationForm", (int)ShopReconciliationForm.Other),

            LookupSelect("OwnershipDocument", "مستند الملكية", "Ownership document",
                SectionLicensingAndOwnership, AdFormLookupKeys.ShopOwnershipDocuments, required: true),
            RealEstateOtherText("OtherOwnershipDocument", "اسم المستند", "Other ownership document",
                SectionLicensingAndOwnership, "OwnershipDocument", (int)ShopOwnershipDocument.Other),

            YesNo("IsRegistered", "مسجل بالشهر العقاري", "Registered", SectionLicensingAndOwnership),

            RealEstateMultiSelect("Utilities", "المرافق", "Utilities", SectionUtilities,
                AdFormLookupKeys.ShopUtilities, ShopCatalog.Utilities.Count),

            YesNo("WasPreviouslyOperating", "كان يعمل سابقًا؟", "Was previously operating", SectionActivity),
            Text("PreviousActivity", "النشاط السابق", "Previous activity", SectionActivity,
                    required: false, maxLength: 150)
                .VisibleOnlyWhen("WasPreviouslyOperating", true)
                .MandatoryWhen("WasPreviouslyOperating", true),
            Text("PreviousOperatingPeriod", "مدة التشغيل السابقة", "Previous operating period",
                    SectionActivity, required: false, maxLength: 100)
                .VisibleOnlyWhen("WasPreviouslyOperating", true),
            TextArea("VacancyReason", "سبب الإخلاء", "Vacancy reason", SectionActivity,
                    required: false, maxLength: 1000)
                .VisibleOnlyWhen("WasPreviouslyOperating", true)
        };

        var conditionalFields = new List<FormFieldDto>
        {
            LookupSelect("PaymentMethod", "طريقة السداد", "Payment method", SectionSale,
                    AdFormLookupKeys.ShopPaymentMethods, required: false)
                .VisibleOnlyWhen("ListingType", (int)RealEstateListingType.Sale)
                .MandatoryWhen("ListingType", (int)RealEstateListingType.Sale),

            Number("DownPayment", "قيمة المقدم", "Down payment", SectionSale, required: false, min: 0.01m)
                .VisibleOnlyWhen("PaymentMethod", (int)ShopPaymentMethod.Installments)
                .MandatoryWhen("PaymentMethod", (int)ShopPaymentMethod.Installments),
            Text("InstallmentPeriod", "مدة التقسيط", "Installment period", SectionSale,
                    required: false, maxLength: 100, placeholder: "مثال: 3 سنوات")
                .VisibleOnlyWhen("PaymentMethod", (int)ShopPaymentMethod.Installments)
                .MandatoryWhen("PaymentMethod", (int)ShopPaymentMethod.Installments),
            Number("InstallmentAmount", "قيمة القسط", "Installment amount", SectionSale,
                    required: false, min: 0.01m)
                .VisibleOnlyWhen("PaymentMethod", (int)ShopPaymentMethod.Installments)
                .MandatoryWhen("PaymentMethod", (int)ShopPaymentMethod.Installments),
            LookupSelect("InstallmentProvider", "جهة التقسيط", "Installment provider", SectionSale,
                    AdFormLookupKeys.ShopInstallmentProviders, required: false)
                .VisibleOnlyWhen("PaymentMethod", (int)ShopPaymentMethod.Installments)
                .MandatoryWhen("PaymentMethod", (int)ShopPaymentMethod.Installments),

            LookupSelect("RentType", "نوع الإيجار", "Rent type", SectionRentDetails,
                    AdFormLookupKeys.ShopRentTypes, required: false)
                .VisibleOnlyWhen("ListingType", (int)RealEstateListingType.Rent)
                .MandatoryWhen("ListingType", (int)RealEstateListingType.Rent)
        };

        conditionalFields.AddRange(
            RealEstateRentMoneyFields("RentDownPayment", "متاح من", "متاح حتى"));

        conditionalFields.AddRange(new[]
        {
            Number("MinimumRentPeriod", "الحد الأدنى لمدة الإيجار", "Minimum rent period",
                    SectionRentDetails, required: false, min: 1)
                .VisibleOnlyWhen("ListingType", (int)RealEstateListingType.Rent),

            RealEstateMultiSelect("RentInclusions", "يشمل الإيجار", "Rent includes", SectionRentDetails,
                    AdFormLookupKeys.ShopRentInclusions, ShopCatalog.RentInclusions.Count)
                .VisibleOnlyWhen("ListingType", (int)RealEstateListingType.Rent),

            YesNoWhen("AllowsActivityChange", "يسمح بتغيير النشاط", "Allows changing the activity",
                SectionRentDetails, "ListingType", (int)RealEstateListingType.Rent),

            TextArea("OwnerConditions", "شروط المالك", "Owner conditions", SectionRentDetails,
                    required: false, maxLength: 4000)
                .VisibleOnlyWhen("ListingType", (int)RealEstateListingType.Rent)
        });

        conditionalFields.AddRange(RealEstateExchangeFields(AdFormLookupKeys.ShopExchangeTargets));

        return RealEstateSchema("shops", ShopsSubmit, propertyFields, conditionalFields,
            "مثال: محل للبيع بشارع الحرية",
            "وصف المحل، موقعه، النشاط المناسب له، المميزات وأي تفاصيل إضافية.",
            AdFormLookupKeys.ShopSuitableActivities, AdFormLookupKeys.ShopFloorTypes,
            AdFormLookupKeys.ShopFacadesCounts, AdFormLookupKeys.ShopFacadeDirections,
            AdFormLookupKeys.ShopFinishingTypes, AdFormLookupKeys.ShopPropertyAges,
            AdFormLookupKeys.ShopEntrancesCounts, AdFormLookupKeys.ShopLegalStatuses,
            AdFormLookupKeys.ShopLicenseTypes, AdFormLookupKeys.ShopReconciliationForms,
            AdFormLookupKeys.ShopOwnershipDocuments, AdFormLookupKeys.ShopUtilities,
            AdFormLookupKeys.ShopPaymentMethods, AdFormLookupKeys.ShopInstallmentProviders,
            AdFormLookupKeys.ShopRentTypes, AdFormLookupKeys.ShopRentInclusions,
            AdFormLookupKeys.ShopExchangeTargets);
    }

    private static IReadOnlyList<FormFieldDto> Order(List<FormFieldDto> fields)
    {
        for (var index = 0; index < fields.Count; index++)
            fields[index].Order = index + 1;

        return fields;
    }
}
