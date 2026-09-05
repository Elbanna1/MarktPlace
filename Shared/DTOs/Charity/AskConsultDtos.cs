using Shared.Enums;

using Shared.DTOs.Common;

namespace Shared.DTOs.Charity;

public class CreateAskConsultRequest : CreateCharityRequestBase
{
    public AskConsultCategory? Category { get; set; }

    public string? OtherCategory { get; set; }

    public string AskerName { get; set; } = default!;

    public string Title { get; set; } = default!;

    public string Question { get; set; } = default!;
}

public class UpdateAskConsultRequest : CreateAskConsultRequest, ICharityGalleryEdit
{
    public List<Guid> RemoveImageIds { get; set; } = new();
}

public class AskConsultDetailsDto : CharityDetailsDtoBase
{
    public AskConsultCategory Category { get; set; }

    public string CategoryLabel { get; set; } = default!;

    public string? OtherCategory { get; set; }

    public string AskerName { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Question { get; set; } = default!;

    public string? Governorate { get; set; }
    public string? Center { get; set; }

    public int LikesCount { get; set; }

    public bool IsLikedByCurrentUser { get; set; }

    public int CommentsCount { get; set; }

    public override ListingModuleType StatsListingType => ListingModuleType.AskConsult;
}

public class AskConsultListItemDto : CharityDetailsDtoBase
{
    public AskConsultCategory Category { get; set; }

    public string CategoryLabel { get; set; } = default!;

    public string? OtherCategory { get; set; }
    public string AskerName { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Question { get; set; } = default!;
    public string? Governorate { get; set; }
    public string? Center { get; set; }
    public int LikesCount { get; set; }
    public bool IsLikedByCurrentUser { get; set; }
    public int CommentsCount { get; set; }

    public override ListingModuleType StatsListingType => ListingModuleType.AskConsult;
}

public class AskConsultFilterParams : CharityFilterParamsBase
{
    public AskConsultCategory? Category { get; set; }
}

public class AskConsultLikeResultDto
{
    public bool Liked { get; set; }

    public int LikesCount { get; set; }
}

public class AskConsultCommentDto
{
    public Guid Id { get; set; }

    public Guid AskConsultId { get; set; }

    public ListingCommentAuthorDto Author { get; set; } = new();

    public string UserId { get; set; } = default!;

    public string UserName { get; set; } = default!;

    public string Comment { get; set; } = default!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsMine { get; set; }

    public bool CanEdit { get; set; }

    public bool CanDelete { get; set; }
}

public class CreateAskConsultCommentRequest
{
    public string Comment { get; set; } = default!;
}
