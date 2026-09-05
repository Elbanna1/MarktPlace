using Microsoft.AspNetCore.Http;
using Shared.Enums;

namespace Shared.DTOs.Antiques;

public class CreateAntiqueRequest
{
    public string SellerName { get; set; } = default!;

    public string Phone { get; set; } = default!;

    public string? WhatsApp { get; set; }

    public string AntiqueName { get; set; } = default!;

    public AntiqueType AntiqueType { get; set; }

    public string? OtherType { get; set; }

    public int? ManufactureYear { get; set; }

    public string? CountryOfOrigin { get; set; }

    public string? Manufacturer { get; set; }

    public AntiqueMaterial Material { get; set; }

    public string? OtherMaterial { get; set; }

    public AntiqueCondition Condition { get; set; }

    public AntiqueWorkingStatus WorkingStatus { get; set; }

    public AntiqueOriginality Originality { get; set; }

    public decimal Price { get; set; }

    public bool Negotiable { get; set; }

    public string Center { get; set; } = default!;

    public string Address { get; set; } = default!;

    public string? GoogleMaps { get; set; }

    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;

    public List<IFormFile> Images { get; set; } = new();

    public IFormFile? Video { get; set; }
}
