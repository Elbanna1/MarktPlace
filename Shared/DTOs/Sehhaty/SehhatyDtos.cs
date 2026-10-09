using Shared.DTOs.Directory;
using Shared.Enums;

namespace Shared.DTOs.Sehhaty;

public class CreateDoctorRequest : CreateDirectoryListingRequest
{
    public DoctorSpecialty? Specialty { get; set; }

    public string? OtherSpecialty { get; set; }
}

public class UpdateDoctorRequest : CreateDoctorRequest, IDirectoryGalleryEdit
{
    public List<Guid> RemoveImageIds { get; set; } = new();
}

public class DoctorDto : DirectoryListingDto
{
    public DoctorSpecialty Specialty { get; set; }

    public string SpecialtyName { get; set; } = default!;

    public string? OtherSpecialty { get; set; }

    public string DisplaySpecialty { get; set; } = default!;

    public override ListingModuleType StatsListingType => ListingModuleType.Doctor;
}

public class DoctorFilterParams : DirectoryFilterParams
{
    public DoctorSpecialty? Specialty { get; set; }
}

public class CreateMedicalLaboratoryRequest : CreateDirectoryListingRequest;

public class UpdateMedicalLaboratoryRequest : CreateMedicalLaboratoryRequest, IDirectoryGalleryEdit
{
    public List<Guid> RemoveImageIds { get; set; } = new();
}

public class MedicalLaboratoryDto : DirectoryListingDto
{
    public override ListingModuleType StatsListingType => ListingModuleType.MedicalLaboratory;
}

public class MedicalLaboratoryFilterParams : DirectoryFilterParams;

public class CreateDentalLaboratoryRequest : CreateDirectoryListingRequest;

public class UpdateDentalLaboratoryRequest : CreateDentalLaboratoryRequest, IDirectoryGalleryEdit
{
    public List<Guid> RemoveImageIds { get; set; } = new();
}

public class DentalLaboratoryDto : DirectoryListingDto
{
    public override ListingModuleType StatsListingType => ListingModuleType.DentalLaboratory;
}

public class DentalLaboratoryFilterParams : DirectoryFilterParams;

public class CreateRadiologyLaboratoryRequest : CreateDirectoryListingRequest;

public class UpdateRadiologyLaboratoryRequest : CreateRadiologyLaboratoryRequest, IDirectoryGalleryEdit
{
    public List<Guid> RemoveImageIds { get; set; } = new();
}

public class RadiologyLaboratoryDto : DirectoryListingDto
{
    public override ListingModuleType StatsListingType => ListingModuleType.RadiologyLaboratory;
}

public class RadiologyLaboratoryFilterParams : DirectoryFilterParams;

public class CreateMedicalCenterRequest : CreateDirectoryListingRequest;

public class UpdateMedicalCenterRequest : CreateMedicalCenterRequest, IDirectoryGalleryEdit
{
    public List<Guid> RemoveImageIds { get; set; } = new();
}

public class MedicalCenterDto : DirectoryListingDto
{
    public override ListingModuleType StatsListingType => ListingModuleType.MedicalCenter;
}

public class MedicalCenterFilterParams : DirectoryFilterParams;

public class CreateHospitalRequest : CreateDirectoryListingRequest;

public class UpdateHospitalRequest : CreateHospitalRequest, IDirectoryGalleryEdit
{
    public List<Guid> RemoveImageIds { get; set; } = new();
}

public class HospitalDto : DirectoryListingDto
{
    public override ListingModuleType StatsListingType => ListingModuleType.Hospital;
}

public class HospitalFilterParams : DirectoryFilterParams;
