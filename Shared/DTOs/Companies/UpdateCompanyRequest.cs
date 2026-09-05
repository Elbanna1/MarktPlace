using Microsoft.AspNetCore.Http;
using Shared.Enums;

namespace Shared.DTOs.Companies;

public class UpdateCompanyRequest
{
    public string CompanyName { get; set; } = default!;

    public CompanyField CompanyField { get; set; }

    public string? OtherCompanyField { get; set; }

    public string Address { get; set; } = default!;

    public string? GoogleMaps { get; set; }

    public string Phone { get; set; } = default!;

    public string WhatsApp { get; set; } = default!;

    public string? Email { get; set; }

    public string? Website { get; set; }

    public IFormFile? Logo { get; set; }

    public List<IFormFile> Images { get; set; } = new();

    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;

    public List<Guid> RemoveImageIds { get; set; } = new();

    public bool RemoveLogo { get; set; }
}
