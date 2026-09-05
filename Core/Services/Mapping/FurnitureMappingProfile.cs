using AutoMapper;
using Domain.Entities;
using Shared.Constants;
using Shared.DTOs.HomeFurnishing;

namespace Services.Mapping;

public class FurnitureMappingProfile : Profile
{
    public FurnitureMappingProfile()
    {
        CreateMap<FurnitureImage, HomeFurnishingImageDto>()
            .ForMember(d => d.Url, o => o.MapFrom(s => s.ImageUrl));

        CreateMap<FurnitureTypeLookup, HomeFurnishingLookupItemDto>();
        CreateMap<FurnitureMaterialLookup, HomeFurnishingLookupItemDto>();
        CreateMap<FurnitureColorLookup, HomeFurnishingLookupItemDto>();
        CreateMap<FurnitureConditionLookup, HomeFurnishingLookupItemDto>();

        CreateMap<Furniture, FurnitureListItemDto>()
            .ForMember(d => d.FurnitureTypeName,
                o => o.MapFrom(s => FurnitureCatalog.GetFurnitureTypeName(s.FurnitureType)))
            .ForMember(d => d.MaterialName,
                o => o.MapFrom(s => FurnitureCatalog.GetMaterialName(s.Material)))
            .ForMember(d => d.ConditionName,
                o => o.MapFrom(s => FurnitureCatalog.GetConditionName(s.Condition)))
            .ForMember(d => d.Colors,
                o => o.MapFrom(s => FurnitureCatalog.SelectedColors(s.Colors.Select(x => x.Color))))
            .ForMember(d => d.Images,
                o => o.MapFrom(s => s.Images.OrderBy(i => i.SortOrder)))
            .ForMember(d => d.PrimaryImageUrl, o => o.MapFrom(s =>
                s.Images.OrderBy(i => i.SortOrder).Select(i => i.ImageUrl).FirstOrDefault()));

        CreateMap<Furniture, FurnitureDetailsDto>()
            .ForMember(d => d.OwnerId, o => o.MapFrom(s => s.UserId))
            .ForMember(d => d.ShareUrl, o => o.Ignore())
            .ForMember(d => d.FurnitureTypeName,
                o => o.MapFrom(s => FurnitureCatalog.GetFurnitureTypeName(s.FurnitureType)))
            .ForMember(d => d.MaterialName,
                o => o.MapFrom(s => FurnitureCatalog.GetMaterialName(s.Material)))
            .ForMember(d => d.ConditionName,
                o => o.MapFrom(s => FurnitureCatalog.GetConditionName(s.Condition)))
            .ForMember(d => d.Colors,
                o => o.MapFrom(s => FurnitureCatalog.SelectedColors(s.Colors.Select(x => x.Color))))
            .ForMember(d => d.Images,
                o => o.MapFrom(s => s.Images.OrderBy(i => i.SortOrder)));
    }
}
