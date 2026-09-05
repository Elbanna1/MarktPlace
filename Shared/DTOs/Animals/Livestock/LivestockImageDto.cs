namespace Shared.DTOs.Animals;

public class LivestockImageDto
{
    public Guid Id { get; set; }
    public string Url { get; set; } = default!;
    public bool IsPrimary { get; set; }
}
