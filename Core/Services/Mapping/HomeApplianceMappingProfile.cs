using AutoMapper;
using Domain.Entities;
using Shared.Constants;
using Shared.DTOs.HomeFurnishing;

namespace Services.Mapping;

public class HomeApplianceMappingProfile : Profile
{
    public HomeApplianceMappingProfile()
    {
        CreateMap<HomeApplianceImage, HomeFurnishingImageDto>()
            .ForMember(d => d.Url, o => o.MapFrom(s => s.ImageUrl));

        CreateMap<HomeApplianceDeviceTypeLookup, HomeFurnishingLookupItemDto>();
        CreateMap<HomeApplianceBrandLookup, HomeFurnishingLookupItemDto>();
        CreateMap<HomeApplianceConditionLookup, HomeFurnishingLookupItemDto>();
        CreateMap<HomeApplianceWarrantyLookup, HomeFurnishingLookupItemDto>();
        CreateMap<HomeApplianceColorLookup, HomeFurnishingLookupItemDto>();

        CreateMap<HomeAppliance, HomeApplianceListItemDto>()
            .ForMember(d => d.DeviceTypeName,
                o => o.MapFrom(s => HomeApplianceCatalog.GetDeviceTypeName(s.DeviceType)))
            .ForMember(d => d.BrandName,
                o => o.MapFrom(s => HomeApplianceCatalog.GetBrandName(s.Brand)))
            .ForMember(d => d.ConditionName,
                o => o.MapFrom(s => HomeApplianceCatalog.GetConditionName(s.Condition)))
            .ForMember(d => d.WarrantyName,
                o => o.MapFrom(s => HomeApplianceCatalog.GetWarrantyName(s.Warranty)))
            .ForMember(d => d.Colors,
                o => o.MapFrom(s => HomeApplianceCatalog.SelectedColors(s.Colors.Select(x => x.Color))))
            .ForMember(d => d.Images,
                o => o.MapFrom(s => s.Images.OrderBy(i => i.SortOrder)))
            .ForMember(d => d.PrimaryImageUrl, o => o.MapFrom(s =>
                s.Images.OrderBy(i => i.SortOrder).Select(i => i.ImageUrl).FirstOrDefault()));

        CreateMap<HomeAppliance, HomeApplianceDetailsDto>()
            .ForMember(d => d.OwnerId, o => o.MapFrom(s => s.UserId))
            .ForMember(d => d.ShareUrl, o => o.Ignore())
            .ForMember(d => d.DeviceTypeName,
                o => o.MapFrom(s => HomeApplianceCatalog.GetDeviceTypeName(s.DeviceType)))
            .ForMember(d => d.BrandName,
                o => o.MapFrom(s => HomeApplianceCatalog.GetBrandName(s.Brand)))
            .ForMember(d => d.ConditionName,
                o => o.MapFrom(s => HomeApplianceCatalog.GetConditionName(s.Condition)))
            .ForMember(d => d.WarrantyName,
                o => o.MapFrom(s => HomeApplianceCatalog.GetWarrantyName(s.Warranty)))
            .ForMember(d => d.Colors,
                o => o.MapFrom(s => HomeApplianceCatalog.SelectedColors(s.Colors.Select(x => x.Color))))
            .ForMember(d => d.Images,
                o => o.MapFrom(s => s.Images.OrderBy(i => i.SortOrder)));
    }
}
