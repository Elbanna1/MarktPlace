namespace Domain.Entities;

public class AntiqueImage : IListingImage
{
    public Guid Id { get; set; }

    public Guid AntiqueId { get; set; }
    public Antique Antique { get; set; } = default!;

    public string FileName { get; set; } = default!;

    public string ImagePath { get; set; } = default!;

    public string ImageUrl { get; set; } = default!;

    public bool IsPrimary { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class AntiqueVideo : IListingVideo
{
    public Guid Id { get; set; }

    public Guid AntiqueId { get; set; }
    public Antique Antique { get; set; } = default!;

    public string FileName { get; set; } = default!;

    public string VideoPath { get; set; } = default!;

    public string VideoUrl { get; set; } = default!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
