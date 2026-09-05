using AutoMapper;
using Domain.Entities;
using Shared.Constants;
using Shared.DTOs.Antiques;

namespace Services.Mapping;

public class PaintingMappingProfile : Profile
{
    public PaintingMappingProfile()
    {
        CreateMap<PaintingImage, PaintingImageDto>()
            .ForMember(d => d.Url, o => o.MapFrom(s => s.ImageUrl));

        CreateMap<PaintingVideo, PaintingVideoDto>()
            .ForMember(d => d.Url, o => o.MapFrom(s => s.VideoUrl));

        CreateMap<PaintingTypeLookup, AntiqueLookupItemDto>();
        CreateMap<PaintingMaterialLookup, AntiqueLookupItemDto>();
        CreateMap<PaintingOriginalityLookup, AntiqueLookupItemDto>();

        CreateMap<Painting, PaintingListItemDto>()
            .ForMember(d => d.PaintingTypeName,
                o => o.MapFrom(s => PaintingCatalog.GetTypeName(s.PaintingType)))
            .ForMember(d => d.MaterialName,
                o => o.MapFrom(s => PaintingCatalog.GetMaterialName(s.Material)))
            .ForMember(d => d.OriginalityName,
                o => o.MapFrom(s => PaintingCatalog.GetOriginalityName(s.Originality)))
            .ForMember(d => d.PrimaryImageUrl, o => o.MapFrom(s =>
                s.Images.OrderByDescending(i => i.IsPrimary).Select(i => i.ImageUrl).FirstOrDefault()));

        CreateMap<Painting, PaintingDetailsDto>()
            .ForMember(d => d.OwnerId, o => o.MapFrom(s => s.UserId))
            .ForMember(d => d.PaintingTypeName,
                o => o.MapFrom(s => PaintingCatalog.GetTypeName(s.PaintingType)))
            .ForMember(d => d.MaterialName,
                o => o.MapFrom(s => PaintingCatalog.GetMaterialName(s.Material)))
            .ForMember(d => d.OriginalityName,
                o => o.MapFrom(s => PaintingCatalog.GetOriginalityName(s.Originality)));

        CreateMap<CreatePaintingRequest, Painting>()
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
