namespace Shared.Enums;

public enum LightingDecorProductType
{
    Chandelier = 1,

    TableLamp = 2,

    Spotlight = 3,

    LedStrip = 4,

    WallClock = 5,

    Mirror = 6,

    Painting = 7,

    Vase = 8,

    Candlestick = 9,

    Shelves = 10,

    WallDecor = 11,

    Other = 12
}

public enum LightingDecorMaterial
{
    Glass = 1,

    Crystal = 2,

    Metal = 3,

    Wood = 4,

    Plastic = 5,

    Ceramic = 6,

    Fabric = 7,

    Other = 8
}

public enum LightingDecorColor
{
    White = 1,

    Black = 2,

    Brown = 3,

    Beige = 4,

    Gray = 5,

    Other = 6
}

public enum LightingDecorLightType
{
    White = 1,

    Yellow = 2,

    Rgb = 3
}

public static class LightingDecorProductTypes
{
    public static readonly IReadOnlySet<LightingDecorProductType> Lighting =
        new HashSet<LightingDecorProductType>
        {
            LightingDecorProductType.Chandelier,
            LightingDecorProductType.TableLamp,
            LightingDecorProductType.Spotlight,
            LightingDecorProductType.LedStrip
        };

    public static bool IsLighting(LightingDecorProductType productType) => Lighting.Contains(productType);
}
