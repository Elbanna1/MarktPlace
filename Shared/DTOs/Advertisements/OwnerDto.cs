namespace Shared.DTOs.Advertisements;

public class ListingOwnerDto
{
    public string Id { get; set; } = default!;
    public string FullName { get; set; } = default!;
    public string Governorate { get; set; } = default!;
    public string Center { get; set; } = default!;

    public string? ProfileImageUrl { get; set; }

    public DateTime CreatedAt { get; set; }
}

public class OwnerDto : ListingOwnerDto
{
    public string PhoneNumber { get; set; } = default!;
}
