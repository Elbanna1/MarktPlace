using AutoMapper;
using Domain.Entities;
using Shared.Constants;
using Shared.DTOs.HomeFurnishing;

namespace Services.Mapping;

public class LightingDecorMappingProfile : Profile
{
    public LightingDecorMappingProfile()
    {
        CreateMap<LightingDecorImage, HomeFurnishingImageDto>()
            .ForMember(d => d.Url, o => o.MapFrom(s => s.ImageUrl));

        CreateMap<LightingDecorProductTypeLookup, HomeFurnishingLookupItemDto>();
        CreateMap<LightingDecorMaterialLookup, HomeFurnishingLookupItemDto>();
        CreateMap<LightingDecorColorLookup, HomeFurnishingLookupItemDto>();
        CreateMap<LightingDecorLightTypeLookup, HomeFurnishingLookupItemDto>();

        CreateMap<LightingDecor, LightingDecorListItemDto>()
            .ForMember(d => d.ProductTypeName,
                o => o.MapFrom(s => LightingDecorCatalog.GetProductTypeName(s.ProductType)))
            .ForMember(d => d.MaterialName,
                o => o.MapFrom(s => LightingDecorCatalog.GetMaterialName(s.Material)))
            .ForMember(d => d.LightTypeName,
                o => o.MapFrom(s => LightingDecorCatalog.GetLightTypeName(s.LightType)))
            .ForMember(d => d.Colors,
                o => o.MapFrom(s => LightingDecorCatalog.SelectedColors(s.Colors.Select(x => x.Color))))
            .ForMember(d => d.Images,
                o => o.MapFrom(s => s.Images.OrderBy(i => i.SortOrder)))
            .ForMember(d => d.PrimaryImageUrl, o => o.MapFrom(s =>
                s.Images.OrderBy(i => i.SortOrder).Select(i => i.ImageUrl).FirstOrDefault()));

        CreateMap<LightingDecor, LightingDecorDetailsDto>()
            .ForMember(d => d.OwnerId, o => o.MapFrom(s => s.UserId))
            .ForMember(d => d.ShareUrl, o => o.Ignore())
            .ForMember(d => d.ProductTypeName,
                o => o.MapFrom(s => LightingDecorCatalog.GetProductTypeName(s.ProductType)))
            .ForMember(d => d.MaterialName,
                o => o.MapFrom(s => LightingDecorCatalog.GetMaterialName(s.Material)))
            .ForMember(d => d.LightTypeName,
                o => o.MapFrom(s => LightingDecorCatalog.GetLightTypeName(s.LightType)))
            .ForMember(d => d.Colors,
                o => o.MapFrom(s => LightingDecorCatalog.SelectedColors(s.Colors.Select(x => x.Color))))
            .ForMember(d => d.Images,
                o => o.MapFrom(s => s.Images.OrderBy(i => i.SortOrder)));
    }
}
