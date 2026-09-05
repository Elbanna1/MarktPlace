using AutoMapper;
using Domain.Entities;
using Shared.Constants;
using Shared.DTOs.Animals;

namespace Services.Mapping;

public class BirdMappingProfile : Profile
{
    public BirdMappingProfile()
    {
        CreateMap<BirdImage, BirdImageDto>()
            .ForMember(d => d.Url, o => o.MapFrom(s => s.ImageUrl));

        CreateMap<BirdTypeLookup, AnimalLookupItemDto>();
        CreateMap<BirdPurposeLookup, AnimalLookupItemDto>();
        CreateMap<BirdAgeLookup, AnimalLookupItemDto>();
        CreateMap<BirdGenderLookup, AnimalLookupItemDto>();
        CreateMap<BirdHealthStatusLookup, AnimalLookupItemDto>();
        CreateMap<BirdVaccinationLookup, AnimalLookupItemDto>();

        CreateMap<Bird, BirdListItemDto>()
            .ForMember(d => d.AnimalTypeName,
                o => o.MapFrom(s => BirdCatalog.GetTypeName(s.AnimalType)))
            .ForMember(d => d.PurposeName,
                o => o.MapFrom(s => BirdCatalog.GetPurposeName(s.Purpose)))
            .ForMember(d => d.AgeName,
                o => o.MapFrom(s => BirdCatalog.GetAgeName(s.Age)))
            .ForMember(d => d.GenderName,
                o => o.MapFrom(s => BirdCatalog.GetGenderName(s.Gender)))
            .ForMember(d => d.HealthStatusName,
                o => o.MapFrom(s => BirdCatalog.GetHealthStatusName(s.HealthStatus)))
            .ForMember(d => d.VaccinationName,
                o => o.MapFrom(s => BirdCatalog.GetVaccinationName(s.Vaccination)))
            .ForMember(d => d.PrimaryImageUrl, o => o.MapFrom(s =>
                s.Images.OrderByDescending(i => i.IsPrimary).Select(i => i.ImageUrl).FirstOrDefault()));

        CreateMap<Bird, BirdDetailsDto>()
            .ForMember(d => d.OwnerId, o => o.MapFrom(s => s.UserId))
            .ForMember(d => d.AnimalTypeName,
                o => o.MapFrom(s => BirdCatalog.GetTypeName(s.AnimalType)))
            .ForMember(d => d.PurposeName,
                o => o.MapFrom(s => BirdCatalog.GetPurposeName(s.Purpose)))
            .ForMember(d => d.AgeName,
                o => o.MapFrom(s => BirdCatalog.GetAgeName(s.Age)))
            .ForMember(d => d.GenderName,
                o => o.MapFrom(s => BirdCatalog.GetGenderName(s.Gender)))
            .ForMember(d => d.HealthStatusName,
                o => o.MapFrom(s => BirdCatalog.GetHealthStatusName(s.HealthStatus)))
            .ForMember(d => d.VaccinationName,
                o => o.MapFrom(s => BirdCatalog.GetVaccinationName(s.Vaccination)));

        CreateMap<CreateBirdRequest, Bird>()
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
