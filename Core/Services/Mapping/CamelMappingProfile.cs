using AutoMapper;
using Domain.Entities;
using Shared.Constants;
using Shared.DTOs.Animals;

namespace Services.Mapping;

public class CamelMappingProfile : Profile
{
    public CamelMappingProfile()
    {
        CreateMap<CamelImage, CamelImageDto>()
            .ForMember(d => d.Url, o => o.MapFrom(s => s.ImageUrl));

        CreateMap<CamelBreedLookup, AnimalLookupItemDto>();
        CreateMap<CamelPurposeLookup, AnimalLookupItemDto>();
        CreateMap<CamelAgeLookup, AnimalLookupItemDto>();
        CreateMap<CamelGenderLookup, AnimalLookupItemDto>();
        CreateMap<CamelHealthStatusLookup, AnimalLookupItemDto>();
        CreateMap<CamelVaccinationLookup, AnimalLookupItemDto>();

        CreateMap<Camel, CamelListItemDto>()
            .ForMember(d => d.BreedName,
                o => o.MapFrom(s => CamelCatalog.GetBreedName(s.Breed)))
            .ForMember(d => d.PurposeName,
                o => o.MapFrom(s => CamelCatalog.GetPurposeName(s.Purpose)))
            .ForMember(d => d.AgeName,
                o => o.MapFrom(s => CamelCatalog.GetAgeName(s.Age)))
            .ForMember(d => d.GenderName,
                o => o.MapFrom(s => CamelCatalog.GetGenderName(s.Gender)))
            .ForMember(d => d.HealthStatusName,
                o => o.MapFrom(s => CamelCatalog.GetHealthStatusName(s.HealthStatus)))
            .ForMember(d => d.VaccinationName,
                o => o.MapFrom(s => CamelCatalog.GetVaccinationName(s.Vaccination)))
            .ForMember(d => d.PrimaryImageUrl, o => o.MapFrom(s =>
                s.Images.OrderByDescending(i => i.IsPrimary).Select(i => i.ImageUrl).FirstOrDefault()));

        CreateMap<Camel, CamelDetailsDto>()
            .ForMember(d => d.OwnerId, o => o.MapFrom(s => s.UserId))
            .ForMember(d => d.BreedName,
                o => o.MapFrom(s => CamelCatalog.GetBreedName(s.Breed)))
            .ForMember(d => d.PurposeName,
                o => o.MapFrom(s => CamelCatalog.GetPurposeName(s.Purpose)))
            .ForMember(d => d.AgeName,
                o => o.MapFrom(s => CamelCatalog.GetAgeName(s.Age)))
            .ForMember(d => d.GenderName,
                o => o.MapFrom(s => CamelCatalog.GetGenderName(s.Gender)))
            .ForMember(d => d.HealthStatusName,
                o => o.MapFrom(s => CamelCatalog.GetHealthStatusName(s.HealthStatus)))
            .ForMember(d => d.VaccinationName,
                o => o.MapFrom(s => CamelCatalog.GetVaccinationName(s.Vaccination)));

        CreateMap<CreateCamelRequest, Camel>()
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
