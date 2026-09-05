using AutoMapper;
using Domain.Entities;
using Shared.Constants;
using Shared.DTOs.HomeFurnishing;

namespace Services.Mapping;

public class PlantOrnamentMappingProfile : Profile
{
    public PlantOrnamentMappingProfile()
    {
        CreateMap<PlantOrnamentImage, HomeFurnishingImageDto>()
            .ForMember(d => d.Url, o => o.MapFrom(s => s.ImageUrl));

        CreateMap<PlantOrnamentProductTypeLookup, HomeFurnishingLookupItemDto>();
        CreateMap<PlantOrnamentSuitableForLookup, HomeFurnishingLookupItemDto>();

        CreateMap<PlantOrnament, PlantOrnamentListItemDto>()
            .ForMember(d => d.ProductTypeName,
                o => o.MapFrom(s => PlantOrnamentCatalog.GetProductTypeName(s.ProductType)))
            .ForMember(d => d.SuitableForName,
                o => o.MapFrom(s => PlantOrnamentCatalog.GetSuitableForName(s.SuitableFor)))
            .ForMember(d => d.Images,
                o => o.MapFrom(s => s.Images.OrderBy(i => i.SortOrder)))
            .ForMember(d => d.PrimaryImageUrl, o => o.MapFrom(s =>
                s.Images.OrderBy(i => i.SortOrder).Select(i => i.ImageUrl).FirstOrDefault()));

        CreateMap<PlantOrnament, PlantOrnamentDetailsDto>()
            .ForMember(d => d.OwnerId, o => o.MapFrom(s => s.UserId))
            .ForMember(d => d.ShareUrl, o => o.Ignore())
            .ForMember(d => d.ProductTypeName,
                o => o.MapFrom(s => PlantOrnamentCatalog.GetProductTypeName(s.ProductType)))
            .ForMember(d => d.SuitableForName,
                o => o.MapFrom(s => PlantOrnamentCatalog.GetSuitableForName(s.SuitableFor)))
            .ForMember(d => d.Images,
                o => o.MapFrom(s => s.Images.OrderBy(i => i.SortOrder)));
    }
}
