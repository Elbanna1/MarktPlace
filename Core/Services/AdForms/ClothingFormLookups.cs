using Shared.Constants;
using Shared.DTOs.Lookups.Forms;

namespace Services.AdForms;

internal static class ClothingFormLookups
{
    public static bool TryApply(CreateAdFormLookupsDto lookups, string key)
    {
        switch (key)
        {
            case AdFormLookupKeys.MenClothingTypes:
                lookups.MenClothingTypes = MenClothingCatalog.ClothingTypeOptions;
                return true;
            case AdFormLookupKeys.MenClothingBrands:
                lookups.MenClothingBrands = MenClothingCatalog.BrandOptions;
                return true;
            case AdFormLookupKeys.MenClothingSizes:
                lookups.MenClothingSizes = MenClothingCatalog.SizeOptions;
                return true;
            case AdFormLookupKeys.MenClothingColors:
                lookups.MenClothingColors = MenClothingCatalog.ColorOptions;
                return true;
            case AdFormLookupKeys.MenClothingConditions:
                lookups.MenClothingConditions = MenClothingCatalog.ConditionOptions;
                return true;
            case AdFormLookupKeys.MenClothingSellingMethods:
                lookups.MenClothingSellingMethods = MenClothingCatalog.SellingMethodOptions;
                return true;

            case AdFormLookupKeys.WomenClothingTypes:
                lookups.WomenClothingTypes = WomenClothingCatalog.ClothingTypeOptions;
                return true;
            case AdFormLookupKeys.WomenClothingBrands:
                lookups.WomenClothingBrands = WomenClothingCatalog.BrandOptions;
                return true;
            case AdFormLookupKeys.WomenClothingSizes:
                lookups.WomenClothingSizes = WomenClothingCatalog.SizeOptions;
                return true;
            case AdFormLookupKeys.WomenClothingColors:
                lookups.WomenClothingColors = WomenClothingCatalog.ColorOptions;
                return true;
            case AdFormLookupKeys.WomenClothingConditions:
                lookups.WomenClothingConditions = WomenClothingCatalog.ConditionOptions;
                return true;
            case AdFormLookupKeys.WomenClothingSellingMethods:
                lookups.WomenClothingSellingMethods = WomenClothingCatalog.SellingMethodOptions;
                return true;

            case AdFormLookupKeys.KidsClothingTypes:
                lookups.KidsClothingTypes = KidsClothingCatalog.ClothingTypeOptions;
                return true;
            case AdFormLookupKeys.KidsClothingBrands:
                lookups.KidsClothingBrands = KidsClothingCatalog.BrandOptions;
                return true;
            case AdFormLookupKeys.KidsClothingSizes:
                lookups.KidsClothingSizes = KidsClothingCatalog.SizeOptions;
                return true;
            case AdFormLookupKeys.KidsClothingColors:
                lookups.KidsClothingColors = KidsClothingCatalog.ColorOptions;
                return true;
            case AdFormLookupKeys.KidsClothingConditions:
                lookups.KidsClothingConditions = KidsClothingCatalog.ConditionOptions;
                return true;
            case AdFormLookupKeys.KidsClothingSellingMethods:
                lookups.KidsClothingSellingMethods = KidsClothingCatalog.SellingMethodOptions;
                return true;

            default:
                return false;
        }
    }
}
