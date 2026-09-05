using AutoMapper;
using Domain.Entities;
using Shared.Constants;
using Shared.DTOs.OnlineShopping;

namespace Services.Mapping;

public class ShoppingElectronicMappingProfile : Profile
{
    public ShoppingElectronicMappingProfile()
    {
        CreateMap<ShoppingElectronicImage, ShoppingElectronicImageDto>()
            .ForMember(d => d.Url, o => o.MapFrom(s => s.ImageUrl));

        CreateMap<ShoppingElectronicSectionLookup, OnlineShoppingLookupItemDto>();
        CreateMap<ShoppingElectronicCompatibilityLookup, OnlineShoppingLookupItemDto>();
        CreateMap<ShoppingElectronicConditionLookup, OnlineShoppingLookupItemDto>();
        CreateMap<ShoppingElectronicWarrantyLookup, OnlineShoppingLookupItemDto>();

        CreateMap<ShoppingElectronic, ShoppingElectronicListItemDto>()
            .ForMember(d => d.SectionName,
                o => o.MapFrom(s => ShoppingElectronicCatalog.GetSectionName(s.Section)))
            .ForMember(d => d.CompatibleWithName,
                o => o.MapFrom(s => ShoppingElectronicCatalog.GetCompatibilityName(s.CompatibleWith)))
            .ForMember(d => d.ProductConditionName,
                o => o.MapFrom(s => ShoppingElectronicCatalog.GetConditionName(s.ProductCondition)))
            .ForMember(d => d.WarrantyName,
                o => o.MapFrom(s => ShoppingElectronicCatalog.GetWarrantyName(s.Warranty)))
            .ForMember(d => d.PrimaryImageUrl, o => o.MapFrom(s =>
                s.Images.OrderByDescending(i => i.IsPrimary).Select(i => i.ImageUrl).FirstOrDefault()));

        CreateMap<ShoppingElectronic, ShoppingElectronicDetailsDto>()
            .ForMember(d => d.OwnerId, o => o.MapFrom(s => s.UserId))
            .ForMember(d => d.SectionName,
                o => o.MapFrom(s => ShoppingElectronicCatalog.GetSectionName(s.Section)))
            .ForMember(d => d.CompatibleWithName,
                o => o.MapFrom(s => ShoppingElectronicCatalog.GetCompatibilityName(s.CompatibleWith)))
            .ForMember(d => d.ProductConditionName,
                o => o.MapFrom(s => ShoppingElectronicCatalog.GetConditionName(s.ProductCondition)))
            .ForMember(d => d.WarrantyName,
                o => o.MapFrom(s => ShoppingElectronicCatalog.GetWarrantyName(s.Warranty)));

        CreateMap<CreateShoppingElectronicRequest, ShoppingElectronic>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.User, o => o.Ignore())
            .ForMember(d => d.UserId, o => o.Ignore())
            .ForMember(d => d.VideoPath, o => o.Ignore())
            .ForMember(d => d.VideoUrl, o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.UpdatedAt, o => o.Ignore())
            .ForMember(d => d.IsDeleted, o => o.Ignore())
            .ForMember(d => d.DeletedAt, o => o.Ignore())
            .ForMember(d => d.Images, o => o.Ignore());
    }
}
