namespace Domain.Entities;

public class SupplierSpecializationLookup
{
    public int Id { get; set; }

    public string Group { get; set; } = default!;

    public string GroupAr { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string NameEn { get; set; } = default!;
}
