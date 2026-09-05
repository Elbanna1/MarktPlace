using AutoMapper;
using Domain.Entities;
using Shared.Constants;
using Shared.DTOs.Workshops;

namespace Services.Mapping;

public class WorkshopMappingProfile : Profile
{
    public WorkshopMappingProfile()
    {
        CreateMap<WorkshopImage, WorkshopImageDto>()
            .ForMember(d => d.Url, o => o.MapFrom(s => s.ImageUrl));

        CreateMap<Workshop, WorkshopListItemDto>()
            .ForMember(d => d.WorkshopTypeName,
                o => o.MapFrom(s => WorkshopCraftsmenCatalog.GetWorkshopTypeName(s.WorkshopType)))
            .ForMember(d => d.PrimaryImageUrl, o => o.MapFrom(s =>
                s.Images.OrderByDescending(i => i.IsPrimary).Select(i => i.ImageUrl).FirstOrDefault()));

        CreateMap<Workshop, WorkshopDetailsDto>()
            .ForMember(d => d.OwnerId, o => o.MapFrom(s => s.UserId))
            .ForMember(d => d.WorkshopTypeName,
                o => o.MapFrom(s => WorkshopCraftsmenCatalog.GetWorkshopTypeName(s.WorkshopType)));

        CreateMap<CreateWorkshopRequest, Workshop>()
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
