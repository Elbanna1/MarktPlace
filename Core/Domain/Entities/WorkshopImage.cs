namespace Domain.Entities;

public class WorkshopImage : IListingImage
{
    public Guid Id { get; set; }

    public Guid WorkshopId { get; set; }
    public Workshop Workshop { get; set; } = default!;

    public string FileName { get; set; } = default!;

    public string ImagePath { get; set; } = default!;

    public string ImageUrl { get; set; } = default!;

    public bool IsPrimary { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
