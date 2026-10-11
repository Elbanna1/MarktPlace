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
[Route(RealEstateRoutes.Buildings)]
[Authorize]
[Produces("application/json")]
public class BuildingsController : ControllerBase
{
    private readonly IBuildingService _service;

    public BuildingsController(IBuildingService service)
    {
        _service = service;
    }

    private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    private bool IsAdmin => User.IsInRole(AppRoles.Admin);

    [HttpPost]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(FileUploadConstants.MaxRealEstateRequestBodySizeBytes)]
    [ProducesResponseType(typeof(ApiResponse<BuildingDetailsDto>), StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<BuildingDetailsDto>>> Create(
        [FromForm] CreateBuildingRequest request)
    {
        if (UnboundMultiSelect() is { } invalid)
            return invalid;

        var cancellationToken = HttpContext.RequestAborted;

        var images = await request.Images.ToUploadModelsAsync(cancellationToken);
        var video = await request.Video.ToUploadModelAsync(cancellationToken);

        var result = await _service.CreateAsync(CurrentUserId, request, images, video, cancellationToken);

        return StatusCode(StatusCodes.Status201Created,
            ApiResponse<BuildingDetailsDto>.Ok(result, UserMessages.Listings.Created));
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<BuildingListItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<BuildingListItemDto>>>> GetList(
        [FromQuery] BuildingFilterParams filter)
    {
        var result = await _service.GetListAsync(filter, HttpContext.RequestAborted);
        return Ok(ApiResponse<PaginatedResult<BuildingListItemDto>>.Ok(
            result, UserMessages.Listings.Loaded));
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<BuildingDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<BuildingDetailsDto>>> GetById(Guid id)
    {
        var result = await _service.GetByIdAsync(id, countView: true, HttpContext.RequestAborted);
        return Ok(ApiResponse<BuildingDetailsDto>.Ok(result, UserMessages.Listings.DetailsLoaded));
    }

    [HttpPut("{id:guid}")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(FileUploadConstants.MaxRealEstateRequestBodySizeBytes)]
    [ProducesResponseType(typeof(ApiResponse<BuildingDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<BuildingDetailsDto>>> Update(
        Guid id, [FromForm] UpdateBuildingRequest request)
    {
        if (UnboundMultiSelect() is { } invalid)
            return invalid;

        var cancellationToken = HttpContext.RequestAborted;

        var newImages = await request.Images.ToUploadModelsAsync(cancellationToken);
        var video = await request.Video.ToUploadModelAsync(cancellationToken);

        var result = await _service.UpdateAsync(
            CurrentUserId, IsAdmin, id, request, newImages, video, cancellationToken);

        return Ok(ApiResponse<BuildingDetailsDto>.Ok(result, UserMessages.Listings.Updated));
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
    [ProducesResponseType(typeof(ApiResponse<BuildingDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<BuildingDetailsDto>>> ReorderImages(
        Guid id, [FromBody] ReorderRealEstateImagesRequest request)
    {
        var result = await _service.ReorderImagesAsync(
            CurrentUserId, IsAdmin, id, request, HttpContext.RequestAborted);

        return Ok(ApiResponse<BuildingDetailsDto>.Ok(result, UserMessages.Listings.ImagesReordered));
    }

    [HttpPut("{id:guid}/promotion")]
    [ProducesResponseType(typeof(ApiResponse<BuildingDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<BuildingDetailsDto>>> SetPromotion(
        Guid id, [FromBody] PromoteRealEstateRequest request)
    {
        if (!IsAdmin)
            throw new ForbiddenException("الترويج للإعلان متاح للإدارة بس.");

        var result = await _service.SetPromotionAsync(id, request, HttpContext.RequestAborted);
        return Ok(ApiResponse<BuildingDetailsDto>.Ok(result, UserMessages.Listings.PromotionUpdated));
    }

    [HttpGet("{id:guid}/similar")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<BuildingListItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<BuildingListItemDto>>>> GetSimilar(
        Guid id, [FromQuery] int count = 8)
    {
        var result = await _service.GetSimilarAsync(id, count, HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<BuildingListItemDto>>.Ok(
            result, UserMessages.Listings.SimilarLoaded));
    }

    [HttpGet("{id:guid}/related")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<BuildingListItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<BuildingListItemDto>>>> GetRelated(
        Guid id, [FromQuery] int count = 8)
    {
        var result = await _service.GetRelatedAsync(id, count, HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<BuildingListItemDto>>.Ok(
            result, UserMessages.Listings.RelatedLoaded));
    }

    [HttpGet("recently-added")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<BuildingListItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<BuildingListItemDto>>>> GetRecentlyAdded(
        [FromQuery] int count = 8)
    {
        var result = await _service.GetRecentlyAddedAsync(count, HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<BuildingListItemDto>>.Ok(
            result, UserMessages.Listings.RecentlyAddedLoaded));
    }

    [HttpGet("price-statistics")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<RealEstatePriceStatisticsDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<RealEstatePriceStatisticsDto>>> GetPriceStatistics(
        [FromQuery] BuildingFilterParams filter)
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

    [HttpGet(RealEstateLookupKeys.BuildingTypes)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>), StatusCodes.Status200OK)]
    public Task<ActionResult<ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>>> GetBuildingTypes() =>
        Lookup(RealEstateLookupKeys.BuildingTypes);

    [HttpGet(RealEstateLookupKeys.ReceptionPieces)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>), StatusCodes.Status200OK)]
    public Task<ActionResult<ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>>> GetReceptionPieces() =>
        Lookup(RealEstateLookupKeys.ReceptionPieces);

    [HttpGet(RealEstateLookupKeys.FurnishedStatuses)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>), StatusCodes.Status200OK)]
    public Task<ActionResult<ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>>> GetFurnishedStatuses() =>
        Lookup(RealEstateLookupKeys.FurnishedStatuses);

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

    [HttpGet(RealEstateLookupKeys.Directions)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>), StatusCodes.Status200OK)]
    public Task<ActionResult<ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>>> GetDirections() =>
        Lookup(RealEstateLookupKeys.Directions);

    [HttpGet(RealEstateLookupKeys.ViewTypes)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>), StatusCodes.Status200OK)]
    public Task<ActionResult<ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>>> GetViewTypes() =>
        Lookup(RealEstateLookupKeys.ViewTypes);

    [HttpGet(RealEstateLookupKeys.LegalStatuses)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>), StatusCodes.Status200OK)]
    public Task<ActionResult<ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>>> GetLegalStatuses() =>
        Lookup(RealEstateLookupKeys.LegalStatuses);

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

    [HttpGet(RealEstateLookupKeys.Features)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>), StatusCodes.Status200OK)]
    public Task<ActionResult<ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>>> GetFeatures() =>
        Lookup(RealEstateLookupKeys.Features);

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

    [HttpGet(RealEstateLookupKeys.ExchangeTargets)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>), StatusCodes.Status200OK)]
    public Task<ActionResult<ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>>> GetExchangeTargets() =>
        Lookup(RealEstateLookupKeys.ExchangeTargets);

    private ObjectResult? UnboundMultiSelect()
    {
        var errors = new List<string>();

        if (HasBindingError(nameof(CreateBuildingRequest.Features)))
            errors.Add("من فضلك اختار مرافق ومميزات صحيحة.");

        if (HasBindingError(nameof(CreateBuildingRequest.RentInclusions)))
            errors.Add("من فضلك اختار بنود إيجار صحيحة.");

        return errors.Count == 0
            ? null
            : new ObjectResult(ApiResponse.Fail("فيه بيانات ناقصة أو مش صحيحة.", errors))
            {
                StatusCode = StatusCodes.Status422UnprocessableEntity
            };
    }

    private bool HasBindingError(string field) =>
        ModelState.Any(entry =>
            (entry.Key == field || entry.Key.StartsWith(field + "[", StringComparison.Ordinal)) &&
            entry.Value.Errors.Count > 0);

    private async Task<ActionResult<ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>>> Lookup(
        string lookupKey)
    {
        var result = await _service.GetLookupAsync(lookupKey, HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>.Ok(
            result, UserMessages.Lookups.Loaded));
    }
}
