using AutoMapper;
using Domain.Entities;
using Shared.Constants;
using Shared.DTOs.Sehhaty;

namespace Services.Mapping;

public class SehhatyMappingProfile : Profile
{
    public SehhatyMappingProfile()
    {
        this.MapDirectoryListing<Doctor, DoctorImage, DoctorDto>()
            .ForMember(d => d.SpecialtyName,
                o => o.MapFrom(s => SehhatyCatalog.GetSpecialtyName(s.Specialty)))
            .ForMember(d => d.DisplaySpecialty,
                o => o.MapFrom(s => SehhatyCatalog.DisplaySpecialty(s.Specialty, s.OtherSpecialty)));

        this.MapDirectoryListing<MedicalLaboratory, MedicalLaboratoryImage, MedicalLaboratoryDto>();
        this.MapDirectoryListing<DentalLaboratory, DentalLaboratoryImage, DentalLaboratoryDto>();
        this.MapDirectoryListing<RadiologyLaboratory, RadiologyLaboratoryImage, RadiologyLaboratoryDto>();
        this.MapDirectoryListing<MedicalCenter, MedicalCenterImage, MedicalCenterDto>();
        this.MapDirectoryListing<Hospital, HospitalImage, HospitalDto>();
    }
}
