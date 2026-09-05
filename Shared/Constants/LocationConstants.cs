namespace Shared.Constants;

public static class LocationConstants
{
    public const string Governorate = "الفيوم";

    public static readonly IReadOnlyList<string> Centers = new List<string>
    {
        "الفيوم",
        "سنورس",
        "طامية",
        "يوسف الصديق",
        "اطسا",
        "ابشواي",
        "الفيوم الجديدة"
    };

    public static bool IsValidCenter(string? center) =>
        !string.IsNullOrWhiteSpace(center) && Centers.Contains(center);

    public static bool IsValidGovernorate(string? governorate) =>
        string.Equals(governorate, Governorate, StringComparison.Ordinal);

    public const string GovernorateLatin = "fayoum";

    public static bool IsValidGovernorateInput(string? governorate) =>
        IsValidGovernorate(governorate) ||
        string.Equals(governorate?.Trim(), GovernorateLatin, StringComparison.OrdinalIgnoreCase);
}
