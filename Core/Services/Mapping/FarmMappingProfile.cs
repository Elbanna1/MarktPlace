using AutoMapper;
using Domain.Entities;
using Shared.Constants;
using Shared.DTOs.Farms;

namespace Services.Mapping;

public class FarmMappingProfile : Profile
{
    public FarmMappingProfile()
    {
        CreateMap<FarmImage, FarmImageDto>()
            .ForMember(d => d.Url, o => o.MapFrom(s => s.ImageUrl));

        CreateMap<FarmTypeLookup, FarmTypeOptionDto>();
        CreateMap<FarmingMethodLookup, FarmingMethodOptionDto>();
        CreateMap<AvailabilitySeasonLookup, AvailabilitySeasonOptionDto>();

        CreateMap<Farm, FarmListItemDto>()
            .ForMember(d => d.FarmTypeName, o => o.MapFrom(s => BusinessCatalog.GetFarmTypeName(s.FarmType)))
            .ForMember(d => d.FarmTypeGroup, o => o.MapFrom(s => BusinessCatalog.GetFarmTypeGroup(s.FarmType)))
            .ForMember(d => d.AvailabilitySeasonName, o => o.MapFrom(s =>
                s.AvailabilitySeason == null
                    ? null
                    : BusinessCatalog.GetAvailabilitySeasonName(s.AvailabilitySeason.Value)))
            .ForMember(d => d.FarmingMethodName, o => o.MapFrom(s =>
                s.FarmingMethod == null ? null : BusinessCatalog.GetFarmingMethodName(s.FarmingMethod.Value)))
            .ForMember(d => d.PrimaryImageUrl, o => o.MapFrom(s =>
                s.Images.OrderByDescending(i => i.IsPrimary).Select(i => i.ImageUrl).FirstOrDefault()));

        CreateMap<Farm, FarmDetailsDto>()
            .ForMember(d => d.OwnerId, o => o.MapFrom(s => s.UserId))
            .ForMember(d => d.FarmTypeName, o => o.MapFrom(s => BusinessCatalog.GetFarmTypeName(s.FarmType)))
            .ForMember(d => d.FarmTypeGroup, o => o.MapFrom(s => BusinessCatalog.GetFarmTypeGroup(s.FarmType)))
            .ForMember(d => d.AvailabilitySeasonName, o => o.MapFrom(s =>
                s.AvailabilitySeason == null
                    ? null
                    : BusinessCatalog.GetAvailabilitySeasonName(s.AvailabilitySeason.Value)))
            .ForMember(d => d.FarmingMethodName, o => o.MapFrom(s =>
                s.FarmingMethod == null ? null : BusinessCatalog.GetFarmingMethodName(s.FarmingMethod.Value)));

        CreateMap<CreateFarmRequest, Farm>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.User, o => o.Ignore())
            .ForMember(d => d.UserId, o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.UpdatedAt, o => o.Ignore())
            .ForMember(d => d.IsDeleted, o => o.Ignore())
            .ForMember(d => d.DeletedAt, o => o.Ignore())
            .ForMember(d => d.Images, o => o.Ignore());
    }
}
