using Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.BackgroundServices;
using Persistence.Data;
using Persistence.Listings;
using Persistence.Repositories;
using Persistence.Interceptors;
using Persistence.Services;
using ServicesAbstraction;
using Shared.Settings;

namespace Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        services.Configure<EmailSettings>(configuration.GetSection(EmailSettings.SectionName));

        services.Configure<AppSettings>(configuration.GetSection(AppSettings.SectionName));

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException(
                "Connection string 'DefaultConnection' is not configured. In production provide it via the " +
                "environment variable 'ConnectionStrings__DefaultConnection'.");

        var sqlCounterEnabled = configuration.GetValue<bool>("Diagnostics:SqlCounter");

        if (sqlCounterEnabled)
        {
            services.AddScoped<RequestSqlCounter>();
            services.AddScoped<SqlCountInterceptor>();
        }

        services.AddScoped<ListingEditModerationInterceptor>();

        services.AddDbContext<AppDbContext>((provider, options) =>
        {
            options.UseSqlServer(connectionString, sql =>
                sql.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName));

            options.AddInterceptors(provider.GetRequiredService<ListingEditModerationInterceptor>());

            if (sqlCounterEnabled)
                options.AddInterceptors(provider.GetRequiredService<SqlCountInterceptor>());
        });

        services.AddIdentityCore<ApplicationUser>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;
            options.Password.RequiredUniqueChars = 1;

            options.User.RequireUniqueEmail = true;

            options.User.AllowedUserNameCharacters = string.Empty;

            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);

            options.SignIn.RequireConfirmedEmail = false;
        })
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

        services.AddSingleton<ITokenService, JwtService>();
        services.AddTransient<IEmailService, EmailService>();
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddHttpContextAccessor();
        services.AddScoped<IAdvertisementRepository, AdvertisementRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();

        services.AddScoped<INotificationInterestRepository, NotificationInterestRepository>();

        services.AddScoped<IReferralRepository, ReferralRepository>();
        services.AddScoped<IReferralLinkBuilder, ReferralLinkBuilder>();

        services.AddScoped<IUnitOfWork, AppUnitOfWork>();
        services.AddScoped<ILostFoundRepository, LostFoundRepository>();
        services.AddScoped<IWorkshopRepository, WorkshopRepository>();
        services.AddScoped<ICraftsmanRepository, CraftsmanRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();

        services.AddScoped<IFeedbackRepository, FeedbackRepository>();

        services.AddScoped<IBannerRepository, BannerRepository>();

        services.AddScoped<IBannerBookingRepository, BannerBookingRepository>();

        services.AddScoped<ISupplierRepository, SupplierRepository>();
        services.AddScoped<IWholesaleTraderRepository, WholesaleTraderRepository>();
        services.AddScoped<IFruitVegetableMerchantRepository, FruitVegetableMerchantRepository>();

        services.AddScoped<IJobRequestRepository, JobRequestRepository>();
        services.AddScoped<IJobOpportunityRepository, JobOpportunityRepository>();

        services.AddScoped<ILivestockRepository, LivestockRepository>();
        services.AddScoped<ISheepGoatRepository, SheepGoatRepository>();
        services.AddScoped<IHorseRepository, HorseRepository>();
        services.AddScoped<ICamelRepository, CamelRepository>();
        services.AddScoped<IBirdRepository, BirdRepository>();
        services.AddScoped<IPetRepository, PetRepository>();
        services.AddScoped<IFishRepository, FishRepository>();
        services.AddScoped<IBeeRepository, BeeRepository>();
        services.AddScoped<IOtherAnimalRepository, OtherAnimalRepository>();

        services.AddScoped<IDecorAntiqueRepository, DecorAntiqueRepository>();
        services.AddScoped<IAntiqueRepository, AntiqueRepository>();
        services.AddScoped<IPaintingRepository, PaintingRepository>();
        services.AddScoped<IHandmadeRepository, HandmadeRepository>();
        services.AddScoped<ICoinStampRepository, CoinStampRepository>();

        services.AddScoped<IMenClothingRepository, MenClothingRepository>();
        services.AddScoped<IWomenClothingRepository, WomenClothingRepository>();
        services.AddScoped<IKidsClothingRepository, KidsClothingRepository>();

        services.AddScoped<IAccessoryRepository, AccessoryRepository>();
        services.AddScoped<ICosmeticRepository, CosmeticRepository>();
        services.AddScoped<IHomeKitchenRepository, HomeKitchenRepository>();
        services.AddScoped<IShoppingElectronicRepository, ShoppingElectronicRepository>();
        services.AddScoped<IGiftToyRepository, GiftToyRepository>();
        services.AddScoped<IHomemadeFoodRepository, HomemadeFoodRepository>();

        services.AddScoped<IFurnitureRepository, FurnitureRepository>();
        services.AddScoped<IFurnishingCurtainRepository, FurnishingCurtainRepository>();
        services.AddScoped<ILightingDecorRepository, LightingDecorRepository>();
        services.AddScoped<IKitchenToolRepository, KitchenToolRepository>();
        services.AddScoped<IHomeApplianceRepository, HomeApplianceRepository>();
        services.AddScoped<IBathroomSupplyRepository, BathroomSupplyRepository>();
        services.AddScoped<IPlantOrnamentRepository, PlantOrnamentRepository>();

        services.AddScoped<ILandRepository, LandRepository>();
        services.AddScoped<IApartmentRepository, ApartmentRepository>();
        services.AddScoped<IShopRepository, ShopRepository>();

        services.AddScoped<IRescueRepository, RescueRepository>();
        services.AddScoped<IBloodRequestRepository, BloodRequestRepository>();
        services.AddScoped<IAskConsultRepository, AskConsultRepository>();

        services.AddScoped<IFactoryRepository, FactoryRepository>();
        services.AddScoped<IFarmRepository, FarmRepository>();
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<IFileService, FileService>();

        services.AddScoped<IUserListingSource, AdvertisementListingSource>();
        services.AddScoped<IUserListingSource, CraftsmanListingSource>();
        services.AddScoped<IUserListingSource, WorkshopListingSource>();
        services.AddScoped<IUserListingSource, FactoryListingSource>();
        services.AddScoped<IUserListingSource, FarmListingSource>();
        services.AddScoped<IUserListingSource, CompanyListingSource>();
        services.AddScoped<IUserListingSource, SupplierListingSource>();
        services.AddScoped<IUserListingSource, WholesaleTraderListingSource>();
        services.AddScoped<IUserListingSource, FruitVegetableMerchantListingSource>();
        services.AddScoped<IUserListingSource, JobRequestListingSource>();
        services.AddScoped<IUserListingSource, JobOpportunityListingSource>();

        services.AddScoped<IUserListingSource, LostItemListingSource>();
        services.AddScoped<IUserListingSource, FoundItemListingSource>();

        services.AddScoped<IUserListingSource, LivestockListingSource>();
        services.AddScoped<IUserListingSource, SheepGoatListingSource>();
        services.AddScoped<IUserListingSource, HorseListingSource>();
        services.AddScoped<IUserListingSource, CamelListingSource>();
        services.AddScoped<IUserListingSource, BirdListingSource>();
        services.AddScoped<IUserListingSource, PetListingSource>();
        services.AddScoped<IUserListingSource, FishListingSource>();
        services.AddScoped<IUserListingSource, BeeListingSource>();
        services.AddScoped<IUserListingSource, OtherAnimalListingSource>();

        services.AddScoped<IUserListingSource, DecorAntiqueListingSource>();
        services.AddScoped<IUserListingSource, AntiqueListingSource>();
        services.AddScoped<IUserListingSource, PaintingListingSource>();
        services.AddScoped<IUserListingSource, HandmadeListingSource>();
        services.AddScoped<IUserListingSource, CoinStampListingSource>();

        services.AddScoped<IUserListingSource, MenClothingListingSource>();
        services.AddScoped<IUserListingSource, WomenClothingListingSource>();
        services.AddScoped<IUserListingSource, KidsClothingListingSource>();

        services.AddScoped<IUserListingSource, AccessoryListingSource>();
        services.AddScoped<IUserListingSource, CosmeticListingSource>();
        services.AddScoped<IUserListingSource, HomeKitchenListingSource>();
        services.AddScoped<IUserListingSource, ShoppingElectronicListingSource>();
        services.AddScoped<IUserListingSource, GiftToyListingSource>();
        services.AddScoped<IUserListingSource, HomemadeFoodListingSource>();

        services.AddScoped<IUserListingSource, FurnitureListingSource>();
        services.AddScoped<IUserListingSource, FurnishingCurtainListingSource>();
        services.AddScoped<IUserListingSource, LightingDecorListingSource>();
        services.AddScoped<IUserListingSource, KitchenToolListingSource>();
        services.AddScoped<IUserListingSource, HomeApplianceListingSource>();
        services.AddScoped<IUserListingSource, BathroomSupplyListingSource>();
        services.AddScoped<IUserListingSource, PlantOrnamentListingSource>();

        services.AddScoped<IUserListingSource, LandListingSource>();
        services.AddScoped<IUserListingSource, ApartmentListingSource>();
        services.AddScoped<IUserListingSource, ShopListingSource>();

        services.AddScoped<IUserListingSource, RescueListingSource>();
        services.AddScoped<IUserListingSource, BloodRequestListingSource>();
        services.AddScoped<IUserListingSource, AskConsultListingSource>();

        services.AddScoped<IUserListingRepository, UserListingRepository>();

        foreach (var (module, entityType) in ModerationModuleRegistry.EntityTypes)
        {
            var implementationType = typeof(ListingModerationSource<>).MakeGenericType(entityType);
            var moduleType = module;

            services.AddScoped(typeof(IListingModerationSource), provider =>
                Activator.CreateInstance(
                    implementationType,
                    provider.GetRequiredService<AppDbContext>(),
                    moduleType)!);
        }

        services.AddScoped<IListingLifecycleRepository, ListingLifecycleRepository>();

        services.AddScoped<IAdminAdRepository, AdminAdRepository>();

        services.AddScoped<IAdminDashboardRepository, AdminDashboardRepository>();
        services.AddScoped<IAdminUserRepository, AdminUserRepository>();

        services.AddScoped<IAdminAccountRepository, AdminAccountRepository>();
        services.AddScoped<IAdminCatalogRepository, AdminCatalogRepository>();
        services.AddScoped<IAdminLocationRepository, AdminLocationRepository>();
        services.AddScoped<IAdminHomeRepository, AdminHomeRepository>();
        services.AddScoped<IAdminSettingsRepository, AdminSettingsRepository>();
        services.AddScoped<IAdminFormRepository, AdminFormRepository>();
        services.AddScoped<IAdminAuditRepository, AdminAuditRepository>();

        services.AddScoped<IAdminActionContext, AdminActionContext>();

        services.AddSingleton<IUserRoleCache, UserRoleCache>();
        services.AddScoped<IClaimsTransformation, DatabaseRoleClaimsTransformation>();

        services.AddScoped<IListingInteractionRepository, ListingInteractionRepository>();

        services.AddScoped<ILookupRepository, LookupRepository>();

        services.AddHostedService<ListingLifecycleService>();

        services.AddHostedService<BannerBookingLifecycleService>();

        return services;
    }
}
