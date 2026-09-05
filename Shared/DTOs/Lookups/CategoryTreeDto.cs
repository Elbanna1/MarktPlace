namespace Shared.DTOs.Lookups;

public class CategoryTreeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameAr { get; set; } = default!;

    public string? Icon { get; set; }

    public int SortOrder { get; set; }

    public List<CategoryTreeSubCategoryDto> SubCategories { get; set; } = new();
}

public class CategoryTreeSubCategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameAr { get; set; } = default!;

    public string? Icon { get; set; }

    public int SortOrder { get; set; }
}
