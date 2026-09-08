using Shared.Enums;

namespace Shared.Constants;

public static class UserAccountCatalog
{
    public static readonly IReadOnlyDictionary<UserAccountStatus, string> StatusNames =
        new Dictionary<UserAccountStatus, string>
        {
            [UserAccountStatus.Active] = "فعال",
            [UserAccountStatus.Suspended] = "موقوف",
            [UserAccountStatus.Blocked] = "محظور",
            [UserAccountStatus.Deactivated] = "مقفول بطلب صاحبه"
        };

    public static readonly IReadOnlyList<UserAccountStatus> AdminAssignableStatuses =
    [
        UserAccountStatus.Active,
        UserAccountStatus.Suspended,
        UserAccountStatus.Blocked
    ];

    public static bool IsAdminAssignable(UserAccountStatus status) =>
        AdminAssignableStatuses.Contains(status);

    public static string GetStatusName(UserAccountStatus status) =>
        StatusNames.TryGetValue(status, out var name) ? name : status.ToString();

    public static IReadOnlyList<(UserAccountStatus Status, string Name)> Options { get; } =
        AdminAssignableStatuses.Select(status => (status, GetStatusName(status))).ToList();
}
