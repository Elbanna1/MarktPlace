using Microsoft.AspNetCore.Http;

namespace Shared.DTOs.Banners;

public class BannerDto
{
    public Guid Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string ImageUrl { get; set; } = default!;

    public string? RedirectUrl { get; set; }

    public int DisplayOrder { get; set; }

    public bool IsActive { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsCurrentlyVisible { get; set; }
}

public class CreateBannerRequest
{
    public string? Title { get; set; }

    public string? Description { get; set; }

    public IFormFile? Image { get; set; }

    public string? RedirectUrl { get; set; }

    public int DisplayOrder { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }
}

public class UpdateBannerRequest
{
    public string? Title { get; set; }

    public string? Description { get; set; }

    public IFormFile? Image { get; set; }

    public string? RedirectUrl { get; set; }

    public int DisplayOrder { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }
}

public class BannerFilterParams
{
    public bool? IsActive { get; set; }
}
