using Shared.DTOs.Lookups.Read;

namespace ServicesAbstraction;

public interface IReadConfigService
{
    Task<ReadConfigDto> GetReadConfigAsync(
        int categoryId, int? subCategoryId = null, CancellationToken cancellationToken = default);
}
