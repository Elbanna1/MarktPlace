using AutoMapper;
using Domain.Entities;
using Shared.Constants;
using Shared.DTOs.OnlineShopping;

namespace Services.Mapping;

public class AccessoryMappingProfile : Profile
{
    public AccessoryMappingProfile()
    {
        CreateMap<AccessoryImage, AccessoryImageDto>()
            .ForMember(d => d.Url, o => o.MapFrom(s => s.ImageUrl));

        CreateMap<AccessoryTypeLookup, OnlineShoppingLookupItemDto>();
        CreateMap<AccessoryCategoryLookup, OnlineShoppingLookupItemDto>();
        CreateMap<AccessoryMaterialLookup, OnlineShoppingLookupItemDto>();
        CreateMap<AccessoryColorLookup, OnlineShoppingLookupItemDto>();

        CreateMap<AccessoryColorSelection, OnlineShoppingLookupItemDto>()
            .ForMember(d => d.Id, o => o.MapFrom(s => (int)s.Color))
            .ForMember(d => d.Name, o => o.MapFrom(s => AccessoryCatalog.GetColorName(s.Color)))
            .ForMember(d => d.NameEn, o => o.MapFrom(s => AccessoryCatalog.GetColorNameEn(s.Color)));

        CreateMap<Accessory, AccessoryListItemDto>()
            .ForMember(d => d.AccessoryTypeName,
                o => o.MapFrom(s => AccessoryCatalog.GetAccessoryTypeName(s.AccessoryType)))
            .ForMember(d => d.CategoryName,
                o => o.MapFrom(s => AccessoryCatalog.GetCategoryName(s.Category)))
            .ForMember(d => d.MaterialName,
                o => o.MapFrom(s => AccessoryCatalog.GetMaterialName(s.Material)))
            .ForMember(d => d.Colors,
                o => o.MapFrom(s => AccessoryCatalog.SelectedColors(s.Colors.Select(x => x.Color))))
            .ForMember(d => d.PrimaryImageUrl, o => o.MapFrom(s =>
                s.Images.OrderByDescending(i => i.IsPrimary).Select(i => i.ImageUrl).FirstOrDefault()));

        CreateMap<Accessory, AccessoryDetailsDto>()
            .ForMember(d => d.OwnerId, o => o.MapFrom(s => s.UserId))
            .ForMember(d => d.AccessoryTypeName,
                o => o.MapFrom(s => AccessoryCatalog.GetAccessoryTypeName(s.AccessoryType)))
            .ForMember(d => d.CategoryName,
                o => o.MapFrom(s => AccessoryCatalog.GetCategoryName(s.Category)))
            .ForMember(d => d.MaterialName,
                o => o.MapFrom(s => AccessoryCatalog.GetMaterialName(s.Material)))
            .ForMember(d => d.Colors,
                o => o.MapFrom(s => AccessoryCatalog.SelectedColors(s.Colors.Select(x => x.Color))));

        CreateMap<CreateAccessoryRequest, Accessory>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.User, o => o.Ignore())
            .ForMember(d => d.UserId, o => o.Ignore())
            .ForMember(d => d.LogoPath, o => o.Ignore())
            .ForMember(d => d.LogoUrl, o => o.Ignore())
            .ForMember(d => d.VideoPath, o => o.Ignore())
            .ForMember(d => d.VideoUrl, o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.UpdatedAt, o => o.Ignore())
            .ForMember(d => d.IsDeleted, o => o.Ignore())
            .ForMember(d => d.DeletedAt, o => o.Ignore())
            .ForMember(d => d.Images, o => o.Ignore())
            .ForMember(d => d.Colors, o => o.Ignore());
    }
}
