using Shared.DTOs.GovernorateGuide;

namespace ServicesAbstraction;

public interface ICommercialShopService
    : IDirectoryListingService<CreateCommercialShopRequest, UpdateCommercialShopRequest,
        CommercialShopDto, CommercialShopFilterParams>;

public interface IGeneralServiceService
    : IDirectoryListingService<CreateGeneralServiceRequest, UpdateGeneralServiceRequest,
        GeneralServiceDto, GeneralServiceFilterParams>;

public interface IResortHotelAmusementService
    : IDirectoryListingService<CreateResortHotelAmusementRequest, UpdateResortHotelAmusementRequest,
        ResortHotelAmusementDto, ResortHotelAmusementFilterParams>;
