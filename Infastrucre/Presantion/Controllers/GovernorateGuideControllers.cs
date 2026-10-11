using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.GovernorateGuide;
using Shared.Responses;

namespace Presentation.Controllers;

[Route(GovernorateGuideRoutes.CommercialShops)]
[Tags("Governorate Guide - Commercial Shops")]
public class CommercialShopsController(ICommercialShopService service)
    : DirectoryControllerBase<CreateCommercialShopRequest, UpdateCommercialShopRequest,
        CommercialShopDto, CommercialShopFilterParams>(service)
{
    [HttpGet("activity-types")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<ApiResponse<IReadOnlyList<object>>> GetActivityTypes()
    {
        var activities = ShopCatalog.SuitableActivities
            .Select(entry => (object)new { id = (int)entry.Value, name = entry.Name, nameEn = entry.NameEn })
            .ToList();

        return Ok(ApiResponse<IReadOnlyList<object>>.Ok(activities, UserMessages.Lookups.Loaded));
    }
}

[Route(GovernorateGuideRoutes.GeneralServices)]
[Tags("Governorate Guide - General Services")]
public class GeneralServicesController(IGeneralServiceService service)
    : DirectoryControllerBase<CreateGeneralServiceRequest, UpdateGeneralServiceRequest,
        GeneralServiceDto, GeneralServiceFilterParams>(service)
{
    [HttpGet("service-types")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<ApiResponse<IReadOnlyList<object>>> GetServiceTypes()
    {
        var types = GovernorateGuideCatalog.ServiceTypes
            .Select(type => (object)new
            {
                id = (int)type.Id,
                name = type.Name,
                nameEn = type.NameEn,
                subtypes = GovernorateGuideCatalog.SubtypesOf(type.Id)
                    .Select(entry => new { id = (int)entry.Id, name = entry.Name, nameEn = entry.NameEn })
                    .ToList()
            })
            .ToList();

        return Ok(ApiResponse<IReadOnlyList<object>>.Ok(types, UserMessages.Lookups.Loaded));
    }
}

[Route(GovernorateGuideRoutes.ResortsHotelsAndAmusement)]
[Tags("Governorate Guide - Resorts, Hotels & Amusement")]
public class ResortsHotelsAndAmusementController(IResortHotelAmusementService service)
    : DirectoryControllerBase<CreateResortHotelAmusementRequest, UpdateResortHotelAmusementRequest,
        ResortHotelAmusementDto, ResortHotelAmusementFilterParams>(service);
