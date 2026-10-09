using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Sehhaty;
using Shared.Responses;

namespace Presentation.Controllers;

[Route(SehhatyRoutes.Doctors)]
[Tags("Sehhaty - Doctors")]
public class DoctorsController(IDoctorService service)
    : DirectoryControllerBase<CreateDoctorRequest, UpdateDoctorRequest, DoctorDto, DoctorFilterParams>(service)
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
    : DirectoryControllerBase<CreateMedicalLaboratoryRequest, UpdateMedicalLaboratoryRequest,
        MedicalLaboratoryDto, MedicalLaboratoryFilterParams>(service);

[Route(SehhatyRoutes.DentalLaboratories)]
[Tags("Sehhaty - Dental Laboratories")]
public class DentalLaboratoriesController(IDentalLaboratoryService service)
    : DirectoryControllerBase<CreateDentalLaboratoryRequest, UpdateDentalLaboratoryRequest,
        DentalLaboratoryDto, DentalLaboratoryFilterParams>(service);

[Route(SehhatyRoutes.RadiologyLaboratories)]
[Tags("Sehhaty - Radiology Laboratories")]
public class RadiologyLaboratoriesController(IRadiologyLaboratoryService service)
    : DirectoryControllerBase<CreateRadiologyLaboratoryRequest, UpdateRadiologyLaboratoryRequest,
        RadiologyLaboratoryDto, RadiologyLaboratoryFilterParams>(service);

[Route(SehhatyRoutes.MedicalCenters)]
[Tags("Sehhaty - Medical Centers")]
public class MedicalCentersController(IMedicalCenterService service)
    : DirectoryControllerBase<CreateMedicalCenterRequest, UpdateMedicalCenterRequest,
        MedicalCenterDto, MedicalCenterFilterParams>(service);

[Route(SehhatyRoutes.Hospitals)]
[Tags("Sehhaty - Hospitals")]
public class HospitalsController(IHospitalService service)
    : DirectoryControllerBase<CreateHospitalRequest, UpdateHospitalRequest, HospitalDto, HospitalFilterParams>(service);
