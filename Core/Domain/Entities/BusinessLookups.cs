namespace Domain.Entities;

public class ProductionSpecialtyLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
}

public class FarmTypeLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;

    public string Group { get; set; } = default!;

    public string GroupAr { get; set; } = default!;
}

public class AvailabilitySeasonLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
}

public class FarmingMethodLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;

    public string Code { get; set; } = default!;
}

public class CompanyFieldLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;

    public string Code { get; set; } = default!;

    public string Group { get; set; } = default!;

    public string GroupAr { get; set; } = default!;
}

public class SupplierTypeLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;

    public string Group { get; set; } = default!;

    public string GroupAr { get; set; } = default!;
}

public class TradeTypeLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;

    public string Group { get; set; } = default!;

    public string GroupAr { get; set; } = default!;
}

public class SaleTypeLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;

    public string Code { get; set; } = default!;
}
