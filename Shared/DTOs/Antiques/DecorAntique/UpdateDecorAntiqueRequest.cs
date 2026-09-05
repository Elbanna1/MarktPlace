using Microsoft.AspNetCore.Http;
using Shared.Enums;

namespace Shared.DTOs.Antiques;

public class UpdateDecorAntiqueRequest
{
    public string SellerName { get; set; } = default!;

    public string Phone { get; set; } = default!;

    public string? WhatsApp { get; set; }

    public string ItemName { get; set; } = default!;

    public DecorAntiqueItemType ItemType { get; set; }

    public string? OtherItemType { get; set; }

    public DecorAntiqueMaterial Material { get; set; }

    public string? OtherMaterial { get; set; }

    public DecorAntiqueCondition Condition { get; set; }

    public decimal? Length { get; set; }

    public decimal? Width { get; set; }

    public decimal? Height { get; set; }

    public decimal? Weight { get; set; }

    public DecorAntiqueOriginality Originality { get; set; }

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
