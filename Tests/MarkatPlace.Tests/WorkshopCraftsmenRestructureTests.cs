using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Persistence.Data;
using Persistence.Repositories;
using Services;
using Services.AdForms;
using Services.Lookups;
using Services.Mapping;
using Services.Validation;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Craftsmen;
using Shared.DTOs.Workshops;
using Shared.Enums;
using Shared.Exceptions;
using Shared.Settings;
using Xunit;

namespace MarkatPlace.Tests;

public class WorkshopCraftsmenRestructureTests
{
    internal static AppDbContext NewContext()
    {
        var context = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
        context.Database.EnsureCreated();
        return context;
    }

    private static IMapper Mapper() =>
        new MapperConfiguration(c => c.AddMaps(typeof(WorkshopMappingProfile).Assembly),
            NullLoggerFactory.Instance).CreateMapper();

    internal static LookupService Lookups(AppDbContext context) =>
        new(new LookupRepository(context), new MemoryCache(new MemoryCacheOptions()), new LookupCache(), Mapper());

    private static CraftsmanService Craftsmen(AppDbContext context) =>
        new(new CraftsmanRepository(context), new StubFileService(), Mapper(), SilentProxy<INotificationService>.Create());

    internal static WorkshopService Workshops(AppDbContext context) =>
        new(new WorkshopRepository(context), new StubFileService(), Mapper(), SilentProxy<INotificationService>.Create());

    internal static Shared.DTOs.Advertisements.UploadImageModel Upload() =>
        new() { FileName = "photo.jpg", ContentType = "image/jpeg", Length = 3, Content = [1, 2, 3] };

    internal static string SeedUser(AppDbContext context)
    {
        var id = Guid.NewGuid().ToString();
        context.Users.Add(new ApplicationUser
        {
            Id = id, UserName = id, FirstName = "حسن", SecondName = "علي", PhoneNumber = "01012345678",
            Email = $"{id}@example.com", Governorate = LocationConstants.Governorate, Center = "الفيوم"
        });
        context.SaveChanges();
        return id;
    }

    private static CreateCraftsmanRequest CraftsmanRequest(CraftsmanSpecialization specialization) => new()
    {
        Name = "أسطى حسن", Specialization = specialization, ExperienceLevel = ExperienceLevel.FiveToTenYears,
        Center = "الفيوم", Address = "شارع الحرية", PhoneNumber = "01012345678", WhatsApp = "01012345678",
        AdTitle = "عنوان", AdDescription = "وصف"
    };

    private static UpdateCraftsmanRequest CraftsmanUpdate(CraftsmanSpecialization specialization) => new()
    {
        Name = "أسطى حسن", Specialization = specialization, ExperienceLevel = ExperienceLevel.FiveToTenYears,
        Center = "الفيوم", Address = "شارع الحرية", PhoneNumber = "01012345678", WhatsApp = "01012345678",
        AdTitle = "عنوان", AdDescription = "وصف"
    };

    internal static CreateWorkshopRequest WorkshopRequest(WorkshopType type) => new()
    {
        Name = "ورشة الأمانة", WorkshopType = type, Center = "الفيوم", Address = "شارع الحرية",
        PhoneNumber = "01012345678", WhatsApp = "01012345678", AdTitle = "عنوان", AdDescription = "وصف"
    };

    internal static UpdateWorkshopRequest WorkshopUpdate(WorkshopType type) => new()
    {
        Name = "ورشة الأمانة", WorkshopType = type, Center = "الفيوم", Address = "شارع الحرية",
        PhoneNumber = "01012345678", WhatsApp = "01012345678", AdTitle = "عنوان", AdDescription = "وصف"
    };

    public static readonly string[] FinalWorkshopTypes =
    [
        "ورشة باب وشباك", "حداد", "ورشة زجاج", "ورشة رخام وجرانيت", "نجار موبيليا",
        "ورشة ألوميتال ومطابخ", "ورشة تنجيد إفرنجي", "ورشة خراطة", "ورشة لحام", "مراكز صيانة", "ورشة سمكرة",
        "دوكو سيارات", "ورشة كهرباء سيارات", "ميكانيكا سيارات", "ورشة دراجات نارية", "صيانة أجهزة منزلية",
        "ورشة تكييف وتبريد", "لحام أرجون ومعادن", "مشغل ملابس", "ورشة أحذية", "ورشة جلود", "مطابع ومكتبات",
        "ورشة تنجيد بلدي", "الحفر بالكمبيوتر CNC", "ورشة ستائر", "أخرى"
    ];

    public static readonly string[] FinalSpecializations =
    [
        "نجار أثاث", "نجار أبواب", "نجار مطابخ", "سباكة", "كهربائي منازل و3 فاز", "نقاش", "سيراميك وبورسلين",
        "رخام وجرانيت", "باركيه", "جبس بورد وبلدي", "ديكورات جبس", "حداد", "ألوميتال", "زجاج", "لحام أرجون",
        "تكييف وتبريد", "صيانة بوتاجازات", "تركيب مطابخ", "تركيب ستائر", "تنظيف منازل", "تنظيف شركات",
        "فك وتركيب أثاث", "نقل أثاث", "نيوماتيك", "فني خزائن ومفاتيح", "إيبوكسي", "أخرى"
    ];

    [Fact]
    public async Task The_workshop_types_lookup_returns_exactly_the_final_list_with_other_last()
    {
        using var context = NewContext();

        var types = await Lookups(context).GetWorkshopTypesAsync();

        Assert.Equal(FinalWorkshopTypes, types.Select(t => t.Name));
        Assert.DoesNotContain(types, t => t.Name is "ورشة ألوميتال" or "ورشة تصنيع بلاستيك" or "ورشة تطريز");
    }

    [Fact]
    public async Task The_specializations_lookup_returns_exactly_the_final_list_with_other_last()
    {
        using var context = NewContext();

        var specializations = await Lookups(context).GetSpecializationsAsync();

        Assert.Equal(FinalSpecializations, specializations.Select(s => s.Name));
        foreach (var removed in new[]
                 {
                     "سباك", "تأسيس سباكة", "صيانة سباكة", "كهربائي صناعي", "دهانات ديكورية", "حداد كريتال",
                     "لحام كهرباء", "صيانة غسالات", "صيانة ثلاجات", "صيانة سخانات", "تركيب طاقة شمسية",
                     "سيراميك", "بورسلين", "جرانيت", "فني تكييف", "فني تبريد"
                 })
            Assert.DoesNotContain(specializations, s => s.Name == removed);
    }

    [Theory]
    [InlineData(WorkshopType.Carpentry, "ورشة باب وشباك")]
    [InlineData(WorkshopType.Blacksmithing, "حداد")]
    [InlineData(WorkshopType.Furniture, "نجار موبيليا")]
    [InlineData(WorkshopType.Kitchens, "ورشة ألوميتال ومطابخ")]
    [InlineData(WorkshopType.Upholstery, "ورشة تنجيد إفرنجي")]
    [InlineData(WorkshopType.CarRepair, "مراكز صيانة")]
    [InlineData(WorkshopType.CarPainting, "دوكو سيارات")]
    [InlineData(WorkshopType.Motor, "ميكانيكا سيارات")]
    [InlineData(WorkshopType.ElectricalApplianceRepair, "صيانة أجهزة منزلية")]
    [InlineData(WorkshopType.MetalManufacturing, "لحام أرجون ومعادن")]
    [InlineData(WorkshopType.Clothing, "مشغل ملابس")]
    [InlineData(WorkshopType.Printing, "مطابع ومكتبات")]
    public void Renamed_workshop_types_keep_their_ids(WorkshopType type, string name)
    {
        Assert.Equal(name, WorkshopCraftsmenCatalog.GetWorkshopTypeName(type));
        Assert.True(WorkshopCraftsmenCatalog.IsActiveWorkshopType(type));
    }

    [Fact]
    public void New_ids_follow_the_existing_ones_and_retired_values_keep_their_names()
    {
        Assert.Equal(27, (int)WorkshopType.TraditionalUpholstery);
        Assert.Equal(28, (int)WorkshopType.CncEngraving);
        Assert.Equal(29, (int)WorkshopType.Curtains);
        Assert.Equal(38, (int)CraftsmanSpecialization.Pneumatics);
        Assert.Equal(39, (int)CraftsmanSpecialization.SafesAndLocksmith);
        Assert.Equal(40, (int)CraftsmanSpecialization.Epoxy);

        Assert.Equal("ورشة تصنيع بلاستيك", WorkshopCraftsmenCatalog.GetWorkshopTypeName(WorkshopType.PlasticManufacturing));
        Assert.Equal("ورشة تطريز", WorkshopCraftsmenCatalog.GetWorkshopTypeName(WorkshopType.Embroidery));
        Assert.Equal("تركيب طاقة شمسية",
            WorkshopCraftsmenCatalog.GetSpecializationName(CraftsmanSpecialization.SolarEnergyInstallation));
        Assert.False(WorkshopCraftsmenCatalog.IsActiveWorkshopType(WorkshopType.PlasticManufacturing));
        Assert.Equal(Enum.GetValues<WorkshopType>().Length, WorkshopCraftsmenCatalog.WorkshopTypeNames.Count);
        Assert.Equal(Enum.GetValues<CraftsmanSpecialization>().Length, WorkshopCraftsmenCatalog.Specializations.Count);
    }

    [Theory]
    [InlineData(CraftsmanSpecialization.PlumbingInstallation, CraftsmanSpecialization.Plumber)]
    [InlineData(CraftsmanSpecialization.PlumbingMaintenance, CraftsmanSpecialization.Plumber)]
    [InlineData(CraftsmanSpecialization.IndustrialElectrician, CraftsmanSpecialization.HomeElectrician)]
    [InlineData(CraftsmanSpecialization.Porcelain, CraftsmanSpecialization.Ceramic)]
    [InlineData(CraftsmanSpecialization.Granite, CraftsmanSpecialization.Marble)]
    [InlineData(CraftsmanSpecialization.RefrigerationTechnician, CraftsmanSpecialization.AirConditioningTechnician)]
    public void Merged_specializations_resolve_to_an_active_target(CraftsmanSpecialization merged, CraftsmanSpecialization target)
    {
        Assert.Equal(target, WorkshopCraftsmenCatalog.CanonicalSpecialization(merged));
        Assert.True(WorkshopCraftsmenCatalog.IsActiveSpecialization(merged));
        Assert.True(WorkshopCraftsmenCatalog.Specializations.Single(s => s.Value == target).IsActive);
        Assert.False(WorkshopCraftsmenCatalog.Specializations.Single(s => s.Value == merged).IsActive);
    }

    [Fact]
    public void The_migration_remaps_exactly_the_catalog_merges()
    {
        var migration = Directory.EnumerateFiles(
                Path.Combine(RepositoryRoot.Path, "Infastrucre", "Presitance", "Migrations"),
                "*_RestructureWorkshopTypesAndCraftsmanSpecializations.cs")
            .Single();
        var source = File.ReadAllText(migration);

        foreach (var group in WorkshopCraftsmenCatalog.MergedSpecializations.GroupBy(pair => pair.Value))
        {
            var sources = string.Join(", ", group.Select(pair => (int)pair.Key).Order());
            var expected = group.Count() == 1
                ? $"SET [Specialization] = {(int)group.Key} WHERE [Specialization] = {sources};"
                : $"SET [Specialization] = {(int)group.Key} WHERE [Specialization] IN ({sources});";
            Assert.Contains(expected, source);
        }
    }

    [Theory]
    [InlineData(CraftsmanSpecialization.DecorativePainting)]
    [InlineData(CraftsmanSpecialization.CrystalBlacksmith)]
    [InlineData(CraftsmanSpecialization.ElectricWelding)]
    [InlineData(CraftsmanSpecialization.WashingMachineMaintenance)]
    [InlineData(CraftsmanSpecialization.RefrigeratorMaintenance)]
    [InlineData(CraftsmanSpecialization.WaterHeaterMaintenance)]
    [InlineData(CraftsmanSpecialization.SolarEnergyInstallation)]
    public void Removed_specializations_are_refused_for_new_listings(CraftsmanSpecialization removed)
    {
        var errors = new CreateCraftsmanRequestValidator().Validate(CraftsmanRequest(removed)).Errors;
        Assert.Contains(errors, e => e.ErrorMessage == WorkshopCraftsmenCatalog.RetiredChoiceMessage);
    }

    [Theory]
    [InlineData(WorkshopType.Aluminum)]
    [InlineData(WorkshopType.PlasticManufacturing)]
    [InlineData(WorkshopType.Embroidery)]
    public void Replaced_workshop_types_are_refused_for_new_listings(WorkshopType retired)
    {
        var errors = new CreateWorkshopRequestValidator().Validate(WorkshopRequest(retired)).Errors;
        Assert.Contains(errors, e => e.ErrorMessage == WorkshopCraftsmenCatalog.RetiredChoiceMessage);
    }

    [Theory]
    [InlineData(CraftsmanSpecialization.Pneumatics)]
    [InlineData(CraftsmanSpecialization.SafesAndLocksmith)]
    [InlineData(CraftsmanSpecialization.Epoxy)]
    [InlineData(CraftsmanSpecialization.PlumbingMaintenance)]
    public void New_and_merged_specializations_are_accepted(CraftsmanSpecialization specialization)
    {
        var errors = new CreateCraftsmanRequestValidator().Validate(CraftsmanRequest(specialization)).Errors;
        Assert.DoesNotContain(errors, e => e.PropertyName == nameof(CreateCraftsmanRequest.Specialization));
    }

    [Fact]
    public async Task A_legacy_merged_id_from_an_old_client_is_stored_as_the_merged_target()
    {
        using var context = NewContext();
        var owner = SeedUser(context);

        var created = await Craftsmen(context).CreateAsync(owner, CraftsmanRequest(CraftsmanSpecialization.Porcelain), [Upload()]);

        Assert.Equal(CraftsmanSpecialization.Ceramic, created.Specialization);
        Assert.Equal("سيراميك وبورسلين", created.SpecializationName);
    }

    [Fact]
    public async Task A_historical_listing_on_a_retired_value_stays_visible_and_editable_but_cannot_switch_to_one()
    {
        using var context = NewContext();
        var owner = SeedUser(context);

        var legacy = new Craftsman
        {
            Id = Guid.NewGuid(), UserId = owner, Name = "أسطى قديم",
            Specialization = CraftsmanSpecialization.SolarEnergyInstallation,
            ExperienceLevel = ExperienceLevel.OneToThreeYears, Governorate = LocationConstants.Governorate,
            Center = "الفيوم", Address = "عنوان", PhoneNumber = "01012345678", WhatsApp = "01012345678",
            AdTitle = "طاقة شمسية", AdDescription = "وصف", ModerationStatus = ModerationStatus.Approved,
            PublishedAt = DateTime.UtcNow, ExpireAt = DateTime.UtcNow.AddDays(30)
        };
        context.Craftsmen.Add(legacy);
        await context.SaveChangesAsync();

        var details = await Craftsmen(context).GetByIdAsync(legacy.Id);
        Assert.Equal("تركيب طاقة شمسية", details.SpecializationName);
        Assert.Equal(1, (await Craftsmen(context).GetListAsync(
            new CraftsmanFilterParams { Specialization = CraftsmanSpecialization.SolarEnergyInstallation })).TotalCount);

        var kept = await Craftsmen(context).UpdateAsync(owner, false, legacy.Id,
            CraftsmanUpdate(CraftsmanSpecialization.SolarEnergyInstallation), []);
        Assert.Equal(CraftsmanSpecialization.SolarEnergyInstallation, kept.Specialization);

        await Assert.ThrowsAsync<BadRequestException>(() => Craftsmen(context).UpdateAsync(owner, false, legacy.Id,
            CraftsmanUpdate(CraftsmanSpecialization.CrystalBlacksmith), []));

        var moved = await Craftsmen(context).UpdateAsync(owner, false, legacy.Id,
            CraftsmanUpdate(CraftsmanSpecialization.Epoxy), []);
        Assert.Equal("إيبوكسي", moved.SpecializationName);
    }

    [Fact]
    public async Task Workshops_on_a_replaced_type_keep_working_and_new_ones_cannot_pick_it()
    {
        using var context = NewContext();
        var owner = SeedUser(context);

        var created = await Workshops(context).CreateAsync(owner, WorkshopRequest(WorkshopType.CncEngraving), [Upload()]);
        Assert.Equal("الحفر بالكمبيوتر CNC", created.WorkshopTypeName);

        await Assert.ThrowsAsync<BadRequestException>(() =>
            Workshops(context).CreateAsync(owner, WorkshopRequest(WorkshopType.Embroidery), [Upload()]));

        var row = await context.Workshops.IgnoreQueryFilters().SingleAsync();
        row.WorkshopType = WorkshopType.Embroidery;
        await context.SaveChangesAsync();

        var kept = await Workshops(context).UpdateAsync(owner, false, row.Id, WorkshopUpdate(WorkshopType.Embroidery), []);
        Assert.Equal("ورشة تطريز", kept.WorkshopTypeName);

        var moved = await Workshops(context).UpdateAsync(owner, false, row.Id, WorkshopUpdate(WorkshopType.Curtains), []);
        Assert.Equal("ورشة ستائر", moved.WorkshopTypeName);

        await Assert.ThrowsAsync<BadRequestException>(() =>
            Workshops(context).UpdateAsync(owner, false, row.Id, WorkshopUpdate(WorkshopType.PlasticManufacturing), []));
    }

    [Fact]
    public async Task Filtering_by_a_merged_id_finds_the_merged_listings()
    {
        using var context = NewContext();
        var owner = SeedUser(context);

        var created = await Craftsmen(context).CreateAsync(owner, CraftsmanRequest(CraftsmanSpecialization.Plumber), [Upload()]);
        var row = await context.Craftsmen.IgnoreQueryFilters().SingleAsync(c => c.Id == created.Id);
        row.ModerationStatus = ModerationStatus.Approved;
        row.PublishedAt = DateTime.UtcNow;
        row.ExpireAt = DateTime.UtcNow.AddDays(30);
        await context.SaveChangesAsync();

        Assert.Equal(1, (await Craftsmen(context).GetListAsync(
            new CraftsmanFilterParams { Specialization = CraftsmanSpecialization.PlumbingMaintenance })).TotalCount);
    }

    [Fact]
    public async Task The_create_form_offers_only_active_choices()
    {
        using var context = NewContext();
        var form = new CreateAdFormService(Resolver(context), new AdFormBreadcrumbBuilder(Options.Create(new AppSettings())),
            Lookups(context), new AdminFormRepository(context), Mapper());

        var workshops = await form.GetCreateAdFormAsync((int)CategoryType.WorkshopsAndCraftsmen, (int)SubCategoryType.Workshops);
        Assert.Equal(FinalWorkshopTypes, workshops.Lookups.WorkshopTypes!.Select(t => t.Name));

        var craftsmen = await form.GetCreateAdFormAsync((int)CategoryType.WorkshopsAndCraftsmen, (int)SubCategoryType.Craftsmen);
        Assert.Equal(FinalSpecializations, craftsmen.Lookups.Specializations!.Select(s => s.Name));
    }

    private static CategorySelectionResolver Resolver(AppDbContext context) => new(new LookupRepository(context));

    [Fact]
    public void The_governorate_guide_modules_and_buildings_keep_their_ids_and_routes()
    {
        Assert.Equal(new[] { 5, 6, 66, 67, 68 },
            CategoryCatalog.SubCategories.Where(s => s.Category == CategoryType.WorkshopsAndCraftsmen).Select(s => (int)s.Id));
        Assert.Equal(101, (int)ListingModuleCatalog.ModuleOf(SubCategoryType.CommercialShops)!);
        Assert.Equal(102, (int)ListingModuleCatalog.ModuleOf(SubCategoryType.GeneralServices)!);
        Assert.Equal(103, (int)ListingModuleCatalog.ModuleOf(SubCategoryType.ResortsHotelsAndAmusement)!);
        Assert.Equal(63, (int)ListingModuleCatalog.ModuleOf(SubCategoryType.Buildings)!);
        Assert.Equal("api/commercial-shops", GovernorateGuideRoutes.CommercialShops);
        Assert.Equal("api/general-services", GovernorateGuideRoutes.GeneralServices);
        Assert.Equal("api/resorts-hotels-amusement", GovernorateGuideRoutes.ResortsHotelsAndAmusement);
        Assert.Equal("api/buildings", RealEstateRoutes.Buildings);
    }
}
