using Shared.Enums;

namespace Domain.Entities;

public class Doctor : DirectoryListing<DoctorImage>
{
    public DoctorSpecialty Specialty { get; set; }

    public string? OtherSpecialty { get; set; }
}

public class DoctorImage : DirectoryListingImage
{
    public Doctor Listing { get; set; } = default!;
}

public class MedicalLaboratory : DirectoryListing<MedicalLaboratoryImage>;

public class MedicalLaboratoryImage : DirectoryListingImage
{
    public MedicalLaboratory Listing { get; set; } = default!;
}

public class DentalLaboratory : DirectoryListing<DentalLaboratoryImage>;

public class DentalLaboratoryImage : DirectoryListingImage
{
    public DentalLaboratory Listing { get; set; } = default!;
}

public class RadiologyLaboratory : DirectoryListing<RadiologyLaboratoryImage>;

public class RadiologyLaboratoryImage : DirectoryListingImage
{
    public RadiologyLaboratory Listing { get; set; } = default!;
}

public class MedicalCenter : DirectoryListing<MedicalCenterImage>;

public class MedicalCenterImage : DirectoryListingImage
{
    public MedicalCenter Listing { get; set; } = default!;
}

public class Hospital : DirectoryListing<HospitalImage>;

public class HospitalImage : DirectoryListingImage
{
    public Hospital Listing { get; set; } = default!;
}
