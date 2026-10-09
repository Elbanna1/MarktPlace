using Domain.Entities;
using Persistence.Data;
using Shared.Enums;

namespace Persistence.Listings;

public sealed class DoctorListingSource(AppDbContext context)
    : DirectoryListingSource<Doctor, DoctorImage>(context)
{
    public override ListingModuleType Type => ListingModuleType.Doctor;
}

public sealed class MedicalLaboratoryListingSource(AppDbContext context)
    : DirectoryListingSource<MedicalLaboratory, MedicalLaboratoryImage>(context)
{
    public override ListingModuleType Type => ListingModuleType.MedicalLaboratory;
}

public sealed class DentalLaboratoryListingSource(AppDbContext context)
    : DirectoryListingSource<DentalLaboratory, DentalLaboratoryImage>(context)
{
    public override ListingModuleType Type => ListingModuleType.DentalLaboratory;
}

public sealed class RadiologyLaboratoryListingSource(AppDbContext context)
    : DirectoryListingSource<RadiologyLaboratory, RadiologyLaboratoryImage>(context)
{
    public override ListingModuleType Type => ListingModuleType.RadiologyLaboratory;
}

public sealed class MedicalCenterListingSource(AppDbContext context)
    : DirectoryListingSource<MedicalCenter, MedicalCenterImage>(context)
{
    public override ListingModuleType Type => ListingModuleType.MedicalCenter;
}

public sealed class HospitalListingSource(AppDbContext context)
    : DirectoryListingSource<Hospital, HospitalImage>(context)
{
    public override ListingModuleType Type => ListingModuleType.Hospital;
}
