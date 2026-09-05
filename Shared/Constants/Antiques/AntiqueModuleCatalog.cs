using Shared.DTOs.Antiques;
using Shared.Enums;

namespace Shared.Constants;

public static class AntiqueModuleCatalog
{
    public static readonly IReadOnlyList<AntiqueLookupEntry<AntiqueType>> Types =
        new List<AntiqueLookupEntry<AntiqueType>>
        {
            new(AntiqueType.Clock, "ساعة", "Clock"),
            new(AntiqueType.Radio, "راديو", "Radio"),
            new(AntiqueType.Telephone, "تليفون", "Telephone"),
            new(AntiqueType.Camera, "كاميرا", "Camera"),
            new(AntiqueType.SewingMachine, "ماكينة خياطة", "Sewing machine"),
            new(AntiqueType.Typewriter, "آلة كاتبة", "Typewriter"),
            new(AntiqueType.Gramophone, "جرامافون", "Gramophone"),
            new(AntiqueType.OldFurniture, "أثاث قديم", "Old furniture"),
            new(AntiqueType.Lamp, "مصباح", "Lamp"),
            new(AntiqueType.ClassicCar, "سيارة قديمة", "Classic car"),
            new(AntiqueType.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<AntiqueLookupEntry<AntiqueMaterial>> Materials =
        new List<AntiqueLookupEntry<AntiqueMaterial>>
        {
            new(AntiqueMaterial.Wood, "خشب", "Wood"),
            new(AntiqueMaterial.Copper, "نحاس", "Copper"),
            new(AntiqueMaterial.Iron, "حديد", "Iron"),
            new(AntiqueMaterial.Glass, "زجاج", "Glass"),
            new(AntiqueMaterial.Bronze, "برونز", "Bronze"),
            new(AntiqueMaterial.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<AntiqueLookupEntry<AntiqueCondition>> Conditions =
        new List<AntiqueLookupEntry<AntiqueCondition>>
        {
            new(AntiqueCondition.Excellent, "ممتازة", "Excellent"),
            new(AntiqueCondition.VeryGood, "جيدة جدًا", "Very good"),
            new(AntiqueCondition.Good, "جيدة", "Good"),
            new(AntiqueCondition.NeedsMaintenance, "تحتاج صيانة", "Needs maintenance")
        };

    public static readonly IReadOnlyList<AntiqueLookupEntry<AntiqueWorkingStatus>> WorkingStatuses =
        new List<AntiqueLookupEntry<AntiqueWorkingStatus>>
        {
            new(AntiqueWorkingStatus.Working, "يعمل", "Working"),
            new(AntiqueWorkingStatus.NotWorking, "لا يعمل", "Not working"),
            new(AntiqueWorkingStatus.PartiallyWorking, "يعمل جزئيًا", "Partially working")
        };

    public static readonly IReadOnlyList<AntiqueLookupEntry<AntiqueOriginality>> Originalities =
        new List<AntiqueLookupEntry<AntiqueOriginality>>
        {
            new(AntiqueOriginality.Original, "أصلي", "Original"),
            new(AntiqueOriginality.Replica, "نسخة", "Replica"),
            new(AntiqueOriginality.Unknown, "غير معروف", "Unknown")
        };

    public static readonly IReadOnlyList<AntiqueLookupItemDto> TypeOptions = AntiqueCatalog.ToOptions(Types);
    public static readonly IReadOnlyList<AntiqueLookupItemDto> MaterialOptions = AntiqueCatalog.ToOptions(Materials);
    public static readonly IReadOnlyList<AntiqueLookupItemDto> ConditionOptions = AntiqueCatalog.ToOptions(Conditions);
    public static readonly IReadOnlyList<AntiqueLookupItemDto> WorkingStatusOptions = AntiqueCatalog.ToOptions(WorkingStatuses);
    public static readonly IReadOnlyList<AntiqueLookupItemDto> OriginalityOptions = AntiqueCatalog.ToOptions(Originalities);

    public static string GetTypeName(AntiqueType value) => AntiqueCatalog.GetName(Types, value);
    public static string GetMaterialName(AntiqueMaterial value) => AntiqueCatalog.GetName(Materials, value);
    public static string GetConditionName(AntiqueCondition value) => AntiqueCatalog.GetName(Conditions, value);
    public static string GetWorkingStatusName(AntiqueWorkingStatus value) => AntiqueCatalog.GetName(WorkingStatuses, value);
    public static string GetOriginalityName(AntiqueOriginality value) => AntiqueCatalog.GetName(Originalities, value);
}
