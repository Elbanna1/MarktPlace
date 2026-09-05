using AutoMapper;
using Domain.Entities;
using Shared.Constants;
using Shared.DTOs.Companies;

namespace Services.Mapping;

public class CompanyMappingProfile : Profile
{
    public CompanyMappingProfile()
    {
        CreateMap<CompanyImage, CompanyImageDto>()
            .ForMember(d => d.Url, o => o.MapFrom(s => s.ImageUrl));

        CreateMap<CompanyFieldLookup, CompanyFieldOptionDto>();

        CreateMap<Company, CompanyListItemDto>()
            .ForMember(d => d.CompanyFieldName,
                o => o.MapFrom(s => BusinessCatalog.GetCompanyFieldName(s.CompanyField)))
            .ForMember(d => d.CompanyFieldGroup,
                o => o.MapFrom(s => BusinessCatalog.GetCompanyFieldGroup(s.CompanyField)))
            .ForMember(d => d.PrimaryImageUrl, o => o.MapFrom(s =>
                s.Images.OrderByDescending(i => i.IsPrimary).Select(i => i.ImageUrl).FirstOrDefault()));

        CreateMap<Company, CompanyDetailsDto>()
            .ForMember(d => d.OwnerId, o => o.MapFrom(s => s.UserId))
            .ForMember(d => d.CompanyFieldName,
                o => o.MapFrom(s => BusinessCatalog.GetCompanyFieldName(s.CompanyField)))
            .ForMember(d => d.CompanyFieldGroup,
                o => o.MapFrom(s => BusinessCatalog.GetCompanyFieldGroup(s.CompanyField)));

        CreateMap<CreateCompanyRequest, Company>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.User, o => o.Ignore())
            .ForMember(d => d.UserId, o => o.Ignore())
            .ForMember(d => d.LogoPath, o => o.Ignore())
            .ForMember(d => d.LogoUrl, o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.UpdatedAt, o => o.Ignore())
            .ForMember(d => d.IsDeleted, o => o.Ignore())
            .ForMember(d => d.DeletedAt, o => o.Ignore())
            .ForMember(d => d.Images, o => o.Ignore());
    }
}
