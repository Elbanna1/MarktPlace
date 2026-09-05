using Shared.DTOs.HomeFurnishing;

namespace Shared.Constants;

public readonly record struct HomeFurnishingLookupEntry<TValue>(TValue Value, string Name, string NameEn)
    where TValue : struct, Enum;

public static class HomeFurnishingCatalog
{
    public static IReadOnlyList<HomeFurnishingLookupItemDto> ToOptions<TValue>(
        IEnumerable<HomeFurnishingLookupEntry<TValue>> entries)
        where TValue : struct, Enum =>
        entries
            .Select(entry => new HomeFurnishingLookupItemDto
            {
                Id = Convert.ToInt32(entry.Value),
                Name = entry.Name,
                NameEn = entry.NameEn
            })
            .ToList();

    public static string GetName<TValue>(
        IReadOnlyList<HomeFurnishingLookupEntry<TValue>> entries, TValue value)
        where TValue : struct, Enum
    {
        foreach (var entry in entries)
        {
            if (EqualityComparer<TValue>.Default.Equals(entry.Value, value))
                return entry.Name;
        }

        return string.Empty;
    }

    public static string GetNameEn<TValue>(
        IReadOnlyList<HomeFurnishingLookupEntry<TValue>> entries, TValue value)
        where TValue : struct, Enum
    {
        foreach (var entry in entries)
        {
            if (EqualityComparer<TValue>.Default.Equals(entry.Value, value))
                return entry.NameEn;
        }

        return string.Empty;
    }

    public static string? GetNameOrNull<TValue>(
        IReadOnlyList<HomeFurnishingLookupEntry<TValue>> entries, TValue? value)
        where TValue : struct, Enum =>
        value is { } chosen ? GetName(entries, chosen) : null;

    public static List<HomeFurnishingLookupItemDto> SelectedOptions<TValue>(
        IReadOnlyList<HomeFurnishingLookupEntry<TValue>> entries, IEnumerable<TValue> selected)
        where TValue : struct, Enum
    {
        var chosen = selected.ToHashSet();

        return entries
            .Where(entry => chosen.Contains(entry.Value))
            .Select(entry => new HomeFurnishingLookupItemDto
            {
                Id = Convert.ToInt32(entry.Value),
                Name = entry.Name,
                NameEn = entry.NameEn
            })
            .ToList();
    }
}
