using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.DTOs.Banners;
using Shared.Responses;

namespace Presentation.Controllers;

[ApiController]
[Route("api/banners")]
[AllowAnonymous]
[Produces("application/json")]
public class BannersController : ControllerBase
{
    private readonly IBannerService _bannerService;

    public BannersController(IBannerService bannerService)
    {
        _bannerService = bannerService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<BannerDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<BannerDto>>>> GetActive()
    {
        var result = await _bannerService.GetActiveAsync(HttpContext.RequestAborted);

        return Ok(ApiResponse<IReadOnlyList<BannerDto>>.Ok(result, "تم استرجاع البانرات."));
    }
}
