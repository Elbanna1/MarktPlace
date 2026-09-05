using Microsoft.AspNetCore.Http;
using Shared.Enums;

namespace Shared.DTOs.Antiques;

public class UpdateHandmadeRequest
{
    public string SellerName { get; set; } = default!;

    public string Phone { get; set; } = default!;

    public string? WhatsApp { get; set; }

    public string ProductName { get; set; } = default!;

    public HandmadeType HandmadeType { get; set; }

    public string? OtherType { get; set; }

    public string Material { get; set; } = default!;

    public bool IsFullyHandmade { get; set; }

    public bool CustomOrder { get; set; }

    public string? ProductionTime { get; set; }

    public string? Size { get; set; }

    public List<HandmadeColor> Colors { get; set; } = new();

    public string? OtherColor { get; set; }

    public decimal Price { get; set; }

    public bool Negotiable { get; set; }

    public string Center { get; set; } = default!;

    public string Address { get; set; } = default!;

    public string? GoogleMaps { get; set; }

    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;

    public List<IFormFile> Images { get; set; } = new();

    public List<Guid> RemoveImageIds { get; set; } = new();

    public IFormFile? Video { get; set; }

    public bool RemoveVideo { get; set; }
}
