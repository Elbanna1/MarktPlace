namespace Domain.Entities;

public class Governorate
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public bool IsActive { get; set; } = true;

    public int SortOrder { get; set; }

    public ICollection<Center> Centers { get; set; } = new List<Center>();
}
