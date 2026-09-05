namespace Shared.DTOs.Lookups;

public class ProjectDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;

    public int CenterId { get; set; }
    public string CenterName { get; set; } = default!;

    public int GovernorateId { get; set; }
    public string GovernorateName { get; set; } = default!;

    public int SortOrder { get; set; }
}
