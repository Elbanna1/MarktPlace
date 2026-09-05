using Shared.Constants;
using Shared.DTOs.Clothing;
using Shared.Enums;

namespace Services.ClothingModules;

public static class ClothingModuleExamples
{
    private static readonly Guid OwnerId = new("55555555-5555-5555-5555-555555555555");
    private static readonly Guid ImageId = new("44444444-4444-4444-4444-444444444444");

    private static readonly DateTime CreatedAt = new(2026, 7, 25, 11, 15, 0, DateTimeKind.Utc);

    private static readonly Guid MenClothingId = new("c1000000-0000-0000-0000-000000000001");

    public static MenClothingDetailsDto MenClothingDetails() => new()
    {
        Id = MenClothingId,
        OwnerId = OwnerId.ToString(),
        StoreName = "محل الأناقة للملابس الرجالي",
        SellingMethod = MenClothingSellingMethod.StoreAndOnline,
        SellingMethodName = MenClothingCatalog.GetSellingMethodName(MenClothingSellingMethod.StoreAndOnline),
        ClothingType = MenClothingType.TShirt,
        ClothingTypeName = MenClothingCatalog.GetClothingTypeName(MenClothingType.TShirt),
        OtherClothingType = null,
        Brand = MenClothingBrand.Imported,
        BrandName = MenClothingCatalog.GetBrandName(MenClothingBrand.Imported),
        OtherBrand = null,
        Sizes = MenClothingCatalog.SelectedSizes(
        [
            MenClothingSize.M, MenClothingSize.L, MenClothingSize.XL
        ]),
        Colors = MenClothingCatalog.SelectedColors(
        [
            MenClothingColor.Black, MenClothingColor.White, MenClothingColor.Navy
        ]),
        OtherColor = null,
        Condition = MenClothingCondition.New,
        ConditionName = MenClothingCatalog.GetConditionName(MenClothingCondition.New),
        Price = 350.00m,
        DeliveryAvailable = true,
        Governorate = LocationConstants.Governorate,
        Center = "الفيوم",
        Address = "شارع الحرية، أمام المحكمة",
        GoogleMaps = "https://maps.app.goo.gl/example",
        Phone = "01012345678",
        WhatsApp = "01087654321",
        Email = "store@example.com",
        Title = "تيشيرتات رجالي قطن مستورد",
        Description = "تيشيرتات قطن 100% مستوردة، متوفرة بعدة مقاسات وألوان، والتوصيل متاح داخل الفيوم.",
        Images =
        [
            new MenClothingImageDto
            {
                Id = ImageId,
                Url = "https://api.example.com/uploads/men-clothing/sample.jpg",
                IsPrimary = true
            }
        ],
        VideoUrl = "https://api.example.com/uploads/men-clothing-video/sample.mp4",
        CreatedAt = CreatedAt,
        UpdatedAt = null
    };

    public static MenClothingListItemDto MenClothingListItem()
    {
        var details = MenClothingDetails();

        return new MenClothingListItemDto
        {
            Id = details.Id,
            StoreName = details.StoreName,
            SellingMethod = details.SellingMethod,
            SellingMethodName = details.SellingMethodName,
            ClothingType = details.ClothingType,
            ClothingTypeName = details.ClothingTypeName,
            OtherClothingType = details.OtherClothingType,
            Brand = details.Brand,
            BrandName = details.BrandName,
            OtherBrand = details.OtherBrand,
            Sizes = details.Sizes,
            Colors = details.Colors,
            OtherColor = details.OtherColor,
            Condition = details.Condition,
            ConditionName = details.ConditionName,
            Price = details.Price,
            DeliveryAvailable = details.DeliveryAvailable,
            Governorate = details.Governorate,
            Center = details.Center,
            Phone = details.Phone,
            WhatsApp = details.WhatsApp,
            Title = details.Title,
            Description = details.Description,
            PrimaryImageUrl = details.Images[0].Url,
            Images = details.Images,
            VideoUrl = details.VideoUrl,
            CreatedAt = details.CreatedAt
        };
    }

    private static readonly Guid WomenClothingId = new("c2000000-0000-0000-0000-000000000002");

    public static WomenClothingDetailsDto WomenClothingDetails() => new()
    {
        Id = WomenClothingId,
        OwnerId = OwnerId.ToString(),
        StoreName = "بوتيك الياسمين",
        SellingMethod = WomenClothingSellingMethod.Store,
        SellingMethodName = WomenClothingCatalog.GetSellingMethodName(WomenClothingSellingMethod.Store),
        ClothingType = WomenClothingType.Dress,
        ClothingTypeName = WomenClothingCatalog.GetClothingTypeName(WomenClothingType.Dress),
        OtherClothingType = null,
        Brand = WomenClothingBrand.Local,
        BrandName = WomenClothingCatalog.GetBrandName(WomenClothingBrand.Local),
        OtherBrand = null,
        Sizes = WomenClothingCatalog.SelectedSizes(
        [
            WomenClothingSize.S, WomenClothingSize.M, WomenClothingSize.L
        ]),
        Colors = WomenClothingCatalog.SelectedColors(
        [
            WomenClothingColor.Black, WomenClothingColor.Pink, WomenClothingColor.Purple
        ]),
        OtherColor = null,
        Condition = WomenClothingCondition.NewClearance,
        ConditionName = WomenClothingCatalog.GetConditionName(WomenClothingCondition.NewClearance),
        Price = 750.00m,
        DeliveryAvailable = false,
        Governorate = LocationConstants.Governorate,
        Center = "سنورس",
        Address = "شارع السوق الرئيسي",
        GoogleMaps = "https://maps.app.goo.gl/example",
        Phone = "01112345678",
        WhatsApp = "01187654321",
        Email = null,
        Title = "فساتين سواريه حريمي",
        Description = "فساتين سواريه بخامات ممتازة، تشكيلة متجددة ومقاسات متعددة، خصومات على التشكيلة القديمة.",
        Images =
        [
            new WomenClothingImageDto
            {
                Id = ImageId,
                Url = "https://api.example.com/uploads/women-clothing/sample.jpg",
                IsPrimary = true
            }
        ],
        VideoUrl = null,
        CreatedAt = CreatedAt,
        UpdatedAt = null
    };

    public static WomenClothingListItemDto WomenClothingListItem()
    {
        var details = WomenClothingDetails();

        return new WomenClothingListItemDto
        {
            Id = details.Id,
            StoreName = details.StoreName,
            SellingMethod = details.SellingMethod,
            SellingMethodName = details.SellingMethodName,
            ClothingType = details.ClothingType,
            ClothingTypeName = details.ClothingTypeName,
            OtherClothingType = details.OtherClothingType,
            Brand = details.Brand,
            BrandName = details.BrandName,
            OtherBrand = details.OtherBrand,
            Sizes = details.Sizes,
            Colors = details.Colors,
            OtherColor = details.OtherColor,
            Condition = details.Condition,
            ConditionName = details.ConditionName,
            Price = details.Price,
            DeliveryAvailable = details.DeliveryAvailable,
            Governorate = details.Governorate,
            Center = details.Center,
            Phone = details.Phone,
            WhatsApp = details.WhatsApp,
            Title = details.Title,
            Description = details.Description,
            PrimaryImageUrl = details.Images[0].Url,
            Images = details.Images,
            VideoUrl = details.VideoUrl,
            CreatedAt = details.CreatedAt
        };
    }

    private static readonly Guid KidsClothingId = new("c3000000-0000-0000-0000-000000000003");

    public static KidsClothingDetailsDto KidsClothingDetails() => new()
    {
        Id = KidsClothingId,
        OwnerId = OwnerId.ToString(),
        StoreName = "عالم الأطفال",
        SellingMethod = KidsClothingSellingMethod.Online,
        SellingMethodName = KidsClothingCatalog.GetSellingMethodName(KidsClothingSellingMethod.Online),
        ClothingType = KidsClothingType.NewbornClothing,
        ClothingTypeName = KidsClothingCatalog.GetClothingTypeName(KidsClothingType.NewbornClothing),
        OtherClothingType = null,
        Brand = KidsClothingBrand.Local,
        BrandName = KidsClothingCatalog.GetBrandName(KidsClothingBrand.Local),
        OtherBrand = null,
        Sizes = KidsClothingCatalog.SelectedSizes(
        [
            KidsClothingSize.Newborn, KidsClothingSize.Months0To3, KidsClothingSize.Months3To6
        ]),
        Colors = KidsClothingCatalog.SelectedColors(
        [
            KidsClothingColor.White, KidsClothingColor.SkyBlue, KidsClothingColor.Pink
        ]),
        OtherColor = null,
        Condition = KidsClothingCondition.New,
        ConditionName = KidsClothingCatalog.GetConditionName(KidsClothingCondition.New),
        Price = 180.00m,
        DeliveryAvailable = true,
        Governorate = LocationConstants.Governorate,
        Center = "طامية",
        Address = "شارع المدارس، بجوار الصيدلية",

        GoogleMaps = null,
        Phone = "01212345678",
        WhatsApp = "01287654321",
        Email = "kids@example.com",
        Title = "أطقم أطفال قطن مواليد",
        Description = "أطقم مواليد قطن ناعم على البشرة، متوفرة بعدة مقاسات وألوان، والتوصيل متاح لكل المراكز.",
        Images =
        [
            new KidsClothingImageDto
            {
                Id = ImageId,
                Url = "https://api.example.com/uploads/kids-clothing/sample.jpg",
                IsPrimary = true
            }
        ],
        VideoUrl = null,
        CreatedAt = CreatedAt,
        UpdatedAt = null
    };

    public static KidsClothingListItemDto KidsClothingListItem()
    {
        var details = KidsClothingDetails();

        return new KidsClothingListItemDto
        {
            Id = details.Id,
            StoreName = details.StoreName,
            SellingMethod = details.SellingMethod,
            SellingMethodName = details.SellingMethodName,
            ClothingType = details.ClothingType,
            ClothingTypeName = details.ClothingTypeName,
            OtherClothingType = details.OtherClothingType,
            Brand = details.Brand,
            BrandName = details.BrandName,
            OtherBrand = details.OtherBrand,
            Sizes = details.Sizes,
            Colors = details.Colors,
            OtherColor = details.OtherColor,
            Condition = details.Condition,
            ConditionName = details.ConditionName,
            Price = details.Price,
            DeliveryAvailable = details.DeliveryAvailable,
            Governorate = details.Governorate,
            Center = details.Center,
            Phone = details.Phone,
            WhatsApp = details.WhatsApp,
            Title = details.Title,
            Description = details.Description,
            PrimaryImageUrl = details.Images[0].Url,
            Images = details.Images,
            VideoUrl = details.VideoUrl,
            CreatedAt = details.CreatedAt
        };
    }
}
