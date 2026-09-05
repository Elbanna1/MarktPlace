using AutoMapper;
using Domain.Entities;
using Shared.Constants;
using Shared.DTOs.Charity;

namespace Services.Mapping;

public class CharityMappingProfile : Profile
{
    public CharityMappingProfile()
    {
        CreateMap<RescueImage, CharityImageDto>()
            .ForMember(d => d.Url, o => o.MapFrom(s => s.ImageUrl));

        CreateMap<BloodRequestImage, CharityImageDto>()
            .ForMember(d => d.Url, o => o.MapFrom(s => s.ImageUrl));

        CreateMap<AskConsultImage, CharityImageDto>()
            .ForMember(d => d.Url, o => o.MapFrom(s => s.ImageUrl));

        CreateMap<Rescue, RescueDetailsDto>()
            .ForMember(d => d.ApprovedAt, o => o.MapFrom(s => s.PublishedAt))
            .ForMember(d => d.OwnerId, o => o.MapFrom(s => s.UserId))
            .ForMember(d => d.Images, o => o.MapFrom(s => s.Images.OrderBy(i => i.SortOrder)));

        CreateMap<Rescue, RescueListItemDto>()
            .ForMember(d => d.ApprovedAt, o => o.MapFrom(s => s.PublishedAt))
            .ForMember(d => d.OwnerId, o => o.MapFrom(s => s.UserId))
            .ForMember(d => d.Images, o => o.MapFrom(s => s.Images.OrderBy(i => i.SortOrder)));

        CreateMap<BloodRequest, BloodRequestDetailsDto>()
            .ForMember(d => d.ApprovedAt, o => o.MapFrom(s => s.PublishedAt))
            .ForMember(d => d.OwnerId, o => o.MapFrom(s => s.UserId))
            .ForMember(d => d.BloodGroupName,
                o => o.MapFrom(s => CharityCatalog.GetBloodGroupName(s.BloodGroup)))
            .ForMember(d => d.Images, o => o.MapFrom(s => s.Images.OrderBy(i => i.SortOrder)));

        CreateMap<BloodRequest, BloodRequestListItemDto>()
            .ForMember(d => d.ApprovedAt, o => o.MapFrom(s => s.PublishedAt))
            .ForMember(d => d.OwnerId, o => o.MapFrom(s => s.UserId))
            .ForMember(d => d.BloodGroupName,
                o => o.MapFrom(s => CharityCatalog.GetBloodGroupName(s.BloodGroup)))
            .ForMember(d => d.Images, o => o.MapFrom(s => s.Images.OrderBy(i => i.SortOrder)));

        CreateMap<AskConsult, AskConsultDetailsDto>()
            .ForMember(d => d.ApprovedAt, o => o.MapFrom(s => s.PublishedAt))
            .ForMember(d => d.OwnerId, o => o.MapFrom(s => s.UserId))
            .ForMember(d => d.CategoryLabel,
                o => o.MapFrom(s => CharityCatalog.GetAskConsultCategoryName(s.Category)))
            .ForMember(d => d.Images, o => o.MapFrom(s => s.Images.OrderBy(i => i.SortOrder)));

        CreateMap<AskConsult, AskConsultListItemDto>()
            .ForMember(d => d.ApprovedAt, o => o.MapFrom(s => s.PublishedAt))
            .ForMember(d => d.OwnerId, o => o.MapFrom(s => s.UserId))
            .ForMember(d => d.CategoryLabel,
                o => o.MapFrom(s => CharityCatalog.GetAskConsultCategoryName(s.Category)))
            .ForMember(d => d.Images, o => o.MapFrom(s => s.Images.OrderBy(i => i.SortOrder)));
    }
}
