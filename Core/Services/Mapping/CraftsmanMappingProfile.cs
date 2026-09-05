using AutoMapper;
using Domain.Entities;
using Shared.Constants;
using Shared.DTOs.Craftsmen;

namespace Services.Mapping;

public class CraftsmanMappingProfile : Profile
{
    public CraftsmanMappingProfile()
    {
        CreateMap<CraftsmanImage, CraftsmanImageDto>()
            .ForMember(d => d.Url, o => o.MapFrom(s => s.ImageUrl));

        CreateMap<Craftsman, CraftsmanListItemDto>()
            .ForMember(d => d.SpecializationName,
                o => o.MapFrom(s => WorkshopCraftsmenCatalog.GetSpecializationName(s.Specialization)))
            .ForMember(d => d.ExperienceLevelName,
                o => o.MapFrom(s => WorkshopCraftsmenCatalog.GetExperienceLevelName(s.ExperienceLevel)))
            .ForMember(d => d.PrimaryImageUrl, o => o.MapFrom(s =>
                s.Images.OrderByDescending(i => i.IsPrimary).Select(i => i.ImageUrl).FirstOrDefault()));

        CreateMap<Craftsman, CraftsmanDetailsDto>()
            .ForMember(d => d.OwnerId, o => o.MapFrom(s => s.UserId))
            .ForMember(d => d.SpecializationName,
                o => o.MapFrom(s => WorkshopCraftsmenCatalog.GetSpecializationName(s.Specialization)))
            .ForMember(d => d.ExperienceLevelName,
                o => o.MapFrom(s => WorkshopCraftsmenCatalog.GetExperienceLevelName(s.ExperienceLevel)));

        CreateMap<CreateCraftsmanRequest, Craftsman>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.User, o => o.Ignore())
            .ForMember(d => d.UserId, o => o.Ignore())
            .ForMember(d => d.Governorate, o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.UpdatedAt, o => o.Ignore())
            .ForMember(d => d.IsDeleted, o => o.Ignore())
            .ForMember(d => d.DeletedAt, o => o.Ignore())
            .ForMember(d => d.Images, o => o.Ignore());
    }
}
