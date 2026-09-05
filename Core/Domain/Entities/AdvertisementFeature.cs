namespace Domain.Entities;

public class AdvertisementFeature
{
    public Guid AdvertisementId { get; set; }
    public Advertisement Advertisement { get; set; } = default!;

    public int FeatureId { get; set; }
    public Feature Feature { get; set; } = default!;
}
