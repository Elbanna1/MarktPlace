using AutoMapper;
using Domain.Entities;
using Shared.Constants;
using Shared.DTOs.Clothing;

namespace Services.Mapping;

public class WomenClothingMappingProfile : Profile
{
    public WomenClothingMappingProfile()
    {
        CreateMap<WomenClothingImage, WomenClothingImageDto>()
            .ForMember(d => d.Url, o => o.MapFrom(s => s.ImageUrl));

        CreateMap<WomenClothingTypeLookup, ClothingLookupItemDto>();
        CreateMap<WomenClothingBrandLookup, ClothingLookupItemDto>();
        CreateMap<WomenClothingSizeLookup, ClothingLookupItemDto>();
        CreateMap<WomenClothingColorLookup, ClothingLookupItemDto>();
        CreateMap<WomenClothingConditionLookup, ClothingLookupItemDto>();
        CreateMap<WomenClothingSellingMethodLookup, ClothingLookupItemDto>();

        CreateMap<WomenClothingSizeSelection, ClothingLookupItemDto>()
            .ForMember(d => d.Id, o => o.MapFrom(s => (int)s.Size))
            .ForMember(d => d.Name, o => o.MapFrom(s => WomenClothingCatalog.GetSizeName(s.Size)))
            .ForMember(d => d.NameEn, o => o.MapFrom(s => WomenClothingCatalog.GetSizeNameEn(s.Size)));

        CreateMap<WomenClothingColorSelection, ClothingLookupItemDto>()
            .ForMember(d => d.Id, o => o.MapFrom(s => (int)s.Color))
            .ForMember(d => d.Name, o => o.MapFrom(s => WomenClothingCatalog.GetColorName(s.Color)))
            .ForMember(d => d.NameEn, o => o.MapFrom(s => WomenClothingCatalog.GetColorNameEn(s.Color)));

        CreateMap<WomenClothing, WomenClothingListItemDto>()
            .ForMember(d => d.SellingMethodName,
                o => o.MapFrom(s => WomenClothingCatalog.GetSellingMethodName(s.SellingMethod)))
            .ForMember(d => d.ClothingTypeName,
                o => o.MapFrom(s => WomenClothingCatalog.GetClothingTypeName(s.ClothingType)))
            .ForMember(d => d.BrandName,
                o => o.MapFrom(s => WomenClothingCatalog.GetBrandName(s.Brand)))
            .ForMember(d => d.ConditionName,
                o => o.MapFrom(s => WomenClothingCatalog.GetConditionName(s.Condition)))
            .ForMember(d => d.Sizes,
                o => o.MapFrom(s => WomenClothingCatalog.SelectedSizes(s.Sizes.Select(x => x.Size))))
            .ForMember(d => d.Colors,
                o => o.MapFrom(s => WomenClothingCatalog.SelectedColors(s.Colors.Select(x => x.Color))))
            .ForMember(d => d.PrimaryImageUrl, o => o.MapFrom(s =>
                s.Images.OrderByDescending(i => i.IsPrimary).Select(i => i.ImageUrl).FirstOrDefault()));

        CreateMap<WomenClothing, WomenClothingDetailsDto>()
            .ForMember(d => d.OwnerId, o => o.MapFrom(s => s.UserId))
            .ForMember(d => d.SellingMethodName,
                o => o.MapFrom(s => WomenClothingCatalog.GetSellingMethodName(s.SellingMethod)))
            .ForMember(d => d.ClothingTypeName,
                o => o.MapFrom(s => WomenClothingCatalog.GetClothingTypeName(s.ClothingType)))
            .ForMember(d => d.BrandName,
                o => o.MapFrom(s => WomenClothingCatalog.GetBrandName(s.Brand)))
            .ForMember(d => d.ConditionName,
                o => o.MapFrom(s => WomenClothingCatalog.GetConditionName(s.Condition)))
            .ForMember(d => d.Sizes,
                o => o.MapFrom(s => WomenClothingCatalog.SelectedSizes(s.Sizes.Select(x => x.Size))))
            .ForMember(d => d.Colors,
                o => o.MapFrom(s => WomenClothingCatalog.SelectedColors(s.Colors.Select(x => x.Color))));

        CreateMap<CreateWomenClothingRequest, WomenClothing>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.User, o => o.Ignore())
            .ForMember(d => d.UserId, o => o.Ignore())
            .ForMember(d => d.Governorate, o => o.Ignore())
            .ForMember(d => d.VideoPath, o => o.Ignore())
            .ForMember(d => d.VideoUrl, o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.UpdatedAt, o => o.Ignore())
            .ForMember(d => d.IsDeleted, o => o.Ignore())
            .ForMember(d => d.DeletedAt, o => o.Ignore())
            .ForMember(d => d.Images, o => o.Ignore())
            .ForMember(d => d.Sizes, o => o.Ignore())
            .ForMember(d => d.Colors, o => o.Ignore());
    }
}
