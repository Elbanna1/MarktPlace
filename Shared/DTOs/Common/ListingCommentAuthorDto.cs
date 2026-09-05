namespace Shared.DTOs.Common;

public class ListingCommentAuthorDto
{
    public string Id { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string? ProfileImageUrl { get; set; }

    public bool IsListingOwner { get; set; }
}
