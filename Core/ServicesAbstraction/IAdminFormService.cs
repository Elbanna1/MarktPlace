using Domain.Entities;
using Shared.DTOs.Admin;

namespace ServicesAbstraction;

public interface IAdminFormService
{
    Task<AdminFormSchemaDto> GetFormAsync(
        int categoryId, int? subCategoryId, CancellationToken cancellationToken = default);

    Task<AdminFormFieldDto> CreateFieldAsync(
        int categoryId, int? subCategoryId, string adminUserId, CreateFormFieldRequest request,
        CancellationToken cancellationToken = default);

    Task<AdminFormFieldDto> UpdateFieldAsync(
        Guid fieldId, string adminUserId, UpdateFormFieldRequest request,
        CancellationToken cancellationToken = default);

    Task DeleteFieldAsync(Guid fieldId, CancellationToken cancellationToken = default);

    Task ResetFieldAsync(
        int categoryId, int? subCategoryId, string fieldName, CancellationToken cancellationToken = default);

    Task<AdminFormOptionDto> AddOptionAsync(
        Guid fieldId, string adminUserId, SaveFormOptionRequest request,
        CancellationToken cancellationToken = default);

    Task<AdminFormOptionDto> UpdateOptionAsync(
        Guid optionId, SaveFormOptionRequest request, CancellationToken cancellationToken = default);

    Task DeleteOptionAsync(Guid optionId, CancellationToken cancellationToken = default);
}

public interface IAdminFormRepository
{
    Task<IReadOnlyList<AdFormFieldOverride>> GetOverridesAsync(
        int categoryId, int? subCategoryId, CancellationToken cancellationToken = default);

    Task<AdFormFieldOverride?> FindOverrideAsync(Guid id, CancellationToken cancellationToken = default);

    Task<AdFormFieldOverride?> FindOverrideAsync(
        int categoryId, int? subCategoryId, string fieldName, CancellationToken cancellationToken = default);

    Task<AdFormFieldOption?> FindOptionAsync(Guid id, CancellationToken cancellationToken = default);

    void AddOverride(AdFormFieldOverride fieldOverride);

    void RemoveOverride(AdFormFieldOverride fieldOverride);

    void AddOption(AdFormFieldOption option);

    void RemoveOption(AdFormFieldOption option);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
