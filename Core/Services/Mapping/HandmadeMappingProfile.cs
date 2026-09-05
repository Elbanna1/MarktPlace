using AutoMapper;
using Domain.Entities;
using Shared.Constants;
using Shared.DTOs.Antiques;

namespace Services.Mapping;

public class HandmadeMappingProfile : Profile
{
    public HandmadeMappingProfile()
    {
        CreateMap<HandmadeImage, HandmadeImageDto>()
            .ForMember(d => d.Url, o => o.MapFrom(s => s.ImageUrl));

        CreateMap<HandmadeVideo, HandmadeVideoDto>()
            .ForMember(d => d.Url, o => o.MapFrom(s => s.VideoUrl));

        CreateMap<HandmadeColorSelection, HandmadeColorDto>()
            .ForMember(d => d.Name, o => o.MapFrom(s => HandmadeCatalog.GetColorName(s.Color)));

        CreateMap<HandmadeTypeLookup, AntiqueLookupItemDto>();
        CreateMap<HandmadeColorLookup, AntiqueLookupItemDto>();

        CreateMap<Handmade, HandmadeListItemDto>()
            .ForMember(d => d.HandmadeTypeName,
                o => o.MapFrom(s => HandmadeCatalog.GetTypeName(s.HandmadeType)))
            .ForMember(d => d.PrimaryImageUrl, o => o.MapFrom(s =>
                s.Images.OrderByDescending(i => i.IsPrimary).Select(i => i.ImageUrl).FirstOrDefault()));

        CreateMap<Handmade, HandmadeDetailsDto>()
            .ForMember(d => d.OwnerId, o => o.MapFrom(s => s.UserId))
            .ForMember(d => d.HandmadeTypeName,
                o => o.MapFrom(s => HandmadeCatalog.GetTypeName(s.HandmadeType)));

        CreateMap<CreateHandmadeRequest, Handmade>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.User, o => o.Ignore())
            .ForMember(d => d.UserId, o => o.Ignore())
            .ForMember(d => d.Governorate, o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.UpdatedAt, o => o.Ignore())
            .ForMember(d => d.IsDeleted, o => o.Ignore())
            .ForMember(d => d.DeletedAt, o => o.Ignore())
            .ForMember(d => d.Images, o => o.Ignore())
            .ForMember(d => d.Video, o => o.Ignore())
            .ForMember(d => d.Colors, o => o.Ignore());
    }
}
