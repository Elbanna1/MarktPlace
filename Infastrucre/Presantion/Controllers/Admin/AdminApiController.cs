using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;

namespace Presentation.Controllers.Admin;

[ApiController]
[Authorize(Roles = AppRoles.Admin)]
[ApiExplorerSettings(GroupName = ApiVersions.AdminV2)]
[Produces("application/json")]
public abstract class AdminApiController : ControllerBase
{
    protected string CurrentAdminId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    protected CancellationToken RequestAborted => HttpContext.RequestAborted;
}
