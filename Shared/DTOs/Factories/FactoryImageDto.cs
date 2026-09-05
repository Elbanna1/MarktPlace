namespace Shared.DTOs.Factories;

public class FactoryImageDto
{
    public Guid Id { get; set; }
    public string Url { get; set; } = default!;
    public bool IsPrimary { get; set; }
}
