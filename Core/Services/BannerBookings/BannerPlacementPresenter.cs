using System.Globalization;
using Domain.Entities;
using Shared.Constants;
using Shared.DTOs.BannerBookings;
using Shared.Enums;

namespace Services.BannerBookings;

public static class BannerPlacementPresenter
{
    public static BannerPlacementDto ToDto(BannerPlacementSetting placement)
    {
        var requiresSubCategory = placement.Location == BannerLocation.SubCategoryBanner;

        return new BannerPlacementDto
        {
            Id = placement.Id,
            Location = placement.Location,
            LocationName = BannerBookingCatalog.GetLocationName(placement.Location),

            Price = placement.Price,
            Currency = BannerBookingCatalog.DefaultCurrency,
            PriceDisplay = FormatPricePerRun(placement.Price, placement.DurationDays),

            MaxSlots = requiresSubCategory ? 1 : placement.MaxSlots,

            DurationDays = placement.DurationDays,
            DurationDisplay = BannerBookingCatalog.FormatDuration(placement.DurationDays),

            RequiresSubCategory = requiresSubCategory,
            IsActive = placement.IsActive,
            DisplayOrder = placement.DisplayOrder,

            Desktop = BannerImageRules.ToSpec(
                BannerImageRules.DesktopRequirement(placement), BannerBookingCatalog.DesktopSafeAreaNote),

            Mobile = BannerImageRules.ToSpec(BannerImageRules.MobileRequirement(placement)),

            ImageUsageNote = BannerBookingCatalog.ImageUsageNote
        };
    }

    public static string FormatPricePerRun(decimal price, int durationDays) =>
        $"{FormatPrice(price)} / {BannerBookingCatalog.FormatDuration(durationDays)}";

    public static string FormatPrice(decimal price)
    {
        var amount = price == decimal.Truncate(price)
            ? price.ToString("#,##0", CultureInfo.InvariantCulture)
            : price.ToString("#,##0.00", CultureInfo.InvariantCulture);

        return $"{amount} جنيه";
    }

    public static string FormatPlacement(
        BannerLocation location, int slotNumber, string? categoryName, string? subCategoryName)
    {
        var name = BannerBookingCatalog.GetLocationName(location);

        if (location == BannerLocation.SubCategoryBanner)
        {
            var path = string.Join(" — ", new[] { categoryName, subCategoryName }.Where(part => !string.IsNullOrWhiteSpace(part)));
            return string.IsNullOrWhiteSpace(path) ? name : $"{name} — {path}";
        }

        return $"{name} — Slot {slotNumber}";
    }
}
