using Shared.DTOs.RealEstate;
using Shared.Enums;

namespace Shared.Constants;

public static class BuildingCatalog
{
    public const int MaxFloors = 200;

    public const int MaxRooms = 200;

    public const int MaxUnits = 2000;

    public static readonly IReadOnlyList<RealEstateLookupEntry<BuildingType>> BuildingTypes =
        new List<RealEstateLookupEntry<BuildingType>>
        {
            new(BuildingType.Villa, "فيلا", "Villa"),
            new(BuildingType.House, "بيت", "House"),
            new(BuildingType.ApartmentBuilding, "عمارة", "Apartment building")
        };

    public static readonly IReadOnlyList<RealEstateLookupItemDto> BuildingTypeOptions =
        RealEstateCatalog.ToOptions(BuildingTypes);

    public static string? GetBuildingTypeName(BuildingType? value) =>
        RealEstateCatalog.GetNameOrNull(BuildingTypes, value);

    public static bool IsVilla(BuildingType? type) => type == BuildingType.Villa;

    public static bool IsHouse(BuildingType? type) => type == BuildingType.House;

    public static bool IsApartmentBuilding(BuildingType? type) => type == BuildingType.ApartmentBuilding;

    public static IEnumerable<ApartmentFeature> ImpliedFeatures(
        BuildingType? type, bool? hasSwimmingPool, bool? hasIndependentEntrance, bool? hasElevator, bool? hasGarage)
    {
        if (IsVilla(type) && hasSwimmingPool == true)
            yield return ApartmentFeature.SwimmingPool;

        if (IsHouse(type) && hasIndependentEntrance == true)
            yield return ApartmentFeature.PrivateEntrance;

        if (IsApartmentBuilding(type) && hasElevator == true)
            yield return ApartmentFeature.Elevator;

        if (IsApartmentBuilding(type) && hasGarage == true)
            yield return ApartmentFeature.Garage;
    }
}
