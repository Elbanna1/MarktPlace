using AutoMapper;
using Domain.Entities;
using Shared.Constants;
using Shared.DTOs.Animals;

namespace Services.Mapping;

public class FishMappingProfile : Profile
{
    public FishMappingProfile()
    {
        CreateMap<FishImage, FishImageDto>()
            .ForMember(d => d.Url, o => o.MapFrom(s => s.ImageUrl));

        CreateMap<FishTypeLookup, AnimalLookupItemDto>();
        CreateMap<FishPurposeLookup, AnimalLookupItemDto>();
        CreateMap<FishAgeLookup, AnimalLookupItemDto>();
        CreateMap<FishHealthStatusLookup, AnimalLookupItemDto>();

        CreateMap<Fish, FishListItemDto>()
            .ForMember(d => d.AnimalTypeName,
                o => o.MapFrom(s => FishCatalog.GetTypeName(s.AnimalType)))
            .ForMember(d => d.PurposeName,
                o => o.MapFrom(s => FishCatalog.GetPurposeName(s.Purpose)))
            .ForMember(d => d.AgeName,
                o => o.MapFrom(s => FishCatalog.GetAgeName(s.Age)))
            .ForMember(d => d.HealthStatusName,
                o => o.MapFrom(s => FishCatalog.GetHealthStatusName(s.HealthStatus)))
            .ForMember(d => d.PrimaryImageUrl, o => o.MapFrom(s =>
                s.Images.OrderByDescending(i => i.IsPrimary).Select(i => i.ImageUrl).FirstOrDefault()));

        CreateMap<Fish, FishDetailsDto>()
            .ForMember(d => d.OwnerId, o => o.MapFrom(s => s.UserId))
            .ForMember(d => d.AnimalTypeName,
                o => o.MapFrom(s => FishCatalog.GetTypeName(s.AnimalType)))
            .ForMember(d => d.PurposeName,
                o => o.MapFrom(s => FishCatalog.GetPurposeName(s.Purpose)))
            .ForMember(d => d.AgeName,
                o => o.MapFrom(s => FishCatalog.GetAgeName(s.Age)))
            .ForMember(d => d.HealthStatusName,
                o => o.MapFrom(s => FishCatalog.GetHealthStatusName(s.HealthStatus)));

        CreateMap<CreateFishRequest, Fish>()
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
