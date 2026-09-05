namespace Domain.Entities;

public class CraftsmanImage : IListingImage
{
    public Guid Id { get; set; }

    public Guid CraftsmanId { get; set; }
    public Craftsman Craftsman { get; set; } = default!;

    public string FileName { get; set; } = default!;

    public string ImagePath { get; set; } = default!;

    public string ImageUrl { get; set; } = default!;

    public bool IsPrimary { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
