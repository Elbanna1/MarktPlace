using Microsoft.AspNetCore.Http;
using Shared.Enums;

namespace Shared.DTOs.WholesaleTraders;

public class UpdateWholesaleTraderRequest
{
    public string TraderName { get; set; } = default!;

    public WholesaleTradeType TradeType { get; set; }

    public string? OtherTradeType { get; set; }

    public string ProductsName { get; set; } = default!;

    public string ProductDetails { get; set; } = default!;

    public WholesaleSaleType SaleType { get; set; }

    public string Address { get; set; } = default!;

    public string? GoogleMaps { get; set; }

    public string Phone { get; set; } = default!;

    public string WhatsApp { get; set; } = default!;

    public string? Email { get; set; }

    public List<IFormFile> Images { get; set; } = new();

    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;

    public List<Guid> RemoveImageIds { get; set; } = new();
}
