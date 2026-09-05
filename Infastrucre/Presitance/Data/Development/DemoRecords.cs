using Shared.Enums;
using static Persistence.Data.Development.DemoImageCatalog;

namespace Persistence.Data.Development;

internal static class DemoRecords
{
    internal sealed record DemoUser(
        string UserName, string FirstName, string SecondName, string Phone, string Center, bool IsMale);

    public static readonly DemoUser[] Users =
    [
        new("demo.ahmed",   "أحمد",   "حسن",     "01055500001", "الفيوم",       true),
        new("demo.mahmoud", "محمود",  "عبد الله", "01055500002", "سنورس",        true),
        new("demo.mostafa", "مصطفى",  "السيد",   "01055500003", "طامية",        true),
        new("demo.khaled",  "خالد",   "إبراهيم",  "01055500004", "يوسف الصديق",  true),
        new("demo.yousef",  "يوسف",   "رمضان",   "01055500005", "اطسا",         true),
        new("demo.omar",    "عمر",    "فتحي",    "01055500006", "ابشواي",       true),
        new("demo.karim",   "كريم",   "صلاح",    "01055500007", "الفيوم",       true),
        new("demo.hossam",  "حسام",   "طارق",    "01055500008", "سنورس",        true),
        new("demo.mona",    "منى",    "عادل",    "01055500009", "طامية",        false),
        new("demo.sara",    "سارة",   "محمود",   "01055500010", "يوسف الصديق",  false),
        new("demo.fatma",   "فاطمة",  "عبد الرحمن", "01055500011", "اطسا",      false),
        new("demo.nourhan", "نورهان", "أشرف",    "01055500012", "ابشواي",       false)
    ];

    internal sealed record DemoAd(
        string Title,
        string Description,
        SubCategoryType SubCategory,
        ListingType Listing,
        string Brand,
        string Model,
        int Year,
        decimal Price,
        string Topic,
        bool WithInterior = true,
        string? Color = null,
        int? Kilometers = null,
        TransmissionType? Transmission = null,
        FuelType? Fuel = null,
        VehicleCondition? Condition = null,
        int? EngineCc = null,
        BodyType? Body = null,
        int? Doors = null,
        CoolingType? Cooling = null,
        EquipmentMachineType? MachineType = null,
        int? WorkingHours = null,
        decimal? PowerHorsepower = null,
        RentSystem? RentSystem = null,
        DamageLevel? DamageLevel = null,
        InterestedIn? InterestedIn = null);

    public static readonly DemoAd[] Advertisements =
    [

        new("تويوتا كورولا 2020 فابريكا بالكامل",
            "تويوتا كورولا موديل 2020، فابريكا بالكامل، صيانة دورية بالتوكيل، أول مالك، الفرش أصلي والمكينة ممتازة. السيارة بحالة الزيرو ولا تحتاج أي مصاريف.",
            SubCategoryType.Private, ListingType.Sale, "Toyota", "Corolla", 2020, 850_000m,
            Names.CarSedan, Color: "أبيض", Kilometers: 62_000, Transmission: TransmissionType.Automatic,
            Fuel: FuelType.Gasoline, Condition: VehicleCondition.Used, EngineCc: 1600,
            Body: BodyType.Sedan, Doors: 4),

        new("هيونداي إلنترا 2019 حالة ممتازة",
            "هيونداي إلنترا موديل 2019، ترخيص ساري لمدة سنة، كاوتش جديد، تكييف يثلج، بدون حوادث ومفتوحة للكشف عند أي فني.",
            SubCategoryType.Private, ListingType.Sale, "Hyundai", "Elantra", 2019, 720_000m,
            Names.CarSedan, Color: "رمادي", Kilometers: 88_000, Transmission: TransmissionType.Automatic,
            Fuel: FuelType.Gasoline, Condition: VehicleCondition.Used, EngineCc: 1600,
            Body: BodyType.Sedan, Doors: 4),

        new("كيا سبورتاج 2021 دفع رباعي",
            "كيا سبورتاج 2021 دفع رباعي، فتحة سقف، شاشة أصلية وكاميرا خلفية، مقاعد جلد. مناسبة للطرق الزراعية والسفر.",
            SubCategoryType.Private, ListingType.Sale, "Kia", "Sportage", 2021, 1_450_000m,
            Names.CarSuv, Color: "أسود", Kilometers: 41_000, Transmission: TransmissionType.Automatic,
            Fuel: FuelType.Gasoline, Condition: VehicleCondition.Used, EngineCc: 2000,
            Body: BodyType.SUV, Doors: 5),

        new("نيسان قشقاي 2022 للإيجار اليومي",
            "نيسان قشقاي 2022 متاحة للإيجار اليومي داخل الفيوم والمحافظات، السيارة مؤمنة بالكامل ويمكن الإيجار بسائق أو بدون.",
            SubCategoryType.Private, ListingType.Rent, "Nissan", "Qashqai", 2022, 1_800m,
            Names.CarSuv, Color: "فضي", Kilometers: 30_000, Transmission: TransmissionType.Automatic,
            Fuel: FuelType.Gasoline, Condition: VehicleCondition.Used, EngineCc: 1600,
            Body: BodyType.SUV, Doors: 5, RentSystem: Shared.Enums.RentSystem.Daily),

        new("شيفروليه أفيو هاتشباك 2018",
            "شيفروليه أفيو هاتشباك موديل 2018، اقتصادية جدًا في البنزين، مناسبة للاستخدام اليومي داخل المدينة.",
            SubCategoryType.Private, ListingType.Sale, "Chevrolet", "Aveo", 2018, 430_000m,
            Names.CarHatchback, Color: "أحمر", Kilometers: 105_000, Transmission: TransmissionType.Manual,
            Fuel: FuelType.Gasoline, Condition: VehicleCondition.Used, EngineCc: 1600,
            Body: BodyType.Hatchback, Doors: 5),

        new("سوزوكي سويفت 2017 للبدل",
            "سوزوكي سويفت 2017 بحالة جيدة، مطلوب البدل مع سيارة أكبر مع دفع الفرق حسب الحالة.",
            SubCategoryType.Private, ListingType.Exchange, "Suzuki", "Swift", 2017, 390_000m,
            Names.CarHatchback, Color: "أزرق", Kilometers: 120_000, Transmission: TransmissionType.Manual,
            Fuel: FuelType.Gasoline, Condition: VehicleCondition.Used, EngineCc: 1200,
            Body: BodyType.Hatchback, Doors: 5, InterestedIn: Shared.Enums.InterestedIn.AnyCar),

        new("تويوتا هايلكس 2019 نص نقل",
            "تويوتا هايلكس 2019 دبل كابينة، مجهزة للعمل الشاق، الشاسيه سليم والمكينة لم تفتح.",
            SubCategoryType.Private, ListingType.Sale, "Toyota", "Hilux", 2019, 1_250_000m,
            Names.CarPickup, Color: "أبيض", Kilometers: 140_000, Transmission: TransmissionType.Manual,
            Fuel: FuelType.Diesel, Condition: VehicleCondition.Used, EngineCc: 2500,
            Body: BodyType.Van, Doors: 4),

        new("إيسوزو نقل 2016 حوادث",
            "سيارة نقل إيسوزو 2016، تعرضت لحادث في المقدمة، تدور وتتحرك ويمكن معاينتها. تصلح للإصلاح أو لقطع الغيار.",
            SubCategoryType.Private, ListingType.Accident, "Isuzu", "D-Max", 2016, 260_000m,
            Names.CarPickup, Color: "أبيض", Kilometers: 210_000, Transmission: TransmissionType.Manual,
            Fuel: FuelType.Diesel, Condition: VehicleCondition.Used, EngineCc: 2500,
            DamageLevel: Shared.Enums.DamageLevel.Medium),

        new("شيفروليه أوبترا أجرة 2018 برخصة سارية",
            "شيفروليه أوبترا أجرة موديل 2018، الرخصة سارية والعداد يعمل، جاهزة للعمل من أول يوم.",
            SubCategoryType.Taxi, ListingType.Sale, "Chevrolet", "Optra", 2018, 480_000m,
            Names.CarTaxi, Kilometers: 190_000, Transmission: TransmissionType.Manual,
            Fuel: FuelType.Gasoline, Body: BodyType.Sedan, Doors: 4),

        new("لادا 2107 أجرة بحالة جيدة",
            "لادا 2107 أجرة، صيانة كاملة حديثة، كاوتش شبه جديد، مناسبة للعمل داخل الفيوم.",
            SubCategoryType.Taxi, ListingType.Sale, "Lada", "2107", 2014, 190_000m,
            Names.CarTaxi, Kilometers: 260_000, Transmission: TransmissionType.Manual,
            Fuel: FuelType.Gasoline, Body: BodyType.Sedan, Doors: 4),

        new("هيونداي فيرنا أجرة للإيجار الشهري",
            "هيونداي فيرنا أجرة متاحة للإيجار الشهري لسائق جاد، العقد رسمي والصيانة على المالك.",
            SubCategoryType.Taxi, ListingType.Rent, "Hyundai", "Verna", 2017, 9_000m,
            Names.CarTaxi, Kilometers: 220_000, Transmission: TransmissionType.Manual,
            Fuel: FuelType.Gasoline, Body: BodyType.Sedan, Doors: 4,
            RentSystem: Shared.Enums.RentSystem.Monthly),

        new("هوندا CB 150 موديل 2021",
            "موتوسيكل هوندا CB سعة 150 سي سي موديل 2021، الترخيص ساري، الكاوتش والسلسلة جديدة.",
            SubCategoryType.Motorcycles, ListingType.Sale, "Honda", "CB 150", 2021, 78_000m,
            Names.Motorcycle, WithInterior: false, Color: "أسود", Kilometers: 12_000,
            EngineCc: 150, Cooling: CoolingType.Air),

        new("باجاج بوكسر 150 حالة ممتازة",
            "باجاج بوكسر 150 سي سي، اقتصادي جدًا ومناسب للعمل والتوصيل، الصيانة بالتوكيل.",
            SubCategoryType.Motorcycles, ListingType.Sale, "Bajaj", "Boxer 150", 2020, 52_000m,
            Names.Motorcycle, WithInterior: false, Color: "أحمر", Kilometers: 26_000,
            EngineCc: 150, Cooling: CoolingType.Air),

        new("ياماها YBR 125 موديل 2022",
            "ياماها YBR 125 موديل 2022 بحالة الزيرو، استعمال خفيف جدًا، جميع الأوراق سليمة.",
            SubCategoryType.Motorcycles, ListingType.Sale, "Yamaha", "YBR 125", 2022, 95_000m,
            Names.Motorcycle, WithInterior: false, Color: "أزرق", Kilometers: 6_000,
            EngineCc: 125, Cooling: CoolingType.Air),

        new("لودر كاتربيلر 2015 جاهز للعمل",
            "لودر كاتربيلر موديل 2015، ساعات تشغيل قليلة بالنسبة للموديل، الهيدروليك سليم والإطارات جيدة.",
            SubCategoryType.HeavyEquipment, ListingType.Sale, "Caterpillar", "950H", 2015, 3_200_000m,
            Names.HeavyEquipmentLoader, WithInterior: false, Condition: VehicleCondition.Used,
            MachineType: EquipmentMachineType.Loader, WorkingHours: 9_800, PowerHorsepower: 200),

        new("حفار هيونداي 2017 للبيع",
            "حفار هيونداي موديل 2017، يعمل بكفاءة عالية، تم تغيير الجنزير حديثًا، متاح للمعاينة في الموقع.",
            SubCategoryType.HeavyEquipment, ListingType.Sale, "Hyundai", "R220", 2017, 4_100_000m,
            Names.HeavyEquipment, WithInterior: false, Condition: VehicleCondition.Used,
            MachineType: EquipmentMachineType.Excavator, WorkingHours: 7_400, PowerHorsepower: 150),

        new("لودر للإيجار اليومي داخل الفيوم",
            "لودر متاح للإيجار اليومي لأعمال الردم والتسوية داخل الفيوم والمراكز، الإيجار يشمل السائق.",
            SubCategoryType.HeavyEquipment, ListingType.Rent, "JCB", "3CX", 2018, 4_500m,
            Names.HeavyEquipmentLoader, WithInterior: false, Condition: VehicleCondition.Used,
            MachineType: EquipmentMachineType.Backhoe, WorkingHours: 5_200, PowerHorsepower: 110,
            RentSystem: Shared.Enums.RentSystem.Daily),

        new("بلدوزر كوماتسو موديل 2014",
            "بلدوزر كوماتسو 2014 بحالة جيدة، يصلح لأعمال التسوية والطرق، تم عمل عمرة للمكينة.",
            SubCategoryType.HeavyEquipment, ListingType.Sale, "Komatsu", "D65", 2014, 2_750_000m,
            Names.HeavyEquipment, WithInterior: false, Condition: VehicleCondition.Used,
            MachineType: EquipmentMachineType.Bulldozer, WorkingHours: 12_500, PowerHorsepower: 180)
    ];

    internal sealed record DemoWorkshop(
        string Name, WorkshopType Type, string AdTitle, string AdDescription, string Address, string Topic,
        string? OtherType = null);

    public static readonly DemoWorkshop[] Workshops =
    [
        new("ورشة النجار الحديثة", WorkshopType.Carpentry,
            "ورشة نجارة أثاث ومطابخ بالفيوم",
            "تنفيذ غرف نوم وأطقم أنتريهات ومطابخ خشب زان وأرو بأعلى جودة، تصميمات حديثة وتسليم في الميعاد.",
            "شارع الحرفيين، خلف موقف الفيوم", Names.WorkshopCarpentry),

        new("ورشة المعادن للحدادة واللحام", WorkshopType.Welding,
            "أعمال حدادة ولحام أرجون وكهرباء",
            "تصنيع أبواب وشبابيك ودرابزين وهناجر حديد، لحام أرجون وكهرباء بضمان على التنفيذ.",
            "الطريق الدائري، أمام محطة الوقود", Names.WorkshopWelding),

        new("ورشة الألوميتال المتحدة", WorkshopType.Aluminum,
            "تفصيل وتركيب ألوميتال وقطاعات",
            "تفصيل شبابيك وأبواب ألوميتال وقطاعات سيكوريت بجميع المقاسات مع التركيب والضمان.",
            "شارع الجمهورية، بجوار البنك", Names.WorkshopAluminium),

        new("ورشة الزجاج والمرايا", WorkshopType.Glass,
            "زجاج سيكوريت ومرايا وديكورات",
            "تركيب زجاج سيكوريت للمحلات والمنازل، مرايا حمامات وديكورات زجاجية بمقاسات حسب الطلب.",
            "شارع السوق التجاري", Names.WorkshopGlass),

        new("ورشة الرخام والجرانيت", WorkshopType.MarbleAndGranite,
            "تشطيب رخام وجرانيت للأرضيات والمطابخ",
            "توريد وتركيب رخام وجرانيت للأرضيات والسلالم وأسطح المطابخ، قص وتلميع بأحدث الماكينات.",
            "طريق الفيوم أسيوط الصحراوي", Names.WorkshopMarble),

        new("ورشة مطابخ البيت", WorkshopType.Kitchens,
            "مطابخ خشب وأكريليك بالمقاس",
            "تصميم وتنفيذ مطابخ بالمقاس بخامات مستوردة، معاينة وتصميم ثلاثي الأبعاد مجانًا.",
            "شارع المستشفى العام", Names.WorkshopKitchen),

        new("مركز خدمة السيارات السريع", WorkshopType.CarRepair,
            "صيانة سيارات ميكانيكا وكهرباء",
            "صيانة دورية، تغيير زيوت وفلاتر، كشف كمبيوتر وإصلاح أعطال الميكانيكا والكهرباء بضمان.",
            "الطريق الدائري، أمام معرض السيارات", Names.WorkshopCarRepair),

        new("ورشة كهرباء السيارات", WorkshopType.CarElectrical,
            "كهرباء سيارات وأنظمة إنذار",
            "إصلاح أعطال الكهرباء، تركيب أنظمة إنذار وسنتر لوك وشاشات وكاميرات خلفية.",
            "شارع الترعة، بجوار الكوبري", Names.WorkshopElectrical),

        new("ورشة السباكة والتأسيس", WorkshopType.Other,
            "تأسيس وصيانة سباكة للمنازل",
            "تأسيس شبكات المياه والصرف للمنازل والعمارات، كشف وإصلاح تسريبات بأحدث الأجهزة.",
            "شارع النصر، متفرع من شارع الحرية", Names.WorkshopPlumbing, OtherType: "ورشة سباكة")
    ];

    internal sealed record DemoCraftsman(
        string Name, CraftsmanSpecialization Specialization, ExperienceLevel Experience,
        string AdTitle, string AdDescription, string Address, string PortraitTopic, string WorkTopic);

    public static readonly DemoCraftsman[] Craftsmen =
    [
        new("أسطى سعيد النجار", CraftsmanSpecialization.FurnitureCarpenter, ExperienceLevel.MoreThanTenYears,
            "نجار أثاث ومطابخ خبرة أكثر من 10 سنوات",
            "تنفيذ جميع أعمال نجارة الأثاث والمطابخ والأبواب، دقة في المواعيد وأسعار مناسبة.",
            "الفيوم، شارع الحرفيين", Names.CraftsmanCarpenter, Names.WorkshopCarpentry),

        new("أسطى رمضان الكهربائي", CraftsmanSpecialization.HomeElectrician, ExperienceLevel.FiveToTenYears,
            "كهربائي منازل لجميع أعمال التأسيس والصيانة",
            "تأسيس كهرباء الشقق والفلل، إصلاح الأعطال، تركيب لوحات توزيع وإنارة LED.",
            "سنورس، شارع المحطة", Names.CraftsmanElectrician, Names.WorkshopElectrical),

        new("أسطى جمال السباك", CraftsmanSpecialization.Plumber, ExperienceLevel.ThreeToFiveYears,
            "سباك محترف لتأسيس وصيانة المواسير",
            "تأسيس سباكة كامل للشقق، كشف تسريبات بدون تكسير، تركيب أطقم حمامات ومطابخ.",
            "طامية، شارع السوق", Names.CraftsmanPlumber, Names.WorkshopPlumbing),

        new("أسطى عبد الناصر النقاش", CraftsmanSpecialization.Painter, ExperienceLevel.MoreThanTenYears,
            "نقاش دهانات حديثة وديكورات",
            "جميع أنواع الدهانات البلاستيك والزيت والدهانات الديكورية، معجون وتجهيز الحوائط.",
            "يوسف الصديق، شارع المدرسة", Names.CraftsmanPainter, Names.WorkshopCarpentry),

        new("أسطى إبراهيم اللحام", CraftsmanSpecialization.ElectricWelding, ExperienceLevel.FiveToTenYears,
            "لحام كهرباء وأرجون لجميع الأعمال",
            "لحام حديد وستانلس، تصنيع وتركيب أبواب وشبابيك ومظلات، خدمة داخل وخارج الورشة.",
            "اطسا، الطريق الرئيسي", Names.CraftsmanWelder, Names.WorkshopWelding),

        new("أسطى وليد فني التكييف", CraftsmanSpecialization.AirConditioningTechnician, ExperienceLevel.OneToThreeYears,
            "فني تكييف وتبريد تركيب وصيانة",
            "تركيب وصيانة جميع أنواع التكييفات، شحن فريون، تنظيف وغسيل بالبخار.",
            "ابشواي، شارع المستشفى", Names.CraftsmanTechnician, Names.WorkshopElectrical)
    ];

    internal sealed record DemoPost(
        PostType Type, string ItemName, string Description, string Topic, int DaysAgo);

    public static readonly DemoPost[] Posts =
    [
        new(PostType.Lost, "موبايل سامسونج ضاع في السوق",
            "ضاع مني موبايل سامسونج لونه أسود في سوق الفيوم يوم الجمعة، عليه جراب أزرق. مكافأة لمن يجده.",
            Names.ItemPhone, 4),

        new(PostType.Lost, "محفظة جلد بها بطاقة شخصية",
            "فقدت محفظة جلد بني بها البطاقة الشخصية ورخصة القيادة، المبلغ لمن يجدها والأوراق أهم من أي شيء.",
            Names.ItemWallet, 7),

        new(PostType.Lost, "مفاتيح شقة وسيارة في ميدان المحطة",
            "ضاعت مجموعة مفاتيح بها مفتاح سيارة وميدالية جلد في ميدان المحطة، من فضلك اتواصل فورًا.",
            Names.ItemKeys, 2),

        new(PostType.Lost, "بطاقة شخصية باسم صاحبها",
            "فقدت بطاقتي الشخصية أثناء الانتقال بين سنورس والفيوم، من يجدها يتواصل على الرقم المرفق.",
            Names.ItemIdCard, 10),

        new(PostType.Found, "شنطة ظهر تم العثور عليها",
            "لقيت شنطة ظهر لونها أسود بها كتب وكشكول أمام مدرسة ثانوية، للتواصل مع صاحبها لتسليمها.",
            Names.ItemBag, 3),

        new(PostType.Found, "ساعة يد تم العثور عليها في الحديقة",
            "لقيت ساعة يد رجالي في الحديقة العامة، سيتم تسليمها لصاحبها بعد وصف الساعة بدقة.",
            Names.ItemWatch, 5),

        new(PostType.Found, "لاب توب تم العثور عليه في المواصلات",
            "لقيت لاب توب داخل حقيبة في ميكروباص الفيوم سنورس، أمانة عند صاحبها بعد إثبات الملكية.",
            Names.ItemLaptop, 1),

        new(PostType.Found, "سماعات لاسلكية أمام الجامعة",
            "لقيت سماعات لاسلكية داخل علبتها أمام بوابة الجامعة، للتواصل لتسليمها لصاحبها.",
            Names.ItemHeadphones, 6)
    ];
}
