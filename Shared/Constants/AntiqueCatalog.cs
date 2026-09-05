using Shared.DTOs.Antiques;

namespace Shared.Constants;

public readonly record struct AntiqueLookupEntry<TValue>(TValue Value, string Name, string NameEn)
    where TValue : struct, Enum;

public static class AntiqueCatalog
{
    public static IReadOnlyList<AntiqueLookupItemDto> ToOptions<TValue>(
        IEnumerable<AntiqueLookupEntry<TValue>> entries)
        where TValue : struct, Enum =>
        entries
            .Select(entry => new AntiqueLookupItemDto
            {
                Id = Convert.ToInt32(entry.Value),
                Name = entry.Name,
                NameEn = entry.NameEn
            })
            .ToList();

    public static string GetName<TValue>(IReadOnlyList<AntiqueLookupEntry<TValue>> entries, TValue value)
        where TValue : struct, Enum
    {
        foreach (var entry in entries)
        {
            if (EqualityComparer<TValue>.Default.Equals(entry.Value, value))
                return entry.Name;
        }

        return string.Empty;
    }

    public static string GetName<TValue>(IReadOnlyList<AntiqueLookupEntry<TValue>> entries, TValue? value)
        where TValue : struct, Enum =>
        value is { } resolved ? GetName(entries, resolved) : string.Empty;

    public const int MinYear = 1000;

    public static int MaxYear => DateTime.UtcNow.Year;
}
