using AutoMapper;
using Domain.Entities;
using Shared.Constants;
using Shared.DTOs.Feedback;

namespace Services.Mapping;

public class FeedbackMappingProfile : Profile
{
    public FeedbackMappingProfile()
    {
        CreateMap<FeedbackImage, FeedbackImageDto>()
            .ForMember(d => d.Url, o => o.MapFrom(s => s.ImageUrl));

        CreateMap<Feedback, FeedbackListItemDto>()
            .ForMember(d => d.Type, o => o.MapFrom(s => s.Type.ToString()))
            .ForMember(d => d.TypeId, o => o.MapFrom(s => s.Type))
            .ForMember(d => d.TypeName, o => o.MapFrom(s => FeedbackCatalog.NameOf(s.Type)))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
            .ForMember(d => d.StatusId, o => o.MapFrom(s => s.Status))
            .ForMember(d => d.StatusName, o => o.MapFrom(s => FeedbackCatalog.NameOf(s.Status)))
            .ForMember(d => d.HasReply, o => o.MapFrom(s => s.AdminReply != null && s.AdminReply != ""))
            .ForMember(d => d.IsClosed, o => o.MapFrom(s => FeedbackCatalog.IsClosed(s.Status)))
            .ForMember(d => d.ImagesCount, o => o.MapFrom(s => s.Images.Count))
            .ForMember(d => d.PrimaryImageUrl, o => o.MapFrom(s =>
                s.Images.OrderByDescending(i => i.IsPrimary).Select(i => i.ImageUrl).FirstOrDefault()))
            .ForMember(d => d.User, o => o.Ignore());

        CreateMap<Feedback, FeedbackDetailsDto>()
            .IncludeBase<Feedback, FeedbackListItemDto>()
            .ForMember(d => d.Images, o => o.MapFrom(s =>
                s.Images.OrderByDescending(i => i.IsPrimary).ThenBy(i => i.CreatedAt)));

        CreateMap<CreateFeedbackRequest, Feedback>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.User, o => o.Ignore())
            .ForMember(d => d.UserId, o => o.Ignore())
            .ForMember(d => d.Status, o => o.Ignore())
            .ForMember(d => d.AdminReply, o => o.Ignore())
            .ForMember(d => d.ReviewedBy, o => o.Ignore())
            .ForMember(d => d.Reviewer, o => o.Ignore())
            .ForMember(d => d.ReviewedAt, o => o.Ignore())
            .ForMember(d => d.ClosedAt, o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.UpdatedAt, o => o.Ignore())
            .ForMember(d => d.Images, o => o.Ignore());
    }
}
