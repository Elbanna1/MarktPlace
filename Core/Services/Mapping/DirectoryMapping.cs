using AutoMapper;
using Domain.Entities;
using Shared.DTOs.Directory;

namespace Services.Mapping;

public static class DirectoryMapping
{
    public static IMappingExpression<TListing, TDto> MapDirectoryListing<TListing, TImage, TDto>(
        this Profile profile)
        where TListing : DirectoryListing<TImage>
        where TImage : DirectoryListingImage
        where TDto : DirectoryListingDto
    {
        profile.CreateMap<TImage, DirectoryImageDto>()
            .ForMember(d => d.Url, o => o.MapFrom(s => s.ImageUrl));

        return profile.CreateMap<TListing, TDto>()
            .ForMember(d => d.ApprovedAt, o => o.MapFrom(s => s.PublishedAt))
            .ForMember(d => d.OwnerId, o => o.MapFrom(s => s.UserId))
            .ForMember(d => d.OwnerName, o => o.Ignore())
            .ForMember(d => d.ListingType, o => o.Ignore())
            .ForMember(d => d.ListingTypeName, o => o.Ignore())
            .ForMember(d => d.CategoryId, o => o.Ignore())
            .ForMember(d => d.CategoryName, o => o.Ignore())
            .ForMember(d => d.SubCategoryId, o => o.Ignore())
            .ForMember(d => d.SubCategoryName, o => o.Ignore())
            .ForMember(d => d.ShareUrl, o => o.Ignore())
            .ForMember(d => d.PrimaryImageUrl, o => o.Ignore())
            .ForMember(d => d.Views, o => o.Ignore())
            .ForMember(d => d.AverageRating, o => o.Ignore())
            .ForMember(d => d.RatingsCount, o => o.Ignore())
            .ForMember(d => d.IsFavorite, o => o.Ignore())
            .ForMember(d => d.Images, o => o.MapFrom(s => s.Images.OrderBy(i => i.SortOrder)));
    }
}
