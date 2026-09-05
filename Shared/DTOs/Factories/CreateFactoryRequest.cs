using Microsoft.AspNetCore.Http;
using Shared.Enums;

namespace Shared.DTOs.Factories;

public class CreateFactoryRequest
{
    public string FactoryName { get; set; } = default!;

    public ProductionSpecialty ProductionSpecialty { get; set; }

    public string? OtherSpecialty { get; set; }

    public string Address { get; set; } = default!;

    public string? GoogleMaps { get; set; }

    public string Phone { get; set; } = default!;

    public string WhatsApp { get; set; } = default!;

    public string? Email { get; set; }

    public List<IFormFile> Images { get; set; } = new();

    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;
}
