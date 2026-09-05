using Shared.Constants;
using Shared.DTOs.Antiques;
using Shared.Enums;

namespace Services.AntiqueModules;

public static class AntiqueModuleExamples
{
    private static readonly Guid OwnerId = new("55555555-5555-5555-5555-555555555555");
    private static readonly Guid ImageId = new("44444444-4444-4444-4444-444444444444");
    private static readonly Guid VideoId = new("33333333-3333-3333-3333-333333333333");

    private static readonly DateTime CreatedAt = new(2026, 7, 20, 9, 30, 0, DateTimeKind.Utc);

    private const string Address = "الفيوم، شارع الحرية، بجوار المسجد الكبير";
    private const string Center = "الفيوم";
    private const string GoogleMaps = "https://maps.app.goo.gl/example";
    private const string Phone = "01012345678";
    private const string WhatsApp = "01087654321";

    private static readonly Guid DecorAntiqueId = new("b1000000-0000-0000-0000-000000000001");

    public static DecorAntiqueDetailsDto DecorAntiqueDetails() => new()
    {
        Id = DecorAntiqueId,
        OwnerId = OwnerId.ToString(),
        SellerName = "هالة عبد العزيز",
        Phone = Phone,
        WhatsApp = WhatsApp,
        ItemName = "فازة نحاسية منقوشة",
        ItemType = DecorAntiqueItemType.Vase,
        ItemTypeName = DecorAntiqueCatalog.GetItemTypeName(DecorAntiqueItemType.Vase),
        OtherItemType = null,
        Material = DecorAntiqueMaterial.Copper,
        MaterialName = DecorAntiqueCatalog.GetMaterialName(DecorAntiqueMaterial.Copper),
        OtherMaterial = null,
        Condition = DecorAntiqueCondition.Excellent,
        ConditionName = DecorAntiqueCatalog.GetConditionName(DecorAntiqueCondition.Excellent),
        Length = 18.00m,
        Width = 18.00m,
        Height = 42.50m,
        Weight = 3.20m,
        Originality = DecorAntiqueOriginality.Original,
        OriginalityName = DecorAntiqueCatalog.GetOriginalityName(DecorAntiqueOriginality.Original),
        Price = 4500.00m,
        Negotiable = true,
        Governorate = LocationConstants.Governorate,
        Center = Center,
        Address = Address,
        GoogleMaps = GoogleMaps,
        Title = "فازة نحاسية أثرية للبيع",
        Description = "فازة نحاس أصلية بنقش يدوي، محفوظة بحالة ممتازة ولم تُستخدم سوى للعرض.",
        Images =
        [
            new DecorAntiqueImageDto
            {
                Id = ImageId,
                Url = "https://api.example.com/uploads/decor-antiques/sample.jpg",
                IsPrimary = true
            }
        ],
        Video = new DecorAntiqueVideoDto
        {
            Id = VideoId,
            Url = "https://api.example.com/uploads/decor-antiques-video/sample.mp4"
        },
        CreatedAt = CreatedAt,
        UpdatedAt = null
    };

    public static DecorAntiqueListItemDto DecorAntiqueListItem()
    {
        var details = DecorAntiqueDetails();

        return new DecorAntiqueListItemDto
        {
            Id = details.Id,
            SellerName = details.SellerName,
            Phone = details.Phone,
            WhatsApp = details.WhatsApp,
            ItemName = details.ItemName,
            ItemType = details.ItemType,
            ItemTypeName = details.ItemTypeName,
            OtherItemType = details.OtherItemType,
            Material = details.Material,
            MaterialName = details.MaterialName,
            OtherMaterial = details.OtherMaterial,
            Condition = details.Condition,
            ConditionName = details.ConditionName,
            Originality = details.Originality,
            OriginalityName = details.OriginalityName,
            Price = details.Price,
            Negotiable = details.Negotiable,
            Center = details.Center,
            Title = details.Title,
            Description = details.Description,
            PrimaryImageUrl = details.Images[0].Url,
            Images = details.Images,
            Video = details.Video,
            CreatedAt = details.CreatedAt
        };
    }

    private static readonly Guid AntiqueId = new("b2000000-0000-0000-0000-000000000002");

    public static AntiqueDetailsDto AntiqueDetails() => new()
    {
        Id = AntiqueId,
        OwnerId = OwnerId.ToString(),
        SellerName = "مجدي شحاتة",
        Phone = Phone,
        WhatsApp = WhatsApp,
        AntiqueName = "راديو فيليبس قديم",
        AntiqueType = AntiqueType.Radio,
        AntiqueTypeName = AntiqueModuleCatalog.GetTypeName(AntiqueType.Radio),
        OtherType = null,
        ManufactureYear = 1958,
        CountryOfOrigin = "هولندا",
        Manufacturer = "Philips",
        Material = AntiqueMaterial.Wood,
        MaterialName = AntiqueModuleCatalog.GetMaterialName(AntiqueMaterial.Wood),
        OtherMaterial = null,
        Condition = AntiqueCondition.VeryGood,
        ConditionName = AntiqueModuleCatalog.GetConditionName(AntiqueCondition.VeryGood),
        WorkingStatus = AntiqueWorkingStatus.Working,
        WorkingStatusName = AntiqueModuleCatalog.GetWorkingStatusName(AntiqueWorkingStatus.Working),
        Originality = AntiqueOriginality.Original,
        OriginalityName = AntiqueModuleCatalog.GetOriginalityName(AntiqueOriginality.Original),
        Price = 12000.00m,
        Negotiable = true,
        Governorate = LocationConstants.Governorate,
        Center = Center,
        Address = Address,
        GoogleMaps = GoogleMaps,
        Title = "راديو قديم يعمل بحالة ممتازة",
        Description = "راديو فيليبس أصلي بصندوق خشبي، يعمل بكفاءة وجميع مفاتيحه سليمة.",
        Images =
        [
            new AntiqueImageDto
            {
                Id = ImageId,
                Url = "https://api.example.com/uploads/antiques/sample.jpg",
                IsPrimary = true
            }
        ],
        Video = new AntiqueVideoDto
        {
            Id = VideoId,
            Url = "https://api.example.com/uploads/antiques-video/sample.mp4"
        },
        CreatedAt = CreatedAt,
        UpdatedAt = null
    };

    public static AntiqueListItemDto AntiqueListItem()
    {
        var details = AntiqueDetails();

        return new AntiqueListItemDto
        {
            Id = details.Id,
            SellerName = details.SellerName,
            Phone = details.Phone,
            WhatsApp = details.WhatsApp,
            AntiqueName = details.AntiqueName,
            AntiqueType = details.AntiqueType,
            AntiqueTypeName = details.AntiqueTypeName,
            OtherType = details.OtherType,
            ManufactureYear = details.ManufactureYear,
            CountryOfOrigin = details.CountryOfOrigin,
            Manufacturer = details.Manufacturer,
            Material = details.Material,
            MaterialName = details.MaterialName,
            OtherMaterial = details.OtherMaterial,
            Condition = details.Condition,
            ConditionName = details.ConditionName,
            WorkingStatus = details.WorkingStatus,
            WorkingStatusName = details.WorkingStatusName,
            Originality = details.Originality,
            OriginalityName = details.OriginalityName,
            Price = details.Price,
            Negotiable = details.Negotiable,
            Center = details.Center,
            Title = details.Title,
            Description = details.Description,
            PrimaryImageUrl = details.Images[0].Url,
            Images = details.Images,
            Video = details.Video,
            CreatedAt = details.CreatedAt
        };
    }

    private static readonly Guid PaintingId = new("b3000000-0000-0000-0000-000000000003");

    public static PaintingDetailsDto PaintingDetails() => new()
    {
        Id = PaintingId,
        OwnerId = OwnerId.ToString(),
        SellerName = "ياسمين فؤاد",
        Phone = Phone,
        WhatsApp = WhatsApp,
        PaintingName = "غروب على بحيرة قارون",
        PaintingType = PaintingType.Oil,
        PaintingTypeName = PaintingCatalog.GetTypeName(PaintingType.Oil),
        OtherType = null,
        ArtistName = "ياسمين فؤاد",
        ExecutionYear = 2021,
        Width = 70.00m,
        Height = 50.00m,
        Material = PaintingMaterial.Canvas,
        MaterialName = PaintingCatalog.GetMaterialName(PaintingMaterial.Canvas),
        OtherMaterial = null,
        Framed = true,
        Originality = PaintingOriginality.Original,
        OriginalityName = PaintingCatalog.GetOriginalityName(PaintingOriginality.Original),
        SignedByArtist = true,
        Price = 8500.00m,
        Negotiable = false,
        Governorate = LocationConstants.Governorate,
        Center = Center,
        Address = Address,
        GoogleMaps = GoogleMaps,
        Title = "لوحة زيتية أصلية بتوقيع الفنان",
        Description = "لوحة زيتية على كانفاس تصور غروب الشمس على بحيرة قارون، بإطار خشبي مذهب.",
        Images =
        [
            new PaintingImageDto
            {
                Id = ImageId,
                Url = "https://api.example.com/uploads/paintings/sample.jpg",
                IsPrimary = true
            }
        ],
        Video = new PaintingVideoDto
        {
            Id = VideoId,
            Url = "https://api.example.com/uploads/paintings-video/sample.mp4"
        },
        CreatedAt = CreatedAt,
        UpdatedAt = null
    };

    public static PaintingListItemDto PaintingListItem()
    {
        var details = PaintingDetails();

        return new PaintingListItemDto
        {
            Id = details.Id,
            SellerName = details.SellerName,
            Phone = details.Phone,
            WhatsApp = details.WhatsApp,
            PaintingName = details.PaintingName,
            PaintingType = details.PaintingType,
            PaintingTypeName = details.PaintingTypeName,
            OtherType = details.OtherType,
            ArtistName = details.ArtistName,
            ExecutionYear = details.ExecutionYear,
            Width = details.Width,
            Height = details.Height,
            Material = details.Material,
            MaterialName = details.MaterialName,
            OtherMaterial = details.OtherMaterial,
            Framed = details.Framed,
            Originality = details.Originality,
            OriginalityName = details.OriginalityName,
            SignedByArtist = details.SignedByArtist,
            Price = details.Price,
            Negotiable = details.Negotiable,
            Center = details.Center,
            Title = details.Title,
            Description = details.Description,
            PrimaryImageUrl = details.Images[0].Url,
            Images = details.Images,
            Video = details.Video,
            CreatedAt = details.CreatedAt
        };
    }

    private static readonly Guid HandmadeId = new("b4000000-0000-0000-0000-000000000004");

    public static HandmadeDetailsDto HandmadeDetails() => new()
    {
        Id = HandmadeId,
        OwnerId = OwnerId.ToString(),
        SellerName = "منى السيد",
        Phone = Phone,
        WhatsApp = WhatsApp,
        ProductName = "شنطة كروشيه يدوية",
        HandmadeType = HandmadeType.Crochet,
        HandmadeTypeName = HandmadeCatalog.GetTypeName(HandmadeType.Crochet),
        OtherType = null,
        Material = "خيط قطن مصري 100%",
        IsFullyHandmade = true,
        CustomOrder = true,
        ProductionTime = "من 3 إلى 5 أيام",
        Size = "30 × 40 سم",
        Colors =
        [
            new HandmadeColorDto
            {
                Color = HandmadeColor.Beige,
                Name = HandmadeCatalog.GetColorName(HandmadeColor.Beige)
            },
            new HandmadeColorDto
            {
                Color = HandmadeColor.Brown,
                Name = HandmadeCatalog.GetColorName(HandmadeColor.Brown)
            }
        ],
        OtherColor = null,
        Price = 650.00m,
        Negotiable = true,
        Governorate = LocationConstants.Governorate,
        Center = Center,
        Address = Address,
        GoogleMaps = GoogleMaps,
        Title = "شنطة كروشيه هاند ميد",
        Description = "شنطة كروشيه مصنوعة يدويًا بالكامل من خيط قطن، وتتوفر بألوان أخرى حسب الطلب.",
        Images =
        [
            new HandmadeImageDto
            {
                Id = ImageId,
                Url = "https://api.example.com/uploads/handmade/sample.jpg",
                IsPrimary = true
            }
        ],
        Video = new HandmadeVideoDto
        {
            Id = VideoId,
            Url = "https://api.example.com/uploads/handmade-video/sample.mp4"
        },
        CreatedAt = CreatedAt,
        UpdatedAt = null
    };

    public static HandmadeListItemDto HandmadeListItem()
    {
        var details = HandmadeDetails();

        return new HandmadeListItemDto
        {
            Id = details.Id,
            SellerName = details.SellerName,
            Phone = details.Phone,
            WhatsApp = details.WhatsApp,
            ProductName = details.ProductName,
            HandmadeType = details.HandmadeType,
            HandmadeTypeName = details.HandmadeTypeName,
            OtherType = details.OtherType,
            Material = details.Material,
            IsFullyHandmade = details.IsFullyHandmade,
            CustomOrder = details.CustomOrder,
            ProductionTime = details.ProductionTime,
            Size = details.Size,
            Colors = details.Colors,
            OtherColor = details.OtherColor,
            Price = details.Price,
            Negotiable = details.Negotiable,
            Center = details.Center,
            Title = details.Title,
            Description = details.Description,
            PrimaryImageUrl = details.Images[0].Url,
            Images = details.Images,
            Video = details.Video,
            CreatedAt = details.CreatedAt
        };
    }

    private static readonly Guid CoinStampId = new("b5000000-0000-0000-0000-000000000005");

    public static CoinStampDetailsDto CoinStampDetails() => new()
    {
        Id = CoinStampId,
        OwnerId = OwnerId.ToString(),
        SellerName = "طارق الجندي",
        Phone = Phone,
        WhatsApp = WhatsApp,
        ItemName = "جنيه فضة مصري 1968",
        ItemType = CoinStampItemType.Coin,
        ItemTypeName = CoinStampCatalog.GetItemTypeName(CoinStampItemType.Coin),
        OtherType = null,
        Country = "مصر",
        IssueYear = 1968,
        Denomination = "1 جنيه",
        Metal = CoinStampMetal.Silver,
        MetalName = CoinStampCatalog.GetMetalName(CoinStampMetal.Silver),
        OtherMetal = null,
        Condition = CoinStampCondition.Uncirculated,
        ConditionName = CoinStampCatalog.GetConditionName(CoinStampCondition.Uncirculated),
        IsOriginal = true,
        IsRare = true,
        HasCertificate = true,
        Price = 3200.00m,
        Negotiable = false,
        Governorate = LocationConstants.Governorate,
        Center = Center,
        Address = Address,
        GoogleMaps = GoogleMaps,
        Title = "جنيه فضة مصري 1968",
        Description = "جنيه فضة إصدار السد العالي، حالة UNC ومحفوظ في كبسولة مع شهادة توثيق.",
        Images =
        [
            new CoinStampImageDto
            {
                Id = ImageId,
                Url = "https://api.example.com/uploads/coins-stamps/sample.jpg",
                IsPrimary = true
            }
        ],
        Video = new CoinStampVideoDto
        {
            Id = VideoId,
            Url = "https://api.example.com/uploads/coins-stamps-video/sample.mp4"
        },
        CreatedAt = CreatedAt,
        UpdatedAt = null
    };

    public static CoinStampListItemDto CoinStampListItem()
    {
        var details = CoinStampDetails();

        return new CoinStampListItemDto
        {
            Id = details.Id,
            SellerName = details.SellerName,
            Phone = details.Phone,
            WhatsApp = details.WhatsApp,
            ItemName = details.ItemName,
            ItemType = details.ItemType,
            ItemTypeName = details.ItemTypeName,
            OtherType = details.OtherType,
            Country = details.Country,
            IssueYear = details.IssueYear,
            Denomination = details.Denomination,
            Metal = details.Metal,
            MetalName = details.MetalName,
            OtherMetal = details.OtherMetal,
            Condition = details.Condition,
            ConditionName = details.ConditionName,
            IsOriginal = details.IsOriginal,
            IsRare = details.IsRare,
            HasCertificate = details.HasCertificate,
            Price = details.Price,
            Negotiable = details.Negotiable,
            Center = details.Center,
            Title = details.Title,
            Description = details.Description,
            PrimaryImageUrl = details.Images[0].Url,
            Images = details.Images,
            Video = details.Video,
            CreatedAt = details.CreatedAt
        };
    }
}
