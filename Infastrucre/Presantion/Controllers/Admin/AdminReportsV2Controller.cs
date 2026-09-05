using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Admin;
using Shared.DTOs.Listings;
using Shared.Enums;
using Shared.Responses;
using Shared.Authorization;

namespace Presentation.Controllers.Admin;

[Route(ApiVersions.AdminRoutePrefix + "/reports")]
[Tags("Admin Reports")]
[AdminPage(AdminPageCatalog.Keys.Reports)]
public class AdminReportsV2Controller : AdminApiController
{
    private readonly IListingInteractionService _interactions;
    private readonly IAdminAdService _ads;

    public AdminReportsV2Controller(IListingInteractionService interactions, IAdminAdService ads)
    {
        _interactions = interactions;
        _ads = ads;
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<ListingReportDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<ListingReportDto>>>> GetReports(
        [FromQuery] ListingReportFilterParams filter)
    {
        var result = await _interactions.GetReportsAsync(filter, RequestAborted);

        return Ok(ApiResponse<PaginatedResult<ListingReportDto>>.Ok(result, "تم استرجاع البلاغات."));
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

    [RequireAdminPermission(AdminPermission.Manage)]
    [HttpPatch("{id:guid}/ignore")]
    [ProducesResponseType(typeof(ApiResponse<ListingReportDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ListingReportDto>>> Ignore(
        Guid id, [FromBody] AdminReportDecisionRequest? request)
    {
        var result = await _interactions.UpdateReportAsync(
            id,
            CurrentAdminId,
            new UpdateListingReportRequest
            {
                Status = ListingReportStatus.Dismissed,
                AdminNote = request?.Note
            },
            RequestAborted);

        return Ok(ApiResponse<ListingReportDto>.Ok(result, "تم تجاهل البلاغ."));
    }

    [RequireAdminPermission(AdminPermission.Manage)]
    [HttpPatch("{id:guid}/action")]
    [ProducesResponseType(typeof(ApiResponse<ListingReportDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ListingReportDto>>> TakeAction(
        Guid id, [FromBody] AdminReportActionRequest request)
    {
        var report = await _interactions.UpdateReportAsync(
            id,
            CurrentAdminId,
            new UpdateListingReportRequest
            {
                Status = ListingReportStatus.ActionTaken,
                AdminNote = request.Note
            },
            RequestAborted);

        var message = "تم اتخاذ إجراء بشأن البلاغ.";

        switch (request.Action)
        {
            case AdminReportAction.Suspend:
                await _ads.SuspendAsync(
                    report.ListingType,
                    report.ListingId,
                    CurrentAdminId,
                    new SuspendListingRequest { Notes = request.Note },
                    RequestAborted);

                message = "تم إيقاف الإعلان بناءً على البلاغ.";
                break;

            case AdminReportAction.Delete:
                await _ads.DeleteAsync(
                    report.ListingType, report.ListingId, CurrentAdminId, RequestAborted);

                message = "تم حذف الإعلان بناءً على البلاغ.";
                break;
        }

        return Ok(ApiResponse<ListingReportDto>.Ok(report, message));
    }

    [RequireAdminPermission(AdminPermission.Manage)]
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<ListingReportDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ListingReportDto>>> UpdateStatus(
        Guid id, [FromBody] UpdateListingReportRequest request)
    {
        var result = await _interactions.UpdateReportAsync(id, CurrentAdminId, request, RequestAborted);

        return Ok(ApiResponse<ListingReportDto>.Ok(result, "تم تحديث حالة البلاغ."));
    }
}
