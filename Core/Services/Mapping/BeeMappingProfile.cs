using AutoMapper;
using Domain.Entities;
using Shared.Constants;
using Shared.DTOs.Animals;

namespace Services.Mapping;

public class BeeMappingProfile : Profile
{
    public BeeMappingProfile()
    {
        CreateMap<BeeImage, BeeImageDto>()
            .ForMember(d => d.Url, o => o.MapFrom(s => s.ImageUrl));

        CreateMap<BeeTypeLookup, AnimalLookupItemDto>();
        CreateMap<BeePurposeLookup, AnimalLookupItemDto>();
        CreateMap<BeeHealthStatusLookup, AnimalLookupItemDto>();
        CreateMap<BeeProductionLookup, AnimalLookupItemDto>();

        CreateMap<Bee, BeeListItemDto>()
            .ForMember(d => d.AnimalTypeName,
                o => o.MapFrom(s => BeeCatalog.GetTypeName(s.AnimalType)))
            .ForMember(d => d.PurposeName,
                o => o.MapFrom(s => BeeCatalog.GetPurposeName(s.Purpose)))
            .ForMember(d => d.HealthStatusName,
                o => o.MapFrom(s => BeeCatalog.GetHealthStatusName(s.HealthStatus)))
            .ForMember(d => d.ProductionName,
                o => o.MapFrom(s => BeeCatalog.GetProductionName(s.Production)))
            .ForMember(d => d.PrimaryImageUrl, o => o.MapFrom(s =>
                s.Images.OrderByDescending(i => i.IsPrimary).Select(i => i.ImageUrl).FirstOrDefault()));

        CreateMap<Bee, BeeDetailsDto>()
            .ForMember(d => d.OwnerId, o => o.MapFrom(s => s.UserId))
            .ForMember(d => d.AnimalTypeName,
                o => o.MapFrom(s => BeeCatalog.GetTypeName(s.AnimalType)))
            .ForMember(d => d.PurposeName,
                o => o.MapFrom(s => BeeCatalog.GetPurposeName(s.Purpose)))
            .ForMember(d => d.HealthStatusName,
                o => o.MapFrom(s => BeeCatalog.GetHealthStatusName(s.HealthStatus)))
            .ForMember(d => d.ProductionName,
                o => o.MapFrom(s => BeeCatalog.GetProductionName(s.Production)));

        CreateMap<CreateBeeRequest, Bee>()
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
