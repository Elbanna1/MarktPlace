using Microsoft.AspNetCore.Http;
using Shared.Enums;

namespace Shared.DTOs.LostFound;

public class CreateLostFoundPostRequest
{
    public List<IFormFile> Images { get; set; } = new();

    public PostType PostType { get; set; }

    public string Name { get; set; } = default!;

    public string ItemName { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;

    public string? Governorate { get; set; }
    public string Center { get; set; } = default!;

    public DateOnly? LostDate { get; set; }

    public DateOnly? FoundDate { get; set; }
}
