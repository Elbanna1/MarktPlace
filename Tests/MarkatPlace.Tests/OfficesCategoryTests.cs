using AutoMapper;
using Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Persistence.Data;
using Persistence.Listings;
using Persistence.Repositories;
using Services;
using Services.AdForms;
using Services.Lookups;
using Services.Mapping;
using Services.Offices;
using Services.ReadConfigs;
using Services.Validation;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Advertisements;
using Shared.DTOs.Directory;
using Shared.DTOs.Lookups;
using Shared.DTOs.Lookups.Forms;
using Shared.DTOs.Lookups.Read;
using Shared.DTOs.Offices;
using Shared.Enums;
using Shared.Exceptions;
using Shared.Settings;
using Xunit;

namespace MarkatPlace.Tests;

public class OfficesCategoryTests
{
    private const int Offices = 14;

    private const int Accountants = 63;

    public static TheoryData<int, string, string, string, string, string> Modules() => new()
    {
        { 59, "lawyerOffices", "/api/lawyer-offices", "LawyerOffices", "مكاتب محامين", "اسم المكتب أو المحامي" },
        { 60, "engineeringConsultancyOffices", "/api/engineering-consultancy-offices", "EngineeringConsultancyOffices", "مكاتب استشارية هندسية", "اسم المكتب الهندسي" },
        { 61, "contractors", "/api/contractors", "Contractors", "مقاولين (هدم وبناء)", "اسم المكتب أو المقاول" },
        { 62, "industrialConsultancyOffices", "/api/industrial-consultancy-offices", "IndustrialConsultancyOffices", "مكاتب استشارية صناعية", "اسم مكتب الاستشارة الصناعية" },
        { 63, "accountantOffices", "/api/accountant-offices", "AccountantOffices", "مكاتب محاسبين قانونيين", "اسم مكتب المحاسب القانوني أو اسم المحاسب القانوني" }
    };

    public static TheoryData<int> SubCategories() => new() { 59, 60, 61, 62, 63 };

    private static AppDbContext NewContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new AppDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }

    private static IModel SqlServerModel()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer("Server=localhost;Database=ModelOnly;Trusted_Connection=True;")
            .Options;

        using var context = new AppDbContext(options);
        return context.GetService<IDesignTimeModel>().Model;
    }

    private static IMapper Mapper() =>
        new MapperConfiguration(
            configuration => configuration.AddMaps(typeof(OfficesMappingProfile).Assembly),
            NullLoggerFactory.Instance).CreateMapper();

    private static CategorySelectionResolver Resolver(AppDbContext context) => new(new LookupRepository(context));

    private static CreateAdFormService FormService(AppDbContext context)
    {
        var mapper = Mapper();

        return new CreateAdFormService(
            Resolver(context),
            new AdFormBreadcrumbBuilder(Options.Create(new AppSettings())),
            new LookupService(
                new LookupRepository(context), new MemoryCache(new MemoryCacheOptions()), new LookupCache(), mapper),
            new AdminFormRepository(context),
            mapper);
    }

    private static ReadConfigService ReadService(AppDbContext context) => new(Resolver(context), Mapper());

    private static AdvertiserLookupService AdvertiserService(AppDbContext context) =>
        new(Resolver(context), new AdvertiserLookupRepository(context));

    private static DirectoryModule ModuleOf(int subCategoryId) =>
        OfficesCatalog.ModuleOf((SubCategoryType)subCategoryId)!;

    [Fact]
    public void The_category_and_its_five_sub_categories_carry_stable_ids()
    {
        Assert.Equal(14, (int)CategoryType.Offices);
        Assert.Equal(new[] { 59, 60, 61, 62, 63 }, OfficesSubCategories.All.Select(s => (int)s).OrderBy(id => id));
        Assert.Equal(new[] { 90, 91, 92, 93, 94 }, OfficesCatalog.Modules.Select(m => (int)m.Type));
        Assert.All(OfficesCatalog.Modules, module => Assert.Equal(CategoryType.Offices, module.Category));
    }

    [Fact]
    public async Task The_seed_publishes_the_Arabic_category_and_its_sub_categories()
    {
        using var context = NewContext();

        var category = await context.Categories.Include(c => c.SubCategories).SingleAsync(c => c.Id == Offices);

        Assert.Equal("مكاتب", category.NameAr);
        Assert.Equal(
            new[] { "مكاتب محامين", "مكاتب استشارية هندسية", "مقاولين (هدم وبناء)", "مكاتب استشارية صناعية", "مكاتب محاسبين قانونيين" },
            category.SubCategories.OrderBy(s => s.Id).Select(s => s.NameAr));

        var selection = await Resolver(context).ResolveAsync(Offices, null);
        Assert.True(selection.RequiresSubCategory);
        Assert.Equal(5, selection.SubCategories.Count);
    }

    [Theory]
    [MemberData(nameof(SubCategories))]
    public void Every_module_is_registered_in_every_seam_and_has_its_own_types(int subCategoryId)
    {
        var sub = (SubCategoryType)subCategoryId;
        var module = ModuleOf(subCategoryId);

        Assert.Equal(CategoryType.Offices, ListingModuleCatalog.CategoryOf(sub));
        Assert.Equal(module.Type, ListingModuleCatalog.ModuleOf(sub));
        Assert.Equal(module.Route, "api/" + ListingModuleCatalog.RouteOf(module.Type));
        Assert.Equal(module.Type, ListingModuleCatalog.ModuleOfRoute(module.Route["api/".Length..]));
        Assert.Equal(module.EntityName, NotificationCatalog.For(module.Type).EntityName);
        Assert.Contains(module.ImagesFolder, ImageConstants.AllFolders);
        Assert.True(module.NameFilterIsExact);
        Assert.True(module.ImagesRequired);

        var entity = ModerationModuleRegistry.EntityTypes[module.Type];
        Assert.Equal(module.Type.ToString(), entity.Name);

        Assert.Equal(AdvertiserLookupCatalog.NameParameter, AdvertiserLookupCatalog.FilterParameterOf(sub));
        Assert.Contains(sub, AdvertiserLookupRepository.SupportedSubCategories);

        Assert.NotNull(AdFormSchemaCatalog.GetSchema(Offices, subCategoryId));
        Assert.NotNull(ReadConfigCatalog.GetSchema(Offices, subCategoryId));
    }

    [Theory]
    [MemberData(nameof(Modules))]
    public void Every_CRUD_endpoint_has_a_real_controller_action(
        int subCategoryId, string key, string route, string table, string subName, string nameLabel)
    {
        Assert.True(ApiRouteIndex.Exists("POST", route));
        Assert.True(ApiRouteIndex.Exists("GET", route));
        Assert.True(ApiRouteIndex.Exists("GET", route + "/{id}"));
        Assert.True(ApiRouteIndex.Exists("PUT", route + "/{id}"));
        Assert.True(ApiRouteIndex.Exists("DELETE", route + "/{id}"));
    }

    [Theory]
    [MemberData(nameof(Modules))]
    public void The_entity_maps_to_its_own_tables_with_the_expected_columns(
        int subCategoryId, string key, string route, string table, string subName, string nameLabel)
    {
        var model = SqlServerModel();
        var module = ModuleOf(subCategoryId);
        var entity = model.FindEntityType(ModerationModuleRegistry.EntityTypes[module.Type])!;

        Assert.Equal(table, entity.GetTableName());

        AssertColumn(entity, "Name", required: true, maxLength: 150);
        AssertColumn(entity, "Title", required: true, maxLength: 150);
        AssertColumn(entity, "Description", required: true, maxLength: 4000);
        AssertColumn(entity, "Address", required: true, maxLength: 300);
        AssertColumn(entity, "Phone", required: true, maxLength: 20);
        AssertColumn(entity, "GoogleMapsUrl", required: false, maxLength: 1000);
        AssertColumn(entity, "WhatsApp", required: false, maxLength: 20);
        AssertColumn(entity, "Email", required: false, maxLength: 256);

        var owner = entity.GetForeignKeys().Single(fk => fk.PrincipalEntityType.ClrType == typeof(ApplicationUser));
        Assert.Equal(DeleteBehavior.NoAction, owner.DeleteBehavior);

        var images = entity.GetReferencingForeignKeys().Single();
        Assert.Equal(module.Type + "Images", images.DeclaringEntityType.GetTableName());
        Assert.Equal(DeleteBehavior.Cascade, images.DeleteBehavior);

        Assert.Contains(entity.GetIndexes(), index => index.GetDatabaseName() == $"IX_{table}_Advertisers");
    }

    private static void AssertColumn(IEntityType entity, string name, bool required, int maxLength)
    {
        var property = entity.FindProperty(name)!;
        Assert.Equal(!required, property.IsNullable);
        Assert.Equal(maxLength, property.GetMaxLength());
    }

    [Theory]
    [MemberData(nameof(Modules))]
    public async Task Create_ad_form_returns_the_module_schema_in_the_specified_order(
        int subCategoryId, string key, string route, string table, string subName, string nameLabel)
    {
        using var context = NewContext();

        var form = await FormService(context).GetCreateAdFormAsync(Offices, subCategoryId);

        Assert.False(form.RequiresSubCategory);
        Assert.Equal("مكاتب", form.Category.NameAr);
        Assert.Equal(subName, form.SubCategory!.NameAr);
        Assert.Equal(key, form.Module);
        Assert.Equal(route, form.Submit!.Endpoint);

        Assert.Equal(
            new[] { "Name", "Title", "Description", "Images", "Address", "GoogleMapsUrl", "Phone", "WhatsApp", "Email" },
            form.Fields.Select(field => field.Name));

        var fields = form.Fields.ToDictionary(field => field.Name);

        Assert.Equal(nameLabel, fields["Name"].Label);
        Assert.True(fields["Name"].Required);
        Assert.Equal("عنوان الإعلان", fields["Title"].Label);
        Assert.True(fields["Title"].Required);
        Assert.Equal("وصف الإعلان", fields["Description"].Label);
        Assert.True(fields["Images"].Required);
        Assert.True(fields["Images"].Multiple);
        Assert.Equal(FormFieldTypes.Image, fields["Images"].Type);

        Assert.True(fields["Address"].Required);
        Assert.Equal("الشارع، العلامة المميزة…", fields["Address"].Placeholder);
        Assert.False(fields["GoogleMapsUrl"].Required);
        Assert.Equal("https://maps.google.com/…", fields["GoogleMapsUrl"].Placeholder);

        Assert.True(fields["Phone"].Required);
        Assert.Equal("01012345678", fields["Phone"].Placeholder);
        Assert.Equal("من فضلك اكتب رقم موبايل مصري صحيح (مثال: 01012345678).", fields["Phone"].PatternMessage);
        Assert.False(fields["WhatsApp"].Required);
        Assert.Equal(AdvertisementCatalog.EgyptianPhonePattern, fields["WhatsApp"].Pattern);
        Assert.False(fields["Email"].Required);
        Assert.Equal("name@example.com", fields["Email"].Placeholder);
        Assert.Equal("من فضلك اكتب بريد إلكتروني صحيح.", fields["Email"].PatternMessage);
    }

    [Theory]
    [MemberData(nameof(Modules))]
    public async Task Read_config_publishes_the_list_and_a_searchable_name_select(
        int subCategoryId, string key, string route, string table, string subName, string nameLabel)
    {
        using var context = NewContext();

        var config = await ReadService(context).GetReadConfigAsync(Offices, subCategoryId);

        Assert.Equal(key, config.Module);
        Assert.Equal(route, config.List!.Endpoint);
        Assert.Equal(route + "/{id}", config.Details!.Endpoint);

        var parameters = config.List.QueryParameters!.ToDictionary(p => p.Name);
        Assert.Equal(
            new[] { "search", "name", "sortBy", "pageIndex", "pageSize" },
            config.List.QueryParameters!.Select(p => p.Name));

        Assert.Equal(nameLabel, parameters["name"].Label);
        Assert.Equal(ReadOperationKeys.Advertisers, parameters["name"].OptionsSource);
        Assert.Equal(ReadOptionsValues.Name, parameters["name"].OptionsValue);

        var advertisers = config.Operations[ReadOperationKeys.Advertisers];
        Assert.Equal(subCategoryId, advertisers.RouteParameters!.Single(p => p.Name == "subCategoryId").DefaultValue);
        Assert.Equal(Offices, advertisers.RouteParameters!.Single(p => p.Name == "categoryId").DefaultValue);
        Assert.Equal((int)ModuleOf(subCategoryId).Type, config.Operations[ReadOperationKeys.MyListings].Query!["type"]);
    }

    [Theory]
    [InlineData(14, 53, typeof(BadRequestException))]
    [InlineData(13, 59, typeof(BadRequestException))]
    [InlineData(14, 999, typeof(NotFoundException))]
    public async Task Wrong_or_unknown_selections_follow_the_existing_conventions(int category, int sub, Type expected)
    {
        using var context = NewContext();

        await Assert.ThrowsAsync(expected, () => FormService(context).GetCreateAdFormAsync(category, sub));
        await Assert.ThrowsAsync(expected, () => ReadService(context).GetReadConfigAsync(category, sub));
        await Assert.ThrowsAsync(expected, () =>
            AdvertiserService(context).GetAdvertisersAsync(category, sub, new AdvertiserLookupParams()));
    }

    public static TheoryData<IValidator, string> CreateValidators() => new()
    {
        { new CreateLawyerOfficeRequestValidator(), "اسم المكتب أو المحامي مطلوب." },
        { new CreateEngineeringConsultancyOfficeRequestValidator(), "اسم المكتب الهندسي مطلوب." },
        { new CreateContractorRequestValidator(), "اسم المكتب أو المقاول مطلوب." },
        { new CreateIndustrialConsultancyOfficeRequestValidator(), "اسم مكتب الاستشارة الصناعية مطلوب." },
        { new CreateAccountantOfficeRequestValidator(), "اسم مكتب المحاسب القانوني أو اسم المحاسب القانوني مطلوب." }
    };

    [Theory]
    [MemberData(nameof(CreateValidators))]
    public void A_minimal_request_is_valid_and_leaving_optional_fields_blank_is_fine(IValidator validator, string _)
    {
        var request = RequestFor(validator);
        request.WhatsApp = "";
        request.Email = "   ";
        request.GoogleMapsUrl = null;

        var result = validator.Validate(new ValidationContext<object>(request));

        Assert.True(result.IsValid, string.Join(" | ", result.Errors.Select(e => e.ErrorMessage)));
    }

    [Theory]
    [MemberData(nameof(CreateValidators))]
    public void Every_module_names_its_own_required_name_field(IValidator validator, string message)
    {
        var request = RequestFor(validator);
        request.Name = " ";

        var error = Assert.Single(validator.Validate(new ValidationContext<object>(request)).Errors);
        Assert.Equal(nameof(CreateDirectoryListingRequest.Name), error.PropertyName);
        Assert.Equal(message, error.ErrorMessage);
    }

    [Theory]
    [MemberData(nameof(CreateValidators))]
    public void Title_address_phone_and_images_are_required(IValidator validator, string _)
    {
        var request = RequestFor(validator);
        request.Title = "";
        request.Address = "";
        request.Phone = "";
        request.Images.Clear();

        var errors = validator.Validate(new ValidationContext<object>(request)).Errors
            .ToDictionary(e => e.PropertyName, e => e.ErrorMessage);

        Assert.Equal("عنوان الإعلان مطلوب.", errors["Title"]);
        Assert.Equal("العنوان مطلوب.", errors["Address"]);
        Assert.Equal("رقم الهاتف مطلوب.", errors["Phone"]);
        Assert.Equal("لازم ترفع صورة واحدة على الأقل.", errors["Images"]);
        Assert.Equal(4, errors.Count);
    }

    [Theory]
    [InlineData("0101234567")]
    [InlineData("01712345678")]
    [InlineData("+201012345678")]
    public void Phone_and_whatsapp_use_the_Egyptian_mobile_rule(string number)
    {
        var validator = new CreateAccountantOfficeRequestValidator();
        var request = (CreateAccountantOfficeRequest)RequestFor(validator);
        request.Phone = number;
        request.WhatsApp = number;

        var errors = validator.Validate(request).Errors;

        Assert.Equal(2, errors.Count);
        Assert.All(errors, e => Assert.Equal("من فضلك اكتب رقم موبايل مصري صحيح (مثال: 01012345678).", e.ErrorMessage));
    }

    [Fact]
    public void A_bad_email_or_maps_link_is_refused_only_when_supplied()
    {
        var validator = new CreateLawyerOfficeRequestValidator();
        var request = (CreateLawyerOfficeRequest)RequestFor(validator);
        request.Email = "not-an-email";
        request.GoogleMapsUrl = "maps.google.com/x";

        var errors = validator.Validate(request).Errors.ToDictionary(e => e.PropertyName, e => e.ErrorMessage);

        Assert.Equal("من فضلك اكتب بريد إلكتروني صحيح.", errors["Email"]);
        Assert.Equal("لينك خرائط جوجل لازم يبدأ بـ http أو https.", errors["GoogleMapsUrl"]);
    }

    [Fact]
    public void An_update_may_keep_its_existing_images()
    {
        var update = new UpdateContractorRequest
        {
            Name = "مقاول", Title = "عنوان", Description = "وصف", Address = "عنوان", Phone = "01012345678"
        };

        Assert.True(new UpdateContractorRequestValidator().Validate(update).IsValid);
    }

    [Fact]
    public async Task The_accountant_select_lists_every_published_office_name_once_and_searches()
    {
        using var context = NewContext();
        var owner = SeedUser(context);

        context.AccountantOffices.AddRange(
            Listing<AccountantOffice>(owner, "مكتب أحمد للمحاسبة"),
            Listing<AccountantOffice>(owner, "مكتب أحمد للمحاسبة"),
            Listing<AccountantOffice>(owner, "مكتب منى للمراجعة"),
            Listing<AccountantOffice>(owner, "قيد المراجعة", ModerationStatus.Pending));
        context.LawyerOffices.Add(Listing<LawyerOffice>(owner, "مكتب محاماة لا يظهر هنا"));
        await context.SaveChangesAsync();

        var all = await AdvertiserService(context).GetAdvertisersAsync(Offices, Accountants, new AdvertiserLookupParams());

        Assert.Equal(new[] { "مكتب أحمد للمحاسبة", "مكتب منى للمراجعة" }, all.Items.Select(i => i.Name));
        Assert.Equal(2, all.Items[0].ListingsCount);

        var searched = await AdvertiserService(context)
            .GetAdvertisersAsync(Offices, Accountants, new AdvertiserLookupParams { Search = "منى" });
        Assert.Equal("مكتب منى للمراجعة", Assert.Single(searched.Items).Name);
    }

    [Fact]
    public async Task Choosing_an_accountant_returns_only_that_offices_listings_end_to_end()
    {
        using var context = NewContext();
        var owner = SeedUser(context);

        context.AccountantOffices.AddRange(
            Listing<AccountantOffice>(owner, "مكتب أحمد"),
            Listing<AccountantOffice>(owner, "مكتب أحمد"),
            Listing<AccountantOffice>(owner, "مكتب أحمد وشركاه"),
            Listing<AccountantOffice>(owner, "مكتب منى"));
        await context.SaveChangesAsync();

        var options = await AdvertiserService(context).GetAdvertisersAsync(Offices, Accountants, new AdvertiserLookupParams());
        var chosen = options.Items.Single(option => option.Name == "مكتب أحمد");

        var service = Service<AccountantOfficeService>(context);
        var result = await service.GetListAsync(new AccountantOfficeFilterParams { Name = chosen.Name });

        Assert.Equal(chosen.ListingsCount, result.TotalCount);
        Assert.All(result.Items, item => Assert.Equal("مكتب أحمد", item.Name));
        Assert.All(result.Items, item => Assert.Equal("مكاتب", item.CategoryName));
    }

    [Fact]
    public async Task Search_pagination_and_sorting_work_on_an_office_list()
    {
        using var context = NewContext();
        var owner = SeedUser(context);

        for (var i = 1; i <= 5; i++)
        {
            var listing = Listing<EngineeringConsultancyOffice>(owner, $"مكتب {i}");
            listing.CreatedAt = DateTime.UtcNow.AddDays(-i);
            listing.Description = i % 2 == 0 ? "تصميم وإشراف" : "تراخيص";
            context.EngineeringConsultancyOffices.Add(listing);
        }

        context.EngineeringConsultancyOffices.Add(Listing<EngineeringConsultancyOffice>(owner, "مرفوض", ModerationStatus.Rejected));
        await context.SaveChangesAsync();

        var repository = new EngineeringConsultancyOfficeRepository(context);

        var page = await repository.GetPagedAsync(new EngineeringConsultancyOfficeFilterParams { PageIndex = 2, PageSize = 2 });
        Assert.Equal(5, page.TotalCount);
        Assert.Equal(new[] { "مكتب 3", "مكتب 4" }, page.Items.Select(x => x.Name));

        var oldest = await repository.GetPagedAsync(new EngineeringConsultancyOfficeFilterParams { SortBy = DirectorySortBy.Oldest });
        Assert.Equal("مكتب 5", oldest.Items[0].Name);

        var searched = await repository.GetPagedAsync(new EngineeringConsultancyOfficeFilterParams { Search = "إشراف" });
        Assert.Equal(new[] { "مكتب 2", "مكتب 4" }, searched.Items.Select(x => x.Name));

        var none = await repository.GetPagedAsync(new EngineeringConsultancyOfficeFilterParams { Name = "مكتب" });
        Assert.Empty(none.Items);
    }

    [Fact]
    public async Task Each_module_reads_only_its_own_table()
    {
        using var context = NewContext();
        var owner = SeedUser(context);

        context.LawyerOffices.Add(Listing<LawyerOffice>(owner, "الاسم"));
        context.Contractors.Add(Listing<Contractor>(owner, "الاسم"));
        await context.SaveChangesAsync();

        Assert.Equal(1, (await new LawyerOfficeRepository(context).GetPagedAsync(new LawyerOfficeFilterParams { Name = "الاسم" })).TotalCount);
        Assert.Equal(1, (await new ContractorRepository(context).GetPagedAsync(new ContractorFilterParams { Name = "الاسم" })).TotalCount);
        Assert.Equal(0, (await new IndustrialConsultancyOfficeRepository(context).GetPagedAsync(new IndustrialConsultancyOfficeFilterParams())).TotalCount);
    }

    [Fact]
    public async Task Creating_a_listing_stores_its_images_and_classifies_it()
    {
        using var context = NewContext();
        var owner = SeedUser(context);

        var created = await Service<IndustrialConsultancyOfficeService>(context).CreateAsync(
            owner, (CreateIndustrialConsultancyOfficeRequest)RequestFor(new CreateIndustrialConsultancyOfficeRequestValidator()),
            [Upload(), Upload()]);

        Assert.Equal(2, created.Images.Count);
        Assert.True(created.Images.Single(i => i.SortOrder == 0).IsPrimary);
        Assert.StartsWith("https://example.test/uploads/industrial-consultancy-offices/", created.PrimaryImageUrl);
        Assert.Equal(Offices, created.CategoryId);
        Assert.Equal("مكاتب", created.CategoryName);
        Assert.Equal(62, created.SubCategoryId);
        Assert.Equal("مكاتب استشارية صناعية", created.SubCategoryName);
        Assert.Equal(ListingModuleType.IndustrialConsultancyOffice, created.ListingType);
        Assert.Equal("01012345678", created.Phone);
        Assert.Null(created.WhatsApp);

        var stored = await context.IndustrialConsultancyOffices.IgnoreQueryFilters().Include(x => x.Images).SingleAsync();
        Assert.Equal(ModerationStatus.Pending, stored.ModerationStatus);
        Assert.Equal(2, stored.Images.Count);
    }

    [Fact]
    public async Task A_listing_without_images_is_refused_and_only_owner_or_admin_may_delete()
    {
        using var context = NewContext();
        var owner = SeedUser(context);
        var stranger = SeedUser(context);
        var service = Service<LawyerOfficeService>(context);
        var request = (CreateLawyerOfficeRequest)RequestFor(new CreateLawyerOfficeRequestValidator());

        await Assert.ThrowsAsync<BadRequestException>(() => service.CreateAsync(owner, request, []));

        var created = await service.CreateAsync(owner, request, [Upload()]);

        await Assert.ThrowsAsync<NotFoundException>(() => service.DeleteAsync(stranger, created.Id, isAdmin: false));
        await service.DeleteAsync(owner, created.Id, isAdmin: false);

        Assert.True((await context.LawyerOffices.IgnoreQueryFilters().SingleAsync()).IsDeleted);
    }

    private static TService Service<TService>(AppDbContext context) where TService : class
    {
        var mapper = Mapper();
        var notifications = SilentProxy<INotificationService>.Create();
        var files = new StubFileService();

        object service = typeof(TService).Name switch
        {
            nameof(LawyerOfficeService) => new LawyerOfficeService(new LawyerOfficeRepository(context), files, mapper, notifications),
            nameof(AccountantOfficeService) => new AccountantOfficeService(new AccountantOfficeRepository(context), files, mapper, notifications),
            nameof(IndustrialConsultancyOfficeService) => new IndustrialConsultancyOfficeService(new IndustrialConsultancyOfficeRepository(context), files, mapper, notifications),
            _ => throw new ArgumentOutOfRangeException(typeof(TService).Name)
        };

        return (TService)service;
    }

    private static CreateDirectoryListingRequest RequestFor(IValidator validator)
    {
        var requestType = validator.GetType().BaseType!.GetGenericArguments()[0];
        var request = (CreateDirectoryListingRequest)Activator.CreateInstance(requestType)!;

        request.Name = "مكتب";
        request.Title = "عنوان";
        request.Description = "وصف";
        request.Address = "شارع الحرية";
        request.Phone = "01012345678";
        request.Images = [new FormFile(new MemoryStream([1, 2, 3]), 0, 3, "Images", "photo.jpg")];

        return request;
    }

    private static UploadImageModel Upload() =>
        new() { FileName = "photo.jpg", ContentType = "image/jpeg", Length = 3, Content = [1, 2, 3] };

    private static string SeedUser(AppDbContext context)
    {
        var id = Guid.NewGuid().ToString();

        context.Users.Add(new ApplicationUser
        {
            Id = id, UserName = id, FirstName = "أحمد", SecondName = "علي",
            Governorate = LocationConstants.Governorate, Center = "الفيوم"
        });
        context.SaveChanges();

        return id;
    }

    private static T Listing<T>(string owner, string name, ModerationStatus status = ModerationStatus.Approved)
        where T : DirectoryListing, new() =>
        new()
        {
            Id = Guid.NewGuid(),
            UserId = owner,
            Name = name,
            Title = "عنوان",
            Description = "وصف",
            Address = "العنوان",
            Phone = "01012345678",
            ModerationStatus = status,
            PublishedAt = status == ModerationStatus.Approved ? DateTime.UtcNow : null,
            ExpireAt = status == ModerationStatus.Approved ? DateTime.UtcNow.AddDays(30) : null
        };
}
