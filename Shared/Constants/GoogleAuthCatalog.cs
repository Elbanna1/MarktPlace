using System.Text;

namespace Shared.Constants;

public static class GoogleAuthCatalog
{
    public const string ProviderName = "Google";

    public const string ProviderDisplayName = "Google";

    public const string PostMessageRedirectUri = "postmessage";

    public const string TokenEndpoint = "https://oauth2.googleapis.com/token";

    public const int MaxUsernameLength = 50;

    public const int MinUsernameLength = 3;

    public const int MaxPersonNameLength = 50;

    public const int MaxUsernameAttempts = 25;

    public const string FallbackFirstName = "مستخدم";

    public const string FallbackUsernamePrefix = "مستخدم";

    public static string DefaultCenter => LocationConstants.Centers[0];

    public static string DefaultGovernorate => LocationConstants.Governorate;

    public static string SanitizeUsername(string? value) =>
        Sanitize(value, AccountNameRules.IsValidUsername, MaxUsernameLength);

    public static string SanitizePersonName(string? value) =>
        Sanitize(value, AccountNameRules.IsValidPersonName, MaxPersonNameLength);

    public static (string FirstName, string SecondName) SplitDisplayName(
        string? givenName, string? familyName, string? fullName, string? email)
    {
        var first = SanitizePersonName(givenName);
        var second = SanitizePersonName(familyName);

        if (first.Length == 0 || second.Length == 0)
        {
            var parts = SanitizePersonName(fullName)
                .Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (first.Length == 0 && parts.Length > 0)
                first = parts[0];

            if (second.Length == 0 && parts.Length > 1)
                second = string.Join(' ', parts.Skip(1));
        }

        if (first.Length == 0)
            first = SanitizePersonName(LocalPartOf(email));

        if (first.Length == 0)
            first = FallbackFirstName;

        if (second.Length == 0)
            second = first;

        return (Truncate(first, MaxPersonNameLength), Truncate(second, MaxPersonNameLength));
    }

    public static string SuggestUsername(string? fullName, string? email)
    {
        var fromEmail = SanitizeUsername(LocalPartOf(email));

        if (IsUsable(fromEmail))
            return fromEmail;

        var fromName = SanitizeUsername(fullName);

        return IsUsable(fromName) ? fromName : FallbackUsernamePrefix;
    }

    public static string UsernameCandidate(string baseName, int attempt)
    {
        var suffix = attempt <= 0 ? string.Empty : attempt.ToString();
        var room = MaxUsernameLength - suffix.Length;
        var trimmed = Truncate(baseName, room);

        if (trimmed.Length == 0)
            trimmed = FallbackUsernamePrefix;

        return trimmed + suffix;
    }

    public static string LocalPartOf(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return string.Empty;

        var at = email.IndexOf('@');
        return at <= 0 ? email.Trim() : email[..at].Trim();
    }

    private static bool IsUsable(string candidate) =>
        candidate.Length >= MinUsernameLength;

    private static string Sanitize(string? value, Func<string?, bool> accepts, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;

        var builder = new StringBuilder();

        foreach (var character in AccountNameRules.NormalizeWhitespace(value))
            builder.Append(character == ' ' || accepts(character.ToString()) ? character : ' ');

        var cleaned = AccountNameRules.NormalizeWhitespace(builder.ToString());

        return accepts(cleaned) ? Truncate(cleaned, maxLength) : string.Empty;
    }

    private static string Truncate(string value, int maxLength)
    {
        if (maxLength <= 0)
            return string.Empty;

        return value.Length <= maxLength
            ? value
            : AccountNameRules.NormalizeWhitespace(value[..maxLength]);
    }
}
