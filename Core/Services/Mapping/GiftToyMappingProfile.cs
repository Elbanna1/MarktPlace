using AutoMapper;
using Domain.Entities;
using Shared.Constants;
using Shared.DTOs.OnlineShopping;

namespace Services.Mapping;

public class GiftToyMappingProfile : Profile
{
    public GiftToyMappingProfile()
    {
        CreateMap<GiftToyImage, GiftToyImageDto>()
            .ForMember(d => d.Url, o => o.MapFrom(s => s.ImageUrl));

        CreateMap<GiftToyTypeLookup, OnlineShoppingLookupItemDto>();
        CreateMap<GiftToySuitableForLookup, OnlineShoppingLookupItemDto>();

        CreateMap<GiftToy, GiftToyListItemDto>()
            .ForMember(d => d.GiftTypeName,
                o => o.MapFrom(s => GiftToyCatalog.GetTypeName(s.GiftType)))
            .ForMember(d => d.SuitableForName,
                o => o.MapFrom(s => GiftToyCatalog.GetSuitableForName(s.SuitableFor)))
            .ForMember(d => d.PrimaryImageUrl, o => o.MapFrom(s =>
                s.Images.OrderByDescending(i => i.IsPrimary).Select(i => i.ImageUrl).FirstOrDefault()));

        CreateMap<GiftToy, GiftToyDetailsDto>()
            .ForMember(d => d.OwnerId, o => o.MapFrom(s => s.UserId))
            .ForMember(d => d.GiftTypeName,
                o => o.MapFrom(s => GiftToyCatalog.GetTypeName(s.GiftType)))
            .ForMember(d => d.SuitableForName,
                o => o.MapFrom(s => GiftToyCatalog.GetSuitableForName(s.SuitableFor)));

        CreateMap<CreateGiftToyRequest, GiftToy>()
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
