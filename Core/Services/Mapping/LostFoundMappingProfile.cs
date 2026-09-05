using AutoMapper;
using Domain.Entities;
using Shared.DTOs.LostFound;

namespace Services.Mapping;

public class LostFoundMappingProfile : Profile
{
    public LostFoundMappingProfile()
    {
        CreateMap<LostFoundImage, LostFoundImageDto>()
            .ForMember(d => d.Url, o => o.MapFrom(s => s.ImageUrl));

        CreateMap<LostFoundPost, LostFoundPostListItemDto>()
            .ForMember(d => d.IsFavorite, o => o.Ignore())
            .ForMember(d => d.IsLikedByCurrentUser, o => o.Ignore())
            .ForMember(d => d.PostType, o => o.MapFrom(s => s.PostType.ToString()))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
            .ForMember(d => d.PrimaryImageUrl, o => o.MapFrom(s =>
                s.Images.OrderByDescending(i => i.IsPrimary).Select(i => i.ImageUrl).FirstOrDefault()));

        CreateMap<LostFoundPost, LostFoundPostDetailsDto>()
            .ForMember(d => d.IsFavorite, o => o.Ignore())
            .ForMember(d => d.IsLikedByCurrentUser, o => o.Ignore());

        CreateMap<CreateLostFoundPostRequest, LostFoundPost>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.User, o => o.Ignore())
            .ForMember(d => d.UserId, o => o.Ignore())
            .ForMember(d => d.Status, o => o.Ignore())
            .ForMember(d => d.Governorate, o => o.Ignore())
            .ForMember(d => d.LikesCount, o => o.Ignore())
            .ForMember(d => d.CommentsCount, o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.UpdatedAt, o => o.Ignore())
            .ForMember(d => d.IsDeleted, o => o.Ignore())
            .ForMember(d => d.DeletedAt, o => o.Ignore())
            .ForMember(d => d.Images, o => o.Ignore())
            .ForMember(d => d.Likes, o => o.Ignore())
            .ForMember(d => d.Comments, o => o.Ignore());
    }
}
