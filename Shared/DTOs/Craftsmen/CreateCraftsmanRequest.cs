using Shared.Enums;

namespace Shared.DTOs.Craftsmen;

public class CreateCraftsmanRequest
{
    public string Name { get; set; } = default!;

    public CraftsmanSpecialization Specialization { get; set; }

    public string? OtherSpecialization { get; set; }

    public ExperienceLevel ExperienceLevel { get; set; }

    public string? Governorate { get; set; }

    public string Center { get; set; } = default!;

    public string Address { get; set; } = default!;

    public string? GoogleMapsUrl { get; set; }

    public string PhoneNumber { get; set; } = default!;

    public string WhatsApp { get; set; } = default!;

    public string? Email { get; set; }

    public string AdTitle { get; set; } = default!;

    public string AdDescription { get; set; } = default!;
}
