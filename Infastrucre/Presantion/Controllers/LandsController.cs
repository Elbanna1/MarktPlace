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
[Route(RealEstateRoutes.Lands)]
[Authorize]
[Produces("application/json")]
public class LandsController : ControllerBase
{
    private readonly ILandService _service;

    public LandsController(ILandService service)
    {
        _service = service;
    }

    private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    private bool IsAdmin => User.IsInRole(AppRoles.Admin);

    [HttpPost]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(FileUploadConstants.MaxRealEstateRequestBodySizeBytes)]
    [ProducesResponseType(typeof(ApiResponse<LandDetailsDto>), StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<LandDetailsDto>>> Create([FromForm] CreateLandRequest request)
    {
        var cancellationToken = HttpContext.RequestAborted;

        var images = await request.Images.ToUploadModelsAsync(cancellationToken);
        var video = await request.Video.ToUploadModelAsync(cancellationToken);

        var result = await _service.CreateAsync(CurrentUserId, request, images, video, cancellationToken);

        return StatusCode(StatusCodes.Status201Created,
            ApiResponse<LandDetailsDto>.Ok(result, UserMessages.Listings.Created));
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<LandListItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<LandListItemDto>>>> GetList(
        [FromQuery] LandFilterParams filter)
    {
        var result = await _service.GetListAsync(filter, HttpContext.RequestAborted);
        return Ok(ApiResponse<PaginatedResult<LandListItemDto>>.Ok(
            result, UserMessages.Listings.Loaded));
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<LandDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<LandDetailsDto>>> GetById(Guid id)
    {
        var result = await _service.GetByIdAsync(id, countView: true, HttpContext.RequestAborted);
        return Ok(ApiResponse<LandDetailsDto>.Ok(result, UserMessages.Listings.DetailsLoaded));
    }

    [HttpPut("{id:guid}")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(FileUploadConstants.MaxRealEstateRequestBodySizeBytes)]
    [ProducesResponseType(typeof(ApiResponse<LandDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<LandDetailsDto>>> Update(
        Guid id, [FromForm] UpdateLandRequest request)
    {
        var cancellationToken = HttpContext.RequestAborted;

        var newImages = await request.Images.ToUploadModelsAsync(cancellationToken);
        var video = await request.Video.ToUploadModelAsync(cancellationToken);

        var result = await _service.UpdateAsync(
            CurrentUserId, IsAdmin, id, request, newImages, video, cancellationToken);

        return Ok(ApiResponse<LandDetailsDto>.Ok(result, UserMessages.Listings.Updated));
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
    [ProducesResponseType(typeof(ApiResponse<LandDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<LandDetailsDto>>> ReorderImages(
        Guid id, [FromBody] ReorderRealEstateImagesRequest request)
    {
        var result = await _service.ReorderImagesAsync(
            CurrentUserId, IsAdmin, id, request, HttpContext.RequestAborted);

        return Ok(ApiResponse<LandDetailsDto>.Ok(result, UserMessages.Listings.ImagesReordered));
    }

    [HttpPut("{id:guid}/promotion")]
    [ProducesResponseType(typeof(ApiResponse<LandDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<LandDetailsDto>>> SetPromotion(
        Guid id, [FromBody] PromoteRealEstateRequest request)
    {
        if (!IsAdmin)
            throw new ForbiddenException("الترويج للإعلان متاح للإدارة بس.");

        var result = await _service.SetPromotionAsync(id, request, HttpContext.RequestAborted);
        return Ok(ApiResponse<LandDetailsDto>.Ok(result, UserMessages.Listings.PromotionUpdated));
    }

    [HttpGet("{id:guid}/similar")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<LandListItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<LandListItemDto>>>> GetSimilar(
        Guid id, [FromQuery] int count = 8)
    {
        var result = await _service.GetSimilarAsync(id, count, HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<LandListItemDto>>.Ok(
            result, UserMessages.Listings.SimilarLoaded));
    }

    [HttpGet("{id:guid}/related")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<LandListItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<LandListItemDto>>>> GetRelated(
        Guid id, [FromQuery] int count = 8)
    {
        var result = await _service.GetRelatedAsync(id, count, HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<LandListItemDto>>.Ok(
            result, UserMessages.Listings.RelatedLoaded));
    }

    [HttpGet("recently-added")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<LandListItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<LandListItemDto>>>> GetRecentlyAdded(
        [FromQuery] int count = 8)
    {
        var result = await _service.GetRecentlyAddedAsync(count, HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<LandListItemDto>>.Ok(
            result, UserMessages.Listings.RecentlyAddedLoaded));
    }

    [HttpGet("price-statistics")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<RealEstatePriceStatisticsDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<RealEstatePriceStatisticsDto>>> GetPriceStatistics(
        [FromQuery] LandFilterParams filter)
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

    [HttpGet(RealEstateLookupKeys.LandTypes)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>), StatusCodes.Status200OK)]
    public Task<ActionResult<ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>>> GetLandTypes() =>
        Lookup(RealEstateLookupKeys.LandTypes);

    [HttpGet(RealEstateLookupKeys.AreaUnits)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>), StatusCodes.Status200OK)]
    public Task<ActionResult<ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>>> GetAreaUnits() =>
        Lookup(RealEstateLookupKeys.AreaUnits);

    [HttpGet(RealEstateLookupKeys.FacadesCounts)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>), StatusCodes.Status200OK)]
    public Task<ActionResult<ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>>> GetFacadesCounts() =>
        Lookup(RealEstateLookupKeys.FacadesCounts);

    [HttpGet(RealEstateLookupKeys.Directions)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>), StatusCodes.Status200OK)]
    public Task<ActionResult<ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>>> GetDirections() =>
        Lookup(RealEstateLookupKeys.Directions);

    [HttpGet(RealEstateLookupKeys.RoadTypes)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>), StatusCodes.Status200OK)]
    public Task<ActionResult<ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>>> GetRoadTypes() =>
        Lookup(RealEstateLookupKeys.RoadTypes);

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

    [HttpGet(RealEstateLookupKeys.Utilities)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>), StatusCodes.Status200OK)]
    public Task<ActionResult<ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>>> GetUtilities() =>
        Lookup(RealEstateLookupKeys.Utilities);

    [HttpGet(RealEstateLookupKeys.RentTypes)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>), StatusCodes.Status200OK)]
    public Task<ActionResult<ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>>> GetRentTypes() =>
        Lookup(RealEstateLookupKeys.RentTypes);

    [HttpGet(RealEstateLookupKeys.MinimumRentPeriods)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>), StatusCodes.Status200OK)]
    public Task<ActionResult<ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>>> GetMinimumRentPeriods() =>
        Lookup(RealEstateLookupKeys.MinimumRentPeriods);

    [HttpGet(RealEstateLookupKeys.RentInclusions)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>), StatusCodes.Status200OK)]
    public Task<ActionResult<ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>>> GetRentInclusions() =>
        Lookup(RealEstateLookupKeys.RentInclusions);

    [HttpGet(RealEstateLookupKeys.ContractDurations)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>), StatusCodes.Status200OK)]
    public Task<ActionResult<ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>>> GetContractDurations() =>
        Lookup(RealEstateLookupKeys.ContractDurations);

    [HttpGet(RealEstateLookupKeys.ExchangeTargets)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>), StatusCodes.Status200OK)]
    public Task<ActionResult<ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>>> GetExchangeTargets() =>
        Lookup(RealEstateLookupKeys.ExchangeTargets);

    [HttpGet(RealEstateLookupKeys.HarvestSeasons)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>), StatusCodes.Status200OK)]
    public Task<ActionResult<ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>>> GetHarvestSeasons() =>
        Lookup(RealEstateLookupKeys.HarvestSeasons);

    [HttpGet(RealEstateLookupKeys.SoilTypes)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>), StatusCodes.Status200OK)]
    public Task<ActionResult<ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>>> GetSoilTypes() =>
        Lookup(RealEstateLookupKeys.SoilTypes);

    [HttpGet(RealEstateLookupKeys.IrrigationSources)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>), StatusCodes.Status200OK)]
    public Task<ActionResult<ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>>> GetIrrigationSources() =>
        Lookup(RealEstateLookupKeys.IrrigationSources);

    [HttpGet(RealEstateLookupKeys.QualityCertificates)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>), StatusCodes.Status200OK)]
    public Task<ActionResult<ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>>> GetQualityCertificates() =>
        Lookup(RealEstateLookupKeys.QualityCertificates);

    [HttpGet(RealEstateLookupKeys.ExistingBuildingTypes)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>), StatusCodes.Status200OK)]
    public Task<ActionResult<ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>>> GetExistingBuildingTypes() =>
        Lookup(RealEstateLookupKeys.ExistingBuildingTypes);

    [HttpGet(RealEstateLookupKeys.BuildingCompletionRatios)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>), StatusCodes.Status200OK)]
    public Task<ActionResult<ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>>> GetBuildingCompletionRatios() =>
        Lookup(RealEstateLookupKeys.BuildingCompletionRatios);

    private async Task<ActionResult<ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>>> Lookup(
        string lookupKey)
    {
        var result = await _service.GetLookupAsync(lookupKey, HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<RealEstateLookupItemDto>>.Ok(
            result, UserMessages.Lookups.Loaded));
    }
}
