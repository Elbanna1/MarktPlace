using Microsoft.AspNetCore.Http;
using Shared.Enums;

namespace Shared.DTOs.Farms;

public class CreateFarmRequest
{
    public string FarmName { get; set; } = default!;

    public FarmType FarmType { get; set; }

    public string? OtherFarmType { get; set; }

    public string Address { get; set; } = default!;

    public string? GoogleMaps { get; set; }

    public string Phone { get; set; } = default!;

    public string WhatsApp { get; set; } = default!;

    public string? Email { get; set; }

    public decimal? AreaInFeddan { get; set; }

    public string? AvailableQuantity { get; set; }

    public AvailabilitySeason? AvailabilitySeason { get; set; }

    public FarmingMethod? FarmingMethod { get; set; }

    public List<IFormFile> Images { get; set; } = new();

    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;
}
