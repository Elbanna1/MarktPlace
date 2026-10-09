using Domain.Entities;

namespace Persistence.Configurations;

public sealed record ListingSortIndex(string Column, bool Descending, string Suffix);

public static class ListingSortIndexCatalog
{
    private static readonly ListingSortIndex PriceAscending = new("Price", Descending: false, "PriceAsc");
    private static readonly ListingSortIndex PriceDescending = new("Price", Descending: true, "PriceDesc");

    private static readonly ListingSortIndex TotalPriceAscending =
        new("TotalPrice", Descending: false, "PriceAsc");

    private static readonly ListingSortIndex TotalPriceDescending =
        new("TotalPrice", Descending: true, "PriceDesc");

    private static readonly ListingSortIndex MostViewed =
        new("ViewCount", Descending: true, "MostViewed");

    private static readonly ListingSortIndex[] ByPrice = [PriceAscending, PriceDescending];

    private static readonly ListingSortIndex[] ByPriceAndViews =
        [PriceAscending, PriceDescending, MostViewed];

    private static readonly ListingSortIndex[] ByTotalPriceAndViews =
        [TotalPriceAscending, TotalPriceDescending, MostViewed];

    private static readonly ListingSortIndex[] ByViews = [MostViewed];

    // Only the modules whose repository actually offers the sort. Advertisements are absent on
    // purpose: AdvertisementConfiguration writes its feed indexes by hand, because that table
    // filters on Status as well as ModerationStatus and nothing else does.
    public static readonly IReadOnlyDictionary<Type, IReadOnlyList<ListingSortIndex>> ForEntity =
        new Dictionary<Type, IReadOnlyList<ListingSortIndex>>
        {
            // AntiqueSortBy — price only.
            [typeof(Antique)] = ByPrice,
            [typeof(CoinStamp)] = ByPrice,
            [typeof(DecorAntique)] = ByPrice,
            [typeof(Handmade)] = ByPrice,
            [typeof(Painting)] = ByPrice,

            // OnlineShoppingSortBy — price only.
            [typeof(Accessory)] = ByPrice,
            [typeof(Cosmetic)] = ByPrice,
            [typeof(GiftToy)] = ByPrice,
            [typeof(HomeKitchen)] = ByPrice,
            [typeof(HomemadeFood)] = ByPrice,
            [typeof(ShoppingElectronic)] = ByPrice,

            // HomeFurnishingSortBy — price and most-viewed.
            [typeof(BathroomSupply)] = ByPriceAndViews,
            [typeof(FurnishingCurtain)] = ByPriceAndViews,
            [typeof(Furniture)] = ByPriceAndViews,
            [typeof(HomeAppliance)] = ByPriceAndViews,
            [typeof(KitchenTool)] = ByPriceAndViews,
            [typeof(LightingDecor)] = ByPriceAndViews,
            [typeof(PlantOrnament)] = ByPriceAndViews,

            // RealEstateSortBy — price and most-viewed. Land sorts on TotalPrice, not Price.
            [typeof(Land)] = ByTotalPriceAndViews,
            [typeof(Apartment)] = ByPriceAndViews,
            [typeof(Shop)] = ByPriceAndViews,

            // CharitySortBy — most-viewed only.
            [typeof(Rescue)] = ByViews,
            [typeof(BloodRequest)] = ByViews,
            [typeof(AskConsult)] = ByViews,

            [typeof(Doctor)] = ByViews,
            [typeof(MedicalLaboratory)] = ByViews,
            [typeof(DentalLaboratory)] = ByViews,
            [typeof(RadiologyLaboratory)] = ByViews,
            [typeof(MedicalCenter)] = ByViews,
            [typeof(Hospital)] = ByViews
        };
}
