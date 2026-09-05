namespace Shared.DTOs.Lookups;

public class SubCategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string NameAr { get; set; } = default!;
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = default!;

    public string? Icon { get; set; }

    public int SortOrder { get; set; }
}
