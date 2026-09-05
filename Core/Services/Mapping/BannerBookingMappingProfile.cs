using AutoMapper;
using Domain.Entities;
using Services.BannerBookings;
using Shared.Constants;
using Shared.DTOs.BannerBookings;
using Shared.Enums;

namespace Services.Mapping;

public class BannerBookingMappingProfile : Profile
{
    public BannerBookingMappingProfile()
    {
        CreateMap<BannerBooking, BannerBookingListItemDto>()
            .ForMember(d => d.LocationName, o => o.MapFrom(s => BannerBookingCatalog.GetLocationName(s.Location)))
            .ForMember(d => d.StatusName, o => o.MapFrom(s => BannerBookingCatalog.GetStatusName(s.Status)))
            .ForMember(d => d.PaymentStatusName,
                o => o.MapFrom(s => BannerBookingCatalog.GetPaymentStatusName(s.PaymentStatus)))
            .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category != null ? s.Category.NameAr : null))
            .ForMember(d => d.SubCategoryName, o => o.MapFrom(s => s.SubCategory != null ? s.SubCategory.NameAr : null))
            .ForMember(d => d.IsLive, o => o.Ignore());

        CreateMap<BannerBooking, PublishedBannerDto>()
            .ForMember(d => d.LocationName, o => o.MapFrom(s => BannerBookingCatalog.GetLocationName(s.Location)))
            .ForMember(d => d.SlotName, o => o.MapFrom(s => BannerBookingCatalog.GetSlotName(s.SlotNumber)))
            .ForMember(d => d.StatusName, o => o.MapFrom(s => BannerBookingCatalog.GetStatusName(s.Status)));

        CreateMap<BannerBooking, BannerBookingDto>()
            .ForMember(d => d.LocationName, o => o.MapFrom(s => BannerBookingCatalog.GetLocationName(s.Location)))
            .ForMember(d => d.StatusName, o => o.MapFrom(s => BannerBookingCatalog.GetStatusName(s.Status)))
            .ForMember(d => d.PaymentStatusName,
                o => o.MapFrom(s => BannerBookingCatalog.GetPaymentStatusName(s.PaymentStatus)))
            .ForMember(d => d.RejectionReasonName, o => o.MapFrom(s =>
                s.RejectionReason.HasValue
                    ? BannerBookingCatalog.GetRejectionReasonName(s.RejectionReason.Value)
                    : null))
            .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category != null ? s.Category.NameAr : null))
            .ForMember(d => d.SubCategoryName, o => o.MapFrom(s => s.SubCategory != null ? s.SubCategory.NameAr : null))
            .ForMember(d => d.DurationDays, o => o.MapFrom(s => s.DurationDays))
            .ForMember(d => d.DurationDisplay, o => o.MapFrom(s => BannerBookingCatalog.FormatDuration(s.DurationDays)))
            .ForMember(d => d.PriceDisplay, o => o.MapFrom(s => BannerPlacementPresenter.FormatPrice(s.Price)))
            .ForMember(d => d.IsLive, o => o.Ignore())
            .ForMember(d => d.Preview, o => o.Ignore())
            .ForMember(d => d.Summary, o => o.Ignore())
            .AfterMap((booking, dto) =>
            {
                dto.Preview = BuildPreview(booking);
                dto.Summary = BuildSummary(booking, dto.CategoryName, dto.SubCategoryName, dto.PaymentMethod?.Name);
            });
    }

    private static BannerPreviewDto BuildPreview(BannerBooking booking) => new()
    {
        Desktop = new BannerImageDto
        {
            Kind = BannerImageKind.Desktop,
            KindName = BannerBookingCatalog.GetImageKindName(BannerImageKind.Desktop),
            Url = booking.DesktopImageUrl,
            Width = booking.DesktopImageWidth,
            Height = booking.DesktopImageHeight,
            Resolution = $"{booking.DesktopImageWidth} × {booking.DesktopImageHeight} px"
        },
        Mobile = new BannerImageDto
        {
            Kind = BannerImageKind.Mobile,
            KindName = BannerBookingCatalog.GetImageKindName(BannerImageKind.Mobile),
            Url = booking.MobileImageUrl,
            Width = booking.MobileImageWidth,
            Height = booking.MobileImageHeight,
            Resolution = $"{booking.MobileImageWidth} × {booking.MobileImageHeight} px"
        },
        UsageNote = BannerBookingCatalog.ImageUsageNote
    };

    private static BannerBookingSummaryDto BuildSummary(
        BannerBooking booking, string? categoryName, string? subCategoryName, string? paymentMethodName) => new()
    {
        Placement = BannerPlacementPresenter.FormatPlacement(
            booking.Location, booking.SlotNumber, categoryName, subCategoryName),
        Location = booking.Location,
        LocationName = BannerBookingCatalog.GetLocationName(booking.Location),
        SlotNumber = booking.SlotNumber,
        CategoryName = categoryName,
        SubCategoryName = subCategoryName,

        DurationDays = booking.DurationDays,
        DurationDisplay = BannerBookingCatalog.FormatDuration(booking.DurationDays),

        Price = booking.Price,
        Currency = booking.Currency,
        PriceDisplay = BannerPlacementPresenter.FormatPrice(booking.Price),

        PaymentMethodName = paymentMethodName,
        PaymentProofUploaded = !string.IsNullOrWhiteSpace(booking.PaymentProofUrl),

        StartDate = booking.StartDate,
        EndDate = booking.EndDate
    };
}
