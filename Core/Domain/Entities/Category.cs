namespace Domain.Entities;

public class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public string NameAr { get; set; } = default!;

    public string? Icon { get; set; }

    public bool IsActive { get; set; } = true;

    public int SortOrder { get; set; }

    public ICollection<SubCategory> SubCategories { get; set; } = new List<SubCategory>();
    public ICollection<Advertisement> Advertisements { get; set; } = new List<Advertisement>();
}
