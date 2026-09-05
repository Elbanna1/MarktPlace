using AutoMapper;
using Domain.Entities;
using Shared.Constants;
using Shared.DTOs.Clothing;

namespace Services.Mapping;

public class MenClothingMappingProfile : Profile
{
    public MenClothingMappingProfile()
    {
        CreateMap<MenClothingImage, MenClothingImageDto>()
            .ForMember(d => d.Url, o => o.MapFrom(s => s.ImageUrl));

        CreateMap<MenClothingTypeLookup, ClothingLookupItemDto>();
        CreateMap<MenClothingBrandLookup, ClothingLookupItemDto>();
        CreateMap<MenClothingSizeLookup, ClothingLookupItemDto>();
        CreateMap<MenClothingColorLookup, ClothingLookupItemDto>();
        CreateMap<MenClothingConditionLookup, ClothingLookupItemDto>();
        CreateMap<MenClothingSellingMethodLookup, ClothingLookupItemDto>();

        CreateMap<MenClothingSizeSelection, ClothingLookupItemDto>()
            .ForMember(d => d.Id, o => o.MapFrom(s => (int)s.Size))
            .ForMember(d => d.Name, o => o.MapFrom(s => MenClothingCatalog.GetSizeName(s.Size)))
            .ForMember(d => d.NameEn, o => o.MapFrom(s => MenClothingCatalog.GetSizeNameEn(s.Size)));

        CreateMap<MenClothingColorSelection, ClothingLookupItemDto>()
            .ForMember(d => d.Id, o => o.MapFrom(s => (int)s.Color))
            .ForMember(d => d.Name, o => o.MapFrom(s => MenClothingCatalog.GetColorName(s.Color)))
            .ForMember(d => d.NameEn, o => o.MapFrom(s => MenClothingCatalog.GetColorNameEn(s.Color)));

        CreateMap<MenClothing, MenClothingListItemDto>()
            .ForMember(d => d.SellingMethodName,
                o => o.MapFrom(s => MenClothingCatalog.GetSellingMethodName(s.SellingMethod)))
            .ForMember(d => d.ClothingTypeName,
                o => o.MapFrom(s => MenClothingCatalog.GetClothingTypeName(s.ClothingType)))
            .ForMember(d => d.BrandName,
                o => o.MapFrom(s => MenClothingCatalog.GetBrandName(s.Brand)))
            .ForMember(d => d.ConditionName,
                o => o.MapFrom(s => MenClothingCatalog.GetConditionName(s.Condition)))
            .ForMember(d => d.Sizes,
                o => o.MapFrom(s => MenClothingCatalog.SelectedSizes(s.Sizes.Select(x => x.Size))))
            .ForMember(d => d.Colors,
                o => o.MapFrom(s => MenClothingCatalog.SelectedColors(s.Colors.Select(x => x.Color))))
            .ForMember(d => d.PrimaryImageUrl, o => o.MapFrom(s =>
                s.Images.OrderByDescending(i => i.IsPrimary).Select(i => i.ImageUrl).FirstOrDefault()));

        CreateMap<MenClothing, MenClothingDetailsDto>()
            .ForMember(d => d.OwnerId, o => o.MapFrom(s => s.UserId))
            .ForMember(d => d.SellingMethodName,
                o => o.MapFrom(s => MenClothingCatalog.GetSellingMethodName(s.SellingMethod)))
            .ForMember(d => d.ClothingTypeName,
                o => o.MapFrom(s => MenClothingCatalog.GetClothingTypeName(s.ClothingType)))
            .ForMember(d => d.BrandName,
                o => o.MapFrom(s => MenClothingCatalog.GetBrandName(s.Brand)))
            .ForMember(d => d.ConditionName,
                o => o.MapFrom(s => MenClothingCatalog.GetConditionName(s.Condition)))
            .ForMember(d => d.Sizes,
                o => o.MapFrom(s => MenClothingCatalog.SelectedSizes(s.Sizes.Select(x => x.Size))))
            .ForMember(d => d.Colors,
                o => o.MapFrom(s => MenClothingCatalog.SelectedColors(s.Colors.Select(x => x.Color))));

        CreateMap<CreateMenClothingRequest, MenClothing>()
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
