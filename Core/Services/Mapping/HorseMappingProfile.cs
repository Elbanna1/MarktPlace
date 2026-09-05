using AutoMapper;
using Domain.Entities;
using Shared.Constants;
using Shared.DTOs.Animals;

namespace Services.Mapping;

public class HorseMappingProfile : Profile
{
    public HorseMappingProfile()
    {
        CreateMap<HorseImage, HorseImageDto>()
            .ForMember(d => d.Url, o => o.MapFrom(s => s.ImageUrl));

        CreateMap<HorseBreedLookup, AnimalLookupItemDto>();
        CreateMap<HorsePurposeLookup, AnimalLookupItemDto>();
        CreateMap<HorseAgeLookup, AnimalLookupItemDto>();
        CreateMap<HorseGenderLookup, AnimalLookupItemDto>();
        CreateMap<HorseHealthStatusLookup, AnimalLookupItemDto>();
        CreateMap<HorseTrainingLevelLookup, AnimalLookupItemDto>();
        CreateMap<HorseVaccinationLookup, AnimalLookupItemDto>();

        CreateMap<Horse, HorseListItemDto>()
            .ForMember(d => d.BreedName,
                o => o.MapFrom(s => HorseCatalog.GetBreedName(s.Breed)))
            .ForMember(d => d.PurposeName,
                o => o.MapFrom(s => HorseCatalog.GetPurposeName(s.Purpose)))
            .ForMember(d => d.AgeName,
                o => o.MapFrom(s => HorseCatalog.GetAgeName(s.Age)))
            .ForMember(d => d.GenderName,
                o => o.MapFrom(s => HorseCatalog.GetGenderName(s.Gender)))
            .ForMember(d => d.HealthStatusName,
                o => o.MapFrom(s => HorseCatalog.GetHealthStatusName(s.HealthStatus)))
            .ForMember(d => d.TrainingLevelName,
                o => o.MapFrom(s => HorseCatalog.GetTrainingLevelName(s.TrainingLevel)))
            .ForMember(d => d.VaccinationName,
                o => o.MapFrom(s => HorseCatalog.GetVaccinationName(s.Vaccination)))
            .ForMember(d => d.PrimaryImageUrl, o => o.MapFrom(s =>
                s.Images.OrderByDescending(i => i.IsPrimary).Select(i => i.ImageUrl).FirstOrDefault()));

        CreateMap<Horse, HorseDetailsDto>()
            .ForMember(d => d.OwnerId, o => o.MapFrom(s => s.UserId))
            .ForMember(d => d.BreedName,
                o => o.MapFrom(s => HorseCatalog.GetBreedName(s.Breed)))
            .ForMember(d => d.PurposeName,
                o => o.MapFrom(s => HorseCatalog.GetPurposeName(s.Purpose)))
            .ForMember(d => d.AgeName,
                o => o.MapFrom(s => HorseCatalog.GetAgeName(s.Age)))
            .ForMember(d => d.GenderName,
                o => o.MapFrom(s => HorseCatalog.GetGenderName(s.Gender)))
            .ForMember(d => d.HealthStatusName,
                o => o.MapFrom(s => HorseCatalog.GetHealthStatusName(s.HealthStatus)))
            .ForMember(d => d.TrainingLevelName,
                o => o.MapFrom(s => HorseCatalog.GetTrainingLevelName(s.TrainingLevel)))
            .ForMember(d => d.VaccinationName,
                o => o.MapFrom(s => HorseCatalog.GetVaccinationName(s.Vaccination)));

        CreateMap<CreateHorseRequest, Horse>()
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
