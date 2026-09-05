using Shared.Constants;
using Shared.DTOs.Lookups.Forms;
using Shared.DTOs.Lookups.Read;
using Shared.Enums;
using static Services.ReadConfigs.ReadOperationFactory;

namespace Services.ReadConfigs;

public static class ReadConfigCatalog
{
    private const string AdvertisementsModule = "advertisements";
    private const string WorkshopsModule = "workshops";
    private const string CraftsmenModule = "craftsmen";
    private const string LostFoundModule = "lostFound";
    private const string LookupsModule = "lookups";

    private const string FactoriesModule = "factories";
    private const string FarmsModule = "farms";
    private const string CompaniesModule = "companies";
    private const string SuppliersModule = "suppliers";
    private const string WholesaleTradersModule = "wholesaleTraders";
    private const string FruitMerchantsModule = "fruitVegetableMerchants";

    private const string FactoriesRoute = "/api/factories";
    private const string FarmsRoute = "/api/farms";
    private const string CompaniesRoute = "/api/companies";
    private const string SuppliersRoute = "/api/suppliers";
    private const string WholesaleTradersRoute = "/api/wholesale-traders";
    private const string FruitMerchantsRoute = "/api/fruit-vegetable-merchants";

    private const string JobRequestsModule = "jobRequests";
    private const string JobOpportunitiesModule = "jobOpportunities";

    private const string JobRequestsRoute = "/api/job-requests";
    private const string JobOpportunitiesRoute = "/api/job-opportunities";

    private const string LivestockModule = "livestock";
    private const string SheepGoatModule = "sheepGoats";
    private const string HorseModule = "horses";
    private const string CamelModule = "camels";
    private const string BirdModule = "birds";
    private const string PetModule = "pets";
    private const string FishModule = "fish";
    private const string BeeModule = "bees";
    private const string OtherAnimalModule = "otherAnimals";

    private const string LivestockRoute = "/api/livestock";
    private const string SheepGoatRoute = "/api/sheep-goats";
    private const string HorseRoute = "/api/horses";
    private const string CamelRoute = "/api/camels";
    private const string BirdRoute = "/api/birds";
    private const string PetRoute = "/api/pets";
    private const string FishRoute = "/api/fish";
    private const string BeeRoute = "/api/bees";
    private const string OtherAnimalRoute = "/api/other-animals";

    private const string DecorAntiqueModule = "decorAntiques";
    private const string AntiqueModuleName = "antiques";
    private const string PaintingModule = "paintings";
    private const string HandmadeModule = "handmade";
    private const string CoinStampModule = "coinsStamps";

    private const string DecorAntiqueRoute = "/api/decor-antiques";
    private const string AntiqueRoute = "/api/antiques";
    private const string PaintingRoute = "/api/paintings";
    private const string HandmadeRoute = "/api/handmade";
    private const string CoinStampRoute = "/api/coins-stamps";

    private const string MenClothingModule = "menClothing";
    private const string WomenClothingModule = "womenClothing";
    private const string KidsClothingModule = "kidsClothing";

    private const string MenClothingRoute = "/api/men-clothing";
    private const string WomenClothingRoute = "/api/women-clothing";
    private const string KidsClothingRoute = "/api/kids-clothing";

    private const string LandsModule = "lands";
    private const string ApartmentsModule = "apartments";
    private const string ShopsModule = "shops";

    private const string LandsRoute = "/api/lands";
    private const string ApartmentsRoute = "/api/apartments";
    private const string ShopsRoute = "/api/shops";

    private const string RescuesModule = "rescues";
    private const string BloodRequestsModule = "bloodRequests";
    private const string AskConsultsModule = "askConsults";

    private const string RescuesRoute = "/api/rescues";
    private const string BloodRequestsRoute = "/api/blood-requests";
    private const string AskConsultsRoute = "/api/ask-consults";

    private const string AccessoriesModule = "accessories";
    private const string CosmeticsModule = "cosmetics";
    private const string HomeKitchenModule = "homeKitchen";
    private const string ShoppingElectronicsModule = "shoppingElectronics";
    private const string GiftsToysModule = "giftsToys";
    private const string HomemadeFoodModule = "homemadeFood";

    private const string AccessoriesRoute = "/" + OnlineShoppingRoutes.Accessories;
    private const string CosmeticsRoute = "/" + OnlineShoppingRoutes.Cosmetics;
    private const string HomeKitchenRoute = "/" + OnlineShoppingRoutes.HomeKitchen;
    private const string ShoppingElectronicsRoute = "/" + OnlineShoppingRoutes.ShoppingElectronics;
    private const string GiftsToysRoute = "/" + OnlineShoppingRoutes.GiftsToys;
    private const string HomemadeFoodRoute = "/" + OnlineShoppingRoutes.HomemadeFood;

    private const string FurnitureModule = "furniture";
    private const string FurnishingCurtainModule = "furnishingsCurtains";
    private const string LightingDecorModule = "lightingDecor";
    private const string KitchenToolModule = "kitchenTools";
    private const string HomeApplianceModule = "homeAppliances";
    private const string BathroomSupplyModule = "bathroomSupplies";
    private const string PlantOrnamentModule = "plantsOrnaments";

    private const string FurnitureRoute = "/" + HomeFurnishingRoutes.Furniture;
    private const string FurnishingCurtainRoute = "/" + HomeFurnishingRoutes.FurnishingCurtains;
    private const string LightingDecorRoute = "/" + HomeFurnishingRoutes.LightingDecor;
    private const string KitchenToolRoute = "/" + HomeFurnishingRoutes.KitchenTools;
    private const string HomeApplianceRoute = "/" + HomeFurnishingRoutes.HomeAppliances;
    private const string BathroomSupplyRoute = "/" + HomeFurnishingRoutes.BathroomSupplies;
    private const string PlantOrnamentRoute = "/" + HomeFurnishingRoutes.PlantsOrnaments;

    public static ReadConfigSchema? GetSchema(int categoryId, int? subCategoryId)
    {
        if (subCategoryId is { } id && Enum.IsDefined(typeof(SubCategoryType), id))
            return GetSubCategorySchema(categoryId, (SubCategoryType)id);

        return categoryId switch
        {
            _ => null
        };
    }

    private static ReadConfigSchema? GetSubCategorySchema(int categoryId, SubCategoryType subCategory) =>
        subCategory switch
        {
            SubCategoryType.Private or SubCategoryType.Taxi or SubCategoryType.Motorcycles
                or SubCategoryType.HeavyEquipment =>
                Advertisements(categoryId, subCategory, vehicleFilters: true),

            SubCategoryType.Workshops => Workshops(categoryId, subCategory),
            SubCategoryType.Craftsmen => Craftsmen(categoryId, subCategory),

            SubCategoryType.LostItems => LostFound(categoryId, subCategory, PostType.Lost),
            SubCategoryType.FoundItems => LostFound(categoryId, subCategory, PostType.Found),

            SubCategoryType.Factories => Factories(categoryId, subCategory),
            SubCategoryType.Farms => Farms(categoryId, subCategory),
            SubCategoryType.Companies => Companies(categoryId, subCategory),
            SubCategoryType.Suppliers => Suppliers(categoryId, subCategory),
            SubCategoryType.WholesaleTraders => WholesaleTraders(categoryId, subCategory),
            SubCategoryType.FruitAndVegetableTraders => FruitMerchants(categoryId, subCategory),

            SubCategoryType.JobRequests => JobRequests(categoryId, subCategory),
            SubCategoryType.JobOpportunities => JobOpportunities(categoryId, subCategory),

            SubCategoryType.Livestock => Livestock(categoryId, subCategory),
            SubCategoryType.SheepAndGoats => SheepGoat(categoryId, subCategory),
            SubCategoryType.Horses => Horse(categoryId, subCategory),
            SubCategoryType.Camels => Camel(categoryId, subCategory),
            SubCategoryType.Birds => Bird(categoryId, subCategory),
            SubCategoryType.Pets => Pet(categoryId, subCategory),
            SubCategoryType.Fish => Fish(categoryId, subCategory),
            SubCategoryType.Bees => Bee(categoryId, subCategory),
            SubCategoryType.OtherAnimals => OtherAnimal(categoryId, subCategory),

            SubCategoryType.DecorAntiques => DecorAntiques(categoryId, subCategory),
            SubCategoryType.Antiques => Antiques(categoryId, subCategory),
            SubCategoryType.Paintings => Paintings(categoryId, subCategory),
            SubCategoryType.Handmade => Handmade(categoryId, subCategory),
            SubCategoryType.CoinsAndStamps => CoinsStamps(categoryId, subCategory),

            SubCategoryType.MenClothing => MenClothing(categoryId, subCategory),
            SubCategoryType.WomenClothing => WomenClothing(categoryId, subCategory),
            SubCategoryType.KidsClothing => KidsClothing(categoryId, subCategory),

            SubCategoryType.Accessories => Accessories(categoryId, subCategory),
            SubCategoryType.Cosmetics => Cosmetics(categoryId, subCategory),
            SubCategoryType.HomeAndKitchen => HomeKitchen(categoryId, subCategory),
            SubCategoryType.ShoppingElectronics => ShoppingElectronics(categoryId, subCategory),
            SubCategoryType.GiftsAndToys => GiftsToys(categoryId, subCategory),
            SubCategoryType.HomemadeFood => HomemadeFood(categoryId, subCategory),

            SubCategoryType.Furniture => Furniture(categoryId, subCategory),
            SubCategoryType.FurnishingsAndCurtains => FurnishingCurtains(categoryId, subCategory),
            SubCategoryType.LightingAndDecor => LightingDecor(categoryId, subCategory),
            SubCategoryType.KitchenTools => KitchenTools(categoryId, subCategory),
            SubCategoryType.HomeAppliances => HomeAppliances(categoryId, subCategory),
            SubCategoryType.BathroomSupplies => BathroomSupplies(categoryId, subCategory),
            SubCategoryType.PlantsAndOrnaments => PlantsOrnaments(categoryId, subCategory),

            SubCategoryType.Lands => Lands(categoryId, subCategory),
            SubCategoryType.Apartments => Apartments(categoryId, subCategory),
            SubCategoryType.Shops => Shops(categoryId, subCategory),

            SubCategoryType.Rescues => Rescues(categoryId, subCategory),
            SubCategoryType.BloodRequests => BloodRequests(categoryId, subCategory),
            SubCategoryType.AskConsults => AskConsults(categoryId, subCategory),

            _ => null
        };

    public static ReadConfigSchema SubCategorySelectionSchema(int categoryId) =>
        new(LookupsModule,
        [
            SubCategories(categoryId),
            Categories(),
            CategoriesTree(),
            ReadConfig(categoryId, subCategoryId: null)
        ]);

    private static ReadConfigSchema Advertisements(
        int categoryId, SubCategoryType subCategory, bool vehicleFilters)
    {
        var selection = Selection(categoryId, (int)subCategory);
        var filters = AdvertisementFilters(categoryId, subCategory, vehicleFilters).ToList();

        var operations = new List<ReadOperationDto>
        {
            Operation(ReadOperationKeys.List, "قائمة الإعلانات", "Advertisements list",
                AdsRoute, paginated: true, queryParameters: filters, query: selection),

            Operation(ReadOperationKeys.Details, "تفاصيل الإعلان", "Advertisement details",
                $"{AdsRoute}/{{id}}", routeParameters: IdRoute()),

            Operation(ReadOperationKeys.MyListings, "إعلاناتي", "My advertisements",
                $"{AdsRoute}/my", requiresAuthentication: true, paginated: true,
                queryParameters: filters, query: selection),

            Operation(ReadOperationKeys.Statistics, "إحصائيات إعلاناتي", "My advertisement statistics",
                $"{AdsRoute}/my/statistics", requiresAuthentication: true)
        };

        if (vehicleFilters)
        {
            operations.Add(ListingTypes());
            operations.Add(Features());
        }

        operations.AddRange(SharedTail(categoryId, (int)subCategory));

        return new ReadConfigSchema(AdvertisementsModule, operations);
    }

    private static IEnumerable<ReadParameterDto> AdvertisementFilters(
        int categoryId, SubCategoryType subCategory, bool vehicleFilters)
    {
        var subCategoryId = (int)subCategory;

        yield return Param("search", "بحث", "Search", ReadParameterTypes.String);

        yield return Param("sortBy", "الترتيب", "Sort by", ReadParameterTypes.Enum,
            defaultValue: (int)AdvertisementSortBy.Newest,
            options: EnumOptions(
                (AdvertisementSortBy.Newest, "الأحدث", "Newest"),
                (AdvertisementSortBy.Oldest, "الأقدم", "Oldest"),
                (AdvertisementSortBy.PriceAsc, "السعر: من الأقل للأعلى", "Price: low to high"),
                (AdvertisementSortBy.PriceDesc, "السعر: من الأعلى للأقل", "Price: high to low"),
                (AdvertisementSortBy.MostViewed, "الأكثر مشاهدة", "Most viewed")));

        yield return Param("categoryId", "القسم الرئيسي", "Category", ReadParameterTypes.Integer,
            defaultValue: categoryId, optionsSource: ReadOperationKeys.Categories,
            optionsValue: ReadOptionsValues.Id);

        yield return Param("subCategoryId", "القسم الفرعي", "Sub category", ReadParameterTypes.Integer,
            defaultValue: subCategoryId, optionsSource: ReadOperationKeys.SubCategories,
            optionsValue: ReadOptionsValues.Id);

        if (vehicleFilters)
        {
            yield return Param("listingType", "نوع الإعلان", "Listing type", ReadParameterTypes.Enum,
                optionsSource: ReadOperationKeys.ListingTypes, optionsValue: ReadOptionsValues.Id);

            yield return Param("brand", "الماركة", "Brand", ReadParameterTypes.String);
            yield return Param("model", "الموديل", "Model", ReadParameterTypes.String);
            yield return Param("color", "اللون", "Color", ReadParameterTypes.String);

            var minYear = CarCatalog.MinManufacturingYear;
            var maxYear = CarCatalog.MaxManufacturingYear;

            yield return Param("year", "سنة الصنع", "Manufacturing year", ReadParameterTypes.Integer,
                min: minYear, max: maxYear);
            yield return Param("yearFrom", "سنة الصنع من", "Year from", ReadParameterTypes.Integer,
                min: minYear, max: maxYear);
            yield return Param("yearTo", "سنة الصنع إلى", "Year to", ReadParameterTypes.Integer,
                min: minYear, max: maxYear);

            yield return Param("condition", "الحالة", "Condition", ReadParameterTypes.Enum,
                options: EnumOptions(ConditionNamesOf(subCategory)));

            yield return Param("technicalCondition", "الحالة الفنية", "Technical condition",
                ReadParameterTypes.Enum, options: EnumOptions(CarCatalog.TechnicalConditionNames));

            yield return Param("fuelType", "نوع الوقود", "Fuel type", ReadParameterTypes.Enum,
                options: EnumOptions(FuelTypeNamesOf(subCategory)));

            if (subCategory is not SubCategoryType.HeavyEquipment)
            {
                yield return Param("transmission", "ناقل الحركة", "Transmission", ReadParameterTypes.Enum,
                    options: EnumOptions(TransmissionNamesOf(subCategory)));
            }

            foreach (var typeFilter in VehicleTypeFilterOf(subCategory))
                yield return typeFilter;

            yield return Param("originCountry", "بلد المنشأ", "Origin country", ReadParameterTypes.Enum,
                options: EnumOptions(OriginCountryNamesOf(subCategory)));

            yield return Param("licenseStatus", "حالة الرخصة", "License status", ReadParameterTypes.Enum,
                options: EnumOptions(LicenseStatusNamesOf(subCategory)));

            yield return Param("kilometersTo", "أقصى عدد كيلومترات", "Max kilometers",
                ReadParameterTypes.Integer, min: 0);

            yield return Param("featureIds", "المميزات", "Features", ReadParameterTypes.Integer,
                multiple: true, optionsSource: ReadOperationKeys.Features,
                optionsValue: ReadOptionsValues.Id);

            yield return Param("negotiable", "السعر قابل للتفاوض", "Negotiable", ReadParameterTypes.Boolean);
        }

        foreach (var location in LocationFilters("center"))
            yield return location;

        yield return Param("priceFrom", "السعر من", "Price from", ReadParameterTypes.Decimal, min: 0);
        yield return Param("priceTo", "السعر إلى", "Price to", ReadParameterTypes.Decimal, min: 0);

        foreach (var paging in Paging())
            yield return paging;
    }

    private static IReadOnlyDictionary<VehicleCondition, string> ConditionNamesOf(
        SubCategoryType subCategory) =>
        subCategory is SubCategoryType.Motorcycles
            ? CarCatalog.MotorcycleConditionNames
            : CarCatalog.ConditionNames;

    private static IReadOnlyDictionary<FuelType, string> FuelTypeNamesOf(SubCategoryType subCategory) =>
        subCategory switch
        {
            SubCategoryType.Taxi => CarCatalog.TaxiFuelTypeNames,
            SubCategoryType.Motorcycles => CarCatalog.MotorcycleFuelTypeNames,
            SubCategoryType.HeavyEquipment => CarCatalog.EquipmentFuelTypeNames,
            _ => CarCatalog.FuelTypeNames
        };

    private static IReadOnlyDictionary<TransmissionType, string> TransmissionNamesOf(
        SubCategoryType subCategory) =>
        subCategory is SubCategoryType.Motorcycles
            ? CarCatalog.MotorcycleTransmissionNames
            : CarCatalog.TransmissionNames;

    private static IReadOnlyDictionary<VehicleOriginCountry, string> OriginCountryNamesOf(
        SubCategoryType subCategory) =>
        subCategory switch
        {
            SubCategoryType.Taxi => CarCatalog.TaxiOriginCountryNames,
            SubCategoryType.Motorcycles => CarCatalog.MotorcycleOriginCountryNames,
            SubCategoryType.HeavyEquipment => CarCatalog.EquipmentOriginCountryNames,
            _ => CarCatalog.OriginCountryNames
        };

    private static IReadOnlyDictionary<LicenseStatus, string> LicenseStatusNamesOf(
        SubCategoryType subCategory) =>
        subCategory switch
        {
            SubCategoryType.Motorcycles => CarCatalog.MotorcycleLicenseStatusNames,
            SubCategoryType.HeavyEquipment => CarCatalog.EquipmentLicenseStatusNames,
            _ => CarCatalog.LicenseStatusNames
        };

    private static IEnumerable<ReadParameterDto> VehicleTypeFilterOf(SubCategoryType subCategory)
    {
        switch (subCategory)
        {
            case SubCategoryType.Private:
                yield return Param("bodyType", "نوع الهيكل", "Body type", ReadParameterTypes.Enum,
                    options: EnumOptions(CarCatalog.BodyTypeNames));
                break;

            case SubCategoryType.Taxi:
                yield return Param("vehicleType", "نوع المركبة", "Vehicle type", ReadParameterTypes.Enum,
                    options: EnumOptions(CarCatalog.TaxiVehicleTypeNames));
                break;

            case SubCategoryType.Motorcycles:
                yield return Param("motorcycleType", "النوع", "Type", ReadParameterTypes.Enum,
                    options: EnumOptions(CarCatalog.MotorcycleTypeNames));
                break;

            case SubCategoryType.HeavyEquipment:
                yield return Param("machineType", "نوع المعدة", "Machine type", ReadParameterTypes.Enum,
                    options: EnumOptions(CarCatalog.MachineTypeNames));
                break;
        }
    }

    private static ReadConfigSchema BusinessModule(
        int categoryId, SubCategoryType subCategory, string module, string route,
        string listLabelAr, string listLabelEn, string detailsLabelAr, string detailsLabelEn,
        ListingModuleType listingType, string myListingsAr, string myListingsEn,
        IEnumerable<ReadParameterDto> filters, IEnumerable<ReadOperationDto> lookupOperations)
    {
        var queryParameters = filters.ToList();
        queryParameters.Add(Param("search", "بحث", "Search", ReadParameterTypes.String));
        queryParameters.AddRange(Paging());

        var operations = new List<ReadOperationDto>
        {
            Operation(ReadOperationKeys.List, listLabelAr, listLabelEn,
                route, paginated: true, queryParameters: queryParameters),

            Operation(ReadOperationKeys.Details, detailsLabelAr, detailsLabelEn,
                $"{route}/{{id}}", routeParameters: IdRoute()),

            UnifiedMyListings(listingType, myListingsAr, myListingsEn)
        };

        operations.AddRange(lookupOperations);
        operations.AddRange(SharedTail(categoryId, (int)subCategory));

        return new ReadConfigSchema(module, operations);
    }

    private static ReadConfigSchema Factories(int categoryId, SubCategoryType subCategory) =>
        BusinessModule(categoryId, subCategory, FactoriesModule, FactoriesRoute,
            "قائمة المصانع", "Factories list", "تفاصيل المصنع", "Factory details",
            ListingModuleType.Factory, "المصانع المضافة بواسطتي", "My factories",
            [
                Param("factoryName", "اسم المصنع", "Factory name", ReadParameterTypes.String),
                Param("productionSpecialty", "تخصص الإنتاج", "Production specialty",
                    ReadParameterTypes.Enum,
                    optionsSource: "productionSpecialties", optionsValue: ReadOptionsValues.Id)
            ],
            [
                Operation("productionSpecialties", "تخصصات الإنتاج", "Production specialties",
                    $"{FactoriesRoute}/production-specialties")
            ]);

    private static ReadConfigSchema Farms(int categoryId, SubCategoryType subCategory) =>
        BusinessModule(categoryId, subCategory, FarmsModule, FarmsRoute,
            "قائمة المزارع", "Farms list", "تفاصيل المزرعة", "Farm details",
            ListingModuleType.Farm, "المزارع المضافة بواسطتي", "My farms",
            [
                Param("farmName", "اسم المزرعة", "Farm name", ReadParameterTypes.String),
                Param("farmType", "نوع المزرعة", "Farm type", ReadParameterTypes.Enum,
                    optionsSource: "farmTypes", optionsValue: ReadOptionsValues.Id)
            ],
            [
                Operation("farmTypes", "أنواع المزارع", "Farm types", $"{FarmsRoute}/types"),
                Operation("farmingMethods", "أساليب الزراعة", "Farming methods",
                    $"{FarmsRoute}/farming-methods"),
                Operation("availabilitySeasons", "مواسم التوفر", "Availability seasons",
                    $"{FarmsRoute}/availability-seasons")
            ]);

    private static ReadConfigSchema Companies(int categoryId, SubCategoryType subCategory) =>
        BusinessModule(categoryId, subCategory, CompaniesModule, CompaniesRoute,
            "قائمة الشركات", "Companies list", "تفاصيل الشركة", "Company details",
            ListingModuleType.Company, "الشركات المضافة بواسطتي", "My companies",
            [
                Param("companyName", "اسم الشركة", "Company name", ReadParameterTypes.String),
                Param("companyField", "مجال الشركة", "Company field", ReadParameterTypes.Enum,
                    optionsSource: "companyFields", optionsValue: ReadOptionsValues.Id)
            ],
            [
                Operation("companyFields", "مجالات الشركات", "Company fields",
                    $"{CompaniesRoute}/fields")
            ]);

    private static ReadConfigSchema Suppliers(int categoryId, SubCategoryType subCategory) =>
        BusinessModule(categoryId, subCategory, SuppliersModule, SuppliersRoute,
            "قائمة الموردين", "Suppliers list", "تفاصيل المورد", "Supplier details",
            ListingModuleType.Supplier, "الموردون المضافون بواسطتي", "My suppliers",
            [
                Param("supplierName", "اسم المورد", "Supplier name", ReadParameterTypes.String),
                Param("supplierType", "تخصص التوريد", "Supplier type", ReadParameterTypes.Enum,
                    optionsSource: "supplierTypes", optionsValue: ReadOptionsValues.Id),
                Param("suppliedProduct", "المنتج الموّرد", "Supplied product", ReadParameterTypes.String)
            ],
            [
                Operation("supplierTypes", "تخصصات التوريد", "Supplier types",
                    $"{SuppliersRoute}/types")
            ]);

    private static ReadConfigSchema WholesaleTraders(int categoryId, SubCategoryType subCategory) =>
        BusinessModule(categoryId, subCategory, WholesaleTradersModule, WholesaleTradersRoute,
            "قائمة تجار الجملة", "Wholesale traders list", "تفاصيل تاجر الجملة", "Wholesale trader details",
            ListingModuleType.WholesaleTrader, "تجار الجملة المضافون بواسطتي", "My wholesale traders",
            [
                Param("traderName", "اسم التاجر", "Trader name", ReadParameterTypes.String),
                Param("tradeType", "تخصص التجارة", "Trade type", ReadParameterTypes.Enum,
                    optionsSource: "tradeTypes", optionsValue: ReadOptionsValues.Id),
                Param("saleType", "نوع البيع", "Sale type", ReadParameterTypes.Enum,
                    optionsSource: "saleTypes", optionsValue: ReadOptionsValues.Id)
            ],
            [
                Operation("tradeTypes", "تخصصات التجارة", "Trade types",
                    $"{WholesaleTradersRoute}/trade-types"),
                Operation("saleTypes", "أنواع البيع", "Sale types",
                    $"{WholesaleTradersRoute}/sale-types")
            ]);

    private static ReadConfigSchema FruitMerchants(int categoryId, SubCategoryType subCategory) =>
        BusinessModule(categoryId, subCategory, FruitMerchantsModule, FruitMerchantsRoute,
            "قائمة تجار الخضر والفاكهة", "Fruit & vegetable merchants list",
            "تفاصيل تاجر الخضر والفاكهة", "Fruit & vegetable merchant details",
            ListingModuleType.FruitVegetableMerchant,
            "تجار الخضر والفاكهة المضافون بواسطتي", "My fruit & vegetable merchants",
            [
                Param("stallName", "اسم المحل", "Stall name", ReadParameterTypes.String),
                Param("merchantName", "اسم التاجر", "Merchant name", ReadParameterTypes.String),
                Param("saleType", "نوع البيع", "Sale type", ReadParameterTypes.Enum,
                    optionsSource: "saleTypes", optionsValue: ReadOptionsValues.Id)
            ],
            [
                Operation("saleTypes", "أنواع البيع", "Sale types",
                    $"{FruitMerchantsRoute}/sale-types")
            ]);

    private static ReadConfigSchema JobRequests(int categoryId, SubCategoryType subCategory)
    {
        var filters = new List<ReadParameterDto>
        {
            Param("search", "بحث", "Search", ReadParameterTypes.String),
            Param("jobField", "مجال العمل", "Job field", ReadParameterTypes.Enum,
                optionsSource: "jobFields", optionsValue: ReadOptionsValues.Id),
            Param("experience", "الخبرة", "Experience", ReadParameterTypes.Enum,
                optionsSource: "experienceLevels", optionsValue: ReadOptionsValues.Id),
            Param("education", "المؤهل الدراسي", "Education", ReadParameterTypes.Enum,
                optionsSource: "educationLevels", optionsValue: ReadOptionsValues.Id),
            Param("center", "المدينة (المركز)", "City / center", ReadParameterTypes.String,
                optionsSource: ReadOperationKeys.Centers, optionsValue: ReadOptionsValues.Name)
        };

        filters.AddRange(Paging());

        var operations = new List<ReadOperationDto>
        {
            Operation(ReadOperationKeys.List, "قائمة طلبات العمل", "Job requests list",
                JobRequestsRoute, paginated: true, queryParameters: filters),

            Operation(ReadOperationKeys.Details, "تفاصيل طلب العمل", "Job request details",
                $"{JobRequestsRoute}/{{id}}", routeParameters: IdRoute()),

            UnifiedMyListings(ListingModuleType.JobRequest,
                "طلبات العمل المضافة بواسطتي", "My job requests"),

            Operation("jobFields", "مجالات العمل", "Job fields", $"{JobRequestsRoute}/job-fields"),
            Operation("experienceLevels", "مستويات الخبرة", "Experience levels",
                $"{JobRequestsRoute}/experience-levels"),
            Operation("educationLevels", "المؤهلات الدراسية", "Education levels",
                $"{JobRequestsRoute}/education-levels")
        };

        operations.AddRange(SharedTail(categoryId, (int)subCategory));

        return new ReadConfigSchema(JobRequestsModule, operations);
    }

    private static ReadConfigSchema JobOpportunities(int categoryId, SubCategoryType subCategory)
    {
        var filters = new List<ReadParameterDto>
        {
            Param("search", "بحث", "Search", ReadParameterTypes.String),
            Param("jobTitle", "المسمى الوظيفي", "Job title", ReadParameterTypes.String),
            Param("jobField", "مجال العمل", "Job field", ReadParameterTypes.Enum,
                optionsSource: "jobFields", optionsValue: ReadOptionsValues.Id),
            Param("workType", "نوع الدوام", "Work type", ReadParameterTypes.Enum,
                optionsSource: "workTypes", optionsValue: ReadOptionsValues.Id),
            Param("requiredExperience", "الخبرة المطلوبة", "Required experience", ReadParameterTypes.Enum,
                optionsSource: "experienceLevels", optionsValue: ReadOptionsValues.Id),
            Param("center", "المدينة (المركز)", "City / center", ReadParameterTypes.String,
                optionsSource: ReadOperationKeys.Centers, optionsValue: ReadOptionsValues.Name)
        };

        filters.AddRange(Paging());

        var operations = new List<ReadOperationDto>
        {
            Operation(ReadOperationKeys.List, "قائمة فرص العمل", "Job opportunities list",
                JobOpportunitiesRoute, paginated: true, queryParameters: filters),

            Operation(ReadOperationKeys.Details, "تفاصيل فرصة العمل", "Job opportunity details",
                $"{JobOpportunitiesRoute}/{{id}}", routeParameters: IdRoute()),

            UnifiedMyListings(ListingModuleType.JobOpportunity,
                "فرص العمل المضافة بواسطتي", "My job opportunities"),

            Operation("jobFields", "مجالات العمل", "Job fields",
                $"{JobOpportunitiesRoute}/job-fields"),
            Operation("experienceLevels", "مستويات الخبرة", "Experience levels",
                $"{JobOpportunitiesRoute}/experience-levels"),
            Operation("workTypes", "أنواع الدوام", "Work types",
                $"{JobOpportunitiesRoute}/work-types"),
            Operation("salaryTypes", "أنواع الراتب", "Salary types",
                $"{JobOpportunitiesRoute}/salary-types")
        };

        operations.AddRange(SharedTail(categoryId, (int)subCategory));

        return new ReadConfigSchema(JobOpportunitiesModule, operations);
    }

    private static ReadConfigSchema Workshops(int categoryId, SubCategoryType subCategory)
    {
        var filters = new List<ReadParameterDto>
        {
            Param("search", "بحث", "Search", ReadParameterTypes.String),
            Param("workshopType", "نوع الورشة", "Workshop type", ReadParameterTypes.Enum,
                optionsSource: ReadOperationKeys.WorkshopTypes, optionsValue: ReadOptionsValues.Id)
        };

        filters.AddRange(LocationFilters("city"));
        filters.AddRange(Paging());

        var operations = new List<ReadOperationDto>
        {
            Operation(ReadOperationKeys.List, "قائمة الورش", "Workshops list",
                WorkshopsRoute, paginated: true, queryParameters: filters),

            Operation(ReadOperationKeys.Details, "تفاصيل الورشة", "Workshop details",
                $"{WorkshopsRoute}/{{id}}", routeParameters: IdRoute()),

            UnifiedMyListings(ListingModuleType.Workshop, "الورش المضافة بواسطتي", "My workshops"),

            WorkshopTypes()
        };

        operations.AddRange(SharedTail(categoryId, (int)subCategory));

        return new ReadConfigSchema(WorkshopsModule, operations);
    }

    private static ReadConfigSchema Craftsmen(int categoryId, SubCategoryType subCategory)
    {
        var filters = new List<ReadParameterDto>
        {
            Param("search", "بحث", "Search", ReadParameterTypes.String),
            Param("specialization", "التخصص", "Specialization", ReadParameterTypes.Enum,
                optionsSource: ReadOperationKeys.Specializations, optionsValue: ReadOptionsValues.Id),
            Param("experienceLevel", "سنوات الخبرة", "Experience level", ReadParameterTypes.Enum,
                optionsSource: ReadOperationKeys.ExperienceLevels, optionsValue: ReadOptionsValues.Id)
        };

        filters.AddRange(LocationFilters("city"));
        filters.AddRange(Paging());

        var operations = new List<ReadOperationDto>
        {
            Operation(ReadOperationKeys.List, "قائمة الحرفيين", "Craftsmen list",
                CraftsmenRoute, paginated: true, queryParameters: filters),

            Operation(ReadOperationKeys.Details, "تفاصيل الحرفي", "Craftsman details",
                $"{CraftsmenRoute}/{{id}}", routeParameters: IdRoute()),

            UnifiedMyListings(ListingModuleType.Craftsman, "الحرفيون المضافون بواسطتي", "My craftsmen"),

            Specializations(),
            ExperienceLevels()
        };

        operations.AddRange(SharedTail(categoryId, (int)subCategory));

        return new ReadConfigSchema(CraftsmenModule, operations);
    }

    private static ReadConfigSchema LostFound(
        int categoryId, SubCategoryType subCategory, PostType postType)
    {
        var isLost = postType == PostType.Lost;

        var filters = new List<ReadParameterDto>
        {
            Param("postType", "نوع الإعلان", "Post type", ReadParameterTypes.Enum,
                required: true, defaultValue: (int)postType,
                options: isLost
                    ? EnumOptions((PostType.Lost, "ضايع مني", "Lost"))
                    : EnumOptions((PostType.Found, "لقيت", "Found"))),

            Param("search", "بحث", "Search", ReadParameterTypes.String)
        };

        filters.AddRange(LocationFilters("city"));

        filters.Add(isLost
            ? Param("date", "تاريخ الفقدان", "Lost date", ReadParameterTypes.Date)
            : Param("date", "تاريخ العثور", "Found date", ReadParameterTypes.Date));

        filters.AddRange(Paging());

        var pinnedType = new Dictionary<string, object> { ["postType"] = (int)postType };

        var operations = new List<ReadOperationDto>
        {
            Operation(ReadOperationKeys.List,
                isLost ? "منشورات ضايع مني" : "منشورات لقيت",
                isLost ? "Lost items feed" : "Found items feed",
                LostFoundRoute, paginated: true, queryParameters: filters, query: pinnedType),

            Operation(ReadOperationKeys.Details, "تفاصيل المنشور", "Post details",
                $"{LostFoundRoute}/{{id}}", routeParameters: IdRoute()),

            Operation(ReadOperationKeys.Comments, "تعليقات المنشور", "Post comments",
                $"{LostFoundRoute}/{{id}}/comments", routeParameters: IdRoute()),

            UnifiedMyListings(
                ListingModuleCatalog.ModuleOf(postType),
                isLost ? "إعلانات ضايع مني الخاصة بي" : "إعلانات لقيت الخاصة بي",
                isLost ? "My lost items" : "My found items")
        };

        operations.AddRange(SharedTail(categoryId, (int)subCategory));

        return new ReadConfigSchema(LostFoundModule, operations);
    }

    private static ReadConfigSchema AnimalModule(
        int categoryId, SubCategoryType subCategory, string module, string route,
        string listLabelAr, string listLabelEn, string detailsLabelAr, string detailsLabelEn,
        ListingModuleType listingType, string myListingsAr, string myListingsEn,
        IEnumerable<ReadParameterDto> filters, IEnumerable<ReadOperationDto> lookupOperations)
    {
        var queryParameters = new List<ReadParameterDto>
        {
            Param("search", "بحث", "Search", ReadParameterTypes.String),
            Param("sellerName", "اسم البائع", "Seller name", ReadParameterTypes.String)
        };

        queryParameters.AddRange(filters);
        queryParameters.Add(Param("priceFrom", "السعر من", "Price from", ReadParameterTypes.Decimal, min: 0));
        queryParameters.Add(Param("priceTo", "السعر إلى", "Price to", ReadParameterTypes.Decimal, min: 0));
        queryParameters.AddRange(Paging());

        var operations = new List<ReadOperationDto>
        {
            Operation(ReadOperationKeys.List, listLabelAr, listLabelEn,
                route, paginated: true, queryParameters: queryParameters),

            Operation(ReadOperationKeys.Details, detailsLabelAr, detailsLabelEn,
                $"{route}/{{id}}", routeParameters: IdRoute()),

            UnifiedMyListings(listingType, myListingsAr, myListingsEn)
        };

        operations.AddRange(lookupOperations);
        operations.AddRange(SharedTail(categoryId, (int)subCategory));

        return new ReadConfigSchema(module, operations);
    }

    private static ReadConfigSchema Livestock(int categoryId, SubCategoryType subCategory) =>
        AnimalModule(categoryId, subCategory, LivestockModule, LivestockRoute,
            "قائمة المواشي", "Livestock list", "تفاصيل إعلان المواشي", "Livestock details",
            ListingModuleType.Livestock, "إعلانات المواشي المضافة بواسطتي", "My livestock listings",
            [
                Param("breed", "السلالة", "Breed", ReadParameterTypes.Enum,
                    optionsSource: "livestockBreeds", optionsValue: ReadOptionsValues.Id),
                Param("purpose", "الغرض", "Purpose", ReadParameterTypes.Enum,
                    optionsSource: "livestockPurposes", optionsValue: ReadOptionsValues.Id),
                Param("age", "العمر", "Age", ReadParameterTypes.Enum,
                    optionsSource: "livestockAges", optionsValue: ReadOptionsValues.Id),
                Param("gender", "الجنس", "Gender", ReadParameterTypes.Enum,
                    optionsSource: "livestockGenders", optionsValue: ReadOptionsValues.Id),
                Param("healthStatus", "الحالة الصحية", "Health status", ReadParameterTypes.Enum,
                    optionsSource: "livestockHealthStatuses", optionsValue: ReadOptionsValues.Id)
            ],
            [
                Operation("livestockBreeds", "السلالة", "Breed",
                    $"{LivestockRoute}/breeds"),
                Operation("livestockPurposes", "الغرض", "Purpose",
                    $"{LivestockRoute}/purposes"),
                Operation("livestockAges", "العمر", "Age",
                    $"{LivestockRoute}/ages"),
                Operation("livestockGenders", "الجنس", "Gender",
                    $"{LivestockRoute}/genders"),
                Operation("livestockHealthStatuses", "الحالة الصحية", "Health status",
                    $"{LivestockRoute}/health-status"),
                Operation("livestockVaccinations", "حالة التحصين", "Vaccination status",
                    $"{LivestockRoute}/vaccinations"),
                Operation("livestockProductions", "الإنتاج", "Production",
                    $"{LivestockRoute}/productions")
            ]);

    private static ReadConfigSchema SheepGoat(int categoryId, SubCategoryType subCategory) =>
        AnimalModule(categoryId, subCategory, SheepGoatModule, SheepGoatRoute,
            "قائمة الأغنام والماعز", "Sheep & goats list", "تفاصيل إعلان الأغنام والماعز", "Sheep & goat details",
            ListingModuleType.SheepGoat, "إعلانات الأغنام والماعز المضافة بواسطتي", "My sheep & goats listings",
            [
                Param("breed", "السلالة", "Breed", ReadParameterTypes.Enum,
                    optionsSource: "sheepGoatBreeds", optionsValue: ReadOptionsValues.Id),
                Param("purpose", "الغرض", "Purpose", ReadParameterTypes.Enum,
                    optionsSource: "sheepGoatPurposes", optionsValue: ReadOptionsValues.Id),
                Param("age", "العمر", "Age", ReadParameterTypes.Enum,
                    optionsSource: "sheepGoatAges", optionsValue: ReadOptionsValues.Id),
                Param("gender", "الجنس", "Gender", ReadParameterTypes.Enum,
                    optionsSource: "sheepGoatGenders", optionsValue: ReadOptionsValues.Id),
                Param("healthStatus", "الحالة الصحية", "Health status", ReadParameterTypes.Enum,
                    optionsSource: "sheepGoatHealthStatuses", optionsValue: ReadOptionsValues.Id)
            ],
            [
                Operation("sheepGoatBreeds", "السلالة", "Breed",
                    $"{SheepGoatRoute}/breeds"),
                Operation("sheepGoatPurposes", "الغرض", "Purpose",
                    $"{SheepGoatRoute}/purposes"),
                Operation("sheepGoatAges", "العمر", "Age",
                    $"{SheepGoatRoute}/ages"),
                Operation("sheepGoatGenders", "الجنس", "Gender",
                    $"{SheepGoatRoute}/genders"),
                Operation("sheepGoatHealthStatuses", "الحالة الصحية", "Health status",
                    $"{SheepGoatRoute}/health-status"),
                Operation("sheepGoatVaccinations", "حالة التحصين", "Vaccination status",
                    $"{SheepGoatRoute}/vaccinations")
            ]);

    private static ReadConfigSchema Horse(int categoryId, SubCategoryType subCategory) =>
        AnimalModule(categoryId, subCategory, HorseModule, HorseRoute,
            "قائمة الخيول", "Horses list", "تفاصيل إعلان الخيل", "Horse details",
            ListingModuleType.Horse, "إعلانات الخيول المضافة بواسطتي", "My horse listings",
            [
                Param("breed", "السلالة", "Breed", ReadParameterTypes.Enum,
                    optionsSource: "horseBreeds", optionsValue: ReadOptionsValues.Id),
                Param("purpose", "الغرض", "Purpose", ReadParameterTypes.Enum,
                    optionsSource: "horsePurposes", optionsValue: ReadOptionsValues.Id),
                Param("age", "العمر", "Age", ReadParameterTypes.Enum,
                    optionsSource: "horseAges", optionsValue: ReadOptionsValues.Id),
                Param("gender", "الجنس", "Gender", ReadParameterTypes.Enum,
                    optionsSource: "horseGenders", optionsValue: ReadOptionsValues.Id),
                Param("healthStatus", "الحالة الصحية", "Health status", ReadParameterTypes.Enum,
                    optionsSource: "horseHealthStatuses", optionsValue: ReadOptionsValues.Id)
            ],
            [
                Operation("horseBreeds", "السلالة", "Breed",
                    $"{HorseRoute}/breeds"),
                Operation("horsePurposes", "الغرض", "Purpose",
                    $"{HorseRoute}/purposes"),
                Operation("horseAges", "العمر", "Age",
                    $"{HorseRoute}/ages"),
                Operation("horseGenders", "الجنس", "Gender",
                    $"{HorseRoute}/genders"),
                Operation("horseHealthStatuses", "الحالة الصحية", "Health status",
                    $"{HorseRoute}/health-status"),
                Operation("horseTrainingLevels", "مستوى التدريب", "Training level",
                    $"{HorseRoute}/training-levels"),
                Operation("horseVaccinations", "حالة التحصين", "Vaccination status",
                    $"{HorseRoute}/vaccinations")
            ]);

    private static ReadConfigSchema Camel(int categoryId, SubCategoryType subCategory) =>
        AnimalModule(categoryId, subCategory, CamelModule, CamelRoute,
            "قائمة الإبل", "Camels list", "تفاصيل إعلان الإبل", "Camel details",
            ListingModuleType.Camel, "إعلانات الإبل المضافة بواسطتي", "My camel listings",
            [
                Param("breed", "السلالة", "Breed", ReadParameterTypes.Enum,
                    optionsSource: "camelBreeds", optionsValue: ReadOptionsValues.Id),
                Param("purpose", "الغرض", "Purpose", ReadParameterTypes.Enum,
                    optionsSource: "camelPurposes", optionsValue: ReadOptionsValues.Id),
                Param("age", "العمر", "Age", ReadParameterTypes.Enum,
                    optionsSource: "camelAges", optionsValue: ReadOptionsValues.Id),
                Param("gender", "الجنس", "Gender", ReadParameterTypes.Enum,
                    optionsSource: "camelGenders", optionsValue: ReadOptionsValues.Id),
                Param("healthStatus", "الحالة الصحية", "Health status", ReadParameterTypes.Enum,
                    optionsSource: "camelHealthStatuses", optionsValue: ReadOptionsValues.Id)
            ],
            [
                Operation("camelBreeds", "السلالة", "Breed",
                    $"{CamelRoute}/breeds"),
                Operation("camelPurposes", "الغرض", "Purpose",
                    $"{CamelRoute}/purposes"),
                Operation("camelAges", "العمر", "Age",
                    $"{CamelRoute}/ages"),
                Operation("camelGenders", "الجنس", "Gender",
                    $"{CamelRoute}/genders"),
                Operation("camelHealthStatuses", "الحالة الصحية", "Health status",
                    $"{CamelRoute}/health-status"),
                Operation("camelVaccinations", "حالة التحصين", "Vaccination status",
                    $"{CamelRoute}/vaccinations")
            ]);

    private static ReadConfigSchema Bird(int categoryId, SubCategoryType subCategory) =>
        AnimalModule(categoryId, subCategory, BirdModule, BirdRoute,
            "قائمة الطيور", "Birds list", "تفاصيل إعلان الطيور", "Bird details",
            ListingModuleType.Bird, "إعلانات الطيور المضافة بواسطتي", "My bird listings",
            [
                Param("animalType", "النوع", "Type", ReadParameterTypes.Enum,
                    optionsSource: "birdTypes", optionsValue: ReadOptionsValues.Id),
                Param("purpose", "الغرض", "Purpose", ReadParameterTypes.Enum,
                    optionsSource: "birdPurposes", optionsValue: ReadOptionsValues.Id),
                Param("age", "العمر", "Age", ReadParameterTypes.Enum,
                    optionsSource: "birdAges", optionsValue: ReadOptionsValues.Id),
                Param("gender", "الجنس", "Gender", ReadParameterTypes.Enum,
                    optionsSource: "birdGenders", optionsValue: ReadOptionsValues.Id),
                Param("healthStatus", "الحالة الصحية", "Health status", ReadParameterTypes.Enum,
                    optionsSource: "birdHealthStatuses", optionsValue: ReadOptionsValues.Id)
            ],
            [
                Operation("birdTypes", "النوع", "Type",
                    $"{BirdRoute}/types"),
                Operation("birdPurposes", "الغرض", "Purpose",
                    $"{BirdRoute}/purposes"),
                Operation("birdAges", "العمر", "Age",
                    $"{BirdRoute}/ages"),
                Operation("birdGenders", "الجنس", "Gender",
                    $"{BirdRoute}/genders"),
                Operation("birdHealthStatuses", "الحالة الصحية", "Health status",
                    $"{BirdRoute}/health-status"),
                Operation("birdVaccinations", "حالة التحصين", "Vaccination status",
                    $"{BirdRoute}/vaccinations")
            ]);

    private static ReadConfigSchema Pet(int categoryId, SubCategoryType subCategory) =>
        AnimalModule(categoryId, subCategory, PetModule, PetRoute,
            "قائمة الحيوانات الأليفة", "Pets list", "تفاصيل إعلان الحيوان الأليف", "Pet details",
            ListingModuleType.Pet, "إعلانات الحيوانات الأليفة المضافة بواسطتي", "My pet listings",
            [
                Param("breed", "السلالة", "Breed", ReadParameterTypes.Enum,
                    optionsSource: "petBreeds", optionsValue: ReadOptionsValues.Id),
                Param("purpose", "الغرض", "Purpose", ReadParameterTypes.Enum,
                    optionsSource: "petPurposes", optionsValue: ReadOptionsValues.Id),
                Param("age", "العمر", "Age", ReadParameterTypes.Enum,
                    optionsSource: "petAges", optionsValue: ReadOptionsValues.Id),
                Param("gender", "الجنس", "Gender", ReadParameterTypes.Enum,
                    optionsSource: "petGenders", optionsValue: ReadOptionsValues.Id),
                Param("healthStatus", "الحالة الصحية", "Health status", ReadParameterTypes.Enum,
                    optionsSource: "petHealthStatuses", optionsValue: ReadOptionsValues.Id)
            ],
            [
                Operation("petBreeds", "السلالة", "Breed",
                    $"{PetRoute}/breeds"),
                Operation("petPurposes", "الغرض", "Purpose",
                    $"{PetRoute}/purposes"),
                Operation("petAges", "العمر", "Age",
                    $"{PetRoute}/ages"),
                Operation("petGenders", "الجنس", "Gender",
                    $"{PetRoute}/genders"),
                Operation("petHealthStatuses", "الحالة الصحية", "Health status",
                    $"{PetRoute}/health-status"),
                Operation("petTrainingLevels", "مستوى التدريب", "Training level",
                    $"{PetRoute}/training-levels"),
                Operation("petVaccinations", "حالة التحصين", "Vaccination status",
                    $"{PetRoute}/vaccinations")
            ]);

    private static ReadConfigSchema Fish(int categoryId, SubCategoryType subCategory) =>
        AnimalModule(categoryId, subCategory, FishModule, FishRoute,
            "قائمة الأسماك", "Fish list", "تفاصيل إعلان الأسماك", "Fish details",
            ListingModuleType.Fish, "إعلانات الأسماك المضافة بواسطتي", "My fish listings",
            [
                Param("animalType", "النوع", "Type", ReadParameterTypes.Enum,
                    optionsSource: "fishTypes", optionsValue: ReadOptionsValues.Id),
                Param("purpose", "الغرض", "Purpose", ReadParameterTypes.Enum,
                    optionsSource: "fishPurposes", optionsValue: ReadOptionsValues.Id),
                Param("age", "العمر", "Age", ReadParameterTypes.Enum,
                    optionsSource: "fishAges", optionsValue: ReadOptionsValues.Id),
                Param("healthStatus", "الحالة الصحية", "Health status", ReadParameterTypes.Enum,
                    optionsSource: "fishHealthStatuses", optionsValue: ReadOptionsValues.Id)
            ],
            [
                Operation("fishTypes", "النوع", "Type",
                    $"{FishRoute}/types"),
                Operation("fishPurposes", "الغرض", "Purpose",
                    $"{FishRoute}/purposes"),
                Operation("fishAges", "العمر", "Age",
                    $"{FishRoute}/ages"),
                Operation("fishHealthStatuses", "الحالة الصحية", "Health status",
                    $"{FishRoute}/health-status")
            ]);

    private static ReadConfigSchema Bee(int categoryId, SubCategoryType subCategory) =>
        AnimalModule(categoryId, subCategory, BeeModule, BeeRoute,
            "قائمة النحل", "Bees list", "تفاصيل إعلان النحل", "Bee details",
            ListingModuleType.Bee, "إعلانات النحل المضافة بواسطتي", "My bee listings",
            [
                Param("animalType", "النوع", "Type", ReadParameterTypes.Enum,
                    optionsSource: "beeTypes", optionsValue: ReadOptionsValues.Id),
                Param("purpose", "الغرض", "Purpose", ReadParameterTypes.Enum,
                    optionsSource: "beePurposes", optionsValue: ReadOptionsValues.Id),
                Param("healthStatus", "الحالة الصحية", "Health status", ReadParameterTypes.Enum,
                    optionsSource: "beeHealthStatuses", optionsValue: ReadOptionsValues.Id)
            ],
            [
                Operation("beeTypes", "النوع", "Type",
                    $"{BeeRoute}/types"),
                Operation("beePurposes", "الغرض", "Purpose",
                    $"{BeeRoute}/purposes"),
                Operation("beeHealthStatuses", "الحالة الصحية", "Health status",
                    $"{BeeRoute}/health-status"),
                Operation("beeProductions", "الإنتاج", "Production",
                    $"{BeeRoute}/productions")
            ]);

    private static ReadConfigSchema OtherAnimal(int categoryId, SubCategoryType subCategory) =>
        AnimalModule(categoryId, subCategory, OtherAnimalModule, OtherAnimalRoute,
            "قائمة الحيوانات الأخرى", "Other animals list", "تفاصيل إعلان الحيوان", "Other animal details",
            ListingModuleType.OtherAnimal, "إعلانات الحيوانات الأخرى المضافة بواسطتي", "My other-animal listings",
            [
                Param("animalType", "النوع", "Type", ReadParameterTypes.Enum,
                    optionsSource: "otherAnimalTypes", optionsValue: ReadOptionsValues.Id),
                Param("purpose", "الغرض", "Purpose", ReadParameterTypes.Enum,
                    optionsSource: "otherAnimalPurposes", optionsValue: ReadOptionsValues.Id),
                Param("age", "العمر", "Age", ReadParameterTypes.Enum,
                    optionsSource: "otherAnimalAges", optionsValue: ReadOptionsValues.Id),
                Param("gender", "الجنس", "Gender", ReadParameterTypes.Enum,
                    optionsSource: "otherAnimalGenders", optionsValue: ReadOptionsValues.Id),
                Param("healthStatus", "الحالة الصحية", "Health status", ReadParameterTypes.Enum,
                    optionsSource: "otherAnimalHealthStatuses", optionsValue: ReadOptionsValues.Id)
            ],
            [
                Operation("otherAnimalTypes", "النوع", "Type",
                    $"{OtherAnimalRoute}/types"),
                Operation("otherAnimalPurposes", "الغرض", "Purpose",
                    $"{OtherAnimalRoute}/purposes"),
                Operation("otherAnimalAges", "العمر", "Age",
                    $"{OtherAnimalRoute}/ages"),
                Operation("otherAnimalGenders", "الجنس", "Gender",
                    $"{OtherAnimalRoute}/genders"),
                Operation("otherAnimalHealthStatuses", "الحالة الصحية", "Health status",
                    $"{OtherAnimalRoute}/health-status"),
                Operation("otherAnimalVaccinations", "حالة التحصين", "Vaccination status",
                    $"{OtherAnimalRoute}/vaccinations")
            ]);

    private static ReadConfigSchema AntiqueModule(
        int categoryId, SubCategoryType subCategory, string module, string route,
        string listLabelAr, string listLabelEn, string detailsLabelAr, string detailsLabelEn,
        ListingModuleType listingType, string myListingsAr, string myListingsEn,
        IEnumerable<ReadParameterDto> filters, IEnumerable<ReadOperationDto> lookupOperations)
    {
        var queryParameters = new List<ReadParameterDto>
        {
            Param("search", "بحث", "Search", ReadParameterTypes.String),
            Param("sellerName", "اسم البائع", "Seller name", ReadParameterTypes.String)
        };

        queryParameters.AddRange(filters);

        queryParameters.Add(Param("negotiable", "السعر قابل للتفاوض", "Negotiable", ReadParameterTypes.Boolean));
        queryParameters.Add(Param("center", "المركز", "Center", ReadParameterTypes.String,
            optionsSource: ReadOperationKeys.Centers, optionsValue: ReadOptionsValues.Name));
        queryParameters.Add(Param("priceFrom", "السعر من", "Price from", ReadParameterTypes.Decimal, min: 0));
        queryParameters.Add(Param("priceTo", "السعر إلى", "Price to", ReadParameterTypes.Decimal, min: 0));
        queryParameters.Add(AntiqueSortParameter());
        queryParameters.AddRange(Paging());

        var operations = new List<ReadOperationDto>
        {
            Operation(ReadOperationKeys.List, listLabelAr, listLabelEn,
                route, paginated: true, queryParameters: queryParameters),

            Operation(ReadOperationKeys.Details, detailsLabelAr, detailsLabelEn,
                $"{route}/{{id}}", routeParameters: IdRoute()),

            UnifiedMyListings(listingType, myListingsAr, myListingsEn)
        };

        operations.AddRange(lookupOperations);
        operations.AddRange(SharedTail(categoryId, (int)subCategory));

        return new ReadConfigSchema(module, operations);
    }

    private static ReadParameterDto AntiqueSortParameter() =>
        Param("sortBy", "الترتيب", "Sort by", ReadParameterTypes.Enum,
            defaultValue: (int)AntiqueSortBy.Newest,
            options: EnumOptions(
                (AntiqueSortBy.Newest, "الأحدث", "Newest"),
                (AntiqueSortBy.Oldest, "الأقدم", "Oldest"),
                (AntiqueSortBy.PriceAsc, "السعر: من الأقل للأعلى", "Price: low to high"),
                (AntiqueSortBy.PriceDesc, "السعر: من الأعلى للأقل", "Price: high to low")));

    private static ReadConfigSchema DecorAntiques(int categoryId, SubCategoryType subCategory) =>
        AntiqueModule(categoryId, subCategory, DecorAntiqueModule, DecorAntiqueRoute,
            "قائمة التحف", "Antiques decor list", "تفاصيل إعلان التحفة", "Antiques decor details",
            ListingModuleType.DecorAntique, "إعلانات التحف المضافة بواسطتي", "My antiques decor listings",
            [
                Param("itemType", "نوع القطعة", "Item type", ReadParameterTypes.Enum,
                    optionsSource: "decorAntiqueItemTypes", optionsValue: ReadOptionsValues.Id),
                Param("material", "الخامة", "Material", ReadParameterTypes.Enum,
                    optionsSource: "decorAntiqueMaterials", optionsValue: ReadOptionsValues.Id),
                Param("condition", "الحالة", "Condition", ReadParameterTypes.Enum,
                    optionsSource: "decorAntiqueConditions", optionsValue: ReadOptionsValues.Id),
                Param("originality", "الأصالة", "Originality", ReadParameterTypes.Enum,
                    optionsSource: "decorAntiqueOriginalities", optionsValue: ReadOptionsValues.Id)
            ],
            [
                Operation("decorAntiqueItemTypes", "نوع القطعة", "Item type",
                    $"{DecorAntiqueRoute}/item-types"),
                Operation("decorAntiqueMaterials", "الخامة", "Material",
                    $"{DecorAntiqueRoute}/materials"),
                Operation("decorAntiqueConditions", "الحالة", "Condition",
                    $"{DecorAntiqueRoute}/conditions"),
                Operation("decorAntiqueOriginalities", "الأصالة", "Originality",
                    $"{DecorAntiqueRoute}/originality")
            ]);

    private static ReadConfigSchema Antiques(int categoryId, SubCategoryType subCategory) =>
        AntiqueModule(categoryId, subCategory, AntiqueModuleName, AntiqueRoute,
            "قائمة الأنتيكات", "Antiques list", "تفاصيل إعلان الأنتيك", "Antique details",
            ListingModuleType.Antique, "إعلانات الأنتيكات المضافة بواسطتي", "My antique listings",
            [
                Param("antiqueType", "نوع الأنتيك", "Antique type", ReadParameterTypes.Enum,
                    optionsSource: "antiqueTypes", optionsValue: ReadOptionsValues.Id),
                Param("manufactureYear", "سنة الصنع", "Manufacture year", ReadParameterTypes.Integer,
                    min: AntiqueCatalog.MinYear, max: AntiqueCatalog.MaxYear),
                Param("countryOfOrigin", "بلد المنشأ", "Country of origin", ReadParameterTypes.String),
                Param("originality", "الأصالة", "Originality", ReadParameterTypes.Enum,
                    optionsSource: "antiqueOriginalities", optionsValue: ReadOptionsValues.Id),
                Param("condition", "الحالة", "Condition", ReadParameterTypes.Enum,
                    optionsSource: "antiqueConditions", optionsValue: ReadOptionsValues.Id)
            ],
            [
                Operation("antiqueTypes", "نوع الأنتيك", "Antique type", $"{AntiqueRoute}/types"),
                Operation("antiqueMaterials", "الخامة", "Material", $"{AntiqueRoute}/materials"),
                Operation("antiqueConditions", "الحالة", "Condition", $"{AntiqueRoute}/conditions"),
                Operation("antiqueWorkingStatuses", "هل يعمل؟", "Working status",
                    $"{AntiqueRoute}/working-statuses"),
                Operation("antiqueOriginalities", "الأصالة", "Originality", $"{AntiqueRoute}/originality")
            ]);

    private static ReadConfigSchema Paintings(int categoryId, SubCategoryType subCategory) =>
        AntiqueModule(categoryId, subCategory, PaintingModule, PaintingRoute,
            "قائمة اللوحات الفنية", "Paintings list", "تفاصيل إعلان اللوحة", "Painting details",
            ListingModuleType.Painting, "اللوحات الفنية المضافة بواسطتي", "My painting listings",
            [
                Param("paintingType", "نوع اللوحة", "Painting type", ReadParameterTypes.Enum,
                    optionsSource: "paintingTypes", optionsValue: ReadOptionsValues.Id),
                Param("artistName", "اسم الفنان", "Artist name", ReadParameterTypes.String),
                Param("originality", "الأصالة", "Originality", ReadParameterTypes.Enum,
                    optionsSource: "paintingOriginalities", optionsValue: ReadOptionsValues.Id),
                Param("framed", "مؤطرة", "Framed", ReadParameterTypes.Boolean)
            ],
            [
                Operation("paintingTypes", "نوع اللوحة", "Painting type", $"{PaintingRoute}/types"),
                Operation("paintingMaterials", "الخامة", "Material", $"{PaintingRoute}/materials"),
                Operation("paintingOriginalities", "الأصالة", "Originality",
                    $"{PaintingRoute}/originality")
            ]);

    private static ReadConfigSchema Handmade(int categoryId, SubCategoryType subCategory) =>
        AntiqueModule(categoryId, subCategory, HandmadeModule, HandmadeRoute,
            "قائمة الأعمال اليدوية", "Handmade list", "تفاصيل إعلان العمل اليدوي", "Handmade details",
            ListingModuleType.Handmade, "الأعمال اليدوية المضافة بواسطتي", "My handmade listings",
            [
                Param("handmadeType", "نوع العمل اليدوي", "Handmade type", ReadParameterTypes.Enum,
                    optionsSource: "handmadeTypes", optionsValue: ReadOptionsValues.Id),
                Param("isFullyHandmade", "هاند ميد بالكامل", "Fully handmade", ReadParameterTypes.Boolean),
                Param("customOrder", "الطلب حسب المواصفات", "Custom order", ReadParameterTypes.Boolean),
                Param("color", "اللون", "Color", ReadParameterTypes.Enum,
                    optionsSource: "handmadeColors", optionsValue: ReadOptionsValues.Id)
            ],
            [
                Operation("handmadeTypes", "نوع العمل اليدوي", "Handmade type", $"{HandmadeRoute}/types"),
                Operation("handmadeColors", "الألوان", "Colors", $"{HandmadeRoute}/colors")
            ]);

    private static ReadConfigSchema CoinsStamps(int categoryId, SubCategoryType subCategory) =>
        AntiqueModule(categoryId, subCategory, CoinStampModule, CoinStampRoute,
            "قائمة العملات والطوابع", "Coins & stamps list",
            "تفاصيل إعلان العملة / الطابع", "Coin & stamp details",
            ListingModuleType.CoinStamp,
            "العملات والطوابع المضافة بواسطتي", "My coin & stamp listings",
            [
                Param("itemType", "نوع القطعة", "Item type", ReadParameterTypes.Enum,
                    optionsSource: "coinStampItemTypes", optionsValue: ReadOptionsValues.Id),
                Param("country", "بلد الإصدار", "Country", ReadParameterTypes.String),
                Param("issueYear", "سنة الإصدار", "Issue year", ReadParameterTypes.Integer,
                    min: AntiqueCatalog.MinYear, max: AntiqueCatalog.MaxYear),
                Param("isOriginal", "أصلية", "Original", ReadParameterTypes.Boolean),
                Param("isRare", "نادرة", "Rare", ReadParameterTypes.Boolean),
                Param("condition", "الحالة", "Condition", ReadParameterTypes.Enum,
                    optionsSource: "coinStampConditions", optionsValue: ReadOptionsValues.Id)
            ],
            [
                Operation("coinStampItemTypes", "نوع القطعة", "Item type",
                    $"{CoinStampRoute}/item-types"),
                Operation("coinStampMetals", "المعدن", "Metal", $"{CoinStampRoute}/metals"),
                Operation("coinStampConditions", "الحالة", "Condition", $"{CoinStampRoute}/conditions")
            ]);

    private static ReadConfigSchema ClothingModule(
        int categoryId, SubCategoryType subCategory, string module, string route,
        string listLabelAr, string listLabelEn, string detailsLabelAr, string detailsLabelEn,
        ListingModuleType listingType, string myListingsAr, string myListingsEn,
        IEnumerable<ReadParameterDto> filters, IEnumerable<ReadOperationDto> lookupOperations)
    {
        var queryParameters = new List<ReadParameterDto>
        {
            Param("search", "بحث", "Search", ReadParameterTypes.String),
            Param("storeName", "اسم المحل", "Store name", ReadParameterTypes.String)
        };

        queryParameters.AddRange(filters);
        queryParameters.Add(Param("center", "المدينة (المركز)", "City / center", ReadParameterTypes.String,
            optionsSource: ReadOperationKeys.Centers, optionsValue: ReadOptionsValues.Name));
        queryParameters.Add(Param("priceFrom", "السعر من", "Price from", ReadParameterTypes.Decimal, min: 0));
        queryParameters.Add(Param("priceTo", "السعر إلى", "Price to", ReadParameterTypes.Decimal, min: 0));
        queryParameters.AddRange(Paging());

        var operations = new List<ReadOperationDto>
        {
            Operation(ReadOperationKeys.List, listLabelAr, listLabelEn,
                route, paginated: true, queryParameters: queryParameters),

            Operation(ReadOperationKeys.Details, detailsLabelAr, detailsLabelEn,
                $"{route}/{{id}}", routeParameters: IdRoute()),

            UnifiedMyListings(listingType, myListingsAr, myListingsEn)
        };

        operations.AddRange(lookupOperations);
        operations.AddRange(SharedTail(categoryId, (int)subCategory));

        return new ReadConfigSchema(module, operations);
    }

    private static IEnumerable<ReadParameterDto> ClothingFilters(
        string typesKey, string brandsKey, string sizesKey, string colorsKey, string sellingMethodsKey) =>
    [
        Param("clothingType", "نوع الملابس", "Clothing type", ReadParameterTypes.Enum,
            optionsSource: typesKey, optionsValue: ReadOptionsValues.Id),
        Param("brand", "الماركة", "Brand", ReadParameterTypes.Enum,
            optionsSource: brandsKey, optionsValue: ReadOptionsValues.Id),
        Param("size", "المقاس", "Size", ReadParameterTypes.Enum,
            optionsSource: sizesKey, optionsValue: ReadOptionsValues.Id),
        Param("color", "اللون", "Color", ReadParameterTypes.Enum,
            optionsSource: colorsKey, optionsValue: ReadOptionsValues.Id),
        Param("sellingMethod", "طريقة البيع", "Selling method", ReadParameterTypes.Enum,
            optionsSource: sellingMethodsKey, optionsValue: ReadOptionsValues.Id)
    ];

    private static IEnumerable<ReadOperationDto> ClothingLookups(
        string route, string typesKey, string brandsKey, string sizesKey,
        string colorsKey, string conditionsKey, string sellingMethodsKey) =>
    [
        Operation(typesKey, "أنواع الملابس", "Clothing types", $"{route}/clothing-types"),
        Operation(brandsKey, "الماركات", "Brands", $"{route}/brands"),
        Operation(sizesKey, "المقاسات", "Sizes", $"{route}/sizes"),
        Operation(colorsKey, "الألوان", "Colors", $"{route}/colors"),
        Operation(conditionsKey, "الحالة", "Conditions", $"{route}/conditions"),
        Operation(sellingMethodsKey, "طرق البيع", "Selling methods", $"{route}/selling-methods")
    ];

    private static ReadConfigSchema MenClothing(int categoryId, SubCategoryType subCategory) =>
        ClothingModule(categoryId, subCategory, MenClothingModule, MenClothingRoute,
            "قائمة الملابس الرجالي", "Men clothing list",
            "تفاصيل إعلان الملابس الرجالي", "Men clothing details",
            ListingModuleType.MenClothing,
            "إعلانات الملابس الرجالي المضافة بواسطتي", "My men clothing listings",
            ClothingFilters("menClothingTypes", "menClothingBrands", "menClothingSizes",
                "menClothingColors", "menClothingSellingMethods"),
            ClothingLookups(MenClothingRoute, "menClothingTypes", "menClothingBrands",
                "menClothingSizes", "menClothingColors", "menClothingConditions",
                "menClothingSellingMethods"));

    private static ReadConfigSchema WomenClothing(int categoryId, SubCategoryType subCategory) =>
        ClothingModule(categoryId, subCategory, WomenClothingModule, WomenClothingRoute,
            "قائمة الملابس الحريمي", "Women clothing list",
            "تفاصيل إعلان الملابس الحريمي", "Women clothing details",
            ListingModuleType.WomenClothing,
            "إعلانات الملابس الحريمي المضافة بواسطتي", "My women clothing listings",
            ClothingFilters("womenClothingTypes", "womenClothingBrands", "womenClothingSizes",
                "womenClothingColors", "womenClothingSellingMethods"),
            ClothingLookups(WomenClothingRoute, "womenClothingTypes", "womenClothingBrands",
                "womenClothingSizes", "womenClothingColors", "womenClothingConditions",
                "womenClothingSellingMethods"));

    private static ReadConfigSchema KidsClothing(int categoryId, SubCategoryType subCategory) =>
        ClothingModule(categoryId, subCategory, KidsClothingModule, KidsClothingRoute,
            "قائمة ملابس الأطفال", "Kids clothing list",
            "تفاصيل إعلان ملابس الأطفال", "Kids clothing details",
            ListingModuleType.KidsClothing,
            "إعلانات ملابس الأطفال المضافة بواسطتي", "My kids clothing listings",
            ClothingFilters("kidsClothingTypes", "kidsClothingBrands", "kidsClothingSizes",
                "kidsClothingColors", "kidsClothingSellingMethods"),
            ClothingLookups(KidsClothingRoute, "kidsClothingTypes", "kidsClothingBrands",
                "kidsClothingSizes", "kidsClothingColors", "kidsClothingConditions",
                "kidsClothingSellingMethods"));

    private static ReadParameterDto OnlineShoppingSortParameter() =>
        Param("sortBy", "الترتيب", "Sort by", ReadParameterTypes.Enum,
            defaultValue: (int)OnlineShoppingSortBy.Newest,
            options: EnumOptions(
                (OnlineShoppingSortBy.Newest, "الأحدث", "Newest"),
                (OnlineShoppingSortBy.Oldest, "الأقدم", "Oldest"),
                (OnlineShoppingSortBy.PriceAsc, "السعر: من الأقل للأعلى", "Price: low to high"),
                (OnlineShoppingSortBy.PriceDesc, "السعر: من الأعلى للأقل", "Price: high to low")));

    private static ReadConfigSchema OnlineShoppingModule(
        int categoryId, SubCategoryType subCategory, string module, string route,
        string listLabelAr, string listLabelEn, string detailsLabelAr, string detailsLabelEn,
        ListingModuleType listingType, string myListingsAr, string myListingsEn,
        ReadParameterDto nameParameter,
        IEnumerable<ReadParameterDto> filters, IEnumerable<ReadOperationDto> lookupOperations)
    {
        var queryParameters = new List<ReadParameterDto>
        {
            Param("search", "بحث", "Search", ReadParameterTypes.String),
            nameParameter
        };

        queryParameters.AddRange(filters);
        queryParameters.Add(Param("priceFrom", "السعر من", "Price from", ReadParameterTypes.Decimal, min: 0));
        queryParameters.Add(Param("priceTo", "السعر إلى", "Price to", ReadParameterTypes.Decimal, min: 0));
        queryParameters.Add(OnlineShoppingSortParameter());
        queryParameters.AddRange(Paging());

        var operations = new List<ReadOperationDto>
        {
            Operation(ReadOperationKeys.List, listLabelAr, listLabelEn,
                route, paginated: true, queryParameters: queryParameters),

            Operation(ReadOperationKeys.Details, detailsLabelAr, detailsLabelEn,
                $"{route}/{{id}}", routeParameters: IdRoute()),

            UnifiedMyListings(listingType, myListingsAr, myListingsEn)
        };

        operations.AddRange(lookupOperations);
        operations.AddRange(SharedTail(categoryId, (int)subCategory));

        return new ReadConfigSchema(module, operations);
    }

    private static ReadParameterDto StoreNameParameter() =>
        Param("storeName", "اسم المحل", "Store name", ReadParameterTypes.String);

    private static ReadConfigSchema Accessories(int categoryId, SubCategoryType subCategory) =>
        OnlineShoppingModule(categoryId, subCategory, AccessoriesModule, AccessoriesRoute,
            "قائمة الإكسسوارات", "Accessories list",
            "تفاصيل إعلان الإكسسوار", "Accessory details",
            ListingModuleType.Accessory,
            "إعلانات الإكسسوارات المضافة بواسطتي", "My accessory listings",
            StoreNameParameter(),
            [
                Param("accessoryType", "نوع الإكسسوار", "Accessory type", ReadParameterTypes.Enum,
                    optionsSource: "accessoryTypes", optionsValue: ReadOptionsValues.Id),
                Param("category", "الفئة", "Category", ReadParameterTypes.Enum,
                    optionsSource: "accessoryCategories", optionsValue: ReadOptionsValues.Id),
                Param("material", "الخامة", "Material", ReadParameterTypes.Enum,
                    optionsSource: "accessoryMaterials", optionsValue: ReadOptionsValues.Id),
                Param("color", "اللون", "Color", ReadParameterTypes.Enum,
                    optionsSource: "accessoryColors", optionsValue: ReadOptionsValues.Id),
                Param("shippingAvailable", "الشحن متاح", "Shipping available", ReadParameterTypes.Boolean)
            ],
            [
                Operation("accessoryTypes", "أنواع الإكسسوارات", "Accessory types", $"{AccessoriesRoute}/types"),
                Operation("accessoryCategories", "الفئات", "Categories", $"{AccessoriesRoute}/categories"),
                Operation("accessoryMaterials", "الخامات", "Materials", $"{AccessoriesRoute}/materials"),
                Operation("accessoryColors", "الألوان", "Colors", $"{AccessoriesRoute}/colors")
            ]);

    private static ReadConfigSchema Cosmetics(int categoryId, SubCategoryType subCategory) =>
        OnlineShoppingModule(categoryId, subCategory, CosmeticsModule, CosmeticsRoute,
            "قائمة مستحضرات التجميل", "Cosmetics list",
            "تفاصيل إعلان مستحضرات التجميل", "Cosmetic details",
            ListingModuleType.Cosmetic,
            "إعلانات مستحضرات التجميل المضافة بواسطتي", "My cosmetic listings",
            StoreNameParameter(),
            [
                Param("section", "القسم", "Section", ReadParameterTypes.Enum,
                    optionsSource: "cosmeticSections", optionsValue: ReadOptionsValues.Id),

                Param("brand", "الماركة", "Brand", ReadParameterTypes.String),
                Param("suitableFor", "مناسب لـ", "Suitable for", ReadParameterTypes.Enum,
                    optionsSource: "cosmeticSuitableFor", optionsValue: ReadOptionsValues.Id),
                Param("discountAvailable", "يوجد خصم", "Discount available", ReadParameterTypes.Boolean),
                Param("shippingAvailable", "الشحن متاح", "Shipping available", ReadParameterTypes.Boolean)
            ],
            [
                Operation("cosmeticSections", "الأقسام", "Sections", $"{CosmeticsRoute}/sections"),
                Operation("cosmeticSuitableFor", "مناسب لـ", "Suitable for", $"{CosmeticsRoute}/suitable-for")
            ]);

    private static ReadConfigSchema HomeKitchen(int categoryId, SubCategoryType subCategory) =>
        OnlineShoppingModule(categoryId, subCategory, HomeKitchenModule, HomeKitchenRoute,
            "قائمة المنزل والمطبخ", "Home & kitchen list",
            "تفاصيل إعلان المنزل والمطبخ", "Home & kitchen details",
            ListingModuleType.HomeKitchen,
            "إعلانات المنزل والمطبخ المضافة بواسطتي", "My home & kitchen listings",
            StoreNameParameter(),
            [
                Param("section", "القسم", "Section", ReadParameterTypes.Enum,
                    optionsSource: "homeKitchenSections", optionsValue: ReadOptionsValues.Id),
                Param("material", "الخامة", "Material", ReadParameterTypes.Enum,
                    optionsSource: "homeKitchenMaterials", optionsValue: ReadOptionsValues.Id),
                Param("color", "اللون", "Color", ReadParameterTypes.Enum,
                    optionsSource: "homeKitchenColors", optionsValue: ReadOptionsValues.Id),
                Param("deliveryAvailable", "التوصيل متاح", "Delivery available", ReadParameterTypes.Boolean)
            ],
            [
                Operation("homeKitchenSections", "الأقسام", "Sections", $"{HomeKitchenRoute}/sections"),
                Operation("homeKitchenMaterials", "الخامات", "Materials", $"{HomeKitchenRoute}/materials"),
                Operation("homeKitchenColors", "الألوان", "Colors", $"{HomeKitchenRoute}/colors")
            ]);

    private static ReadConfigSchema ShoppingElectronics(int categoryId, SubCategoryType subCategory) =>
        OnlineShoppingModule(categoryId, subCategory, ShoppingElectronicsModule, ShoppingElectronicsRoute,
            "قائمة الإلكترونيات", "Electronics list",
            "تفاصيل إعلان الإلكترونيات", "Electronics details",
            ListingModuleType.ShoppingElectronic,
            "إعلانات الإلكترونيات المضافة بواسطتي", "My electronics listings",
            StoreNameParameter(),
            [
                Param("section", "القسم", "Section", ReadParameterTypes.Enum,
                    optionsSource: "shoppingElectronicSections", optionsValue: ReadOptionsValues.Id),

                Param("brand", "الماركة", "Brand", ReadParameterTypes.String),
                Param("compatibleWith", "متوافق مع", "Compatible with", ReadParameterTypes.Enum,
                    optionsSource: "shoppingElectronicCompatibilities", optionsValue: ReadOptionsValues.Id),
                Param("productCondition", "حالة المنتج", "Product condition", ReadParameterTypes.Enum,
                    optionsSource: "shoppingElectronicConditions", optionsValue: ReadOptionsValues.Id),
                Param("warranty", "الضمان", "Warranty", ReadParameterTypes.Enum,
                    optionsSource: "shoppingElectronicWarranties", optionsValue: ReadOptionsValues.Id),
                Param("shippingAvailable", "الشحن متاح", "Shipping available", ReadParameterTypes.Boolean)
            ],
            [
                Operation("shoppingElectronicSections", "الأقسام", "Sections",
                    $"{ShoppingElectronicsRoute}/sections"),
                Operation("shoppingElectronicCompatibilities", "متوافق مع", "Compatibilities",
                    $"{ShoppingElectronicsRoute}/compatibilities"),
                Operation("shoppingElectronicConditions", "حالة المنتج", "Conditions",
                    $"{ShoppingElectronicsRoute}/conditions"),
                Operation("shoppingElectronicWarranties", "الضمان", "Warranties",
                    $"{ShoppingElectronicsRoute}/warranties")
            ]);

    private static ReadConfigSchema GiftsToys(int categoryId, SubCategoryType subCategory) =>
        OnlineShoppingModule(categoryId, subCategory, GiftsToysModule, GiftsToysRoute,
            "قائمة الهدايا والألعاب", "Gifts & toys list",
            "تفاصيل إعلان الهدايا والألعاب", "Gifts & toys details",
            ListingModuleType.GiftToy,
            "إعلانات الهدايا والألعاب المضافة بواسطتي", "My gifts & toys listings",
            StoreNameParameter(),
            [
                Param("giftType", "النوع", "Gift type", ReadParameterTypes.Enum,
                    optionsSource: "giftToyTypes", optionsValue: ReadOptionsValues.Id),
                Param("suitableFor", "مناسب لـ", "Suitable for", ReadParameterTypes.Enum,
                    optionsSource: "giftToySuitableFor", optionsValue: ReadOptionsValues.Id),
                Param("giftWrapping", "تغليف هدايا", "Gift wrapping", ReadParameterTypes.Boolean),
                Param("deliveryAvailable", "التوصيل متاح", "Delivery available", ReadParameterTypes.Boolean)
            ],
            [
                Operation("giftToyTypes", "أنواع الهدايا", "Gift types", $"{GiftsToysRoute}/types"),
                Operation("giftToySuitableFor", "مناسب لـ", "Suitable for", $"{GiftsToysRoute}/suitable-for")
            ]);

    private static ReadConfigSchema HomemadeFood(int categoryId, SubCategoryType subCategory) =>
        OnlineShoppingModule(categoryId, subCategory, HomemadeFoodModule, HomemadeFoodRoute,
            "قائمة الأكل المنزلي", "Homemade food list",
            "تفاصيل إعلان الأكل المنزلي", "Homemade food details",
            ListingModuleType.HomemadeFood,
            "إعلانات الأكل المنزلي المضافة بواسطتي", "My homemade food listings",

            Param("projectName", "اسم المشروع", "Project name", ReadParameterTypes.String),
            [
                Param("section", "القسم", "Section", ReadParameterTypes.Enum,
                    optionsSource: "homemadeFoodSections", optionsValue: ReadOptionsValues.Id),
                Param("deliveryAvailable", "التوصيل متاح", "Delivery available", ReadParameterTypes.Boolean),
                Param("deliveryArea", "منطقة التوصيل", "Delivery area", ReadParameterTypes.Enum,
                    optionsSource: "homemadeFoodDeliveryAreas", optionsValue: ReadOptionsValues.Id),
                Param("preparedOnDemand", "يتم التحضير عند الطلب", "Prepared on demand",
                    ReadParameterTypes.Boolean)
            ],
            [
                Operation("homemadeFoodSections", "الأقسام", "Sections", $"{HomemadeFoodRoute}/sections"),
                Operation("homemadeFoodDeliveryAreas", "مناطق التوصيل", "Delivery areas",
                    $"{HomemadeFoodRoute}/delivery-areas")
            ]);

    private static ReadParameterDto HomeFurnishingSortParameter() =>
        Param("sortBy", "الترتيب", "Sort by", ReadParameterTypes.Enum,
            defaultValue: (int)HomeFurnishingSortBy.Newest,
            options: EnumOptions(
                (HomeFurnishingSortBy.Newest, "الأحدث", "Newest"),
                (HomeFurnishingSortBy.Oldest, "الأقدم", "Oldest"),
                (HomeFurnishingSortBy.PriceAsc, "السعر: من الأقل للأعلى", "Price: low to high"),
                (HomeFurnishingSortBy.PriceDesc, "السعر: من الأعلى للأقل", "Price: high to low"),
                (HomeFurnishingSortBy.MostViewed, "الأكثر مشاهدة", "Most viewed")));

    private static IEnumerable<ReadParameterDto> HomeFurnishingSharedFilters() =>
    [
        Param("center", "المدينة (المركز)", "City / center", ReadParameterTypes.String,
            optionsSource: ReadOperationKeys.Centers, optionsValue: ReadOptionsValues.Name),
        Param("priceFrom", "السعر من", "Price from", ReadParameterTypes.Decimal, min: 0),
        Param("priceTo", "السعر إلى", "Price to", ReadParameterTypes.Decimal, min: 0),
        Param("negotiable", "السعر قابل للتفاوض", "Negotiable", ReadParameterTypes.Boolean),
        Param("isFeatured", "إعلان مميز", "Featured", ReadParameterTypes.Boolean),
        Param("isPremium", "إعلان بريميوم", "Premium", ReadParameterTypes.Boolean),
        Param("isUrgent", "بيع سريع", "Urgent", ReadParameterTypes.Boolean)
    ];

    private static IEnumerable<ReadOperationDto> HomeFurnishingExtras(
        string route, IReadOnlyList<ReadParameterDto> listQueryParameters) =>
    [
        Operation("recentlyAdded", "أحدث الإعلانات", "Recently added", $"{route}/recently-added",
            queryParameters:
            [
                Param("count", "العدد", "Count", ReadParameterTypes.Integer,
                    defaultValue: 8, min: 1, max: 50)
            ]),

        Operation("priceStatistics", "إحصائيات الأسعار", "Price statistics",
            $"{route}/price-statistics", queryParameters: listQueryParameters),

        Operation("searchSuggestions", "اقتراحات البحث", "Search suggestions",
            $"{route}/search-suggestions",
            queryParameters:
            [
                Param("term", "كلمة البحث", "Search term", ReadParameterTypes.String, required: true),
                Param("count", "العدد", "Count", ReadParameterTypes.Integer,
                    defaultValue: 10, min: 1, max: 50)
            ])
    ];

    private static ReadConfigSchema HomeFurnishingModule(
        int categoryId, SubCategoryType subCategory, string module, string route,
        string listLabelAr, string listLabelEn, string detailsLabelAr, string detailsLabelEn,
        ListingModuleType listingType, string myListingsAr, string myListingsEn,
        IEnumerable<ReadParameterDto> filters, IEnumerable<ReadOperationDto> lookupOperations)
    {
        var queryParameters = new List<ReadParameterDto>
        {
            Param("search", "بحث", "Search", ReadParameterTypes.String)
        };

        queryParameters.AddRange(filters);
        queryParameters.AddRange(HomeFurnishingSharedFilters());
        queryParameters.Add(HomeFurnishingSortParameter());
        queryParameters.AddRange(Paging());

        var operations = new List<ReadOperationDto>
        {
            Operation(ReadOperationKeys.List, listLabelAr, listLabelEn,
                route, paginated: true, queryParameters: queryParameters),

            Operation(ReadOperationKeys.Details, detailsLabelAr, detailsLabelEn,
                $"{route}/{{id}}", routeParameters: IdRoute()),

            UnifiedMyListings(listingType, myListingsAr, myListingsEn)
        };

        operations.AddRange(lookupOperations);
        operations.AddRange(HomeFurnishingExtras(route, queryParameters));
        operations.AddRange(SharedTail(categoryId, (int)subCategory));

        return new ReadConfigSchema(module, operations);
    }

    private static ReadConfigSchema Furniture(int categoryId, SubCategoryType subCategory) =>
        HomeFurnishingModule(categoryId, subCategory, FurnitureModule, FurnitureRoute,
            "قائمة الأثاث", "Furniture list", "تفاصيل إعلان الأثاث", "Furniture details",
            ListingModuleType.Furniture, "إعلانات الأثاث المضافة بواسطتي", "My furniture listings",
            [
                Param("furnitureType", "نوع الأثاث", "Furniture type", ReadParameterTypes.Enum,
                    optionsSource: "furnitureTypes", optionsValue: ReadOptionsValues.Id),
                Param("material", "الخامة", "Material", ReadParameterTypes.Enum,
                    optionsSource: "furnitureMaterials", optionsValue: ReadOptionsValues.Id),
                Param("color", "اللون", "Color", ReadParameterTypes.Enum,
                    optionsSource: "furnitureColors", optionsValue: ReadOptionsValues.Id),
                Param("condition", "الحالة", "Condition", ReadParameterTypes.Enum,
                    optionsSource: "furnitureConditions", optionsValue: ReadOptionsValues.Id),
                Param("deliveryAvailable", "التوصيل متاح", "Delivery available", ReadParameterTypes.Boolean),
                Param("canBeDisassembled", "قابل للفك", "Can be disassembled", ReadParameterTypes.Boolean)
            ],
            [
                Operation("furnitureTypes", "أنواع الأثاث", "Furniture types", $"{FurnitureRoute}/furniture-types"),
                Operation("furnitureMaterials", "الخامات", "Materials", $"{FurnitureRoute}/materials"),
                Operation("furnitureColors", "الألوان", "Colors", $"{FurnitureRoute}/colors"),
                Operation("furnitureConditions", "الحالة", "Conditions", $"{FurnitureRoute}/conditions")
            ]);

    private static ReadConfigSchema FurnishingCurtains(int categoryId, SubCategoryType subCategory) =>
        HomeFurnishingModule(categoryId, subCategory, FurnishingCurtainModule, FurnishingCurtainRoute,
            "قائمة المفروشات والستائر", "Furnishings & curtains list",
            "تفاصيل إعلان المفروشات والستائر", "Furnishings & curtains details",
            ListingModuleType.FurnishingCurtain,
            "إعلانات المفروشات والستائر المضافة بواسطتي", "My furnishings & curtains listings",
            [
                Param("productType", "نوع المنتج", "Product type", ReadParameterTypes.Enum,
                    optionsSource: "furnishingCurtainProductTypes", optionsValue: ReadOptionsValues.Id),
                Param("size", "المقاس", "Size", ReadParameterTypes.Enum,
                    optionsSource: "furnishingCurtainSizes", optionsValue: ReadOptionsValues.Id),
                Param("material", "الخامة", "Material", ReadParameterTypes.Enum,
                    optionsSource: "furnishingCurtainMaterials", optionsValue: ReadOptionsValues.Id),
                Param("color", "اللون", "Color", ReadParameterTypes.Enum,
                    optionsSource: "furnishingCurtainColors", optionsValue: ReadOptionsValues.Id),
                Param("deliveryAvailable", "التوصيل متاح", "Delivery available", ReadParameterTypes.Boolean)
            ],
            [
                Operation("furnishingCurtainProductTypes", "أنواع المنتجات", "Product types",
                    $"{FurnishingCurtainRoute}/product-types"),
                Operation("furnishingCurtainSizes", "المقاسات", "Sizes", $"{FurnishingCurtainRoute}/sizes"),
                Operation("furnishingCurtainMaterials", "الخامات", "Materials",
                    $"{FurnishingCurtainRoute}/materials"),
                Operation("furnishingCurtainColors", "الألوان", "Colors", $"{FurnishingCurtainRoute}/colors")
            ]);

    private static ReadConfigSchema LightingDecor(int categoryId, SubCategoryType subCategory) =>
        HomeFurnishingModule(categoryId, subCategory, LightingDecorModule, LightingDecorRoute,
            "قائمة الإضاءة والديكور", "Lighting & decor list",
            "تفاصيل إعلان الإضاءة والديكور", "Lighting & decor details",
            ListingModuleType.LightingDecor,
            "إعلانات الإضاءة والديكور المضافة بواسطتي", "My lighting & decor listings",
            [
                Param("productType", "نوع المنتج", "Product type", ReadParameterTypes.Enum,
                    optionsSource: "lightingDecorProductTypes", optionsValue: ReadOptionsValues.Id),
                Param("material", "الخامة", "Material", ReadParameterTypes.Enum,
                    optionsSource: "lightingDecorMaterials", optionsValue: ReadOptionsValues.Id),
                Param("color", "اللون", "Color", ReadParameterTypes.Enum,
                    optionsSource: "lightingDecorColors", optionsValue: ReadOptionsValues.Id),
                Param("lightType", "نوع الإضاءة", "Light type", ReadParameterTypes.Enum,
                    optionsSource: "lightingDecorLightTypes", optionsValue: ReadOptionsValues.Id)
            ],
            [
                Operation("lightingDecorProductTypes", "أنواع المنتجات", "Product types",
                    $"{LightingDecorRoute}/product-types"),
                Operation("lightingDecorMaterials", "الخامات", "Materials", $"{LightingDecorRoute}/materials"),
                Operation("lightingDecorColors", "الألوان", "Colors", $"{LightingDecorRoute}/colors"),
                Operation("lightingDecorLightTypes", "أنواع الإضاءة", "Light types",
                    $"{LightingDecorRoute}/light-types")
            ]);

    private static ReadConfigSchema KitchenTools(int categoryId, SubCategoryType subCategory) =>
        HomeFurnishingModule(categoryId, subCategory, KitchenToolModule, KitchenToolRoute,
            "قائمة أدوات المطبخ", "Kitchen tools list",
            "تفاصيل إعلان أدوات المطبخ", "Kitchen tools details",
            ListingModuleType.KitchenTool,
            "إعلانات أدوات المطبخ المضافة بواسطتي", "My kitchen tools listings",
            [
                Param("productType", "نوع المنتج", "Product type", ReadParameterTypes.Enum,
                    optionsSource: "kitchenToolProductTypes", optionsValue: ReadOptionsValues.Id),
                Param("material", "الخامة", "Material", ReadParameterTypes.Enum,
                    optionsSource: "kitchenToolMaterials", optionsValue: ReadOptionsValues.Id),
                Param("color", "اللون", "Color", ReadParameterTypes.Enum,
                    optionsSource: "kitchenToolColors", optionsValue: ReadOptionsValues.Id)
            ],
            [
                Operation("kitchenToolProductTypes", "أنواع المنتجات", "Product types",
                    $"{KitchenToolRoute}/product-types"),
                Operation("kitchenToolMaterials", "الخامات", "Materials", $"{KitchenToolRoute}/materials"),
                Operation("kitchenToolColors", "الألوان", "Colors", $"{KitchenToolRoute}/colors")
            ]);

    private static ReadConfigSchema HomeAppliances(int categoryId, SubCategoryType subCategory) =>
        HomeFurnishingModule(categoryId, subCategory, HomeApplianceModule, HomeApplianceRoute,
            "قائمة الأجهزة الكهربائية المنزلية", "Home appliances list",
            "تفاصيل إعلان الأجهزة الكهربائية المنزلية", "Home appliance details",
            ListingModuleType.HomeAppliance,
            "إعلانات الأجهزة الكهربائية المضافة بواسطتي", "My home appliance listings",
            [
                Param("deviceType", "نوع الجهاز", "Device type", ReadParameterTypes.Enum,
                    optionsSource: "homeApplianceDeviceTypes", optionsValue: ReadOptionsValues.Id),
                Param("brand", "الماركة", "Brand", ReadParameterTypes.Enum,
                    optionsSource: "homeApplianceBrands", optionsValue: ReadOptionsValues.Id),
                Param("color", "اللون", "Color", ReadParameterTypes.Enum,
                    optionsSource: "homeApplianceColors", optionsValue: ReadOptionsValues.Id),
                Param("condition", "الحالة", "Condition", ReadParameterTypes.Enum,
                    optionsSource: "homeApplianceConditions", optionsValue: ReadOptionsValues.Id),
                Param("warranty", "الضمان", "Warranty", ReadParameterTypes.Enum,
                    optionsSource: "homeApplianceWarranties", optionsValue: ReadOptionsValues.Id),
                Param("deliveryAvailable", "التوصيل متاح", "Delivery available", ReadParameterTypes.Boolean)
            ],
            [
                Operation("homeApplianceDeviceTypes", "أنواع الأجهزة", "Device types",
                    $"{HomeApplianceRoute}/device-types"),
                Operation("homeApplianceBrands", "الماركات", "Brands", $"{HomeApplianceRoute}/brands"),
                Operation("homeApplianceConditions", "الحالة", "Conditions", $"{HomeApplianceRoute}/conditions"),
                Operation("homeApplianceWarranties", "الضمان", "Warranties", $"{HomeApplianceRoute}/warranties"),
                Operation("homeApplianceColors", "الألوان", "Colors", $"{HomeApplianceRoute}/colors")
            ]);

    private static ReadConfigSchema BathroomSupplies(int categoryId, SubCategoryType subCategory) =>
        HomeFurnishingModule(categoryId, subCategory, BathroomSupplyModule, BathroomSupplyRoute,
            "قائمة مستلزمات الحمام", "Bathroom supplies list",
            "تفاصيل إعلان مستلزمات الحمام", "Bathroom supply details",
            ListingModuleType.BathroomSupply,
            "إعلانات مستلزمات الحمام المضافة بواسطتي", "My bathroom supply listings",
            [
                Param("productType", "نوع المنتج", "Product type", ReadParameterTypes.Enum,
                    optionsSource: "bathroomSupplyProductTypes", optionsValue: ReadOptionsValues.Id),
                Param("material", "الخامة", "Material", ReadParameterTypes.Enum,
                    optionsSource: "bathroomSupplyMaterials", optionsValue: ReadOptionsValues.Id),
                Param("color", "اللون", "Color", ReadParameterTypes.Enum,
                    optionsSource: "bathroomSupplyColors", optionsValue: ReadOptionsValues.Id)
            ],
            [
                Operation("bathroomSupplyProductTypes", "أنواع المنتجات", "Product types",
                    $"{BathroomSupplyRoute}/product-types"),
                Operation("bathroomSupplyMaterials", "الخامات", "Materials", $"{BathroomSupplyRoute}/materials"),
                Operation("bathroomSupplyColors", "الألوان", "Colors", $"{BathroomSupplyRoute}/colors")
            ]);

    private static ReadConfigSchema PlantsOrnaments(int categoryId, SubCategoryType subCategory) =>
        HomeFurnishingModule(categoryId, subCategory, PlantOrnamentModule, PlantOrnamentRoute,
            "قائمة النباتات والزينة", "Plants & ornaments list",
            "تفاصيل إعلان النباتات والزينة", "Plant & ornament details",
            ListingModuleType.PlantOrnament,
            "إعلانات النباتات والزينة المضافة بواسطتي", "My plants & ornaments listings",
            [
                Param("productType", "نوع المنتج", "Product type", ReadParameterTypes.Enum,
                    optionsSource: "plantOrnamentProductTypes", optionsValue: ReadOptionsValues.Id),
                Param("suitableFor", "مناسب لـ", "Suitable for", ReadParameterTypes.Enum,
                    optionsSource: "plantOrnamentSuitableFor", optionsValue: ReadOptionsValues.Id),

                Param("heightFrom", "الارتفاع من", "Height from", ReadParameterTypes.Decimal, min: 0),
                Param("heightTo", "الارتفاع إلى", "Height to", ReadParameterTypes.Decimal, min: 0)
            ],
            [
                Operation("plantOrnamentProductTypes", "أنواع المنتجات", "Product types",
                    $"{PlantOrnamentRoute}/product-types"),
                Operation("plantOrnamentSuitableFor", "مناسب لـ", "Suitable for",
                    $"{PlantOrnamentRoute}/suitable-for")
            ]);

    private static ReadConfigSchema RealEstateModule(
        int categoryId, SubCategoryType subCategory, string module, string route,
        string listLabelAr, string listLabelEn, string detailsLabelAr, string detailsLabelEn,
        ListingModuleType listingType, string myListingsAr, string myListingsEn,
        IEnumerable<ReadParameterDto> filters, IEnumerable<ReadOperationDto> lookupOperations)
    {
        var queryParameters = new List<ReadParameterDto>
        {
            Param("search", "بحث", "Search", ReadParameterTypes.String),

            Param("listingType", "نوع الإعلان", "Listing type", ReadParameterTypes.Enum,
                optionsSource: RealEstateLookupKeys.ListingTypes, optionsValue: ReadOptionsValues.Id),

            Param("priceFrom", "السعر من", "Price from", ReadParameterTypes.Decimal, min: 0),
            Param("priceTo", "السعر إلى", "Price to", ReadParameterTypes.Decimal, min: 0)
        };

        queryParameters.AddRange(filters);

        queryParameters.Add(Param("governorate", "المحافظة", "Governorate", ReadParameterTypes.String,
            defaultValue: LocationConstants.Governorate,
            optionsSource: ReadOperationKeys.Governorates, optionsValue: ReadOptionsValues.Name));

        queryParameters.Add(Param("center", "المركز", "Center", ReadParameterTypes.String,
            optionsSource: ReadOperationKeys.Centers, optionsValue: ReadOptionsValues.Name));

        queryParameters.Add(Param("project", "المشروع", "Project", ReadParameterTypes.Enum,
            optionsSource: RealEstateLookupKeys.Projects, optionsValue: ReadOptionsValues.Id));

        queryParameters.Add(Param("negotiable", "السعر قابل للتفاوض", "Negotiable", ReadParameterTypes.Boolean));

        queryParameters.Add(Param("sortBy", "الترتيب", "Sort by", ReadParameterTypes.Enum,
            defaultValue: (int)RealEstateSortBy.Newest,
            options: EnumOptions(
                (RealEstateSortBy.Newest, "الأحدث", "Newest"),
                (RealEstateSortBy.Oldest, "الأقدم", "Oldest"),
                (RealEstateSortBy.PriceAsc, "السعر: من الأقل للأعلى", "Price: low to high"),
                (RealEstateSortBy.PriceDesc, "السعر: من الأعلى للأقل", "Price: high to low"),
                (RealEstateSortBy.MostViewed, "الأكثر مشاهدة", "Most viewed"))));

        queryParameters.AddRange(Paging());

        var operations = new List<ReadOperationDto>
        {
            Operation(ReadOperationKeys.List, listLabelAr, listLabelEn,
                route, paginated: true, queryParameters: queryParameters),

            Operation(ReadOperationKeys.Details, detailsLabelAr, detailsLabelEn,
                $"{route}/{{id}}", routeParameters: IdRoute()),

            UnifiedMyListings(listingType, myListingsAr, myListingsEn),

            Operation("similar", "إعلانات مشابهة", "Similar listings", $"{route}/{{id}}/similar",
                routeParameters: IdRoute()),
            Operation("related", "إعلانات ذات صلة", "Related listings", $"{route}/{{id}}/related",
                routeParameters: IdRoute()),
            Operation("recentlyAdded", "أحدث الإعلانات", "Recently added", $"{route}/recently-added"),
            Operation("priceStatistics", "إحصائيات الأسعار", "Price statistics",
                $"{route}/price-statistics"),
            Operation("searchSuggestions", "اقتراحات البحث", "Search suggestions",
                $"{route}/search-suggestions")
        };

        operations.Add(Operation(RealEstateLookupKeys.ListingTypes, "أنواع الإعلانات", "Listing types",
            $"{route}/{RealEstateLookupKeys.ListingTypes}"));
        operations.Add(Operation(RealEstateLookupKeys.Projects, "المشروعات", "Projects",
            $"{route}/{RealEstateLookupKeys.Projects}"));

        operations.AddRange(lookupOperations);
        operations.AddRange(SharedTail(categoryId, (int)subCategory));

        return new ReadConfigSchema(module, operations);
    }

    private static ReadOperationDto RealEstateLookup(
        string route, string key, string labelAr, string labelEn) =>
        Operation(key, labelAr, labelEn, $"{route}/{key}");

    private static ReadConfigSchema Lands(int categoryId, SubCategoryType subCategory) =>
        RealEstateModule(categoryId, subCategory, LandsModule, LandsRoute,
            "قائمة الأراضي", "Lands list",
            "تفاصيل إعلان الأرض", "Land details",
            ListingModuleType.Land,
            "إعلانات الأراضي المضافة بواسطتي", "My land listings",
            [
                Param("landType", "نوع الأرض", "Land type", ReadParameterTypes.Enum,
                    optionsSource: RealEstateLookupKeys.LandTypes, optionsValue: ReadOptionsValues.Id),

                Param("pricePerMeterFrom", "سعر المتر من", "Price per meter from",
                    ReadParameterTypes.Decimal, min: 0),
                Param("pricePerMeterTo", "سعر المتر إلى", "Price per meter to",
                    ReadParameterTypes.Decimal, min: 0),

                Param("areaFrom", "المساحة من", "Area from", ReadParameterTypes.Decimal, min: 0),
                Param("areaTo", "المساحة إلى", "Area to", ReadParameterTypes.Decimal, min: 0),
                Param("areaUnit", "وحدة المساحة", "Area unit", ReadParameterTypes.Enum,
                    optionsSource: RealEstateLookupKeys.AreaUnits, optionsValue: ReadOptionsValues.Id),

                Param("insideBuildingCordon", "داخل كردون المباني", "Inside building cordon",
                    ReadParameterTypes.Boolean),
                Param("isBuildable", "صالحة للبناء", "Buildable", ReadParameterTypes.Boolean),

                Param("legalStatus", "الحالة القانونية", "Legal status", ReadParameterTypes.Enum,
                    optionsSource: RealEstateLookupKeys.LegalStatuses, optionsValue: ReadOptionsValues.Id),
                Param("ownershipDocument", "نوع مستند الملكية", "Ownership document",
                    ReadParameterTypes.Enum,
                    optionsSource: RealEstateLookupKeys.OwnershipDocuments,
                    optionsValue: ReadOptionsValues.Id),

                Param("roadType", "نوع الطريق", "Road type", ReadParameterTypes.Enum,
                    optionsSource: RealEstateLookupKeys.RoadTypes, optionsValue: ReadOptionsValues.Id),
                Param("facadesCount", "عدد الواجهات", "Facades count", ReadParameterTypes.Enum,
                    optionsSource: RealEstateLookupKeys.FacadesCounts, optionsValue: ReadOptionsValues.Id),
                Param("direction", "اتجاه الأرض", "Land direction", ReadParameterTypes.Enum,
                    optionsSource: RealEstateLookupKeys.Directions, optionsValue: ReadOptionsValues.Id),

                Param("isCurrentlyCultivated", "الأرض مزروعة", "Currently cultivated",
                    ReadParameterTypes.Boolean),
                Param("irrigationSource", "مصدر الري", "Irrigation source", ReadParameterTypes.Enum,
                    optionsSource: RealEstateLookupKeys.IrrigationSources,
                    optionsValue: ReadOptionsValues.Id),
                Param("isOrganic", "الأرض عضوية", "Organic land", ReadParameterTypes.Boolean),
                Param("hasWell", "يوجد بئر", "Has a well", ReadParameterTypes.Boolean),
                Param("hasIrrigationNetwork", "يوجد شبكة ري", "Has an irrigation network",
                    ReadParameterTypes.Boolean),
                Param("hasFence", "يوجد سور", "Has a fence", ReadParameterTypes.Boolean)
            ],
            [
                RealEstateLookup(LandsRoute, RealEstateLookupKeys.LandTypes, "أنواع الأراضي", "Land types"),
                RealEstateLookup(LandsRoute, RealEstateLookupKeys.AreaUnits, "وحدات المساحة", "Area units"),
                RealEstateLookup(LandsRoute, RealEstateLookupKeys.FacadesCounts, "عدد الواجهات", "Facades counts"),
                RealEstateLookup(LandsRoute, RealEstateLookupKeys.Directions, "الاتجاهات", "Directions"),
                RealEstateLookup(LandsRoute, RealEstateLookupKeys.RoadTypes, "أنواع الطرق", "Road types"),
                RealEstateLookup(LandsRoute, RealEstateLookupKeys.LegalStatuses, "الحالة القانونية", "Legal statuses"),
                RealEstateLookup(LandsRoute, RealEstateLookupKeys.ReconciliationForms, "نماذج التصالح", "Reconciliation forms"),
                RealEstateLookup(LandsRoute, RealEstateLookupKeys.OwnershipDocuments, "مستندات الملكية", "Ownership documents"),
                RealEstateLookup(LandsRoute, RealEstateLookupKeys.Utilities, "المرافق", "Utilities"),
                RealEstateLookup(LandsRoute, RealEstateLookupKeys.RentTypes, "أنواع الإيجار", "Rent types"),
                RealEstateLookup(LandsRoute, RealEstateLookupKeys.MinimumRentPeriods, "الحد الأدنى لمدة الإيجار", "Minimum rent periods"),
                RealEstateLookup(LandsRoute, RealEstateLookupKeys.RentInclusions, "يشمل الإيجار", "Rent inclusions"),
                RealEstateLookup(LandsRoute, RealEstateLookupKeys.ContractDurations, "مدد العقد", "Contract durations"),
                RealEstateLookup(LandsRoute, RealEstateLookupKeys.ExchangeTargets, "خيارات البدل", "Exchange targets"),
                RealEstateLookup(LandsRoute, RealEstateLookupKeys.HarvestSeasons, "مواسم الحصاد", "Harvest seasons"),
                RealEstateLookup(LandsRoute, RealEstateLookupKeys.SoilTypes, "أنواع التربة", "Soil types"),
                RealEstateLookup(LandsRoute, RealEstateLookupKeys.IrrigationSources, "مصادر الري", "Irrigation sources"),
                RealEstateLookup(LandsRoute, RealEstateLookupKeys.QualityCertificates, "شهادات الجودة", "Quality certificates"),
                RealEstateLookup(LandsRoute, RealEstateLookupKeys.ExistingBuildingTypes, "أنواع المباني", "Existing building types"),
                RealEstateLookup(LandsRoute, RealEstateLookupKeys.BuildingCompletionRatios, "نسب تنفيذ المبنى", "Building completion ratios")
            ]);

    private static ReadConfigSchema Apartments(int categoryId, SubCategoryType subCategory) =>
        RealEstateModule(categoryId, subCategory, ApartmentsModule, ApartmentsRoute,
            "قائمة الشقق", "Apartments list",
            "تفاصيل إعلان الشقة", "Apartment details",
            ListingModuleType.Apartment,
            "إعلانات الشقق المضافة بواسطتي", "My apartment listings",
            [
                Param("apartmentType", "نوع الشقة", "Apartment type", ReadParameterTypes.Enum,
                    optionsSource: RealEstateLookupKeys.ApartmentTypes, optionsValue: ReadOptionsValues.Id),

                Param("areaFrom", "المساحة من", "Area from", ReadParameterTypes.Decimal, min: 0),
                Param("areaTo", "المساحة إلى", "Area to", ReadParameterTypes.Decimal, min: 0),

                Param("roomsCount", "عدد الغرف", "Rooms count", ReadParameterTypes.Integer, min: 1),
                Param("bathroomsCount", "عدد الحمامات", "Bathrooms count", ReadParameterTypes.Integer, min: 1),

                Param("floorType", "الدور", "Floor", ReadParameterTypes.Enum,
                    optionsSource: RealEstateLookupKeys.FloorTypes, optionsValue: ReadOptionsValues.Id),
                Param("finishingType", "التشطيب", "Finishing", ReadParameterTypes.Enum,
                    optionsSource: RealEstateLookupKeys.FinishingTypes, optionsValue: ReadOptionsValues.Id),
                Param("furnishedStatus", "مفروشة", "Furnished", ReadParameterTypes.Enum,
                    optionsSource: RealEstateLookupKeys.FurnishedStatuses, optionsValue: ReadOptionsValues.Id),

                Param("hasElevator", "مصعد", "Elevator", ReadParameterTypes.Boolean),
                Param("hasGarage", "جراج", "Garage", ReadParameterTypes.Boolean),
                Param("hasNaturalGas", "غاز طبيعي", "Natural gas", ReadParameterTypes.Boolean),
                Param("hasAirConditioning", "تكييف", "Air conditioning", ReadParameterTypes.Boolean),
                Param("hasBalcony", "بلكونة", "Balcony", ReadParameterTypes.Boolean),

                Param("ownershipType", "نوع الملكية", "Ownership type", ReadParameterTypes.Enum,
                    optionsSource: RealEstateLookupKeys.OwnershipTypes, optionsValue: ReadOptionsValues.Id),
                Param("legalStatus", "الحالة القانونية", "Legal status", ReadParameterTypes.Enum,
                    optionsSource: RealEstateLookupKeys.LegalStatuses, optionsValue: ReadOptionsValues.Id),
                Param("ownershipDocument", "مستند الملكية", "Ownership document", ReadParameterTypes.Enum,
                    optionsSource: RealEstateLookupKeys.OwnershipDocuments,
                    optionsValue: ReadOptionsValues.Id)
            ],
            [
                RealEstateLookup(ApartmentsRoute, RealEstateLookupKeys.ApartmentTypes, "أنواع الشقق", "Apartment types"),
                RealEstateLookup(ApartmentsRoute, RealEstateLookupKeys.OwnershipTypes, "أنواع الملكية", "Ownership types"),
                RealEstateLookup(ApartmentsRoute, RealEstateLookupKeys.ReceptionPieces, "قطع الريسبشن", "Reception pieces"),
                RealEstateLookup(ApartmentsRoute, RealEstateLookupKeys.FloorTypes, "الأدوار", "Floor types"),
                RealEstateLookup(ApartmentsRoute, RealEstateLookupKeys.FurnishedStatuses, "حالة الفرش", "Furnished statuses"),
                RealEstateLookup(ApartmentsRoute, RealEstateLookupKeys.FinishingTypes, "أنواع التشطيب", "Finishing types"),
                RealEstateLookup(ApartmentsRoute, RealEstateLookupKeys.PropertyAges, "عمر العقار", "Property ages"),
                RealEstateLookup(ApartmentsRoute, RealEstateLookupKeys.Directions, "الاتجاهات", "Directions"),
                RealEstateLookup(ApartmentsRoute, RealEstateLookupKeys.ViewTypes, "الإطلالة", "View types"),
                RealEstateLookup(ApartmentsRoute, RealEstateLookupKeys.LegalStatuses, "الحالة القانونية", "Legal statuses"),
                RealEstateLookup(ApartmentsRoute, RealEstateLookupKeys.ReconciliationForms, "نماذج التصالح", "Reconciliation forms"),
                RealEstateLookup(ApartmentsRoute, RealEstateLookupKeys.OwnershipDocuments, "مستندات الملكية", "Ownership documents"),
                RealEstateLookup(ApartmentsRoute, RealEstateLookupKeys.Features, "المرافق والمميزات", "Features"),
                RealEstateLookup(ApartmentsRoute, RealEstateLookupKeys.PaymentMethods, "طرق السداد", "Payment methods"),
                RealEstateLookup(ApartmentsRoute, RealEstateLookupKeys.InstallmentProviders, "جهات التقسيط", "Installment providers"),
                RealEstateLookup(ApartmentsRoute, RealEstateLookupKeys.RentTypes, "أنواع الإيجار", "Rent types"),
                RealEstateLookup(ApartmentsRoute, RealEstateLookupKeys.RentInclusions, "يشمل الإيجار", "Rent inclusions"),
                RealEstateLookup(ApartmentsRoute, RealEstateLookupKeys.SuitableFor, "مناسب لـ", "Suitable for"),
                RealEstateLookup(ApartmentsRoute, RealEstateLookupKeys.ExchangeTargets, "خيارات البدل", "Exchange targets")
            ]);

    private static ReadConfigSchema Shops(int categoryId, SubCategoryType subCategory) =>
        RealEstateModule(categoryId, subCategory, ShopsModule, ShopsRoute,
            "قائمة المحلات", "Shops list",
            "تفاصيل إعلان المحل", "Shop details",
            ListingModuleType.Shop,
            "إعلانات المحلات المضافة بواسطتي", "My shop listings",
            [
                Param("suitableActivity", "النشاط المناسب", "Suitable activity", ReadParameterTypes.Enum,
                    optionsSource: RealEstateLookupKeys.SuitableActivities,
                    optionsValue: ReadOptionsValues.Id),

                Param("areaFrom", "المساحة من", "Area from", ReadParameterTypes.Decimal, min: 0),
                Param("areaTo", "المساحة إلى", "Area to", ReadParameterTypes.Decimal, min: 0),

                Param("floorType", "الدور", "Floor", ReadParameterTypes.Enum,
                    optionsSource: RealEstateLookupKeys.FloorTypes, optionsValue: ReadOptionsValues.Id),
                Param("finishingType", "التشطيب", "Finishing", ReadParameterTypes.Enum,
                    optionsSource: RealEstateLookupKeys.FinishingTypes, optionsValue: ReadOptionsValues.Id),
                Param("facadesCount", "عدد الواجهات", "Facades count", ReadParameterTypes.Enum,
                    optionsSource: RealEstateLookupKeys.FacadesCounts, optionsValue: ReadOptionsValues.Id),

                Param("hasStorage", "يوجد مخزن", "Has storage", ReadParameterTypes.Boolean),
                Param("hasBathroom", "يوجد حمام", "Has a bathroom", ReadParameterTypes.Boolean),

                Param("isLicensed", "مرخص", "Licensed", ReadParameterTypes.Boolean),
                Param("isReconciliation", "تصالح", "Reconciliation", ReadParameterTypes.Boolean),

                Param("licenseType", "نوع الرخصة", "License type", ReadParameterTypes.Enum,
                    optionsSource: RealEstateLookupKeys.LicenseTypes, optionsValue: ReadOptionsValues.Id),
                Param("ownershipDocument", "مستند الملكية", "Ownership document", ReadParameterTypes.Enum,
                    optionsSource: RealEstateLookupKeys.OwnershipDocuments,
                    optionsValue: ReadOptionsValues.Id),

                Param("hasParking", "موقف سيارات", "Parking", ReadParameterTypes.Boolean),
                Param("hasAirConditioning", "تكييف", "Air conditioning", ReadParameterTypes.Boolean),
                Param("hasNaturalGas", "غاز طبيعي", "Natural gas", ReadParameterTypes.Boolean),
                Param("hasSurveillanceCameras", "كاميرات مراقبة", "Surveillance cameras",
                    ReadParameterTypes.Boolean)
            ],
            [
                RealEstateLookup(ShopsRoute, RealEstateLookupKeys.SuitableActivities, "الأنشطة المناسبة", "Suitable activities"),
                RealEstateLookup(ShopsRoute, RealEstateLookupKeys.FloorTypes, "الأدوار", "Floor types"),
                RealEstateLookup(ShopsRoute, RealEstateLookupKeys.FacadesCounts, "عدد الواجهات", "Facades counts"),
                RealEstateLookup(ShopsRoute, RealEstateLookupKeys.FacadeDirections, "اتجاه الواجهة", "Facade directions"),
                RealEstateLookup(ShopsRoute, RealEstateLookupKeys.FinishingTypes, "أنواع التشطيب", "Finishing types"),
                RealEstateLookup(ShopsRoute, RealEstateLookupKeys.PropertyAges, "عمر العقار", "Property ages"),
                RealEstateLookup(ShopsRoute, RealEstateLookupKeys.EntrancesCounts, "عدد المداخل", "Entrances counts"),
                RealEstateLookup(ShopsRoute, RealEstateLookupKeys.LegalStatuses, "حالة المحل", "Legal statuses"),
                RealEstateLookup(ShopsRoute, RealEstateLookupKeys.LicenseTypes, "أنواع الرخص", "License types"),
                RealEstateLookup(ShopsRoute, RealEstateLookupKeys.ReconciliationForms, "نماذج التصالح", "Reconciliation forms"),
                RealEstateLookup(ShopsRoute, RealEstateLookupKeys.OwnershipDocuments, "مستندات الملكية", "Ownership documents"),
                RealEstateLookup(ShopsRoute, RealEstateLookupKeys.Utilities, "المرافق", "Utilities"),
                RealEstateLookup(ShopsRoute, RealEstateLookupKeys.PaymentMethods, "طرق السداد", "Payment methods"),
                RealEstateLookup(ShopsRoute, RealEstateLookupKeys.InstallmentProviders, "جهات التقسيط", "Installment providers"),
                RealEstateLookup(ShopsRoute, RealEstateLookupKeys.RentTypes, "أنواع الإيجار", "Rent types"),
                RealEstateLookup(ShopsRoute, RealEstateLookupKeys.RentInclusions, "يشمل الإيجار", "Rent inclusions"),
                RealEstateLookup(ShopsRoute, RealEstateLookupKeys.RentSuitableActivities, "مناسب للنشاط", "Rent suitable activities"),
                RealEstateLookup(ShopsRoute, RealEstateLookupKeys.ExchangeTargets, "خيارات البدل", "Exchange targets")
            ]);

    private static ReadConfigSchema CharityModule(
        int categoryId, SubCategoryType subCategory, string module, string route,
        string listLabelAr, string listLabelEn, string detailsLabelAr, string detailsLabelEn,
        ListingModuleType listingType, string myListingsAr, string myListingsEn,
        IEnumerable<ReadParameterDto> filters, IEnumerable<ReadOperationDto> extraOperations)
    {
        var queryParameters = new List<ReadParameterDto>
        {
            Param("search", "بحث", "Search", ReadParameterTypes.String)
        };

        queryParameters.AddRange(filters);

        if (listingType != ListingModuleType.AskConsult)
        {
            queryParameters.Add(Param("hasLocation", "محدد على الخريطة", "Has a map location",
                ReadParameterTypes.Boolean));
        }

        queryParameters.Add(Param("sortBy", "الترتيب", "Sort by", ReadParameterTypes.Enum,
            defaultValue: (int)CharitySortBy.Newest,
            options: CharitySortOptions(includeMostLiked: listingType == ListingModuleType.AskConsult)));

        queryParameters.AddRange(Paging());

        var operations = new List<ReadOperationDto>
        {
            Operation(ReadOperationKeys.List, listLabelAr, listLabelEn,
                route, paginated: true, queryParameters: queryParameters),

            Operation(ReadOperationKeys.Details, detailsLabelAr, detailsLabelEn,
                $"{route}/{{id}}", routeParameters: IdRoute()),

            UnifiedMyListings(listingType, myListingsAr, myListingsEn)
        };

        operations.AddRange(extraOperations);
        operations.AddRange(SharedTail(categoryId, (int)subCategory));

        return new ReadConfigSchema(module, operations);
    }

    private static List<FormFieldOptionDto> CharitySortOptions(bool includeMostLiked) =>
        CharityCatalog.SortOptions
            .Where(option => includeMostLiked || option.Id != CharitySortBy.MostLiked)
            .Select(option => new FormFieldOptionDto((int)option.Id, option.Name, option.NameEn))
            .ToList();

    private static ReadConfigSchema Rescues(int categoryId, SubCategoryType subCategory) =>
        CharityModule(categoryId, subCategory, RescuesModule, RescuesRoute,
            "قائمة الاستغاثات", "Rescues list",
            "تفاصيل الاستغاثة", "Rescue details",
            ListingModuleType.Rescue,
            "الاستغاثات المضافة بواسطتي", "My rescues",
            [],
            []);

    private static ReadConfigSchema BloodRequests(int categoryId, SubCategoryType subCategory) =>
        CharityModule(categoryId, subCategory, BloodRequestsModule, BloodRequestsRoute,
            "قائمة طلبات فصائل الدم", "Blood requests list",
            "تفاصيل طلب فصيلة الدم", "Blood request details",
            ListingModuleType.BloodRequest,
            "طلبات فصائل الدم المضافة بواسطتي", "My blood requests",
            [
                Param("bloodGroup", "فصيلة الدم", "Blood group", ReadParameterTypes.Enum,
                    optionsSource: ReadOperationKeys.BloodGroups, optionsValue: ReadOptionsValues.Id),
                Param("center", "المركز", "Center", ReadParameterTypes.String,
                    optionsSource: ReadOperationKeys.Centers, optionsValue: ReadOptionsValues.Name)
            ],
            [
                Operation(ReadOperationKeys.BloodGroups, "فصائل الدم", "Blood groups",
                    $"{BloodRequestsRoute}/blood-groups")
            ]);

    private static ReadConfigSchema AskConsults(int categoryId, SubCategoryType subCategory) =>
        CharityModule(categoryId, subCategory, AskConsultsModule, AskConsultsRoute,
            "قائمة الأسئلة والاستشارات", "Ask & consult list",
            "تفاصيل السؤال", "Question details",
            ListingModuleType.AskConsult,
            "الأسئلة المضافة بواسطتي", "My questions",
            [

                Param("category", "مجال السؤال", "Question field", ReadParameterTypes.Enum,
                    optionsSource: ReadOperationKeys.AskConsultCategories,
                    optionsValue: ReadOptionsValues.Id)
            ],
            [
                Operation(ReadOperationKeys.Comments, "تعليقات السؤال", "Question comments",
                    $"{AskConsultsRoute}/{{id}}/comments", paginated: true,
                    routeParameters: IdRoute(), queryParameters: Paging().ToList()),

                Operation(ReadOperationKeys.AskConsultCategories, "مجالات الأسئلة", "Question fields",
                    $"{AskConsultsRoute}/categories")
            ]);

    private static IEnumerable<ReadOperationDto> SharedTail(int categoryId, int subCategoryId)
    {
        yield return Governorates();
        yield return Centers();
        yield return Categories();
        yield return SubCategories(categoryId);

        foreach (var operation in Interactions(subCategoryId))
            yield return operation;

        yield return CreateForm(categoryId, subCategoryId);
        yield return ReadConfig(categoryId, subCategoryId);
    }

    private static IEnumerable<ReadOperationDto> Interactions(int subCategoryId)
    {
        if (ListingModuleCatalog.ModuleOf(subCategoryId) is not { } module)
            yield break;

        var pinned = new Dictionary<string, object> { ["type"] = (int)module };

        yield return Operation("listingActions", "إجراءات الإعلان", "Listing actions",
            $"{InteractionsRoute}/{{id}}/actions", routeParameters: IdRoute(), query: pinned);

        yield return Operation("similarListings", "إعلانات مشابهة", "Similar listings",
            $"{InteractionsRoute}/{{id}}/similar", paginated: true,
            routeParameters: IdRoute(), queryParameters: Paging().ToList(), query: pinned);

        yield return Operation("recentlyViewed", "شوهد مؤخرًا", "Recently viewed",
            $"{InteractionsRoute}/recently-viewed", requiresAuthentication: true, paginated: true,
            queryParameters: Paging().ToList(), query: pinned);

        yield return Operation("favorites", "المفضلة", "Favorites",
            $"{InteractionsRoute}/favorites", requiresAuthentication: true, paginated: true,
            queryParameters: Paging().ToList(), query: pinned);

        yield return Operation("interactionMetadata", "بيانات التفاعلات", "Interaction metadata",
            $"{InteractionsRoute}/metadata");
    }

    private static ReadOperationDto UnifiedMyListings(
        ListingModuleType module, string label, string labelEn) =>
        Operation(ReadOperationKeys.MyListings, label, labelEn,
            $"{ProfileRoute}/my-listings", requiresAuthentication: true, paginated: true,
            queryParameters:
            [
                Param("type", "نوع الإعلان", "Listing module", ReadParameterTypes.Enum,
                    defaultValue: (int)module,
                    options: EnumOptions(
                        (ListingModuleType.Advertisement, "إعلان", "Advertisement"),
                        (ListingModuleType.Craftsman, "حرفي", "Craftsman"),
                        (ListingModuleType.Workshop, "ورشة", "Workshop"),
                        (ListingModuleType.Factory, "مصنع", "Factory"),
                        (ListingModuleType.Farm, "مزرعة", "Farm"),
                        (ListingModuleType.Company, "شركة", "Company"),
                        (ListingModuleType.Supplier, "مورد", "Supplier"),
                        (ListingModuleType.WholesaleTrader, "تاجر جملة", "Wholesale trader"),
                        (ListingModuleType.FruitVegetableMerchant, "تاجر خضر وفاكهة", "Fruit & vegetable merchant"),
                        (ListingModuleType.JobRequest, "طلب عمل", "Job request"),
                        (ListingModuleType.JobOpportunity, "فرصة عمل", "Job opportunity"),
                        (ListingModuleType.Livestock, "المواشي", "Livestock"),
                        (ListingModuleType.SheepGoat, "الأغنام والماعز", "Sheep & Goats"),
                        (ListingModuleType.Horse, "الخيول", "Horses"),
                        (ListingModuleType.Camel, "الإبل", "Camels"),
                        (ListingModuleType.Bird, "الطيور", "Birds"),
                        (ListingModuleType.Pet, "الحيوانات الأليفة", "Pets"),
                        (ListingModuleType.Fish, "الأسماك", "Fish"),
                        (ListingModuleType.Bee, "النحل", "Bees"),
                        (ListingModuleType.OtherAnimal, "حيوانات أخرى", "Other Animals"),
                        (ListingModuleType.DecorAntique, "تحف", "Antiques Decor"),
                        (ListingModuleType.Antique, "أنتيكات", "Antiques"),
                        (ListingModuleType.Painting, "لوحات فنية", "Paintings"),
                        (ListingModuleType.Handmade, "أعمال يدوية", "Handmade"),
                        (ListingModuleType.CoinStamp, "عملات وطوابع", "Coins & Stamps"),
                        (ListingModuleType.MenClothing, "ملابس رجالي", "Men Clothing"),
                        (ListingModuleType.WomenClothing, "ملابس حريمي", "Women Clothing"),
                        (ListingModuleType.KidsClothing, "ملابس أطفال", "Kids Clothing"),
                        (ListingModuleType.Accessory, "إكسسوارات", "Accessories"),
                        (ListingModuleType.Cosmetic, "مستحضرات التجميل", "Cosmetics"),
                        (ListingModuleType.HomeKitchen, "المنزل والمطبخ", "Home & Kitchen"),
                        (ListingModuleType.ShoppingElectronic, "إلكترونيات", "Electronics"),
                        (ListingModuleType.GiftToy, "هدايا وألعاب", "Gifts & Toys"),
                        (ListingModuleType.HomemadeFood, "أكل منزلي", "Homemade Food"),
                        (ListingModuleType.Furniture, "أثاث", "Furniture"),
                        (ListingModuleType.FurnishingCurtain, "مفروشات وستائر", "Furnishings & Curtains"),
                        (ListingModuleType.LightingDecor, "إضاءة وديكور", "Lighting & Decor"),
                        (ListingModuleType.KitchenTool, "أدوات المطبخ", "Kitchen Tools"),
                        (ListingModuleType.HomeAppliance, "أجهزة كهربائية منزلية", "Home Appliances"),
                        (ListingModuleType.BathroomSupply, "مستلزمات الحمام", "Bathroom Supplies"),
                        (ListingModuleType.PlantOrnament, "نباتات وزينة", "Plants & Ornaments"),
                        (ListingModuleType.Land, "أراضي", "Lands"),
                        (ListingModuleType.Apartment, "شقق", "Apartments"),
                        (ListingModuleType.Shop, "محلات", "Shops"),
                        (ListingModuleType.Rescue, "الاستغاثة", "Rescues"),
                        (ListingModuleType.BloodRequest, "فصائل الدم", "Blood requests"),
                        (ListingModuleType.AskConsult, "اسأل واستشير", "Ask & consult"),
                        (ListingModuleType.LostItem, "ضايع مني", "Lost items"),
                        (ListingModuleType.FoundItem, "لقيت", "Found items"))),
                Param("status", "الحالة", "Status", ReadParameterTypes.Enum,
                    options: EnumOptions(
                        (ListingStatus.Pending, "قيد المراجعة", "Pending"),
                        (ListingStatus.Active, "نشط", "Active"),
                        (ListingStatus.Expired, "منتهي", "Expired"),
                        (ListingStatus.Rejected, "مرفوض", "Rejected"))),
                Param("pageIndex", "رقم الصفحة", "Page index", ReadParameterTypes.Integer,
                    defaultValue: 1, min: 1),
                Param("pageSize", "حجم الصفحة", "Page size", ReadParameterTypes.Integer,
                    defaultValue: 10, min: 1, max: 50)
            ],
            query: new Dictionary<string, object> { ["type"] = (int)module });

    private static IReadOnlyDictionary<string, object> Selection(int categoryId, int subCategoryId) =>
        new Dictionary<string, object>
        {
            ["categoryId"] = categoryId,
            ["subCategoryId"] = subCategoryId
        };
}
