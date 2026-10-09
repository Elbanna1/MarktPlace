using Shared.DTOs.Sehhaty;

namespace ServicesAbstraction;

public interface IDoctorService
    : IDirectoryListingService<CreateDoctorRequest, UpdateDoctorRequest, DoctorDto, DoctorFilterParams>;

public interface IMedicalLaboratoryService
    : IDirectoryListingService<CreateMedicalLaboratoryRequest, UpdateMedicalLaboratoryRequest,
        MedicalLaboratoryDto, MedicalLaboratoryFilterParams>;

public interface IDentalLaboratoryService
    : IDirectoryListingService<CreateDentalLaboratoryRequest, UpdateDentalLaboratoryRequest,
        DentalLaboratoryDto, DentalLaboratoryFilterParams>;

public interface IRadiologyLaboratoryService
    : IDirectoryListingService<CreateRadiologyLaboratoryRequest, UpdateRadiologyLaboratoryRequest,
        RadiologyLaboratoryDto, RadiologyLaboratoryFilterParams>;

public interface IMedicalCenterService
    : IDirectoryListingService<CreateMedicalCenterRequest, UpdateMedicalCenterRequest,
        MedicalCenterDto, MedicalCenterFilterParams>;

public interface IHospitalService
    : IDirectoryListingService<CreateHospitalRequest, UpdateHospitalRequest, HospitalDto, HospitalFilterParams>;
