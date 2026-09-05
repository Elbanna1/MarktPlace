using Shared.DTOs.RealEstate;
using Shared.Enums;

namespace Shared.Constants;

public readonly record struct RealEstateLookupEntry<TValue>(TValue Value, string Name, string NameEn)
    where TValue : struct, Enum;

public static class RealEstateCatalog
{
    public static readonly IReadOnlyList<RealEstateLookupEntry<RealEstateListingType>> ListingTypes =
        new List<RealEstateLookupEntry<RealEstateListingType>>
        {
            new(RealEstateListingType.Sale, "بيع", "Sale"),
            new(RealEstateListingType.Rent, "إيجار", "Rent"),
            new(RealEstateListingType.Exchange, "بدل", "Exchange")
        };

    public static readonly IReadOnlyList<RealEstateLookupEntry<RealEstateProject>> Projects =
        new List<RealEstateLookupEntry<RealEstateProject>>
        {
            new(RealEstateProject.EbniBeetak, "ابني بيتك", "Ebni Beetak"),
            new(RealEstateProject.SocialHousing, "الإسكان الاجتماعي", "Social Housing"),
            new(RealEstateProject.SakanMisr, "سكن مصر", "Sakan Misr"),
            new(RealEstateProject.DarMisr, "دار مصر", "Dar Misr"),
            new(RealEstateProject.Janna, "جنة", "Janna"),
            new(RealEstateProject.BeitAlWatan, "بيت الوطن", "Beit Al Watan"),
            new(RealEstateProject.FirstDistrict, "الحي الأول", "First District"),
            new(RealEstateProject.SecondDistrict, "الحي الثاني", "Second District"),
            new(RealEstateProject.ThirdDistrict, "الحي الثالث", "Third District"),
            new(RealEstateProject.ServicesArea, "منطقة الخدمات", "Services Area"),
            new(RealEstateProject.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<RealEstateLookupItemDto> ListingTypeOptions = ToOptions(ListingTypes);
    public static readonly IReadOnlyList<RealEstateLookupItemDto> ProjectOptions = ToOptions(Projects);

    public static string GetListingTypeName(RealEstateListingType value) => GetName(ListingTypes, value);
    public static string? GetProjectName(RealEstateProject? value) => GetNameOrNull(Projects, value);

    public const string ProjectCenter = "الفيوم الجديدة";

    public static bool IsProjectCenter(string? center) =>
        string.Equals(center?.Trim(), ProjectCenter, StringComparison.Ordinal);

    public static IReadOnlyList<RealEstateLookupItemDto> ToOptions<TValue>(
        IEnumerable<RealEstateLookupEntry<TValue>> entries)
        where TValue : struct, Enum =>
        entries
            .Select(entry => new RealEstateLookupItemDto
            {
                Id = Convert.ToInt32(entry.Value),
                Name = entry.Name,
                NameEn = entry.NameEn
            })
            .ToList();

    public static string GetName<TValue>(
        IReadOnlyList<RealEstateLookupEntry<TValue>> entries, TValue value)
        where TValue : struct, Enum
    {
        foreach (var entry in entries)
        {
            if (EqualityComparer<TValue>.Default.Equals(entry.Value, value))
                return entry.Name;
        }

        return string.Empty;
    }

    public static string? GetNameOrNull<TValue>(
        IReadOnlyList<RealEstateLookupEntry<TValue>> entries, TValue? value)
        where TValue : struct, Enum =>
        value is { } chosen ? GetName(entries, chosen) : null;

    public static List<RealEstateLookupItemDto> SelectedOptions<TValue>(
        IReadOnlyList<RealEstateLookupEntry<TValue>> entries, IEnumerable<TValue> selected)
        where TValue : struct, Enum
    {
        var chosen = selected.ToHashSet();

        return entries
            .Where(entry => chosen.Contains(entry.Value))
            .Select(entry => new RealEstateLookupItemDto
            {
                Id = Convert.ToInt32(entry.Value),
                Name = entry.Name,
                NameEn = entry.NameEn
            })
            .ToList();
    }
}
