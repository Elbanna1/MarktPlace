using Shared.Constants;
using Shared.DTOs.FruitVegetableMerchants;
using Shared.DTOs.Suppliers;
using Shared.DTOs.WholesaleTraders;
using Shared.Enums;

namespace Services.BusinessModules;

public static class BusinessModuleExamples
{
    private static readonly Guid SupplierId = new("11111111-1111-1111-1111-111111111111");
    private static readonly Guid TraderId = new("22222222-2222-2222-2222-222222222222");
    private static readonly Guid MerchantId = new("33333333-3333-3333-3333-333333333333");
    private static readonly Guid ImageId = new("44444444-4444-4444-4444-444444444444");
    private static readonly Guid OwnerId = new("55555555-5555-5555-5555-555555555555");

    private static readonly DateTime CreatedAt = new(2026, 7, 20, 9, 30, 0, DateTimeKind.Utc);

    private const string Center = "سنورس";

    public static SupplierDetailsDto SupplierDetails() => new()
    {
        Id = SupplierId,
        OwnerId = OwnerId.ToString(),
        SupplierName = "مؤسسة الأمل للتوريدات",
        SupplierType = SupplierSpecialization.PlasticRawMaterials,
        SupplierTypeName = SupplierCatalog.GetName(SupplierSpecialization.PlasticRawMaterials),
        SupplierTypeGroup = SupplierCatalog.GetGroup(SupplierSpecialization.PlasticRawMaterials),
        OtherSupplierType = null,
        SuppliedProduct = "حبيبات بولي إيثيلين",
        SupplyDetails =
            "بلد المنشأ: السعودية\n" +
            "نوع المنتج: حبيبات بولي إيثيلين عالي الكثافة\n" +
            "المواصفات: درجة نقاء 99%، مطابقة للمواصفة القياسية\n" +
            "المقاسات: شكاير 25 كجم\n" +
            "الجودة: درجة أولى\n" +
            "الكمية المتاحة: 40 طن شهريًا\n" +
            "مدة التسليم: من 3 إلى 5 أيام عمل\n" +
            "التوصيل: متاح داخل محافظة الفيوم",
        Address = "المنطقة الصناعية، شارع المصانع",
        GoogleMaps = "https://maps.app.goo.gl/example",
        Phone = "01012345678",
        WhatsApp = "01087654321",
        Email = "info@alamal-supply.com",
        Title = "توريد خامات بلاستيك بأسعار الجملة",
        Description = "نوفر خامات بلاستيك بجميع الدرجات مع خدمة التوصيل داخل الفيوم.",
        Images =
        [
            new SupplierImageDto { Id = ImageId, Url = "https://api.example.com/uploads/suppliers/sample.jpg", IsPrimary = true }
        ],
        CreatedAt = CreatedAt,
        UpdatedAt = null
    };

    public static SupplierListItemDto SupplierListItem()
    {
        var details = SupplierDetails();

        return new SupplierListItemDto
        {
            Id = details.Id,
            SupplierName = details.SupplierName,
            SupplierType = details.SupplierType,
            SupplierTypeName = details.SupplierTypeName,
            SupplierTypeGroup = details.SupplierTypeGroup,
            OtherSupplierType = details.OtherSupplierType,
            SuppliedProduct = details.SuppliedProduct,
            SupplyDetails = details.SupplyDetails,
            Phone = details.Phone,
            WhatsApp = details.WhatsApp,
            Title = details.Title,
            Description = details.Description,
            PrimaryImageUrl = details.Images[0].Url,
            Images = details.Images,
            CreatedAt = details.CreatedAt
        };
    }

    public static IReadOnlyList<SupplierSpecializationDto> SupplierTypes() =>
        SupplierCatalog.Specializations
            .Where(entry => entry.Value is SupplierSpecialization.PlasticRawMaterials
                or SupplierSpecialization.Seeds
                or SupplierSpecialization.VeterinaryMedicines
                or SupplierSpecialization.Cement
                or SupplierSpecialization.RestaurantSupplies
                or SupplierSpecialization.Other)
            .Select(entry => new SupplierSpecializationDto
            {
                Id = (int)entry.Value,
                Group = entry.Group,
                GroupAr = entry.GroupAr,
                Name = entry.Name
            })
            .ToList();

    public static WholesaleTraderDetailsDto WholesaleTraderDetails() => new()
    {
        Id = TraderId,
        OwnerId = OwnerId.ToString(),
        TraderName = "تجارة الفيوم للجملة",
        TradeType = WholesaleTradeType.Food,
        TradeTypeName = WholesaleTraderCatalog.GetTradeTypeName(WholesaleTradeType.Food),
        OtherTradeType = null,
        ProductsName = "زيوت وسمن ومعلبات",
        ProductDetails =
            "أنواع المنتجات: زيوت طعام، سمن نباتي، معلبات\n" +
            "الماركات: عافية، كريستال، السعدي\n" +
            "الجودة: درجة أولى\n" +
            "المقاسات: عبوات 1 لتر و5 لتر و10 لتر\n" +
            "الحد الأدنى للطلب: 20 كرتونة\n" +
            "التوصيل: متاح داخل محافظة الفيوم\n" +
            "البيع: جملة فقط",
        SaleType = WholesaleSaleType.Wholesale,
        SaleTypeName = WholesaleTraderCatalog.GetSaleTypeName(WholesaleSaleType.Wholesale),
        Address = "سوق الجملة، شارع النصر",
        GoogleMaps = "https://maps.app.goo.gl/example",
        Phone = "01112345678",
        WhatsApp = "01187654321",
        Email = "sales@fayoum-wholesale.com",
        Title = "تجارة جملة زيوت ومعلبات",
        Description = "توريد بالجملة للمحلات والسوبر ماركت داخل الفيوم.",
        Images =
        [
            new WholesaleTraderImageDto { Id = ImageId, Url = "https://api.example.com/uploads/wholesale-traders/sample.jpg", IsPrimary = true }
        ],
        CreatedAt = CreatedAt,
        UpdatedAt = null
    };

    public static WholesaleTraderListItemDto WholesaleTraderListItem()
    {
        var details = WholesaleTraderDetails();

        return new WholesaleTraderListItemDto
        {
            Id = details.Id,
            TraderName = details.TraderName,
            TradeType = details.TradeType,
            TradeTypeName = details.TradeTypeName,
            OtherTradeType = details.OtherTradeType,
            ProductsName = details.ProductsName,
            ProductDetails = details.ProductDetails,
            SaleType = details.SaleType,
            SaleTypeName = details.SaleTypeName,
            Phone = details.Phone,
            WhatsApp = details.WhatsApp,
            Title = details.Title,
            Description = details.Description,
            PrimaryImageUrl = details.Images[0].Url,
            Images = details.Images,
            CreatedAt = details.CreatedAt
        };
    }

    public static IReadOnlyList<WholesaleTradeTypeDto> WholesaleTradeTypes() =>
        WholesaleTraderCatalog.TradeTypes
            .Select(entry => new WholesaleTradeTypeDto
            {
                Id = (int)entry.Value,
                Name = entry.Name
            })
            .ToList();

    public static IReadOnlyList<WholesaleSaleTypeDto> WholesaleSaleTypes() =>
        WholesaleTraderCatalog.SaleTypeNames
            .Select(entry => new WholesaleSaleTypeDto
            {
                Id = (int)entry.Key,
                Name = entry.Value
            })
            .ToList();

    public static FruitVegetableMerchantDetailsDto MerchantDetails() => new()
    {
        Id = MerchantId,
        OwnerId = OwnerId.ToString(),
        StallName = "محل الخير للخضار والفاكهة",
        MerchantName = "محمود عبد الرحمن",
        Phone = "01212345678",
        WhatsApp = "01287654321",
        Address = "سوق الخضار، شارع الجمهورية",
        GoogleMaps = "https://maps.app.goo.gl/example",
        ProductName = "طماطم وبطاطس وبصل",
        SaleType = MerchantSaleType.Both,
        SaleTypeName = FruitVegetableMerchantCatalog.GetSaleTypeName(MerchantSaleType.Both),
        ProductDetails =
            "نوع المنتج: خضروات طازجة يوميًا\n" +
            "الجودة: درجة أولى، فرز ممتاز\n" +
            "الحجم: عبوات 10 كجم و25 كجم\n" +
            "الكمية المتاحة: 3 طن يوميًا",
        Title = "خضار وفاكهة طازجة قطاعي وجملة",
        Description = "توريد يومي من المزرعة مباشرة، مع خدمة التوصيل للمحلات.",
        Images =
        [
            new FruitVegetableMerchantImageDto { Id = ImageId, Url = "https://api.example.com/uploads/fruit-vegetable-merchants/sample.jpg", IsPrimary = true }
        ],
        CreatedAt = CreatedAt,
        UpdatedAt = null
    };

    public static FruitVegetableMerchantListItemDto MerchantListItem()
    {
        var details = MerchantDetails();

        return new FruitVegetableMerchantListItemDto
        {
            Id = details.Id,
            StallName = details.StallName,
            MerchantName = details.MerchantName,
            Phone = details.Phone,
            WhatsApp = details.WhatsApp,
            ProductName = details.ProductName,
            SaleType = details.SaleType,
            SaleTypeName = details.SaleTypeName,
            ProductDetails = details.ProductDetails,
            Title = details.Title,
            Description = details.Description,
            PrimaryImageUrl = details.Images[0].Url,
            Images = details.Images,
            CreatedAt = details.CreatedAt
        };
    }

    public static IReadOnlyList<MerchantSaleTypeDto> MerchantSaleTypes() =>
        FruitVegetableMerchantCatalog.SaleTypeNames
            .Select(entry => new MerchantSaleTypeDto
            {
                Id = (int)entry.Key,
                Name = entry.Value
            })
            .ToList();
}
