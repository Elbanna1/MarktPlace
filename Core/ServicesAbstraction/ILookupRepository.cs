using Domain.Entities;

namespace ServicesAbstraction;

public interface ILookupRepository
{
    Task<List<Category>> GetCategoriesAsync(CancellationToken cancellationToken = default);

    Task<List<Category>> GetCategoriesWithSubCategoriesAsync(CancellationToken cancellationToken = default);

    Task<List<SubCategory>> GetSubCategoriesAsync(int? categoryId = null, CancellationToken cancellationToken = default);
    Task<List<Feature>> GetFeaturesAsync(CancellationToken cancellationToken = default);
    Task<List<ListingTypeLookup>> GetListingTypesAsync(CancellationToken cancellationToken = default);
    Task<List<Governorate>> GetGovernoratesAsync(CancellationToken cancellationToken = default);
    Task<List<Center>> GetCentersAsync(int? governorateId = null, CancellationToken cancellationToken = default);

    Task<List<Project>> GetProjectsAsync(int? centerId = null, CancellationToken cancellationToken = default);
    Task<List<WorkshopTypeLookup>> GetWorkshopTypesAsync(CancellationToken cancellationToken = default);
    Task<List<CraftsmanSpecializationLookup>> GetSpecializationsAsync(CancellationToken cancellationToken = default);
    Task<List<ExperienceLevelLookup>> GetExperienceLevelsAsync(CancellationToken cancellationToken = default);

    Task<List<ProductionSpecialtyLookup>> GetProductionSpecialtiesAsync(CancellationToken cancellationToken = default);
    Task<List<FarmTypeLookup>> GetFarmTypesAsync(CancellationToken cancellationToken = default);
    Task<List<AvailabilitySeasonLookup>> GetAvailabilitySeasonsAsync(CancellationToken cancellationToken = default);
    Task<List<FarmingMethodLookup>> GetFarmingMethodsAsync(CancellationToken cancellationToken = default);
    Task<List<CompanyFieldLookup>> GetCompanyFieldsAsync(CancellationToken cancellationToken = default);
    Task<List<SupplierTypeLookup>> GetSupplierTypesAsync(CancellationToken cancellationToken = default);
    Task<List<TradeTypeLookup>> GetTradeTypesAsync(CancellationToken cancellationToken = default);
    Task<List<SaleTypeLookup>> GetSaleTypesAsync(CancellationToken cancellationToken = default);
}
