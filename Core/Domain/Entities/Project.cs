namespace Domain.Entities;

public class Project
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public int CenterId { get; set; }
    public Center Center { get; set; } = default!;

    public bool IsActive { get; set; } = true;

    public int SortOrder { get; set; }
}
