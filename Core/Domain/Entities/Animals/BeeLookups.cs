namespace Domain.Entities;

public class BeeTypeLookup
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public string NameEn { get; set; } = default!;
}

public class BeePurposeLookup
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public string NameEn { get; set; } = default!;
}

public class BeeHealthStatusLookup
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public string NameEn { get; set; } = default!;
}

public class BeeProductionLookup
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public string NameEn { get; set; } = default!;
}
