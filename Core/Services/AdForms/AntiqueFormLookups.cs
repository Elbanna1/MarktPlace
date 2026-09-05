using Shared.Constants;
using Shared.DTOs.Lookups.Forms;

namespace Services.AdForms;

internal static class AntiqueFormLookups
{
    public static bool TryApply(CreateAdFormLookupsDto lookups, string key)
    {
        switch (key)
        {
            case AdFormLookupKeys.DecorAntiqueItemTypes:
                lookups.DecorAntiqueItemTypes = DecorAntiqueCatalog.ItemTypeOptions;
                return true;
            case AdFormLookupKeys.DecorAntiqueMaterials:
                lookups.DecorAntiqueMaterials = DecorAntiqueCatalog.MaterialOptions;
                return true;
            case AdFormLookupKeys.DecorAntiqueConditions:
                lookups.DecorAntiqueConditions = DecorAntiqueCatalog.ConditionOptions;
                return true;
            case AdFormLookupKeys.DecorAntiqueOriginalities:
                lookups.DecorAntiqueOriginalities = DecorAntiqueCatalog.OriginalityOptions;
                return true;

            case AdFormLookupKeys.AntiqueTypes:
                lookups.AntiqueTypes = AntiqueModuleCatalog.TypeOptions;
                return true;
            case AdFormLookupKeys.AntiqueMaterials:
                lookups.AntiqueMaterials = AntiqueModuleCatalog.MaterialOptions;
                return true;
            case AdFormLookupKeys.AntiqueConditions:
                lookups.AntiqueConditions = AntiqueModuleCatalog.ConditionOptions;
                return true;
            case AdFormLookupKeys.AntiqueWorkingStatuses:
                lookups.AntiqueWorkingStatuses = AntiqueModuleCatalog.WorkingStatusOptions;
                return true;
            case AdFormLookupKeys.AntiqueOriginalities:
                lookups.AntiqueOriginalities = AntiqueModuleCatalog.OriginalityOptions;
                return true;

            case AdFormLookupKeys.PaintingTypes:
                lookups.PaintingTypes = PaintingCatalog.TypeOptions;
                return true;
            case AdFormLookupKeys.PaintingMaterials:
                lookups.PaintingMaterials = PaintingCatalog.MaterialOptions;
                return true;
            case AdFormLookupKeys.PaintingOriginalities:
                lookups.PaintingOriginalities = PaintingCatalog.OriginalityOptions;
                return true;

            case AdFormLookupKeys.HandmadeTypes:
                lookups.HandmadeTypes = HandmadeCatalog.TypeOptions;
                return true;
            case AdFormLookupKeys.HandmadeColors:
                lookups.HandmadeColors = HandmadeCatalog.ColorOptions;
                return true;

            case AdFormLookupKeys.CoinStampItemTypes:
                lookups.CoinStampItemTypes = CoinStampCatalog.ItemTypeOptions;
                return true;
            case AdFormLookupKeys.CoinStampMetals:
                lookups.CoinStampMetals = CoinStampCatalog.MetalOptions;
                return true;
            case AdFormLookupKeys.CoinStampConditions:
                lookups.CoinStampConditions = CoinStampCatalog.ConditionOptions;
                return true;

            default:
                return false;
        }
    }
}
