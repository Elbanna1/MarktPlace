using Microsoft.AspNetCore.Http;
using Shared.Enums;

namespace Shared.DTOs.Antiques;

public class UpdateCoinStampRequest
{
    public string SellerName { get; set; } = default!;

    public string Phone { get; set; } = default!;

    public string? WhatsApp { get; set; }

    public string ItemName { get; set; } = default!;

    public CoinStampItemType ItemType { get; set; }

    public string? OtherType { get; set; }

    public string? Country { get; set; }

    public int? IssueYear { get; set; }

    public string? Denomination { get; set; }

    public CoinStampMetal Metal { get; set; }

    public string? OtherMetal { get; set; }

    public CoinStampCondition Condition { get; set; }

    public bool IsOriginal { get; set; }

    public bool IsRare { get; set; }

    public bool HasCertificate { get; set; }

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
