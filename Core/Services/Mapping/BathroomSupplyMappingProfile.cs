using AutoMapper;
using Domain.Entities;
using Shared.Constants;
using Shared.DTOs.HomeFurnishing;

namespace Services.Mapping;

public class BathroomSupplyMappingProfile : Profile
{
    public BathroomSupplyMappingProfile()
    {
        CreateMap<BathroomSupplyImage, HomeFurnishingImageDto>()
            .ForMember(d => d.Url, o => o.MapFrom(s => s.ImageUrl));

        CreateMap<BathroomSupplyProductTypeLookup, HomeFurnishingLookupItemDto>();
        CreateMap<BathroomSupplyMaterialLookup, HomeFurnishingLookupItemDto>();
        CreateMap<BathroomSupplyColorLookup, HomeFurnishingLookupItemDto>();

        CreateMap<BathroomSupply, BathroomSupplyListItemDto>()
            .ForMember(d => d.ProductTypeName,
                o => o.MapFrom(s => BathroomSupplyCatalog.GetProductTypeName(s.ProductType)))
            .ForMember(d => d.MaterialName,
                o => o.MapFrom(s => BathroomSupplyCatalog.GetMaterialName(s.Material)))
            .ForMember(d => d.Colors,
                o => o.MapFrom(s => BathroomSupplyCatalog.SelectedColors(s.Colors.Select(x => x.Color))))
            .ForMember(d => d.Images,
                o => o.MapFrom(s => s.Images.OrderBy(i => i.SortOrder)))
            .ForMember(d => d.PrimaryImageUrl, o => o.MapFrom(s =>
                s.Images.OrderBy(i => i.SortOrder).Select(i => i.ImageUrl).FirstOrDefault()));

        CreateMap<BathroomSupply, BathroomSupplyDetailsDto>()
            .ForMember(d => d.OwnerId, o => o.MapFrom(s => s.UserId))
            .ForMember(d => d.ShareUrl, o => o.Ignore())
            .ForMember(d => d.ProductTypeName,
                o => o.MapFrom(s => BathroomSupplyCatalog.GetProductTypeName(s.ProductType)))
            .ForMember(d => d.MaterialName,
                o => o.MapFrom(s => BathroomSupplyCatalog.GetMaterialName(s.Material)))
            .ForMember(d => d.Colors,
                o => o.MapFrom(s => BathroomSupplyCatalog.SelectedColors(s.Colors.Select(x => x.Color))))
            .ForMember(d => d.Images,
                o => o.MapFrom(s => s.Images.OrderBy(i => i.SortOrder)));
    }
}
