using Shared.DTOs.Animals;

namespace Shared.Constants;

public readonly record struct AnimalLookupEntry<TValue>(TValue Value, string Name, string NameEn)
    where TValue : struct, Enum;

public static class AnimalCatalog
{
    public static IReadOnlyList<AnimalLookupItemDto> ToOptions<TValue>(
        IEnumerable<AnimalLookupEntry<TValue>> entries)
        where TValue : struct, Enum =>
        entries
            .Select(entry => new AnimalLookupItemDto
            {
                Id = Convert.ToInt32(entry.Value),
                Name = entry.Name,
                NameEn = entry.NameEn
            })
            .ToList();

    public static string GetName<TValue>(IReadOnlyList<AnimalLookupEntry<TValue>> entries, TValue value)
        where TValue : struct, Enum
    {
        foreach (var entry in entries)
        {
            if (EqualityComparer<TValue>.Default.Equals(entry.Value, value))
                return entry.Name;
        }

        return string.Empty;
    }

    public static string GetName<TValue>(IReadOnlyList<AnimalLookupEntry<TValue>> entries, TValue? value)
        where TValue : struct, Enum =>
        value is { } resolved ? GetName(entries, resolved) : string.Empty;
}
