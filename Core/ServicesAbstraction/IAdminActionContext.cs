namespace ServicesAbstraction;

public interface IAdminActionContext
{
    string? UserId { get; }

    string? UserName { get; }

    string? IpAddress { get; }

    bool IsAdmin { get; }
}
