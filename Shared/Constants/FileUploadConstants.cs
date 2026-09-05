namespace Shared.Constants;

public static class FileUploadConstants
{
    public const long MaxDocumentSizeBytes = 10 * 1024 * 1024;

    public const long MaxVideoSizeBytes = 50 * 1024 * 1024;

    public const long MaxJobRequestBodySizeBytes =
        MaxVideoSizeBytes + MaxDocumentSizeBytes + ImageConstants.MaxFileSizeBytes + (5 * 1024 * 1024);

    public const long MaxAntiqueRequestBodySizeBytes =
        ImageConstants.MaxRequestBodySizeBytes + MaxVideoSizeBytes;

    public const long MaxClothingRequestBodySizeBytes =
        ImageConstants.MaxRequestBodySizeBytes + MaxVideoSizeBytes;

    public const long MaxOnlineShoppingRequestBodySizeBytes =
        ImageConstants.MaxRequestBodySizeBytes + MaxVideoSizeBytes;

    public const long MaxHomeFurnishingRequestBodySizeBytes =
        ImageConstants.MaxRequestBodySizeBytes + MaxVideoSizeBytes;

    public const long MaxRealEstateRequestBodySizeBytes =
        ImageConstants.MaxRequestBodySizeBytes + MaxVideoSizeBytes;

    public const long MaxAdvertisementRequestBodySizeBytes =
        ImageConstants.MaxRequestBodySizeBytes + MaxVideoSizeBytes;

    public const long MaxBannerBookingRequestBodySizeBytes =
        (3 * ImageConstants.MaxFileSizeBytes) + (2 * 1024 * 1024);

    public static readonly long MaxRequestBodySizeBytes = new[]
    {
        ImageConstants.MaxRequestBodySizeBytes,
        MaxAdvertisementRequestBodySizeBytes,
        MaxJobRequestBodySizeBytes,
        MaxAntiqueRequestBodySizeBytes,
        MaxClothingRequestBodySizeBytes,
        MaxOnlineShoppingRequestBodySizeBytes,
        MaxHomeFurnishingRequestBodySizeBytes,
        MaxRealEstateRequestBodySizeBytes,
        MaxBannerBookingRequestBodySizeBytes
    }.Max();

    public const string AdsVideoFolder = "ads-video";

    public static readonly string AllowedDocumentFormatNames = DocumentFormatCatalog.DisplayNames;

    public static readonly string AllowedVideoFormatNames = VideoFormatCatalog.DisplayNames;

    public const string JobRequestsFolder = "job-requests";

    public const string JobRequestCvFolder = "job-requests-cv";

    public const string JobRequestVideoFolder = "job-requests-video";

    public const string JobOpportunitiesFolder = "job-opportunities";

    public static readonly string[] JobFolders =
    {
        JobRequestsFolder, JobRequestCvFolder, JobRequestVideoFolder, JobOpportunitiesFolder
    };

    public const string DecorAntiqueVideoFolder = "decor-antiques-video";

    public const string AntiqueVideoFolder = "antiques-video";

    public const string PaintingVideoFolder = "paintings-video";

    public const string HandmadeVideoFolder = "handmade-video";

    public const string CoinStampVideoFolder = "coins-stamps-video";

    public static readonly string[] AntiqueVideoFolders =
    {
        DecorAntiqueVideoFolder, AntiqueVideoFolder, PaintingVideoFolder,
        HandmadeVideoFolder, CoinStampVideoFolder
    };

    public const string MenClothingVideoFolder = "men-clothing-video";

    public const string WomenClothingVideoFolder = "women-clothing-video";

    public const string KidsClothingVideoFolder = "kids-clothing-video";

    public static readonly string[] ClothingVideoFolders =
    {
        MenClothingVideoFolder, WomenClothingVideoFolder, KidsClothingVideoFolder
    };

    public const string AccessoryVideoFolder = "accessories-video";

    public const string CosmeticVideoFolder = "cosmetics-video";

    public const string HomeKitchenVideoFolder = "home-kitchen-video";

    public const string ShoppingElectronicVideoFolder = "shopping-electronics-video";

    public const string GiftToyVideoFolder = "gifts-toys-video";

    public const string HomemadeFoodVideoFolder = "homemade-food-video";

    public static readonly string[] OnlineShoppingVideoFolders =
    {
        AccessoryVideoFolder, CosmeticVideoFolder, HomeKitchenVideoFolder,
        ShoppingElectronicVideoFolder, GiftToyVideoFolder, HomemadeFoodVideoFolder
    };

    public const string FurnitureVideoFolder = "furniture-video";

    public const string FurnishingCurtainVideoFolder = "furnishings-curtains-video";

    public const string LightingDecorVideoFolder = "lighting-decor-video";

    public const string KitchenToolVideoFolder = "kitchen-tools-video";

    public const string HomeApplianceVideoFolder = "home-appliances-video";

    public const string BathroomSupplyVideoFolder = "bathroom-supplies-video";

    public const string PlantOrnamentVideoFolder = "plants-ornaments-video";

    public static readonly string[] HomeFurnishingVideoFolders =
    {
        FurnitureVideoFolder, FurnishingCurtainVideoFolder, LightingDecorVideoFolder,
        KitchenToolVideoFolder, HomeApplianceVideoFolder, BathroomSupplyVideoFolder,
        PlantOrnamentVideoFolder
    };

    public const string LandVideoFolder = "lands-video";

    public const string ApartmentVideoFolder = "apartments-video";

    public const string ShopVideoFolder = "shops-video";

    public static readonly string[] RealEstateVideoFolders =
    {
        LandVideoFolder, ApartmentVideoFolder, ShopVideoFolder
    };

    public const string RescuesFolder = "rescues";

    public const string BloodRequestsFolder = "blood-requests";

    public const string AskConsultsFolder = "ask-consults";

    public static readonly string[] CharityFolders =
    {
        RescuesFolder, BloodRequestsFolder, AskConsultsFolder
    };
}
