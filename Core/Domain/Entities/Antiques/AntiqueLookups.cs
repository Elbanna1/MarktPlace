namespace Domain.Entities;

public class AntiqueTypeLookup
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public string NameEn { get; set; } = default!;
}

public class AntiqueMaterialLookup
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public string NameEn { get; set; } = default!;
}

public class AntiqueConditionLookup
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public string NameEn { get; set; } = default!;
}

public class AntiqueWorkingStatusLookup
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public string NameEn { get; set; } = default!;
}

public class AntiqueOriginalityLookup
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public string NameEn { get; set; } = default!;
}
