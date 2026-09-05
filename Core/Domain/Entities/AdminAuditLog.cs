using Shared.Enums;

namespace Domain.Entities;

public class AdminAuditLog
{
    public Guid Id { get; set; }

    public string AdminUserId { get; set; } = default!;

    public string? AdminName { get; set; }

    public AdminAuditAction Action { get; set; }

    public string TargetType { get; set; } = default!;

    public string? TargetId { get; set; }

    public string Description { get; set; } = default!;

    public string? OldValue { get; set; }

    public string? NewValue { get; set; }

    public string? IpAddress { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
