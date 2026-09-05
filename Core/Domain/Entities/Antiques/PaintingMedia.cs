namespace Domain.Entities;

public class PaintingImage : IListingImage
{
    public Guid Id { get; set; }

    public Guid PaintingId { get; set; }
    public Painting Painting { get; set; } = default!;

    public string FileName { get; set; } = default!;

    public string ImagePath { get; set; } = default!;

    public string ImageUrl { get; set; } = default!;

    public bool IsPrimary { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class PaintingVideo : IListingVideo
{
    public Guid Id { get; set; }

    public Guid PaintingId { get; set; }
    public Painting Painting { get; set; } = default!;

    public string FileName { get; set; } = default!;

    public string VideoPath { get; set; } = default!;

    public string VideoUrl { get; set; } = default!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
