using Shared.DTOs.Common;
using Shared.Enums;

namespace Shared.DTOs.Admin;

public class AdminAuditLogFilterParams : PaginationParams
{
    public string? Search { get; set; }

    public string? AdminUserId { get; set; }

    public AdminAuditAction? Action { get; set; }

    public string? TargetType { get; set; }

    public string? TargetId { get; set; }

    public DateTime? DateFrom { get; set; }

    public DateTime? DateTo { get; set; }

    public string? Sort { get; set; }
}

public class AdminAuditLogDto
{
    public Guid Id { get; set; }

    public string AdminUserId { get; set; } = default!;

    public string? AdminName { get; set; }

    public AdminAuditAction Action { get; set; }

    public string ActionName { get; set; } = default!;

    public string TargetType { get; set; } = default!;

    public string TargetTypeName { get; set; } = default!;

    public string? TargetId { get; set; }

    public string Description { get; set; } = default!;

    public string? OldValue { get; set; }

    public string? NewValue { get; set; }

    public bool HasChange { get; set; }

    public string? IpAddress { get; set; }

    public DateTime CreatedAt { get; set; }
}

public class AdminAuditMetadataDto
{
    public IReadOnlyList<AdminOptionDto> Actions { get; set; } = Array.Empty<AdminOptionDto>();

    public IReadOnlyList<AdminAuditTargetOptionDto> TargetTypes { get; set; } =
        Array.Empty<AdminAuditTargetOptionDto>();

    public IReadOnlyList<AdminAuditAuthorDto> Admins { get; set; } = Array.Empty<AdminAuditAuthorDto>();
}

public class AdminAuditTargetOptionDto
{
    public string Value { get; set; } = default!;

    public string Name { get; set; } = default!;
}

public class AdminAuditAuthorDto
{
    public string AdminUserId { get; set; } = default!;

    public string? AdminName { get; set; }

    public int Count { get; set; }
}
