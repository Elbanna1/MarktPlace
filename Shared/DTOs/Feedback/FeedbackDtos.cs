using Microsoft.AspNetCore.Http;
using Shared.DTOs.Advertisements;
using Shared.DTOs.Common;
using Shared.Enums;

namespace Shared.DTOs.Feedback;

public class CreateFeedbackRequest
{
    public int? Rating { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public FeedbackType Type { get; set; } = FeedbackType.Other;

    public List<IFormFile> Images { get; set; } = new();
}

public class UpdateFeedbackStatusRequest
{
    public FeedbackStatus Status { get; set; }

    public string? AdminReply { get; set; }
}

public class FeedbackImageDto
{
    public Guid Id { get; set; }
    public string Url { get; set; } = default!;
    public bool IsPrimary { get; set; }
}

public class FeedbackListItemDto
{
    public Guid Id { get; set; }

    public string Title { get; set; } = default!;

    public string Type { get; set; } = default!;

    public FeedbackType TypeId { get; set; }

    public string TypeName { get; set; } = default!;

    public string Status { get; set; } = default!;

    public FeedbackStatus StatusId { get; set; }

    public string StatusName { get; set; } = default!;

    public int? Rating { get; set; }

    public string? PrimaryImageUrl { get; set; }

    public int ImagesCount { get; set; }

    public bool HasReply { get; set; }

    public bool IsClosed { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public OwnerDto? User { get; set; }
}

public class FeedbackDetailsDto : FeedbackListItemDto
{
    public string Description { get; set; } = default!;

    public List<FeedbackImageDto> Images { get; set; } = new();

    public string? AdminReply { get; set; }

    public DateTime? ReviewedAt { get; set; }

    public DateTime? ClosedAt { get; set; }

    public string? ReviewedBy { get; set; }
}

public class FeedbackFilterParams : PaginationParams
{
    public FeedbackType? Type { get; set; }

    public FeedbackStatus? Status { get; set; }

    public string? Search { get; set; }

    public string? UserId { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }
}

public class FeedbackOptionDto
{
    public int Id { get; set; }

    public string Key { get; set; } = default!;

    public string Name { get; set; } = default!;
}

public class FeedbackMetadataDto
{
    public IReadOnlyList<FeedbackOptionDto> Types { get; set; } = new List<FeedbackOptionDto>();

    public IReadOnlyList<FeedbackOptionDto> Statuses { get; set; } = new List<FeedbackOptionDto>();

    public int MaxTitleLength { get; set; }

    public int MaxDescriptionLength { get; set; }

    public int MinRating { get; set; }

    public int MaxRating { get; set; }

    public int MaxImages { get; set; }
}
