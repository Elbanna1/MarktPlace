using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Offices;

namespace Presentation.Controllers;

[Route(OfficesRoutes.LawyerOffices)]
[Tags("Offices - Lawyer Offices")]
public class LawyerOfficesController(ILawyerOfficeService service)
    : DirectoryControllerBase<CreateLawyerOfficeRequest, UpdateLawyerOfficeRequest, LawyerOfficeDto, LawyerOfficeFilterParams>(service);

[Route(OfficesRoutes.EngineeringConsultancyOffices)]
[Tags("Offices - Engineering Consultancy Offices")]
public class EngineeringConsultancyOfficesController(IEngineeringConsultancyOfficeService service)
    : DirectoryControllerBase<CreateEngineeringConsultancyOfficeRequest, UpdateEngineeringConsultancyOfficeRequest, EngineeringConsultancyOfficeDto, EngineeringConsultancyOfficeFilterParams>(service);

[Route(OfficesRoutes.Contractors)]
[Tags("Offices - Contractors")]
public class ContractorsController(IContractorService service)
    : DirectoryControllerBase<CreateContractorRequest, UpdateContractorRequest, ContractorDto, ContractorFilterParams>(service);

[Route(OfficesRoutes.IndustrialConsultancyOffices)]
[Tags("Offices - Industrial Consultancy Offices")]
public class IndustrialConsultancyOfficesController(IIndustrialConsultancyOfficeService service)
    : DirectoryControllerBase<CreateIndustrialConsultancyOfficeRequest, UpdateIndustrialConsultancyOfficeRequest, IndustrialConsultancyOfficeDto, IndustrialConsultancyOfficeFilterParams>(service);

[Route(OfficesRoutes.AccountantOffices)]
[Tags("Offices - Accountant Offices")]
public class AccountantOfficesController(IAccountantOfficeService service)
    : DirectoryControllerBase<CreateAccountantOfficeRequest, UpdateAccountantOfficeRequest, AccountantOfficeDto, AccountantOfficeFilterParams>(service);
