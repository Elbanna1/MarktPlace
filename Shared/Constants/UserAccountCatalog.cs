using Shared.Enums;

namespace Shared.Constants;

public static class UserAccountCatalog
{
    public static readonly IReadOnlyDictionary<UserAccountStatus, string> StatusNames =
        new Dictionary<UserAccountStatus, string>
        {
            [UserAccountStatus.Active] = "فعال",
            [UserAccountStatus.Suspended] = "موقوف",
            [UserAccountStatus.Blocked] = "محظور"
        };

    public static string GetStatusName(UserAccountStatus status) =>
        StatusNames.TryGetValue(status, out var name) ? name : status.ToString();

    public static IReadOnlyList<(UserAccountStatus Status, string Name)> Options { get; } =
        StatusNames.Select(entry => (entry.Key, entry.Value)).ToList();
}
