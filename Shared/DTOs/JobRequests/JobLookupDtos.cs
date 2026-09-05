namespace Shared.DTOs.JobRequests;

public class JobFieldDto
{
    public int Id { get; set; }

    public string Group { get; set; } = default!;

    public string GroupAr { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string NameEn { get; set; } = default!;
}

public class JobExperienceLevelDto
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public string NameEn { get; set; } = default!;
}

public class EducationLevelDto
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public string NameEn { get; set; } = default!;
}
