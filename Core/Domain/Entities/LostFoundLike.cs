namespace Domain.Entities;

public class LostFoundLike
{
    public long Id { get; set; }

    public Guid PostId { get; set; }
    public LostFoundPost Post { get; set; } = default!;

    public string UserId { get; set; } = default!;
    public ApplicationUser User { get; set; } = default!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
