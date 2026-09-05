using Shared.Constants;
using Shared.DTOs.Lookups.Forms;

namespace Services.AdForms;

internal static class HomeFurnishingFormLookups
{
    public static bool TryApply(CreateAdFormLookupsDto lookups, string key)
    {
        switch (key)
        {
            case AdFormLookupKeys.FurnitureTypes:
                lookups.FurnitureTypes = FurnitureCatalog.FurnitureTypeOptions;
                return true;
            case AdFormLookupKeys.FurnitureMaterials:
                lookups.FurnitureMaterials = FurnitureCatalog.MaterialOptions;
                return true;
            case AdFormLookupKeys.FurnitureColors:
                lookups.FurnitureColors = FurnitureCatalog.ColorOptions;
                return true;
            case AdFormLookupKeys.FurnitureConditions:
                lookups.FurnitureConditions = FurnitureCatalog.ConditionOptions;
                return true;

            case AdFormLookupKeys.FurnishingCurtainProductTypes:
                lookups.FurnishingCurtainProductTypes = FurnishingCurtainCatalog.ProductTypeOptions;
                return true;
            case AdFormLookupKeys.FurnishingCurtainSizes:
                lookups.FurnishingCurtainSizes = FurnishingCurtainCatalog.SizeOptions;
                return true;
            case AdFormLookupKeys.FurnishingCurtainMaterials:
                lookups.FurnishingCurtainMaterials = FurnishingCurtainCatalog.MaterialOptions;
                return true;
            case AdFormLookupKeys.FurnishingCurtainColors:
                lookups.FurnishingCurtainColors = FurnishingCurtainCatalog.ColorOptions;
                return true;

            case AdFormLookupKeys.LightingDecorProductTypes:
                lookups.LightingDecorProductTypes = LightingDecorCatalog.ProductTypeOptions;
                return true;
            case AdFormLookupKeys.LightingDecorMaterials:
                lookups.LightingDecorMaterials = LightingDecorCatalog.MaterialOptions;
                return true;
            case AdFormLookupKeys.LightingDecorColors:
                lookups.LightingDecorColors = LightingDecorCatalog.ColorOptions;
                return true;
            case AdFormLookupKeys.LightingDecorLightTypes:
                lookups.LightingDecorLightTypes = LightingDecorCatalog.LightTypeOptions;
                return true;

            case AdFormLookupKeys.KitchenToolProductTypes:
                lookups.KitchenToolProductTypes = KitchenToolCatalog.ProductTypeOptions;
                return true;
            case AdFormLookupKeys.KitchenToolMaterials:
                lookups.KitchenToolMaterials = KitchenToolCatalog.MaterialOptions;
                return true;
            case AdFormLookupKeys.KitchenToolColors:
                lookups.KitchenToolColors = KitchenToolCatalog.ColorOptions;
                return true;

            case AdFormLookupKeys.HomeApplianceDeviceTypes:
                lookups.HomeApplianceDeviceTypes = HomeApplianceCatalog.DeviceTypeOptions;
                return true;
            case AdFormLookupKeys.HomeApplianceBrands:
                lookups.HomeApplianceBrands = HomeApplianceCatalog.BrandOptions;
                return true;
            case AdFormLookupKeys.HomeApplianceConditions:
                lookups.HomeApplianceConditions = HomeApplianceCatalog.ConditionOptions;
                return true;
            case AdFormLookupKeys.HomeApplianceWarranties:
                lookups.HomeApplianceWarranties = HomeApplianceCatalog.WarrantyOptions;
                return true;
            case AdFormLookupKeys.HomeApplianceColors:
                lookups.HomeApplianceColors = HomeApplianceCatalog.ColorOptions;
                return true;

            case AdFormLookupKeys.BathroomSupplyProductTypes:
                lookups.BathroomSupplyProductTypes = BathroomSupplyCatalog.ProductTypeOptions;
                return true;
            case AdFormLookupKeys.BathroomSupplyMaterials:
                lookups.BathroomSupplyMaterials = BathroomSupplyCatalog.MaterialOptions;
                return true;
            case AdFormLookupKeys.BathroomSupplyColors:
                lookups.BathroomSupplyColors = BathroomSupplyCatalog.ColorOptions;
                return true;

            case AdFormLookupKeys.PlantOrnamentProductTypes:
                lookups.PlantOrnamentProductTypes = PlantOrnamentCatalog.ProductTypeOptions;
                return true;
            case AdFormLookupKeys.PlantOrnamentSuitableFor:
                lookups.PlantOrnamentSuitableFor = PlantOrnamentCatalog.SuitableForOptions;
                return true;

            default:
                return false;
        }
    }
}
