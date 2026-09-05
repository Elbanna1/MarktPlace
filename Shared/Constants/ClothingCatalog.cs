using Shared.DTOs.Clothing;

namespace Shared.Constants;

public readonly record struct ClothingLookupEntry<TValue>(TValue Value, string Name, string NameEn)
    where TValue : struct, Enum;

public static class ClothingCatalog
{
    public static IReadOnlyList<ClothingLookupItemDto> ToOptions<TValue>(
        IEnumerable<ClothingLookupEntry<TValue>> entries)
        where TValue : struct, Enum =>
        entries
            .Select(entry => new ClothingLookupItemDto
            {
                Id = Convert.ToInt32(entry.Value),
                Name = entry.Name,
                NameEn = entry.NameEn
            })
            .ToList();

    public static string GetName<TValue>(IReadOnlyList<ClothingLookupEntry<TValue>> entries, TValue value)
        where TValue : struct, Enum
    {
        foreach (var entry in entries)
        {
            if (EqualityComparer<TValue>.Default.Equals(entry.Value, value))
                return entry.Name;
        }

        return string.Empty;
    }

    public static string GetNameEn<TValue>(IReadOnlyList<ClothingLookupEntry<TValue>> entries, TValue value)
        where TValue : struct, Enum
    {
        foreach (var entry in entries)
        {
            if (EqualityComparer<TValue>.Default.Equals(entry.Value, value))
                return entry.NameEn;
        }

        return string.Empty;
    }

    public static List<ClothingLookupItemDto> SelectedOptions<TValue>(
        IReadOnlyList<ClothingLookupEntry<TValue>> entries, IEnumerable<TValue> selected)
        where TValue : struct, Enum
    {
        var chosen = selected.ToHashSet();

        return entries
            .Where(entry => chosen.Contains(entry.Value))
            .Select(entry => new ClothingLookupItemDto
            {
                Id = Convert.ToInt32(entry.Value),
                Name = entry.Name,
                NameEn = entry.NameEn
            })
            .ToList();
    }
}
