using AutoMapper;
using Domain.Entities;
using Shared.Constants;
using Shared.DTOs.Animals;

namespace Services.Mapping;

public class SheepGoatMappingProfile : Profile
{
    public SheepGoatMappingProfile()
    {
        CreateMap<SheepGoatImage, SheepGoatImageDto>()
            .ForMember(d => d.Url, o => o.MapFrom(s => s.ImageUrl));

        CreateMap<SheepGoatBreedLookup, AnimalLookupItemDto>();
        CreateMap<SheepGoatPurposeLookup, AnimalLookupItemDto>();
        CreateMap<SheepGoatAgeLookup, AnimalLookupItemDto>();
        CreateMap<SheepGoatGenderLookup, AnimalLookupItemDto>();
        CreateMap<SheepGoatHealthStatusLookup, AnimalLookupItemDto>();
        CreateMap<SheepGoatVaccinationLookup, AnimalLookupItemDto>();

        CreateMap<SheepGoat, SheepGoatListItemDto>()
            .ForMember(d => d.BreedName,
                o => o.MapFrom(s => SheepGoatCatalog.GetBreedName(s.Breed)))
            .ForMember(d => d.PurposeName,
                o => o.MapFrom(s => SheepGoatCatalog.GetPurposeName(s.Purpose)))
            .ForMember(d => d.AgeName,
                o => o.MapFrom(s => SheepGoatCatalog.GetAgeName(s.Age)))
            .ForMember(d => d.GenderName,
                o => o.MapFrom(s => SheepGoatCatalog.GetGenderName(s.Gender)))
            .ForMember(d => d.HealthStatusName,
                o => o.MapFrom(s => SheepGoatCatalog.GetHealthStatusName(s.HealthStatus)))
            .ForMember(d => d.VaccinationName,
                o => o.MapFrom(s => SheepGoatCatalog.GetVaccinationName(s.Vaccination)))
            .ForMember(d => d.PrimaryImageUrl, o => o.MapFrom(s =>
                s.Images.OrderByDescending(i => i.IsPrimary).Select(i => i.ImageUrl).FirstOrDefault()));

        CreateMap<SheepGoat, SheepGoatDetailsDto>()
            .ForMember(d => d.OwnerId, o => o.MapFrom(s => s.UserId))
            .ForMember(d => d.BreedName,
                o => o.MapFrom(s => SheepGoatCatalog.GetBreedName(s.Breed)))
            .ForMember(d => d.PurposeName,
                o => o.MapFrom(s => SheepGoatCatalog.GetPurposeName(s.Purpose)))
            .ForMember(d => d.AgeName,
                o => o.MapFrom(s => SheepGoatCatalog.GetAgeName(s.Age)))
            .ForMember(d => d.GenderName,
                o => o.MapFrom(s => SheepGoatCatalog.GetGenderName(s.Gender)))
            .ForMember(d => d.HealthStatusName,
                o => o.MapFrom(s => SheepGoatCatalog.GetHealthStatusName(s.HealthStatus)))
            .ForMember(d => d.VaccinationName,
                o => o.MapFrom(s => SheepGoatCatalog.GetVaccinationName(s.Vaccination)));

        CreateMap<CreateSheepGoatRequest, SheepGoat>()
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
