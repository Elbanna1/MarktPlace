using Shared.Constants;
using Shared.DTOs.Lookups.Forms;

namespace Services.AdForms;

internal static class OnlineShoppingFormLookups
{
    public static bool TryApply(CreateAdFormLookupsDto lookups, string key)
    {
        switch (key)
        {
            case AdFormLookupKeys.AccessoryTypes:
                lookups.AccessoryTypes = AccessoryCatalog.AccessoryTypeOptions;
                return true;
            case AdFormLookupKeys.AccessoryCategories:
                lookups.AccessoryCategories = AccessoryCatalog.CategoryOptions;
                return true;
            case AdFormLookupKeys.AccessoryMaterials:
                lookups.AccessoryMaterials = AccessoryCatalog.MaterialOptions;
                return true;
            case AdFormLookupKeys.AccessoryColors:
                lookups.AccessoryColors = AccessoryCatalog.ColorOptions;
                return true;

            case AdFormLookupKeys.CosmeticSections:
                lookups.CosmeticSections = CosmeticCatalog.SectionOptions;
                return true;
            case AdFormLookupKeys.CosmeticSuitableFor:
                lookups.CosmeticSuitableFor = CosmeticCatalog.SuitableForOptions;
                return true;

            case AdFormLookupKeys.HomeKitchenSections:
                lookups.HomeKitchenSections = HomeKitchenCatalog.SectionOptions;
                return true;
            case AdFormLookupKeys.HomeKitchenMaterials:
                lookups.HomeKitchenMaterials = HomeKitchenCatalog.MaterialOptions;
                return true;
            case AdFormLookupKeys.HomeKitchenColors:
                lookups.HomeKitchenColors = HomeKitchenCatalog.ColorOptions;
                return true;

            case AdFormLookupKeys.ShoppingElectronicSections:
                lookups.ShoppingElectronicSections = ShoppingElectronicCatalog.SectionOptions;
                return true;
            case AdFormLookupKeys.ShoppingElectronicCompatibilities:
                lookups.ShoppingElectronicCompatibilities = ShoppingElectronicCatalog.CompatibilityOptions;
                return true;
            case AdFormLookupKeys.ShoppingElectronicConditions:
                lookups.ShoppingElectronicConditions = ShoppingElectronicCatalog.ConditionOptions;
                return true;
            case AdFormLookupKeys.ShoppingElectronicWarranties:
                lookups.ShoppingElectronicWarranties = ShoppingElectronicCatalog.WarrantyOptions;
                return true;

            case AdFormLookupKeys.GiftToyTypes:
                lookups.GiftToyTypes = GiftToyCatalog.TypeOptions;
                return true;
            case AdFormLookupKeys.GiftToySuitableFor:
                lookups.GiftToySuitableFor = GiftToyCatalog.SuitableForOptions;
                return true;

            case AdFormLookupKeys.HomemadeFoodSections:
                lookups.HomemadeFoodSections = HomemadeFoodCatalog.SectionOptions;
                return true;
            case AdFormLookupKeys.HomemadeFoodDeliveryAreas:
                lookups.HomemadeFoodDeliveryAreas = HomemadeFoodCatalog.DeliveryAreaOptions;
                return true;

            default:
                return false;
        }
    }
}
