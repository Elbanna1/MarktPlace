using AutoMapper;
using Domain.Entities;
using Shared.Constants;
using Shared.DTOs.HomeFurnishing;

namespace Services.Mapping;

public class KitchenToolMappingProfile : Profile
{
    public KitchenToolMappingProfile()
    {
        CreateMap<KitchenToolImage, HomeFurnishingImageDto>()
            .ForMember(d => d.Url, o => o.MapFrom(s => s.ImageUrl));

        CreateMap<KitchenToolProductTypeLookup, HomeFurnishingLookupItemDto>();
        CreateMap<KitchenToolMaterialLookup, HomeFurnishingLookupItemDto>();
        CreateMap<KitchenToolColorLookup, HomeFurnishingLookupItemDto>();

        CreateMap<KitchenTool, KitchenToolListItemDto>()
            .ForMember(d => d.ProductTypeName,
                o => o.MapFrom(s => KitchenToolCatalog.GetProductTypeName(s.ProductType)))
            .ForMember(d => d.MaterialName,
                o => o.MapFrom(s => KitchenToolCatalog.GetMaterialName(s.Material)))
            .ForMember(d => d.Colors,
                o => o.MapFrom(s => KitchenToolCatalog.SelectedColors(s.Colors.Select(x => x.Color))))
            .ForMember(d => d.Images,
                o => o.MapFrom(s => s.Images.OrderBy(i => i.SortOrder)))
            .ForMember(d => d.PrimaryImageUrl, o => o.MapFrom(s =>
                s.Images.OrderBy(i => i.SortOrder).Select(i => i.ImageUrl).FirstOrDefault()));

        CreateMap<KitchenTool, KitchenToolDetailsDto>()
            .ForMember(d => d.OwnerId, o => o.MapFrom(s => s.UserId))
            .ForMember(d => d.ShareUrl, o => o.Ignore())
            .ForMember(d => d.ProductTypeName,
                o => o.MapFrom(s => KitchenToolCatalog.GetProductTypeName(s.ProductType)))
            .ForMember(d => d.MaterialName,
                o => o.MapFrom(s => KitchenToolCatalog.GetMaterialName(s.Material)))
            .ForMember(d => d.Colors,
                o => o.MapFrom(s => KitchenToolCatalog.SelectedColors(s.Colors.Select(x => x.Color))))
            .ForMember(d => d.Images,
                o => o.MapFrom(s => s.Images.OrderBy(i => i.SortOrder)));
    }
}
