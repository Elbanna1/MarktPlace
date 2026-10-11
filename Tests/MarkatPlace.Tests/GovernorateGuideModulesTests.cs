using System.Reflection;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Persistence.Data;
using Persistence.Interceptors;
using Persistence.Listings;
using Persistence.Repositories;
using Presentation.Controllers;
using Services;
using Services.AdForms;
using Services.Admin;
using Services.GovernorateGuide;
using Services.Lookups;
using Services.Mapping;
using Services.ReadConfigs;
using Services.Validation;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Admin;
using Shared.DTOs.Advertisements;
using Shared.DTOs.GovernorateGuide;
using Shared.DTOs.Lookups;
using Shared.DTOs.Lookups.Forms;
using Shared.DTOs.Lookups.Read;
using Shared.DTOs.Realtime;
using Shared.Enums;
using Shared.Exceptions;
using Shared.Settings;
using Xunit;

namespace MarkatPlace.Tests;

public class GovernorateGuideModulesTests
{
    private const int Category = 2;

    private sealed class RecordingPublisher : IListingEventPublisher
    {
        public List<ListingChange> Changes { get; } = new();

        public Task PublishAsync(IReadOnlyList<ListingChange> changes, CancellationToken cancellationToken = default)
        {
            Changes.AddRange(changes);
            return Task.CompletedTask;
        }
    }

    private static AppDbContext NewContext(params IInterceptor[] interceptors)
    {
        var builder = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString());
        if (interceptors.Length > 0)
            builder.AddInterceptors(interceptors);

        var context = new AppDbContext(builder.Options);
        context.Database.EnsureCreated();
        return context;
    }

    private static IMapper Mapper() =>
        new MapperConfiguration(c => c.AddMaps(typeof(GovernorateGuideMappingProfile).Assembly),
            NullLoggerFactory.Instance).CreateMapper();

    private static LookupService Lookups(AppDbContext context) =>
        new(new LookupRepository(context), new MemoryCache(new MemoryCacheOptions()), new LookupCache(), Mapper());

    private static CategorySelectionResolver Resolver(AppDbContext context) => new(new LookupRepository(context));

    private static CreateAdFormService FormService(AppDbContext context) =>
        new(Resolver(context), new AdFormBreadcrumbBuilder(Options.Create(new AppSettings())),
            Lookups(context), new AdminFormRepository(context), Mapper());

    private static AdvertiserLookupService AdvertiserService(AppDbContext context) =>
        new(Resolver(context), new AdvertiserLookupRepository(context));

    private static GeneralServiceService Services(AppDbContext context, IFileService? files = null) =>
        new(new GeneralServiceRepository(context), files ?? new StubFileService(), Mapper(),
            SilentProxy<INotificationService>.Create());

    private static CommercialShopService Shops(AppDbContext context) =>
        new(new CommercialShopRepository(context), new StubFileService(), Mapper(),
            SilentProxy<INotificationService>.Create());

    private static ResortHotelAmusementService Resorts(AppDbContext context) =>
        new(new ResortHotelAmusementRepository(context), new StubFileService(), Mapper(),
            SilentProxy<INotificationService>.Create());

    private static (AdminAdService Service, SilentProxy<INotificationService> Notifications) Admin(AppDbContext context)
    {
        var repository = new AdminAdRepository(
            context,
            [new CommercialShopListingSource(context), new GeneralServiceListingSource(context), new ResortHotelAmusementListingSource(context)],
            [
                new ListingModerationSource<CommercialShop>(context, ListingModuleType.CommercialShop),
                new ListingModerationSource<GeneralService>(context, ListingModuleType.GeneralService),
                new ListingModerationSource<ResortHotelAmusement>(context, ListingModuleType.ResortHotelAmusement)
            ]);

        var notifications = SilentProxy<INotificationService>.Create();
        var service = new AdminAdService(repository, SilentProxy<IListingInteractionService>.Create(), Lookups(context),
            notifications, SilentProxy<IListingInterestNotifier>.Create(), SilentProxy<IAdminAuditService>.Create(),
            NullLogger<AdminAdService>.Instance);

        return (service, SilentProxy<INotificationService>.Of(notifications));
    }

    private static UploadImageModel Upload() =>
        new() { FileName = "photo.jpg", ContentType = "image/jpeg", Length = 3, Content = [1, 2, 3] };

    private static string SeedUser(AppDbContext context)
    {
        var id = Guid.NewGuid().ToString();
        context.Users.Add(new ApplicationUser
        {
            Id = id, UserName = id, FirstName = "منى", SecondName = "سالم", PhoneNumber = "01012345678",
            Email = $"{id}@example.com", Governorate = LocationConstants.Governorate, Center = "الفيوم"
        });
        context.SaveChanges();
        return id;
    }

    private static T Base<T>(T request, string name) where T : Shared.DTOs.Directory.CreateDirectoryListingRequest
    {
        request.Name = name;
        request.Title = "عنوان الإعلان";
        request.Description = "وصف الإعلان";
        request.Address = "شارع الجمهورية";
        request.Phone = "01012345678";
        request.Images = [new Microsoft.AspNetCore.Http.FormFile(new MemoryStream([1, 2, 3]), 0, 3, "Images", "photo.jpg")];
        return request;
    }

    private static T Service<T>(GeneralServiceType type, GeneralServiceSubtype subtype, string name = "أحمد للخدمات")
        where T : CreateGeneralServiceRequest, new()
    {
        var request = Base(new T(), name);
        request.ServiceType = type;
        if (type == GeneralServiceType.Fixed) request.FixedServiceSubtype = subtype;
        else request.MobileServiceSubtype = subtype;
        return request;
    }

    private static FormFieldDto Field(AdFormSchema schema, string name) => schema.Fields.Single(f => f.Name == name);

    [Fact]
    public void The_three_modules_are_new_sub_categories_of_the_governorate_guide()
    {
        Assert.Equal("دليل محافظتي", CategoryCatalog.FindCategory(CategoryType.WorkshopsAndCraftsmen)!.NameAr);

        var expected = new (SubCategoryType Sub, int Id, ListingModuleType Module, int ModuleId, string NameAr)[]
        {
            (SubCategoryType.CommercialShops, 66, ListingModuleType.CommercialShop, 101, "محلات تجارية"),
            (SubCategoryType.GeneralServices, 67, ListingModuleType.GeneralService, 102, "خدمات عامة"),
            (SubCategoryType.ResortsHotelsAndAmusement, 68, ListingModuleType.ResortHotelAmusement, 103, "قرى سياحية وفنادق وملاهي")
        };

        foreach (var (sub, id, module, moduleId, nameAr) in expected)
        {
            Assert.Equal(id, (int)sub);
            Assert.Equal(moduleId, (int)module);
            var entry = CategoryCatalog.SubCategories.Single(s => s.Id == sub);
            Assert.Equal(CategoryType.WorkshopsAndCraftsmen, entry.Category);
            Assert.Equal(nameAr, entry.NameAr);
            Assert.Equal(module, ListingModuleCatalog.ModuleOf(sub));
            Assert.Equal(CategoryType.WorkshopsAndCraftsmen, ListingModuleCatalog.CategoryOf(module));
        }

        Assert.Single(CategoryCatalog.SubCategories, s => s.NameAr.Contains("فنادق"));
        Assert.DoesNotContain(CategoryCatalog.SubCategories, s => s.NameAr is "قرى سياحية" or "فنادق" or "ملاهي");
        Assert.Equal(CategoryType.RealEstate, CategoryCatalog.SubCategories.Single(s => s.Id == SubCategoryType.Shops).Category);
    }

    [Fact]
    public async Task The_categories_tree_lists_them_under_the_governorate_guide_beside_the_existing_subs()
    {
        using var context = NewContext();

        var guide = (await Lookups(context).GetCategoriesTreeAsync()).Single(c => c.Id == Category);

        Assert.Equal(
            new[] { "الورش", "الحرفيين", "محلات تجارية", "خدمات عامة", "قرى سياحية وفنادق وملاهي" },
            guide.SubCategories.OrderBy(s => s.Id).Select(s => s.NameAr));
    }

    public static TheoryData<SubCategoryType> Modules() => new()
    {
        SubCategoryType.CommercialShops, SubCategoryType.GeneralServices, SubCategoryType.ResortsHotelsAndAmusement
    };

    [Theory]
    [MemberData(nameof(Modules))]
    public void Each_module_is_registered_in_all_three_lookup_systems_and_every_seam(SubCategoryType sub)
    {
        var module = GovernorateGuideCatalog.ModuleOf(sub)!;

        Assert.NotNull(AdFormSchemaCatalog.GetSchema(Category, (int)sub));
        Assert.Null(AdFormSchemaCatalog.GetSchema((int)CategoryType.RealEstate, (int)sub));
        Assert.NotNull(ReadConfigCatalog.GetSchema(Category, (int)sub));
        Assert.Equal(AdvertiserLookupCatalog.NameParameter, AdvertiserLookupCatalog.FilterParameterOf(sub));
        Assert.Contains(sub, AdvertiserLookupRepository.SupportedSubCategories);

        Assert.True(ModerationModuleRegistry.EntityTypes.ContainsKey(module.Type));
        Assert.Contains(module.ImagesFolder, ImageConstants.AllFolders);
        Assert.Equal(module.Route["api/".Length..], ListingModuleCatalog.RouteOf(module.Type));

        var list = ReadConfigCatalog.GetSchema(Category, (int)sub)!;
        Assert.Equal(module.PublicRoute, list.Find(ReadOperationKeys.List)!.Endpoint);
        Assert.Equal(ReadOperationKeys.Advertisers,
            list.Find(ReadOperationKeys.List)!.QueryParameters!.Single(p => p.Name == "name").OptionsSource);
        foreach (var key in new[] { "listingActions", "similarListings", "recentlyViewed", "favorites" })
            Assert.Equal((int)module.Type, list.Find(key)!.Query!["type"]);

        foreach (var verb in new[] { "POST", "GET" })
            Assert.True(ApiRouteIndex.Exists(verb, module.PublicRoute));
        foreach (var verb in new[] { "GET", "PUT", "DELETE" })
            Assert.True(ApiRouteIndex.Exists(verb, module.PublicRoute + "/{id}"));
    }

    [Fact]
    public void Writes_need_a_signed_in_user_and_lookup_routes_are_public()
    {
        foreach (var controller in new[] { typeof(CommercialShopsController), typeof(GeneralServicesController), typeof(ResortsHotelsAndAmusementController) })
        {
            Assert.NotNull(controller.BaseType!.GetCustomAttribute<AuthorizeAttribute>(inherit: true));
            foreach (var name in new[] { "Create", "Update", "Delete" })
                Assert.Null(controller.GetMethod(name)!.GetCustomAttribute<AllowAnonymousAttribute>());
        }

        Assert.True(ApiRouteIndex.Exists("GET", "/api/commercial-shops/activity-types"));
        Assert.True(ApiRouteIndex.Exists("GET", "/api/general-services/service-types"));
    }

    [Fact]
    public void The_commercial_shop_form_matches_the_specification()
    {
        var schema = AdFormSchemaCatalog.GetSchema(Category, (int)SubCategoryType.CommercialShops)!;

        Assert.Equal(
            new[] { "Name", "ActivityType", "OtherActivityType", "Title", "Description", "Images", "Address", "GoogleMapsUrl", "Phone", "WhatsApp", "Email" },
            schema.Fields.Select(f => f.Name));

        Assert.Equal("اسم المحل", Field(schema, "Name").Label);
        Assert.Equal("نوع النشاط", Field(schema, "ActivityType").Label);
        Assert.False(Field(schema, "ActivityType").Required);
        Assert.Equal(ShopCatalog.SuitableActivities.Count, Field(schema, "ActivityType").Options!.Count);
        Assert.Equal((int)ShopSuitableActivity.Other, Field(schema, "OtherActivityType").RequiredWhen!.Values.Single());
        Assert.True(Field(schema, "Title").Required);
        Assert.True(Field(schema, "Description").Required);
        Assert.True(Field(schema, "Address").Required);
        Assert.Equal("اكتب العنوان بالتفصيل، الشارع والعلامة المميزة", Field(schema, "Address").Placeholder);
        Assert.True(Field(schema, "Phone").Required);
        Assert.False(Field(schema, "WhatsApp").Required);
        Assert.False(Field(schema, "Email").Required);
        Assert.False(Field(schema, "GoogleMapsUrl").Required);
        Assert.True(Field(schema, "Images").Multiple);
    }

    [Fact]
    public void The_general_service_form_switches_the_subtype_list_with_the_service_type()
    {
        var schema = AdFormSchemaCatalog.GetSchema(Category, (int)SubCategoryType.GeneralServices)!;

        Assert.Equal(
            new[] { "Name", "ServiceType", "FixedServiceSubtype", "MobileServiceSubtype", "Title", "Description", "Images", "Address", "GoogleMapsUrl", "Phone", "WhatsApp", "Email" },
            schema.Fields.Select(f => f.Name));

        Assert.Equal("اسم المعلن", Field(schema, "Name").Label);
        var type = Field(schema, "ServiceType");
        Assert.True(type.Required);
        Assert.Equal(new[] { "خدمات ثابتة", "خدمات متنقلة" }, type.Options!.Select(o => o.Label));

        var fixedField = Field(schema, "FixedServiceSubtype");
        Assert.Equal(1, fixedField.VisibleWhen!.Values.Single());
        Assert.Equal(1, fixedField.RequiredWhen!.Values.Single());
        Assert.Equal(
            new[] { "الإيبوكسي والفايبر جلاس", "صيانة موبايلات", "حلواني شرقي وعربي", "صالات تصوير أفراح", "مدارس خاصة", "دعاية وإعلان", "طاقة شمسية" },
            fixedField.Options!.Select(o => o.Label));

        var mobileField = Field(schema, "MobileServiceSubtype");
        Assert.Equal(2, mobileField.VisibleWhen!.Values.Single());
        Assert.Equal(2, mobileField.RequiredWhen!.Values.Single());
        Assert.Equal(
            new[] { "شيف أفراح", "سيارات زفاف", "فرش العروسة والنيش", "نقل جماعي", "مشاوير خاصة", "نقل مدرسي", "نقل بضائع وأثاث", "ونش سيارات", "دليفري وتوصيل" },
            mobileField.Options!.Select(o => o.Label));
    }

    [Fact]
    public void The_resorts_form_is_one_module_with_the_combined_name_and_the_specified_fields()
    {
        var schema = AdFormSchemaCatalog.GetSchema(Category, (int)SubCategoryType.ResortsHotelsAndAmusement)!;

        Assert.Equal("resortsHotelsAndAmusement", schema.Module);
        Assert.Equal("/api/resorts-hotels-amusement", schema.Submit.Endpoint);
        Assert.Equal("اسم النشاط / اسم المعلن", Field(schema, "Name").Label);
        Assert.Equal(
            new[] { "Name", "Title", "Description", "Images", "Address", "GoogleMapsUrl", "Phone", "WhatsApp", "Email" },
            schema.Fields.Select(f => f.Name));
    }

    [Fact]
    public async Task The_create_form_api_serves_all_three_forms()
    {
        using var context = NewContext();

        foreach (var sub in new[] { 66, 67, 68 })
        {
            var form = await FormService(context).GetCreateAdFormAsync(Category, sub);
            Assert.Equal(sub, form.SubCategory!.Id);
            Assert.NotEmpty(form.Fields);
        }
    }

    [Fact]
    public void The_read_config_publishes_module_filters()
    {
        var shops = ReadConfigCatalog.GetSchema(Category, 66)!.Find(ReadOperationKeys.List)!.QueryParameters!;
        Assert.Equal(ShopCatalog.SuitableActivities.Count, shops.Single(p => p.Name == "activityType").Options!.Count);

        var services = ReadConfigCatalog.GetSchema(Category, 67)!.Find(ReadOperationKeys.List)!.QueryParameters!;
        Assert.Equal(2, services.Single(p => p.Name == "serviceType").Options!.Count);
        Assert.Equal(16, services.Single(p => p.Name == "serviceSubtype").Options!.Count);
        Assert.NotNull(ReadConfigCatalog.GetSchema(Category, 67)!.Find(ReadOperationKeys.GeneralServiceTypes));
    }

    [Theory]
    [InlineData(GeneralServiceType.Fixed, GeneralServiceSubtype.SolarEnergy)]
    [InlineData(GeneralServiceType.Mobile, GeneralServiceSubtype.TowTruck)]
    public void A_matching_service_type_and_subtype_is_valid(GeneralServiceType type, GeneralServiceSubtype subtype)
    {
        var result = new CreateGeneralServiceRequestValidator().Validate(Service<CreateGeneralServiceRequest>(type, subtype));
        Assert.True(result.IsValid, string.Join(" | ", result.Errors.Select(e => e.ErrorMessage)));
    }

    [Fact]
    public void Missing_or_mismatched_service_classification_is_refused_in_Arabic()
    {
        var validator = new CreateGeneralServiceRequestValidator();

        var missingType = Base(new CreateGeneralServiceRequest(), "أحمد");
        Assert.Contains(validator.Validate(missingType).Errors, e => e.ErrorMessage == "نوع الخدمة مطلوب.");

        var missingSubtype = Base(new CreateGeneralServiceRequest { ServiceType = GeneralServiceType.Mobile }, "أحمد");
        Assert.Equal("نوع الخدمة المتنقلة مطلوب.", Assert.Single(validator.Validate(missingSubtype).Errors).ErrorMessage);

        var mismatch = Base(new CreateGeneralServiceRequest
        {
            ServiceType = GeneralServiceType.Fixed, FixedServiceSubtype = GeneralServiceSubtype.WeddingCars
        }, "أحمد");
        Assert.Equal("من فضلك اختار خدمة من قائمة الخدمات الثابتة.", Assert.Single(validator.Validate(mismatch).Errors).ErrorMessage);

        var unknown = Base(new CreateGeneralServiceRequest
        {
            ServiceType = GeneralServiceType.Mobile, MobileServiceSubtype = (GeneralServiceSubtype)55
        }, "أحمد");
        Assert.Equal("من فضلك اختار خدمة من قائمة الخدمات المتنقلة.", Assert.Single(validator.Validate(unknown).Errors).ErrorMessage);

        var badType = Base(new CreateGeneralServiceRequest { ServiceType = (GeneralServiceType)9 }, "أحمد");
        Assert.Contains(validator.Validate(badType).Errors, e => e.ErrorMessage.Contains("نوع خدمة صحيح"));
    }

    [Fact]
    public void A_stale_hidden_subtype_is_ignored_and_never_stored()
    {
        var request = Service<CreateGeneralServiceRequest>(GeneralServiceType.Fixed, GeneralServiceSubtype.MobileRepair);
        request.MobileServiceSubtype = (GeneralServiceSubtype)999;

        Assert.True(new CreateGeneralServiceRequestValidator().Validate(request).IsValid);

        var entity = new GeneralService();
        GeneralServiceService.Apply(entity, request);
        Assert.Equal(GeneralServiceType.Fixed, entity.ServiceType);
        Assert.Equal(GeneralServiceSubtype.MobileRepair, entity.ServiceSubtype);
    }

    [Fact]
    public void Every_subtype_belongs_to_exactly_one_service_type()
    {
        Assert.Equal(7, GovernorateGuideCatalog.SubtypesOf(GeneralServiceType.Fixed).Count());
        Assert.Equal(9, GovernorateGuideCatalog.SubtypesOf(GeneralServiceType.Mobile).Count());
        Assert.Equal(Enum.GetValues<GeneralServiceSubtype>().Order(), GovernorateGuideCatalog.ServiceSubtypes.Select(s => s.Id).Order());
        Assert.All(GovernorateGuideCatalog.ServiceSubtypes, s =>
            Assert.False(GovernorateGuideCatalog.BelongsTo(s.Id,
                s.ServiceType == GeneralServiceType.Fixed ? GeneralServiceType.Mobile : GeneralServiceType.Fixed)));
    }

    [Fact]
    public void Commercial_shop_activity_rules()
    {
        var validator = new CreateCommercialShopRequestValidator();

        var optional = Base(new CreateCommercialShopRequest(), "محل النور");
        Assert.True(validator.Validate(optional).IsValid);

        var other = Base(new CreateCommercialShopRequest { ActivityType = ShopSuitableActivity.Other }, "محل النور");
        Assert.Equal("من فضلك اكتب نوع النشاط عند اختيار 'أخرى'.", Assert.Single(validator.Validate(other).Errors).ErrorMessage);

        var invalid = Base(new CreateCommercialShopRequest { ActivityType = (ShopSuitableActivity)500 }, "محل النور");
        Assert.Single(validator.Validate(invalid).Errors);

        var stale = Base(new CreateCommercialShopRequest { ActivityType = ShopSuitableActivity.Pharmacy, OtherActivityType = "قديم" }, "محل النور");
        var entity = new CommercialShop();
        CommercialShopService.Apply(entity, stale);
        Assert.Null(entity.OtherActivityType);
    }

    [Fact]
    public void Base_fields_are_enforced_with_the_shared_Arabic_messages()
    {
        foreach (var errors in new[]
                 {
                     new CreateCommercialShopRequestValidator().Validate(new CreateCommercialShopRequest()).Errors,
                     new CreateResortHotelAmusementRequestValidator().Validate(new CreateResortHotelAmusementRequest()).Errors
                 })
        {
            var messages = errors.Select(e => e.ErrorMessage).ToList();
            Assert.Contains("عنوان الإعلان مطلوب.", messages);
            Assert.Contains("وصف الإعلان مطلوب.", messages);
            Assert.Contains("رقم الهاتف مطلوب.", messages);
            Assert.All(messages, m => Assert.False(ArabicText.ContainsEnglishProse(m), m));
        }

        var phone = Base(new CreateResortHotelAmusementRequest(), "قرية النخيل");
        phone.Phone = "0123";
        phone.WhatsApp = "123";
        phone.Email = "bad";
        phone.GoogleMapsUrl = "ftp://x";
        Assert.Equal(4, new CreateResortHotelAmusementRequestValidator().Validate(phone).Errors.Count);
    }

    [Fact]
    public async Task Services_persist_switch_type_on_update_and_filter_by_classification()
    {
        using var context = NewContext();
        var owner = SeedUser(context);
        var stranger = SeedUser(context);
        var service = Services(context);

        var created = await service.CreateAsync(owner,
            Service<CreateGeneralServiceRequest>(GeneralServiceType.Fixed, GeneralServiceSubtype.SolarEnergy), [Upload(), Upload()]);

        Assert.Equal("خدمات ثابتة", created.ServiceTypeName);
        Assert.Equal("طاقة شمسية", created.ServiceSubtypeName);
        Assert.Equal(67, created.SubCategoryId);
        Assert.Equal("دليل محافظتي", created.CategoryName);
        Assert.Equal(2, created.Images.Count);

        var update = Service<UpdateGeneralServiceRequest>(GeneralServiceType.Mobile, GeneralServiceSubtype.DeliveryServices);
        await Assert.ThrowsAsync<NotFoundException>(() => service.UpdateAsync(stranger, false, created.Id, update, []));
        var updated = await service.UpdateAsync(owner, false, created.Id, update, []);
        Assert.Equal(GeneralServiceType.Mobile, updated.ServiceType);
        Assert.Equal("دليفري وتوصيل", updated.ServiceSubtypeName);

        foreach (var entity in context.GeneralServices.IgnoreQueryFilters())
        {
            entity.ModerationStatus = ModerationStatus.Approved;
            entity.PublishedAt = DateTime.UtcNow;
            entity.ExpireAt = DateTime.UtcNow.AddDays(30);
        }
        await context.SaveChangesAsync();

        Assert.Equal(1, (await service.GetListAsync(new GeneralServiceFilterParams { ServiceType = GeneralServiceType.Mobile })).TotalCount);
        Assert.Equal(0, (await service.GetListAsync(new GeneralServiceFilterParams { ServiceType = GeneralServiceType.Fixed })).TotalCount);
        Assert.Equal(1, (await service.GetListAsync(new GeneralServiceFilterParams { ServiceSubtype = GeneralServiceSubtype.DeliveryServices })).TotalCount);
    }

    [Fact]
    public async Task Commercial_shops_store_the_activity_and_the_advertiser_lookup_lists_only_published_names()
    {
        using var context = NewContext();
        var owner = SeedUser(context);
        var shops = Shops(context);

        var pharmacy = Base(new CreateCommercialShopRequest { ActivityType = ShopSuitableActivity.Pharmacy }, "صيدلية الشفاء");
        var created = await shops.CreateAsync(owner, pharmacy, [Upload()]);
        Assert.Equal("صيدلية", created.ActivityTypeName);

        await shops.CreateAsync(owner, Base(new CreateCommercialShopRequest(), "قيد المراجعة"), [Upload()]);

        var stored = await context.CommercialShops.IgnoreQueryFilters().SingleAsync(x => x.Id == created.Id);
        stored.ModerationStatus = ModerationStatus.Approved;
        stored.PublishedAt = DateTime.UtcNow;
        stored.ExpireAt = DateTime.UtcNow.AddDays(30);
        await context.SaveChangesAsync();

        var advertisers = await AdvertiserService(context).GetAdvertisersAsync(Category, 66, new AdvertiserLookupParams());
        Assert.Equal("صيدلية الشفاء", Assert.Single(advertisers.Items).Name);

        Assert.Equal(1, (await shops.GetListAsync(new CommercialShopFilterParams { ActivityType = ShopSuitableActivity.Pharmacy })).TotalCount);
        Assert.Equal(1, (await shops.GetListAsync(new CommercialShopFilterParams { Name = "صيدلية الشفاء" })).TotalCount);
    }

    [Fact]
    public async Task Approval_publishes_once_notifies_once_and_makes_the_listing_public()
    {
        var publisher = new RecordingPublisher();
        using var context = NewContext(new ListingRealtimeInterceptor([publisher], NullLogger<ListingRealtimeInterceptor>.Instance));
        var owner = SeedUser(context);

        var created = await Resorts(context).CreateAsync(owner, Base(new CreateResortHotelAmusementRequest(), "قرية الواحة"), [Upload()]);
        Assert.Equal(0, (await Resorts(context).GetListAsync(new ResortHotelAmusementFilterParams())).TotalCount);
        publisher.Changes.Clear();

        var (admin, notifications) = Admin(context);
        await admin.ApproveAsync(ListingModuleType.ResortHotelAmusement, created.Id, "admin", new ApproveListingRequest());
        await admin.ApproveAsync(ListingModuleType.ResortHotelAmusement, created.Id, "admin", new ApproveListingRequest());

        var approval = Assert.Single(publisher.Changes, c => c.Event.Change == ListingChangeKinds.Approved);
        Assert.True(approval.Event.IsPublic);
        Assert.Equal(68, approval.Event.SubCategoryId);
        Assert.Equal("resorts-hotels-amusement", approval.Event.Route);
        Assert.Single(notifications.Calls, c => c.Method == nameof(INotificationService.CreateAsync));

        Assert.Equal(created.Id, Assert.Single((await Resorts(context).GetListAsync(new ResortHotelAmusementFilterParams())).Items).Id);
    }
}

public class ListingMediaFileInterceptorTests
{
    private sealed class RecordingFileService : IFileService
    {
        public List<string> Deleted { get; } = new();

        private readonly StubFileService _inner = new();

        public Task<StoredFile> SaveAsync(UploadImageModel file, string folder, CancellationToken cancellationToken = default) =>
            _inner.SaveAsync(file, folder, cancellationToken);

        public Task<IReadOnlyList<StoredFile>> SaveImagesAsync(IReadOnlyList<UploadImageModel> files, string folder,
            int existingCount = 0, CancellationToken cancellationToken = default) =>
            _inner.SaveImagesAsync(files, folder, existingCount, cancellationToken);

        public Task<StoredFile> SaveDocumentAsync(UploadImageModel file, string folder, CancellationToken cancellationToken = default) =>
            _inner.SaveDocumentAsync(file, folder, cancellationToken);

        public Task<StoredFile> SaveVideoAsync(UploadImageModel file, string folder, CancellationToken cancellationToken = default) =>
            _inner.SaveVideoAsync(file, folder, cancellationToken);

        public void Delete(string relativePath) => Deleted.Add(relativePath);

        public string BuildPublicUrl(string relativePath) => _inner.BuildPublicUrl(relativePath);
    }

    private sealed class FailingSaveInterceptor : SaveChangesInterceptor
    {
        public bool Fail { get; set; }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default) =>
            Fail ? throw new DbUpdateException("simulated failure") : base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static (AppDbContext Context, RecordingFileService Files, FailingSaveInterceptor Failer) NewContext()
    {
        var files = new RecordingFileService();
        var failer = new FailingSaveInterceptor();
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .AddInterceptors(new ListingMediaFileInterceptor(files, NullLogger<ListingMediaFileInterceptor>.Instance), failer)
            .Options;

        var context = new AppDbContext(options);
        context.Database.EnsureCreated();
        return (context, files, failer);
    }

    private static async Task<(GeneralService Listing, GeneralServiceImage Image)> SeedAsync(AppDbContext context)
    {
        var listing = new GeneralService
        {
            Id = Guid.NewGuid(), UserId = "owner", Name = "اسم", Title = "عنوان", Description = "وصف",
            Address = "عنوان", Phone = "01012345678", ServiceType = GeneralServiceType.Fixed,
            ServiceSubtype = GeneralServiceSubtype.SolarEnergy
        };
        var image = new GeneralServiceImage
        {
            Id = Guid.NewGuid(), ListingId = listing.Id, FileName = "a.jpg",
            ImagePath = "/uploads/general-services/a.jpg", ImageUrl = "https://x/uploads/general-services/a.jpg"
        };
        listing.Images.Add(image);
        context.Add(listing);
        await context.SaveChangesAsync();
        return (listing, image);
    }

    [Fact]
    public async Task A_removed_image_file_is_deleted_only_after_the_save_succeeds()
    {
        var (context, files, _) = NewContext();
        var (_, image) = await SeedAsync(context);

        context.Remove(image);
        Assert.Empty(files.Deleted);

        await context.SaveChangesAsync();

        Assert.Equal(["/uploads/general-services/a.jpg"], files.Deleted);
    }

    [Fact]
    public async Task A_failed_save_keeps_the_removed_file_and_deletes_the_newly_uploaded_one()
    {
        var (context, files, _) = NewContext();
        var (listing, image) = await SeedAsync(context);

        context.Remove(image);
        context.Add(new GeneralServiceImage
        {
            Id = Guid.NewGuid(), ListingId = listing.Id, FileName = null!,
            ImagePath = "/uploads/general-services/b.jpg", ImageUrl = "https://x/uploads/general-services/b.jpg"
        });

        await Assert.ThrowsAnyAsync<Exception>(() => context.SaveChangesAsync());

        Assert.Equal(["/uploads/general-services/b.jpg"], files.Deleted);
    }

    [Fact]
    public async Task A_successful_save_never_deletes_new_or_untouched_files()
    {
        var (context, files, _) = NewContext();
        var (listing, _) = await SeedAsync(context);

        listing.Title = "عنوان جديد";
        context.Add(new GeneralServiceImage
        {
            Id = Guid.NewGuid(), ListingId = listing.Id, FileName = "c.jpg",
            ImagePath = "/uploads/general-services/c.jpg", ImageUrl = "https://x/uploads/general-services/c.jpg"
        });
        await context.SaveChangesAsync();

        Assert.Empty(files.Deleted);
    }

    [Fact]
    public void No_service_deletes_a_removed_image_file_before_saving_any_more()
    {
        foreach (var file in Directory.EnumerateFiles(System.IO.Path.Combine(RepositoryRoot.Path, "Core", "Services"), "*.cs", SearchOption.AllDirectories))
        {
            var lines = File.ReadAllLines(file);
            for (var i = 0; i + 1 < lines.Length; i++)
            {
                Assert.False(
                    lines[i].Trim() == "_fileService.Delete(image.ImagePath);" && lines[i + 1].Contains("RemoveImage(image)"),
                    $"{file}:{i + 1} deletes the file before the database save");
            }
        }
    }
}
