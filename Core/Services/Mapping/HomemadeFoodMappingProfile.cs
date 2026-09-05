using AutoMapper;
using Domain.Entities;
using Shared.Constants;
using Shared.DTOs.OnlineShopping;

namespace Services.Mapping;

public class HomemadeFoodMappingProfile : Profile
{
    public HomemadeFoodMappingProfile()
    {
        CreateMap<HomemadeFoodImage, HomemadeFoodImageDto>()
            .ForMember(d => d.Url, o => o.MapFrom(s => s.ImageUrl));

        CreateMap<HomemadeFoodSectionLookup, OnlineShoppingLookupItemDto>();
        CreateMap<HomemadeFoodDeliveryAreaLookup, OnlineShoppingLookupItemDto>();

        CreateMap<HomemadeFoodDeliveryAreaSelection, OnlineShoppingLookupItemDto>()
            .ForMember(d => d.Id, o => o.MapFrom(s => (int)s.DeliveryArea))
            .ForMember(d => d.Name, o => o.MapFrom(s => HomemadeFoodCatalog.GetDeliveryAreaName(s.DeliveryArea)))
            .ForMember(d => d.NameEn, o => o.MapFrom(s => HomemadeFoodCatalog.GetDeliveryAreaNameEn(s.DeliveryArea)));

        CreateMap<HomemadeFood, HomemadeFoodListItemDto>()
            .ForMember(d => d.SectionName,
                o => o.MapFrom(s => HomemadeFoodCatalog.GetSectionName(s.Section)))
            .ForMember(d => d.DeliveryAreas, o => o.MapFrom(s =>
                HomemadeFoodCatalog.SelectedDeliveryAreas(s.DeliveryAreas.Select(x => x.DeliveryArea))))
            .ForMember(d => d.PrimaryImageUrl, o => o.MapFrom(s =>
                s.Images.OrderByDescending(i => i.IsPrimary).Select(i => i.ImageUrl).FirstOrDefault()));

        CreateMap<HomemadeFood, HomemadeFoodDetailsDto>()
            .ForMember(d => d.OwnerId, o => o.MapFrom(s => s.UserId))
            .ForMember(d => d.SectionName,
                o => o.MapFrom(s => HomemadeFoodCatalog.GetSectionName(s.Section)))
            .ForMember(d => d.DeliveryAreas, o => o.MapFrom(s =>
                HomemadeFoodCatalog.SelectedDeliveryAreas(s.DeliveryAreas.Select(x => x.DeliveryArea))));

        CreateMap<CreateHomemadeFoodRequest, HomemadeFood>()
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
            .ForMember(d => d.DeliveryAreas, o => o.Ignore());
    }
}
