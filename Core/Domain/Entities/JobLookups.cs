namespace Domain.Entities;

public class JobFieldLookup
{
    public int Id { get; set; }

    public string Group { get; set; } = default!;

    public string GroupAr { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string NameEn { get; set; } = default!;
}

public class JobExperienceLevelLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class EducationLevelLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class WorkTypeLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}

public class SalaryTypeLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}
