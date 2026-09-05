namespace Shared.DTOs.Workshops;

public class WorkshopImageDto
{
    public Guid Id { get; set; }
    public string Url { get; set; } = default!;
    public bool IsPrimary { get; set; }
}
