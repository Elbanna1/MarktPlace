namespace Shared.Constants;

public static class ImageConstants
{
    public const int MaxImagesPerItem = 10;

    public const int MaxImagesPerAdvertisement = MaxImagesPerItem;

    public const long MaxFileSizeBytes = 5 * 1024 * 1024;

    public const long MaxRequestBodySizeBytes = FileUploadConstants.MaxRequestBodySizeBytes;

    public static readonly string[] AllowedExtensions = ImageFormatCatalog.AllExtensions;

    public static readonly string[] AllowedContentTypes = ImageFormatCatalog.AllContentTypes;

    public static readonly string AllowedFormatNames = ImageFormatCatalog.DisplayNames;

    public const string UploadsRootFolder = "uploads";

    public const string AdsFolder = "ads";

    public const string ProfileFolder = "profile";

    public const string LostFoundFolder = "lostfound";

    public const string WorkshopsFolder = "workshops";

    public const string CraftsmenFolder = "craftsmen";

    public const string PaymentsFolder = "payments";

    public const string BannersFolder = "banners";

    public const string HomeFolder = "home";

    public const string PlatformFolder = "platform";

    public const string BannerBookingsFolder = "banner-bookings";

    public const string BannerBookingProofFolder = "banner-bookings-proof";

    public const string BannerBookingPreviewFolder = "banner-bookings-preview";

    public const string FeedbackFolder = "feedback";

    public const string FactoriesFolder = "factories";

    public const string FarmsFolder = "farms";

    public const string CompaniesFolder = "companies";

    public const string SuppliersFolder = "suppliers";

    public const string WholesaleTradersFolder = "wholesale-traders";

    public const string FruitVegetableMerchantsFolder = "fruit-vegetable-merchants";

    public const string LivestockFolder = "livestock";

    public const string SheepGoatFolder = "sheep-goats";

    public const string HorseFolder = "horses";

    public const string CamelFolder = "camels";

    public const string BirdFolder = "birds";

    public const string PetFolder = "pets";

    public const string FishFolder = "fish";

    public const string BeeFolder = "bees";

    public const string OtherAnimalFolder = "other-animals";

    public const string DecorAntiqueFolder = "decor-antiques";

    public const string AntiqueFolder = "antiques";

    public const string PaintingFolder = "paintings";

    public const string HandmadeFolder = "handmade";

    public const string CoinStampFolder = "coins-stamps";

    public const string MenClothingFolder = "men-clothing";

    public const string WomenClothingFolder = "women-clothing";

    public const string KidsClothingFolder = "kids-clothing";

    public const string AccessoryFolder = "accessories";

    public const string CosmeticFolder = "cosmetics";

    public const string HomeKitchenFolder = "home-kitchen";

    public const string ShoppingElectronicFolder = "shopping-electronics";

    public const string GiftToyFolder = "gifts-toys";

    public const string HomemadeFoodFolder = "homemade-food";

    public const string FurnitureFolder = "furniture";

    public const string FurnishingCurtainFolder = "furnishings-curtains";

    public const string LightingDecorFolder = "lighting-decor";

    public const string KitchenToolFolder = "kitchen-tools";

    public const string HomeApplianceFolder = "home-appliances";

    public const string BathroomSupplyFolder = "bathroom-supplies";

    public const string PlantOrnamentFolder = "plants-ornaments";

    public const string LandFolder = "lands";

    public const string ApartmentFolder = "apartments";

    public const string ShopFolder = "shops";

    public static readonly string[] AllFolders =
    {
        AdsFolder, ProfileFolder, LostFoundFolder, WorkshopsFolder, CraftsmenFolder, PaymentsFolder,
        FactoriesFolder, FarmsFolder, CompaniesFolder, SuppliersFolder,
        WholesaleTradersFolder, FruitVegetableMerchantsFolder,

        BannersFolder,

        HomeFolder, PlatformFolder,

        BannerBookingsFolder, BannerBookingProofFolder, BannerBookingPreviewFolder,

        FeedbackFolder,

        FileUploadConstants.AdsVideoFolder,

        LivestockFolder, SheepGoatFolder, HorseFolder, CamelFolder, BirdFolder,
        PetFolder, FishFolder, BeeFolder, OtherAnimalFolder,

        DecorAntiqueFolder, AntiqueFolder, PaintingFolder, HandmadeFolder, CoinStampFolder,

        FileUploadConstants.JobRequestsFolder, FileUploadConstants.JobRequestCvFolder,
        FileUploadConstants.JobRequestVideoFolder, FileUploadConstants.JobOpportunitiesFolder,

        FileUploadConstants.DecorAntiqueVideoFolder, FileUploadConstants.AntiqueVideoFolder,
        FileUploadConstants.PaintingVideoFolder, FileUploadConstants.HandmadeVideoFolder,
        FileUploadConstants.CoinStampVideoFolder,

        MenClothingFolder, WomenClothingFolder, KidsClothingFolder,
        FileUploadConstants.MenClothingVideoFolder, FileUploadConstants.WomenClothingVideoFolder,
        FileUploadConstants.KidsClothingVideoFolder,

        AccessoryFolder, CosmeticFolder, HomeKitchenFolder, ShoppingElectronicFolder,
        GiftToyFolder, HomemadeFoodFolder,
        FileUploadConstants.AccessoryVideoFolder, FileUploadConstants.CosmeticVideoFolder,
        FileUploadConstants.HomeKitchenVideoFolder, FileUploadConstants.ShoppingElectronicVideoFolder,
        FileUploadConstants.GiftToyVideoFolder, FileUploadConstants.HomemadeFoodVideoFolder,

        FurnitureFolder, FurnishingCurtainFolder, LightingDecorFolder, KitchenToolFolder,
        HomeApplianceFolder, BathroomSupplyFolder, PlantOrnamentFolder,
        FileUploadConstants.FurnitureVideoFolder, FileUploadConstants.FurnishingCurtainVideoFolder,
        FileUploadConstants.LightingDecorVideoFolder, FileUploadConstants.KitchenToolVideoFolder,
        FileUploadConstants.HomeApplianceVideoFolder, FileUploadConstants.BathroomSupplyVideoFolder,
        FileUploadConstants.PlantOrnamentVideoFolder,

        LandFolder, ApartmentFolder, ShopFolder,
        FileUploadConstants.LandVideoFolder, FileUploadConstants.ApartmentVideoFolder,
        FileUploadConstants.ShopVideoFolder,

        FileUploadConstants.RescuesFolder, FileUploadConstants.BloodRequestsFolder,
        FileUploadConstants.AskConsultsFolder
    };
}
