using AutoMapper;
using Domain.Entities;
using Shared.Constants;
using Shared.DTOs.WholesaleTraders;

namespace Services.Mapping;

public class WholesaleTraderMappingProfile : Profile
{
    public WholesaleTraderMappingProfile()
    {
        CreateMap<WholesaleTraderImage, WholesaleTraderImageDto>()
            .ForMember(d => d.Url, o => o.MapFrom(s => s.ImageUrl));

        CreateMap<WholesaleTradeTypeLookup, WholesaleTradeTypeDto>();

        CreateMap<WholesaleTrader, WholesaleTraderListItemDto>()
            .ForMember(d => d.TradeTypeName,
                o => o.MapFrom(s => WholesaleTraderCatalog.GetTradeTypeName(s.TradeType)))
            .ForMember(d => d.SaleTypeName,
                o => o.MapFrom(s => WholesaleTraderCatalog.GetSaleTypeName(s.SaleType)))
            .ForMember(d => d.PrimaryImageUrl, o => o.MapFrom(s =>
                s.Images.OrderByDescending(i => i.IsPrimary).Select(i => i.ImageUrl).FirstOrDefault()));

        CreateMap<WholesaleTrader, WholesaleTraderDetailsDto>()
            .ForMember(d => d.OwnerId, o => o.MapFrom(s => s.UserId))
            .ForMember(d => d.TradeTypeName,
                o => o.MapFrom(s => WholesaleTraderCatalog.GetTradeTypeName(s.TradeType)))
            .ForMember(d => d.SaleTypeName,
                o => o.MapFrom(s => WholesaleTraderCatalog.GetSaleTypeName(s.SaleType)));

        CreateMap<CreateWholesaleTraderRequest, WholesaleTrader>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.User, o => o.Ignore())
            .ForMember(d => d.UserId, o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.UpdatedAt, o => o.Ignore())
            .ForMember(d => d.IsDeleted, o => o.Ignore())
            .ForMember(d => d.DeletedAt, o => o.Ignore())
            .ForMember(d => d.Images, o => o.Ignore());
    }
}
