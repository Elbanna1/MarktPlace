using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Extensions;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.FruitVegetableMerchants;
using Shared.Responses;

namespace Presentation.Controllers;

[ApiController]
[Route("api/fruit-vegetable-merchants")]
[Authorize]
[Produces("application/json")]
public class FruitVegetableMerchantsController : ControllerBase
{
    private readonly IFruitVegetableMerchantService _merchantService;

    public FruitVegetableMerchantsController(IFruitVegetableMerchantService merchantService)
    {
        _merchantService = merchantService;
    }

    private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    private bool IsAdmin => User.IsInRole(AppRoles.Admin);

    [HttpPost]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(ImageConstants.MaxRequestBodySizeBytes)]
    [ProducesResponseType(typeof(ApiResponse<FruitVegetableMerchantDetailsDto>), StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<FruitVegetableMerchantDetailsDto>>> Create(
        [FromForm] CreateFruitVegetableMerchantRequest request)
    {
        var images = await Request.FormFilesOrEmpty().ToUploadModelsAsync(HttpContext.RequestAborted);
        var result = await _merchantService.CreateAsync(CurrentUserId, request, images);
        return StatusCode(StatusCodes.Status201Created,
            ApiResponse<FruitVegetableMerchantDetailsDto>.Ok(result, UserMessages.Listings.Created));
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<FruitVegetableMerchantListItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<FruitVegetableMerchantListItemDto>>>> GetList(
        [FromQuery] FruitVegetableMerchantFilterParams filter)
    {
        var result = await _merchantService.GetListAsync(filter, HttpContext.RequestAborted);
        return Ok(ApiResponse<PaginatedResult<FruitVegetableMerchantListItemDto>>.Ok(
            result, UserMessages.Listings.Loaded));
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<FruitVegetableMerchantDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<FruitVegetableMerchantDetailsDto>>> GetById(Guid id)
    {
        var result = await _merchantService.GetByIdAsync(id, HttpContext.RequestAborted);
        return Ok(ApiResponse<FruitVegetableMerchantDetailsDto>.Ok(result, UserMessages.Listings.DetailsLoaded));
    }

    [HttpPut("{id:guid}")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(ImageConstants.MaxRequestBodySizeBytes)]
    [ProducesResponseType(typeof(ApiResponse<FruitVegetableMerchantDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<FruitVegetableMerchantDetailsDto>>> Update(
        Guid id, [FromForm] UpdateFruitVegetableMerchantRequest request)
    {
        var newImages = await Request.FormFilesOrEmpty().ToUploadModelsAsync(HttpContext.RequestAborted);
        var result = await _merchantService.UpdateAsync(CurrentUserId, IsAdmin, id, request, newImages);
        return Ok(ApiResponse<FruitVegetableMerchantDetailsDto>.Ok(result, UserMessages.Listings.Updated));
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse>> Delete(Guid id)
    {
        await _merchantService.DeleteAsync(CurrentUserId, id, IsAdmin);
        return Ok(ApiResponse.Ok(UserMessages.Listings.Deleted));
    }

    [HttpGet("sale-types")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<MerchantSaleTypeDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<MerchantSaleTypeDto>>>> GetSaleTypes()
    {
        var result = await _merchantService.GetSaleTypesAsync(HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<MerchantSaleTypeDto>>.Ok(result, UserMessages.Lookups.Loaded));
    }
}
