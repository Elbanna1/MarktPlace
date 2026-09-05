using System.Text.RegularExpressions;

namespace MarkatPlace.Tests;

internal static class ArabicText
{
    private static readonly Regex Arabic = new(@"[؀-ۿ]", RegexOptions.Compiled);

    public static bool IsArabic(string? text) =>
        !string.IsNullOrWhiteSpace(text) && Arabic.IsMatch(text);

    public static bool ContainsEnglishProse(string? text) =>
        !string.IsNullOrWhiteSpace(text) && Regex.IsMatch(text, @"[A-Za-z]{4,}");
}
