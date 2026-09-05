using Shared.Enums;

namespace Domain.Entities;

public class Feature
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;

    public string Group { get; set; } = default!;

    public int Scope { get; set; }

    public ICollection<AdvertisementFeature> AdvertisementFeatures { get; set; } = new List<AdvertisementFeature>();
}
