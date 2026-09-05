using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.DTOs.Home;
using Shared.DTOs.Lookups;
using Shared.Responses;

namespace Presentation.Controllers;

[ApiController]
[Route("api/home")]
[AllowAnonymous]
[Produces("application/json")]
public class HomeController : ControllerBase
{
    private readonly IHomeService _home;

    public HomeController(IHomeService home)
    {
        _home = home;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<HomeConfigurationDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<HomeConfigurationDto>>> Get()
    {
        var result = await _home.GetAsync(HttpContext.RequestAborted);

        return Ok(ApiResponse<HomeConfigurationDto>.Ok(result, "تم استرجاع إعدادات الصفحة الرئيسية."));
    }
}

[ApiController]
[Route("api/projects")]
[AllowAnonymous]
[Produces("application/json")]
public class ProjectsController : ControllerBase
{
    private readonly ILookupService _lookups;

    public ProjectsController(ILookupService lookups)
    {
        _lookups = lookups;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<ProjectDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<ProjectDto>>>> Get(
        [FromQuery] int? centerId)
    {
        var result = await _lookups.GetProjectsAsync(centerId, HttpContext.RequestAborted);

        return Ok(ApiResponse<IReadOnlyList<ProjectDto>>.Ok(result, "تم استرجاع المشاريع."));
    }
}
