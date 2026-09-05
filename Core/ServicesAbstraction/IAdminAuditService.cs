using Domain.Entities;
using Shared.DTOs.Admin;
using Shared.Enums;
using Shared.Responses;

namespace ServicesAbstraction;

public interface IAdminAuditService
{
    Task LogAsync(
        AdminAuditAction action,
        string targetType,
        string? targetId,
        string description,
        string? oldValue = null,
        string? newValue = null,
        string? adminUserId = null,
        CancellationToken cancellationToken = default);

    Task<PaginatedResult<AdminAuditLogDto>> GetAsync(
        AdminAuditLogFilterParams filter, CancellationToken cancellationToken = default);

    Task<AdminAuditLogDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<AdminAuditMetadataDto> GetMetadataAsync(CancellationToken cancellationToken = default);
}

public interface IAdminAuditRepository
{
    Task AddAsync(AdminAuditLog entry, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<AdminAuditLog> Items, int TotalCount)> GetPagedAsync(
        AdminAuditLogFilterParams filter, CancellationToken cancellationToken = default);

    Task<AdminAuditLog?> FindAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AdminAuditAuthorDto>> GetAuthorsAsync(CancellationToken cancellationToken = default);
}
