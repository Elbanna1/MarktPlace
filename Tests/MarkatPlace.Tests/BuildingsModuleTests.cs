using System.Reflection;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
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
using Services.Lookups;
using Services.Mapping;
using Services.ReadConfigs;
using Services.RealEstate;
using Services.Validation;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Admin;
using Shared.DTOs.Advertisements;
using Shared.DTOs.Lookups;
using Shared.DTOs.Lookups.Forms;
using Shared.DTOs.Lookups.Read;
using Shared.DTOs.RealEstate;
using Shared.Enums;
using Shared.Exceptions;
using Shared.DTOs.Realtime;
using Shared.Settings;
using Xunit;

namespace MarkatPlace.Tests;

public class BuildingsModuleTests
{
    private const int Category = 11;

    private const int SubCategory = 65;

    private const ListingModuleType Module = ListingModuleType.Building;

    private sealed class RecordingPublisher : IListingEventPublisher
    {
        public List<ListingChange> Changes { get; } = new();

        public Task PublishAsync(IReadOnlyList<ListingChange> changes, CancellationToken cancellationToken = default)
        {
            Changes.AddRange(changes);
            return Task.CompletedTask;
        }
    }

    private sealed class FailingPublisher : IListingEventPublisher
    {
        public Task PublishAsync(IReadOnlyList<ListingChange> changes, CancellationToken cancellationToken = default) =>
            throw new InvalidOperationException("hub down");
    }

    private static AppDbContext NewContext(IListingEventPublisher? publisher = null)
    {
        var builder = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());

        if (publisher is not null)
            builder.AddInterceptors(
                new ListingRealtimeInterceptor([publisher], NullLogger<ListingRealtimeInterceptor>.Instance));

        var context = new AppDbContext(builder.Options);
        context.Database.EnsureCreated();
        return context;
    }

    private static IMapper Mapper() =>
        new MapperConfiguration(
            configuration => configuration.AddMaps(typeof(BuildingMappingProfile).Assembly),
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

    private static BuildingService Service(AppDbContext context, INotificationService? notifications = null) =>
        new(new BuildingRepository(context), new StubFileService(), Mapper(),
            notifications ?? SilentProxy<INotificationService>.Create());

    private static (AdminAdService Service, SilentProxy<INotificationService> Notifications) Admin(AppDbContext context)
    {
        var repository = new AdminAdRepository(
            context,
            [new BuildingListingSource(context)],
            [new ListingModerationSource<Building>(context, Module)]);

        var notifications = SilentProxy<INotificationService>.Create();

        var service = new AdminAdService(
            repository,
            SilentProxy<IListingInteractionService>.Create(),
            Lookups(context),
            notifications,
            SilentProxy<IListingInterestNotifier>.Create(),
            SilentProxy<IAdminAuditService>.Create(),
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
            Id = id, UserName = id, FirstName = "أحمد", SecondName = "علي", PhoneNumber = "01012345678",
            Email = $"{id}@example.com", Governorate = LocationConstants.Governorate, Center = "الفيوم"
        });
        context.SaveChanges();
        return id;
    }

    private static T Request<T>(BuildingType type, RealEstateListingType listingType = RealEstateListingType.Sale)
        where T : CreateBuildingRequest, new()
    {
        var request = new T
        {
            Title = "فيلا للبيع بحديقة",
            Description = "فيلا مستقلة بالقرب من الخدمات",
            AdvertiserName = "أحمد محمود",
            ListingType = listingType,
            BuildingType = type,
            Area = 350,
            FurnishedStatus = ApartmentFurnishedStatus.No,
            FinishingType = ApartmentFinishingType.SuperLux,
            PropertyAge = ApartmentPropertyAge.New,
            Direction = ApartmentDirection.North,
            FloorsCount = 3,
            LegalStatus = ApartmentLegalStatus.Unlicensed,
            OwnershipDocument = ApartmentOwnershipDocument.GreenContract,
            Center = "الفيوم",
            Address = "شارع الجمهورية",
            Phone = "01012345678"
        };

        switch (type)
        {
            case BuildingType.Villa:
                request.BathroomsCount = 3;
                request.RoomsCount = 5;
                request.ReceptionPieces = ApartmentReceptionPieces.Two;
                break;
            case BuildingType.House:
                request.ResidentialUnitsCount = 4;
                break;
            case BuildingType.ApartmentBuilding:
                request.ApartmentsCount = 12;
                request.ShopsCount = 0;
                break;
        }

        switch (listingType)
        {
            case RealEstateListingType.Sale:
                request.Price = 2_500_000;
                request.PricePerMeter = 7_000;
                request.PaymentMethod = ApartmentPaymentMethod.Cash;
                break;
            case RealEstateListingType.Rent:
                request.RentType = ApartmentRentType.Monthly;
                request.RentValue = 9_000;
                request.SecurityDeposit = 9_000;
                request.RentDownPayment = 0;
                request.MinimumRentPeriod = 12;
                request.AvailableFrom = new DateTime(2026, 11, 1);
                request.AvailableTo = new DateTime(2027, 11, 1);
                request.RentInclusions = [ApartmentRentInclusion.Water];
                request.OwnerConditions = "عائلات فقط";
                break;
            case RealEstateListingType.Exchange:
                request.ExchangeWith = ApartmentExchangeWith.Land;
                request.ExchangeDetails = "بدل بأرض زراعية";
                break;
        }

        return request;
    }

    private static CreateBuildingRequest Valid(BuildingType type, RealEstateListingType listingType = RealEstateListingType.Sale) =>
        Request<CreateBuildingRequest>(type, listingType);

    private static Dictionary<string, string> Errors(CreateBuildingRequest request) =>
        new CreateBuildingRequestValidator().Validate(request).Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(g => g.Key, g => g.First().ErrorMessage);

    private static FormFieldDto Field(AdFormSchema schema, string name) => schema.Fields.Single(f => f.Name == name);

    [Fact]
    public void The_ids_are_the_next_free_ones_under_real_estate()
    {
        Assert.Equal(65, (int)SubCategoryType.Buildings);
        Assert.Equal(63, (int)ListingModuleType.Building);

        var entry = CategoryCatalog.SubCategories.Single(s => s.Id == SubCategoryType.Buildings);
        Assert.Equal(CategoryType.RealEstate, entry.Category);
        Assert.Equal("مباني", entry.NameAr);

        Assert.True(RealEstateSubCategories.Contains(SubCategory));
        Assert.Single(Enum.GetValues<SubCategoryType>(), value => (int)value == 65);
        Assert.Single(Enum.GetValues<ListingModuleType>(), value => (int)value == 63);
    }

    [Fact]
    public void The_module_is_registered_in_all_three_lookup_systems_and_every_seam()
    {
        Assert.NotNull(AdFormSchemaCatalog.GetSchema(Category, SubCategory));
        Assert.NotNull(ReadConfigCatalog.GetSchema(Category, SubCategory));
        Assert.Null(AdFormSchemaCatalog.GetSchema((int)CategoryType.Business, SubCategory));

        Assert.True(AdvertiserLookupCatalog.Supports(SubCategory));
        Assert.Equal("advertiserName", AdvertiserLookupCatalog.FilterParameterOf(SubCategoryType.Buildings));
        Assert.Contains(SubCategoryType.Buildings, AdvertiserLookupRepository.SupportedSubCategories);

        Assert.Equal(Module, ListingModuleCatalog.ModuleOf(SubCategoryType.Buildings));
        Assert.Equal(CategoryType.RealEstate, ListingModuleCatalog.CategoryOf(Module));
        Assert.Equal("buildings", ListingModuleCatalog.RouteOf(Module));
        Assert.Equal(typeof(Building), ModerationModuleRegistry.EntityTypes[Module]);

        Assert.Contains(ImageConstants.BuildingFolder, ImageConstants.AllFolders);
        Assert.Contains(FileUploadConstants.BuildingVideoFolder, ImageConstants.AllFolders);
    }

    [Fact]
    public void Every_route_exists_and_writes_need_a_signed_in_user()
    {
        const string route = "/api/buildings";

        Assert.True(ApiRouteIndex.Exists("POST", route));
        Assert.True(ApiRouteIndex.Exists("GET", route));
        Assert.True(ApiRouteIndex.Exists("GET", route + "/{id}"));
        Assert.True(ApiRouteIndex.Exists("PUT", route + "/{id}"));
        Assert.True(ApiRouteIndex.Exists("DELETE", route + "/{id}"));
        Assert.True(ApiRouteIndex.Exists("GET", route + "/building-types"));
        Assert.True(ApiRouteIndex.Exists("GET", route + "/features"));

        var controller = typeof(BuildingsController);
        Assert.NotNull(controller.GetCustomAttribute<AuthorizeAttribute>());

        foreach (var name in new[] { "GetList", "GetById", "GetSimilar", "GetRelated", "GetRecentlyAdded" })
            Assert.NotNull(controller.GetMethod(name)!.GetCustomAttribute<AllowAnonymousAttribute>());

        foreach (var name in new[] { "Create", "Update", "Delete", "ReorderImages", "SetPromotion" })
            Assert.Null(controller.GetMethod(name)!.GetCustomAttribute<AllowAnonymousAttribute>());
    }

    [Fact]
    public void The_form_publishes_the_base_fields_with_the_specified_flags()
    {
        var schema = AdFormSchemaCatalog.GetSchema(Category, SubCategory)!;

        Assert.Equal("buildings", schema.Module);
        Assert.Equal("/api/buildings", schema.Submit.Endpoint);

        foreach (var name in new[]
                 {
                     "Title", "BuildingType", "Description", "AdvertiserName", "ListingType", "Images",
                     "Area", "FurnishedStatus", "FinishingType", "PropertyAge", "Direction",
                     "LegalStatus", "OwnershipDocument", "Center", "Address", "Phone"
                 })
            Assert.True(Field(schema, name).Required, name);

        foreach (var name in new[]
                 {
                     "Video", "ViewType", "Governorate", "District", "GoogleMaps", "WhatsApp", "Email",
                     "Negotiable", "Notes", "Features", "IsRegistered", "HasViolations"
                 })
        {
            Assert.False(Field(schema, name).Required, name);
            Assert.Null(Field(schema, name).RequiredWhen);
        }

        Assert.Equal(AdFormLookupKeys.BuildingTypes, Field(schema, "BuildingType").OptionsSource);
        Assert.Equal("نوع المبنى", Field(schema, "BuildingType").Label);
        Assert.Equal("حالة المبنى", Field(schema, "LegalStatus").Label);
        Assert.True(Field(schema, "Governorate").ReadOnly);
        Assert.Equal(ApartmentCatalog.Features.Count, Field(schema, "Features").MaxSelections);
    }

    public static TheoryData<string, BuildingType[]> ConditionalBuildingFields() => new()
    {
        { "GardenArea", [BuildingType.Villa] },
        { "BathroomsCount", [BuildingType.Villa] },
        { "ReceptionPieces", [BuildingType.Villa] },
        { "RoomsCount", [BuildingType.Villa] },
        { "HasSwimmingPool", [BuildingType.Villa] },
        { "ResidentialUnitsCount", [BuildingType.House] },
        { "HasIndependentEntrance", [BuildingType.House] },
        { "ApartmentsCount", [BuildingType.ApartmentBuilding] },
        { "ShopsCount", [BuildingType.ApartmentBuilding] },
        { "HasElevator", [BuildingType.ApartmentBuilding] },
        { "HasGarage", [BuildingType.ApartmentBuilding] },
        { "FloorsCount", [BuildingType.Villa, BuildingType.House, BuildingType.ApartmentBuilding] }
    };

    [Theory]
    [MemberData(nameof(ConditionalBuildingFields))]
    public void Each_building_type_field_is_shown_only_for_its_types(string name, BuildingType[] types)
    {
        var field = Field(AdFormSchemaCatalog.GetSchema(Category, SubCategory)!, name);

        Assert.False(field.Visible);
        Assert.Equal("BuildingType", field.VisibleWhen!.Field);
        Assert.Equal(types.Select(t => (object)(int)t), field.VisibleWhen.Values);
        Assert.False(field.Required);
    }

    [Theory]
    [InlineData("GardenArea", false)]
    [InlineData("HasSwimmingPool", false)]
    [InlineData("HasIndependentEntrance", false)]
    [InlineData("HasElevator", false)]
    [InlineData("HasGarage", false)]
    [InlineData("BathroomsCount", true)]
    [InlineData("ReceptionPieces", true)]
    [InlineData("RoomsCount", true)]
    [InlineData("ResidentialUnitsCount", true)]
    [InlineData("ApartmentsCount", true)]
    [InlineData("ShopsCount", true)]
    [InlineData("FloorsCount", true)]
    public void Conditional_fields_are_required_only_inside_their_own_condition(string name, bool required)
    {
        var field = Field(AdFormSchemaCatalog.GetSchema(Category, SubCategory)!, name);

        if (required)
            Assert.Equal(field.VisibleWhen!.Values, field.RequiredWhen!.Values);
        else
            Assert.Null(field.RequiredWhen);
    }

    [Fact]
    public void No_checkbox_is_ever_required_because_the_frontend_would_force_it_true()
    {
        var schema = AdFormSchemaCatalog.GetSchema(Category, SubCategory)!;

        Assert.All(schema.Fields.Where(f => f.Type == FormFieldTypes.Checkbox), field =>
        {
            Assert.False(field.Required, field.Name);
            Assert.Null(field.RequiredWhen);
        });
    }

    [Fact]
    public void Transaction_and_licence_fields_follow_the_listing_type_and_legal_status()
    {
        var schema = AdFormSchemaCatalog.GetSchema(Category, SubCategory)!;

        Assert.Equal((int)RealEstateListingType.Sale, Field(schema, "Price").RequiredWhen!.Values.Single());
        Assert.Equal((int)RealEstateListingType.Sale, Field(schema, "PaymentMethod").RequiredWhen!.Values.Single());
        Assert.Equal((int)RealEstateListingType.Rent, Field(schema, "RentValue").RequiredWhen!.Values.Single());
        Assert.Equal((int)RealEstateListingType.Rent, Field(schema, "RentInclusions").RequiredWhen!.Values.Single());
        Assert.Equal((int)RealEstateListingType.Exchange, Field(schema, "ExchangeWith").RequiredWhen!.Values.Single());
        Assert.Null(Field(schema, "MonthlyFees").RequiredWhen);

        Assert.Equal((int)ApartmentLegalStatus.Reconciliation,
            Field(schema, "ReconciliationForm").RequiredWhen!.Values.Single());
        Assert.Equal((int)ApartmentLegalStatus.Licensed, Field(schema, "LicenseNumber").VisibleWhen!.Values.Single());
        Assert.Null(Field(schema, "LicenseNumber").RequiredWhen);
    }

    [Fact]
    public async Task The_create_form_endpoint_returns_the_shared_lookups_and_the_building_types()
    {
        using var context = NewContext();

        var form = await FormService(context).GetCreateAdFormAsync(Category, SubCategory);

        Assert.Equal("buildings", form.Module);
        Assert.Equal(
            new[] { "فيلا", "بيت", "عمارة" },
            form.Lookups.BuildingTypes!.Select(option => option.Name));
        Assert.Equal(25, form.Lookups.ApartmentFeatures!.Count);
        Assert.Equal(ApartmentCatalog.FeatureOptions.Select(o => o.Id), form.Lookups.ApartmentFeatures.Select(o => o.Id));
        Assert.NotNull(form.Lookups.ApartmentOwnershipDocuments);
        Assert.NotNull(form.Lookups.ApartmentLegalStatuses);
    }

    [Fact]
    public async Task The_read_config_publishes_the_list_interactions_and_advertiser_filter()
    {
        using var context = NewContext();

        var config = await ReadService(context).GetReadConfigAsync(Category, SubCategory);

        Assert.Equal("/api/buildings", config.List!.Endpoint);

        var list = ReadConfigCatalog.GetSchema(Category, SubCategory)!.Find(ReadOperationKeys.List)!;
        var advertiser = list.QueryParameters!.Single(p => p.Name == "advertiserName");
        Assert.Equal(ReadOperationKeys.Advertisers, advertiser.OptionsSource);
        Assert.Equal(ReadOptionsValues.Name, advertiser.OptionsValue);

        var schema = ReadConfigCatalog.GetSchema(Category, SubCategory)!;
        foreach (var key in new[] { "listingActions", "similarListings", "recentlyViewed", "favorites", ReadOperationKeys.Advertisers })
            Assert.NotNull(schema.Find(key));

        Assert.Equal((int)Module, schema.Find("favorites")!.Query!["type"]);
    }

    [Theory]
    [InlineData(BuildingType.Villa, RealEstateListingType.Sale)]
    [InlineData(BuildingType.House, RealEstateListingType.Rent)]
    [InlineData(BuildingType.ApartmentBuilding, RealEstateListingType.Exchange)]
    public void A_complete_request_for_each_type_and_transaction_is_valid(BuildingType type, RealEstateListingType listingType)
    {
        var result = new CreateBuildingRequestValidator().Validate(Valid(type, listingType));

        Assert.True(result.IsValid, string.Join(" | ", result.Errors.Select(e => e.ErrorMessage)));
    }

    [Fact]
    public void A_villa_requires_its_own_fields_only()
    {
        var request = Valid(BuildingType.Villa);
        request.BathroomsCount = null;
        request.RoomsCount = null;
        request.ReceptionPieces = null;
        request.FloorsCount = null;

        var errors = Errors(request);

        Assert.Equal("عدد الحمامات مطلوب.", errors["BathroomsCount"]);
        Assert.Equal("عدد الغرف مطلوب.", errors["RoomsCount"]);
        Assert.Equal("عدد قطع الريسبشن مطلوب.", errors["ReceptionPieces"]);
        Assert.Equal("عدد الطوابق مطلوب.", errors["FloorsCount"]);
        Assert.Equal(4, errors.Count);
    }

    [Fact]
    public void A_house_requires_units_and_floors_but_never_villa_or_tower_fields()
    {
        var request = Valid(BuildingType.House);
        request.ResidentialUnitsCount = null;

        var errors = Errors(request);

        Assert.Equal("عدد الوحدات السكنية مطلوب.", Assert.Single(errors).Value);
    }

    [Fact]
    public void An_apartment_building_requires_apartments_shops_and_floors_and_accepts_zero_shops()
    {
        var request = Valid(BuildingType.ApartmentBuilding);
        request.ApartmentsCount = null;
        request.ShopsCount = null;
        request.FloorsCount = null;

        var errors = Errors(request);

        Assert.Equal("عدد الشقق مطلوب.", errors["ApartmentsCount"]);
        Assert.Equal("عدد المحلات التجارية مطلوب.", errors["ShopsCount"]);
        Assert.Equal("عدد الأدوار مطلوب.", errors["FloorsCount"]);
        Assert.Equal(3, errors.Count);

        var zeroShops = Valid(BuildingType.ApartmentBuilding);
        zeroShops.ShopsCount = 0;
        Assert.Empty(Errors(zeroShops));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(BuildingCatalog.MaxFloors + 1)]
    public void Counts_are_range_checked(int floors)
    {
        var request = Valid(BuildingType.House);
        request.FloorsCount = floors;
        request.ResidentialUnitsCount = -2;

        var errors = Errors(request);

        Assert.Contains("FloorsCount", errors.Keys);
        Assert.Contains("ResidentialUnitsCount", errors.Keys);
    }

    [Fact]
    public void Negative_shops_and_negative_garden_area_are_refused()
    {
        var tower = Valid(BuildingType.ApartmentBuilding);
        tower.ShopsCount = -1;
        Assert.Contains("ShopsCount", Errors(tower).Keys);

        var villa = Valid(BuildingType.Villa);
        villa.GardenArea = -5;
        Assert.Equal("مساحة الحديقة لا يمكن أن تكون سالبة.", Errors(villa)["GardenArea"]);
    }

    [Fact]
    public void Stale_fields_of_another_building_type_do_not_fail_validation()
    {
        var request = Valid(BuildingType.House);
        request.ShopsCount = -50;
        request.GardenArea = -1;
        request.BathroomsCount = 0;

        Assert.Empty(Errors(request));
    }

    [Fact]
    public void The_missing_base_fields_are_reported_in_Arabic()
    {
        var errors = Errors(new CreateBuildingRequest());

        Assert.Equal("نوع المبنى مطلوب.", errors["BuildingType"]);
        Assert.Equal("المساحة مطلوبة.", errors["Area"]);
        Assert.Equal("حالة الفرش مطلوبة.", errors["FurnishedStatus"]);
        Assert.Equal("التشطيب مطلوب.", errors["FinishingType"]);
        Assert.Equal("عمر العقار مطلوب.", errors["PropertyAge"]);
        Assert.Equal("الاتجاه مطلوب.", errors["Direction"]);
        Assert.Equal("حالة المبنى مطلوبة.", errors["LegalStatus"]);
        Assert.Equal("مستند الملكية مطلوب.", errors["OwnershipDocument"]);
        Assert.Equal("المركز مطلوب.", errors["Center"]);
        Assert.Equal("العنوان التفصيلي مطلوب.", errors["Address"]);
        Assert.Equal("رقم الهاتف مطلوب.", errors["Phone"]);
        Assert.Equal("عنوان الإعلان مطلوب.", errors["Title"]);
        Assert.Equal("اسم المعلن مطلوب.", errors["AdvertiserName"]);
        Assert.All(errors.Values, message => Assert.False(ArabicText.ContainsEnglishProse(message), message));
    }

    [Fact]
    public void Each_transaction_type_requires_its_own_block()
    {
        var sale = Valid(BuildingType.Villa);
        sale.Price = null;
        sale.PaymentMethod = ApartmentPaymentMethod.Installments;
        var saleErrors = Errors(sale);
        Assert.Equal("السعر مطلوب.", saleErrors["Price"]);
        Assert.Equal("المقدم مطلوب عند اختيار التقسيط.", saleErrors["DownPayment"]);

        var rent = Valid(BuildingType.Villa, RealEstateListingType.Rent);
        rent.RentValue = null;
        rent.RentInclusions = [];
        var rentErrors = Errors(rent);
        Assert.Equal("قيمة الإيجار مطلوبة.", rentErrors["RentValue"]);
        Assert.Equal("من فضلك اختار بند إيجار واحد على الأقل.", rentErrors["RentInclusions"]);
        Assert.DoesNotContain("Price", rentErrors.Keys);

        var exchange = Valid(BuildingType.Villa, RealEstateListingType.Exchange);
        exchange.ExchangeWith = null;
        exchange.AcceptsDifferencePayment = true;
        var exchangeErrors = Errors(exchange);
        Assert.Equal("من فضلك اختار ما ترغب بالبدل معه.", exchangeErrors["ExchangeWith"]);
        Assert.Equal("من فضلك اكتب قيمة الفرق.", exchangeErrors["DifferenceAmount"]);
    }

    [Fact]
    public void Reconciliation_needs_its_form_while_licensed_and_unlicensed_need_no_documents()
    {
        var reconciliation = Valid(BuildingType.Villa);
        reconciliation.LegalStatus = ApartmentLegalStatus.Reconciliation;
        Assert.Equal("نموذج التصالح مطلوب عند اختيار 'تصالح'.", Errors(reconciliation)["ReconciliationForm"]);

        var licensed = Valid(BuildingType.Villa);
        licensed.LegalStatus = ApartmentLegalStatus.Licensed;
        Assert.Empty(Errors(licensed));

        licensed.LicenseIssueDate = new DateTime(2025, 1, 1);
        licensed.LicenseExpiryDate = new DateTime(2024, 1, 1);
        Assert.Contains("LicenseExpiryDate", Errors(licensed).Keys);

        Assert.Empty(Errors(Valid(BuildingType.Villa)));
    }

    [Fact]
    public void Invalid_select_values_and_amenities_are_refused()
    {
        var request = Valid(BuildingType.Villa);
        request.BuildingType = (BuildingType)9;
        Assert.Equal("من فضلك اختار نوع مبنى صحيح (فيلا / بيت / عمارة).", Errors(request)["BuildingType"]);

        var features = Valid(BuildingType.Villa);
        features.Features = [ApartmentFeature.Garden, (ApartmentFeature)99];
        Assert.Contains(Errors(features).Values, m => m == "من فضلك اختار مرافق ومميزات صحيحة.");

        var duplicated = Valid(BuildingType.Villa);
        duplicated.Features = [ApartmentFeature.Garden, ApartmentFeature.Garden];
        Assert.Equal("لا يمكن تكرار نفس الميزة.", Errors(duplicated)["Features"]);
    }

    [Theory]
    [InlineData("0101234567")]
    [InlineData("+201012345678")]
    [InlineData("01712345678")]
    public void Phone_and_whatsapp_must_be_Egyptian_mobiles(string number)
    {
        var request = Valid(BuildingType.Villa);
        request.Phone = number;
        request.WhatsApp = number;

        var errors = Errors(request);

        Assert.Equal(AdvertisementCatalog.EgyptianPhoneMessage, errors["Phone"]);
        Assert.Equal(AdvertisementCatalog.EgyptianPhoneMessage, errors["WhatsApp"]);
    }

    [Fact]
    public void Email_and_maps_link_are_validated_only_when_supplied()
    {
        var request = Valid(BuildingType.Villa);
        request.Email = "not-an-email";
        request.GoogleMaps = "javascript:alert(1)";

        var errors = Errors(request);
        Assert.Equal("من فضلك اكتب بريد إلكتروني صحيح.", errors["Email"]);
        Assert.Equal("رابط خرائط جوجل يجب أن يكون رابط http(s) صحيح.", errors["GoogleMaps"]);

        var blank = Valid(BuildingType.Villa);
        blank.Email = " ";
        blank.GoogleMaps = "";
        blank.WhatsApp = "";
        Assert.Empty(Errors(blank));
    }

    [Fact]
    public void Switching_type_and_transaction_drops_every_stale_hidden_value()
    {
        var request = Valid(BuildingType.House, RealEstateListingType.Rent);
        request.GardenArea = 100;
        request.BathroomsCount = 2;
        request.HasSwimmingPool = true;
        request.ApartmentsCount = 8;
        request.ShopsCount = 2;
        request.HasElevator = true;
        request.Price = 1_000_000;
        request.PaymentMethod = ApartmentPaymentMethod.Cash;
        request.ExchangeWith = ApartmentExchangeWith.Car;
        request.ReconciliationForm = ApartmentReconciliationForm.Form8;
        request.LicenseNumber = "123";

        var entity = new Building();
        BuildingService.ApplyEditableFields(entity, request);

        Assert.Equal(4, entity.ResidentialUnitsCount);
        Assert.Equal(3, entity.FloorsCount);
        Assert.Null(entity.GardenArea);
        Assert.Null(entity.BathroomsCount);
        Assert.Null(entity.HasSwimmingPool);
        Assert.Null(entity.ApartmentsCount);
        Assert.Null(entity.ShopsCount);
        Assert.Null(entity.HasElevator);
        Assert.Null(entity.Price);
        Assert.Null(entity.PaymentMethod);
        Assert.Null(entity.ExchangeWith);
        Assert.Null(entity.ReconciliationForm);
        Assert.Null(entity.LicenseNumber);
        Assert.Equal(9_000, entity.RentValue);
        Assert.Equal(LocationConstants.Governorate, new Building { Governorate = LocationConstants.Governorate }.Governorate);
    }

    [Fact]
    public void Ticking_a_building_checkbox_also_records_the_matching_amenity()
    {
        var tower = Valid(BuildingType.ApartmentBuilding);
        tower.HasElevator = true;
        tower.HasGarage = true;
        tower.Features = [ApartmentFeature.Security];

        Assert.Equal(
            new[] { ApartmentFeature.Security, ApartmentFeature.Elevator, ApartmentFeature.Garage },
            BuildingService.RequestedFeatures(tower));

        var villa = Valid(BuildingType.Villa);
        villa.HasSwimmingPool = true;
        villa.HasElevator = true;
        Assert.Equal(new[] { ApartmentFeature.SwimmingPool }, BuildingService.RequestedFeatures(villa));

        var house = Valid(BuildingType.House);
        house.HasIndependentEntrance = true;
        house.Features = [ApartmentFeature.PrivateEntrance];
        Assert.Equal(new[] { ApartmentFeature.PrivateEntrance }, BuildingService.RequestedFeatures(house));
    }

    [Fact]
    public async Task Creating_stores_images_amenities_and_waits_for_review_and_ignores_client_governorate()
    {
        using var context = NewContext();
        var owner = SeedUser(context);
        var notifications = SilentProxy<INotificationService>.Create();

        var request = Valid(BuildingType.Villa);
        request.HasSwimmingPool = true;
        request.Features = [ApartmentFeature.Garden];

        var created = await Service(context, notifications)
            .CreateAsync(owner, request, [Upload(), Upload(), Upload()], Upload());

        Assert.Equal(3, created.Images.Count);
        Assert.Equal(new[] { 0, 1, 2 }, created.Images.Select(i => i.SortOrder));
        Assert.Single(created.Images, i => i.IsPrimary);
        Assert.StartsWith("https://example.test/uploads/buildings/", created.Images[0].Url);
        Assert.StartsWith("https://example.test/uploads/buildings-video/", created.VideoUrl);
        Assert.Equal("فيلا", created.BuildingTypeName);
        Assert.Equal(LocationConstants.Governorate, created.Governorate);
        Assert.Equal(owner, created.OwnerId);
        Assert.Equal(
            new[] { "حديقة", "مسبح" }.OrderBy(x => x),
            created.Features.Select(f => f.Name).OrderBy(x => x));

        var stored = await context.Buildings.IgnoreQueryFilters().SingleAsync();
        Assert.Equal(ModerationStatus.Pending, stored.ModerationStatus);
        Assert.Null(stored.ExpireAt);

        var notified = Assert.Single(SilentProxy<INotificationService>.Of(notifications).Calls);
        Assert.Equal(Module, notified.Args[1]);
        Assert.Equal(NotificationAction.Created, notified.Args[2]);

        await Assert.ThrowsAsync<BadRequestException>(() =>
            Service(context).CreateAsync(owner, Valid(BuildingType.Villa), [], null));
    }

    [Fact]
    public async Task Only_the_owner_or_an_admin_may_update_or_delete()
    {
        using var context = NewContext();
        var owner = SeedUser(context);
        var stranger = SeedUser(context);

        var created = await Service(context).CreateAsync(owner, Valid(BuildingType.Villa), [Upload()], null);

        var update = Request<UpdateBuildingRequest>(BuildingType.House);
        await Assert.ThrowsAsync<NotFoundException>(() =>
            Service(context).UpdateAsync(stranger, false, created.Id, update, [], null));
        await Assert.ThrowsAsync<NotFoundException>(() => Service(context).DeleteAsync(stranger, created.Id, isAdmin: false));

        var updated = await Service(context).UpdateAsync(owner, false, created.Id, update, [], null);
        Assert.Equal(BuildingType.House, updated.BuildingType);
        Assert.Null(updated.RoomsCount);
        Assert.Equal(4, updated.ResidentialUnitsCount);

        await Service(context).DeleteAsync(stranger, created.Id, isAdmin: true);
        Assert.True((await context.Buildings.IgnoreQueryFilters().SingleAsync()).IsDeleted);
    }

    [Fact]
    public async Task The_optional_video_can_be_removed_and_the_last_image_cannot()
    {
        using var context = NewContext();
        var owner = SeedUser(context);

        var created = await Service(context).CreateAsync(owner, Valid(BuildingType.Villa), [Upload()], Upload());
        Assert.NotNull(created.VideoUrl);

        var removeVideo = Request<UpdateBuildingRequest>(BuildingType.Villa);
        removeVideo.RemoveVideo = true;
        var updated = await Service(context).UpdateAsync(owner, false, created.Id, removeVideo, [], null);
        Assert.Null(updated.VideoUrl);

        var removeImage = Request<UpdateBuildingRequest>(BuildingType.Villa);
        removeImage.RemoveImageIds = [updated.Images.Single().Id];
        await Assert.ThrowsAsync<BadRequestException>(() =>
            Service(context).UpdateAsync(owner, false, created.Id, removeImage, [], null));
    }

    [Fact]
    public async Task Approval_publishes_the_listing_immediately_notifies_once_and_emits_one_public_event()
    {
        var publisher = new RecordingPublisher();
        using var context = NewContext(publisher);
        var owner = SeedUser(context);

        var created = await Service(context).CreateAsync(owner, Valid(BuildingType.Villa), [Upload()], null);

        Assert.Equal(0, (await Service(context).GetListAsync(new BuildingFilterParams())).TotalCount);

        var created_event = Assert.Single(publisher.Changes, c => c.Event.Change == ListingChangeKinds.Created).Event;
        Assert.False(created_event.IsPublic);
        Assert.Equal(Module, created_event.Type);
        Assert.Equal(Category, created_event.CategoryId);
        Assert.Equal(SubCategory, created_event.SubCategoryId);
        Assert.Equal("buildings", created_event.Route);
        publisher.Changes.Clear();

        var (admin, notifications) = Admin(context);
        await admin.ApproveAsync(Module, created.Id, "admin", new ApproveListingRequest());
        await admin.ApproveAsync(Module, created.Id, "admin", new ApproveListingRequest());

        var stored = await context.Buildings.IgnoreQueryFilters().SingleAsync();
        Assert.Equal(ModerationStatus.Approved, stored.ModerationStatus);
        Assert.Equal(TimeSpan.FromDays(ListingLifecycle.ActiveDurationDays), stored.ExpireAt - stored.PublishedAt);

        var approvals = publisher.Changes.Where(c => c.Event.Change == ListingChangeKinds.Approved).ToList();
        var approval = Assert.Single(approvals);
        Assert.True(approval.Event.IsPublic);
        Assert.Equal(owner, approval.OwnerUserId);

        var sent = notifications.Calls.Where(call => call.Method == nameof(INotificationService.CreateAsync)).ToList();
        Assert.Equal(owner, Assert.Single(sent).Args[0]);

        var list = await Service(context).GetListAsync(new BuildingFilterParams());
        Assert.Equal(created.Id, Assert.Single(list.Items).Id);
        Assert.Equal(created.Id, (await Service(context).GetByIdAsync(created.Id)).Id);
    }

    [Fact]
    public async Task A_failing_realtime_hub_never_rolls_back_an_approval()
    {
        using var context = NewContext(new FailingPublisher());
        var owner = SeedUser(context);

        var created = await Service(context).CreateAsync(owner, Valid(BuildingType.Villa), [Upload()], null);

        var (admin, _) = Admin(context);
        await admin.ApproveAsync(Module, created.Id, "admin", new ApproveListingRequest());

        Assert.Equal(ModerationStatus.Approved, (await context.Buildings.IgnoreQueryFilters().SingleAsync()).ModerationStatus);
        Assert.Equal(1, (await Service(context).GetListAsync(new BuildingFilterParams())).TotalCount);
    }

    [Fact]
    public async Task Rejection_keeps_the_listing_private_and_owner_still_sees_it_in_my_listings()
    {
        using var context = NewContext();
        var owner = SeedUser(context);
        var created = await Service(context).CreateAsync(owner, Valid(BuildingType.House), [Upload()], null);

        var (admin, _) = Admin(context);
        await admin.RejectAsync(Module, created.Id, "admin",
            new RejectListingRequest { Reason = ListingRejectionReason.Other, Notes = "صور غير واضحة" });

        Assert.Equal(0, (await Service(context).GetListAsync(new BuildingFilterParams())).TotalCount);

        var mine = await new BuildingListingSource(context).Query(owner, DateTime.UtcNow).ToListAsync();
        var row = Assert.Single(mine);
        Assert.Equal(ListingStatus.Rejected, row.Status);
        Assert.Equal(Module, row.Type);
    }

    [Fact]
    public async Task Search_filters_and_the_advertiser_lookup_only_see_published_listings()
    {
        using var context = NewContext();
        var owner = SeedUser(context);
        var service = Service(context);

        async Task<Guid> Create(BuildingType type, string advertiser, bool approve, Action<CreateBuildingRequest>? edit = null)
        {
            var request = Valid(type);
            request.AdvertiserName = advertiser;
            edit?.Invoke(request);
            var created = await service.CreateAsync(owner, request, [Upload()], null);
            if (approve)
            {
                var entity = await context.Buildings.IgnoreQueryFilters().SingleAsync(b => b.Id == created.Id);
                entity.ModerationStatus = ModerationStatus.Approved;
                entity.PublishedAt = DateTime.UtcNow;
                entity.ExpireAt = DateTime.UtcNow.AddDays(30);
                await context.SaveChangesAsync();
            }
            return created.Id;
        }

        await Create(BuildingType.Villa, "مكتب النور", true, r => r.HasSwimmingPool = true);
        await Create(BuildingType.Villa, "مكتب النور", true);
        await Create(BuildingType.ApartmentBuilding, "شركة الأمل", true, r => r.HasElevator = true);
        await Create(BuildingType.House, "معلن قيد المراجعة", false);

        Assert.Equal(3, (await service.GetListAsync(new BuildingFilterParams())).TotalCount);
        Assert.Equal(2, (await service.GetListAsync(new BuildingFilterParams { BuildingType = BuildingType.Villa })).TotalCount);
        Assert.Equal(1, (await service.GetListAsync(new BuildingFilterParams { HasSwimmingPool = true })).TotalCount);
        Assert.Equal(1, (await service.GetListAsync(new BuildingFilterParams { HasElevator = true })).TotalCount);
        Assert.Equal(2, (await service.GetListAsync(new BuildingFilterParams { AdvertiserName = "مكتب النور" })).TotalCount);

        var page = await service.GetListAsync(new BuildingFilterParams { PageSize = 2, PageIndex = 2 });
        Assert.Single(page.Items);
        Assert.Equal(3, page.TotalCount);

        var advertisers = await AdvertiserService(context)
            .GetAdvertisersAsync(Category, SubCategory, new AdvertiserLookupParams());
        Assert.Equal(
            new[] { "شركة الأمل:1", "مكتب النور:2" },
            advertisers.Items.Select(i => $"{i.Name}:{i.ListingsCount}").OrderBy(x => x));

        var searched = await AdvertiserService(context)
            .GetAdvertisersAsync(Category, SubCategory, new AdvertiserLookupParams { Search = "الأمل" });
        Assert.Equal("شركة الأمل", Assert.Single(searched.Items).Name);
    }

    [Fact]
    public async Task Every_lookup_key_the_controller_publishes_is_served_from_seeded_rows()
    {
        using var context = NewContext();
        var service = Service(context);

        foreach (var key in new[]
                 {
                     RealEstateLookupKeys.ListingTypes, RealEstateLookupKeys.Projects, RealEstateLookupKeys.BuildingTypes,
                     RealEstateLookupKeys.ReceptionPieces, RealEstateLookupKeys.FurnishedStatuses,
                     RealEstateLookupKeys.FinishingTypes, RealEstateLookupKeys.PropertyAges, RealEstateLookupKeys.Directions,
                     RealEstateLookupKeys.ViewTypes, RealEstateLookupKeys.LegalStatuses,
                     RealEstateLookupKeys.ReconciliationForms, RealEstateLookupKeys.OwnershipDocuments,
                     RealEstateLookupKeys.Features, RealEstateLookupKeys.PaymentMethods,
                     RealEstateLookupKeys.InstallmentProviders, RealEstateLookupKeys.RentTypes,
                     RealEstateLookupKeys.RentInclusions, RealEstateLookupKeys.ExchangeTargets
                 })
            Assert.NotEmpty(await service.GetLookupAsync(key));

        Assert.Equal(25, (await service.GetLookupAsync(RealEstateLookupKeys.Features)).Count);
        await Assert.ThrowsAsync<NotFoundException>(() => service.GetLookupAsync(RealEstateLookupKeys.ApartmentTypes));
    }
}

public class UploadBufferingTests(Xunit.Abstractions.ITestOutputHelper output)
{
    [Fact]
    public async Task An_upload_is_read_into_one_exactly_sized_buffer()
    {
        var payload = new byte[8 * 1024 * 1024];
        Random.Shared.NextBytes(payload);
        var file = new Microsoft.AspNetCore.Http.FormFile(new MemoryStream(payload), 0, payload.Length, "Video", "tour.mp4")
        {
            Headers = new Microsoft.AspNetCore.Http.HeaderDictionary(),
            ContentType = "video/mp4"
        };

        var before = GC.GetAllocatedBytesForCurrentThread();
        var upload = await Presentation.Extensions.FormFileExtensions.ToUploadModelAsync(file);
        var allocated = GC.GetAllocatedBytesForCurrentThread() - before;

        var legacyBefore = GC.GetAllocatedBytesForCurrentThread();
        using (var memoryStream = new MemoryStream())
        {
            await file.CopyToAsync(memoryStream);
            _ = memoryStream.ToArray();
        }
        var legacyAllocated = GC.GetAllocatedBytesForCurrentThread() - legacyBefore;

        output.WriteLine($"8 MiB upload: exact buffer {allocated:N0} bytes, MemoryStream+ToArray {legacyAllocated:N0} bytes");
        Assert.Equal(payload, upload!.Content);
        Assert.Equal(payload.Length, upload.Length);
        Assert.True(allocated < payload.Length * 1.1, $"allocated {allocated:N0} bytes");
        Assert.True(legacyAllocated > allocated * 2, $"legacy {legacyAllocated:N0} vs {allocated:N0}");
    }
}
