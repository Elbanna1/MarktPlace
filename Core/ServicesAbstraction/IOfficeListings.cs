using Shared.DTOs.Offices;

namespace ServicesAbstraction;

public interface ILawyerOfficeService
    : IDirectoryListingService<CreateLawyerOfficeRequest, UpdateLawyerOfficeRequest, LawyerOfficeDto, LawyerOfficeFilterParams>;

public interface IEngineeringConsultancyOfficeService
    : IDirectoryListingService<CreateEngineeringConsultancyOfficeRequest, UpdateEngineeringConsultancyOfficeRequest, EngineeringConsultancyOfficeDto, EngineeringConsultancyOfficeFilterParams>;

public interface IContractorService
    : IDirectoryListingService<CreateContractorRequest, UpdateContractorRequest, ContractorDto, ContractorFilterParams>;

public interface IIndustrialConsultancyOfficeService
    : IDirectoryListingService<CreateIndustrialConsultancyOfficeRequest, UpdateIndustrialConsultancyOfficeRequest, IndustrialConsultancyOfficeDto, IndustrialConsultancyOfficeFilterParams>;

public interface IAccountantOfficeService
    : IDirectoryListingService<CreateAccountantOfficeRequest, UpdateAccountantOfficeRequest, AccountantOfficeDto, AccountantOfficeFilterParams>;
