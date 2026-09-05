using Shared.Constants;
using Shared.DTOs.Advertisements;
using Shared.DTOs.Lookups;
using Shared.DTOs.FruitVegetableMerchants;
using Shared.DTOs.WholesaleTraders;
using Shared.DTOs.Suppliers;
using Shared.DTOs.Lookups.Forms;
using Shared.Enums;

namespace Services.AdForms;

public static class CreateAdFormExamples
{
    public sealed record Example(string Key, string Summary, CreateAdFormDto Form);

    public static IReadOnlyList<Example> All { get; } = Build();

    private static IReadOnlyList<Example> Build() =>
    [
        Create("cars-private", "Cars → Private (سيارات → ملاكي)",
            CategoryType.Cars, SubCategoryType.Private),
        Create("cars-heavy-equipment", "Cars → Heavy Equipment (سيارات → لوادر ومعدات ثقيلة)",
            CategoryType.Cars, SubCategoryType.HeavyEquipment),
        Create("cars-motorcycles", "Cars → Motorcycles (سيارات → موتوسيكلات)",
            CategoryType.Cars, SubCategoryType.Motorcycles),
        Create("cars-taxi", "Cars → Taxi (سيارات → أجرة)",
            CategoryType.Cars, SubCategoryType.Taxi),
        Create("workshops", "Workshops (الورش)",
            CategoryType.WorkshopsAndCraftsmen, SubCategoryType.Workshops),
        Create("craftsmen", "Craftsmen (الحرفيين)",
            CategoryType.WorkshopsAndCraftsmen, SubCategoryType.Craftsmen),
        Create("lost", "Lost & Found → Lost (المفقودات → ضايع مني)",
            CategoryType.LostAndFound, SubCategoryType.LostItems),
        Create("found", "Lost & Found → Found (المفقودات → لقيت)",
            CategoryType.LostAndFound, SubCategoryType.FoundItems)
,
        Create("business-factories", "Business → Factories (رجال أعمال → المصانع)",
            CategoryType.Business, SubCategoryType.Factories),
        Create("business-farms", "Business → Farms (رجال أعمال → المزارع)",
            CategoryType.Business, SubCategoryType.Farms),
        Create("business-companies", "Business → Companies (رجال أعمال → الشركات)",
            CategoryType.Business, SubCategoryType.Companies),
        Create("business-suppliers", "Business → Suppliers (رجال أعمال → الموردون)",
            CategoryType.Business, SubCategoryType.Suppliers),
        Create("business-wholesale-traders", "Business → Wholesale Traders (رجال أعمال → تجار الجملة)",
            CategoryType.Business, SubCategoryType.WholesaleTraders),
        Create("business-fruit-vegetable-merchants", "Business → Fruit & Vegetable Merchants (رجال أعمال → تجار خضر وفاكهة)",
            CategoryType.Business, SubCategoryType.FruitAndVegetableTraders),

        Create("jobs-job-requests", "Jobs → Job Requests (الوظائف → طلبات عمل)",
            CategoryType.Jobs, SubCategoryType.JobRequests),
        Create("jobs-job-opportunities", "Jobs → Job Opportunities (الوظائف → فرص عمل)",
            CategoryType.Jobs, SubCategoryType.JobOpportunities),

        Create("animals-livestock", "Animals → Livestock (الحيوانات → المواشي)",
            CategoryType.Animals, SubCategoryType.Livestock),
        Create("animals-sheep-goats", "Animals → Sheep & Goats (الحيوانات → الأغنام والماعز)",
            CategoryType.Animals, SubCategoryType.SheepAndGoats),
        Create("animals-horses", "Animals → Horses (الحيوانات → الخيول)",
            CategoryType.Animals, SubCategoryType.Horses),
        Create("animals-camels", "Animals → Camels (الحيوانات → الإبل)",
            CategoryType.Animals, SubCategoryType.Camels),
        Create("animals-birds", "Animals → Birds (الحيوانات → الطيور)",
            CategoryType.Animals, SubCategoryType.Birds),
        Create("animals-pets", "Animals → Pets (الحيوانات → الحيوانات الأليفة)",
            CategoryType.Animals, SubCategoryType.Pets),
        Create("animals-fish", "Animals → Fish (الحيوانات → الأسماك)",
            CategoryType.Animals, SubCategoryType.Fish),
        Create("animals-bees", "Animals → Bees (الحيوانات → النحل)",
            CategoryType.Animals, SubCategoryType.Bees),
        Create("animals-other", "Animals → Other Animals (الحيوانات → حيوانات أخرى)",
            CategoryType.Animals, SubCategoryType.OtherAnimals),

        Create("antiques-decor", "Antiques → Antiques Decor (التحف والأنتيكات → تحف)",
            CategoryType.Antiques, SubCategoryType.DecorAntiques),
        Create("antiques-antiques", "Antiques → Antiques (التحف والأنتيكات → أنتيكات)",
            CategoryType.Antiques, SubCategoryType.Antiques),
        Create("antiques-paintings", "Antiques → Paintings (التحف والأنتيكات → لوحات فنية)",
            CategoryType.Antiques, SubCategoryType.Paintings),
        Create("antiques-handmade", "Antiques → Handmade (التحف والأنتيكات → أعمال يدوية)",
            CategoryType.Antiques, SubCategoryType.Handmade),
        Create("antiques-coins-stamps", "Antiques → Coins & Stamps (التحف والأنتيكات → عملات وطوابع)",
            CategoryType.Antiques, SubCategoryType.CoinsAndStamps),

        Create("clothing-men", "Clothing → Men Clothing (الملابس → ملابس رجالي)",
            CategoryType.Clothing, SubCategoryType.MenClothing),
        Create("clothing-women", "Clothing → Women Clothing (الملابس → ملابس حريمي)",
            CategoryType.Clothing, SubCategoryType.WomenClothing),
        Create("clothing-kids", "Clothing → Kids Clothing (الملابس → ملابس أطفال)",
            CategoryType.Clothing, SubCategoryType.KidsClothing)
    ];

    private static Example Create(string key, string summary, CategoryType category, SubCategoryType subCategory)
    {
        var schema = AdFormSchemaCatalog.GetSchema((int)category, (int)subCategory)!;

        var form = new CreateAdFormDto
        {
            Category = CategoryExample(category),
            SubCategory = SubCategoryExample(category, subCategory),
            RequiresSubCategory = false,
            Module = schema.Module,
            Submit = schema.Submit,
            Lookups = SampleLookups(schema.RequiredLookups),
            Fields = schema.Fields
        };

        return new Example(key, summary, form);
    }

    private static CreateAdFormLookupsDto SampleLookups(IReadOnlyCollection<string> requiredLookups)
    {
        var lookups = new CreateAdFormLookupsDto();

        foreach (var lookupKey in requiredLookups)
        {
            switch (lookupKey)
            {
                case AdFormLookupKeys.ListingTypes:
                    lookups.ListingTypes = AdvertisementCatalog.ListingTypeNames
                        .Select(entry => new ListingTypeDto { Id = (int)entry.Key, Name = entry.Value })
                        .ToList();
                    break;

                case AdFormLookupKeys.Features:
                    lookups.Features =
                    [
                        new FeatureDto { Id = 1, Name = "ABS" },
                        new FeatureDto { Id = 5, Name = "Airbags" },
                        new FeatureDto { Id = 19, Name = "تكييف" }
                    ];
                    break;

                case AdFormLookupKeys.Governorates:
                    lookups.Governorates =
                    [
                        new GovernorateDto { Id = 1, Name = LocationConstants.Governorate }
                    ];
                    break;

                case AdFormLookupKeys.Centers:
                    lookups.Centers = LocationConstants.Centers
                        .Select((name, index) => new CenterDto
                        {
                            Id = index + 1,
                            Name = name,
                            GovernorateId = 1,
                            GovernorateName = LocationConstants.Governorate
                        })
                        .ToList();
                    break;

                case AdFormLookupKeys.WorkshopTypes:
                    lookups.WorkshopTypes = WorkshopCraftsmenCatalog.WorkshopTypeNames
                        .Where(entry => entry.Key is WorkshopType.Carpentry or WorkshopType.Blacksmithing
                            or WorkshopType.CarRepair or WorkshopType.Other)
                        .Select(entry => new WorkshopTypeDto { Id = (int)entry.Key, Name = entry.Value })
                        .ToList();
                    break;

                case AdFormLookupKeys.Specializations:
                    lookups.Specializations = WorkshopCraftsmenCatalog.Specializations
                        .Take(3)
                        .Select(entry => new SpecializationDto
                        {
                            Id = (int)entry.Value,
                            GroupName = entry.Group,
                            Name = entry.Name
                        })
                        .ToList();
                    break;

                case AdFormLookupKeys.ExperienceLevels:
                    lookups.ExperienceLevels = WorkshopCraftsmenCatalog.ExperienceLevelNames
                        .Select(entry => new ExperienceLevelDto { Id = (int)entry.Key, Name = entry.Value })
                        .ToList();
                    break;

                case AdFormLookupKeys.ProductionSpecialties:
                    lookups.ProductionSpecialties = BusinessCatalog.ProductionSpecialtyNames
                        .Where(entry => entry.Key is ProductionSpecialty.Food or ProductionSpecialty.Plastic
                            or ProductionSpecialty.Furniture or ProductionSpecialty.Other)
                        .Select(entry => new ProductionSpecialtyDto { Id = (int)entry.Key, Name = entry.Value })
                        .ToList();
                    break;

                case AdFormLookupKeys.FarmTypes:
                    lookups.FarmTypes = BusinessCatalog.FarmTypes
                        .Where(entry => entry.Value is FarmType.Vegetables or FarmType.Poultry
                            or FarmType.Fish or FarmType.Other)
                        .Select(entry => new FarmTypeDto
                        {
                            Id = (int)entry.Value,
                            Name = entry.Name,
                            Group = entry.Group,
                            GroupAr = entry.GroupAr
                        })
                        .ToList();
                    break;

                case AdFormLookupKeys.Seasons:
                    lookups.Seasons = BusinessCatalog.AvailabilitySeasonNames
                        .Select(entry => new AvailabilitySeasonDto { Id = (int)entry.Key, Name = entry.Value })
                        .ToList();
                    break;

                case AdFormLookupKeys.FarmingMethods:
                    lookups.FarmingMethods = BusinessCatalog.FarmingMethodNames
                        .Select(entry => new FarmingMethodDto
                        {
                            Id = (int)entry.Key,
                            Name = entry.Value,
                            Code = entry.Key.ToString()
                        })
                        .ToList();
                    break;

                case AdFormLookupKeys.CompanyFields:
                    lookups.CompanyFields = BusinessCatalog.CompanyFields
                        .Where(entry => entry.Value is CompanyField.GeneralContracting or CompanyField.Software
                            or CompanyField.MedicalServices or CompanyField.Other)
                        .Select(entry => new CompanyFieldDto
                        {
                            Id = (int)entry.Value,
                            Name = entry.Name,
                            Code = entry.Value.ToString(),
                            Group = entry.Group,
                            GroupAr = entry.GroupAr
                        })
                        .ToList();
                    break;

                case AdFormLookupKeys.SupplierTypes:
                    lookups.SupplierTypes = SupplierCatalog.Specializations
                        .Take(3)
                        .Concat(SupplierCatalog.Specializations.TakeLast(1))
                        .Select(entry => new SupplierSpecializationDto
                        {
                            Id = (int)entry.Value,
                            Name = entry.Name,
                            Group = entry.Group,
                            GroupAr = entry.GroupAr
                        })
                        .ToList();
                    break;

                case AdFormLookupKeys.TradeTypes:
                    lookups.TradeTypes = WholesaleTraderCatalog.TradeTypes
                        .Select(entry => new WholesaleTradeTypeDto
                        {
                            Id = (int)entry.Value,
                            Name = entry.Name
                        })
                        .ToList();
                    break;

                case AdFormLookupKeys.WholesaleSaleTypes:
                    lookups.WholesaleSaleTypes = WholesaleTraderCatalog.SaleTypeNames
                        .Select(entry => new WholesaleSaleTypeDto
                        {
                            Id = (int)entry.Key,
                            Name = entry.Value
                        })
                        .ToList();
                    break;

                case AdFormLookupKeys.MerchantSaleTypes:
                    lookups.MerchantSaleTypes = FruitVegetableMerchantCatalog.SaleTypeNames
                        .Select(entry => new MerchantSaleTypeDto
                        {
                            Id = (int)entry.Key,
                            Name = entry.Value
                        })
                        .ToList();
                    break;

                default:
                    _ = CarFormLookups.TryApply(lookups, lookupKey) ||
                        JobFormLookups.TryApply(lookups, lookupKey) ||
                        AnimalFormLookups.TryApply(lookups, lookupKey) ||
                        AntiqueFormLookups.TryApply(lookups, lookupKey) ||
                        ClothingFormLookups.TryApply(lookups, lookupKey) ||
                        OnlineShoppingFormLookups.TryApply(lookups, lookupKey) ||
                        HomeFurnishingFormLookups.TryApply(lookups, lookupKey) ||
                        RealEstateFormLookups.TryApply(lookups, lookupKey);
                    break;
            }
        }

        return lookups;
    }

    public static CategoryDto CategoryExample(CategoryType category) => category switch
    {
        CategoryType.Cars => new CategoryDto { Id = 1, Name = "Cars", NameAr = "سيارات" },
        CategoryType.WorkshopsAndCraftsmen =>
            new CategoryDto { Id = 2, Name = "Workshops & Craftsmen", NameAr = "الورش والحرفيين" },
        CategoryType.Business => new CategoryDto { Id = 4, Name = "Business", NameAr = "رجال أعمال" },
        CategoryType.Jobs => new CategoryDto { Id = 5, Name = "Jobs", NameAr = "الوظائف" },
        CategoryType.Animals => new CategoryDto { Id = 6, Name = "Animals", NameAr = "الحيوانات" },
        CategoryType.Antiques => new CategoryDto { Id = 7, Name = "Antiques", NameAr = "التحف والأنتيكات" },
        CategoryType.Clothing => new CategoryDto { Id = 8, Name = "Clothing", NameAr = "الملابس" },
        CategoryType.Charity => new CategoryDto { Id = 12, Name = "Charity", NameAr = "بوابة الخيرات" },
        _ => new CategoryDto { Id = 3, Name = "Lost & Found", NameAr = "المفقودات" }
    };

    public static SubCategoryDto SubCategoryExample(CategoryType category, SubCategoryType subCategory)
    {
        var (name, nameAr) = subCategory switch
        {
            SubCategoryType.Private => ("Private", "ملاكي"),
            SubCategoryType.Taxi => ("Taxi", "أجرة"),
            SubCategoryType.Motorcycles => ("Motorcycles", "موتوسيكلات"),
            SubCategoryType.HeavyEquipment => ("Heavy Equipment", "لوادر ومعدات ثقيلة"),
            SubCategoryType.Workshops => ("Workshops", "الورش"),
            SubCategoryType.Craftsmen => ("Craftsmen", "الحرفيين"),
            SubCategoryType.LostItems => ("Lost", "ضايع مني"),
            SubCategoryType.FoundItems => ("Found", "لقيت"),
            SubCategoryType.Factories => ("Factories", "المصانع"),
            SubCategoryType.Farms => ("Farms", "المزارع"),
            SubCategoryType.Companies => ("Companies", "الشركات"),
            SubCategoryType.Suppliers => ("Suppliers", "الموردون"),
            SubCategoryType.WholesaleTraders => ("Wholesale Traders", "تجار الجملة"),
            SubCategoryType.Rescues => ("Rescues", "الاستغاثة"),
            SubCategoryType.BloodRequests => ("Blood Requests", "فصائل الدم"),
            SubCategoryType.AskConsults => ("Ask & Consult", "اسأل واستشير"),
            SubCategoryType.JobRequests => ("Job Requests", "طلبات عمل"),
            SubCategoryType.JobOpportunities => ("Job Opportunities", "فرص عمل"),
            SubCategoryType.Livestock => ("Livestock", "المواشي"),
            SubCategoryType.SheepAndGoats => ("Sheep & Goats", "الأغنام والماعز"),
            SubCategoryType.Horses => ("Horses", "الخيول"),
            SubCategoryType.Camels => ("Camels", "الإبل"),
            SubCategoryType.Birds => ("Birds", "الطيور"),
            SubCategoryType.Pets => ("Pets", "الحيوانات الأليفة"),
            SubCategoryType.Fish => ("Fish", "الأسماك"),
            SubCategoryType.Bees => ("Bees", "النحل"),
            SubCategoryType.OtherAnimals => ("Other Animals", "حيوانات أخرى"),
            SubCategoryType.DecorAntiques => ("Antiques Decor", "تحف"),
            SubCategoryType.Antiques => ("Antiques", "أنتيكات"),
            SubCategoryType.Paintings => ("Paintings", "لوحات فنية"),
            SubCategoryType.Handmade => ("Handmade", "أعمال يدوية"),
            SubCategoryType.CoinsAndStamps => ("Coins & Stamps", "عملات وطوابع"),
            SubCategoryType.MenClothing => ("Men Clothing", "ملابس رجالي"),
            SubCategoryType.WomenClothing => ("Women Clothing", "ملابس حريمي"),
            SubCategoryType.KidsClothing => ("Kids Clothing", "ملابس أطفال"),
            _ => ("Fruit & Vegetable Traders", "تجار خضر وفاكهة")
        };

        var categoryDto = CategoryExample(category);

        return new SubCategoryDto
        {
            Id = (int)subCategory,
            Name = name,
            NameAr = nameAr,
            CategoryId = categoryDto.Id,
            CategoryName = categoryDto.Name
        };
    }
}
