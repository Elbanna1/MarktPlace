using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Extensions;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.RealEstate;
using Shared.Exceptions;
using Shared.Responses;

namespace Presentation.Controllers;

[ApiController]
[Route(RealEstateRoutes.Shops)]
[Authorize]
[Produces("application/json")]
public class ShopsController : ControllerBase
{
    private readonly IShopService _service;

    public ShopsController(IShopService service)
    {
        _service = service;
    }

    private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    private bool IsAdmin => User.IsInRole(AppRoles.Admin);

    [HttpPost]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(FileUploadConstants.MaxRealEstateRequestBodySizeBytes)]
    [ProducesResponseType(typeof(ApiResponse<ShopDetailsDto>), StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<ShopDetailsDto>>> Create([FromForm] CreateShopRequest request)
    {
        var cancellationToken = HttpContext.RequestAborted;

        var images = await request.Images.ToUploadModelsAsync(cancellationToken);
        var video = await request.Video.ToUploadModelAsync(cancellationToken);

        var result = await _service.CreateAsync(CurrentUserId, request, images, video, cancellationToken);

        return StatusCode(StatusCodes.Status201Created,
            ApiResponse<ShopDetailsDto>.Ok(result, UserMessages.Listings.Created));
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<ShopListItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<ShopListItemDto>>>> GetList(
        [FromQuery] ShopFilterParams filter)
    {
        var result = await _service.GetListAsync(filter, HttpContext.RequestAborted);
        return Ok(ApiResponse<PaginatedResult<ShopListItemDto>>.Ok(
            result, UserMessages.Listings.Loaded));
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<ShopDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ShopDetailsDto>>> GetById(Guid id)
    {
        var result = await _service.GetByIdAsync(id, countView: true, HttpContext.RequestAborted);
        return Ok(ApiResponse<ShopDetailsDto>.Ok(result, UserMessages.Listings.DetailsLoaded));
    }

    [HttpPut("{id:guid}")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(FileUploadConstants.MaxRealEstateRequestBodySizeBytes)]
    [ProducesResponseType(typeof(ApiResponse<ShopDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ShopDetailsDto>>> Update(
        Guid id, [FromForm] UpdateShopRequest request)
    {
        var cancellationToken = HttpContext.RequestAborted;

        var newImages = await request.Images.ToUploadModelsAsync(cancellationToken);
        var video = await request.Video.ToUploadModelAsync(cancellationToken);

        var result = await _service.UpdateAsync(
            CurrentUserId, IsAdmin, id, request, newImages, video, cancellationToken);

        return Ok(ApiResponse<ShopDetailsDto>.Ok(result, UserMessages.Listings.Updated));
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse>> Delete(Guid id)
    {
        await _service.DeleteAsync(CurrentUserId, id, IsAdmin);
        return Ok(ApiResponse.Ok(UserMessages.Listings.Deleted));
    }

    [HttpPut("{id:guid}/images/order")]
    [ProducesResponseType(typeof(ApiResponse<ShopDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ShopDetailsDto>>> ReorderImages(
        Guid id, [FromBody] ReorderRealEstateImagesRequest request)
    {
        var result = await _service.ReorderImagesAsync(
            CurrentUserId, IsAdmin, id, request, HttpContext.RequestAborted);

        return Ok(ApiResponse<ShopDetailsDto>.Ok(result, UserMessages.Listings.ImagesReordered));
    }

    [HttpPut("{id:guid}/promotion")]
    [ProducesResponseType(typeof(ApiResponse<ShopDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ShopDetailsDto>>> SetPromotion(
        Guid id, [FromBody] PromoteRealEstateRequest request)
    {
        if (!IsAdmin)
            throw new ForbiddenException("الترويج للإعلان متاح للإدارة بس.");

        var result = await _service.SetPromotionAsync(id, request, HttpContext.RequestAborted);
        return Ok(ApiResponse<ShopDetailsDto>.Ok(result, UserMessages.Listings.PromotionUpdated));
    }

    [HttpGet("{id:guid}/similar")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<ShopListItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<ShopListItemDto>>>> GetSimilar(
        Guid id, [FromQuery] int count = 8)
    {
        var result = await _service.GetSimilarAsync(id, count, HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<ShopListItemDto>>.Ok(
            result, UserMessages.Listings.SimilarLoaded));
    }

    [HttpGet("{id:guid}/related")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<ShopListItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<ShopListItemDto>>>> GetRelated(
        Guid id, [FromQuery] int count = 8)
    {
        var result = await _service.GetRelatedAsync(id, count, HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<ShopListItemDto>>.Ok(
            result, UserMessages.Listings.RelatedLoaded));
    }

    [HttpGet("recently-added")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<ShopListItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<ShopListItemDto>>>> GetRecentlyAdded(
        [FromQuery] int count = 8)
    {
        var result = await _service.GetRecentlyAddedAsync(count, HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<ShopListItemDto>>.Ok(
            result, UserMessages.Listings.RecentlyAddedLoaded));
    }

    [HttpGet("price-statistics")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<RealEstatePriceStatisticsDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<RealEstatePriceStatisticsDto>>> GetPriceStatistics(
        [FromQuery] ShopFilterParams filter)
    {
        var result = await _service.GetPriceStatisticsAsync(filter, HttpContext.RequestAborted);
        return Ok(ApiResponse<RealEstatePriceStatisticsDto>.Ok(
            result, UserMessages.Listings.PriceStatisticsLoaded));
    }

    [HttpGet("search-suggestions")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<RealEstateSuggestionDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<RealEstateSuggestionDto>>>> GetSuggestions(
        [FromQuery] string? term, [FromQuery] int count = 10)
    {
        var result = await _service.GetSuggestionsAsync(term, count, HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<RealEstateSuggestionDto>>.Ok(
            result, UserMessages.Listings.SearchSuggestionsLoaded));
    }

    [HttpGet(RealEstateLookupKeys.ListingTypes)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>), StatusCodes.Status200OK)]
    public Task<ActionResult<ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>>> GetListingTypes() =>
        Lookup(RealEstateLookupKeys.ListingTypes);

    [HttpGet(RealEstateLookupKeys.Projects)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>), StatusCodes.Status200OK)]
    public Task<ActionResult<ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>>> GetProjects() =>
        Lookup(RealEstateLookupKeys.Projects);

    [HttpGet(RealEstateLookupKeys.SuitableActivities)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>), StatusCodes.Status200OK)]
    public Task<ActionResult<ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>>> GetSuitableActivities() =>
        Lookup(RealEstateLookupKeys.SuitableActivities);

    [HttpGet(RealEstateLookupKeys.FloorTypes)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>), StatusCodes.Status200OK)]
    public Task<ActionResult<ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>>> GetFloorTypes() =>
        Lookup(RealEstateLookupKeys.FloorTypes);

    [HttpGet(RealEstateLookupKeys.FacadesCounts)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>), StatusCodes.Status200OK)]
    public Task<ActionResult<ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>>> GetFacadesCounts() =>
        Lookup(RealEstateLookupKeys.FacadesCounts);

    [HttpGet(RealEstateLookupKeys.FacadeDirections)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>), StatusCodes.Status200OK)]
    public Task<ActionResult<ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>>> GetFacadeDirections() =>
        Lookup(RealEstateLookupKeys.FacadeDirections);

    [HttpGet(RealEstateLookupKeys.FinishingTypes)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>), StatusCodes.Status200OK)]
    public Task<ActionResult<ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>>> GetFinishingTypes() =>
        Lookup(RealEstateLookupKeys.FinishingTypes);

    [HttpGet(RealEstateLookupKeys.PropertyAges)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>), StatusCodes.Status200OK)]
    public Task<ActionResult<ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>>> GetPropertyAges() =>
        Lookup(RealEstateLookupKeys.PropertyAges);

    [HttpGet(RealEstateLookupKeys.EntrancesCounts)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>), StatusCodes.Status200OK)]
    public Task<ActionResult<ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>>> GetEntrancesCounts() =>
        Lookup(RealEstateLookupKeys.EntrancesCounts);

    [HttpGet(RealEstateLookupKeys.LegalStatuses)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>), StatusCodes.Status200OK)]
    public Task<ActionResult<ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>>> GetLegalStatuses() =>
        Lookup(RealEstateLookupKeys.LegalStatuses);

    [HttpGet(RealEstateLookupKeys.LicenseTypes)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>), StatusCodes.Status200OK)]
    public Task<ActionResult<ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>>> GetLicenseTypes() =>
        Lookup(RealEstateLookupKeys.LicenseTypes);

    [HttpGet(RealEstateLookupKeys.ReconciliationForms)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>), StatusCodes.Status200OK)]
    public Task<ActionResult<ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>>> GetReconciliationForms() =>
        Lookup(RealEstateLookupKeys.ReconciliationForms);

    [HttpGet(RealEstateLookupKeys.OwnershipDocuments)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>), StatusCodes.Status200OK)]
    public Task<ActionResult<ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>>> GetOwnershipDocuments() =>
        Lookup(RealEstateLookupKeys.OwnershipDocuments);

    [HttpGet(RealEstateLookupKeys.Utilities)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>), StatusCodes.Status200OK)]
    public Task<ActionResult<ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>>> GetUtilities() =>
        Lookup(RealEstateLookupKeys.Utilities);

    [HttpGet(RealEstateLookupKeys.PaymentMethods)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>), StatusCodes.Status200OK)]
    public Task<ActionResult<ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>>> GetPaymentMethods() =>
        Lookup(RealEstateLookupKeys.PaymentMethods);

    [HttpGet(RealEstateLookupKeys.InstallmentProviders)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>), StatusCodes.Status200OK)]
    public Task<ActionResult<ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>>> GetInstallmentProviders() =>
        Lookup(RealEstateLookupKeys.InstallmentProviders);

    [HttpGet(RealEstateLookupKeys.RentTypes)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>), StatusCodes.Status200OK)]
    public Task<ActionResult<ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>>> GetRentTypes() =>
        Lookup(RealEstateLookupKeys.RentTypes);

    [HttpGet(RealEstateLookupKeys.RentInclusions)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>), StatusCodes.Status200OK)]
    public Task<ActionResult<ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>>> GetRentInclusions() =>
        Lookup(RealEstateLookupKeys.RentInclusions);

    [HttpGet(RealEstateLookupKeys.RentSuitableActivities)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>), StatusCodes.Status200OK)]
    public Task<ActionResult<ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>>> GetRentSuitableActivities() =>
        Lookup(RealEstateLookupKeys.RentSuitableActivities);

    [HttpGet(RealEstateLookupKeys.ExchangeTargets)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>), StatusCodes.Status200OK)]
    public Task<ActionResult<ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>>> GetExchangeTargets() =>
        Lookup(RealEstateLookupKeys.ExchangeTargets);

    private async Task<ActionResult<ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>>> Lookup(
        string lookupKey)
    {
        var result = await _service.GetLookupAsync(lookupKey, HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>.Ok(
            result, UserMessages.Lookups.Loaded));
    }
}
