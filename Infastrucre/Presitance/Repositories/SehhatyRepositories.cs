using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Shared.Constants;
using Shared.DTOs.Sehhaty;
using Shared.Enums;

namespace Persistence.Repositories;

public sealed class DoctorRepository : DirectoryRepository<Doctor, DoctorImage, DoctorFilterParams>
{
    public DoctorRepository(AppDbContext context) : base(context, SehhatyCatalog.Doctors) { }

    protected override IQueryable<Doctor> Filter(IQueryable<Doctor> query, DoctorFilterParams filter) =>
        filter.Specialty is { } specialty ? query.Where(x => x.Specialty == specialty) : query;

    protected override IQueryable<Doctor> ApplySearch(IQueryable<Doctor> query, string pattern) =>
        query.Where(x =>
            EF.Functions.Like(x.Name, pattern, EscapeCharacter) ||
            EF.Functions.Like(x.Title, pattern, EscapeCharacter) ||
            EF.Functions.Like(x.Description, pattern, EscapeCharacter) ||
            EF.Functions.Like(x.Address, pattern, EscapeCharacter) ||
            (x.OtherSpecialty != null && EF.Functions.Like(x.OtherSpecialty, pattern, EscapeCharacter)));
}

public sealed class MedicalLaboratoryRepository
    : DirectoryRepository<MedicalLaboratory, MedicalLaboratoryImage, MedicalLaboratoryFilterParams>
{
    public MedicalLaboratoryRepository(AppDbContext context) : base(context, SehhatyCatalog.MedicalLaboratories) { }
}

public sealed class DentalLaboratoryRepository
    : DirectoryRepository<DentalLaboratory, DentalLaboratoryImage, DentalLaboratoryFilterParams>
{
    public DentalLaboratoryRepository(AppDbContext context) : base(context, SehhatyCatalog.DentalLaboratories) { }
}

public sealed class RadiologyLaboratoryRepository
    : DirectoryRepository<RadiologyLaboratory, RadiologyLaboratoryImage, RadiologyLaboratoryFilterParams>
{
    public RadiologyLaboratoryRepository(AppDbContext context) : base(context, SehhatyCatalog.RadiologyLaboratories) { }
}

public sealed class MedicalCenterRepository
    : DirectoryRepository<MedicalCenter, MedicalCenterImage, MedicalCenterFilterParams>
{
    public MedicalCenterRepository(AppDbContext context) : base(context, SehhatyCatalog.MedicalCenters) { }
}

public sealed class HospitalRepository : DirectoryRepository<Hospital, HospitalImage, HospitalFilterParams>
{
    public HospitalRepository(AppDbContext context) : base(context, SehhatyCatalog.Hospitals) { }
}
