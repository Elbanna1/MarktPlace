namespace Shared.DTOs.Suppliers;

public class SupplierImageDto
{
    public Guid Id { get; set; }
    public string Url { get; set; } = default!;
    public bool IsPrimary { get; set; }
}
