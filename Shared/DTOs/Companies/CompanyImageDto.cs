namespace Shared.DTOs.Companies;

public class CompanyImageDto
{
    public Guid Id { get; set; }
    public string Url { get; set; } = default!;
    public bool IsPrimary { get; set; }
}

public class CompanyFieldOptionDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;

    public string Code { get; set; } = default!;

    public string Group { get; set; } = default!;

    public string GroupAr { get; set; } = default!;
}
