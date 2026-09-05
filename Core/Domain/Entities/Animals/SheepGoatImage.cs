namespace Domain.Entities;

public class SheepGoatImage : IListingImage
{
    public Guid Id { get; set; }

    public Guid SheepGoatId { get; set; }
    public SheepGoat SheepGoat { get; set; } = default!;

    public string FileName { get; set; } = default!;

    public string ImagePath { get; set; } = default!;

    public string ImageUrl { get; set; } = default!;

    public bool IsPrimary { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
