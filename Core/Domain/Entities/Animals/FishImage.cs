namespace Domain.Entities;

public class FishImage : IListingImage
{
    public Guid Id { get; set; }

    public Guid FishId { get; set; }
    public Fish Fish { get; set; } = default!;

    public string FileName { get; set; } = default!;

    public string ImagePath { get; set; } = default!;

    public string ImageUrl { get; set; } = default!;

    public bool IsPrimary { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
