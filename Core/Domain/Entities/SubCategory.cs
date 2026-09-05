namespace Domain.Entities;

public class SubCategory
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public string NameAr { get; set; } = default!;

    public string? Icon { get; set; }

    public bool IsActive { get; set; } = true;

    public int SortOrder { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; } = default!;

    public ICollection<Advertisement> Advertisements { get; set; } = new List<Advertisement>();
}
