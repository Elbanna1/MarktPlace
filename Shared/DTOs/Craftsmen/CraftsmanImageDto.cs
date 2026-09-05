namespace Shared.DTOs.Craftsmen;

public class CraftsmanImageDto
{
    public Guid Id { get; set; }
    public string Url { get; set; } = default!;
    public bool IsPrimary { get; set; }
}
