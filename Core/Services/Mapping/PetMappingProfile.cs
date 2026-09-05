using AutoMapper;
using Domain.Entities;
using Shared.Constants;
using Shared.DTOs.Animals;

namespace Services.Mapping;

public class PetMappingProfile : Profile
{
    public PetMappingProfile()
    {
        CreateMap<PetImage, PetImageDto>()
            .ForMember(d => d.Url, o => o.MapFrom(s => s.ImageUrl));

        CreateMap<PetBreedLookup, AnimalLookupItemDto>();
        CreateMap<PetPurposeLookup, AnimalLookupItemDto>();
        CreateMap<PetAgeLookup, AnimalLookupItemDto>();
        CreateMap<PetGenderLookup, AnimalLookupItemDto>();
        CreateMap<PetHealthStatusLookup, AnimalLookupItemDto>();
        CreateMap<PetTrainingLevelLookup, AnimalLookupItemDto>();
        CreateMap<PetVaccinationLookup, AnimalLookupItemDto>();

        CreateMap<Pet, PetListItemDto>()
            .ForMember(d => d.BreedName,
                o => o.MapFrom(s => PetCatalog.GetBreedName(s.Breed)))
            .ForMember(d => d.PurposeName,
                o => o.MapFrom(s => PetCatalog.GetPurposeName(s.Purpose)))
            .ForMember(d => d.AgeName,
                o => o.MapFrom(s => PetCatalog.GetAgeName(s.Age)))
            .ForMember(d => d.GenderName,
                o => o.MapFrom(s => PetCatalog.GetGenderName(s.Gender)))
            .ForMember(d => d.HealthStatusName,
                o => o.MapFrom(s => PetCatalog.GetHealthStatusName(s.HealthStatus)))
            .ForMember(d => d.TrainingLevelName,
                o => o.MapFrom(s => PetCatalog.GetTrainingLevelName(s.TrainingLevel)))
            .ForMember(d => d.VaccinationName,
                o => o.MapFrom(s => PetCatalog.GetVaccinationName(s.Vaccination)))
            .ForMember(d => d.PrimaryImageUrl, o => o.MapFrom(s =>
                s.Images.OrderByDescending(i => i.IsPrimary).Select(i => i.ImageUrl).FirstOrDefault()));

        CreateMap<Pet, PetDetailsDto>()
            .ForMember(d => d.OwnerId, o => o.MapFrom(s => s.UserId))
            .ForMember(d => d.BreedName,
                o => o.MapFrom(s => PetCatalog.GetBreedName(s.Breed)))
            .ForMember(d => d.PurposeName,
                o => o.MapFrom(s => PetCatalog.GetPurposeName(s.Purpose)))
            .ForMember(d => d.AgeName,
                o => o.MapFrom(s => PetCatalog.GetAgeName(s.Age)))
            .ForMember(d => d.GenderName,
                o => o.MapFrom(s => PetCatalog.GetGenderName(s.Gender)))
            .ForMember(d => d.HealthStatusName,
                o => o.MapFrom(s => PetCatalog.GetHealthStatusName(s.HealthStatus)))
            .ForMember(d => d.TrainingLevelName,
                o => o.MapFrom(s => PetCatalog.GetTrainingLevelName(s.TrainingLevel)))
            .ForMember(d => d.VaccinationName,
                o => o.MapFrom(s => PetCatalog.GetVaccinationName(s.Vaccination)));

        CreateMap<CreatePetRequest, Pet>()
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
