using AutoMapper;
using Domain.Entities;
using Shared.Constants;
using Services.Validation;
using Shared.DTOs.Advertisements;
using Shared.Enums;

namespace Services.Mapping;

public class AdvertisementMappingProfile : Profile
{
    public AdvertisementMappingProfile()
    {
        CreateMap<Feature, FeatureDto>();

        CreateMap<AdvertisementImage, AdvertisementImageDto>()
            .ForMember(d => d.Url, o => o.MapFrom(s => s.ImageUrl));

        CreateMap<ApplicationUser, OwnerDto>()
            .ForMember(d => d.FullName, o => o.MapFrom(s => (s.FirstName + " " + s.SecondName).Trim()))
            .ForMember(d => d.PhoneNumber, o => o.MapFrom(s => s.PhoneNumber));

        CreateMap<AdvertisementCardRow, AdvertisementListItemDto>()
            .ForMember(d => d.ListingType, o => o.MapFrom(s => s.ListingType.ToString()))
            .ForMember(d => d.Status, o => o.MapFrom(s =>
                (s.Status == AdvertisementStatus.Active && s.ExpireAt != null && s.ExpireAt <= DateTime.UtcNow
                    ? AdvertisementStatus.Expired
                    : s.Status).ToString()))
            .ForMember(d => d.IsExpired, o => o.MapFrom(s => IsExpired(s)))
            .ForMember(d => d.RemainingDays, o => o.MapFrom(s => RemainingDays(s)))
            .ForMember(d => d.PrimaryImageUrl, o => o.MapFrom(s =>
                s.Images.Where(i => i.IsPrimary).Select(i => i.Url).FirstOrDefault()
                ?? s.Images.Select(i => i.Url).FirstOrDefault()))
            .ForMember(d => d.Owner, o => o.MapFrom(s => new OwnerDto
            {
                Id = s.OwnerId,
                FullName = ((s.OwnerFirstName ?? string.Empty) + " " + (s.OwnerSecondName ?? string.Empty)).Trim(),
                PhoneNumber = s.OwnerPhoneNumber ?? string.Empty,
                Governorate = s.OwnerGovernorate ?? string.Empty,
                Center = s.OwnerCenter ?? string.Empty,
                ProfileImageUrl = s.OwnerProfileImageUrl,
                CreatedAt = s.OwnerCreatedAt
            }))
            .ForMember(d => d.Views, o => o.Ignore())
            .ForMember(d => d.FavoriteCount, o => o.Ignore())
            .ForMember(d => d.AverageRating, o => o.Ignore())
            .ForMember(d => d.RatingsCount, o => o.Ignore())
            .ForMember(d => d.IsFavorite, o => o.Ignore());

        CreateMap<Advertisement, AdvertisementDetailsDto>()
            .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category.Name))
            .ForMember(d => d.SubCategoryName, o => o.MapFrom(s => s.SubCategory.Name))
            .ForMember(d => d.Features, o => o.MapFrom(s => s.AdvertisementFeatures.Select(af => af.Feature)))
            .ForMember(d => d.Owner, o => o.MapFrom(s => s.Owner))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.EffectiveStatusAt(DateTime.UtcNow)))
            .ForMember(d => d.IsExpired, o => o.MapFrom(s => s.IsExpiredAt(DateTime.UtcNow)))
            .ForMember(d => d.Views, o => o.Ignore())
            .ForMember(d => d.FavoriteCount, o => o.Ignore())
            .ForMember(d => d.IsFavorite, o => o.Ignore())
            .ForMember(d => d.CanReport, o => o.Ignore());

        CreateMap<Advertisement, CarDetailsDto>()
            .IncludeBase<Advertisement, AdvertisementDetailsDto>()
            .ForMember(d => d.ChangedParts, o => o.MapFrom(s =>
                CarCatalog.Split<ChangedVehiclePart>(s.ChangedParts)))
            .ForMember(d => d.ChangedPartNames, o => o.MapFrom(s =>
                CarCatalog.NamesOf(
                    CarCatalog.Split<ChangedVehiclePart>(s.ChangedParts),
                    CarCatalog.AllChangedPartNames)))
            .ForMember(d => d.UsageFields, o => o.MapFrom(s =>
                CarCatalog.Split<EquipmentUsageField>(s.UsageFields)))
            .ForMember(d => d.UsageFieldNames, o => o.MapFrom(s =>
                CarCatalog.NamesOf(
                    CarCatalog.Split<EquipmentUsageField>(s.UsageFields),
                    CarCatalog.UsageFieldNames)))
            .ForMember(d => d.RentSystems, o => o.MapFrom(s =>
                CarCatalog.Split<RentSystem>(s.RentSystems)))
            .ForMember(d => d.RentSystemNames, o => o.MapFrom(s =>
                CarCatalog.NamesOf(
                    CarCatalog.Split<RentSystem>(s.RentSystems),
                    CarCatalog.AllRentSystemNames)));

        CreateMap<CreateAdvertisementRequest, Advertisement>()
            .ForMember(d => d.Price, o => o.MapFrom(s => SalePriceOf(s)))
            .ForMember(d => d.ChangedParts, o => o.MapFrom(s => CarCatalog.Combine(s.ChangedParts)))
            .ForMember(d => d.UsageFields, o => o.MapFrom(s => CarCatalog.Combine(s.UsageFields)))
            .ForMember(d => d.RentSystems, o => o.MapFrom(s => CarCatalog.Combine(s.RentSystems)))
            .ForMember(d => d.VideoPath, o => o.Ignore())
            .ForMember(d => d.VideoUrl, o => o.Ignore())
            .ForMember(d => d.FirstPublishedAt, o => o.Ignore())
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.Owner, o => o.Ignore())
            .ForMember(d => d.OwnerId, o => o.Ignore())
            .ForMember(d => d.Category, o => o.Ignore())
            .ForMember(d => d.SubCategory, o => o.Ignore())
            .ForMember(d => d.Governorate, o => o.Ignore())
            .ForMember(d => d.Status, o => o.Ignore())
            .ForMember(d => d.Views, o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.PublishedAt, o => o.Ignore())
            .ForMember(d => d.ExpireAt, o => o.Ignore())
            .ForMember(d => d.ExpiredAt, o => o.Ignore())
            .ForMember(d => d.DeletedAt, o => o.Ignore())
            .ForMember(d => d.Images, o => o.Ignore())
            .ForMember(d => d.AdvertisementFeatures, o => o.Ignore())
            .ForMember(d => d.AdvertisementViews, o => o.Ignore());

        CreateMap<UpdateAdvertisementRequest, Advertisement>()
            .ForMember(d => d.Price, o => o.MapFrom(s => SalePriceOf(s)))
            .ForMember(d => d.ChangedParts, o => o.MapFrom(s => CarCatalog.Combine(s.ChangedParts)))
            .ForMember(d => d.UsageFields, o => o.MapFrom(s => CarCatalog.Combine(s.UsageFields)))
            .ForMember(d => d.RentSystems, o => o.MapFrom(s => CarCatalog.Combine(s.RentSystems)))
            .ForMember(d => d.VideoPath, o => o.Ignore())
            .ForMember(d => d.VideoUrl, o => o.Ignore())
            .ForMember(d => d.FirstPublishedAt, o => o.Ignore())
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.Owner, o => o.Ignore())
            .ForMember(d => d.OwnerId, o => o.Ignore())
            .ForMember(d => d.Category, o => o.Ignore())
            .ForMember(d => d.CategoryId, o => o.Ignore())
            .ForMember(d => d.SubCategory, o => o.Ignore())
            .ForMember(d => d.Governorate, o => o.Ignore())
            .ForMember(d => d.Status, o => o.Ignore())
            .ForMember(d => d.Views, o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.PublishedAt, o => o.Ignore())
            .ForMember(d => d.ExpireAt, o => o.Ignore())
            .ForMember(d => d.ExpiredAt, o => o.Ignore())
            .ForMember(d => d.DeletedAt, o => o.Ignore())
            .ForMember(d => d.Images, o => o.Ignore())
            .ForMember(d => d.AdvertisementFeatures, o => o.Ignore())
            .ForMember(d => d.AdvertisementViews, o => o.Ignore());

        CreateMap<Notification, Shared.DTOs.Notifications.NotificationDto>();
    }

    private static decimal? SalePriceOf(IVehicleAdvertisementFields request) =>
        AdvertisementValidationRules.IsVehicle(request.SubCategoryId) &&
        !AdvertisementValidationRules.PricesBySalePrice(request)
            ? null
            : PriceOf(request);

    private static bool IsExpired(AdvertisementCardRow row) =>
        row.Status == AdvertisementStatus.Expired ||
        (row.Status == AdvertisementStatus.Active && row.ExpireAt is { } end && end <= DateTime.UtcNow);

    private static int? RemainingDays(AdvertisementCardRow row)
    {
        if (row.ExpireAt is null)
            return null;

        if (IsExpired(row) || row.Status == AdvertisementStatus.Deleted)
            return 0;

        return ListingLifecycle.RemainingDays(row.ExpireAt, DateTime.UtcNow);
    }

    private static decimal? PriceOf(IVehicleAdvertisementFields request) =>
        request switch
        {
            CreateAdvertisementRequest create => create.Price,
            UpdateAdvertisementRequest update => update.Price,
            _ => null
        };
}
