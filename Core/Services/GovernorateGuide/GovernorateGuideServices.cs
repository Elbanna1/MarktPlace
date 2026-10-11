using AutoMapper;
using Domain.Entities;
using Services.Directory;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.GovernorateGuide;
using Shared.Enums;

namespace Services.GovernorateGuide;

public sealed class CommercialShopService
    : DirectoryListingService<CommercialShop, CommercialShopImage, CreateCommercialShopRequest,
            UpdateCommercialShopRequest, CommercialShopDto, CommercialShopFilterParams>,
        ICommercialShopService
{
    public CommercialShopService(
        IDirectoryRepository<CommercialShop, CommercialShopImage, CommercialShopFilterParams> repository,
        IFileService fileService, IMapper mapper, INotificationService notifications)
        : base(repository, fileService, mapper, notifications)
    {
    }

    protected override DirectoryModule Module => GovernorateGuideCatalog.CommercialShops;

    protected override void ApplyModuleFields(CommercialShop entity, CreateCommercialShopRequest request) =>
        Apply(entity, request);

    public static void Apply(CommercialShop entity, CreateCommercialShopRequest request)
    {
        entity.ActivityType = request.ActivityType;

        entity.OtherActivityType = request.ActivityType == ShopSuitableActivity.Other
            ? Trimmed(request.OtherActivityType)
            : null;
    }
}

public sealed class GeneralServiceService
    : DirectoryListingService<GeneralService, GeneralServiceImage, CreateGeneralServiceRequest,
            UpdateGeneralServiceRequest, GeneralServiceDto, GeneralServiceFilterParams>,
        IGeneralServiceService
{
    public GeneralServiceService(
        IDirectoryRepository<GeneralService, GeneralServiceImage, GeneralServiceFilterParams> repository,
        IFileService fileService, IMapper mapper, INotificationService notifications)
        : base(repository, fileService, mapper, notifications)
    {
    }

    protected override DirectoryModule Module => GovernorateGuideCatalog.GeneralServices;

    protected override void ApplyModuleFields(GeneralService entity, CreateGeneralServiceRequest request) =>
        Apply(entity, request);

    public static void Apply(GeneralService entity, CreateGeneralServiceRequest request)
    {
        entity.ServiceType = request.ServiceType!.Value;
        entity.ServiceSubtype = request.SelectedSubtype!.Value;
    }
}

public sealed class ResortHotelAmusementService
    : DirectoryListingService<ResortHotelAmusement, ResortHotelAmusementImage, CreateResortHotelAmusementRequest,
            UpdateResortHotelAmusementRequest, ResortHotelAmusementDto, ResortHotelAmusementFilterParams>,
        IResortHotelAmusementService
{
    public ResortHotelAmusementService(
        IDirectoryRepository<ResortHotelAmusement, ResortHotelAmusementImage, ResortHotelAmusementFilterParams> repository,
        IFileService fileService, IMapper mapper, INotificationService notifications)
        : base(repository, fileService, mapper, notifications)
    {
    }

    protected override DirectoryModule Module => GovernorateGuideCatalog.ResortsHotelsAndAmusement;
}
