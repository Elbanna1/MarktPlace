using AutoMapper;
using Domain.Entities;
using Shared.Constants;
using Shared.DTOs.Lookups;

namespace Services.Mapping;

public class LookupMappingProfile : Profile
{
    public LookupMappingProfile()
    {
        CreateMap<Category, CategoryDto>()
            .ForMember(d => d.OpensFormDirectly, o => o.MapFrom(s => CategoryCatalog.OpensFormDirectly(s.Id)))
            .ForMember(d => d.DirectSubCategoryId, o => o.MapFrom(s => CategoryCatalog.DirectSubCategoryOf(s.Id)));

        CreateMap<Category, CategoryTreeDto>()
            .ForMember(d => d.OpensFormDirectly, o => o.MapFrom(s => CategoryCatalog.OpensFormDirectly(s.Id)))
            .ForMember(d => d.DirectSubCategoryId, o => o.MapFrom(s => CategoryCatalog.DirectSubCategoryOf(s.Id)));
        CreateMap<SubCategory, CategoryTreeSubCategoryDto>();

        CreateMap<SubCategory, SubCategoryDto>()
            .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category != null ? s.Category.Name : null));

        CreateMap<ListingTypeLookup, ListingTypeDto>();

        CreateMap<Governorate, GovernorateDto>();

        CreateMap<Center, CenterDto>()
            .ForMember(d => d.GovernorateName, o => o.MapFrom(s => s.Governorate != null ? s.Governorate.Name : null))
            .ForMember(d => d.ProjectsCount, o => o.MapFrom(s => s.Projects.Count(p => p.IsActive)));

        CreateMap<Project, ProjectDto>()
            .ForMember(d => d.CenterName, o => o.MapFrom(s => s.Center != null ? s.Center.Name : null))
            .ForMember(d => d.GovernorateId, o => o.MapFrom(s => s.Center != null ? s.Center.GovernorateId : 0))
            .ForMember(d => d.GovernorateName,
                o => o.MapFrom(s => s.Center != null && s.Center.Governorate != null
                    ? s.Center.Governorate.Name
                    : null));

        CreateMap<WorkshopTypeLookup, WorkshopTypeDto>();

        CreateMap<CraftsmanSpecializationLookup, SpecializationDto>();

        CreateMap<ExperienceLevelLookup, ExperienceLevelDto>();

        CreateMap<ProductionSpecialtyLookup, ProductionSpecialtyDto>();
        CreateMap<FarmTypeLookup, FarmTypeDto>();
        CreateMap<AvailabilitySeasonLookup, AvailabilitySeasonDto>();
        CreateMap<FarmingMethodLookup, FarmingMethodDto>();
        CreateMap<CompanyFieldLookup, CompanyFieldDto>();
        CreateMap<SupplierTypeLookup, SupplierTypeDto>();
        CreateMap<TradeTypeLookup, TradeTypeDto>();
        CreateMap<SaleTypeLookup, SaleTypeDto>();
    }
}
