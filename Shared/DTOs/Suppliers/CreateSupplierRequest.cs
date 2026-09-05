using Microsoft.AspNetCore.Http;
using Shared.Enums;

namespace Shared.DTOs.Suppliers;

public class CreateSupplierRequest
{
    public string SupplierName { get; set; } = default!;

    public SupplierSpecialization SupplierType { get; set; }

    public string? OtherSupplierType { get; set; }

    public string SuppliedProduct { get; set; } = default!;

    public string SupplyDetails { get; set; } = default!;

    public string Address { get; set; } = default!;

    public string? GoogleMaps { get; set; }

    public string Phone { get; set; } = default!;

    public string WhatsApp { get; set; } = default!;

    public string? Email { get; set; }

    public List<IFormFile> Images { get; set; } = new();

    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;
}
