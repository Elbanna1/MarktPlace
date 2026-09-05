using Shared.DTOs.OnlineShopping;

namespace Shared.Constants;

public readonly record struct OnlineShoppingLookupEntry<TValue>(TValue Value, string Name, string NameEn)
    where TValue : struct, Enum;

public static class OnlineShoppingCatalog
{
    public static IReadOnlyList<OnlineShoppingLookupItemDto> ToOptions<TValue>(
        IEnumerable<OnlineShoppingLookupEntry<TValue>> entries)
        where TValue : struct, Enum =>
        entries
            .Select(entry => new OnlineShoppingLookupItemDto
            {
                Id = Convert.ToInt32(entry.Value),
                Name = entry.Name,
                NameEn = entry.NameEn
            })
            .ToList();

    public static string GetName<TValue>(
        IReadOnlyList<OnlineShoppingLookupEntry<TValue>> entries, TValue value)
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
        IReadOnlyList<OnlineShoppingLookupEntry<TValue>> entries, TValue value)
        where TValue : struct, Enum
    {
        foreach (var entry in entries)
        {
            if (EqualityComparer<TValue>.Default.Equals(entry.Value, value))
                return entry.NameEn;
        }

        return string.Empty;
    }

    public static List<OnlineShoppingLookupItemDto> SelectedOptions<TValue>(
        IReadOnlyList<OnlineShoppingLookupEntry<TValue>> entries, IEnumerable<TValue> selected)
        where TValue : struct, Enum
    {
        var chosen = selected.ToHashSet();

        return entries
            .Where(entry => chosen.Contains(entry.Value))
            .Select(entry => new OnlineShoppingLookupItemDto
            {
                Id = Convert.ToInt32(entry.Value),
                Name = entry.Name,
                NameEn = entry.NameEn
            })
            .ToList();
    }
}
