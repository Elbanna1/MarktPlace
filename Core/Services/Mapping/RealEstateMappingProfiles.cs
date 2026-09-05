using AutoMapper;
using Domain.Entities;
using Shared.Constants;
using Shared.DTOs.RealEstate;
using Shared.Enums;

namespace Services.Mapping;

public class LandMappingProfile : Profile
{
    public LandMappingProfile()
    {
        CreateMap<LandImage, RealEstateImageDto>()
            .ForMember(d => d.Url, o => o.MapFrom(s => s.ImageUrl));

        CreateMap<Land, LandListItemDto>()
            .IncludeCommonRealEstateNames(s => s.ListingType, s => s.Project)
            .ForMember(d => d.LandTypeName, o => o.MapFrom(s => LandCatalog.GetLandTypeName(s.LandType)))
            .ForMember(d => d.AreaUnitName, o => o.MapFrom(s => LandCatalog.GetAreaUnitName(s.AreaUnit)))
            .ForMember(d => d.FacadesCountName, o => o.MapFrom(s => LandCatalog.GetFacadesCountName(s.FacadesCount)))
            .ForMember(d => d.DirectionName, o => o.MapFrom(s => LandCatalog.GetDirectionName(s.Direction)))
            .ForMember(d => d.RoadTypeName, o => o.MapFrom(s => LandCatalog.GetRoadTypeName(s.RoadType)))
            .ForMember(d => d.LegalStatusName, o => o.MapFrom(s => LandCatalog.GetLegalStatusName(s.LegalStatus)))
            .ForMember(d => d.OwnershipDocumentName,
                o => o.MapFrom(s => LandCatalog.GetOwnershipDocumentName(s.OwnershipDocument)))
            .ForMember(d => d.IrrigationSourceName,
                o => o.MapFrom(s => LandCatalog.GetIrrigationSourceName(s.IrrigationSource)))
            .ForMember(d => d.Utilities,
                o => o.MapFrom(s => LandCatalog.SelectedUtilities(s.Utilities.Select(x => x.Utility))))
            .ForMember(d => d.Images, o => o.MapFrom(s => s.Images.OrderBy(i => i.SortOrder)))
            .ForMember(d => d.PrimaryImageUrl, o => o.MapFrom(s =>
                s.Images.OrderBy(i => i.SortOrder).Select(i => i.ImageUrl).FirstOrDefault()));

        CreateMap<Land, LandDetailsDto>()
            .IncludeCommonRealEstateNames(s => s.ListingType, s => s.Project)
            .ForMember(d => d.OwnerId, o => o.MapFrom(s => s.UserId))
            .ForMember(d => d.ShareUrl, o => o.Ignore())
            .ForMember(d => d.LandTypeName, o => o.MapFrom(s => LandCatalog.GetLandTypeName(s.LandType)))
            .ForMember(d => d.AreaUnitName, o => o.MapFrom(s => LandCatalog.GetAreaUnitName(s.AreaUnit)))
            .ForMember(d => d.FacadesCountName, o => o.MapFrom(s => LandCatalog.GetFacadesCountName(s.FacadesCount)))
            .ForMember(d => d.DirectionName, o => o.MapFrom(s => LandCatalog.GetDirectionName(s.Direction)))
            .ForMember(d => d.RoadTypeName, o => o.MapFrom(s => LandCatalog.GetRoadTypeName(s.RoadType)))
            .ForMember(d => d.LegalStatusName, o => o.MapFrom(s => LandCatalog.GetLegalStatusName(s.LegalStatus)))
            .ForMember(d => d.ReconciliationFormName,
                o => o.MapFrom(s => LandCatalog.GetReconciliationFormName(s.ReconciliationForm)))
            .ForMember(d => d.OwnershipDocumentName,
                o => o.MapFrom(s => LandCatalog.GetOwnershipDocumentName(s.OwnershipDocument)))
            .ForMember(d => d.RentTypeName, o => o.MapFrom(s => LandCatalog.GetRentTypeName(s.RentType)))
            .ForMember(d => d.MinimumRentPeriodName,
                o => o.MapFrom(s => LandCatalog.GetMinimumRentPeriodName(s.MinimumRentPeriod)))
            .ForMember(d => d.ContractDurationName,
                o => o.MapFrom(s => LandCatalog.GetContractDurationName(s.ContractDuration)))
            .ForMember(d => d.ExchangeWithName,
                o => o.MapFrom(s => LandCatalog.GetExchangeTargetName(s.ExchangeWith)))
            .ForMember(d => d.HarvestSeasonName,
                o => o.MapFrom(s => LandCatalog.GetHarvestSeasonName(s.HarvestSeason)))
            .ForMember(d => d.SoilTypeName, o => o.MapFrom(s => LandCatalog.GetSoilTypeName(s.SoilType)))
            .ForMember(d => d.IrrigationSourceName,
                o => o.MapFrom(s => LandCatalog.GetIrrigationSourceName(s.IrrigationSource)))
            .ForMember(d => d.QualityCertificateName,
                o => o.MapFrom(s => LandCatalog.GetQualityCertificateName(s.QualityCertificate)))
            .ForMember(d => d.ExistingBuildingTypeName,
                o => o.MapFrom(s => LandCatalog.GetExistingBuildingTypeName(s.ExistingBuildingType)))
            .ForMember(d => d.BuildingCompletionRatioName,
                o => o.MapFrom(s => LandCatalog.GetBuildingCompletionRatioName(s.BuildingCompletionRatio)))
            .ForMember(d => d.Utilities,
                o => o.MapFrom(s => LandCatalog.SelectedUtilities(s.Utilities.Select(x => x.Utility))))
            .ForMember(d => d.RentInclusions,
                o => o.MapFrom(s => LandCatalog.SelectedRentInclusions(s.RentInclusions.Select(x => x.Inclusion))))
            .ForMember(d => d.Images, o => o.MapFrom(s => s.Images.OrderBy(i => i.SortOrder)));
    }
}

public class ApartmentMappingProfile : Profile
{
    public ApartmentMappingProfile()
    {
        CreateMap<ApartmentImage, RealEstateImageDto>()
            .ForMember(d => d.Url, o => o.MapFrom(s => s.ImageUrl));

        CreateMap<Apartment, ApartmentListItemDto>()
            .IncludeCommonRealEstateNames(s => s.ListingType, s => s.Project)
            .ForMember(d => d.ApartmentTypeName,
                o => o.MapFrom(s => ApartmentCatalog.GetApartmentTypeName(s.ApartmentType)))
            .ForMember(d => d.OwnershipTypeName,
                o => o.MapFrom(s => ApartmentCatalog.GetOwnershipTypeName(s.OwnershipType)))
            .ForMember(d => d.FloorTypeName, o => o.MapFrom(s => ApartmentCatalog.GetFloorTypeName(s.FloorType)))
            .ForMember(d => d.FurnishedStatusName,
                o => o.MapFrom(s => ApartmentCatalog.GetFurnishedStatusName(s.FurnishedStatus)))
            .ForMember(d => d.FinishingTypeName,
                o => o.MapFrom(s => ApartmentCatalog.GetFinishingTypeName(s.FinishingType)))
            .ForMember(d => d.PropertyAgeName,
                o => o.MapFrom(s => ApartmentCatalog.GetPropertyAgeName(s.PropertyAge)))
            .ForMember(d => d.LegalStatusName,
                o => o.MapFrom(s => ApartmentCatalog.GetLegalStatusName(s.LegalStatus)))
            .ForMember(d => d.OwnershipDocumentName,
                o => o.MapFrom(s => ApartmentCatalog.GetOwnershipDocumentName(s.OwnershipDocument)))
            .ForMember(d => d.Features,
                o => o.MapFrom(s => ApartmentCatalog.SelectedFeatures(s.Features.Select(x => x.Feature))))
            .ForMember(d => d.Images, o => o.MapFrom(s => s.Images.OrderBy(i => i.SortOrder)))
            .ForMember(d => d.PrimaryImageUrl, o => o.MapFrom(s =>
                s.Images.OrderBy(i => i.SortOrder).Select(i => i.ImageUrl).FirstOrDefault()));

        CreateMap<Apartment, ApartmentDetailsDto>()
            .IncludeCommonRealEstateNames(s => s.ListingType, s => s.Project)
            .ForMember(d => d.OwnerId, o => o.MapFrom(s => s.UserId))
            .ForMember(d => d.ShareUrl, o => o.Ignore())
            .ForMember(d => d.ApartmentTypeName,
                o => o.MapFrom(s => ApartmentCatalog.GetApartmentTypeName(s.ApartmentType)))
            .ForMember(d => d.OwnershipTypeName,
                o => o.MapFrom(s => ApartmentCatalog.GetOwnershipTypeName(s.OwnershipType)))
            .ForMember(d => d.ReceptionPiecesName,
                o => o.MapFrom(s => ApartmentCatalog.GetReceptionPiecesName(s.ReceptionPieces)))
            .ForMember(d => d.FloorTypeName, o => o.MapFrom(s => ApartmentCatalog.GetFloorTypeName(s.FloorType)))
            .ForMember(d => d.FurnishedStatusName,
                o => o.MapFrom(s => ApartmentCatalog.GetFurnishedStatusName(s.FurnishedStatus)))
            .ForMember(d => d.FinishingTypeName,
                o => o.MapFrom(s => ApartmentCatalog.GetFinishingTypeName(s.FinishingType)))
            .ForMember(d => d.PropertyAgeName,
                o => o.MapFrom(s => ApartmentCatalog.GetPropertyAgeName(s.PropertyAge)))
            .ForMember(d => d.DirectionName, o => o.MapFrom(s => ApartmentCatalog.GetDirectionName(s.Direction)))
            .ForMember(d => d.ViewTypeName, o => o.MapFrom(s => ApartmentCatalog.GetViewTypeName(s.ViewType)))
            .ForMember(d => d.LegalStatusName,
                o => o.MapFrom(s => ApartmentCatalog.GetLegalStatusName(s.LegalStatus)))
            .ForMember(d => d.ReconciliationFormName,
                o => o.MapFrom(s => ApartmentCatalog.GetReconciliationFormName(s.ReconciliationForm)))
            .ForMember(d => d.OwnershipDocumentName,
                o => o.MapFrom(s => ApartmentCatalog.GetOwnershipDocumentName(s.OwnershipDocument)))
            .ForMember(d => d.PaymentMethodName,
                o => o.MapFrom(s => ApartmentCatalog.GetPaymentMethodName(s.PaymentMethod)))
            .ForMember(d => d.InstallmentProviderName,
                o => o.MapFrom(s => ApartmentCatalog.GetInstallmentProviderName(s.InstallmentProvider)))
            .ForMember(d => d.RentTypeName, o => o.MapFrom(s => ApartmentCatalog.GetRentTypeName(s.RentType)))
            .ForMember(d => d.SuitableForName,
                o => o.MapFrom(s => ApartmentCatalog.GetSuitableForName(s.SuitableFor)))
            .ForMember(d => d.ExchangeWithName,
                o => o.MapFrom(s => ApartmentCatalog.GetExchangeTargetName(s.ExchangeWith)))
            .ForMember(d => d.Features,
                o => o.MapFrom(s => ApartmentCatalog.SelectedFeatures(s.Features.Select(x => x.Feature))))
            .ForMember(d => d.RentInclusions,
                o => o.MapFrom(s =>
                    ApartmentCatalog.SelectedRentInclusions(s.RentInclusions.Select(x => x.Inclusion))))
            .ForMember(d => d.Images, o => o.MapFrom(s => s.Images.OrderBy(i => i.SortOrder)));
    }
}

public class ShopMappingProfile : Profile
{
    public ShopMappingProfile()
    {
        CreateMap<ShopImage, RealEstateImageDto>()
            .ForMember(d => d.Url, o => o.MapFrom(s => s.ImageUrl));

        CreateMap<Shop, ShopListItemDto>()
            .IncludeCommonRealEstateNames(s => s.ListingType, s => s.Project)
            .ForMember(d => d.SuitableActivityName,
                o => o.MapFrom(s => ShopCatalog.GetSuitableActivityName(s.SuitableActivity)))
            .ForMember(d => d.FloorTypeName, o => o.MapFrom(s => ShopCatalog.GetFloorTypeName(s.FloorType)))
            .ForMember(d => d.FinishingTypeName,
                o => o.MapFrom(s => ShopCatalog.GetFinishingTypeName(s.FinishingType)))
            .ForMember(d => d.FacadesCountName,
                o => o.MapFrom(s => ShopCatalog.GetFacadesCountName(s.FacadesCount)))
            .ForMember(d => d.PropertyAgeName,
                o => o.MapFrom(s => ShopCatalog.GetPropertyAgeName(s.PropertyAge)))
            .ForMember(d => d.LegalStatusName,
                o => o.MapFrom(s => ShopCatalog.GetLegalStatusName(s.LegalStatus)))
            .ForMember(d => d.LicenseTypeName,
                o => o.MapFrom(s => ShopCatalog.GetLicenseTypeName(s.LicenseType)))
            .ForMember(d => d.OwnershipDocumentName,
                o => o.MapFrom(s => ShopCatalog.GetOwnershipDocumentName(s.OwnershipDocument)))
            .ForMember(d => d.Utilities,
                o => o.MapFrom(s => ShopCatalog.SelectedUtilities(s.Utilities.Select(x => x.Utility))))
            .ForMember(d => d.Images, o => o.MapFrom(s => s.Images.OrderBy(i => i.SortOrder)))
            .ForMember(d => d.PrimaryImageUrl, o => o.MapFrom(s =>
                s.Images.OrderBy(i => i.SortOrder).Select(i => i.ImageUrl).FirstOrDefault()));

        CreateMap<Shop, ShopDetailsDto>()
            .IncludeCommonRealEstateNames(s => s.ListingType, s => s.Project)
            .ForMember(d => d.OwnerId, o => o.MapFrom(s => s.UserId))
            .ForMember(d => d.ShareUrl, o => o.Ignore())
            .ForMember(d => d.SuitableActivityName,
                o => o.MapFrom(s => ShopCatalog.GetSuitableActivityName(s.SuitableActivity)))
            .ForMember(d => d.FloorTypeName, o => o.MapFrom(s => ShopCatalog.GetFloorTypeName(s.FloorType)))
            .ForMember(d => d.FacadesCountName,
                o => o.MapFrom(s => ShopCatalog.GetFacadesCountName(s.FacadesCount)))
            .ForMember(d => d.FacadeDirectionName,
                o => o.MapFrom(s => ShopCatalog.GetFacadeDirectionName(s.FacadeDirection)))
            .ForMember(d => d.FinishingTypeName,
                o => o.MapFrom(s => ShopCatalog.GetFinishingTypeName(s.FinishingType)))
            .ForMember(d => d.PropertyAgeName,
                o => o.MapFrom(s => ShopCatalog.GetPropertyAgeName(s.PropertyAge)))
            .ForMember(d => d.EntrancesCountName,
                o => o.MapFrom(s => ShopCatalog.GetEntrancesCountName(s.EntrancesCount)))
            .ForMember(d => d.LegalStatusName,
                o => o.MapFrom(s => ShopCatalog.GetLegalStatusName(s.LegalStatus)))
            .ForMember(d => d.LicenseTypeName,
                o => o.MapFrom(s => ShopCatalog.GetLicenseTypeName(s.LicenseType)))
            .ForMember(d => d.ReconciliationFormName,
                o => o.MapFrom(s => ShopCatalog.GetReconciliationFormName(s.ReconciliationForm)))
            .ForMember(d => d.OwnershipDocumentName,
                o => o.MapFrom(s => ShopCatalog.GetOwnershipDocumentName(s.OwnershipDocument)))
            .ForMember(d => d.PaymentMethodName,
                o => o.MapFrom(s => ShopCatalog.GetPaymentMethodName(s.PaymentMethod)))
            .ForMember(d => d.InstallmentProviderName,
                o => o.MapFrom(s => ShopCatalog.GetInstallmentProviderName(s.InstallmentProvider)))
            .ForMember(d => d.RentTypeName, o => o.MapFrom(s => ShopCatalog.GetRentTypeName(s.RentType)))
            .ForMember(d => d.ExchangeWithName,
                o => o.MapFrom(s => ShopCatalog.GetExchangeTargetName(s.ExchangeWith)))
            .ForMember(d => d.Utilities,
                o => o.MapFrom(s => ShopCatalog.SelectedUtilities(s.Utilities.Select(x => x.Utility))))
            .ForMember(d => d.RentInclusions,
                o => o.MapFrom(s => ShopCatalog.SelectedRentInclusions(s.RentInclusions.Select(x => x.Inclusion))))
            .ForMember(d => d.RentSuitableActivities,
                o => o.MapFrom(s =>
                    ShopCatalog.SelectedRentSuitableActivities(s.RentSuitableActivities.Select(x => x.Activity))))
            .ForMember(d => d.Images, o => o.MapFrom(s => s.Images.OrderBy(i => i.SortOrder)));
    }
}

internal static class RealEstateMappingExtensions
{
    public static IMappingExpression<TSource, TDestination> IncludeCommonRealEstateNames<TSource, TDestination>(
        this IMappingExpression<TSource, TDestination> map,
        Func<TSource, RealEstateListingType> listingType,
        Func<TSource, RealEstateProject?> project)
        where TDestination : IRealEstateNamedPayload =>
        map
            .ForMember(d => d.ListingTypeName,
                o => o.MapFrom(src => RealEstateCatalog.GetListingTypeName(listingType(src))))
            .ForMember(d => d.ProjectName,
                o => o.MapFrom(src => RealEstateCatalog.GetProjectName(project(src))));
}
