using Shared.DTOs.HomeFurnishing;
using Shared.Enums;

namespace Shared.Constants;

public static class FurnitureCatalog
{
    public static readonly IReadOnlyList<HomeFurnishingLookupEntry<FurnitureType>> FurnitureTypes =
        new List<HomeFurnishingLookupEntry<FurnitureType>>
        {
            new(FurnitureType.BedroomSet, "غرفة نوم", "Bedroom set"),
            new(FurnitureType.Entree, "انتريه", "Entree"),
            new(FurnitureType.CornerSofa, "ركنة", "Corner sofa"),
            new(FurnitureType.Salon, "صالون", "Salon"),
            new(FurnitureType.DiningRoom, "غرفة سفرة", "Dining room"),
            new(FurnitureType.Sofa, "كنبة", "Sofa"),
            new(FurnitureType.Chair, "كرسي", "Chair"),
            new(FurnitureType.Table, "ترابيزة", "Table"),
            new(FurnitureType.Desk, "مكتب", "Desk"),
            new(FurnitureType.Bookcase, "مكتبة", "Bookcase"),
            new(FurnitureType.Wardrobe, "دولاب", "Wardrobe"),
            new(FurnitureType.Commode, "كومود", "Commode"),
            new(FurnitureType.DressingTable, "تسريحة", "Dressing table"),
            new(FurnitureType.TvUnit, "وحدة تلفزيون", "TV unit"),
            new(FurnitureType.Buffet, "بوفيه", "Buffet"),
            new(FurnitureType.Bed, "سرير", "Bed"),
            new(FurnitureType.KidsBed, "سرير أطفال", "Kids bed"),
            new(FurnitureType.OfficeChair, "كرسي مكتب", "Office chair"),
            new(FurnitureType.GamingChair, "كرسي جيمنج", "Gaming chair"),
            new(FurnitureType.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<HomeFurnishingLookupEntry<FurnitureMaterial>> Materials =
        new List<HomeFurnishingLookupEntry<FurnitureMaterial>>
        {
            new(FurnitureMaterial.Beech, "زان", "Beech"),
            new(FurnitureMaterial.Moski, "موسكي", "Moski pine"),
            new(FurnitureMaterial.Mdf, "MDF", "MDF"),
            new(FurnitureMaterial.Plywood, "كونتر", "Plywood"),
            new(FurnitureMaterial.Metal, "معدن", "Metal"),
            new(FurnitureMaterial.Plastic, "بلاستيك", "Plastic"),
            new(FurnitureMaterial.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<HomeFurnishingLookupEntry<FurnitureColor>> Colors =
        new List<HomeFurnishingLookupEntry<FurnitureColor>>
        {
            new(FurnitureColor.White, "أبيض", "White"),
            new(FurnitureColor.Black, "أسود", "Black"),
            new(FurnitureColor.Brown, "بني", "Brown"),
            new(FurnitureColor.Beige, "بيج", "Beige"),
            new(FurnitureColor.Gray, "رمادي", "Gray"),
            new(FurnitureColor.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<HomeFurnishingLookupEntry<FurnitureCondition>> Conditions =
        new List<HomeFurnishingLookupEntry<FurnitureCondition>>
        {
            new(FurnitureCondition.New, "جديد", "New"),
            new(FurnitureCondition.UsedExcellent, "مستعمل بحالة ممتازة", "Used - excellent condition"),
            new(FurnitureCondition.UsedGood, "مستعمل بحالة جيدة", "Used - good condition")
        };

    public static readonly IReadOnlyList<HomeFurnishingLookupItemDto> FurnitureTypeOptions =
        HomeFurnishingCatalog.ToOptions(FurnitureTypes);
    public static readonly IReadOnlyList<HomeFurnishingLookupItemDto> MaterialOptions =
        HomeFurnishingCatalog.ToOptions(Materials);
    public static readonly IReadOnlyList<HomeFurnishingLookupItemDto> ColorOptions =
        HomeFurnishingCatalog.ToOptions(Colors);
    public static readonly IReadOnlyList<HomeFurnishingLookupItemDto> ConditionOptions =
        HomeFurnishingCatalog.ToOptions(Conditions);

    public static string GetFurnitureTypeName(FurnitureType value) =>
        HomeFurnishingCatalog.GetName(FurnitureTypes, value);
    public static string GetMaterialName(FurnitureMaterial value) =>
        HomeFurnishingCatalog.GetName(Materials, value);
    public static string GetConditionName(FurnitureCondition value) =>
        HomeFurnishingCatalog.GetName(Conditions, value);

    public static string GetColorName(FurnitureColor value) =>
        HomeFurnishingCatalog.GetName(Colors, value);
    public static string GetColorNameEn(FurnitureColor value) =>
        HomeFurnishingCatalog.GetNameEn(Colors, value);

    public static List<HomeFurnishingLookupItemDto> SelectedColors(IEnumerable<FurnitureColor> selected) =>
        HomeFurnishingCatalog.SelectedOptions(Colors, selected);
}
