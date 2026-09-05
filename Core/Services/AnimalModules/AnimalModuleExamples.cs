using Shared.Constants;
using Shared.DTOs.Animals;
using Shared.Enums;

namespace Services.AnimalModules;

public static class AnimalModuleExamples
{
    private static readonly Guid OwnerId = new("55555555-5555-5555-5555-555555555555");
    private static readonly Guid ImageId = new("44444444-4444-4444-4444-444444444444");

    private static readonly DateTime CreatedAt = new(2026, 7, 20, 9, 30, 0, DateTimeKind.Utc);

    private static readonly Guid LivestockId = new("a1000000-0000-0000-0000-000000000001");

    public static LivestockDetailsDto LivestockDetails() => new()
    {
        Id = LivestockId,
        OwnerId = OwnerId.ToString(),
        SellerName = "محمود عبد الرحمن",
        Breed = LivestockBreed.BaladiCow,
        BreedName = LivestockCatalog.GetBreedName(LivestockBreed.BaladiCow),
        OtherBreed = null,
        Purpose = LivestockPurpose.Breeding,
        PurposeName = LivestockCatalog.GetPurposeName(LivestockPurpose.Breeding),
        Age = LivestockAge.Newborn,
        AgeName = LivestockCatalog.GetAgeName(LivestockAge.Newborn),
        Gender = LivestockGender.Male,
        GenderName = LivestockCatalog.GetGenderName(LivestockGender.Male),
        HealthStatus = LivestockHealthStatus.Excellent,
        HealthStatusName = LivestockCatalog.GetHealthStatusName(LivestockHealthStatus.Excellent),
        Vaccination = LivestockVaccination.FullyVaccinated,
        VaccinationName = LivestockCatalog.GetVaccinationName(LivestockVaccination.FullyVaccinated),
        Production = LivestockProduction.Milk,
        ProductionName = LivestockCatalog.GetProductionName(LivestockProduction.Milk),
        Quantity = 5,
        Price = 45000.00m,
        Negotiable = true,
        Address = "الفيوم، طريق سنورس الزراعي",
        GoogleMaps = "https://maps.app.goo.gl/example",
        Phone = "01012345678",
        WhatsApp = "01087654321",
        Title = "عجول تسمين بلدي للبيع",
        Description = "عجول بلدي سليمة ومحصنة بالكامل، متابعة بيطرية منتظمة والتسليم داخل الفيوم.",
        Images =
        [
            new LivestockImageDto
            {
                Id = ImageId,
                Url = "https://api.example.com/uploads/livestock/sample.jpg",
                IsPrimary = true
            }
        ],
        CreatedAt = CreatedAt,
        UpdatedAt = null
    };

    public static LivestockListItemDto LivestockListItem()
    {
        var details = LivestockDetails();

        return new LivestockListItemDto
        {
            Id = details.Id,
            SellerName = details.SellerName,
            Breed = details.Breed,
            BreedName = details.BreedName,
            OtherBreed = details.OtherBreed,
            Purpose = details.Purpose,
            PurposeName = details.PurposeName,
            Age = details.Age,
            AgeName = details.AgeName,
            Gender = details.Gender,
            GenderName = details.GenderName,
            HealthStatus = details.HealthStatus,
            HealthStatusName = details.HealthStatusName,
            Vaccination = details.Vaccination,
            VaccinationName = details.VaccinationName,
            Production = details.Production,
            ProductionName = details.ProductionName,
            Quantity = details.Quantity,
            Price = details.Price,
            Negotiable = details.Negotiable,
            Phone = details.Phone,
            WhatsApp = details.WhatsApp,
            Title = details.Title,
            Description = details.Description,
            PrimaryImageUrl = details.Images[0].Url,
            Images = details.Images,
            CreatedAt = details.CreatedAt
        };
    }

    private static readonly Guid SheepGoatId = new("a2000000-0000-0000-0000-000000000002");

    public static SheepGoatDetailsDto SheepGoatDetails() => new()
    {
        Id = SheepGoatId,
        OwnerId = OwnerId.ToString(),
        SellerName = "سيد الشرقاوي",
        Breed = SheepGoatBreed.Barki,
        BreedName = SheepGoatCatalog.GetBreedName(SheepGoatBreed.Barki),
        OtherBreed = null,
        Purpose = SheepGoatPurpose.Breeding,
        PurposeName = SheepGoatCatalog.GetPurposeName(SheepGoatPurpose.Breeding),
        Age = SheepGoatAge.Newborn,
        AgeName = SheepGoatCatalog.GetAgeName(SheepGoatAge.Newborn),
        Gender = SheepGoatGender.Male,
        GenderName = SheepGoatCatalog.GetGenderName(SheepGoatGender.Male),
        HealthStatus = SheepGoatHealthStatus.Excellent,
        HealthStatusName = SheepGoatCatalog.GetHealthStatusName(SheepGoatHealthStatus.Excellent),
        Vaccination = SheepGoatVaccination.FullyVaccinated,
        VaccinationName = SheepGoatCatalog.GetVaccinationName(SheepGoatVaccination.FullyVaccinated),
        Quantity = 20,
        Price = 9500.00m,
        Negotiable = true,
        Address = "الفيوم، طريق سنورس الزراعي",
        GoogleMaps = "https://maps.app.goo.gl/example",
        Phone = "01012345678",
        WhatsApp = "01087654321",
        Title = "خرفان برقي للبيع",
        Description = "خرفان برقي فرز أول، أوزان من 40 إلى 55 كجم، متاحة للأضاحي والتسمين.",
        Images =
        [
            new SheepGoatImageDto
            {
                Id = ImageId,
                Url = "https://api.example.com/uploads/sheep-goats/sample.jpg",
                IsPrimary = true
            }
        ],
        CreatedAt = CreatedAt,
        UpdatedAt = null
    };

    public static SheepGoatListItemDto SheepGoatListItem()
    {
        var details = SheepGoatDetails();

        return new SheepGoatListItemDto
        {
            Id = details.Id,
            SellerName = details.SellerName,
            Breed = details.Breed,
            BreedName = details.BreedName,
            OtherBreed = details.OtherBreed,
            Purpose = details.Purpose,
            PurposeName = details.PurposeName,
            Age = details.Age,
            AgeName = details.AgeName,
            Gender = details.Gender,
            GenderName = details.GenderName,
            HealthStatus = details.HealthStatus,
            HealthStatusName = details.HealthStatusName,
            Vaccination = details.Vaccination,
            VaccinationName = details.VaccinationName,
            Quantity = details.Quantity,
            Price = details.Price,
            Negotiable = details.Negotiable,
            Phone = details.Phone,
            WhatsApp = details.WhatsApp,
            Title = details.Title,
            Description = details.Description,
            PrimaryImageUrl = details.Images[0].Url,
            Images = details.Images,
            CreatedAt = details.CreatedAt
        };
    }

    private static readonly Guid HorseId = new("a3000000-0000-0000-0000-000000000003");

    public static HorseDetailsDto HorseDetails() => new()
    {
        Id = HorseId,
        OwnerId = OwnerId.ToString(),
        SellerName = "أحمد الفيومي",
        Breed = HorseBreed.ArabianHorse,
        BreedName = HorseCatalog.GetBreedName(HorseBreed.ArabianHorse),
        OtherBreed = null,
        Purpose = HorsePurpose.Riding,
        PurposeName = HorseCatalog.GetPurposeName(HorsePurpose.Riding),
        Age = HorseAge.Newborn,
        AgeName = HorseCatalog.GetAgeName(HorseAge.Newborn),
        Gender = HorseGender.Male,
        GenderName = HorseCatalog.GetGenderName(HorseGender.Male),
        HealthStatus = HorseHealthStatus.Excellent,
        HealthStatusName = HorseCatalog.GetHealthStatusName(HorseHealthStatus.Excellent),
        TrainingLevel = HorseTrainingLevel.Untrained,
        TrainingLevelName = HorseCatalog.GetTrainingLevelName(HorseTrainingLevel.Untrained),
        Vaccination = HorseVaccination.FullyVaccinated,
        VaccinationName = HorseCatalog.GetVaccinationName(HorseVaccination.FullyVaccinated),
        Quantity = 1,
        Price = 180000.00m,
        Negotiable = true,
        Address = "الفيوم، طريق سنورس الزراعي",
        GoogleMaps = "https://maps.app.goo.gl/example",
        Phone = "01012345678",
        WhatsApp = "01087654321",
        Title = "حصان عربي أصيل للبيع",
        Description = "حصان عربي أصيل بشهادة نسب، مدرب على الركوب والعروض وحالته الصحية ممتازة.",
        Images =
        [
            new HorseImageDto
            {
                Id = ImageId,
                Url = "https://api.example.com/uploads/horses/sample.jpg",
                IsPrimary = true
            }
        ],
        CreatedAt = CreatedAt,
        UpdatedAt = null
    };

    public static HorseListItemDto HorseListItem()
    {
        var details = HorseDetails();

        return new HorseListItemDto
        {
            Id = details.Id,
            SellerName = details.SellerName,
            Breed = details.Breed,
            BreedName = details.BreedName,
            OtherBreed = details.OtherBreed,
            Purpose = details.Purpose,
            PurposeName = details.PurposeName,
            Age = details.Age,
            AgeName = details.AgeName,
            Gender = details.Gender,
            GenderName = details.GenderName,
            HealthStatus = details.HealthStatus,
            HealthStatusName = details.HealthStatusName,
            TrainingLevel = details.TrainingLevel,
            TrainingLevelName = details.TrainingLevelName,
            Vaccination = details.Vaccination,
            VaccinationName = details.VaccinationName,
            Quantity = details.Quantity,
            Price = details.Price,
            Negotiable = details.Negotiable,
            Phone = details.Phone,
            WhatsApp = details.WhatsApp,
            Title = details.Title,
            Description = details.Description,
            PrimaryImageUrl = details.Images[0].Url,
            Images = details.Images,
            CreatedAt = details.CreatedAt
        };
    }

    private static readonly Guid CamelId = new("a4000000-0000-0000-0000-000000000004");

    public static CamelDetailsDto CamelDetails() => new()
    {
        Id = CamelId,
        OwnerId = OwnerId.ToString(),
        SellerName = "سالم المغربي",
        Breed = CamelBreed.Maghrabi,
        BreedName = CamelCatalog.GetBreedName(CamelBreed.Maghrabi),
        OtherBreed = null,
        Purpose = CamelPurpose.Breeding,
        PurposeName = CamelCatalog.GetPurposeName(CamelPurpose.Breeding),
        Age = CamelAge.Newborn,
        AgeName = CamelCatalog.GetAgeName(CamelAge.Newborn),
        Gender = CamelGender.Male,
        GenderName = CamelCatalog.GetGenderName(CamelGender.Male),
        HealthStatus = CamelHealthStatus.Excellent,
        HealthStatusName = CamelCatalog.GetHealthStatusName(CamelHealthStatus.Excellent),
        Vaccination = CamelVaccination.FullyVaccinated,
        VaccinationName = CamelCatalog.GetVaccinationName(CamelVaccination.FullyVaccinated),
        Quantity = 3,
        Price = 75000.00m,
        Negotiable = true,
        Address = "الفيوم، طريق سنورس الزراعي",
        GoogleMaps = "https://maps.app.goo.gl/example",
        Phone = "01012345678",
        WhatsApp = "01087654321",
        Title = "نوق مغربي للبيع",
        Description = "نوق مغربي بحالة ممتازة، صالحة للتربية وإنتاج الألبان، التسليم داخل الفيوم.",
        Images =
        [
            new CamelImageDto
            {
                Id = ImageId,
                Url = "https://api.example.com/uploads/camels/sample.jpg",
                IsPrimary = true
            }
        ],
        CreatedAt = CreatedAt,
        UpdatedAt = null
    };

    public static CamelListItemDto CamelListItem()
    {
        var details = CamelDetails();

        return new CamelListItemDto
        {
            Id = details.Id,
            SellerName = details.SellerName,
            Breed = details.Breed,
            BreedName = details.BreedName,
            OtherBreed = details.OtherBreed,
            Purpose = details.Purpose,
            PurposeName = details.PurposeName,
            Age = details.Age,
            AgeName = details.AgeName,
            Gender = details.Gender,
            GenderName = details.GenderName,
            HealthStatus = details.HealthStatus,
            HealthStatusName = details.HealthStatusName,
            Vaccination = details.Vaccination,
            VaccinationName = details.VaccinationName,
            Quantity = details.Quantity,
            Price = details.Price,
            Negotiable = details.Negotiable,
            Phone = details.Phone,
            WhatsApp = details.WhatsApp,
            Title = details.Title,
            Description = details.Description,
            PrimaryImageUrl = details.Images[0].Url,
            Images = details.Images,
            CreatedAt = details.CreatedAt
        };
    }

    private static readonly Guid BirdId = new("a5000000-0000-0000-0000-000000000005");

    public static BirdDetailsDto BirdDetails() => new()
    {
        Id = BirdId,
        OwnerId = OwnerId.ToString(),
        SellerName = "مصطفى عبد الله",
        AnimalType = BirdType.Chickens,
        AnimalTypeName = BirdCatalog.GetTypeName(BirdType.Chickens),
        OtherType = null,
        Purpose = BirdPurpose.Breeding,
        PurposeName = BirdCatalog.GetPurposeName(BirdPurpose.Breeding),
        Age = BirdAge.Newborn,
        AgeName = BirdCatalog.GetAgeName(BirdAge.Newborn),
        Gender = BirdGender.Male,
        GenderName = BirdCatalog.GetGenderName(BirdGender.Male),
        HealthStatus = BirdHealthStatus.Excellent,
        HealthStatusName = BirdCatalog.GetHealthStatusName(BirdHealthStatus.Excellent),
        Vaccination = BirdVaccination.FullyVaccinated,
        VaccinationName = BirdCatalog.GetVaccinationName(BirdVaccination.FullyVaccinated),
        Quantity = 40,
        Price = 350.00m,
        Negotiable = true,
        Address = "الفيوم، طريق سنورس الزراعي",
        GoogleMaps = "https://maps.app.goo.gl/example",
        Phone = "01012345678",
        WhatsApp = "01087654321",
        Title = "حمام زاجل للبيع",
        Description = "حمام زاجل بلدي بحالة ممتازة، متاح للتربية والمسابقات مع إمكانية التوصيل.",
        Images =
        [
            new BirdImageDto
            {
                Id = ImageId,
                Url = "https://api.example.com/uploads/birds/sample.jpg",
                IsPrimary = true
            }
        ],
        CreatedAt = CreatedAt,
        UpdatedAt = null
    };

    public static BirdListItemDto BirdListItem()
    {
        var details = BirdDetails();

        return new BirdListItemDto
        {
            Id = details.Id,
            SellerName = details.SellerName,
            AnimalType = details.AnimalType,
            AnimalTypeName = details.AnimalTypeName,
            OtherType = details.OtherType,
            Purpose = details.Purpose,
            PurposeName = details.PurposeName,
            Age = details.Age,
            AgeName = details.AgeName,
            Gender = details.Gender,
            GenderName = details.GenderName,
            HealthStatus = details.HealthStatus,
            HealthStatusName = details.HealthStatusName,
            Vaccination = details.Vaccination,
            VaccinationName = details.VaccinationName,
            Quantity = details.Quantity,
            Price = details.Price,
            Negotiable = details.Negotiable,
            Phone = details.Phone,
            WhatsApp = details.WhatsApp,
            Title = details.Title,
            Description = details.Description,
            PrimaryImageUrl = details.Images[0].Url,
            Images = details.Images,
            CreatedAt = details.CreatedAt
        };
    }

    private static readonly Guid PetId = new("a6000000-0000-0000-0000-000000000006");

    public static PetDetailsDto PetDetails() => new()
    {
        Id = PetId,
        OwnerId = OwnerId.ToString(),
        SellerName = "نورهان سمير",
        Breed = PetBreed.Dogs,
        BreedName = PetCatalog.GetBreedName(PetBreed.Dogs),
        OtherBreed = null,
        Purpose = PetPurpose.Companionship,
        PurposeName = PetCatalog.GetPurposeName(PetPurpose.Companionship),
        Age = PetAge.Newborn,
        AgeName = PetCatalog.GetAgeName(PetAge.Newborn),
        Gender = PetGender.Male,
        GenderName = PetCatalog.GetGenderName(PetGender.Male),
        HealthStatus = PetHealthStatus.Excellent,
        HealthStatusName = PetCatalog.GetHealthStatusName(PetHealthStatus.Excellent),
        TrainingLevel = PetTrainingLevel.Untrained,
        TrainingLevelName = PetCatalog.GetTrainingLevelName(PetTrainingLevel.Untrained),
        Vaccination = PetVaccination.FullyVaccinated,
        VaccinationName = PetCatalog.GetVaccinationName(PetVaccination.FullyVaccinated),
        Quantity = 2,
        Price = 2500.00m,
        Negotiable = true,
        Address = "الفيوم، طريق سنورس الزراعي",
        GoogleMaps = "https://maps.app.goo.gl/example",
        Phone = "01012345678",
        WhatsApp = "01087654321",
        Title = "كلاب جيرمن شيبرد صغيرة للبيع",
        Description = "جراوٍ جيرمن شيبرد محصنة بالكامل ومدربة على الأساسيات، مع شهادة بيطرية.",
        Images =
        [
            new PetImageDto
            {
                Id = ImageId,
                Url = "https://api.example.com/uploads/pets/sample.jpg",
                IsPrimary = true
            }
        ],
        CreatedAt = CreatedAt,
        UpdatedAt = null
    };

    public static PetListItemDto PetListItem()
    {
        var details = PetDetails();

        return new PetListItemDto
        {
            Id = details.Id,
            SellerName = details.SellerName,
            Breed = details.Breed,
            BreedName = details.BreedName,
            OtherBreed = details.OtherBreed,
            Purpose = details.Purpose,
            PurposeName = details.PurposeName,
            Age = details.Age,
            AgeName = details.AgeName,
            Gender = details.Gender,
            GenderName = details.GenderName,
            HealthStatus = details.HealthStatus,
            HealthStatusName = details.HealthStatusName,
            TrainingLevel = details.TrainingLevel,
            TrainingLevelName = details.TrainingLevelName,
            Vaccination = details.Vaccination,
            VaccinationName = details.VaccinationName,
            Quantity = details.Quantity,
            Price = details.Price,
            Negotiable = details.Negotiable,
            Phone = details.Phone,
            WhatsApp = details.WhatsApp,
            Title = details.Title,
            Description = details.Description,
            PrimaryImageUrl = details.Images[0].Url,
            Images = details.Images,
            CreatedAt = details.CreatedAt
        };
    }

    private static readonly Guid FishId = new("a7000000-0000-0000-0000-000000000007");

    public static FishDetailsDto FishDetails() => new()
    {
        Id = FishId,
        OwnerId = OwnerId.ToString(),
        SellerName = "عماد شعبان",
        AnimalType = FishType.Tilapia,
        AnimalTypeName = FishCatalog.GetTypeName(FishType.Tilapia),
        OtherType = null,
        Purpose = FishPurpose.Ornamental,
        PurposeName = FishCatalog.GetPurposeName(FishPurpose.Ornamental),
        Age = FishAge.Fry,
        AgeName = FishCatalog.GetAgeName(FishAge.Fry),
        HealthStatus = FishHealthStatus.Excellent,
        HealthStatusName = FishCatalog.GetHealthStatusName(FishHealthStatus.Excellent),
        Quantity = 500,
        Price = 1200.00m,
        Negotiable = true,
        Address = "الفيوم، طريق سنورس الزراعي",
        GoogleMaps = "https://maps.app.goo.gl/example",
        Phone = "01012345678",
        WhatsApp = "01087654321",
        Title = "زريعة بلطي للبيع",
        Description = "زريعة بلطي من مفرخ معتمد، أحجام متدرجة وتسليم مبرد داخل محافظة الفيوم.",
        Images =
        [
            new FishImageDto
            {
                Id = ImageId,
                Url = "https://api.example.com/uploads/fish/sample.jpg",
                IsPrimary = true
            }
        ],
        CreatedAt = CreatedAt,
        UpdatedAt = null
    };

    public static FishListItemDto FishListItem()
    {
        var details = FishDetails();

        return new FishListItemDto
        {
            Id = details.Id,
            SellerName = details.SellerName,
            AnimalType = details.AnimalType,
            AnimalTypeName = details.AnimalTypeName,
            OtherType = details.OtherType,
            Purpose = details.Purpose,
            PurposeName = details.PurposeName,
            Age = details.Age,
            AgeName = details.AgeName,
            HealthStatus = details.HealthStatus,
            HealthStatusName = details.HealthStatusName,
            Quantity = details.Quantity,
            Price = details.Price,
            Negotiable = details.Negotiable,
            Phone = details.Phone,
            WhatsApp = details.WhatsApp,
            Title = details.Title,
            Description = details.Description,
            PrimaryImageUrl = details.Images[0].Url,
            Images = details.Images,
            CreatedAt = details.CreatedAt
        };
    }

    private static readonly Guid BeeId = new("a8000000-0000-0000-0000-000000000008");

    public static BeeDetailsDto BeeDetails() => new()
    {
        Id = BeeId,
        OwnerId = OwnerId.ToString(),
        SellerName = "خالد المنشاوي",
        AnimalType = BeeType.Carniolan,
        AnimalTypeName = BeeCatalog.GetTypeName(BeeType.Carniolan),
        OtherType = null,
        Purpose = BeePurpose.HoneyProduction,
        PurposeName = BeeCatalog.GetPurposeName(BeePurpose.HoneyProduction),
        HealthStatus = BeeHealthStatus.Excellent,
        HealthStatusName = BeeCatalog.GetHealthStatusName(BeeHealthStatus.Excellent),
        Production = BeeProduction.Honey,
        ProductionName = BeeCatalog.GetProductionName(BeeProduction.Honey),
        Quantity = 15,
        Price = 3500.00m,
        Negotiable = true,
        Address = "الفيوم، طريق سنورس الزراعي",
        GoogleMaps = "https://maps.app.goo.gl/example",
        Phone = "01012345678",
        WhatsApp = "01087654321",
        Title = "خلايا نحل كرنيولي للبيع",
        Description = "خلايا نحل كرنيولي نشطة بملكات حديثة، جاهزة لإنتاج العسل والتلقيح.",
        Images =
        [
            new BeeImageDto
            {
                Id = ImageId,
                Url = "https://api.example.com/uploads/bees/sample.jpg",
                IsPrimary = true
            }
        ],
        CreatedAt = CreatedAt,
        UpdatedAt = null
    };

    public static BeeListItemDto BeeListItem()
    {
        var details = BeeDetails();

        return new BeeListItemDto
        {
            Id = details.Id,
            SellerName = details.SellerName,
            AnimalType = details.AnimalType,
            AnimalTypeName = details.AnimalTypeName,
            OtherType = details.OtherType,
            Purpose = details.Purpose,
            PurposeName = details.PurposeName,
            HealthStatus = details.HealthStatus,
            HealthStatusName = details.HealthStatusName,
            Production = details.Production,
            ProductionName = details.ProductionName,
            Quantity = details.Quantity,
            Price = details.Price,
            Negotiable = details.Negotiable,
            Phone = details.Phone,
            WhatsApp = details.WhatsApp,
            Title = details.Title,
            Description = details.Description,
            PrimaryImageUrl = details.Images[0].Url,
            Images = details.Images,
            CreatedAt = details.CreatedAt
        };
    }

    private static readonly Guid OtherAnimalId = new("a9000000-0000-0000-0000-000000000009");

    public static OtherAnimalDetailsDto OtherAnimalDetails() => new()
    {
        Id = OtherAnimalId,
        OwnerId = OwnerId.ToString(),
        SellerName = "رمضان فتحي",
        AnimalType = OtherAnimalType.Donkeys,
        AnimalTypeName = OtherAnimalCatalog.GetTypeName(OtherAnimalType.Donkeys),
        OtherType = null,
        Purpose = OtherAnimalPurpose.Breeding,
        PurposeName = OtherAnimalCatalog.GetPurposeName(OtherAnimalPurpose.Breeding),
        Age = OtherAnimalAge.Newborn,
        AgeName = OtherAnimalCatalog.GetAgeName(OtherAnimalAge.Newborn),
        Gender = OtherAnimalGender.Male,
        GenderName = OtherAnimalCatalog.GetGenderName(OtherAnimalGender.Male),
        HealthStatus = OtherAnimalHealthStatus.Excellent,
        HealthStatusName = OtherAnimalCatalog.GetHealthStatusName(OtherAnimalHealthStatus.Excellent),
        Vaccination = OtherAnimalVaccination.FullyVaccinated,
        VaccinationName = OtherAnimalCatalog.GetVaccinationName(OtherAnimalVaccination.FullyVaccinated),
        Quantity = 2,
        Price = 8000.00m,
        Negotiable = true,
        Address = "الفيوم، طريق سنورس الزراعي",
        GoogleMaps = "https://maps.app.goo.gl/example",
        Phone = "01012345678",
        WhatsApp = "01087654321",
        Title = "حمار بلدي للبيع",
        Description = "حمار بلدي قوي وصالح للعمل، بحالة صحية ممتازة والتسليم داخل الفيوم.",
        Images =
        [
            new OtherAnimalImageDto
            {
                Id = ImageId,
                Url = "https://api.example.com/uploads/other-animals/sample.jpg",
                IsPrimary = true
            }
        ],
        CreatedAt = CreatedAt,
        UpdatedAt = null
    };

    public static OtherAnimalListItemDto OtherAnimalListItem()
    {
        var details = OtherAnimalDetails();

        return new OtherAnimalListItemDto
        {
            Id = details.Id,
            SellerName = details.SellerName,
            AnimalType = details.AnimalType,
            AnimalTypeName = details.AnimalTypeName,
            OtherType = details.OtherType,
            Purpose = details.Purpose,
            PurposeName = details.PurposeName,
            Age = details.Age,
            AgeName = details.AgeName,
            Gender = details.Gender,
            GenderName = details.GenderName,
            HealthStatus = details.HealthStatus,
            HealthStatusName = details.HealthStatusName,
            Vaccination = details.Vaccination,
            VaccinationName = details.VaccinationName,
            Quantity = details.Quantity,
            Price = details.Price,
            Negotiable = details.Negotiable,
            Phone = details.Phone,
            WhatsApp = details.WhatsApp,
            Title = details.Title,
            Description = details.Description,
            PrimaryImageUrl = details.Images[0].Url,
            Images = details.Images,
            CreatedAt = details.CreatedAt
        };
    }
}
