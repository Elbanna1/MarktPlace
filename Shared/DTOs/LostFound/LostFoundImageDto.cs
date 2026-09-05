namespace Shared.DTOs.LostFound;

public class LostFoundImageDto
{
    public Guid Id { get; set; }
    public string Url { get; set; } = default!;
    public bool IsPrimary { get; set; }
}
