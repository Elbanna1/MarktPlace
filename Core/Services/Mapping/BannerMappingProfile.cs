using AutoMapper;
using Domain.Entities;
using Shared.DTOs.Banners;

namespace Services.Mapping;

public class BannerMappingProfile : Profile
{
    public BannerMappingProfile()
    {
        CreateMap<Banner, BannerDto>()
            .ForMember(dto => dto.IsCurrentlyVisible, options => options.Ignore());
    }
}
