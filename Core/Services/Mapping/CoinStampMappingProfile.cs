using AutoMapper;
using Domain.Entities;
using Shared.Constants;
using Shared.DTOs.Antiques;

namespace Services.Mapping;

public class CoinStampMappingProfile : Profile
{
    public CoinStampMappingProfile()
    {
        CreateMap<CoinStampImage, CoinStampImageDto>()
            .ForMember(d => d.Url, o => o.MapFrom(s => s.ImageUrl));

        CreateMap<CoinStampVideo, CoinStampVideoDto>()
            .ForMember(d => d.Url, o => o.MapFrom(s => s.VideoUrl));

        CreateMap<CoinStampItemTypeLookup, AntiqueLookupItemDto>();
        CreateMap<CoinStampMetalLookup, AntiqueLookupItemDto>();
        CreateMap<CoinStampConditionLookup, AntiqueLookupItemDto>();

        CreateMap<CoinStamp, CoinStampListItemDto>()
            .ForMember(d => d.ItemTypeName,
                o => o.MapFrom(s => CoinStampCatalog.GetItemTypeName(s.ItemType)))
            .ForMember(d => d.MetalName,
                o => o.MapFrom(s => CoinStampCatalog.GetMetalName(s.Metal)))
            .ForMember(d => d.ConditionName,
                o => o.MapFrom(s => CoinStampCatalog.GetConditionName(s.Condition)))
            .ForMember(d => d.PrimaryImageUrl, o => o.MapFrom(s =>
                s.Images.OrderByDescending(i => i.IsPrimary).Select(i => i.ImageUrl).FirstOrDefault()));

        CreateMap<CoinStamp, CoinStampDetailsDto>()
            .ForMember(d => d.OwnerId, o => o.MapFrom(s => s.UserId))
            .ForMember(d => d.ItemTypeName,
                o => o.MapFrom(s => CoinStampCatalog.GetItemTypeName(s.ItemType)))
            .ForMember(d => d.MetalName,
                o => o.MapFrom(s => CoinStampCatalog.GetMetalName(s.Metal)))
            .ForMember(d => d.ConditionName,
                o => o.MapFrom(s => CoinStampCatalog.GetConditionName(s.Condition)));

        CreateMap<CreateCoinStampRequest, CoinStamp>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.User, o => o.Ignore())
            .ForMember(d => d.UserId, o => o.Ignore())
            .ForMember(d => d.Governorate, o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.UpdatedAt, o => o.Ignore())
            .ForMember(d => d.IsDeleted, o => o.Ignore())
            .ForMember(d => d.DeletedAt, o => o.Ignore())
            .ForMember(d => d.Images, o => o.Ignore())
            .ForMember(d => d.Video, o => o.Ignore());
    }
}
