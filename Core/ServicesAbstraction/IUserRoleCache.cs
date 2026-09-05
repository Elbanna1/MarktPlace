namespace ServicesAbstraction;

public interface IUserRoleCache
{
    bool TryGet(string userId, out IReadOnlyList<string> roles);

    void Set(string userId, IReadOnlyList<string> roles);

    void Invalidate(string userId);
}
