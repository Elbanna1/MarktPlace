namespace Domain.Entities;

public interface IHomeFurnishingLookup
{
    int Id { get; set; }

    string Name { get; set; }

    string NameEn { get; set; }
}

public class FurnitureTypeLookup : IHomeFurnishingLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class FurnitureMaterialLookup : IHomeFurnishingLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class FurnitureColorLookup : IHomeFurnishingLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class FurnitureConditionLookup : IHomeFurnishingLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class FurnishingCurtainProductTypeLookup : IHomeFurnishingLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class FurnishingCurtainSizeLookup : IHomeFurnishingLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class FurnishingCurtainMaterialLookup : IHomeFurnishingLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class FurnishingCurtainColorLookup : IHomeFurnishingLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class LightingDecorProductTypeLookup : IHomeFurnishingLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class LightingDecorMaterialLookup : IHomeFurnishingLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class LightingDecorColorLookup : IHomeFurnishingLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class LightingDecorLightTypeLookup : IHomeFurnishingLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class KitchenToolProductTypeLookup : IHomeFurnishingLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class KitchenToolMaterialLookup : IHomeFurnishingLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class KitchenToolColorLookup : IHomeFurnishingLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class HomeApplianceDeviceTypeLookup : IHomeFurnishingLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class HomeApplianceBrandLookup : IHomeFurnishingLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class HomeApplianceConditionLookup : IHomeFurnishingLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class HomeApplianceWarrantyLookup : IHomeFurnishingLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class HomeApplianceColorLookup : IHomeFurnishingLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class BathroomSupplyProductTypeLookup : IHomeFurnishingLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class BathroomSupplyMaterialLookup : IHomeFurnishingLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class BathroomSupplyColorLookup : IHomeFurnishingLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class PlantOrnamentProductTypeLookup : IHomeFurnishingLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class PlantOrnamentSuitableForLookup : IHomeFurnishingLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}
