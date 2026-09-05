namespace Domain.Entities;

public class Center
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public int GovernorateId { get; set; }
    public Governorate Governorate { get; set; } = default!;

    public bool IsActive { get; set; } = true;

    public int SortOrder { get; set; }

    public ICollection<Project> Projects { get; set; } = new List<Project>();
}
