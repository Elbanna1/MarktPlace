namespace Shared.DTOs.Advertisements;

public class AdvertisementImageDto
{
    public Guid Id { get; set; }
    public string Url { get; set; } = default!;
    public bool IsPrimary { get; set; }
}
