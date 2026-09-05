using Shared.DTOs.OnlineShopping;
using Shared.Enums;

namespace Shared.Constants;

public static class HomemadeFoodCatalog
{
    public static readonly IReadOnlyList<OnlineShoppingLookupEntry<HomemadeFoodSection>> Sections =
        new List<OnlineShoppingLookupEntry<HomemadeFoodSection>>
        {
            new(HomemadeFoodSection.Sweets, "حلويات", "Sweets"),
            new(HomemadeFoodSection.Cakes, "تورت", "Cakes"),
            new(HomemadeFoodSection.Bakery, "مخبوزات", "Bakery"),
            new(HomemadeFoodSection.OrientalDishes, "أكلات شرقية", "Oriental dishes"),
            new(HomemadeFoodSection.WesternDishes, "أكلات غربية", "Western dishes"),
            new(HomemadeFoodSection.Beverages, "مشروبات", "Beverages"),
            new(HomemadeFoodSection.Sandwiches, "سندوتشات", "Sandwiches"),
            new(HomemadeFoodSection.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<OnlineShoppingLookupEntry<HomemadeFoodDeliveryArea>> DeliveryAreas =
        new List<OnlineShoppingLookupEntry<HomemadeFoodDeliveryArea>>
        {
            new(HomemadeFoodDeliveryArea.Fayoum, "الفيوم", "Fayoum"),
            new(HomemadeFoodDeliveryArea.Sinnuris, "سنورس", "Sinnuris"),
            new(HomemadeFoodDeliveryArea.Tamiya, "طامية", "Tamiya"),
            new(HomemadeFoodDeliveryArea.Itsa, "إطسا", "Itsa"),
            new(HomemadeFoodDeliveryArea.Ibshaway, "أبشواي", "Ibshaway"),
            new(HomemadeFoodDeliveryArea.YoussefElSeddik, "يوسف الصديق", "Youssef El Seddik")
        };

    public static readonly IReadOnlyList<string> PreparationTimeExamples =
        new List<string> { "ساعتين", "6 ساعات", "يوم", "يومين" };

    public static readonly IReadOnlyList<OnlineShoppingLookupItemDto> SectionOptions =
        OnlineShoppingCatalog.ToOptions(Sections);

    public static readonly IReadOnlyList<OnlineShoppingLookupItemDto> DeliveryAreaOptions =
        OnlineShoppingCatalog.ToOptions(DeliveryAreas);

    public static string GetSectionName(HomemadeFoodSection value) =>
        OnlineShoppingCatalog.GetName(Sections, value);

    public static string GetDeliveryAreaName(HomemadeFoodDeliveryArea value) =>
        OnlineShoppingCatalog.GetName(DeliveryAreas, value);

    public static string GetDeliveryAreaNameEn(HomemadeFoodDeliveryArea value) =>
        OnlineShoppingCatalog.GetNameEn(DeliveryAreas, value);

    public static List<OnlineShoppingLookupItemDto> SelectedDeliveryAreas(
        IEnumerable<HomemadeFoodDeliveryArea> selected) =>
        OnlineShoppingCatalog.SelectedOptions(DeliveryAreas, selected);
}
