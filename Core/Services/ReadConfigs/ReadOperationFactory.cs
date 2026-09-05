using Shared.DTOs.Lookups.Forms;
using Shared.DTOs.Lookups.Read;

namespace Services.ReadConfigs;

internal static class ReadOperationFactory
{
    public const string AdsRoute = "/api/ads";
    public const string WorkshopsRoute = "/api/workshops";
    public const string CraftsmenRoute = "/api/craftsmen";
    public const string LostFoundRoute = "/api/lost-found";
    public const string ProfileRoute = "/api/profile";
    public const string LookupsRoute = "/api/lookups";

    public const string InteractionsRoute = "/api/advertisements";

    public static ReadOperationDto Operation(
        string key, string label, string? labelEn, string endpoint,
        bool requiresAuthentication = false,
        bool paginated = false,
        IReadOnlyList<ReadParameterDto>? routeParameters = null,
        IReadOnlyList<ReadParameterDto>? queryParameters = null,
        IReadOnlyDictionary<string, object>? query = null) =>
        new()
        {
            Key = key,
            Label = label,
            LabelEn = labelEn,
            Endpoint = endpoint,
            RequiresAuthentication = requiresAuthentication,
            Paginated = paginated,
            RouteParameters = routeParameters,
            QueryParameters = queryParameters,
            Query = query
        };

    public static ReadParameterDto Param(
        string name, string label, string? labelEn, string type,
        bool required = false,
        object? defaultValue = null,
        decimal? min = null,
        decimal? max = null,
        List<FormFieldOptionDto>? options = null,
        string? optionsSource = null,
        string? optionsValue = null,
        bool multiple = false) =>
        new()
        {
            Name = name,
            Label = label,
            LabelEn = labelEn,
            Type = type,
            Required = required,
            Multiple = multiple,
            DefaultValue = defaultValue,
            MinValue = min,
            MaxValue = max,
            Options = options,
            OptionsSource = optionsSource,
            OptionsValue = optionsValue
        };

    public static List<FormFieldOptionDto> EnumOptions<TEnum>(IReadOnlyDictionary<TEnum, string> names)
        where TEnum : struct, Enum =>
        names.Select(entry =>
                new FormFieldOptionDto(Convert.ToInt32(entry.Key), entry.Value, entry.Key.ToString()))
            .ToList();

    public static List<FormFieldOptionDto> EnumOptions<TEnum>(
        params (TEnum Value, string Label, string LabelEn)[] entries)
        where TEnum : struct, Enum =>
        entries.Select(entry =>
                new FormFieldOptionDto(Convert.ToInt32(entry.Value), entry.Label, entry.LabelEn))
            .ToList();

    public static IReadOnlyList<ReadParameterDto> IdRoute() =>
    [
        Param("id", "المعرف", "Id", ReadParameterTypes.Guid, required: true)
    ];

    public static IEnumerable<ReadParameterDto> Paging()
    {
        yield return Param("pageIndex", "رقم الصفحة", "Page index", ReadParameterTypes.Integer,
            defaultValue: 1, min: 1);
        yield return Param("pageSize", "حجم الصفحة", "Page size", ReadParameterTypes.Integer,
            defaultValue: 10, min: 1, max: 50);
    }

    public static IEnumerable<ReadParameterDto> LocationFilters(string centerName)
    {
        yield return Param(centerName, "المدينة (المركز)", "City / center", ReadParameterTypes.String,
            optionsSource: ReadOperationKeys.Centers, optionsValue: ReadOptionsValues.Name);
    }

    public static ReadOperationDto Categories() =>
        Operation(ReadOperationKeys.Categories, "الأقسام الرئيسية", "Categories",
            $"{LookupsRoute}/categories");

    public static ReadOperationDto CategoriesTree() =>
        Operation(ReadOperationKeys.CategoriesTree, "شجرة الأقسام", "Categories tree",
            $"{LookupsRoute}/categories-tree");

    public static ReadOperationDto SubCategories(int categoryId) =>
        Operation(ReadOperationKeys.SubCategories, "الأقسام الفرعية", "Sub categories",
            $"{LookupsRoute}/subcategories/{{categoryId}}",
            routeParameters:
            [
                Param("categoryId", "القسم الرئيسي", "Category", ReadParameterTypes.Integer,
                    required: true, defaultValue: categoryId)
            ]);

    public static ReadOperationDto Governorates() =>
        Operation(ReadOperationKeys.Governorates, "المحافظات", "Governorates",
            $"{LookupsRoute}/governorates");

    public static ReadOperationDto Centers() =>
        Operation(ReadOperationKeys.Centers, "المراكز / المدن", "Centers",
            $"{LookupsRoute}/centers/{{governorateId}}",
            routeParameters:
            [
                Param("governorateId", "المحافظة", "Governorate", ReadParameterTypes.Integer,
                    required: true)
            ]);

    public static ReadOperationDto ListingTypes() =>
        Operation(ReadOperationKeys.ListingTypes, "أنواع الإعلانات", "Listing types",
            $"{LookupsRoute}/listing-types");

    public static ReadOperationDto Features() =>
        Operation(ReadOperationKeys.Features, "المميزات", "Features", $"{LookupsRoute}/features");

    public static ReadOperationDto WorkshopTypes() =>
        Operation(ReadOperationKeys.WorkshopTypes, "أنواع الورش", "Workshop types",
            $"{WorkshopsRoute}/types");

    public static ReadOperationDto Specializations() =>
        Operation(ReadOperationKeys.Specializations, "التخصصات", "Specializations",
            $"{CraftsmenRoute}/specializations");

    public static ReadOperationDto ExperienceLevels() =>
        Operation(ReadOperationKeys.ExperienceLevels, "سنوات الخبرة", "Experience levels",
            $"{CraftsmenRoute}/experience-levels");

    public static ReadOperationDto CreateForm(int categoryId, int? subCategoryId) =>
        Operation(ReadOperationKeys.CreateForm, "نموذج إضافة إعلان", "Create advertisement form",
            $"{LookupsRoute}/create-ad-form/{{categoryId}}/{{subCategoryId}}",
            routeParameters: SelectionRoute(categoryId, subCategoryId));

    public static ReadOperationDto ReadConfig(int categoryId, int? subCategoryId) =>
        Operation(ReadOperationKeys.ReadConfig, "إعدادات القراءة", "Read configuration",
            $"{LookupsRoute}/read-config/{{categoryId}}/{{subCategoryId}}",
            routeParameters: SelectionRoute(categoryId, subCategoryId));

    private static IReadOnlyList<ReadParameterDto> SelectionRoute(int categoryId, int? subCategoryId) =>
    [
        Param("categoryId", "القسم الرئيسي", "Category", ReadParameterTypes.Integer,
            required: true, defaultValue: categoryId),
        Param("subCategoryId", "القسم الفرعي", "Sub category", ReadParameterTypes.Integer,
            required: false, defaultValue: subCategoryId)
    ];
}
