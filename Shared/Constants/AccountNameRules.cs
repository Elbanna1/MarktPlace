using System.Text.RegularExpressions;

namespace Shared.Constants;

public static class AccountNameRules
{
    private const string Arabic =
        "ء-ؿ" +
        "ف-ْ" +
        "٠-٩" +
        "پچژکگی";

    private const string Latin = "a-zA-Z0-9";

    private const string UsernameSeparators = "._-";

    public const string UsernamePattern =
        "^[" + Arabic + Latin + UsernameSeparators + "]+" +
        "( [" + Arabic + Latin + UsernameSeparators + "]+)*$";

    public const string PersonNamePattern =
        "^[" + Arabic + Latin + "]+" +
        "( [" + Arabic + Latin + "]+)*$";

    public const string UsernameMessage =
        "اسم المستخدم يقبل حروف عربي أو إنجليزي وأرقام و . _ - ومسافة واحدة بين الكلمات، " +
        "من غير مسافات في الأول أو الآخر.";

    public const string PersonNameMessage =
        "الاسم يقبل حروف عربي أو إنجليزي وأرقام ومسافة واحدة بين الكلمات، " +
        "من غير مسافات في الأول أو الآخر.";

    public static string NormalizeWhitespace(string? value) =>
        string.IsNullOrWhiteSpace(value)
            ? string.Empty
            : WhitespaceRuns.Replace(value.Trim(), " ");

    private static readonly Regex WhitespaceRuns =
        new(@"\s+", RegexOptions.Compiled | RegexOptions.CultureInvariant);

    private static readonly Regex Username =
        new(UsernamePattern, RegexOptions.Compiled | RegexOptions.CultureInvariant);

    private static readonly Regex PersonName =
        new(PersonNamePattern, RegexOptions.Compiled | RegexOptions.CultureInvariant);

    public static bool IsValidUsername(string? value) =>
        Matches(Username, value);

    public static bool IsValidPersonName(string? value) =>
        Matches(PersonName, value);

    private static bool Matches(Regex pattern, string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return true;

        return pattern.IsMatch(NormalizeWhitespace(value));
    }
}
