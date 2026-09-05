using Shared.DTOs.Advertisements;
using Shared.DTOs.Lookups;

namespace ServicesAbstraction;

public interface ILookupService
{
    Task<IReadOnlyList<CategoryDto>> GetCategoriesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<CategoryTreeDto>> GetCategoriesTreeAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SubCategoryDto>> GetSubCategoriesAsync(
        int categoryId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FeatureDto>> GetFeaturesAsync(
        int? subCategoryId = null, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ListingTypeDto>> GetListingTypesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<GovernorateDto>> GetGovernoratesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<CenterDto>> GetCentersAsync(
        int? governorateId = null, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ProjectDto>> GetProjectsAsync(
        int? centerId = null, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<WorkshopTypeDto>> GetWorkshopTypesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SpecializationDto>> GetSpecializationsAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ExperienceLevelDto>> GetExperienceLevelsAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ProductionSpecialtyDto>> GetProductionSpecialtiesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<FarmTypeDto>> GetFarmTypesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AvailabilitySeasonDto>> GetAvailabilitySeasonsAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<FarmingMethodDto>> GetFarmingMethodsAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<CompanyFieldDto>> GetCompanyFieldsAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SupplierTypeDto>> GetSupplierTypesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TradeTypeDto>> GetTradeTypesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SaleTypeDto>> GetSaleTypesAsync(CancellationToken cancellationToken = default);
}
