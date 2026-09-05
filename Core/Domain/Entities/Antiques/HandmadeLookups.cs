namespace Domain.Entities;

public class HandmadeTypeLookup
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public string NameEn { get; set; } = default!;
}

public class HandmadeColorLookup
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public string NameEn { get; set; } = default!;
}
