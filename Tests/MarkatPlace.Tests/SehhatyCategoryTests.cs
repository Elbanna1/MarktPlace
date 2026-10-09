using System.Reflection;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
using Services.ReadConfigs;
using Services.Sehhaty;
using Services.Validation;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Advertisements;
using Shared.DTOs.Lookups;
using Shared.DTOs.Lookups.Forms;
using Shared.DTOs.Lookups.Read;
using Shared.DTOs.Sehhaty;
using Shared.Enums;
using Shared.Exceptions;
using Shared.Settings;
using Xunit;

namespace MarkatPlace.Tests;

public class SehhatyCategoryTests
{
    private const int Sehhaty = 13;

    private static readonly string[] ExpectedSpecialties =
    [
        "القلب والأوعية الدموية", "طب الأطفال", "الجلدية والتجميل", "العظام", "المخ والأعصاب",
        "النساء والتوليد", "طب وجراحة العيون", "طب وجراحة الأسنان", "الباطنة العامة",
        "الأنف والأذن والحنجرة", "المسالك البولية", "الجراحة العامة", "جراحة المخ والأعصاب",
        "الطب النفسي", "العلاج الطبيعي والتأهيل", "الأشعة التشخيصية", "التحاليل الطبية", "طب الأسرة",
        "طب الطوارئ", "الصدر والحساسية", "أمراض الكلى", "الأورام", "أمراض الدم", "الروماتيزم والمناعة",
        "التخدير والعناية المركزة", "التغذية العلاجية", "أمراض الذكورة والعقم", "جراحة القلب والصدر",
        "جراحة التجميل", "جراحة الأوعية الدموية", "أمراض الجهاز الهضمي والكبد", "الغدد الصماء والسكري",
        "طب الشيخوخة", "طب الألم", "طب النوم", "طب الأورام الإشعاعي", "طب نقل الدم", "الطب الرياضي",
        "جراحة العمود الفقري", "جراحة الأطفال", "جراحة الوجه والفكين", "طب الأسنان التجميلي",
        "تقويم الأسنان", "علاج الجذور والأعصاب", "أمراض اللثة", "طب أسنان الأطفال", "طب حديثي الولادة",
        "طب الوراثة", "الطب النووي", "الطب الشرعي", "الصحة المهنية", "طب المجتمع", "الطب الوقائي",
        "طب السفر", "أمراض المناعة والحساسية", "أخرى"
    ];

    public static TheoryData<int, string, string, string, string> Modules() => new()
    {
        { 53, "doctors", "/api/doctors", "الأطباء", "اسم الطبيب" },
        { 54, "medicalLaboratories", "/api/medical-laboratories", "معامل التحاليل الطبية", "اسم المعمل" },
        { 55, "dentalLaboratories", "/api/dental-laboratories", "معامل الأسنان", "اسم معمل الأسنان" },
        { 56, "radiologyLaboratories", "/api/radiology-laboratories", "معامل الأشعة", "اسم معمل الأشعة" },
        { 57, "medicalCenters", "/api/medical-centers", "مراكز طبية", "اسم المركز الطبي" },
        { 58, "hospitals", "/api/hospitals", "مستشفيات", "اسم المستشفى" }
    };

    public static TheoryData<int> SubCategories() => new() { 53, 54, 55, 56, 57, 58 };

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
            configuration => configuration.AddMaps(typeof(SehhatyMappingProfile).Assembly),
            NullLoggerFactory.Instance).CreateMapper();

    private static CategorySelectionResolver Resolver(AppDbContext context) =>
        new(new LookupRepository(context));

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

    [Fact]
    public void The_category_and_its_six_sub_categories_carry_stable_ids()
    {
        Assert.Equal(13, (int)CategoryType.Sehhaty);
        Assert.Equal(
            new[] { 53, 54, 55, 56, 57, 58 },
            SehhatySubCategories.All.Select(sub => (int)sub).OrderBy(id => id));

        Assert.Equal(
            new[] { 80, 81, 82, 83, 84, 85 },
            SehhatyCatalog.Modules.Select(module => (int)module.Type));
    }

    [Fact]
    public async Task The_seed_publishes_the_Arabic_category_and_its_sub_categories()
    {
        using var context = NewContext();

        var category = await context.Categories.Include(c => c.SubCategories)
            .SingleAsync(c => c.Id == Sehhaty);

        Assert.Equal("صحتي", category.NameAr);
        Assert.Equal("Sehhaty", category.Name);
        Assert.Equal(
            new[] { "الأطباء", "معامل التحاليل الطبية", "معامل الأسنان", "معامل الأشعة", "مراكز طبية", "مستشفيات" },
            category.SubCategories.OrderBy(s => s.Id).Select(s => s.NameAr));
    }

    [Fact]
    public async Task The_seed_is_exactly_the_category_catalog()
    {
        using var context = NewContext();

        var categories = await context.Categories.AsNoTracking().OrderBy(c => c.Id)
            .Select(c => new { c.Id, c.Name, c.NameAr }).ToListAsync();

        Assert.Equal(
            CategoryCatalog.Categories.Select(c => new { Id = (int)c.Id, c.Name, c.NameAr }),
            categories);

        var subCategories = await context.SubCategories.AsNoTracking().OrderBy(s => s.Id)
            .Select(s => new { s.Id, s.CategoryId, s.Name, s.NameAr }).ToListAsync();

        Assert.Equal(
            CategoryCatalog.SubCategories.OrderBy(s => s.Id)
                .Select(s => new { Id = (int)s.Id, CategoryId = (int)s.Category, s.Name, s.NameAr }),
            subCategories);
    }

    [Fact]
    public void Every_enum_member_is_in_the_category_catalog_exactly_once()
    {
        Assert.Equal(
            Enum.GetValues<CategoryType>().OrderBy(c => c),
            CategoryCatalog.Categories.Select(c => c.Id).OrderBy(c => c));

        Assert.Equal(
            Enum.GetValues<SubCategoryType>().OrderBy(s => s),
            CategoryCatalog.SubCategories.Select(s => s.Id).OrderBy(s => s));
    }

    [Theory]
    [MemberData(nameof(SubCategories))]
    public void Every_module_is_registered_in_every_shared_seam(int subCategoryId)
    {
        var sub = (SubCategoryType)subCategoryId;
        var module = SehhatyCatalog.ModuleOf(sub)!;

        Assert.Equal(CategoryType.Sehhaty, ListingModuleCatalog.CategoryOf(sub));
        Assert.Equal(module.Type, ListingModuleCatalog.ModuleOf(sub));
        Assert.Equal(sub, ListingModuleCatalog.SubCategoryOf(module.Type));
        Assert.Equal(module.Route, "api/" + ListingModuleCatalog.RouteOf(module.Type));
        Assert.Equal(module.Type, ListingModuleCatalog.ModuleOfRoute(module.Route["api/".Length..]));
        Assert.Equal(module.EntityName, NotificationCatalog.For(module.Type).EntityName);
        Assert.True(ModerationModuleRegistry.EntityTypes.ContainsKey(module.Type));
        Assert.Contains(module.ImagesFolder, ImageConstants.AllFolders);
        Assert.True(ApiRouteIndex.Exists("POST", module.PublicRoute));
        Assert.True(ApiRouteIndex.Exists("GET", module.PublicRoute + "/{id}"));
        Assert.True(ApiRouteIndex.Exists("PUT", module.PublicRoute + "/{id}"));
        Assert.True(ApiRouteIndex.Exists("DELETE", module.PublicRoute + "/{id}"));
    }

    [Fact]
    public async Task The_resolver_accepts_the_category_alone_and_with_each_sub_category()
    {
        using var context = NewContext();
        var resolver = Resolver(context);

        var categoryOnly = await resolver.ResolveAsync(Sehhaty, null);
        Assert.True(categoryOnly.RequiresSubCategory);
        Assert.Equal(6, categoryOnly.SubCategories.Count);

        foreach (var sub in SehhatySubCategories.All)
        {
            var selection = await resolver.ResolveAsync(Sehhaty, (int)sub);
            Assert.Equal((int)sub, selection.SubCategory!.Id);
            Assert.False(selection.RequiresSubCategory);
        }
    }

    [Theory]
    [MemberData(nameof(Modules))]
    public async Task Create_ad_form_returns_the_module_schema(
        int subCategoryId, string module, string submit, string subCategoryName, string nameLabel)
    {
        using var context = NewContext();

        var form = await FormService(context).GetCreateAdFormAsync(Sehhaty, subCategoryId);

        Assert.False(form.RequiresSubCategory);
        Assert.Equal(Sehhaty, form.Category.Id);
        Assert.Equal("صحتي", form.Category.NameAr);
        Assert.Equal(subCategoryId, form.SubCategory!.Id);
        Assert.Equal(subCategoryName, form.SubCategory.NameAr);
        Assert.Equal(module, form.Module);
        Assert.Equal(submit, form.Submit.Endpoint);

        var fields = form.Fields.ToDictionary(field => field.Name);

        AssertField(fields["Name"], nameLabel, FormFieldTypes.Text, required: true);
        Assert.False(string.IsNullOrWhiteSpace(fields["Name"].Placeholder));

        AssertField(fields["Title"], "عنوان الإعلان", FormFieldTypes.Text, required: true);
        Assert.Equal("اكتب عنوانًا واضحًا ومختصرًا", fields["Title"].Placeholder);

        AssertField(fields["Description"], "وصف الإعلان", FormFieldTypes.TextArea, required: true);
        Assert.StartsWith("اذكر في الوصف", fields["Description"].Placeholder);

        AssertField(fields["Images"], "الصور", FormFieldTypes.Image, required: true);
        Assert.Equal(1, fields["Images"].MinItems);
        Assert.Equal(ImageConstants.MaxImagesPerItem, fields["Images"].MaxFiles);

        AssertField(fields["Phone"], "رقم الهاتف", FormFieldTypes.Text, required: true);
        Assert.Equal("01012345678", fields["Phone"].Placeholder);
        Assert.Equal(AdvertisementCatalog.EgyptianPhonePattern, fields["Phone"].Pattern);
        Assert.Equal("من فضلك اكتب رقم موبايل مصري صحيح (مثال: 01012345678).", fields["Phone"].PatternMessage);

        AssertField(fields["WhatsApp"], "رقم الواتساب", FormFieldTypes.Text, required: false);
        Assert.Equal("01012345678", fields["WhatsApp"].Placeholder);
        Assert.Equal(AdvertisementCatalog.EgyptianPhonePattern, fields["WhatsApp"].Pattern);

        AssertField(fields["Email"], "البريد الإلكتروني", FormFieldTypes.Text, required: false);
        Assert.Equal("name@example.com", fields["Email"].Placeholder);
        Assert.Equal("من فضلك اكتب بريد إلكتروني صحيح.", fields["Email"].PatternMessage);

        AssertField(fields["Address"], "العنوان", FormFieldTypes.Text, required: true);
        Assert.Equal("الشارع، العلامة المميزة…", fields["Address"].Placeholder);

        AssertField(fields["GoogleMapsUrl"], "رابط الموقع على خرائط جوجل", FormFieldTypes.Text, required: false);
        Assert.Equal("https://maps.google.com/…", fields["GoogleMapsUrl"].Placeholder);

        Assert.Equal(
            Enumerable.Range(1, form.Fields.Count),
            form.Fields.Select(field => field.Order));

        var expected = subCategoryId == 53
            ? new[] { "Name", "Specialty", "OtherSpecialty", "Address", "GoogleMapsUrl", "Phone", "WhatsApp", "Email", "Title", "Description", "Images" }
            : new[] { "Name", "Title", "Description", "Images", "Phone", "WhatsApp", "Email", "Address", "GoogleMapsUrl" };

        Assert.Equal(expected, form.Fields.Select(field => field.Name));
    }

    [Fact]
    public async Task The_doctor_specialty_is_a_required_select_with_the_exact_Arabic_options()
    {
        using var context = NewContext();

        var form = await FormService(context).GetCreateAdFormAsync(Sehhaty, 53);
        var specialty = form.Fields.Single(field => field.Name == "Specialty");

        AssertField(specialty, "تخصص الطبيب", FormFieldTypes.Select, required: true);
        Assert.Equal(ExpectedSpecialties, specialty.Options!.Select(option => option.Label));
        Assert.Equal(56, specialty.Options!.Count);
        Assert.Equal(Enumerable.Range(1, 55).Cast<object>().Append(99), specialty.Options.Select(o => o.Value));
        Assert.True(specialty.Searchable);
    }

    [Fact]
    public async Task The_custom_specialty_field_appears_and_becomes_mandatory_only_for_Other()
    {
        using var context = NewContext();

        var form = await FormService(context).GetCreateAdFormAsync(Sehhaty, 53);
        var other = form.Fields.Single(field => field.Name == "OtherSpecialty");

        Assert.False(other.Required);
        Assert.False(other.Visible);
        Assert.Equal("Specialty", other.VisibleWhen!.Field);
        Assert.Equal(new object[] { 99 }, other.VisibleWhen.Values);
        Assert.Equal("Specialty", other.RequiredWhen!.Field);
        Assert.Equal(new object[] { 99 }, other.RequiredWhen.Values);
    }

    [Theory]
    [InlineData(54)]
    [InlineData(55)]
    [InlineData(56)]
    [InlineData(57)]
    [InlineData(58)]
    public async Task Facility_forms_carry_no_specialty(int subCategoryId)
    {
        using var context = NewContext();

        var form = await FormService(context).GetCreateAdFormAsync(Sehhaty, subCategoryId);

        Assert.DoesNotContain(form.Fields, field => field.Name.Contains("Specialty", StringComparison.Ordinal));
    }

    [Fact]
    public async Task Create_ad_form_for_the_category_alone_asks_for_one_of_the_six_sub_categories()
    {
        using var context = NewContext();

        var form = await FormService(context).GetCreateAdFormAsync(Sehhaty);

        Assert.True(form.RequiresSubCategory);
        Assert.Equal(
            new[] { 53, 54, 55, 56, 57, 58 },
            form.Lookups.SubCategories!.Select(sub => sub.Id).OrderBy(id => id));
    }

    public static TheoryData<int, int?, Type> InvalidSelections() => new()
    {
        { 999, 53, typeof(NotFoundException) },
        { Sehhaty, 999, typeof(NotFoundException) },
        { Sehhaty, 48, typeof(BadRequestException) },
        { 12, 53, typeof(BadRequestException) },
        { 0, null, typeof(NotFoundException) }
    };

    [Theory]
    [MemberData(nameof(InvalidSelections))]
    public async Task Invalid_selections_follow_the_existing_error_conventions_on_all_three_endpoints(
        int categoryId, int? subCategoryId, Type expected)
    {
        using var context = NewContext();

        await Assert.ThrowsAsync(expected, () => FormService(context).GetCreateAdFormAsync(categoryId, subCategoryId));
        await Assert.ThrowsAsync(expected, () => ReadService(context).GetReadConfigAsync(categoryId, subCategoryId));

        if (subCategoryId is { } id)
        {
            await Assert.ThrowsAsync(expected, () =>
                AdvertiserService(context).GetAdvertisersAsync(categoryId, id, new AdvertiserLookupParams()));
        }
    }

    [Theory]
    [InlineData(12, 53)]
    [InlineData(13, 48)]
    [InlineData(6, 53)]
    [InlineData(13, null)]
    [InlineData(13, 999)]
    public void The_static_catalogs_refuse_a_sub_category_under_the_wrong_parent(int categoryId, int? subCategoryId)
    {
        Assert.Null(AdFormSchemaCatalog.GetSchema(categoryId, subCategoryId));
        Assert.Null(ReadConfigCatalog.GetSchema(categoryId, subCategoryId));
    }

    [Theory]
    [MemberData(nameof(Modules))]
    public async Task Read_config_publishes_the_module_routes_and_its_name_filter(
        int subCategoryId, string module, string route, string subCategoryName, string nameLabel)
    {
        using var context = NewContext();

        var config = await ReadService(context).GetReadConfigAsync(Sehhaty, subCategoryId);

        Assert.False(config.RequiresSubCategory);
        Assert.Equal(module, config.Module);
        Assert.Equal(subCategoryName, config.SubCategory!.NameAr);
        Assert.Equal(route, config.List!.Endpoint);
        Assert.Equal(route + "/{id}", config.Details!.Endpoint);
        Assert.True(config.List.Paginated);

        var parameters = config.List.QueryParameters!.ToDictionary(parameter => parameter.Name);

        Assert.Equal(nameLabel, parameters["name"].Label);
        Assert.Equal(ReadOperationKeys.Advertisers, parameters["name"].OptionsSource);
        Assert.Equal(ReadOptionsValues.Name, parameters["name"].OptionsValue);
        Assert.Contains("search", parameters.Keys);
        Assert.Equal(
            new object[] { 1, 2, 3 },
            parameters["sortBy"].Options!.Select(option => option.Value));
        Assert.Contains("pageIndex", parameters.Keys);
        Assert.Contains("pageSize", parameters.Keys);
        Assert.Equal(subCategoryId == 53, parameters.ContainsKey("specialty"));

        var advertisers = config.Operations[ReadOperationKeys.Advertisers];
        Assert.Equal("/api/lookups/advertisers/{categoryId}/{subCategoryId}", advertisers.Endpoint);
        Assert.Equal(Sehhaty, advertisers.RouteParameters!.Single(p => p.Name == "categoryId").DefaultValue);
        Assert.Equal(subCategoryId, advertisers.RouteParameters!.Single(p => p.Name == "subCategoryId").DefaultValue);

        var myListings = config.Operations[ReadOperationKeys.MyListings];
        Assert.Equal((int)SehhatyCatalog.ModuleOf((SubCategoryType)subCategoryId)!.Type, myListings.Query!["type"]);

        Assert.Equal(Sehhaty, config.Operations[ReadOperationKeys.CreateForm].RouteParameters!
            .Single(p => p.Name == "categoryId").DefaultValue);
        Assert.Contains("listingActions", config.Operations.Keys);
        Assert.Contains("favorites", config.Operations.Keys);
    }

    [Fact]
    public async Task Read_config_for_doctors_points_the_specialty_filter_at_a_live_lookup()
    {
        using var context = NewContext();

        var config = await ReadService(context).GetReadConfigAsync(Sehhaty, 53);

        var specialty = config.List!.QueryParameters!.Single(p => p.Name == "specialty");
        Assert.Equal("تخصص الطبيب", specialty.Label);
        Assert.Equal(ReadOperationKeys.DoctorSpecialties, specialty.OptionsSource);
        Assert.Equal(ReadOptionsValues.Id, specialty.OptionsValue);

        var lookup = config.Operations[ReadOperationKeys.DoctorSpecialties];
        Assert.Equal("/api/doctors/specialties", lookup.Endpoint);
        Assert.True(ApiRouteIndex.Exists("GET", lookup.Endpoint));
    }

    [Fact]
    public void The_my_listings_type_options_cover_every_listing_module()
    {
        var schema = ReadConfigCatalog.GetSchema(Sehhaty, 53)!;
        var type = schema.Find(ReadOperationKeys.MyListings)!.QueryParameters!.Single(p => p.Name == "type");

        Assert.Equal(
            Enum.GetValues<ListingModuleType>().Select(value => (object)(int)value).OrderBy(value => (int)value),
            type.Options!.Select(option => option.Value).OrderBy(value => (int)value));
    }

    [Theory]
    [MemberData(nameof(SubCategories))]
    public async Task Advertisers_answer_with_an_empty_page_for_a_new_module(int subCategoryId)
    {
        using var context = NewContext();

        var result = await AdvertiserService(context)
            .GetAdvertisersAsync(Sehhaty, subCategoryId, new AdvertiserLookupParams());

        Assert.Empty(result.Items);
        Assert.Equal(0, result.TotalCount);
    }

    [Fact]
    public async Task Advertisers_list_each_published_doctor_name_once_with_its_count()
    {
        using var context = NewContext();
        var owner = SeedUser(context);

        context.Doctors.AddRange(
            Doctor(owner, "د. أحمد"),
            Doctor(owner, "د. أحمد"),
            Doctor(owner, "د. سارة"),
            Doctor(owner, "قيد المراجعة", ModerationStatus.Pending));
        await context.SaveChangesAsync();

        var result = await AdvertiserService(context)
            .GetAdvertisersAsync(Sehhaty, 53, new AdvertiserLookupParams());

        Assert.Equal(2, result.TotalCount);
        Assert.Equal(2, result.Items.Single(item => item.Name == "د. أحمد").ListingsCount);
        Assert.Equal(1, result.Items.Single(item => item.Name == "د. سارة").ListingsCount);

        var searched = await AdvertiserService(context)
            .GetAdvertisersAsync(Sehhaty, 53, new AdvertiserLookupParams { Search = "سارة" });
        Assert.Equal("د. سارة", Assert.Single(searched.Items).Name);
    }

    [Fact]
    public async Task Advertisers_for_a_facility_read_its_own_table()
    {
        using var context = NewContext();
        var owner = SeedUser(context);

        context.Hospitals.Add(Facility<Hospital>(owner, "مستشفى الفيوم التخصصي"));
        context.MedicalCenters.Add(Facility<MedicalCenter>(owner, "مركز الرحمة"));
        await context.SaveChangesAsync();

        var hospitals = await AdvertiserService(context).GetAdvertisersAsync(Sehhaty, 58, new AdvertiserLookupParams());
        var labs = await AdvertiserService(context).GetAdvertisersAsync(Sehhaty, 54, new AdvertiserLookupParams());

        Assert.Equal("مستشفى الفيوم التخصصي", Assert.Single(hospitals.Items).Name);
        Assert.Empty(labs.Items);
    }

    [Fact]
    public async Task The_doctor_list_filters_by_name_specialty_and_custom_specialty_search()
    {
        using var context = NewContext();
        var owner = SeedUser(context);

        var cardiology = Doctor(owner, "د. أحمد محمود");
        var pediatrics = Doctor(owner, "د. منى علي", specialty: DoctorSpecialty.Pediatrics);
        var other = Doctor(owner, "د. خالد", specialty: DoctorSpecialty.Other, otherSpecialty: "طب الأعشاب");
        var pending = Doctor(owner, "د. أحمد المعلق", ModerationStatus.Pending);
        var deleted = Doctor(owner, "د. أحمد المحذوف");
        deleted.IsDeleted = true;

        context.Doctors.AddRange(cardiology, pediatrics, other, pending, deleted);
        await context.SaveChangesAsync();

        var repository = new DoctorRepository(context);

        var byName = await repository.GetPagedAsync(new DoctorFilterParams { Name = "أحمد" });
        Assert.Equal(new[] { cardiology.Id }, byName.Items.Select(x => x.Id));

        var bySpecialty = await repository.GetPagedAsync(
            new DoctorFilterParams { Specialty = DoctorSpecialty.Pediatrics });
        Assert.Equal(new[] { pediatrics.Id }, bySpecialty.Items.Select(x => x.Id));

        var byOther = await repository.GetPagedAsync(new DoctorFilterParams { Specialty = DoctorSpecialty.Other });
        Assert.Equal(new[] { other.Id }, byOther.Items.Select(x => x.Id));

        var byCustomText = await repository.GetPagedAsync(new DoctorFilterParams { Search = "الأعشاب" });
        Assert.Equal(new[] { other.Id }, byCustomText.Items.Select(x => x.Id));

        var everything = await repository.GetPagedAsync(new DoctorFilterParams());
        Assert.Equal(3, everything.TotalCount);
    }

    [Fact]
    public async Task A_wildcard_in_the_name_filter_is_matched_literally()
    {
        using var context = NewContext();
        var owner = SeedUser(context);

        context.Hospitals.AddRange(
            Facility<Hospital>(owner, "مستشفى 100%"),
            Facility<Hospital>(owner, "مستشفى الأمل"));
        await context.SaveChangesAsync();

        var repository = new HospitalRepository(context);

        var literal = await repository.GetPagedAsync(new HospitalFilterParams { Name = "%" });
        Assert.Equal("مستشفى 100%", Assert.Single(literal.Items).Name);

        var underscore = await repository.GetPagedAsync(new HospitalFilterParams { Name = "_" });
        Assert.Empty(underscore.Items);
    }

    [Theory]
    [InlineData(54)]
    [InlineData(55)]
    [InlineData(56)]
    [InlineData(57)]
    [InlineData(58)]
    public async Task Each_facility_list_filters_by_its_own_name(int subCategoryId)
    {
        using var context = NewContext();
        var owner = SeedUser(context);

        context.MedicalLaboratories.Add(Facility<MedicalLaboratory>(owner, "الشفاء"));
        context.DentalLaboratories.Add(Facility<DentalLaboratory>(owner, "الشفاء"));
        context.RadiologyLaboratories.Add(Facility<RadiologyLaboratory>(owner, "الشفاء"));
        context.MedicalCenters.Add(Facility<MedicalCenter>(owner, "الشفاء"));
        context.Hospitals.Add(Facility<Hospital>(owner, "الشفاء"));
        context.Hospitals.Add(Facility<Hospital>(owner, "النور"));
        await context.SaveChangesAsync();

        int matches = subCategoryId switch
        {
            54 => (await new MedicalLaboratoryRepository(context).GetPagedAsync(new MedicalLaboratoryFilterParams { Name = "الشفاء" })).TotalCount,
            55 => (await new DentalLaboratoryRepository(context).GetPagedAsync(new DentalLaboratoryFilterParams { Name = "الشفاء" })).TotalCount,
            56 => (await new RadiologyLaboratoryRepository(context).GetPagedAsync(new RadiologyLaboratoryFilterParams { Name = "الشفاء" })).TotalCount,
            57 => (await new MedicalCenterRepository(context).GetPagedAsync(new MedicalCenterFilterParams { Name = "الشفاء" })).TotalCount,
            _ => (await new HospitalRepository(context).GetPagedAsync(new HospitalFilterParams { Name = "الشفاء" })).TotalCount
        };

        Assert.Equal(1, matches);
    }

    [Fact]
    public void A_complete_doctor_request_is_valid()
    {
        var result = new CreateDoctorRequestValidator().Validate(ValidDoctor());

        Assert.True(result.IsValid, string.Join(" | ", result.Errors.Select(e => e.ErrorMessage)));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Other_requires_a_non_blank_custom_specialty(string? otherSpecialty)
    {
        var request = ValidDoctor();
        request.Specialty = DoctorSpecialty.Other;
        request.OtherSpecialty = otherSpecialty;

        var result = new CreateDoctorRequestValidator().Validate(request);

        var error = Assert.Single(result.Errors);
        Assert.Equal(nameof(CreateDoctorRequest.OtherSpecialty), error.PropertyName);
        Assert.Equal("من فضلك اكتب تخصص الطبيب لما تختار «أخرى».", error.ErrorMessage);
    }

    [Fact]
    public void Other_with_a_custom_specialty_is_valid_on_create_and_update()
    {
        var create = ValidDoctor();
        create.Specialty = DoctorSpecialty.Other;
        create.OtherSpecialty = "طب الأعشاب";

        Assert.True(new CreateDoctorRequestValidator().Validate(create).IsValid);

        var update = new UpdateDoctorRequest
        {
            Name = create.Name, Title = create.Title, Description = create.Description,
            Address = create.Address, Phone = create.Phone,
            Specialty = DoctorSpecialty.Other, OtherSpecialty = "   "
        };

        Assert.Contains(new UpdateDoctorRequestValidator().Validate(update).Errors,
            error => error.PropertyName == nameof(UpdateDoctorRequest.OtherSpecialty));
    }

    [Fact]
    public void A_custom_specialty_never_replaces_the_specialty_itself()
    {
        var request = ValidDoctor();
        request.Specialty = null;
        request.OtherSpecialty = "طب الأعشاب";

        var result = new CreateDoctorRequestValidator().Validate(request);

        Assert.Contains(result.Errors, error =>
            error.PropertyName == nameof(CreateDoctorRequest.Specialty) &&
            error.ErrorMessage == "تخصص الطبيب مطلوب.");
    }

    [Fact]
    public void An_unknown_specialty_number_is_refused()
    {
        var request = ValidDoctor();
        request.Specialty = (DoctorSpecialty)500;

        Assert.Contains(new CreateDoctorRequestValidator().Validate(request).Errors,
            error => error.PropertyName == nameof(CreateDoctorRequest.Specialty));
    }

    public static TheoryData<IValidator, string> FacilityValidators() => new()
    {
        { new CreateMedicalLaboratoryRequestValidator(), "اسم المعمل مطلوب." },
        { new CreateDentalLaboratoryRequestValidator(), "اسم معمل الأسنان مطلوب." },
        { new CreateRadiologyLaboratoryRequestValidator(), "اسم معمل الأشعة مطلوب." },
        { new CreateMedicalCenterRequestValidator(), "اسم المركز الطبي مطلوب." },
        { new CreateHospitalRequestValidator(), "اسم المستشفى مطلوب." },
        { new CreateDoctorRequestValidator(), "اسم الطبيب مطلوب." }
    };

    [Theory]
    [MemberData(nameof(FacilityValidators))]
    public void Every_module_names_its_own_required_name_field(IValidator validator, string message)
    {
        var request = InstanceFor(validator);
        request.Name = " ";

        var errors = validator.Validate(new ValidationContext<object>(request)).Errors;

        var error = Assert.Single(errors);
        Assert.Equal(nameof(CreateSehhatyListingRequest.Name), error.PropertyName);
        Assert.Equal(message, error.ErrorMessage);
    }

    [Theory]
    [InlineData("0101234567")]
    [InlineData("01712345678")]
    [InlineData("+201012345678")]
    [InlineData("phone")]
    public void An_invalid_phone_or_whatsapp_gets_the_Egyptian_mobile_message(string number)
    {
        var request = ValidDoctor();
        request.Phone = number;
        request.WhatsApp = number;

        var errors = new CreateDoctorRequestValidator().Validate(request).Errors;

        Assert.Equal(2, errors.Count);
        Assert.All(errors, error =>
            Assert.Equal("من فضلك اكتب رقم موبايل مصري صحيح (مثال: 01012345678).", error.ErrorMessage));
    }

    [Fact]
    public void The_phone_is_required_while_whatsapp_email_and_maps_are_optional()
    {
        var request = ValidDoctor();
        request.Phone = "";
        request.WhatsApp = null;
        request.Email = "";
        request.GoogleMapsUrl = "  ";

        var error = Assert.Single(new CreateDoctorRequestValidator().Validate(request).Errors);
        Assert.Equal(nameof(CreateDoctorRequest.Phone), error.PropertyName);
        Assert.Equal("رقم الهاتف مطلوب.", error.ErrorMessage);
    }

    [Fact]
    public void A_malformed_email_or_maps_link_is_refused_in_Arabic()
    {
        var request = ValidDoctor();
        request.Email = "not-an-email";
        request.GoogleMapsUrl = "ftp://maps.google.com/x";

        var errors = new CreateDoctorRequestValidator().Validate(request).Errors
            .ToDictionary(error => error.PropertyName, error => error.ErrorMessage);

        Assert.Equal("من فضلك اكتب بريد إلكتروني صحيح.", errors[nameof(CreateDoctorRequest.Email)]);
        Assert.Equal("لينك خرائط جوجل لازم يبدأ بـ http أو https.", errors[nameof(CreateDoctorRequest.GoogleMapsUrl)]);
    }

    [Fact]
    public void A_create_needs_an_image_but_an_update_may_keep_the_existing_ones()
    {
        var create = ValidDoctor();
        create.Images.Clear();

        var error = Assert.Single(new CreateDoctorRequestValidator().Validate(create).Errors);
        Assert.Equal("لازم ترفع صورة واحدة على الأقل.", error.ErrorMessage);

        var update = new UpdateHospitalRequest
        {
            Name = "مستشفى", Title = "عنوان", Description = "وصف", Address = "عنوان", Phone = "01012345678"
        };

        Assert.True(new UpdateHospitalRequestValidator().Validate(update).IsValid);
    }

    [Fact]
    public void Too_many_images_are_refused()
    {
        var request = ValidDoctor();
        request.Images = Enumerable.Range(0, ImageConstants.MaxImagesPerItem + 1).Select(_ => Image()).ToList();

        Assert.Contains(new CreateDoctorRequestValidator().Validate(request).Errors,
            error => error.PropertyName == nameof(CreateDoctorRequest.Images));
    }

    [Fact]
    public async Task Creating_a_doctor_stores_the_specialty_and_the_custom_value_without_ambiguity()
    {
        using var context = NewContext();
        var owner = SeedUser(context);
        var service = DoctorService(context);

        var request = ValidDoctor();
        request.Specialty = DoctorSpecialty.Other;
        request.OtherSpecialty = "  طب الأعشاب  ";

        var created = await service.CreateAsync(owner, request, [Upload()]);

        Assert.Equal(DoctorSpecialty.Other, created.Specialty);
        Assert.Equal("أخرى", created.SpecialtyName);
        Assert.Equal("طب الأعشاب", created.OtherSpecialty);
        Assert.Equal("طب الأعشاب", created.DisplaySpecialty);
        Assert.Equal(Sehhaty, created.CategoryId);
        Assert.Equal("صحتي", created.CategoryName);
        Assert.Equal(53, created.SubCategoryId);
        Assert.Equal("الأطباء", created.SubCategoryName);
        Assert.Equal(ListingModuleType.Doctor, created.ListingType);
        Assert.Single(created.Images);

        var stored = await context.Doctors.IgnoreQueryFilters().SingleAsync();
        Assert.Equal(ModerationStatus.Pending, stored.ModerationStatus);
        Assert.Equal("طب الأعشاب", stored.OtherSpecialty);
    }

    [Fact]
    public async Task A_custom_specialty_is_dropped_when_the_specialty_is_not_Other()
    {
        using var context = NewContext();
        var owner = SeedUser(context);
        var service = DoctorService(context);

        var request = ValidDoctor();
        request.Specialty = DoctorSpecialty.Pediatrics;
        request.OtherSpecialty = "طب الأعشاب";

        var created = await service.CreateAsync(owner, request, [Upload()]);

        Assert.Null(created.OtherSpecialty);
        Assert.Equal("طب الأطفال", created.DisplaySpecialty);
        Assert.Null((await context.Doctors.IgnoreQueryFilters().SingleAsync()).OtherSpecialty);
    }

    [Fact]
    public async Task The_service_refuses_a_listing_without_images_even_if_validation_is_bypassed()
    {
        using var context = NewContext();
        var owner = SeedUser(context);

        var error = await Assert.ThrowsAsync<BadRequestException>(() =>
            DoctorService(context).CreateAsync(owner, ValidDoctor(), []));

        Assert.Equal("لازم ترفع صورة واحدة على الأقل.", error.Message);
        Assert.Empty(await context.Doctors.IgnoreQueryFilters().ToListAsync());
    }

    [Fact]
    public async Task Only_the_owner_or_an_admin_may_change_a_listing()
    {
        using var context = NewContext();
        var owner = SeedUser(context);
        var stranger = SeedUser(context);
        var service = DoctorService(context);

        var created = await service.CreateAsync(owner, ValidDoctor(), [Upload()]);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            service.DeleteAsync(stranger, created.Id, isAdmin: false));

        await service.DeleteAsync(stranger, created.Id, isAdmin: true);

        Assert.True((await context.Doctors.IgnoreQueryFilters().SingleAsync()).IsDeleted);
    }

    [Fact]
    public async Task An_update_may_not_remove_the_last_image()
    {
        using var context = NewContext();
        var owner = SeedUser(context);
        var service = DoctorService(context);

        var created = await service.CreateAsync(owner, ValidDoctor(), [Upload()]);

        var update = new UpdateDoctorRequest
        {
            Name = "د. أحمد", Title = "عنوان", Description = "وصف", Address = "عنوان",
            Phone = "01012345678", Specialty = DoctorSpecialty.Cardiology,
            RemoveImageIds = [created.Images.Single().Id]
        };

        await Assert.ThrowsAsync<BadRequestException>(() =>
            service.UpdateAsync(owner, isAdmin: false, created.Id, update, []));
    }

    private static void AssertField(FormFieldDto field, string label, string type, bool required)
    {
        Assert.Equal(label, field.Label);
        Assert.Equal(type, field.Type);
        Assert.Equal(required, field.Required);
    }

    private static CreateDoctorRequest ValidDoctor() =>
        new()
        {
            Name = "د. أحمد محمود",
            Specialty = DoctorSpecialty.Cardiology,
            Title = "عيادة القلب",
            Description = "كشف واستشارات",
            Address = "شارع الحرية",
            GoogleMapsUrl = "https://maps.google.com/?q=fayoum",
            Phone = "01012345678",
            WhatsApp = "01112345678",
            Email = "doctor@example.com",
            Images = [Image()]
        };

    private static CreateSehhatyListingRequest InstanceFor(IValidator validator)
    {
        var requestType = validator.GetType().BaseType!.GetGenericArguments()[0];
        var request = (CreateSehhatyListingRequest)Activator.CreateInstance(requestType)!;

        request.Name = "اسم";
        request.Title = "عنوان";
        request.Description = "وصف";
        request.Address = "عنوان";
        request.Phone = "01012345678";
        request.Images = [Image()];

        if (request is CreateDoctorRequest doctor)
            doctor.Specialty = DoctorSpecialty.Cardiology;

        return request;
    }

    private static IFormFile Image() =>
        new FormFile(new MemoryStream([1, 2, 3]), 0, 3, "Images", "photo.jpg");

    private static UploadImageModel Upload() =>
        new() { FileName = "photo.jpg", ContentType = "image/jpeg", Length = 3, Content = [1, 2, 3] };

    private static string SeedUser(AppDbContext context)
    {
        var id = Guid.NewGuid().ToString();

        context.Users.Add(new ApplicationUser
        {
            Id = id,
            UserName = id,
            FirstName = "أحمد",
            SecondName = "علي",
            Governorate = LocationConstants.Governorate,
            Center = "الفيوم"
        });
        context.SaveChanges();

        return id;
    }

    private static Doctor Doctor(
        string owner, string name, ModerationStatus status = ModerationStatus.Approved,
        DoctorSpecialty specialty = DoctorSpecialty.Cardiology, string? otherSpecialty = null)
    {
        var doctor = Facility<Doctor>(owner, name, status);
        doctor.Specialty = specialty;
        doctor.OtherSpecialty = otherSpecialty;
        return doctor;
    }

    private static T Facility<T>(string owner, string name, ModerationStatus status = ModerationStatus.Approved)
        where T : SehhatyListing, new() =>
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

    private static DoctorService DoctorService(AppDbContext context) =>
        new(new DoctorRepository(context), new StubFileService(), Mapper(), SilentProxy<INotificationService>.Create());

    private sealed class StubFileService : IFileService
    {
        public Task<StoredFile> SaveAsync(UploadImageModel file, string folder, CancellationToken cancellationToken = default) =>
            Task.FromResult(Stored(file, folder));

        public Task<IReadOnlyList<StoredFile>> SaveImagesAsync(
            IReadOnlyList<UploadImageModel> files, string folder, int existingCount = 0,
            CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<StoredFile>>(files.Select(file => Stored(file, folder)).ToList());

        public Task<StoredFile> SaveDocumentAsync(UploadImageModel file, string folder, CancellationToken cancellationToken = default) =>
            Task.FromResult(Stored(file, folder));

        public Task<StoredFile> SaveVideoAsync(UploadImageModel file, string folder, CancellationToken cancellationToken = default) =>
            Task.FromResult(Stored(file, folder));

        public void Delete(string relativePath)
        {
        }

        public string BuildPublicUrl(string relativePath) => "https://example.test" + relativePath;

        private static StoredFile Stored(UploadImageModel file, string folder)
        {
            var path = $"/uploads/{folder}/{Guid.NewGuid():N}.jpg";
            return new StoredFile { FileName = file.FileName, RelativePath = path, Url = "https://example.test" + path };
        }
    }

    public class SilentProxy<T> : DispatchProxy where T : class
    {
        public static T Create() => DispatchProxy.Create<T, SilentProxy<T>>();

        protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
        {
            var returnType = targetMethod!.ReturnType;

            if (returnType == typeof(Task))
                return Task.CompletedTask;

            if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
            {
                var result = returnType.GetGenericArguments()[0];

                return typeof(Task).GetMethod(nameof(Task.FromResult))!
                    .MakeGenericMethod(result)
                    .Invoke(null, [result.IsValueType ? Activator.CreateInstance(result) : null]);
            }

            return returnType.IsValueType && returnType != typeof(void) ? Activator.CreateInstance(returnType) : null;
        }
    }
}
