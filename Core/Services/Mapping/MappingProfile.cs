using AutoMapper;
using Domain.Entities;
using Shared.DTOs.Profile;

namespace Services.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ApplicationUser, UserDto>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

        CreateMap<ApplicationUser, ProfileDto>()
            .IncludeBase<ApplicationUser, UserDto>()
            .ForMember(dest => dest.Statistics, opt => opt.Ignore());
    }
}
