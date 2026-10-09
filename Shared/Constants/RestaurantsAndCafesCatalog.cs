using Shared.Enums;

namespace Shared.Constants;

public static class RestaurantsAndCafesCatalog
{
    public const string CategoryName = "مطاعم وكافيهات";

    public const string Section = "بيانات المطعم أو الكافيه";

    public static readonly DirectoryModule RestaurantsAndCafes = new(
        ListingModuleType.RestaurantAndCafe, SubCategoryType.RestaurantsAndCafes,
        RestaurantsAndCafesRoutes.RestaurantsAndCafes, "restaurantsAndCafes",
        FileUploadConstants.RestaurantsAndCafesFolder, "إعلان المطعم أو الكافيه", "🍽️",
        "اسم المطعم أو الكافيه", "Restaurant or cafe name", "اكتب اسم المطعم أو الكافيه",
        Section,
        "اكتب عنوان الإعلان",
        "اكتب وصف المطعم أو الكافيه وتفاصيله",
        "قائمة المطاعم والكافيهات", "Restaurants & cafes list",
        "تفاصيل إعلان المطعم أو الكافيه", "Restaurant or cafe details",
        "إعلانات المطاعم والكافيهات المضافة بواسطتي", "My restaurant & cafe listings",
        "إعلان المطعم أو الكافيه غير موجود.",
        NameFilterIsExact: true,
        AdSection: Section,
        AddressPlaceholder: "اكتب العنوان بالتفصيل، الشارع والعلامة المميزة",
        GoogleMapsPlaceholder: "https://maps.google.com/...");

    public static DirectoryModule? ModuleOf(SubCategoryType subCategory) =>
        subCategory == SubCategoryType.RestaurantsAndCafes ? RestaurantsAndCafes : null;
}
