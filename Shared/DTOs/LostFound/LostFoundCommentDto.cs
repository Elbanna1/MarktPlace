using Shared.DTOs.Common;

namespace Shared.DTOs.LostFound;

public class LostFoundCommentDto
{
    public Guid Id { get; set; }

    public Guid PostId { get; set; }

    public ListingCommentAuthorDto Author { get; set; } = new();

    public string UserName { get; set; } = default!;

    public string Comment { get; set; } = default!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsMine { get; set; }

    public bool CanEdit { get; set; }

    public bool CanDelete { get; set; }
}
