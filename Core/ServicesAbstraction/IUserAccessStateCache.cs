using Shared.Enums;

namespace ServicesAbstraction;

public sealed record UserAccessState(
    bool Exists, UserAccountStatus Status, IReadOnlyList<string> Roles)
{
    public static readonly UserAccessState Missing =
        new(false, UserAccountStatus.Blocked, Array.Empty<string>());
}

public interface IUserAccessStateCache
{
    bool TryGet(string userId, out UserAccessState state);

    void Set(string userId, UserAccessState state);

    void Invalidate(string userId);
}
