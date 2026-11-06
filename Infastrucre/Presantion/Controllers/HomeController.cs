using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Filters;
using ServicesAbstraction;
using Shared.Catalog;
using Shared.DTOs.Home;
using Shared.Enums;
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
    private readonly ICatalogConfigurationProvider? _catalog;

    public HomeController(IHomeService home, ICatalogConfigurationProvider? catalog = null)
    {
        _home = home;
        _catalog = catalog;
    }

    [HttpGet]
    [HttpCache(MaxAgeSeconds = 60, Scope = HttpCacheScope.Shared)]
    [ProducesResponseType(typeof(ApiResponse<HomeConfigurationDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<HomeConfigurationDto>>> Get()
    {
        var result = await _home.GetAsync(HttpContext.RequestAborted);

        if (_catalog is not null)
        {
            var snapshot = await _catalog.GetSnapshotAsync(HttpContext.RequestAborted);
            var now = DateTime.UtcNow;

            bool Shown(int? categoryId) =>
                categoryId is not { } id ||
                snapshot.IsVisible(CatalogContext.ForCategory(id), CatalogArea.HomePage, now);

            result = new HomeConfigurationDto
            {
                Sections = result.Sections.Where(section => Shown(section.CategoryId)).ToList(),
                Categories = result.Categories.Where(category => Shown(category.Id)).ToList()
            };
        }

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
    [HttpCache(MaxAgeSeconds = 300, Scope = HttpCacheScope.Shared)]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<ProjectDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<ProjectDto>>>> Get(
        [FromQuery] int? centerId)
    {
        var result = await _lookups.GetProjectsAsync(centerId, HttpContext.RequestAborted);

        return Ok(ApiResponse<IReadOnlyList<ProjectDto>>.Ok(result, "تم استرجاع المشاريع."));
    }
}
