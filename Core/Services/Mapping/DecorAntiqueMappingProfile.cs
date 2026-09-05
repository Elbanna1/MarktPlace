using AutoMapper;
using Domain.Entities;
using Shared.Constants;
using Shared.DTOs.Antiques;

namespace Services.Mapping;

public class DecorAntiqueMappingProfile : Profile
{
    public DecorAntiqueMappingProfile()
    {
        CreateMap<DecorAntiqueImage, DecorAntiqueImageDto>()
            .ForMember(d => d.Url, o => o.MapFrom(s => s.ImageUrl));

        CreateMap<DecorAntiqueVideo, DecorAntiqueVideoDto>()
            .ForMember(d => d.Url, o => o.MapFrom(s => s.VideoUrl));

        CreateMap<DecorAntiqueItemTypeLookup, AntiqueLookupItemDto>();
        CreateMap<DecorAntiqueMaterialLookup, AntiqueLookupItemDto>();
        CreateMap<DecorAntiqueConditionLookup, AntiqueLookupItemDto>();
        CreateMap<DecorAntiqueOriginalityLookup, AntiqueLookupItemDto>();

        CreateMap<DecorAntique, DecorAntiqueListItemDto>()
            .ForMember(d => d.ItemTypeName,
                o => o.MapFrom(s => DecorAntiqueCatalog.GetItemTypeName(s.ItemType)))
            .ForMember(d => d.MaterialName,
                o => o.MapFrom(s => DecorAntiqueCatalog.GetMaterialName(s.Material)))
            .ForMember(d => d.ConditionName,
                o => o.MapFrom(s => DecorAntiqueCatalog.GetConditionName(s.Condition)))
            .ForMember(d => d.OriginalityName,
                o => o.MapFrom(s => DecorAntiqueCatalog.GetOriginalityName(s.Originality)))
            .ForMember(d => d.PrimaryImageUrl, o => o.MapFrom(s =>
                s.Images.OrderByDescending(i => i.IsPrimary).Select(i => i.ImageUrl).FirstOrDefault()));

        CreateMap<DecorAntique, DecorAntiqueDetailsDto>()
            .ForMember(d => d.OwnerId, o => o.MapFrom(s => s.UserId))
            .ForMember(d => d.ItemTypeName,
                o => o.MapFrom(s => DecorAntiqueCatalog.GetItemTypeName(s.ItemType)))
            .ForMember(d => d.MaterialName,
                o => o.MapFrom(s => DecorAntiqueCatalog.GetMaterialName(s.Material)))
            .ForMember(d => d.ConditionName,
                o => o.MapFrom(s => DecorAntiqueCatalog.GetConditionName(s.Condition)))
            .ForMember(d => d.OriginalityName,
                o => o.MapFrom(s => DecorAntiqueCatalog.GetOriginalityName(s.Originality)));

        CreateMap<CreateDecorAntiqueRequest, DecorAntique>()
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
