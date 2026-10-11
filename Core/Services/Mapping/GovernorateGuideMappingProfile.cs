using AutoMapper;
using Domain.Entities;
using Shared.Constants;
using Shared.DTOs.GovernorateGuide;

namespace Services.Mapping;

public class GovernorateGuideMappingProfile : Profile
{
    public GovernorateGuideMappingProfile()
    {
        this.MapDirectoryListing<CommercialShop, CommercialShopImage, CommercialShopDto>()
            .ForMember(d => d.ActivityTypeName,
                o => o.MapFrom(s => GovernorateGuideCatalog.GetActivityTypeName(s.ActivityType)));

        this.MapDirectoryListing<GeneralService, GeneralServiceImage, GeneralServiceDto>()
            .ForMember(d => d.ServiceTypeName,
                o => o.MapFrom(s => GovernorateGuideCatalog.GetServiceTypeName(s.ServiceType)))
            .ForMember(d => d.ServiceSubtypeName,
                o => o.MapFrom(s => GovernorateGuideCatalog.GetServiceSubtypeName(s.ServiceSubtype)));

        this.MapDirectoryListing<ResortHotelAmusement, ResortHotelAmusementImage, ResortHotelAmusementDto>();
    }
}
