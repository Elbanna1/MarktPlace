using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Extensions;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.WholesaleTraders;
using Shared.Responses;

namespace Presentation.Controllers;

[ApiController]
[Route("api/wholesale-traders")]
[Authorize]
[Produces("application/json")]
public class WholesaleTradersController : ControllerBase
{
    private readonly IWholesaleTraderService _wholesaleTraderService;

    public WholesaleTradersController(IWholesaleTraderService wholesaleTraderService)
    {
        _wholesaleTraderService = wholesaleTraderService;
    }

    private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    private bool IsAdmin => User.IsInRole(AppRoles.Admin);

    [HttpPost]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(ImageConstants.MaxRequestBodySizeBytes)]
    [ProducesResponseType(typeof(ApiResponse<WholesaleTraderDetailsDto>), StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<WholesaleTraderDetailsDto>>> Create(
        [FromForm] CreateWholesaleTraderRequest request)
    {
        var images = await Request.FormFilesOrEmpty().ToUploadModelsAsync(HttpContext.RequestAborted);
        var result = await _wholesaleTraderService.CreateAsync(CurrentUserId, request, images);
        return StatusCode(StatusCodes.Status201Created,
            ApiResponse<WholesaleTraderDetailsDto>.Ok(result, UserMessages.Listings.Created));
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<WholesaleTraderListItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<WholesaleTraderListItemDto>>>> GetList(
        [FromQuery] WholesaleTraderFilterParams filter)
    {
        var result = await _wholesaleTraderService.GetListAsync(filter, HttpContext.RequestAborted);
        return Ok(ApiResponse<PaginatedResult<WholesaleTraderListItemDto>>.Ok(
            result, UserMessages.Listings.Loaded));
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<WholesaleTraderDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<WholesaleTraderDetailsDto>>> GetById(Guid id)
    {
        var result = await _wholesaleTraderService.GetByIdAsync(id, HttpContext.RequestAborted);
        return Ok(ApiResponse<WholesaleTraderDetailsDto>.Ok(result, UserMessages.Listings.DetailsLoaded));
    }

    [HttpPut("{id:guid}")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(ImageConstants.MaxRequestBodySizeBytes)]
    [ProducesResponseType(typeof(ApiResponse<WholesaleTraderDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<WholesaleTraderDetailsDto>>> Update(
        Guid id, [FromForm] UpdateWholesaleTraderRequest request)
    {
        var newImages = await Request.FormFilesOrEmpty().ToUploadModelsAsync(HttpContext.RequestAborted);
        var result = await _wholesaleTraderService.UpdateAsync(CurrentUserId, IsAdmin, id, request, newImages);
        return Ok(ApiResponse<WholesaleTraderDetailsDto>.Ok(result, UserMessages.Listings.Updated));
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse>> Delete(Guid id)
    {
        await _wholesaleTraderService.DeleteAsync(CurrentUserId, id, IsAdmin);
        return Ok(ApiResponse.Ok(UserMessages.Listings.Deleted));
    }

    [HttpGet("trade-types")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<WholesaleTradeTypeDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<WholesaleTradeTypeDto>>>> GetTradeTypes()
    {
        var result = await _wholesaleTraderService.GetTradeTypesAsync(HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<WholesaleTradeTypeDto>>.Ok(result, UserMessages.Lookups.Loaded));
    }

    [HttpGet("sale-types")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<WholesaleSaleTypeDto>>), StatusCodes.Status200OK)]
    public ActionResult<ApiResponse<IReadOnlyList<WholesaleSaleTypeDto>>> GetSaleTypes()
    {
        var result = _wholesaleTraderService.GetSaleTypes();
        return Ok(ApiResponse<IReadOnlyList<WholesaleSaleTypeDto>>.Ok(result, UserMessages.Lookups.Loaded));
    }
}
