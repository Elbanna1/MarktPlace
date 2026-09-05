using Shared.Enums;

namespace Shared.DTOs.Workshops;

public class UpdateWorkshopRequest
{
    public string Name { get; set; } = default!;

    public WorkshopType WorkshopType { get; set; }

    public string? OtherWorkshopType { get; set; }

    public string Center { get; set; } = default!;

    public string Address { get; set; } = default!;

    public string? GoogleMapsUrl { get; set; }

    public string PhoneNumber { get; set; } = default!;

    public string WhatsApp { get; set; } = default!;

    public string? Email { get; set; }

    public string AdTitle { get; set; } = default!;

    public string AdDescription { get; set; } = default!;

    public List<Guid> RemoveImageIds { get; set; } = new();
}
