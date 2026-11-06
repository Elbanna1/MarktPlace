using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.DTOs.Catalog;
using Shared.Responses;

namespace Presentation.Controllers;

[ApiController]
[Route("api/v2/public/catalog-configuration")]
[AllowAnonymous]
[Tags("Catalog Configuration")]
[Produces("application/json")]
public class CatalogConfigurationController : ControllerBase
{
    private readonly ICatalogConfigurationService _configuration;

    public CatalogConfigurationController(ICatalogConfigurationService configuration)
    {
        _configuration = configuration;
    }

    [HttpGet]
    [EndpointSummary("Effective visibility and payment for every category, sub-category and module (public, read-only)")]
    [EndpointDescription(
        "Anonymous. Only effective values: per area isVisible and isEnabled, and paymentRequired, paymentAmount, " +
        "currency and isFree. Schedules, inheritance and the always-free charity category are already applied.")]
    [ProducesResponseType(typeof(ApiResponse<PublicCatalogConfigurationDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PublicCatalogConfigurationDto>>> Get()
    {
        var result = await _configuration.GetPublicAsync(HttpContext.RequestAborted);

        Response.Headers.CacheControl = "no-store";

        return Ok(ApiResponse<PublicCatalogConfigurationDto>.Ok(result, "تم استرجاع إعدادات الأقسام."));
    }
}
