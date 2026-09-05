using AutoMapper;
using Domain.Entities;
using Shared.Constants;
using Shared.DTOs.OnlineShopping;

namespace Services.Mapping;

public class CosmeticMappingProfile : Profile
{
    public CosmeticMappingProfile()
    {
        CreateMap<CosmeticImage, CosmeticImageDto>()
            .ForMember(d => d.Url, o => o.MapFrom(s => s.ImageUrl));

        CreateMap<CosmeticSectionLookup, OnlineShoppingLookupItemDto>();
        CreateMap<CosmeticSuitableForLookup, OnlineShoppingLookupItemDto>();

        CreateMap<Cosmetic, CosmeticListItemDto>()
            .ForMember(d => d.SectionName,
                o => o.MapFrom(s => CosmeticCatalog.GetSectionName(s.Section)))
            .ForMember(d => d.SuitableForName,
                o => o.MapFrom(s => CosmeticCatalog.GetSuitableForName(s.SuitableFor)))
            .ForMember(d => d.PrimaryImageUrl, o => o.MapFrom(s =>
                s.Images.OrderByDescending(i => i.IsPrimary).Select(i => i.ImageUrl).FirstOrDefault()));

        CreateMap<Cosmetic, CosmeticDetailsDto>()
            .ForMember(d => d.OwnerId, o => o.MapFrom(s => s.UserId))
            .ForMember(d => d.SectionName,
                o => o.MapFrom(s => CosmeticCatalog.GetSectionName(s.Section)))
            .ForMember(d => d.SuitableForName,
                o => o.MapFrom(s => CosmeticCatalog.GetSuitableForName(s.SuitableFor)));

        CreateMap<CreateCosmeticRequest, Cosmetic>()
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
