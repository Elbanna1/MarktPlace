using Domain.Entities;
using Microsoft.Extensions.Logging;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Admin;
using Shared.Enums;
using Shared.Exceptions;
using Shared.Responses;

namespace Services.Admin;

public class AdminAuditService : IAdminAuditService
{
    private readonly IAdminAuditRepository _repository;
    private readonly IAdminActionContext _context;
    private readonly ILogger<AdminAuditService> _logger;

    public AdminAuditService(
        IAdminAuditRepository repository,
        IAdminActionContext context,
        ILogger<AdminAuditService> logger)
    {
        _repository = repository;
        _context = context;
        _logger = logger;
    }

    public async Task LogAsync(
        AdminAuditAction action,
        string targetType,
        string? targetId,
        string description,
        string? oldValue = null,
        string? newValue = null,
        string? adminUserId = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var resolvedId = adminUserId ?? _context.UserId;

            if (string.IsNullOrWhiteSpace(resolvedId))
            {
                _logger.LogWarning(
                    "Audit skipped for {Action} on {TargetType} {TargetId}: no administrator in context.",
                    action, targetType, targetId);
                return;
            }

            var entry = new AdminAuditLog
            {
                Id = Guid.NewGuid(),
                AdminUserId = resolvedId,
                AdminName = Normalize(_context.UserName),
                Action = action,
                TargetType = targetType,
                TargetId = Truncate(targetId, 100),
                Description = Truncate(description, AdminAuditCatalog.MaxDescriptionLength)!,
                OldValue = Truncate(oldValue, AdminAuditCatalog.MaxValueLength),
                NewValue = Truncate(newValue, AdminAuditCatalog.MaxValueLength),
                IpAddress = _context.IpAddress,
                CreatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(entry, cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "Failed to write the audit record for {Action} on {TargetType} {TargetId}.",
                action, targetType, targetId);
        }
    }

    public async Task<PaginatedResult<AdminAuditLogDto>> GetAsync(
        AdminAuditLogFilterParams filter, CancellationToken cancellationToken = default)
    {
        var (items, totalCount) = await _repository.GetPagedAsync(filter, cancellationToken);

        return new PaginatedResult<AdminAuditLogDto>(
            items.Select(Map).ToList(), totalCount, filter.PageIndex, filter.PageSize);
    }

    public async Task<AdminAuditLogDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entry = await _repository.FindAsync(id, cancellationToken)
            ?? throw new NotFoundException("سجل الإجراء غير موجود.");

        return Map(entry);
    }

    public async Task<AdminAuditMetadataDto> GetMetadataAsync(CancellationToken cancellationToken = default)
    {
        var authors = await _repository.GetAuthorsAsync(cancellationToken);

        return new AdminAuditMetadataDto
        {
            Actions = AdminAuditCatalog.AllActions
                .Select(action => new AdminOptionDto
                {
                    Id = (int)action,
                    Name = AdminAuditCatalog.NameOf(action)
                })
                .ToList(),

            TargetTypes = AdminAuditCatalog.AllTargets
                .Select(target => new AdminAuditTargetOptionDto
                {
                    Value = target,
                    Name = AdminAuditCatalog.NameOfTarget(target)
                })
                .ToList(),

            Admins = authors
        };
    }

    private static string? Normalize(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static string? Truncate(string? value, int max) =>
        string.IsNullOrWhiteSpace(value)
            ? null
            : value.Length <= max ? value : value[..max];

    private static AdminAuditLogDto Map(AdminAuditLog entry) =>
        new()
        {
            Id = entry.Id,
            AdminUserId = entry.AdminUserId,
            AdminName = entry.AdminName,
            Action = entry.Action,
            ActionName = AdminAuditCatalog.NameOf(entry.Action),
            TargetType = entry.TargetType,
            TargetTypeName = AdminAuditCatalog.NameOfTarget(entry.TargetType),
            TargetId = entry.TargetId,
            Description = entry.Description,
            OldValue = entry.OldValue,
            NewValue = entry.NewValue,
            HasChange = entry.OldValue is not null || entry.NewValue is not null,
            IpAddress = entry.IpAddress,
            CreatedAt = entry.CreatedAt
        };
}
