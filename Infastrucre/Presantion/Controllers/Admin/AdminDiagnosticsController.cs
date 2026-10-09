using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.Authorization;
using Shared.Constants;
using Shared.DTOs.Admin;
using Shared.Responses;

namespace Presentation.Controllers.Admin;

[Route(ApiVersions.AdminRoutePrefix + "/diagnostics")]
[Tags("Admin Diagnostics")]
public class AdminDiagnosticsController : AdminApiController
{
    private readonly IUploadsDiagnostics _uploads;

    public AdminDiagnosticsController(IUploadsDiagnostics uploads)
    {
        _uploads = uploads;
    }

    [SuperAdminOnly]
    [HttpGet("uploads")]
    [ProducesResponseType(typeof(ApiResponse<UploadsDiagnosticsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<UploadsDiagnosticsDto>>> Uploads()
    {
        var result = await _uploads.ScanAsync(RequestAborted);

        return Ok(ApiResponse<UploadsDiagnosticsDto>.Ok(result, UserMessages.Listings.StatisticsLoaded));
    }
}
