using Shared.DTOs.Admin;
using Shared.DTOs.Catalog;
using Shared.Enums;
using Shared.Responses;

namespace ServicesAbstraction;

public interface ICatalogConfigurationService
{
    Task<CatalogHierarchyDto> GetHierarchyAsync(CancellationToken cancellationToken = default);

    Task<CatalogNodeDto> GetNodeAsync(string nodeType, int nodeId, CancellationToken cancellationToken = default);

    CatalogConfigurationOptionsDto GetOptions();

    Task<CatalogBulkResultDto> UpdatePaymentAsync(
        UpdateCatalogPaymentRequest request, string adminUserId, CancellationToken cancellationToken = default);

    Task<CatalogBulkResultDto> UpdateVisibilityAsync(
        UpdateCatalogVisibilityRequest request, string adminUserId, CancellationToken cancellationToken = default);

    Task<CatalogBulkResultDto> ResetAsync(
        ResetCatalogConfigurationRequest request, string adminUserId, CancellationToken cancellationToken = default);

    CatalogConfigurationKind ParseKind(string? kind);

    Task<CatalogEffectiveConfigurationDto> GetEffectiveAsync(
        CatalogEffectiveQuery query, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<CatalogScheduledConfigurationDto>> GetScheduledAsync(CancellationToken cancellationToken = default);

    Task<PaginatedResult<AdminAuditLogDto>> GetHistoryAsync(
        AdminAuditLogFilterParams filter, CancellationToken cancellationToken = default);

    Task<PublicCatalogConfigurationDto> GetPublicAsync(CancellationToken cancellationToken = default);
}
