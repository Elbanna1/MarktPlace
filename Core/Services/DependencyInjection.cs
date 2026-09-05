using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Services.AdForms;
using Services.Admin;
using Services.Animals;
using Services.BannerBookings;
using Services.Antiques;
using Services.Clothing;
using Services.HomeFurnishing;
using Services.Listings;
using Services.Lookups;
using Services.Mapping;
using Services.Notifications;
using Services.Referrals;
using Services.OnlineShopping;
using Services.RealEstate;
using Services.ReadConfigs;
using Services.Settings;
using Services.Validation;
using ServicesAbstraction;

namespace Services;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<IAdvertisementService, AdvertisementService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<ILookupService, LookupService>();

        services.AddScoped<INotificationInterestService, NotificationInterestService>();
        services.AddScoped<IListingInterestNotifier, ListingInterestNotifier>();

        services.AddScoped<IReferralService, ReferralService>();

        services.AddSingleton<ILookupCache, LookupCache>();

        services.AddScoped<IHomeService, HomeService>();

        services.AddScoped<IListingInteractionService, ListingInteractionService>();

        services.AddScoped<IListingEditReviewQueue, ListingEditReviewQueue>();
        services.AddScoped<IListingEditReviewNotifier, ListingEditReviewNotifier>();

        services.AddScoped<IListingRepublishService, ListingRepublishService>();

        services.AddScoped<IAdminAdService, AdminAdService>();

        services.AddScoped<IAdminDashboardService, AdminDashboardService>();
        services.AddScoped<IAdminUserService, AdminUserService>();

        services.AddScoped<IAdminAccountService, AdminAccountService>();
        services.AddScoped<IAdminPermissionService, AdminPermissionService>();

        services.AddScoped<IAdminReferralService, AdminReferralService>();
        services.AddScoped<IAdminCatalogService, AdminCatalogService>();
        services.AddScoped<IAdminLocationService, AdminLocationService>();
        services.AddScoped<IAdminHomeService, AdminHomeService>();
        services.AddScoped<IAdminSettingsService, AdminSettingsService>();

        services.AddScoped<IPublicSettingsService, PublicSettingsService>();
        services.AddScoped<IAdminFormService, AdminFormService>();

        services.AddScoped<IAdminAuditService, AdminAuditService>();

        services.AddScoped<IAdminAlertService, AdminAlertService>();

        services.AddScoped<CategorySelectionResolver>();
        services.AddScoped<ICreateAdFormService, CreateAdFormService>();
        services.AddScoped<IReadConfigService, ReadConfigService>();
        services.AddScoped<ILostFoundService, LostFoundService>();
        services.AddScoped<IWorkshopService, WorkshopService>();
        services.AddScoped<ICraftsmanService, CraftsmanService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IBannerService, BannerService>();

        services.AddScoped<IBannerBookingService, BannerBookingService>();
        services.AddScoped<IBannerSettingsService, BannerSettingsService>();

        services.AddScoped<IFeedbackService, FeedbackService>();

        services.AddScoped<IFactoryService, FactoryService>();
        services.AddScoped<IFarmService, FarmService>();
        services.AddScoped<ICompanyService, CompanyService>();

        services.AddScoped<ISupplierService, SupplierService>();
        services.AddScoped<IWholesaleTraderService, WholesaleTraderService>();
        services.AddScoped<IFruitVegetableMerchantService, FruitVegetableMerchantService>();

        services.AddScoped<IJobRequestService, JobRequestService>();
        services.AddScoped<IJobOpportunityService, JobOpportunityService>();

        services.AddScoped<ILivestockService, LivestockService>();
        services.AddScoped<ISheepGoatService, SheepGoatService>();
        services.AddScoped<IHorseService, HorseService>();
        services.AddScoped<ICamelService, CamelService>();
        services.AddScoped<IBirdService, BirdService>();
        services.AddScoped<IPetService, PetService>();
        services.AddScoped<IFishService, FishService>();
        services.AddScoped<IBeeService, BeeService>();
        services.AddScoped<IOtherAnimalService, OtherAnimalService>();

        services.AddScoped<IDecorAntiqueService, DecorAntiqueService>();
        services.AddScoped<IAntiqueService, AntiqueService>();
        services.AddScoped<IPaintingService, PaintingService>();
        services.AddScoped<IHandmadeService, HandmadeService>();
        services.AddScoped<ICoinStampService, CoinStampService>();

        services.AddScoped<IMenClothingService, MenClothingService>();
        services.AddScoped<IWomenClothingService, WomenClothingService>();
        services.AddScoped<IKidsClothingService, KidsClothingService>();

        services.AddScoped<IAccessoryService, AccessoryService>();
        services.AddScoped<ICosmeticService, CosmeticService>();
        services.AddScoped<IHomeKitchenService, HomeKitchenService>();
        services.AddScoped<IShoppingElectronicService, ShoppingElectronicService>();
        services.AddScoped<IGiftToyService, GiftToyService>();
        services.AddScoped<IHomemadeFoodService, HomemadeFoodService>();

        services.AddScoped<IFurnitureService, FurnitureService>();
        services.AddScoped<IFurnishingCurtainService, FurnishingCurtainService>();
        services.AddScoped<ILightingDecorService, LightingDecorService>();
        services.AddScoped<IKitchenToolService, KitchenToolService>();
        services.AddScoped<IHomeApplianceService, HomeApplianceService>();
        services.AddScoped<IBathroomSupplyService, BathroomSupplyService>();
        services.AddScoped<IPlantOrnamentService, PlantOrnamentService>();

        services.AddScoped<ILandService, LandService>();
        services.AddScoped<IApartmentService, ApartmentService>();
        services.AddScoped<IShopService, ShopService>();

        services.AddScoped<IRescueService, Services.Charity.RescueService>();
        services.AddScoped<IBloodRequestService, Services.Charity.BloodRequestService>();
        services.AddScoped<IAskConsultService, Services.Charity.AskConsultService>();

        var mapperConfiguration = new MapperConfiguration(
            cfg =>
            {
                cfg.AddProfile<MappingProfile>();
                cfg.AddProfile<AdvertisementMappingProfile>();
                cfg.AddProfile<LookupMappingProfile>();
                cfg.AddProfile<LostFoundMappingProfile>();
                cfg.AddProfile<WorkshopMappingProfile>();
                cfg.AddProfile<CraftsmanMappingProfile>();
                cfg.AddProfile<PaymentMappingProfile>();
                cfg.AddProfile<BannerMappingProfile>();
                cfg.AddProfile<BannerBookingMappingProfile>();
                cfg.AddProfile<FeedbackMappingProfile>();
                cfg.AddProfile<FactoryMappingProfile>();
                cfg.AddProfile<FarmMappingProfile>();
                cfg.AddProfile<CompanyMappingProfile>();
                cfg.AddProfile<SupplierMappingProfile>();
                cfg.AddProfile<WholesaleTraderMappingProfile>();
                cfg.AddProfile<FruitVegetableMerchantMappingProfile>();
                cfg.AddProfile<JobRequestMappingProfile>();
                cfg.AddProfile<JobOpportunityMappingProfile>();

                cfg.AddProfile<LivestockMappingProfile>();
                cfg.AddProfile<SheepGoatMappingProfile>();
                cfg.AddProfile<HorseMappingProfile>();
                cfg.AddProfile<CamelMappingProfile>();
                cfg.AddProfile<BirdMappingProfile>();
                cfg.AddProfile<PetMappingProfile>();
                cfg.AddProfile<FishMappingProfile>();
                cfg.AddProfile<BeeMappingProfile>();
                cfg.AddProfile<OtherAnimalMappingProfile>();

                cfg.AddProfile<DecorAntiqueMappingProfile>();
                cfg.AddProfile<AntiqueMappingProfile>();
                cfg.AddProfile<PaintingMappingProfile>();
                cfg.AddProfile<HandmadeMappingProfile>();
                cfg.AddProfile<CoinStampMappingProfile>();

                cfg.AddProfile<MenClothingMappingProfile>();
                cfg.AddProfile<WomenClothingMappingProfile>();
                cfg.AddProfile<KidsClothingMappingProfile>();

                cfg.AddProfile<AccessoryMappingProfile>();
                cfg.AddProfile<CosmeticMappingProfile>();
                cfg.AddProfile<HomeKitchenMappingProfile>();
                cfg.AddProfile<ShoppingElectronicMappingProfile>();
                cfg.AddProfile<GiftToyMappingProfile>();
                cfg.AddProfile<HomemadeFoodMappingProfile>();

                cfg.AddProfile<FurnitureMappingProfile>();
                cfg.AddProfile<FurnishingCurtainMappingProfile>();
                cfg.AddProfile<LightingDecorMappingProfile>();
                cfg.AddProfile<KitchenToolMappingProfile>();
                cfg.AddProfile<HomeApplianceMappingProfile>();
                cfg.AddProfile<BathroomSupplyMappingProfile>();
                cfg.AddProfile<PlantOrnamentMappingProfile>();

                cfg.AddProfile<LandMappingProfile>();
                cfg.AddProfile<Services.Mapping.ApartmentMappingProfile>();
                cfg.AddProfile<ShopMappingProfile>();

                cfg.AddProfile<CharityMappingProfile>();
            },
            NullLoggerFactory.Instance);

        services.AddSingleton(mapperConfiguration.CreateMapper());

        services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>(ServiceLifetime.Scoped);

        ValidatorOptions.Global.LanguageManager = new EgyptianArabicLanguageManager();

        return services;
    }
}
