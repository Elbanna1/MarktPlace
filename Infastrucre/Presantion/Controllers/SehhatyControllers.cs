using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Extensions;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Sehhaty;
using Shared.Responses;

namespace Presentation.Controllers;

[ApiController]
[Authorize]
[Produces("application/json")]
public abstract class SehhatyControllerBase<TCreate, TUpdate, TDto, TFilter> : ControllerBase
    where TCreate : CreateSehhatyListingRequest
    where TUpdate : TCreate, ISehhatyGalleryEdit
    where TDto : SehhatyListingDto
    where TFilter : SehhatyFilterParams
{
    private readonly ISehhatyListingService<TCreate, TUpdate, TDto, TFilter> _service;

    protected SehhatyControllerBase(ISehhatyListingService<TCreate, TUpdate, TDto, TFilter> service)
    {
        _service = service;
    }

    private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    private bool IsAdmin => User.IsInRole(AppRoles.Admin);

    [HttpPost]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(ImageConstants.MaxRequestBodySizeBytes)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<TDto>>> Create([FromForm] TCreate request)
    {
        var images = await Request.FormFilesOrEmpty().ToUploadModelsAsync(HttpContext.RequestAborted);
        var result = await _service.CreateAsync(CurrentUserId, request, images, HttpContext.RequestAborted);

        return StatusCode(StatusCodes.Status201Created,
            ApiResponse<TDto>.Ok(result, UserMessages.Listings.Created));
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<TDto>>>> GetList([FromQuery] TFilter filter)
    {
        var result = await _service.GetListAsync(filter, HttpContext.RequestAborted);

        return Ok(ApiResponse<PaginatedResult<TDto>>.Ok(result, UserMessages.Listings.Loaded));
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<TDto>>> GetById(Guid id)
    {
        var result = await _service.GetByIdAsync(id, HttpContext.RequestAborted);

        return Ok(ApiResponse<TDto>.Ok(result, UserMessages.Listings.DetailsLoaded));
    }

    [HttpPut("{id:guid}")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(ImageConstants.MaxRequestBodySizeBytes)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<TDto>>> Update(Guid id, [FromForm] TUpdate request)
    {
        var images = await Request.FormFilesOrEmpty().ToUploadModelsAsync(HttpContext.RequestAborted);
        var result = await _service.UpdateAsync(
            CurrentUserId, IsAdmin, id, request, images, HttpContext.RequestAborted);

        return Ok(ApiResponse<TDto>.Ok(result, UserMessages.Listings.Updated));
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse>> Delete(Guid id)
    {
        await _service.DeleteAsync(CurrentUserId, id, IsAdmin, HttpContext.RequestAborted);

        return Ok(ApiResponse.Ok(UserMessages.Listings.Deleted));
    }
}

[Route(SehhatyRoutes.Doctors)]
[Tags("Sehhaty - Doctors")]
public class DoctorsController(IDoctorService service)
    : SehhatyControllerBase<CreateDoctorRequest, UpdateDoctorRequest, DoctorDto, DoctorFilterParams>(service)
{
    [HttpGet("specialties")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<ApiResponse<IReadOnlyList<object>>> GetSpecialties()
    {
        var specialties = SehhatyCatalog.Specialties
            .Select(entry => (object)new { id = (int)entry.Id, name = entry.Name, nameEn = entry.NameEn })
            .ToList();

        return Ok(ApiResponse<IReadOnlyList<object>>.Ok(specialties, UserMessages.Lookups.Loaded));
    }
}

[Route(SehhatyRoutes.MedicalLaboratories)]
[Tags("Sehhaty - Medical Laboratories")]
public class MedicalLaboratoriesController(IMedicalLaboratoryService service)
    : SehhatyControllerBase<CreateMedicalLaboratoryRequest, UpdateMedicalLaboratoryRequest,
        MedicalLaboratoryDto, MedicalLaboratoryFilterParams>(service);

[Route(SehhatyRoutes.DentalLaboratories)]
[Tags("Sehhaty - Dental Laboratories")]
public class DentalLaboratoriesController(IDentalLaboratoryService service)
    : SehhatyControllerBase<CreateDentalLaboratoryRequest, UpdateDentalLaboratoryRequest,
        DentalLaboratoryDto, DentalLaboratoryFilterParams>(service);

[Route(SehhatyRoutes.RadiologyLaboratories)]
[Tags("Sehhaty - Radiology Laboratories")]
public class RadiologyLaboratoriesController(IRadiologyLaboratoryService service)
    : SehhatyControllerBase<CreateRadiologyLaboratoryRequest, UpdateRadiologyLaboratoryRequest,
        RadiologyLaboratoryDto, RadiologyLaboratoryFilterParams>(service);

[Route(SehhatyRoutes.MedicalCenters)]
[Tags("Sehhaty - Medical Centers")]
public class MedicalCentersController(IMedicalCenterService service)
    : SehhatyControllerBase<CreateMedicalCenterRequest, UpdateMedicalCenterRequest,
        MedicalCenterDto, MedicalCenterFilterParams>(service);

[Route(SehhatyRoutes.Hospitals)]
[Tags("Sehhaty - Hospitals")]
public class HospitalsController(IHospitalService service)
    : SehhatyControllerBase<CreateHospitalRequest, UpdateHospitalRequest, HospitalDto, HospitalFilterParams>(service);
