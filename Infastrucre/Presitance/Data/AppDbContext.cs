using Domain.Entities;
using Domain.Entities.Listings;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Persistence.Configurations;
using ServicesAbstraction;
using System.Reflection;

namespace Persistence.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    private readonly IAdminActionContext? _caller;

    public AppDbContext(DbContextOptions<AppDbContext> options, IAdminActionContext? caller = null)
        : base(options)
    {
        _caller = caller;
    }

    public string? ViewerUserId => _caller?.UserId;

    public DbSet<Advertisement> Advertisements => Set<Advertisement>();
    public DbSet<AdvertisementImage> AdvertisementImages => Set<AdvertisementImage>();
    public DbSet<AdvertisementFeature> AdvertisementFeatures => Set<AdvertisementFeature>();
    public DbSet<AdvertisementView> AdvertisementViews => Set<AdvertisementView>();
    public DbSet<Feature> Features => Set<Feature>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<SubCategory> SubCategories => Set<SubCategory>();
    public DbSet<Notification> Notifications => Set<Notification>();

    public DbSet<UserNotificationInterest> UserNotificationInterests => Set<UserNotificationInterest>();
    public DbSet<UserNotificationPreference> UserNotificationPreferences => Set<UserNotificationPreference>();
    public DbSet<ListingNotificationDispatch> ListingNotificationDispatches => Set<ListingNotificationDispatch>();

    public DbSet<Project> Projects => Set<Project>();

    public DbSet<HomeSection> HomeSections => Set<HomeSection>();

    public DbSet<PlatformSetting> PlatformSettings => Set<PlatformSetting>();

    public DbSet<AdFormFieldOverride> AdFormFieldOverrides => Set<AdFormFieldOverride>();
    public DbSet<AdFormFieldOption> AdFormFieldOptions => Set<AdFormFieldOption>();

    public DbSet<AdminAuditLog> AdminAuditLogs => Set<AdminAuditLog>();

    public DbSet<AdminPageGrant> AdminPageGrants => Set<AdminPageGrant>();
    public DbSet<AdminPagePermission> AdminPagePermissions => Set<AdminPagePermission>();

    public DbSet<ListingViewCounter> ListingViewCounters => Set<ListingViewCounter>();
    public DbSet<ListingViewer> ListingViewers => Set<ListingViewer>();
    public DbSet<ListingFavorite> ListingFavorites => Set<ListingFavorite>();
    public DbSet<ListingReport> ListingReports => Set<ListingReport>();
    public DbSet<ListingRating> ListingRatings => Set<ListingRating>();

    public DbSet<Feedback> Feedbacks => Set<Feedback>();
    public DbSet<FeedbackImage> FeedbackImages => Set<FeedbackImage>();

    public DbSet<LostFoundPost> LostFoundPosts => Set<LostFoundPost>();
    public DbSet<LostFoundImage> LostFoundImages => Set<LostFoundImage>();
    public DbSet<LostFoundLike> LostFoundLikes => Set<LostFoundLike>();
    public DbSet<LostFoundComment> LostFoundComments => Set<LostFoundComment>();

    public DbSet<Workshop> Workshops => Set<Workshop>();
    public DbSet<WorkshopImage> WorkshopImages => Set<WorkshopImage>();
    public DbSet<Craftsman> Craftsmen => Set<Craftsman>();
    public DbSet<CraftsmanImage> CraftsmanImages => Set<CraftsmanImage>();

    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<SupplierImage> SupplierImages => Set<SupplierImage>();
    public DbSet<SupplierSpecializationLookup> SupplierSpecializations => Set<SupplierSpecializationLookup>();

    public DbSet<WholesaleTrader> WholesaleTraders => Set<WholesaleTrader>();
    public DbSet<WholesaleTraderImage> WholesaleTraderImages => Set<WholesaleTraderImage>();
    public DbSet<WholesaleTradeTypeLookup> WholesaleTradeTypes => Set<WholesaleTradeTypeLookup>();

    public DbSet<FruitVegetableMerchant> FruitVegetableMerchants => Set<FruitVegetableMerchant>();
    public DbSet<FruitVegetableMerchantImage> FruitVegetableMerchantImages => Set<FruitVegetableMerchantImage>();
    public DbSet<MerchantSaleTypeLookup> MerchantSaleTypes => Set<MerchantSaleTypeLookup>();

    public DbSet<PaymentMethod> PaymentMethods => Set<PaymentMethod>();
    public DbSet<Payment> Payments => Set<Payment>();

    public DbSet<Banner> Banners => Set<Banner>();

    public DbSet<BannerPlacementSetting> BannerPlacementSettings => Set<BannerPlacementSetting>();
    public DbSet<BannerBooking> BannerBookings => Set<BannerBooking>();

    public DbSet<Factory> Factories => Set<Factory>();
    public DbSet<FactoryImage> FactoryImages => Set<FactoryImage>();
    public DbSet<Farm> Farms => Set<Farm>();
    public DbSet<FarmImage> FarmImages => Set<FarmImage>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<CompanyImage> CompanyImages => Set<CompanyImage>();

    public DbSet<ProductionSpecialtyLookup> ProductionSpecialties => Set<ProductionSpecialtyLookup>();
    public DbSet<FarmTypeLookup> FarmTypes => Set<FarmTypeLookup>();
    public DbSet<AvailabilitySeasonLookup> AvailabilitySeasons => Set<AvailabilitySeasonLookup>();
    public DbSet<FarmingMethodLookup> FarmingMethods => Set<FarmingMethodLookup>();
    public DbSet<CompanyFieldLookup> CompanyFields => Set<CompanyFieldLookup>();
    public DbSet<SupplierTypeLookup> SupplierTypes => Set<SupplierTypeLookup>();
    public DbSet<TradeTypeLookup> TradeTypes => Set<TradeTypeLookup>();
    public DbSet<SaleTypeLookup> SaleTypes => Set<SaleTypeLookup>();

    public DbSet<JobRequest> JobRequests => Set<JobRequest>();

    public DbSet<JobOpportunity> JobOpportunities => Set<JobOpportunity>();
    public DbSet<JobOpportunityImage> JobOpportunityImages => Set<JobOpportunityImage>();

    public DbSet<JobFieldLookup> JobFields => Set<JobFieldLookup>();
    public DbSet<JobExperienceLevelLookup> JobExperienceLevels => Set<JobExperienceLevelLookup>();
    public DbSet<EducationLevelLookup> EducationLevels => Set<EducationLevelLookup>();
    public DbSet<WorkTypeLookup> WorkTypes => Set<WorkTypeLookup>();
    public DbSet<SalaryTypeLookup> SalaryTypes => Set<SalaryTypeLookup>();

    public DbSet<Livestock> Livestock => Set<Livestock>();
    public DbSet<LivestockImage> LivestockImages => Set<LivestockImage>();
    public DbSet<LivestockBreedLookup> LivestockBreeds => Set<LivestockBreedLookup>();
    public DbSet<LivestockPurposeLookup> LivestockPurposes => Set<LivestockPurposeLookup>();
    public DbSet<LivestockAgeLookup> LivestockAges => Set<LivestockAgeLookup>();
    public DbSet<LivestockGenderLookup> LivestockGenders => Set<LivestockGenderLookup>();
    public DbSet<LivestockHealthStatusLookup> LivestockHealthStatuses => Set<LivestockHealthStatusLookup>();
    public DbSet<LivestockVaccinationLookup> LivestockVaccinations => Set<LivestockVaccinationLookup>();
    public DbSet<LivestockProductionLookup> LivestockProductions => Set<LivestockProductionLookup>();

    public DbSet<SheepGoat> SheepGoats => Set<SheepGoat>();
    public DbSet<SheepGoatImage> SheepGoatImages => Set<SheepGoatImage>();
    public DbSet<SheepGoatBreedLookup> SheepGoatBreeds => Set<SheepGoatBreedLookup>();
    public DbSet<SheepGoatPurposeLookup> SheepGoatPurposes => Set<SheepGoatPurposeLookup>();
    public DbSet<SheepGoatAgeLookup> SheepGoatAges => Set<SheepGoatAgeLookup>();
    public DbSet<SheepGoatGenderLookup> SheepGoatGenders => Set<SheepGoatGenderLookup>();
    public DbSet<SheepGoatHealthStatusLookup> SheepGoatHealthStatuses => Set<SheepGoatHealthStatusLookup>();
    public DbSet<SheepGoatVaccinationLookup> SheepGoatVaccinations => Set<SheepGoatVaccinationLookup>();

    public DbSet<Horse> Horses => Set<Horse>();
    public DbSet<HorseImage> HorseImages => Set<HorseImage>();
    public DbSet<HorseBreedLookup> HorseBreeds => Set<HorseBreedLookup>();
    public DbSet<HorsePurposeLookup> HorsePurposes => Set<HorsePurposeLookup>();
    public DbSet<HorseAgeLookup> HorseAges => Set<HorseAgeLookup>();
    public DbSet<HorseGenderLookup> HorseGenders => Set<HorseGenderLookup>();
    public DbSet<HorseHealthStatusLookup> HorseHealthStatuses => Set<HorseHealthStatusLookup>();
    public DbSet<HorseTrainingLevelLookup> HorseTrainingLevels => Set<HorseTrainingLevelLookup>();
    public DbSet<HorseVaccinationLookup> HorseVaccinations => Set<HorseVaccinationLookup>();

    public DbSet<Camel> Camels => Set<Camel>();
    public DbSet<CamelImage> CamelImages => Set<CamelImage>();
    public DbSet<CamelBreedLookup> CamelBreeds => Set<CamelBreedLookup>();
    public DbSet<CamelPurposeLookup> CamelPurposes => Set<CamelPurposeLookup>();
    public DbSet<CamelAgeLookup> CamelAges => Set<CamelAgeLookup>();
    public DbSet<CamelGenderLookup> CamelGenders => Set<CamelGenderLookup>();
    public DbSet<CamelHealthStatusLookup> CamelHealthStatuses => Set<CamelHealthStatusLookup>();
    public DbSet<CamelVaccinationLookup> CamelVaccinations => Set<CamelVaccinationLookup>();

    public DbSet<Bird> Birds => Set<Bird>();
    public DbSet<BirdImage> BirdImages => Set<BirdImage>();
    public DbSet<BirdTypeLookup> BirdTypes => Set<BirdTypeLookup>();
    public DbSet<BirdPurposeLookup> BirdPurposes => Set<BirdPurposeLookup>();
    public DbSet<BirdAgeLookup> BirdAges => Set<BirdAgeLookup>();
    public DbSet<BirdGenderLookup> BirdGenders => Set<BirdGenderLookup>();
    public DbSet<BirdHealthStatusLookup> BirdHealthStatuses => Set<BirdHealthStatusLookup>();
    public DbSet<BirdVaccinationLookup> BirdVaccinations => Set<BirdVaccinationLookup>();

    public DbSet<Pet> Pets => Set<Pet>();
    public DbSet<PetImage> PetImages => Set<PetImage>();
    public DbSet<PetBreedLookup> PetBreeds => Set<PetBreedLookup>();
    public DbSet<PetPurposeLookup> PetPurposes => Set<PetPurposeLookup>();
    public DbSet<PetAgeLookup> PetAges => Set<PetAgeLookup>();
    public DbSet<PetGenderLookup> PetGenders => Set<PetGenderLookup>();
    public DbSet<PetHealthStatusLookup> PetHealthStatuses => Set<PetHealthStatusLookup>();
    public DbSet<PetTrainingLevelLookup> PetTrainingLevels => Set<PetTrainingLevelLookup>();
    public DbSet<PetVaccinationLookup> PetVaccinations => Set<PetVaccinationLookup>();

    public DbSet<Fish> Fishes => Set<Fish>();
    public DbSet<FishImage> FishImages => Set<FishImage>();
    public DbSet<FishTypeLookup> FishTypes => Set<FishTypeLookup>();
    public DbSet<FishPurposeLookup> FishPurposes => Set<FishPurposeLookup>();
    public DbSet<FishAgeLookup> FishAges => Set<FishAgeLookup>();
    public DbSet<FishHealthStatusLookup> FishHealthStatuses => Set<FishHealthStatusLookup>();

    public DbSet<Bee> Bees => Set<Bee>();
    public DbSet<BeeImage> BeeImages => Set<BeeImage>();
    public DbSet<BeeTypeLookup> BeeTypes => Set<BeeTypeLookup>();
    public DbSet<BeePurposeLookup> BeePurposes => Set<BeePurposeLookup>();
    public DbSet<BeeHealthStatusLookup> BeeHealthStatuses => Set<BeeHealthStatusLookup>();
    public DbSet<BeeProductionLookup> BeeProductions => Set<BeeProductionLookup>();

    public DbSet<OtherAnimal> OtherAnimals => Set<OtherAnimal>();
    public DbSet<OtherAnimalImage> OtherAnimalImages => Set<OtherAnimalImage>();
    public DbSet<OtherAnimalTypeLookup> OtherAnimalTypes => Set<OtherAnimalTypeLookup>();
    public DbSet<OtherAnimalPurposeLookup> OtherAnimalPurposes => Set<OtherAnimalPurposeLookup>();
    public DbSet<OtherAnimalAgeLookup> OtherAnimalAges => Set<OtherAnimalAgeLookup>();
    public DbSet<OtherAnimalGenderLookup> OtherAnimalGenders => Set<OtherAnimalGenderLookup>();
    public DbSet<OtherAnimalHealthStatusLookup> OtherAnimalHealthStatuses => Set<OtherAnimalHealthStatusLookup>();
    public DbSet<OtherAnimalVaccinationLookup> OtherAnimalVaccinations => Set<OtherAnimalVaccinationLookup>();

    public DbSet<DecorAntique> DecorAntiques => Set<DecorAntique>();
    public DbSet<DecorAntiqueImage> DecorAntiqueImages => Set<DecorAntiqueImage>();
    public DbSet<DecorAntiqueVideo> DecorAntiqueVideos => Set<DecorAntiqueVideo>();
    public DbSet<DecorAntiqueItemTypeLookup> DecorAntiqueItemTypes => Set<DecorAntiqueItemTypeLookup>();
    public DbSet<DecorAntiqueMaterialLookup> DecorAntiqueMaterials => Set<DecorAntiqueMaterialLookup>();
    public DbSet<DecorAntiqueConditionLookup> DecorAntiqueConditions => Set<DecorAntiqueConditionLookup>();
    public DbSet<DecorAntiqueOriginalityLookup> DecorAntiqueOriginalities => Set<DecorAntiqueOriginalityLookup>();

    public DbSet<Antique> Antiques => Set<Antique>();
    public DbSet<AntiqueImage> AntiqueImages => Set<AntiqueImage>();
    public DbSet<AntiqueVideo> AntiqueVideos => Set<AntiqueVideo>();
    public DbSet<AntiqueTypeLookup> AntiqueTypes => Set<AntiqueTypeLookup>();
    public DbSet<AntiqueMaterialLookup> AntiqueMaterials => Set<AntiqueMaterialLookup>();
    public DbSet<AntiqueConditionLookup> AntiqueConditions => Set<AntiqueConditionLookup>();
    public DbSet<AntiqueWorkingStatusLookup> AntiqueWorkingStatuses => Set<AntiqueWorkingStatusLookup>();
    public DbSet<AntiqueOriginalityLookup> AntiqueOriginalities => Set<AntiqueOriginalityLookup>();

    public DbSet<Painting> Paintings => Set<Painting>();
    public DbSet<PaintingImage> PaintingImages => Set<PaintingImage>();
    public DbSet<PaintingVideo> PaintingVideos => Set<PaintingVideo>();
    public DbSet<PaintingTypeLookup> PaintingTypes => Set<PaintingTypeLookup>();
    public DbSet<PaintingMaterialLookup> PaintingMaterials => Set<PaintingMaterialLookup>();
    public DbSet<PaintingOriginalityLookup> PaintingOriginalities => Set<PaintingOriginalityLookup>();

    public DbSet<Handmade> Handmades => Set<Handmade>();
    public DbSet<HandmadeImage> HandmadeImages => Set<HandmadeImage>();
    public DbSet<HandmadeVideo> HandmadeVideos => Set<HandmadeVideo>();
    public DbSet<HandmadeColorSelection> HandmadeColorSelections => Set<HandmadeColorSelection>();
    public DbSet<HandmadeTypeLookup> HandmadeTypes => Set<HandmadeTypeLookup>();
    public DbSet<HandmadeColorLookup> HandmadeColors => Set<HandmadeColorLookup>();

    public DbSet<CoinStamp> CoinStamps => Set<CoinStamp>();
    public DbSet<CoinStampImage> CoinStampImages => Set<CoinStampImage>();
    public DbSet<CoinStampVideo> CoinStampVideos => Set<CoinStampVideo>();
    public DbSet<CoinStampItemTypeLookup> CoinStampItemTypes => Set<CoinStampItemTypeLookup>();
    public DbSet<CoinStampMetalLookup> CoinStampMetals => Set<CoinStampMetalLookup>();
    public DbSet<CoinStampConditionLookup> CoinStampConditions => Set<CoinStampConditionLookup>();

    public DbSet<MenClothing> MenClothings => Set<MenClothing>();
    public DbSet<MenClothingImage> MenClothingImages => Set<MenClothingImage>();
    public DbSet<MenClothingSizeSelection> MenClothingSizeSelections => Set<MenClothingSizeSelection>();
    public DbSet<MenClothingColorSelection> MenClothingColorSelections => Set<MenClothingColorSelection>();
    public DbSet<MenClothingTypeLookup> MenClothingTypes => Set<MenClothingTypeLookup>();
    public DbSet<MenClothingBrandLookup> MenClothingBrands => Set<MenClothingBrandLookup>();
    public DbSet<MenClothingSizeLookup> MenClothingSizes => Set<MenClothingSizeLookup>();
    public DbSet<MenClothingColorLookup> MenClothingColors => Set<MenClothingColorLookup>();
    public DbSet<MenClothingConditionLookup> MenClothingConditions => Set<MenClothingConditionLookup>();
    public DbSet<MenClothingSellingMethodLookup> MenClothingSellingMethods => Set<MenClothingSellingMethodLookup>();

    public DbSet<WomenClothing> WomenClothings => Set<WomenClothing>();
    public DbSet<WomenClothingImage> WomenClothingImages => Set<WomenClothingImage>();
    public DbSet<WomenClothingSizeSelection> WomenClothingSizeSelections => Set<WomenClothingSizeSelection>();
    public DbSet<WomenClothingColorSelection> WomenClothingColorSelections => Set<WomenClothingColorSelection>();
    public DbSet<WomenClothingTypeLookup> WomenClothingTypes => Set<WomenClothingTypeLookup>();
    public DbSet<WomenClothingBrandLookup> WomenClothingBrands => Set<WomenClothingBrandLookup>();
    public DbSet<WomenClothingSizeLookup> WomenClothingSizes => Set<WomenClothingSizeLookup>();
    public DbSet<WomenClothingColorLookup> WomenClothingColors => Set<WomenClothingColorLookup>();
    public DbSet<WomenClothingConditionLookup> WomenClothingConditions => Set<WomenClothingConditionLookup>();
    public DbSet<WomenClothingSellingMethodLookup> WomenClothingSellingMethods => Set<WomenClothingSellingMethodLookup>();

    public DbSet<KidsClothing> KidsClothings => Set<KidsClothing>();
    public DbSet<KidsClothingImage> KidsClothingImages => Set<KidsClothingImage>();
    public DbSet<KidsClothingSizeSelection> KidsClothingSizeSelections => Set<KidsClothingSizeSelection>();
    public DbSet<KidsClothingColorSelection> KidsClothingColorSelections => Set<KidsClothingColorSelection>();
    public DbSet<KidsClothingTypeLookup> KidsClothingTypes => Set<KidsClothingTypeLookup>();
    public DbSet<KidsClothingBrandLookup> KidsClothingBrands => Set<KidsClothingBrandLookup>();
    public DbSet<KidsClothingSizeLookup> KidsClothingSizes => Set<KidsClothingSizeLookup>();
    public DbSet<KidsClothingColorLookup> KidsClothingColors => Set<KidsClothingColorLookup>();
    public DbSet<KidsClothingConditionLookup> KidsClothingConditions => Set<KidsClothingConditionLookup>();
    public DbSet<KidsClothingSellingMethodLookup> KidsClothingSellingMethods => Set<KidsClothingSellingMethodLookup>();

    public DbSet<Accessory> Accessories => Set<Accessory>();
    public DbSet<AccessoryImage> AccessoryImages => Set<AccessoryImage>();
    public DbSet<AccessoryColorSelection> AccessoryColorSelections => Set<AccessoryColorSelection>();
    public DbSet<AccessoryTypeLookup> AccessoryTypes => Set<AccessoryTypeLookup>();
    public DbSet<AccessoryCategoryLookup> AccessoryCategories => Set<AccessoryCategoryLookup>();
    public DbSet<AccessoryMaterialLookup> AccessoryMaterials => Set<AccessoryMaterialLookup>();
    public DbSet<AccessoryColorLookup> AccessoryColors => Set<AccessoryColorLookup>();

    public DbSet<Cosmetic> Cosmetics => Set<Cosmetic>();
    public DbSet<CosmeticImage> CosmeticImages => Set<CosmeticImage>();
    public DbSet<CosmeticSectionLookup> CosmeticSections => Set<CosmeticSectionLookup>();
    public DbSet<CosmeticSuitableForLookup> CosmeticSuitableFor => Set<CosmeticSuitableForLookup>();

    public DbSet<HomeKitchen> HomeKitchens => Set<HomeKitchen>();
    public DbSet<HomeKitchenImage> HomeKitchenImages => Set<HomeKitchenImage>();
    public DbSet<HomeKitchenColorSelection> HomeKitchenColorSelections => Set<HomeKitchenColorSelection>();
    public DbSet<HomeKitchenSectionLookup> HomeKitchenSections => Set<HomeKitchenSectionLookup>();
    public DbSet<HomeKitchenMaterialLookup> HomeKitchenMaterials => Set<HomeKitchenMaterialLookup>();
    public DbSet<HomeKitchenColorLookup> HomeKitchenColors => Set<HomeKitchenColorLookup>();

    public DbSet<ShoppingElectronic> ShoppingElectronics => Set<ShoppingElectronic>();
    public DbSet<ShoppingElectronicImage> ShoppingElectronicImages => Set<ShoppingElectronicImage>();
    public DbSet<ShoppingElectronicSectionLookup> ShoppingElectronicSections => Set<ShoppingElectronicSectionLookup>();
    public DbSet<ShoppingElectronicCompatibilityLookup> ShoppingElectronicCompatibilities => Set<ShoppingElectronicCompatibilityLookup>();
    public DbSet<ShoppingElectronicConditionLookup> ShoppingElectronicConditions => Set<ShoppingElectronicConditionLookup>();
    public DbSet<ShoppingElectronicWarrantyLookup> ShoppingElectronicWarranties => Set<ShoppingElectronicWarrantyLookup>();

    public DbSet<GiftToy> GiftToys => Set<GiftToy>();
    public DbSet<GiftToyImage> GiftToyImages => Set<GiftToyImage>();
    public DbSet<GiftToyTypeLookup> GiftToyTypes => Set<GiftToyTypeLookup>();
    public DbSet<GiftToySuitableForLookup> GiftToySuitableFor => Set<GiftToySuitableForLookup>();

    public DbSet<HomemadeFood> HomemadeFoods => Set<HomemadeFood>();
    public DbSet<HomemadeFoodImage> HomemadeFoodImages => Set<HomemadeFoodImage>();
    public DbSet<HomemadeFoodDeliveryAreaSelection> HomemadeFoodDeliveryAreaSelections => Set<HomemadeFoodDeliveryAreaSelection>();
    public DbSet<HomemadeFoodSectionLookup> HomemadeFoodSections => Set<HomemadeFoodSectionLookup>();
    public DbSet<HomemadeFoodDeliveryAreaLookup> HomemadeFoodDeliveryAreas => Set<HomemadeFoodDeliveryAreaLookup>();

    public DbSet<Furniture> Furnitures => Set<Furniture>();
    public DbSet<FurnitureImage> FurnitureImages => Set<FurnitureImage>();
    public DbSet<FurnitureColorSelection> FurnitureColorSelections => Set<FurnitureColorSelection>();
    public DbSet<FurnitureTypeLookup> FurnitureTypes => Set<FurnitureTypeLookup>();
    public DbSet<FurnitureMaterialLookup> FurnitureMaterials => Set<FurnitureMaterialLookup>();
    public DbSet<FurnitureColorLookup> FurnitureColors => Set<FurnitureColorLookup>();
    public DbSet<FurnitureConditionLookup> FurnitureConditions => Set<FurnitureConditionLookup>();

    public DbSet<FurnishingCurtain> FurnishingCurtains => Set<FurnishingCurtain>();
    public DbSet<FurnishingCurtainImage> FurnishingCurtainImages => Set<FurnishingCurtainImage>();
    public DbSet<FurnishingCurtainColorSelection> FurnishingCurtainColorSelections => Set<FurnishingCurtainColorSelection>();
    public DbSet<FurnishingCurtainProductTypeLookup> FurnishingCurtainProductTypes => Set<FurnishingCurtainProductTypeLookup>();
    public DbSet<FurnishingCurtainSizeLookup> FurnishingCurtainSizes => Set<FurnishingCurtainSizeLookup>();
    public DbSet<FurnishingCurtainMaterialLookup> FurnishingCurtainMaterials => Set<FurnishingCurtainMaterialLookup>();
    public DbSet<FurnishingCurtainColorLookup> FurnishingCurtainColors => Set<FurnishingCurtainColorLookup>();

    public DbSet<LightingDecor> LightingDecors => Set<LightingDecor>();
    public DbSet<LightingDecorImage> LightingDecorImages => Set<LightingDecorImage>();
    public DbSet<LightingDecorColorSelection> LightingDecorColorSelections => Set<LightingDecorColorSelection>();
    public DbSet<LightingDecorProductTypeLookup> LightingDecorProductTypes => Set<LightingDecorProductTypeLookup>();
    public DbSet<LightingDecorMaterialLookup> LightingDecorMaterials => Set<LightingDecorMaterialLookup>();
    public DbSet<LightingDecorColorLookup> LightingDecorColors => Set<LightingDecorColorLookup>();
    public DbSet<LightingDecorLightTypeLookup> LightingDecorLightTypes => Set<LightingDecorLightTypeLookup>();

    public DbSet<KitchenTool> KitchenTools => Set<KitchenTool>();
    public DbSet<KitchenToolImage> KitchenToolImages => Set<KitchenToolImage>();
    public DbSet<KitchenToolColorSelection> KitchenToolColorSelections => Set<KitchenToolColorSelection>();
    public DbSet<KitchenToolProductTypeLookup> KitchenToolProductTypes => Set<KitchenToolProductTypeLookup>();
    public DbSet<KitchenToolMaterialLookup> KitchenToolMaterials => Set<KitchenToolMaterialLookup>();
    public DbSet<KitchenToolColorLookup> KitchenToolColors => Set<KitchenToolColorLookup>();

    public DbSet<HomeAppliance> HomeAppliances => Set<HomeAppliance>();
    public DbSet<HomeApplianceImage> HomeApplianceImages => Set<HomeApplianceImage>();
    public DbSet<HomeApplianceColorSelection> HomeApplianceColorSelections => Set<HomeApplianceColorSelection>();
    public DbSet<HomeApplianceDeviceTypeLookup> HomeApplianceDeviceTypes => Set<HomeApplianceDeviceTypeLookup>();
    public DbSet<HomeApplianceBrandLookup> HomeApplianceBrands => Set<HomeApplianceBrandLookup>();
    public DbSet<HomeApplianceConditionLookup> HomeApplianceConditions => Set<HomeApplianceConditionLookup>();
    public DbSet<HomeApplianceWarrantyLookup> HomeApplianceWarranties => Set<HomeApplianceWarrantyLookup>();
    public DbSet<HomeApplianceColorLookup> HomeApplianceColors => Set<HomeApplianceColorLookup>();

    public DbSet<BathroomSupply> BathroomSupplies => Set<BathroomSupply>();
    public DbSet<BathroomSupplyImage> BathroomSupplyImages => Set<BathroomSupplyImage>();
    public DbSet<BathroomSupplyColorSelection> BathroomSupplyColorSelections => Set<BathroomSupplyColorSelection>();
    public DbSet<BathroomSupplyProductTypeLookup> BathroomSupplyProductTypes => Set<BathroomSupplyProductTypeLookup>();
    public DbSet<BathroomSupplyMaterialLookup> BathroomSupplyMaterials => Set<BathroomSupplyMaterialLookup>();
    public DbSet<BathroomSupplyColorLookup> BathroomSupplyColors => Set<BathroomSupplyColorLookup>();

    public DbSet<PlantOrnament> PlantOrnaments => Set<PlantOrnament>();
    public DbSet<PlantOrnamentImage> PlantOrnamentImages => Set<PlantOrnamentImage>();
    public DbSet<PlantOrnamentProductTypeLookup> PlantOrnamentProductTypes => Set<PlantOrnamentProductTypeLookup>();
    public DbSet<PlantOrnamentSuitableForLookup> PlantOrnamentSuitableFors => Set<PlantOrnamentSuitableForLookup>();

    public DbSet<RealEstateListingTypeLookup> RealEstateListingTypes => Set<RealEstateListingTypeLookup>();
    public DbSet<RealEstateProjectLookup> RealEstateProjects => Set<RealEstateProjectLookup>();

    public DbSet<Land> Lands => Set<Land>();
    public DbSet<LandImage> LandImages => Set<LandImage>();
    public DbSet<LandUtilitySelection> LandUtilitySelections => Set<LandUtilitySelection>();
    public DbSet<LandRentInclusionSelection> LandRentInclusionSelections => Set<LandRentInclusionSelection>();
    public DbSet<LandTypeLookup> LandTypes => Set<LandTypeLookup>();
    public DbSet<LandAreaUnitLookup> LandAreaUnits => Set<LandAreaUnitLookup>();
    public DbSet<LandFacadesCountLookup> LandFacadesCounts => Set<LandFacadesCountLookup>();
    public DbSet<LandDirectionLookup> LandDirections => Set<LandDirectionLookup>();
    public DbSet<LandRoadTypeLookup> LandRoadTypes => Set<LandRoadTypeLookup>();
    public DbSet<LandLegalStatusLookup> LandLegalStatuses => Set<LandLegalStatusLookup>();
    public DbSet<LandReconciliationFormLookup> LandReconciliationForms => Set<LandReconciliationFormLookup>();
    public DbSet<LandOwnershipDocumentLookup> LandOwnershipDocuments => Set<LandOwnershipDocumentLookup>();
    public DbSet<LandUtilityLookup> LandUtilities => Set<LandUtilityLookup>();
    public DbSet<LandRentTypeLookup> LandRentTypes => Set<LandRentTypeLookup>();
    public DbSet<LandMinimumRentPeriodLookup> LandMinimumRentPeriods => Set<LandMinimumRentPeriodLookup>();
    public DbSet<LandRentInclusionLookup> LandRentInclusions => Set<LandRentInclusionLookup>();
    public DbSet<LandContractDurationLookup> LandContractDurations => Set<LandContractDurationLookup>();
    public DbSet<LandExchangeWithLookup> LandExchangeTargets => Set<LandExchangeWithLookup>();
    public DbSet<LandHarvestSeasonLookup> LandHarvestSeasons => Set<LandHarvestSeasonLookup>();
    public DbSet<LandSoilTypeLookup> LandSoilTypes => Set<LandSoilTypeLookup>();
    public DbSet<LandIrrigationSourceLookup> LandIrrigationSources => Set<LandIrrigationSourceLookup>();
    public DbSet<LandQualityCertificateLookup> LandQualityCertificates => Set<LandQualityCertificateLookup>();
    public DbSet<LandExistingBuildingTypeLookup> LandExistingBuildingTypes => Set<LandExistingBuildingTypeLookup>();
    public DbSet<LandBuildingCompletionRatioLookup> LandBuildingCompletionRatios =>
        Set<LandBuildingCompletionRatioLookup>();

    public DbSet<Apartment> Apartments => Set<Apartment>();
    public DbSet<ApartmentImage> ApartmentImages => Set<ApartmentImage>();
    public DbSet<ApartmentFeatureSelection> ApartmentFeatureSelections => Set<ApartmentFeatureSelection>();
    public DbSet<ApartmentRentInclusionSelection> ApartmentRentInclusionSelections =>
        Set<ApartmentRentInclusionSelection>();
    public DbSet<ApartmentTypeLookup> ApartmentTypes => Set<ApartmentTypeLookup>();
    public DbSet<ApartmentOwnershipTypeLookup> ApartmentOwnershipTypes => Set<ApartmentOwnershipTypeLookup>();
    public DbSet<ApartmentReceptionPiecesLookup> ApartmentReceptionPieces => Set<ApartmentReceptionPiecesLookup>();
    public DbSet<ApartmentFloorTypeLookup> ApartmentFloorTypes => Set<ApartmentFloorTypeLookup>();
    public DbSet<ApartmentFurnishedStatusLookup> ApartmentFurnishedStatuses => Set<ApartmentFurnishedStatusLookup>();
    public DbSet<ApartmentFinishingTypeLookup> ApartmentFinishingTypes => Set<ApartmentFinishingTypeLookup>();
    public DbSet<ApartmentPropertyAgeLookup> ApartmentPropertyAges => Set<ApartmentPropertyAgeLookup>();
    public DbSet<ApartmentDirectionLookup> ApartmentDirections => Set<ApartmentDirectionLookup>();
    public DbSet<ApartmentViewTypeLookup> ApartmentViewTypes => Set<ApartmentViewTypeLookup>();
    public DbSet<ApartmentLegalStatusLookup> ApartmentLegalStatuses => Set<ApartmentLegalStatusLookup>();
    public DbSet<ApartmentReconciliationFormLookup> ApartmentReconciliationForms =>
        Set<ApartmentReconciliationFormLookup>();
    public DbSet<ApartmentOwnershipDocumentLookup> ApartmentOwnershipDocuments =>
        Set<ApartmentOwnershipDocumentLookup>();
    public DbSet<ApartmentFeatureLookup> ApartmentFeatures => Set<ApartmentFeatureLookup>();
    public DbSet<ApartmentPaymentMethodLookup> ApartmentPaymentMethods => Set<ApartmentPaymentMethodLookup>();
    public DbSet<ApartmentInstallmentProviderLookup> ApartmentInstallmentProviders =>
        Set<ApartmentInstallmentProviderLookup>();
    public DbSet<ApartmentRentTypeLookup> ApartmentRentTypes => Set<ApartmentRentTypeLookup>();
    public DbSet<ApartmentRentInclusionLookup> ApartmentRentInclusions => Set<ApartmentRentInclusionLookup>();
    public DbSet<ApartmentSuitableForLookup> ApartmentSuitableFors => Set<ApartmentSuitableForLookup>();
    public DbSet<ApartmentExchangeWithLookup> ApartmentExchangeTargets => Set<ApartmentExchangeWithLookup>();

    public DbSet<Shop> Shops => Set<Shop>();
    public DbSet<ShopImage> ShopImages => Set<ShopImage>();
    public DbSet<ShopUtilitySelection> ShopUtilitySelections => Set<ShopUtilitySelection>();
    public DbSet<ShopRentInclusionSelection> ShopRentInclusionSelections => Set<ShopRentInclusionSelection>();
    public DbSet<ShopRentSuitableActivitySelection> ShopRentSuitableActivitySelections =>
        Set<ShopRentSuitableActivitySelection>();
    public DbSet<ShopSuitableActivityLookup> ShopSuitableActivities => Set<ShopSuitableActivityLookup>();
    public DbSet<ShopFloorTypeLookup> ShopFloorTypes => Set<ShopFloorTypeLookup>();
    public DbSet<ShopFacadesCountLookup> ShopFacadesCounts => Set<ShopFacadesCountLookup>();
    public DbSet<ShopFacadeDirectionLookup> ShopFacadeDirections => Set<ShopFacadeDirectionLookup>();
    public DbSet<ShopFinishingTypeLookup> ShopFinishingTypes => Set<ShopFinishingTypeLookup>();
    public DbSet<ShopPropertyAgeLookup> ShopPropertyAges => Set<ShopPropertyAgeLookup>();
    public DbSet<ShopEntrancesCountLookup> ShopEntrancesCounts => Set<ShopEntrancesCountLookup>();
    public DbSet<ShopLegalStatusLookup> ShopLegalStatuses => Set<ShopLegalStatusLookup>();
    public DbSet<ShopLicenseTypeLookup> ShopLicenseTypes => Set<ShopLicenseTypeLookup>();
    public DbSet<ShopReconciliationFormLookup> ShopReconciliationForms => Set<ShopReconciliationFormLookup>();
    public DbSet<ShopOwnershipDocumentLookup> ShopOwnershipDocuments => Set<ShopOwnershipDocumentLookup>();
    public DbSet<ShopUtilityLookup> ShopUtilities => Set<ShopUtilityLookup>();
    public DbSet<ShopPaymentMethodLookup> ShopPaymentMethods => Set<ShopPaymentMethodLookup>();
    public DbSet<ShopInstallmentProviderLookup> ShopInstallmentProviders => Set<ShopInstallmentProviderLookup>();
    public DbSet<ShopRentTypeLookup> ShopRentTypes => Set<ShopRentTypeLookup>();
    public DbSet<ShopRentInclusionLookup> ShopRentInclusions => Set<ShopRentInclusionLookup>();
    public DbSet<ShopRentSuitableActivityLookup> ShopRentSuitableActivityOptions =>
        Set<ShopRentSuitableActivityLookup>();
    public DbSet<ShopExchangeWithLookup> ShopExchangeTargets => Set<ShopExchangeWithLookup>();

    public DbSet<Rescue> Rescues => Set<Rescue>();
    public DbSet<RescueImage> RescueImages => Set<RescueImage>();

    public DbSet<BloodRequest> BloodRequests => Set<BloodRequest>();
    public DbSet<BloodRequestImage> BloodRequestImages => Set<BloodRequestImage>();

    public DbSet<AskConsult> AskConsults => Set<AskConsult>();
    public DbSet<AskConsultImage> AskConsultImages => Set<AskConsultImage>();
    public DbSet<AskConsultLike> AskConsultLikes => Set<AskConsultLike>();
    public DbSet<AskConsultComment> AskConsultComments => Set<AskConsultComment>();

    public DbSet<Referral> Referrals => Set<Referral>();

    public DbSet<ReferralLinkEvent> ReferralLinkEvents => Set<ReferralLinkEvent>();

    public DbSet<Governorate> Governorates => Set<Governorate>();
    public DbSet<Center> Centers => Set<Center>();
    public DbSet<ListingTypeLookup> ListingTypes => Set<ListingTypeLookup>();
    public DbSet<WorkshopTypeLookup> WorkshopTypes => Set<WorkshopTypeLookup>();
    public DbSet<CraftsmanSpecializationLookup> CraftsmanSpecializations => Set<CraftsmanSpecializationLookup>();
    public DbSet<ExperienceLevelLookup> ExperienceLevels => Set<ExperienceLevelLookup>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        builder.ApplyModerationConfiguration();
    }
}
