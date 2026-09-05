namespace Shared.DTOs.Lookups;

public class CenterDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public int GovernorateId { get; set; }
    public string GovernorateName { get; set; } = default!;

    public int SortOrder { get; set; }

    public int ProjectsCount { get; set; }
}
