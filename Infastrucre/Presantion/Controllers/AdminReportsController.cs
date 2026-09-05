using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Listings;
using Shared.Responses;
using Shared.Authorization;
using Shared.Enums;

namespace Presentation.Controllers;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
[Route("api/admin/reports")]
[Authorize(Roles = AppRoles.Admin)]
[Produces("application/json")]
[AdminPage(AdminPageCatalog.Keys.Reports)]
public class AdminReportsController : ControllerBase
{
    private readonly IListingInteractionService _interactions;

    public AdminReportsController(IListingInteractionService interactions)
    {
        _interactions = interactions;
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<ListingReportDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<ListingReportDto>>>> GetReports(
        [FromQuery] ListingReportFilterParams filter)
    {
        var result = await _interactions.GetReportsAsync(filter, HttpContext.RequestAborted);

        return Ok(ApiResponse<PaginatedResult<ListingReportDto>>.Ok(result, "تم استرجاع البلاغات."));
    }

    [RequireAdminPermission(AdminPermission.Manage)]
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<ListingReportDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ListingReportDto>>> UpdateReport(
        Guid id, [FromBody] UpdateListingReportRequest request)
    {
        var adminUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var result = await _interactions.UpdateReportAsync(
            id, adminUserId, request, HttpContext.RequestAborted);

        return Ok(ApiResponse<ListingReportDto>.Ok(result, "تم تحديث حالة البلاغ."));
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet("metadata")]
    [ProducesResponseType(typeof(ApiResponse<ListingReportMetadataDto>), StatusCodes.Status200OK)]
    public ActionResult<ApiResponse<ListingReportMetadataDto>> GetMetadata()
    {
        var metadata = new ListingReportMetadataDto
        {
            Reasons = ListingInteractionCatalog.ReportReasonNames
                .Select(entry => new ListingReportOptionDto { Id = (int)entry.Key, Name = entry.Value })
                .ToList(),
            Statuses = ListingInteractionCatalog.ReportStatusNames
                .Select(entry => new ListingReportOptionDto { Id = (int)entry.Key, Name = entry.Value })
                .ToList()
        };

        return Ok(ApiResponse<ListingReportMetadataDto>.Ok(metadata, "تم استرجاع بيانات البلاغات."));
    }
}
