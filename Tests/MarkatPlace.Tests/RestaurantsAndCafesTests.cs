using System.Reflection;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Persistence.Data;
using Persistence.Data.Development;
using Persistence.Listings;
using Persistence.Repositories;
using Presentation.Controllers;
using Presentation.Controllers.Admin;
using Services;
using Services.AdForms;
using Services.Admin;
using Services.Lookups;
using Services.Mapping;
using Services.ReadConfigs;
using Services.RestaurantsAndCafes;
using Services.Validation;
using ServicesAbstraction;
using Shared.Authorization;
using Shared.Constants;
using Shared.DTOs.Admin;
using Shared.DTOs.Lookups;
using Shared.DTOs.Lookups.Forms;
using Shared.DTOs.Lookups.Read;
using Shared.DTOs.RestaurantsAndCafes;
using Shared.Enums;
using Shared.Exceptions;
using Shared.Settings;
using Xunit;

namespace MarkatPlace.Tests;

public class RestaurantsAndCafesTests
{
    private const int Category = 15;

    private const int SubCategory = 64;

    private const ListingModuleType Module = ListingModuleType.RestaurantAndCafe;

    private static readonly DateTime Baseline = new(2026, 10, 9, 12, 0, 0, DateTimeKind.Utc);

    private static AppDbContext NewContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new AppDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }

    private static IMapper Mapper() =>
        new MapperConfiguration(
            configuration => configuration.AddMaps(typeof(RestaurantsAndCafesMappingProfile).Assembly),
            NullLoggerFactory.Instance).CreateMapper();

    private static LookupService Lookups(AppDbContext context) =>
        new(new LookupRepository(context), new MemoryCache(new MemoryCacheOptions()), new LookupCache(), Mapper());

    private static CategorySelectionResolver Resolver(AppDbContext context) => new(new LookupRepository(context));

    private static CreateAdFormService FormService(AppDbContext context) =>
        new(Resolver(context), new AdFormBreadcrumbBuilder(Options.Create(new AppSettings())),
            Lookups(context), new AdminFormRepository(context), Mapper());

    private static ReadConfigService ReadService(AppDbContext context) => new(Resolver(context), Mapper());

    private static AdvertiserLookupService AdvertiserService(AppDbContext context) =>
        new(Resolver(context), new AdvertiserLookupRepository(context));

    private static RestaurantAndCafeService Service(AppDbContext context) =>
        new(new RestaurantAndCafeRepository(context), new StubFileService(), Mapper(),
            SilentProxy<INotificationService>.Create());

    private static (AdminAdService Service, SilentProxy<IAdminAuditService> Audit) AdminService(AppDbContext context)
    {
        var repository = new AdminAdRepository(
            context,
            [new RestaurantAndCafeListingSource(context), new AccountantOfficeListingSource(context)],
            [
                new ListingModerationSource<RestaurantAndCafe>(context, ListingModuleType.RestaurantAndCafe),
                new ListingModerationSource<AccountantOffice>(context, ListingModuleType.AccountantOffice)
            ]);

        var audit = SilentProxy<IAdminAuditService>.Create();

        var service = new AdminAdService(
            repository,
            SilentProxy<IListingInteractionService>.Create(),
            Lookups(context),
            SilentProxy<INotificationService>.Create(),
            SilentProxy<IListingInterestNotifier>.Create(),
            audit,
            NullLogger<AdminAdService>.Instance);

        return (service, SilentProxy<IAdminAuditService>.Of(audit));
    }

    [Fact]
    public void The_ids_are_the_next_free_ones_and_the_category_opens_its_form_directly()
    {
        Assert.Equal(15, (int)CategoryType.RestaurantsAndCafes);
        Assert.Equal(64, (int)SubCategoryType.RestaurantsAndCafes);
        Assert.Equal(100, (int)ListingModuleType.RestaurantAndCafe);

        Assert.True(CategoryCatalog.OpensFormDirectly(Category));
        Assert.Equal(SubCategory, CategoryCatalog.DirectSubCategoryOf(Category));

        Assert.All(
            CategoryCatalog.Categories.Where(entry => entry.Id != CategoryType.RestaurantsAndCafes),
            entry => Assert.Null(CategoryCatalog.DirectSubCategoryOf((int)entry.Id)));
    }

    [Fact]
    public async Task The_main_categories_list_flags_the_category_as_opening_the_form_directly()
    {
        using var context = NewContext();

        var tree = await Lookups(context).GetCategoriesTreeAsync();
        var restaurants = tree.Single(category => category.Id == Category);

        Assert.Equal("مطاعم وكافيهات", restaurants.NameAr);
        Assert.True(restaurants.OpensFormDirectly);
        Assert.Equal(SubCategory, restaurants.DirectSubCategoryId);
        Assert.Equal(SubCategory, Assert.Single(restaurants.SubCategories).Id);
        Assert.All(tree.Where(c => c.Id != Category), c => Assert.False(c.OpensFormDirectly));

        var flat = await Lookups(context).GetCategoriesAsync();
        Assert.True(flat.Single(c => c.Id == Category).OpensFormDirectly);
    }

    [Fact]
    public async Task Choosing_the_category_returns_the_full_form_without_a_sub_category_step()
    {
        using var context = NewContext();

        var form = await FormService(context).GetCreateAdFormAsync(Category);

        Assert.False(form.RequiresSubCategory);
        Assert.Equal(SubCategory, form.SubCategory!.Id);
        Assert.Equal("restaurantsAndCafes", form.Module);
        Assert.Equal("/api/restaurants-and-cafes", form.Submit!.Endpoint);

        var withSub = await FormService(context).GetCreateAdFormAsync(Category, SubCategory);
        Assert.Equal(withSub.Fields.Select(f => f.Name), form.Fields.Select(f => f.Name));

        var config = await ReadService(context).GetReadConfigAsync(Category);
        Assert.False(config.RequiresSubCategory);
        Assert.Equal("/api/restaurants-and-cafes", config.List!.Endpoint);
    }

    [Fact]
    public async Task The_form_has_the_specified_fields_order_labels_placeholders_and_flags()
    {
        using var context = NewContext();

        var form = await FormService(context).GetCreateAdFormAsync(Category);

        var expected = new (string Name, string Label, string Placeholder, bool Required, string Section)[]
        {
            ("Name", "اسم المطعم أو الكافيه", "اكتب اسم المطعم أو الكافيه", true, "بيانات المطعم أو الكافيه"),
            ("Title", "عنوان الإعلان", "اكتب عنوان الإعلان", true, "بيانات المطعم أو الكافيه"),
            ("Description", "وصف الإعلان", "اكتب وصف المطعم أو الكافيه وتفاصيله", true, "بيانات المطعم أو الكافيه"),
            ("Images", "الصور", null!, true, "بيانات المطعم أو الكافيه"),
            ("Address", "العنوان", "اكتب العنوان بالتفصيل، الشارع والعلامة المميزة", true, "الموقع"),
            ("GoogleMapsUrl", "رابط الموقع على خرائط جوجل", "https://maps.google.com/...", false, "الموقع"),
            ("Phone", "رقم الهاتف", "01012345678", true, "بيانات التواصل"),
            ("WhatsApp", "رقم الواتساب", "01012345678", false, "بيانات التواصل"),
            ("Email", "البريد الإلكتروني", "name@example.com", false, "بيانات التواصل")
        };

        Assert.Equal(expected.Select(e => e.Name), form.Fields.Select(f => f.Name));

        foreach (var (name, label, placeholder, required, section) in expected)
        {
            var field = form.Fields.Single(f => f.Name == name);
            Assert.Equal(label, field.Label);
            Assert.Equal(required, field.Required);
            Assert.Equal(section, field.Section);

            if (placeholder is not null)
                Assert.Equal(placeholder, field.Placeholder);
        }

        var images = form.Fields.Single(f => f.Name == "Images");
        Assert.True(images.Multiple);
        Assert.Equal(1, images.MinItems);
        Assert.Equal(FormFieldTypes.Image, images.Type);
        Assert.Equal(AdvertisementCatalog.EgyptianPhonePattern, form.Fields.Single(f => f.Name == "WhatsApp").Pattern);
    }

    [Fact]
    public void The_module_is_registered_in_all_three_lookup_locations_and_every_seam()
    {
        Assert.NotNull(AdFormSchemaCatalog.GetSchema(Category, SubCategory));
        Assert.NotNull(ReadConfigCatalog.GetSchema(Category, SubCategory));
        Assert.Equal(AdvertiserLookupCatalog.NameParameter,
            AdvertiserLookupCatalog.FilterParameterOf(SubCategoryType.RestaurantsAndCafes));
        Assert.Contains(SubCategoryType.RestaurantsAndCafes, AdvertiserLookupRepository.SupportedSubCategories);

        var list = ReadConfigCatalog.GetSchema(Category, SubCategory)!.Find(ReadOperationKeys.List)!;
        var name = list.QueryParameters!.Single(p => p.Name == "name");
        Assert.Equal("اسم المطعم أو الكافيه", name.Label);
        Assert.Equal(ReadOperationKeys.Advertisers, name.OptionsSource);

        Assert.Equal(Module, ListingModuleCatalog.ModuleOf(SubCategoryType.RestaurantsAndCafes));
        Assert.Equal(CategoryType.RestaurantsAndCafes, ListingModuleCatalog.CategoryOf(Module));
        Assert.Equal("restaurants-and-cafes", ListingModuleCatalog.RouteOf(Module));
        Assert.Equal(typeof(RestaurantAndCafe), ModerationModuleRegistry.EntityTypes[Module]);
        Assert.Contains(FileUploadConstants.RestaurantsAndCafesFolder, ImageConstants.AllFolders);
        Assert.True(AdminRepublishPolicy.Supports(Module));
    }

    [Fact]
    public void Every_CRUD_route_exists_and_writes_need_a_signed_in_user()
    {
        const string route = "/api/restaurants-and-cafes";

        Assert.True(ApiRouteIndex.Exists("POST", route));
        Assert.True(ApiRouteIndex.Exists("GET", route));
        Assert.True(ApiRouteIndex.Exists("GET", route + "/{id}"));
        Assert.True(ApiRouteIndex.Exists("PUT", route + "/{id}"));
        Assert.True(ApiRouteIndex.Exists("DELETE", route + "/{id}"));

        var controller = typeof(RestaurantsAndCafesController);
        Assert.NotNull(controller.BaseType!.GetCustomAttribute<AuthorizeAttribute>(inherit: true));

        foreach (var name in new[] { "GetList", "GetById" })
            Assert.NotNull(controller.GetMethod(name)!.GetCustomAttribute<AllowAnonymousAttribute>());

        foreach (var name in new[] { "Create", "Update", "Delete" })
            Assert.Null(controller.GetMethod(name)!.GetCustomAttribute<AllowAnonymousAttribute>());
    }

    [Fact]
    public void The_admin_republish_endpoint_is_admin_only_and_needs_the_approve_permission()
    {
        Assert.True(ApiRouteIndex.Exists("PATCH", "/api/v2/admin/ads/{type}/{id}/republish"));

        var controller = typeof(AdminAdsV2Controller);
        var authorize = typeof(AdminApiController).GetCustomAttribute<AuthorizeAttribute>()!;
        Assert.Equal(AppRoles.Admin, authorize.Roles);
        Assert.True(typeof(AdminApiController).IsAssignableFrom(controller));

        var action = controller.GetMethod(nameof(AdminAdsV2Controller.Republish))!;
        Assert.Null(action.GetCustomAttribute<AllowAnonymousAttribute>());
        Assert.Equal(AdminPermission.Approve, action.GetCustomAttribute<RequireAdminPermissionAttribute>()!.Permission);
    }

    [Fact]
    public void A_complete_request_is_valid_and_optional_fields_may_stay_empty()
    {
        var request = ValidRequest();
        request.WhatsApp = "";
        request.Email = null;
        request.GoogleMapsUrl = "  ";

        var result = new CreateRestaurantAndCafeRequestValidator().Validate(request);

        Assert.True(result.IsValid, string.Join(" | ", result.Errors.Select(e => e.ErrorMessage)));
    }

    [Fact]
    public void Every_required_field_is_enforced_by_the_backend()
    {
        var request = new CreateRestaurantAndCafeRequest();

        var errors = new CreateRestaurantAndCafeRequestValidator().Validate(request).Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(g => g.Key, g => g.First().ErrorMessage);

        Assert.Equal("اسم المطعم أو الكافيه مطلوب.", errors["Name"]);
        Assert.Equal("عنوان الإعلان مطلوب.", errors["Title"]);
        Assert.Equal("وصف الإعلان مطلوب.", errors["Description"]);
        Assert.Equal("العنوان مطلوب.", errors["Address"]);
        Assert.Equal("رقم الهاتف مطلوب.", errors["Phone"]);
        Assert.Equal("لازم ترفع صورة واحدة على الأقل.", errors["Images"]);
        Assert.Equal(6, errors.Count);
    }

    [Theory]
    [InlineData("0101234567")]
    [InlineData("01712345678")]
    [InlineData("+201012345678")]
    [InlineData("01O12345678")]
    public void Phone_and_whatsapp_follow_the_Egyptian_mobile_rule(string number)
    {
        var request = ValidRequest();
        request.Phone = number;
        request.WhatsApp = number;

        var errors = new CreateRestaurantAndCafeRequestValidator().Validate(request).Errors;

        Assert.Equal(2, errors.Count);
        Assert.All(errors, e => Assert.Equal("من فضلك اكتب رقم موبايل مصري صحيح (مثال: 01012345678).", e.ErrorMessage));
    }

    [Theory]
    [InlineData("ftp://maps.google.com/x")]
    [InlineData("maps.google.com/x")]
    [InlineData("javascript:alert(1)")]
    public void A_maps_link_must_be_http_or_https(string url)
    {
        var request = ValidRequest();
        request.GoogleMapsUrl = url;

        var error = Assert.Single(new CreateRestaurantAndCafeRequestValidator().Validate(request).Errors);
        Assert.Equal("لينك خرائط جوجل لازم يبدأ بـ http أو https.", error.ErrorMessage);
    }

    [Fact]
    public void A_malformed_email_is_refused_only_when_supplied()
    {
        var request = ValidRequest();
        request.Email = "not-an-email";

        var error = Assert.Single(new CreateRestaurantAndCafeRequestValidator().Validate(request).Errors);
        Assert.Equal("من فضلك اكتب بريد إلكتروني صحيح.", error.ErrorMessage);
    }

    [Fact]
    public async Task Creating_a_listing_stores_every_uploaded_image_and_waits_for_review()
    {
        using var context = NewContext();
        var owner = SeedUser(context);

        var created = await Service(context).CreateAsync(owner, ValidRequest(), [Upload(), Upload(), Upload()]);

        Assert.Equal(3, created.Images.Count);
        Assert.Equal(1, created.Images.Count(i => i.IsPrimary));
        Assert.StartsWith("https://example.test/uploads/restaurants-and-cafes/", created.PrimaryImageUrl);
        Assert.Equal("مطاعم وكافيهات", created.CategoryName);
        Assert.Equal(Category, created.CategoryId);
        Assert.Equal(SubCategory, created.SubCategoryId);

        var stored = await context.RestaurantsAndCafes.IgnoreQueryFilters().Include(x => x.Images).SingleAsync();
        Assert.Equal(3, stored.Images.Count);
        Assert.Equal(ModerationStatus.Pending, stored.ModerationStatus);
        Assert.Null(stored.ExpireAt);

        await Assert.ThrowsAsync<BadRequestException>(() => Service(context).CreateAsync(owner, ValidRequest(), []));
    }

    [Fact]
    public async Task Only_the_owner_or_an_admin_may_delete()
    {
        using var context = NewContext();
        var owner = SeedUser(context);
        var stranger = SeedUser(context);

        var created = await Service(context).CreateAsync(owner, ValidRequest(), [Upload()]);

        await Assert.ThrowsAsync<NotFoundException>(() => Service(context).DeleteAsync(stranger, created.Id, isAdmin: false));
        await Service(context).DeleteAsync(stranger, created.Id, isAdmin: true);

        Assert.True((await context.RestaurantsAndCafes.IgnoreQueryFilters().SingleAsync()).IsDeleted);
    }

    [Fact]
    public async Task The_name_select_lists_published_names_and_filters_exactly_with_paging()
    {
        using var context = NewContext();
        var owner = SeedUser(context);
        var now = DateTime.UtcNow;

        context.RestaurantsAndCafes.AddRange(
            Listing(owner, "كافيه النخيل", now.AddDays(-1)),
            Listing(owner, "كافيه النخيل", now.AddDays(-2)),
            Listing(owner, "كافيه النخيل", now.AddDays(-3)),
            Listing(owner, "كافيه النخيل الجديد", now.AddDays(-4)),
            Listing(owner, "مطعم الريف", now.AddDays(-5)),
            Listing(owner, "قيد المراجعة", now, ModerationStatus.Pending));
        await context.SaveChangesAsync();

        var options = await AdvertiserService(context).GetAdvertisersAsync(Category, SubCategory, new AdvertiserLookupParams());
        Assert.Equal(
            new[] { "كافيه النخيل:3", "كافيه النخيل الجديد:1", "مطعم الريف:1" },
            options.Items.Select(i => $"{i.Name}:{i.ListingsCount}"));

        var searched = await AdvertiserService(context)
            .GetAdvertisersAsync(Category, SubCategory, new AdvertiserLookupParams { Search = "الريف" });
        Assert.Equal("مطعم الريف", Assert.Single(searched.Items).Name);

        var pagedOptions = await AdvertiserService(context)
            .GetAdvertisersAsync(Category, SubCategory, new AdvertiserLookupParams { PageSize = 2, PageIndex = 2 });
        Assert.Equal("مطعم الريف", Assert.Single(pagedOptions.Items).Name);
        Assert.Equal(3, pagedOptions.TotalCount);

        var chosen = await Service(context).GetListAsync(new RestaurantAndCafeFilterParams { Name = "كافيه النخيل" });
        Assert.Equal(3, chosen.TotalCount);
        Assert.All(chosen.Items, item => Assert.Equal("كافيه النخيل", item.Name));

        var page = await Service(context).GetListAsync(
            new RestaurantAndCafeFilterParams { Name = "كافيه النخيل", PageSize = 2, PageIndex = 2 });
        Assert.Single(page.Items);
        Assert.Equal(3, page.TotalCount);

        var search = await Service(context).GetListAsync(new RestaurantAndCafeFilterParams { Search = "النخيل" });
        Assert.Equal(4, search.TotalCount);
    }

    [Fact]
    public async Task Approval_opens_a_thirty_day_window_and_the_countdown_derives_from_it()
    {
        using var context = NewContext();
        var owner = SeedUser(context);
        var created = await Service(context).CreateAsync(owner, ValidRequest(), [Upload()]);
        var (admin, _) = AdminService(context);

        await admin.ApproveAsync(Module, created.Id, "admin", new ApproveListingRequest());

        var stored = await context.RestaurantsAndCafes.IgnoreQueryFilters().SingleAsync();
        Assert.Equal(TimeSpan.FromDays(ListingLifecycle.ActiveDurationDays), stored.ExpireAt - stored.PublishedAt);
        Assert.Equal(30, ListingLifecycle.ActiveDurationDays);

        var published = stored.PublishedAt!.Value;
        Assert.Equal(30, ListingLifecycle.RemainingDays(stored.ExpireAt, published));
        Assert.Equal(20, ListingLifecycle.RemainingDays(stored.ExpireAt, published.AddDays(10)));
        Assert.Equal(20, ListingLifecycle.RemainingDays(stored.ExpireAt, published.AddDays(10.5)));
        Assert.Equal(1, ListingLifecycle.RemainingDays(stored.ExpireAt, published.AddDays(29.5)));
        Assert.Equal(0, ListingLifecycle.RemainingDays(stored.ExpireAt, published.AddDays(30)));
        Assert.True(ListingLifecycle.HasExpired(stored.ExpireAt, published.AddDays(30)));
    }

    [Theory]
    [InlineData(10, false, false)]
    [InlineData(5.01, false, false)]
    [InlineData(5, true, true)]
    [InlineData(1, true, true)]
    [InlineData(0.01, true, true)]
    [InlineData(0, true, false)]
    [InlineData(-3, true, false)]
    public void Republish_opens_five_days_before_expiry_and_stays_open_after_it(
        double daysLeft, bool canRepublish, bool expiringSoon)
    {
        var expireAt = Baseline.AddDays(daysLeft);

        Assert.Equal(canRepublish,
            AdminRepublishPolicy.CanRepublish(Module, ModerationStatus.Approved, expireAt, Baseline));
        Assert.Equal(expiringSoon, AdminRepublishPolicy.IsExpiringSoon(expireAt, Baseline));
    }

    [Fact]
    public void Republish_is_offered_for_every_module_but_never_for_unapproved_listings()
    {
        var expireAt = Baseline.AddDays(1);

        Assert.True(AdminRepublishPolicy.CanRepublish(ListingModuleType.AccountantOffice, ModerationStatus.Approved, expireAt, Baseline));
        Assert.False(AdminRepublishPolicy.CanRepublish(Module, ModerationStatus.Pending, expireAt, Baseline));
        Assert.False(AdminRepublishPolicy.CanRepublish(Module, ModerationStatus.Suspended, expireAt, Baseline));
        Assert.False(AdminRepublishPolicy.CanRepublish(Module, ModerationStatus.Approved, null, Baseline));
    }

    [Fact]
    public async Task The_admin_list_marks_expiring_and_expired_listings_and_filters_them()
    {
        using var context = NewContext();
        var owner = SeedUser(context);
        var now = DateTime.UtcNow;

        var fresh = Listing(owner, "جديد", now.AddDays(-1));
        var expiring = Listing(owner, "قرب ينتهي", now.AddDays(-27));
        var expired = Listing(owner, "منتهي", now.AddDays(-40));
        var republished = Listing(owner, "اتنشر تاني", now.AddDays(-2));
        republished.RepublishCount = 1;

        context.RestaurantsAndCafes.AddRange(fresh, expiring, expired, republished);
        await context.SaveChangesAsync();

        var (admin, _) = AdminService(context);
        var all = await admin.GetAdsAsync(new AdminAdFilterParams { Type = Module, PageSize = 50 });
        var byTitle = all.Items.ToDictionary(item => item.Title);

        Assert.Equal(4, all.TotalCount);

        Assert.False(byTitle["جديد"].CanRepublish);
        Assert.Equal(29, byTitle["جديد"].RemainingDays);

        Assert.True(byTitle["قرب ينتهي"].IsExpiringSoon);
        Assert.True(byTitle["قرب ينتهي"].CanRepublish);
        Assert.Equal(3, byTitle["قرب ينتهي"].RemainingDays);

        Assert.True(byTitle["منتهي"].IsExpired);
        Assert.Equal("Expired", byTitle["منتهي"].Status);
        Assert.Equal(0, byTitle["منتهي"].RemainingDays);
        Assert.True(byTitle["منتهي"].CanRepublish);

        Assert.Equal(1, byTitle["اتنشر تاني"].RepublishCount);

        async Task<string[]> Titles(AdminAdFilterParams filter)
        {
            filter.Type = Module;
            filter.PageSize = 50;
            return (await admin.GetAdsAsync(filter)).Items.Select(i => i.Title).OrderBy(t => t).ToArray();
        }

        Assert.Equal(new[] { "قرب ينتهي" }, await Titles(new AdminAdFilterParams { ExpiringWithinDays = 5 }));
        Assert.Equal(new[] { "قرب ينتهي", "منتهي" }.OrderBy(t => t), await Titles(new AdminAdFilterParams { RepublishDue = true }));
        Assert.Equal(new[] { "منتهي" }, await Titles(new AdminAdFilterParams { Status = ListingStatus.Expired }));
        Assert.Equal(new[] { "اتنشر تاني" }, await Titles(new AdminAdFilterParams { Republished = true }));

        var publicList = await Service(context).GetListAsync(new RestaurantAndCafeFilterParams { PageSize = 50 });
        Assert.DoesNotContain(publicList.Items, item => item.Name == "منتهي");
        Assert.Equal(3, publicList.TotalCount);

        await Assert.ThrowsAsync<NotFoundException>(() => Service(context).GetByIdAsync(expired.Id));

        var details = await admin.GetAdDetailsAsync(Module, expired.Id);
        Assert.True(details.IsExpired);
        Assert.True(details.CanRepublish);
    }

    [Fact]
    public async Task An_admin_republish_of_an_expired_listing_starts_a_new_thirty_days_and_is_audited()
    {
        using var context = NewContext();
        var owner = SeedUser(context);
        var expired = Listing(owner, "منتهي", DateTime.UtcNow.AddDays(-40));
        var firstPublished = expired.FirstPublishedAt;
        context.RestaurantsAndCafes.Add(expired);
        await context.SaveChangesAsync();

        var (admin, audit) = AdminService(context);
        var before = DateTime.UtcNow;

        var result = await admin.RepublishAsync(Module, expired.Id, "admin-1");

        Assert.Equal(1, result.RepublishCount);
        Assert.Equal(30, result.RemainingDays);
        Assert.Equal(TimeSpan.FromDays(30), result.ExpireAt - result.PublishedAt);
        Assert.True(result.PublishedAt >= before);
        Assert.Equal("Active", result.Status);

        var stored = await context.RestaurantsAndCafes.IgnoreQueryFilters().SingleAsync();
        Assert.Equal(result.ExpireAt, stored.ExpireAt);
        Assert.Equal(firstPublished, stored.FirstPublishedAt);
        Assert.Equal(ModerationStatus.Approved, stored.ModerationStatus);
        Assert.Equal("admin-1", stored.ModeratedBy);

        var logged = Assert.Single(audit.Calls, call => call.Method == nameof(IAdminAuditService.LogAsync));
        Assert.Equal(AdminAuditAction.RepublishAd, logged.Args[0]);
        Assert.Equal(expired.Id.ToString(), logged.Args[2]);

        Assert.Equal(expired.Id, (await Service(context).GetByIdAsync(expired.Id)).Id);
    }

    [Fact]
    public async Task An_admin_may_republish_inside_the_five_day_window()
    {
        using var context = NewContext();
        var owner = SeedUser(context);
        var expiring = Listing(owner, "قرب ينتهي", DateTime.UtcNow.AddDays(-26));
        context.RestaurantsAndCafes.Add(expiring);
        await context.SaveChangesAsync();

        var (admin, _) = AdminService(context);
        var result = await admin.RepublishAsync(Module, expiring.Id, "admin");

        Assert.Equal(30, result.RemainingDays);
        Assert.Equal(4, ListingLifecycle.RemainingDays(result.PreviousExpireAt, DateTime.UtcNow));
    }

    [Fact]
    public async Task Ineligible_republish_attempts_are_refused()
    {
        using var context = NewContext();
        var owner = SeedUser(context);

        var running = Listing(owner, "لسه شغال", DateTime.UtcNow.AddDays(-5));
        var pending = Listing(owner, "قيد المراجعة", DateTime.UtcNow.AddDays(-40), ModerationStatus.Pending);
        var office = new AccountantOffice
        {
            Id = Guid.NewGuid(), UserId = owner, Name = "مكتب", Title = "عنوان", Description = "وصف",
            Address = "عنوان", Phone = "01012345678", ModerationStatus = ModerationStatus.Approved,
            PublishedAt = DateTime.UtcNow.AddDays(-40), ExpireAt = DateTime.UtcNow.AddDays(-10)
        };

        context.RestaurantsAndCafes.AddRange(running, pending);
        context.AccountantOffices.Add(office);
        await context.SaveChangesAsync();

        var (admin, audit) = AdminService(context);

        var tooEarly = await Assert.ThrowsAsync<BadRequestException>(() => admin.RepublishAsync(Module, running.Id, "admin"));
        Assert.Contains("25", tooEarly.Message);

        await Assert.ThrowsAsync<BadRequestException>(() => admin.RepublishAsync(Module, pending.Id, "admin"));

await Assert.ThrowsAsync<NotFoundException>(() => admin.RepublishAsync(Module, office.Id, "admin"));
        await Assert.ThrowsAsync<NotFoundException>(() => admin.RepublishAsync(Module, Guid.NewGuid(), "admin"));

        Assert.DoesNotContain(audit.Calls, call => call.Method == nameof(IAdminAuditService.LogAsync));
        Assert.Equal(0, (await context.RestaurantsAndCafes.IgnoreQueryFilters().SingleAsync(x => x.Id == running.Id)).RepublishCount);
    }

    [Fact]
    public async Task The_seed_creates_exactly_one_listing_with_one_day_left_at_the_baseline_and_is_idempotent()
    {
        using var context = NewContext();
        var owner = await SeedUserEntity(context);
        var images = new[]
        {
            new DemoSeedImage("a.jpg", "/uploads/restaurants-and-cafes/a.jpg", "https://example.test/a.jpg"),
            new DemoSeedImage("b.jpg", "/uploads/restaurants-and-cafes/b.jpg", "https://example.test/b.jpg")
        };

        Assert.True(await RestaurantAndCafeDemoSeed.SeedAsync(context, owner, images, Baseline));
        Assert.False(await RestaurantAndCafeDemoSeed.SeedAsync(context, owner, images, Baseline.AddDays(3)));

        var seeded = await context.RestaurantsAndCafes.IgnoreQueryFilters().Include(x => x.Images).SingleAsync();

        Assert.Equal(RestaurantAndCafeDemoSeed.ListingId, seeded.Id);
        Assert.Equal(ModerationStatus.Approved, seeded.ModerationStatus);
        Assert.Equal(2, seeded.Images.Count);
        Assert.Equal(Baseline.AddDays(1), seeded.ExpireAt);
        Assert.Equal(TimeSpan.FromDays(30), seeded.ExpireAt - seeded.PublishedAt);
        Assert.Equal(1, ListingLifecycle.RemainingDays(seeded.ExpireAt, Baseline));
        Assert.True(AdminRepublishPolicy.CanRepublish(Module, seeded.ModerationStatus, seeded.ExpireAt, Baseline));

        var request = new CreateRestaurantAndCafeRequest
        {
            Name = seeded.Name, Title = seeded.Title, Description = seeded.Description, Address = seeded.Address,
            GoogleMapsUrl = seeded.GoogleMapsUrl, Phone = seeded.Phone, WhatsApp = seeded.WhatsApp, Email = seeded.Email,
            Images = [new FormFile(new MemoryStream([1]), 0, 1, "Images", "a.jpg")]
        };
        Assert.True(new CreateRestaurantAndCafeRequestValidator().Validate(request).IsValid);
    }

    [Fact]
    public async Task The_seeded_listing_shows_one_day_left_in_the_admin_list_when_seeded_now()
    {
        using var context = NewContext();
        var owner = await SeedUserEntity(context);

        await RestaurantAndCafeDemoSeed.SeedAsync(
            context, owner, [new DemoSeedImage("a.jpg", "/uploads/a.jpg", "https://example.test/a.jpg")], DateTime.UtcNow);

        var (admin, _) = AdminService(context);
        var item = Assert.Single((await admin.GetAdsAsync(new AdminAdFilterParams { Type = Module })).Items);

        Assert.Equal(1, item.RemainingDays);
        Assert.True(item.IsExpiringSoon);
        Assert.True(item.CanRepublish);
        Assert.Equal("Active", item.Status);
    }

    [Fact]
    public async Task The_seed_refuses_to_reference_images_it_does_not_have()
    {
        using var context = NewContext();
        var owner = await SeedUserEntity(context);

        Assert.False(await RestaurantAndCafeDemoSeed.SeedAsync(context, owner, [], Baseline));
        Assert.Empty(await context.RestaurantsAndCafes.IgnoreQueryFilters().ToListAsync());
    }

    [Fact]
    public void The_seed_only_runs_from_the_guarded_development_seeder()
    {
        var seeder = RepositoryRoot.ReadFile("Infastrucre/Presitance/Data/Development/DevelopmentDataSeeder.cs");

        var guard = seeder.IndexOf("if (!environment.IsDevelopment())", StringComparison.Ordinal);
        var enabled = seeder.IndexOf("DemoData:Enabled", StringComparison.Ordinal);
        var call = seeder.IndexOf("RestaurantAndCafeDemoSeed.SeedAsync", StringComparison.Ordinal);

        Assert.True(guard > 0 && enabled > guard && call > enabled);

        var callers = RepositoryRoot.SourceFiles()
            .Where(file => !file.Contains("/Tests/", StringComparison.Ordinal))
            .Where(file => File.ReadAllText(file).Contains("RestaurantAndCafeDemoSeed.SeedAsync", StringComparison.Ordinal))
            .Select(RepositoryRoot.Relative)
            .ToArray();

        Assert.Equal(new[] { "Infastrucre/Presitance/Data/Development/DevelopmentDataSeeder.cs" }, callers);
    }

    private static CreateRestaurantAndCafeRequest ValidRequest() =>
        new()
        {
            Name = "كافيه النخيل",
            Title = "قهوة مختصة وفطار",
            Description = "مكان هادي للعائلات",
            Address = "شارع الحرية، أمام البنك الأهلي",
            GoogleMapsUrl = "https://maps.google.com/?q=fayoum",
            Phone = "01012345678",
            WhatsApp = "01112345678",
            Email = "cafe@example.com",
            Images = [new FormFile(new MemoryStream([1, 2, 3]), 0, 3, "Images", "photo.jpg")]
        };

    private static Shared.DTOs.Advertisements.UploadImageModel Upload() =>
        new() { FileName = "photo.jpg", ContentType = "image/jpeg", Length = 3, Content = [1, 2, 3] };

    private static string SeedUser(AppDbContext context) => SeedUserEntity(context).GetAwaiter().GetResult().Id;

    private static async Task<ApplicationUser> SeedUserEntity(AppDbContext context)
    {
        var id = Guid.NewGuid().ToString();
        var user = new ApplicationUser
        {
            Id = id, UserName = id, FirstName = "أحمد", SecondName = "علي", PhoneNumber = "01012345678",
            Email = $"{id}@example.com", Governorate = LocationConstants.Governorate, Center = "الفيوم"
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();
        return user;
    }

    private static RestaurantAndCafe Listing(
        string owner, string name, DateTime publishedAt, ModerationStatus status = ModerationStatus.Approved) =>
        new()
        {
            Id = Guid.NewGuid(),
            UserId = owner,
            Name = name,
            Title = name,
            Description = "وصف",
            Address = "العنوان",
            Phone = "01012345678",
            CreatedAt = publishedAt,
            ModerationStatus = status,
            PublishedAt = status == ModerationStatus.Approved ? publishedAt : null,
            FirstPublishedAt = status == ModerationStatus.Approved ? publishedAt : null,
            ExpireAt = status == ModerationStatus.Approved ? publishedAt.AddDays(30) : null
        };
}
