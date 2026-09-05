using Shared.Constants;
using Shared.DTOs.Advertisements;
using Shared.DTOs.Lookups.Forms;
using Shared.Enums;

namespace Services.AdForms;

internal static class CarFormLookups
{
    public static bool TryApply(CreateAdFormLookupsDto lookups, string key)
    {
        switch (key)
        {
            case AdFormLookupKeys.CarBrands:
                lookups.CarBrands = CarCatalog.CarBrands;
                return true;
            case AdFormLookupKeys.MotorcycleBrands:
                lookups.MotorcycleBrands = CarCatalog.MotorcycleBrands;
                return true;
            case AdFormLookupKeys.EquipmentBrands:
                lookups.EquipmentBrands = CarCatalog.EquipmentBrands;
                return true;
            case AdFormLookupKeys.CarModels:
                lookups.CarModels = CarCatalog.CarModels;
                return true;

            case AdFormLookupKeys.CarColors:
                lookups.CarColors = CarCatalog.CarColors;
                return true;
            case AdFormLookupKeys.TaxiColors:
                lookups.TaxiColors = CarCatalog.TaxiColors;
                return true;
            case AdFormLookupKeys.MotorcycleColors:
                lookups.MotorcycleColors = CarCatalog.MotorcycleColors;
                return true;
            case AdFormLookupKeys.EquipmentColors:
                lookups.EquipmentColors = CarCatalog.EquipmentColors;
                return true;

            case AdFormLookupKeys.PrivateCarFeatures:
                lookups.PrivateCarFeatures = FeaturesOf(SubCategoryType.Private);
                return true;
            case AdFormLookupKeys.TaxiFeatures:
                lookups.TaxiFeatures = FeaturesOf(SubCategoryType.Taxi);
                return true;
            case AdFormLookupKeys.MotorcycleFeatures:
                lookups.MotorcycleFeatures = FeaturesOf(SubCategoryType.Motorcycles);
                return true;
            case AdFormLookupKeys.EquipmentFeatures:
                lookups.EquipmentFeatures = FeaturesOf(SubCategoryType.HeavyEquipment);
                return true;

            case AdFormLookupKeys.CarEngineCapacities:
                lookups.CarEngineCapacities = Numbers(CarCatalog.CarEngineCapacities);
                return true;
            case AdFormLookupKeys.TaxiEngineCapacities:
                lookups.TaxiEngineCapacities = Numbers(CarCatalog.TaxiEngineCapacities);
                return true;
            case AdFormLookupKeys.MotorcycleEngineCapacities:
                lookups.MotorcycleEngineCapacities = Numbers(CarCatalog.MotorcycleEngineCapacities);
                return true;
            case AdFormLookupKeys.CarDoorCounts:
                lookups.CarDoorCounts = Numbers(CarCatalog.DoorCounts);
                return true;
            case AdFormLookupKeys.CarSeatCounts:
                lookups.CarSeatCounts = Numbers(CarCatalog.SeatCounts);
                return true;
            case AdFormLookupKeys.TaxiPassengerCounts:
                lookups.TaxiPassengerCounts = Numbers(CarCatalog.PassengerCounts);
                return true;

            default:
                return false;
        }
    }

    private static IReadOnlyList<CarFeatureOptionDto> FeaturesOf(SubCategoryType subCategory) =>
        CarCatalog.FeaturesOf(subCategory)
            .Select(feature => new CarFeatureOptionDto
            {
                Id = feature.Id,
                Name = feature.Name,
                Group = feature.Group
            })
            .ToList();

    private static IReadOnlyList<CarLookupItemDto> Numbers(IEnumerable<int> values) =>
        values
            .Select(value => new CarLookupItemDto { Id = value, Name = value.ToString() })
            .ToList();
}
