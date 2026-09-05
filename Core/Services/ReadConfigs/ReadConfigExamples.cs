using Services.AdForms;
using Shared.DTOs.Lookups.Read;
using Shared.Enums;

namespace Services.ReadConfigs;

public static class ReadConfigExamples
{
    public sealed record Example(string Key, string Summary, ReadConfigDto Config);

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
            CategoryType.LostAndFound, SubCategoryType.FoundItems),
        Create("business-factories", "Business → Factories (رجال أعمال → المصانع)",
            CategoryType.Business, SubCategoryType.Factories),
        Create("business-farms", "Business → Farms (رجال أعمال → المزارع)",
            CategoryType.Business, SubCategoryType.Farms),
        Create("business-companies", "Business → Companies (رجال أعمال → الشركات)",
            CategoryType.Business, SubCategoryType.Companies),
        Create("business-suppliers", "Business → Suppliers (رجال أعمال → الموردون)",
            CategoryType.Business, SubCategoryType.Suppliers),
        Create("business-wholesale", "Business → Wholesale Traders (رجال أعمال → تجار الجملة)",
            CategoryType.Business, SubCategoryType.WholesaleTraders),
        Create("business-fruit-traders", "Business → Fruit & Vegetable Traders (رجال أعمال → تجار خضر وفاكهة)",
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
            CategoryType.Clothing, SubCategoryType.KidsClothing),

        Create("charity-rescues", "Charity → Rescues (بوابة الخيرات → الاستغاثة)",
            CategoryType.Charity, SubCategoryType.Rescues),
        Create("charity-blood-requests", "Charity → Blood Requests (بوابة الخيرات → فصائل الدم)",
            CategoryType.Charity, SubCategoryType.BloodRequests),
        Create("charity-ask-consults", "Charity → Ask & Consult (بوابة الخيرات → اسأل واستشير)",
            CategoryType.Charity, SubCategoryType.AskConsults),

        SubCategoryStep("sub-category-step", "Category only → sub category still to be chosen (رجال أعمال)",
            CategoryType.Business)
    ];

    private static Example Create(
        string key, string summary, CategoryType category, SubCategoryType subCategory)
    {
        var schema = ReadConfigCatalog.GetSchema((int)category, (int)subCategory)!;

        var config = new ReadConfigDto
        {
            Category = CreateAdFormExamples.CategoryExample(category),
            SubCategory = CreateAdFormExamples.SubCategoryExample(category, subCategory),
            RequiresSubCategory = false,
            Module = schema.Module,
            List = schema.Find(ReadOperationKeys.List),
            Details = schema.Find(ReadOperationKeys.Details),
            Operations = schema.ToMap()
        };

        return new Example(key, summary, config);
    }

    private static Example SubCategoryStep(string key, string summary, CategoryType category)
    {
        var schema = ReadConfigCatalog.SubCategorySelectionSchema((int)category);

        var config = new ReadConfigDto
        {
            Category = CreateAdFormExamples.CategoryExample(category),
            RequiresSubCategory = true,
            Module = schema.Module,
            Operations = schema.ToMap()
        };

        return new Example(key, summary, config);
    }
}
