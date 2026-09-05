using AutoMapper;
using Domain.Entities;
using Shared.Constants;
using Shared.DTOs.HomeFurnishing;

namespace Services.Mapping;

public class FurnishingCurtainMappingProfile : Profile
{
    public FurnishingCurtainMappingProfile()
    {
        CreateMap<FurnishingCurtainImage, HomeFurnishingImageDto>()
            .ForMember(d => d.Url, o => o.MapFrom(s => s.ImageUrl));

        CreateMap<FurnishingCurtainProductTypeLookup, HomeFurnishingLookupItemDto>();
        CreateMap<FurnishingCurtainSizeLookup, HomeFurnishingLookupItemDto>();
        CreateMap<FurnishingCurtainMaterialLookup, HomeFurnishingLookupItemDto>();
        CreateMap<FurnishingCurtainColorLookup, HomeFurnishingLookupItemDto>();

        CreateMap<FurnishingCurtain, FurnishingCurtainListItemDto>()
            .ForMember(d => d.ProductTypeName,
                o => o.MapFrom(s => FurnishingCurtainCatalog.GetProductTypeName(s.ProductType)))
            .ForMember(d => d.SizeName,
                o => o.MapFrom(s => FurnishingCurtainCatalog.GetSizeName(s.Size)))
            .ForMember(d => d.MaterialName,
                o => o.MapFrom(s => FurnishingCurtainCatalog.GetMaterialName(s.Material)))
            .ForMember(d => d.Colors,
                o => o.MapFrom(s => FurnishingCurtainCatalog.SelectedColors(s.Colors.Select(x => x.Color))))
            .ForMember(d => d.Images,
                o => o.MapFrom(s => s.Images.OrderBy(i => i.SortOrder)))
            .ForMember(d => d.PrimaryImageUrl, o => o.MapFrom(s =>
                s.Images.OrderBy(i => i.SortOrder).Select(i => i.ImageUrl).FirstOrDefault()));

        CreateMap<FurnishingCurtain, FurnishingCurtainDetailsDto>()
            .ForMember(d => d.OwnerId, o => o.MapFrom(s => s.UserId))
            .ForMember(d => d.ShareUrl, o => o.Ignore())
            .ForMember(d => d.ProductTypeName,
                o => o.MapFrom(s => FurnishingCurtainCatalog.GetProductTypeName(s.ProductType)))
            .ForMember(d => d.SizeName,
                o => o.MapFrom(s => FurnishingCurtainCatalog.GetSizeName(s.Size)))
            .ForMember(d => d.MaterialName,
                o => o.MapFrom(s => FurnishingCurtainCatalog.GetMaterialName(s.Material)))
            .ForMember(d => d.Colors,
                o => o.MapFrom(s => FurnishingCurtainCatalog.SelectedColors(s.Colors.Select(x => x.Color))))
            .ForMember(d => d.Images,
                o => o.MapFrom(s => s.Images.OrderBy(i => i.SortOrder)));
    }
}
