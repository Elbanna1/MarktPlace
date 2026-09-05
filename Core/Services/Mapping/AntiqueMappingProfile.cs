using AutoMapper;
using Domain.Entities;
using Shared.Constants;
using Shared.DTOs.Antiques;

namespace Services.Mapping;

public class AntiqueMappingProfile : Profile
{
    public AntiqueMappingProfile()
    {
        CreateMap<AntiqueImage, AntiqueImageDto>()
            .ForMember(d => d.Url, o => o.MapFrom(s => s.ImageUrl));

        CreateMap<AntiqueVideo, AntiqueVideoDto>()
            .ForMember(d => d.Url, o => o.MapFrom(s => s.VideoUrl));

        CreateMap<AntiqueTypeLookup, AntiqueLookupItemDto>();
        CreateMap<AntiqueMaterialLookup, AntiqueLookupItemDto>();
        CreateMap<AntiqueConditionLookup, AntiqueLookupItemDto>();
        CreateMap<AntiqueWorkingStatusLookup, AntiqueLookupItemDto>();
        CreateMap<AntiqueOriginalityLookup, AntiqueLookupItemDto>();

        CreateMap<Antique, AntiqueListItemDto>()
            .ForMember(d => d.AntiqueTypeName,
                o => o.MapFrom(s => AntiqueModuleCatalog.GetTypeName(s.AntiqueType)))
            .ForMember(d => d.MaterialName,
                o => o.MapFrom(s => AntiqueModuleCatalog.GetMaterialName(s.Material)))
            .ForMember(d => d.ConditionName,
                o => o.MapFrom(s => AntiqueModuleCatalog.GetConditionName(s.Condition)))
            .ForMember(d => d.WorkingStatusName,
                o => o.MapFrom(s => AntiqueModuleCatalog.GetWorkingStatusName(s.WorkingStatus)))
            .ForMember(d => d.OriginalityName,
                o => o.MapFrom(s => AntiqueModuleCatalog.GetOriginalityName(s.Originality)))
            .ForMember(d => d.PrimaryImageUrl, o => o.MapFrom(s =>
                s.Images.OrderByDescending(i => i.IsPrimary).Select(i => i.ImageUrl).FirstOrDefault()));

        CreateMap<Antique, AntiqueDetailsDto>()
            .ForMember(d => d.OwnerId, o => o.MapFrom(s => s.UserId))
            .ForMember(d => d.AntiqueTypeName,
                o => o.MapFrom(s => AntiqueModuleCatalog.GetTypeName(s.AntiqueType)))
            .ForMember(d => d.MaterialName,
                o => o.MapFrom(s => AntiqueModuleCatalog.GetMaterialName(s.Material)))
            .ForMember(d => d.ConditionName,
                o => o.MapFrom(s => AntiqueModuleCatalog.GetConditionName(s.Condition)))
            .ForMember(d => d.WorkingStatusName,
                o => o.MapFrom(s => AntiqueModuleCatalog.GetWorkingStatusName(s.WorkingStatus)))
            .ForMember(d => d.OriginalityName,
                o => o.MapFrom(s => AntiqueModuleCatalog.GetOriginalityName(s.Originality)));

        CreateMap<CreateAntiqueRequest, Antique>()
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
