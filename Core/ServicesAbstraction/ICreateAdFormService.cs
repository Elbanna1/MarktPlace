using Shared.DTOs.Lookups.Forms;

namespace ServicesAbstraction;

public interface ICreateAdFormService
{
    Task<CreateAdFormDto> GetCreateAdFormAsync(
        int categoryId, int? subCategoryId = null, CancellationToken cancellationToken = default);
}
