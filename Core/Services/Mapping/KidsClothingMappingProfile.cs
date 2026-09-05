using AutoMapper;
using Domain.Entities;
using Shared.Constants;
using Shared.DTOs.Clothing;

namespace Services.Mapping;

public class KidsClothingMappingProfile : Profile
{
    public KidsClothingMappingProfile()
    {
        CreateMap<KidsClothingImage, KidsClothingImageDto>()
            .ForMember(d => d.Url, o => o.MapFrom(s => s.ImageUrl));

        CreateMap<KidsClothingTypeLookup, ClothingLookupItemDto>();
        CreateMap<KidsClothingBrandLookup, ClothingLookupItemDto>();
        CreateMap<KidsClothingSizeLookup, ClothingLookupItemDto>();
        CreateMap<KidsClothingColorLookup, ClothingLookupItemDto>();
        CreateMap<KidsClothingConditionLookup, ClothingLookupItemDto>();
        CreateMap<KidsClothingSellingMethodLookup, ClothingLookupItemDto>();

        CreateMap<KidsClothingSizeSelection, ClothingLookupItemDto>()
            .ForMember(d => d.Id, o => o.MapFrom(s => (int)s.Size))
            .ForMember(d => d.Name, o => o.MapFrom(s => KidsClothingCatalog.GetSizeName(s.Size)))
            .ForMember(d => d.NameEn, o => o.MapFrom(s => KidsClothingCatalog.GetSizeNameEn(s.Size)));

        CreateMap<KidsClothingColorSelection, ClothingLookupItemDto>()
            .ForMember(d => d.Id, o => o.MapFrom(s => (int)s.Color))
            .ForMember(d => d.Name, o => o.MapFrom(s => KidsClothingCatalog.GetColorName(s.Color)))
            .ForMember(d => d.NameEn, o => o.MapFrom(s => KidsClothingCatalog.GetColorNameEn(s.Color)));

        CreateMap<KidsClothing, KidsClothingListItemDto>()
            .ForMember(d => d.SellingMethodName,
                o => o.MapFrom(s => KidsClothingCatalog.GetSellingMethodName(s.SellingMethod)))
            .ForMember(d => d.ClothingTypeName,
                o => o.MapFrom(s => KidsClothingCatalog.GetClothingTypeName(s.ClothingType)))
            .ForMember(d => d.BrandName,
                o => o.MapFrom(s => KidsClothingCatalog.GetBrandName(s.Brand)))
            .ForMember(d => d.ConditionName,
                o => o.MapFrom(s => KidsClothingCatalog.GetConditionName(s.Condition)))
            .ForMember(d => d.Sizes,
                o => o.MapFrom(s => KidsClothingCatalog.SelectedSizes(s.Sizes.Select(x => x.Size))))
            .ForMember(d => d.Colors,
                o => o.MapFrom(s => KidsClothingCatalog.SelectedColors(s.Colors.Select(x => x.Color))))
            .ForMember(d => d.PrimaryImageUrl, o => o.MapFrom(s =>
                s.Images.OrderByDescending(i => i.IsPrimary).Select(i => i.ImageUrl).FirstOrDefault()));

        CreateMap<KidsClothing, KidsClothingDetailsDto>()
            .ForMember(d => d.OwnerId, o => o.MapFrom(s => s.UserId))
            .ForMember(d => d.SellingMethodName,
                o => o.MapFrom(s => KidsClothingCatalog.GetSellingMethodName(s.SellingMethod)))
            .ForMember(d => d.ClothingTypeName,
                o => o.MapFrom(s => KidsClothingCatalog.GetClothingTypeName(s.ClothingType)))
            .ForMember(d => d.BrandName,
                o => o.MapFrom(s => KidsClothingCatalog.GetBrandName(s.Brand)))
            .ForMember(d => d.ConditionName,
                o => o.MapFrom(s => KidsClothingCatalog.GetConditionName(s.Condition)))
            .ForMember(d => d.Sizes,
                o => o.MapFrom(s => KidsClothingCatalog.SelectedSizes(s.Sizes.Select(x => x.Size))))
            .ForMember(d => d.Colors,
                o => o.MapFrom(s => KidsClothingCatalog.SelectedColors(s.Colors.Select(x => x.Color))));

        CreateMap<CreateKidsClothingRequest, KidsClothing>()
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
