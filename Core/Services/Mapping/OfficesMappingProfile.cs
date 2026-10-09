using AutoMapper;
using Domain.Entities;
using Shared.DTOs.Offices;

namespace Services.Mapping;

public class OfficesMappingProfile : Profile
{
    public OfficesMappingProfile()
    {
        this.MapDirectoryListing<LawyerOffice, LawyerOfficeImage, LawyerOfficeDto>();
        this.MapDirectoryListing<EngineeringConsultancyOffice, EngineeringConsultancyOfficeImage, EngineeringConsultancyOfficeDto>();
        this.MapDirectoryListing<Contractor, ContractorImage, ContractorDto>();
        this.MapDirectoryListing<IndustrialConsultancyOffice, IndustrialConsultancyOfficeImage, IndustrialConsultancyOfficeDto>();
        this.MapDirectoryListing<AccountantOffice, AccountantOfficeImage, AccountantOfficeDto>();
    }
}
