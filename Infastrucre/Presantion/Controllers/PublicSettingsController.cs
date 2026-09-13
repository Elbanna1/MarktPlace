using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Filters;
using ServicesAbstraction;
using Shared.DTOs.Settings;
using Shared.Responses;

namespace Presentation.Controllers;

[ApiController]
[Route("api/v2/settings")]
[AllowAnonymous]
[Produces("application/json")]
public class PublicSettingsController : ControllerBase
{
    private readonly IPublicSettingsService _settings;

    public PublicSettingsController(IPublicSettingsService settings)
    {
        _settings = settings;
    }

    [HttpGet]
    [HttpCache(Revalidate = true)]
    [ProducesResponseType(typeof(ApiResponse<PublicSettingsDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PublicSettingsDto>>> Get()
    {
        var result = await _settings.GetAsync(HttpContext.RequestAborted);

        return Ok(ApiResponse<PublicSettingsDto>.Ok(result, "تم استرجاع إعدادات المنصة."));
    }
}
