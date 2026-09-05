using AutoMapper;
using Domain.Entities;
using Shared.Constants;
using Shared.DTOs.Animals;

namespace Services.Mapping;

public class OtherAnimalMappingProfile : Profile
{
    public OtherAnimalMappingProfile()
    {
        CreateMap<OtherAnimalImage, OtherAnimalImageDto>()
            .ForMember(d => d.Url, o => o.MapFrom(s => s.ImageUrl));

        CreateMap<OtherAnimalTypeLookup, AnimalLookupItemDto>();
        CreateMap<OtherAnimalPurposeLookup, AnimalLookupItemDto>();
        CreateMap<OtherAnimalAgeLookup, AnimalLookupItemDto>();
        CreateMap<OtherAnimalGenderLookup, AnimalLookupItemDto>();
        CreateMap<OtherAnimalHealthStatusLookup, AnimalLookupItemDto>();
        CreateMap<OtherAnimalVaccinationLookup, AnimalLookupItemDto>();

        CreateMap<OtherAnimal, OtherAnimalListItemDto>()
            .ForMember(d => d.AnimalTypeName,
                o => o.MapFrom(s => OtherAnimalCatalog.GetTypeName(s.AnimalType)))
            .ForMember(d => d.PurposeName,
                o => o.MapFrom(s => OtherAnimalCatalog.GetPurposeName(s.Purpose)))
            .ForMember(d => d.AgeName,
                o => o.MapFrom(s => OtherAnimalCatalog.GetAgeName(s.Age)))
            .ForMember(d => d.GenderName,
                o => o.MapFrom(s => OtherAnimalCatalog.GetGenderName(s.Gender)))
            .ForMember(d => d.HealthStatusName,
                o => o.MapFrom(s => OtherAnimalCatalog.GetHealthStatusName(s.HealthStatus)))
            .ForMember(d => d.VaccinationName,
                o => o.MapFrom(s => OtherAnimalCatalog.GetVaccinationName(s.Vaccination)))
            .ForMember(d => d.PrimaryImageUrl, o => o.MapFrom(s =>
                s.Images.OrderByDescending(i => i.IsPrimary).Select(i => i.ImageUrl).FirstOrDefault()));

        CreateMap<OtherAnimal, OtherAnimalDetailsDto>()
            .ForMember(d => d.OwnerId, o => o.MapFrom(s => s.UserId))
            .ForMember(d => d.AnimalTypeName,
                o => o.MapFrom(s => OtherAnimalCatalog.GetTypeName(s.AnimalType)))
            .ForMember(d => d.PurposeName,
                o => o.MapFrom(s => OtherAnimalCatalog.GetPurposeName(s.Purpose)))
            .ForMember(d => d.AgeName,
                o => o.MapFrom(s => OtherAnimalCatalog.GetAgeName(s.Age)))
            .ForMember(d => d.GenderName,
                o => o.MapFrom(s => OtherAnimalCatalog.GetGenderName(s.Gender)))
            .ForMember(d => d.HealthStatusName,
                o => o.MapFrom(s => OtherAnimalCatalog.GetHealthStatusName(s.HealthStatus)))
            .ForMember(d => d.VaccinationName,
                o => o.MapFrom(s => OtherAnimalCatalog.GetVaccinationName(s.Vaccination)));

        CreateMap<CreateOtherAnimalRequest, OtherAnimal>()
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
