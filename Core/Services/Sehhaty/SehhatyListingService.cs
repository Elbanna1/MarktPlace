using AutoMapper;
using Domain.Entities;
using Services.Directory;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Sehhaty;
using Shared.Enums;

namespace Services.Sehhaty;

public sealed class DoctorService
    : DirectoryListingService<Doctor, DoctorImage, CreateDoctorRequest, UpdateDoctorRequest, DoctorDto, DoctorFilterParams>,
        IDoctorService
{
    public DoctorService(
        IDirectoryRepository<Doctor, DoctorImage, DoctorFilterParams> repository,
        IFileService fileService, IMapper mapper, INotificationService notifications)
        : base(repository, fileService, mapper, notifications)
    {
    }

    protected override DirectoryModule Module => SehhatyCatalog.Doctors;

    protected override void ApplyModuleFields(Doctor entity, CreateDoctorRequest request)
    {
        entity.Specialty = request.Specialty!.Value;

        entity.OtherSpecialty = entity.Specialty == DoctorSpecialty.Other
            ? Trimmed(request.OtherSpecialty)
            : null;
    }
}

public sealed class MedicalLaboratoryService
    : DirectoryListingService<MedicalLaboratory, MedicalLaboratoryImage, CreateMedicalLaboratoryRequest,
            UpdateMedicalLaboratoryRequest, MedicalLaboratoryDto, MedicalLaboratoryFilterParams>,
        IMedicalLaboratoryService
{
    public MedicalLaboratoryService(
        IDirectoryRepository<MedicalLaboratory, MedicalLaboratoryImage, MedicalLaboratoryFilterParams> repository,
        IFileService fileService, IMapper mapper, INotificationService notifications)
        : base(repository, fileService, mapper, notifications)
    {
    }

    protected override DirectoryModule Module => SehhatyCatalog.MedicalLaboratories;
}

public sealed class DentalLaboratoryService
    : DirectoryListingService<DentalLaboratory, DentalLaboratoryImage, CreateDentalLaboratoryRequest,
            UpdateDentalLaboratoryRequest, DentalLaboratoryDto, DentalLaboratoryFilterParams>,
        IDentalLaboratoryService
{
    public DentalLaboratoryService(
        IDirectoryRepository<DentalLaboratory, DentalLaboratoryImage, DentalLaboratoryFilterParams> repository,
        IFileService fileService, IMapper mapper, INotificationService notifications)
        : base(repository, fileService, mapper, notifications)
    {
    }

    protected override DirectoryModule Module => SehhatyCatalog.DentalLaboratories;
}

public sealed class RadiologyLaboratoryService
    : DirectoryListingService<RadiologyLaboratory, RadiologyLaboratoryImage, CreateRadiologyLaboratoryRequest,
            UpdateRadiologyLaboratoryRequest, RadiologyLaboratoryDto, RadiologyLaboratoryFilterParams>,
        IRadiologyLaboratoryService
{
    public RadiologyLaboratoryService(
        IDirectoryRepository<RadiologyLaboratory, RadiologyLaboratoryImage, RadiologyLaboratoryFilterParams> repository,
        IFileService fileService, IMapper mapper, INotificationService notifications)
        : base(repository, fileService, mapper, notifications)
    {
    }

    protected override DirectoryModule Module => SehhatyCatalog.RadiologyLaboratories;
}

public sealed class MedicalCenterService
    : DirectoryListingService<MedicalCenter, MedicalCenterImage, CreateMedicalCenterRequest,
            UpdateMedicalCenterRequest, MedicalCenterDto, MedicalCenterFilterParams>,
        IMedicalCenterService
{
    public MedicalCenterService(
        IDirectoryRepository<MedicalCenter, MedicalCenterImage, MedicalCenterFilterParams> repository,
        IFileService fileService, IMapper mapper, INotificationService notifications)
        : base(repository, fileService, mapper, notifications)
    {
    }

    protected override DirectoryModule Module => SehhatyCatalog.MedicalCenters;
}

public sealed class HospitalService
    : DirectoryListingService<Hospital, HospitalImage, CreateHospitalRequest,
            UpdateHospitalRequest, HospitalDto, HospitalFilterParams>,
        IHospitalService
{
    public HospitalService(
        IDirectoryRepository<Hospital, HospitalImage, HospitalFilterParams> repository,
        IFileService fileService, IMapper mapper, INotificationService notifications)
        : base(repository, fileService, mapper, notifications)
    {
    }

    protected override DirectoryModule Module => SehhatyCatalog.Hospitals;
}
