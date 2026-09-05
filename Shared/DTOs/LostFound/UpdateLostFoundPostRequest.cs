namespace Shared.DTOs.LostFound;

public class UpdateLostFoundPostRequest
{
    public string Name { get; set; } = default!;

    public string ItemName { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;

    public string? Center { get; set; }

    public DateOnly? LostDate { get; set; }

    public DateOnly? FoundDate { get; set; }

    public List<Guid> RemoveImageIds { get; set; } = new();
}
