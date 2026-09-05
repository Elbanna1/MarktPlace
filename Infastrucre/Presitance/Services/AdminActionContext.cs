using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using ServicesAbstraction;
using Shared.Constants;

namespace Persistence.Services;

public class AdminActionContext : IAdminActionContext
{
    private readonly IHttpContextAccessor _accessor;

    public AdminActionContext(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public string? UserId => _accessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

    public string? UserName
    {
        get
        {
            var user = _accessor.HttpContext?.User;

            if (user is null)
                return null;

            var name = user.FindFirstValue("name")
                ?? user.FindFirstValue(ClaimTypes.Name)
                ?? user.Identity?.Name;

            return string.IsNullOrWhiteSpace(name) ? null : name;
        }
    }

    public string? IpAddress => _accessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

    public bool IsAdmin =>
        _accessor.HttpContext?.User.IsInRole(AppRoles.Admin) ?? false;
}
