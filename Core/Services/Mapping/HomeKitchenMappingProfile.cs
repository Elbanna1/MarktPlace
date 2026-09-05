using AutoMapper;
using Domain.Entities;
using Shared.Constants;
using Shared.DTOs.OnlineShopping;

namespace Services.Mapping;

public class HomeKitchenMappingProfile : Profile
{
    public HomeKitchenMappingProfile()
    {
        CreateMap<HomeKitchenImage, HomeKitchenImageDto>()
            .ForMember(d => d.Url, o => o.MapFrom(s => s.ImageUrl));

        CreateMap<HomeKitchenSectionLookup, OnlineShoppingLookupItemDto>();
        CreateMap<HomeKitchenMaterialLookup, OnlineShoppingLookupItemDto>();
        CreateMap<HomeKitchenColorLookup, OnlineShoppingLookupItemDto>();

        CreateMap<HomeKitchenColorSelection, OnlineShoppingLookupItemDto>()
            .ForMember(d => d.Id, o => o.MapFrom(s => (int)s.Color))
            .ForMember(d => d.Name, o => o.MapFrom(s => HomeKitchenCatalog.GetColorName(s.Color)))
            .ForMember(d => d.NameEn, o => o.MapFrom(s => HomeKitchenCatalog.GetColorNameEn(s.Color)));

        CreateMap<HomeKitchen, HomeKitchenListItemDto>()
            .ForMember(d => d.SectionName,
                o => o.MapFrom(s => HomeKitchenCatalog.GetSectionName(s.Section)))
            .ForMember(d => d.MaterialName,
                o => o.MapFrom(s => HomeKitchenCatalog.GetMaterialName(s.Material)))
            .ForMember(d => d.Colors,
                o => o.MapFrom(s => HomeKitchenCatalog.SelectedColors(s.Colors.Select(x => x.Color))))
            .ForMember(d => d.PrimaryImageUrl, o => o.MapFrom(s =>
                s.Images.OrderByDescending(i => i.IsPrimary).Select(i => i.ImageUrl).FirstOrDefault()));

        CreateMap<HomeKitchen, HomeKitchenDetailsDto>()
            .ForMember(d => d.OwnerId, o => o.MapFrom(s => s.UserId))
            .ForMember(d => d.SectionName,
                o => o.MapFrom(s => HomeKitchenCatalog.GetSectionName(s.Section)))
            .ForMember(d => d.MaterialName,
                o => o.MapFrom(s => HomeKitchenCatalog.GetMaterialName(s.Material)))
            .ForMember(d => d.Colors,
                o => o.MapFrom(s => HomeKitchenCatalog.SelectedColors(s.Colors.Select(x => x.Color))));

        CreateMap<CreateHomeKitchenRequest, HomeKitchen>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.User, o => o.Ignore())
            .ForMember(d => d.UserId, o => o.Ignore())
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
