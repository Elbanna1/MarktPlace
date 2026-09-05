using AutoMapper;
using Domain.Entities;
using Shared.Constants;
using Shared.DTOs.Animals;

namespace Services.Mapping;

public class LivestockMappingProfile : Profile
{
    public LivestockMappingProfile()
    {
        CreateMap<LivestockImage, LivestockImageDto>()
            .ForMember(d => d.Url, o => o.MapFrom(s => s.ImageUrl));

        CreateMap<LivestockBreedLookup, AnimalLookupItemDto>();
        CreateMap<LivestockPurposeLookup, AnimalLookupItemDto>();
        CreateMap<LivestockAgeLookup, AnimalLookupItemDto>();
        CreateMap<LivestockGenderLookup, AnimalLookupItemDto>();
        CreateMap<LivestockHealthStatusLookup, AnimalLookupItemDto>();
        CreateMap<LivestockVaccinationLookup, AnimalLookupItemDto>();
        CreateMap<LivestockProductionLookup, AnimalLookupItemDto>();

        CreateMap<Livestock, LivestockListItemDto>()
            .ForMember(d => d.BreedName,
                o => o.MapFrom(s => LivestockCatalog.GetBreedName(s.Breed)))
            .ForMember(d => d.PurposeName,
                o => o.MapFrom(s => LivestockCatalog.GetPurposeName(s.Purpose)))
            .ForMember(d => d.AgeName,
                o => o.MapFrom(s => LivestockCatalog.GetAgeName(s.Age)))
            .ForMember(d => d.GenderName,
                o => o.MapFrom(s => LivestockCatalog.GetGenderName(s.Gender)))
            .ForMember(d => d.HealthStatusName,
                o => o.MapFrom(s => LivestockCatalog.GetHealthStatusName(s.HealthStatus)))
            .ForMember(d => d.VaccinationName,
                o => o.MapFrom(s => LivestockCatalog.GetVaccinationName(s.Vaccination)))
            .ForMember(d => d.ProductionName,
                o => o.MapFrom(s => LivestockCatalog.GetProductionName(s.Production)))
            .ForMember(d => d.PrimaryImageUrl, o => o.MapFrom(s =>
                s.Images.OrderByDescending(i => i.IsPrimary).Select(i => i.ImageUrl).FirstOrDefault()));

        CreateMap<Livestock, LivestockDetailsDto>()
            .ForMember(d => d.OwnerId, o => o.MapFrom(s => s.UserId))
            .ForMember(d => d.BreedName,
                o => o.MapFrom(s => LivestockCatalog.GetBreedName(s.Breed)))
            .ForMember(d => d.PurposeName,
                o => o.MapFrom(s => LivestockCatalog.GetPurposeName(s.Purpose)))
            .ForMember(d => d.AgeName,
                o => o.MapFrom(s => LivestockCatalog.GetAgeName(s.Age)))
            .ForMember(d => d.GenderName,
                o => o.MapFrom(s => LivestockCatalog.GetGenderName(s.Gender)))
            .ForMember(d => d.HealthStatusName,
                o => o.MapFrom(s => LivestockCatalog.GetHealthStatusName(s.HealthStatus)))
            .ForMember(d => d.VaccinationName,
                o => o.MapFrom(s => LivestockCatalog.GetVaccinationName(s.Vaccination)))
            .ForMember(d => d.ProductionName,
                o => o.MapFrom(s => LivestockCatalog.GetProductionName(s.Production)));

        CreateMap<CreateLivestockRequest, Livestock>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.User, o => o.Ignore())
            .ForMember(d => d.UserId, o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.UpdatedAt, o => o.Ignore())
            .ForMember(d => d.IsDeleted, o => o.Ignore())
            .ForMember(d => d.DeletedAt, o => o.Ignore())
            .ForMember(d => d.Images, o => o.Ignore());
    }
}
