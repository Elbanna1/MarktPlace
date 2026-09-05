namespace Shared.DTOs.Lookups;

public class ProductionSpecialtyDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
}

public class FarmTypeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;

    public string Group { get; set; } = default!;

    public string GroupAr { get; set; } = default!;
}

public class AvailabilitySeasonDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
}

public class FarmingMethodDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;

    public string Code { get; set; } = default!;
}

public class CompanyFieldDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;

    public string Code { get; set; } = default!;

    public string Group { get; set; } = default!;

    public string GroupAr { get; set; } = default!;
}

public class SupplierTypeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;

    public string Group { get; set; } = default!;

    public string GroupAr { get; set; } = default!;
}

public class TradeTypeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;

    public string Group { get; set; } = default!;

    public string GroupAr { get; set; } = default!;
}

public class SaleTypeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;

    public string Code { get; set; } = default!;
}
