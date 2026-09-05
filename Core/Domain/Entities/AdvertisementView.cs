namespace Domain.Entities;

public class AdvertisementView
{
    public long Id { get; set; }

    public Guid AdvertisementId { get; set; }
    public Advertisement Advertisement { get; set; } = default!;

    public string? ViewerUserId { get; set; }

    public string? IpAddress { get; set; }

    public DateTime ViewedAt { get; set; } = DateTime.UtcNow;
}
