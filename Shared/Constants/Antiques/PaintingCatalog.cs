using Shared.DTOs.Antiques;
using Shared.Enums;

namespace Shared.Constants;

public static class PaintingCatalog
{
    public static readonly IReadOnlyList<AntiqueLookupEntry<PaintingType>> Types =
        new List<AntiqueLookupEntry<PaintingType>>
        {
            new(PaintingType.Oil, "زيتية", "Oil"),
            new(PaintingType.Acrylic, "أكريليك", "Acrylic"),
            new(PaintingType.Watercolor, "مائية", "Watercolor"),
            new(PaintingType.Charcoal, "فحم", "Charcoal"),
            new(PaintingType.Pencil, "رصاص", "Pencil"),
            new(PaintingType.Pastel, "باستيل", "Pastel"),
            new(PaintingType.ArabicCalligraphy, "خط عربي", "Arabic calligraphy"),
            new(PaintingType.PrintedDigitalArt, "فن رقمي مطبوع", "Printed digital art"),
            new(PaintingType.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<AntiqueLookupEntry<PaintingMaterial>> Materials =
        new List<AntiqueLookupEntry<PaintingMaterial>>
        {
            new(PaintingMaterial.Canvas, "كانفاس", "Canvas"),
            new(PaintingMaterial.Wood, "خشب", "Wood"),
            new(PaintingMaterial.Paper, "ورق", "Paper"),
            new(PaintingMaterial.Fabric, "قماش", "Fabric"),
            new(PaintingMaterial.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<AntiqueLookupEntry<PaintingOriginality>> Originalities =
        new List<AntiqueLookupEntry<PaintingOriginality>>
        {
            new(PaintingOriginality.Original, "أصلية", "Original"),
            new(PaintingOriginality.Printed, "مطبوعة", "Printed")
        };

    public static readonly IReadOnlyList<AntiqueLookupItemDto> TypeOptions = AntiqueCatalog.ToOptions(Types);
    public static readonly IReadOnlyList<AntiqueLookupItemDto> MaterialOptions = AntiqueCatalog.ToOptions(Materials);
    public static readonly IReadOnlyList<AntiqueLookupItemDto> OriginalityOptions = AntiqueCatalog.ToOptions(Originalities);

    public static string GetTypeName(PaintingType value) => AntiqueCatalog.GetName(Types, value);
    public static string GetMaterialName(PaintingMaterial value) => AntiqueCatalog.GetName(Materials, value);
    public static string GetOriginalityName(PaintingOriginality value) => AntiqueCatalog.GetName(Originalities, value);
}
