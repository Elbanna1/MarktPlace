using System.Security.Cryptography;
using Shared.Enums;

namespace Shared.Constants;

public static class ReferralCatalog
{
    public const string CodeAlphabet = "23456789ABCDEFGHJKMNPQRSTUVWXYZ";

    public const int CodeLength = 8;

    public const int MaxCodeLength = 32;

    public const int MaxGenerationAttempts = 8;

    public const string DefaultRegisterPath = "/register";

    public const string DefaultQueryParameter = "ref";

    public static string? Normalize(string? code)
    {
        if (string.IsNullOrWhiteSpace(code))
            return null;

        var trimmed = code.Trim();

        return trimmed.Length > MaxCodeLength ? null : trimmed.ToUpperInvariant();
    }

    public static string GenerateCode()
    {
        var buffer = new char[CodeLength];

        for (var i = 0; i < CodeLength; i++)
            buffer[i] = CodeAlphabet[RandomNumberGenerator.GetInt32(CodeAlphabet.Length)];

        return new string(buffer);
    }

    public static string NameOf(ReferralStatus status) => status switch
    {
        ReferralStatus.Pending => "قيد التفعيل",
        ReferralStatus.Completed => "مكتملة",
        _ => "غير معروف"
    };

    public static IReadOnlyList<(ReferralStatus Status, string Name)> Options { get; } =
        Enum.GetValues<ReferralStatus>()
            .Select(status => (status, NameOf(status)))
            .ToList();
}
