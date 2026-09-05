using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Caching.Memory;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Advertisements;
using Shared.DTOs.Lookups;
using Shared.Enums;

namespace Services;

public class LookupService : ILookupService
{
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);

    private readonly ILookupRepository _repository;
    private readonly IMemoryCache _cache;
    private readonly ILookupCache _cacheVersion;
    private readonly IMapper _mapper;

    public LookupService(
        ILookupRepository repository, IMemoryCache cache, ILookupCache cacheVersion, IMapper mapper)
    {
        _repository = repository;
        _cache = cache;
        _cacheVersion = cacheVersion;
        _mapper = mapper;
    }

    public Task<IReadOnlyList<CategoryDto>> GetCategoriesAsync(CancellationToken cancellationToken = default) =>
        GetOrLoadAsync<Category, CategoryDto>("lookups:categories",
            ct => _repository.GetCategoriesAsync(ct), cancellationToken);

    public Task<IReadOnlyList<CategoryTreeDto>> GetCategoriesTreeAsync(CancellationToken cancellationToken = default) =>
        GetOrLoadAsync<Category, CategoryTreeDto>("lookups:categories-tree",
            ct => _repository.GetCategoriesWithSubCategoriesAsync(ct), cancellationToken);

    public Task<IReadOnlyList<SubCategoryDto>> GetSubCategoriesAsync(
        int categoryId, CancellationToken cancellationToken = default) =>
        GetOrLoadAsync<SubCategory, SubCategoryDto>($"lookups:subcategories:{categoryId}",
            ct => _repository.GetSubCategoriesAsync(categoryId, ct), cancellationToken);

    public async Task<IReadOnlyList<FeatureDto>> GetFeaturesAsync(
        int? subCategoryId = null, CancellationToken cancellationToken = default)
    {
        var all = await GetOrLoadAsync<Feature, FeatureDto>("lookups:features",
            ct => _repository.GetFeaturesAsync(ct), cancellationToken);

        if (subCategoryId is not { } id || !CarSubCategories.Contains(id))
            return all;

        var offered = CarCatalog.FeaturesOf((SubCategoryType)id)
            .ToDictionary(feature => feature.Id, feature => feature.Group);

        return all
            .Where(feature => offered.ContainsKey(feature.Id))
            .Select(feature => new FeatureDto
            {
                Id = feature.Id,
                Name = feature.Name,
                Group = offered[feature.Id]
            })
            .ToList();
    }

    public Task<IReadOnlyList<ListingTypeDto>> GetListingTypesAsync(CancellationToken cancellationToken = default) =>
        GetOrLoadAsync<ListingTypeLookup, ListingTypeDto>("lookups:listing-types",
            ct => _repository.GetListingTypesAsync(ct), cancellationToken);

    public Task<IReadOnlyList<GovernorateDto>> GetGovernoratesAsync(CancellationToken cancellationToken = default) =>
        GetOrLoadAsync<Governorate, GovernorateDto>("lookups:governorates",
            ct => _repository.GetGovernoratesAsync(ct), cancellationToken);

    public Task<IReadOnlyList<CenterDto>> GetCentersAsync(
        int? governorateId = null, CancellationToken cancellationToken = default) =>
        GetOrLoadAsync<Center, CenterDto>($"lookups:centers:{governorateId?.ToString() ?? "all"}",
            ct => _repository.GetCentersAsync(governorateId, ct), cancellationToken);

    public Task<IReadOnlyList<ProjectDto>> GetProjectsAsync(
        int? centerId = null, CancellationToken cancellationToken = default) =>
        GetOrLoadAsync<Project, ProjectDto>($"lookups:projects:{centerId?.ToString() ?? "all"}",
            ct => _repository.GetProjectsAsync(centerId, ct), cancellationToken);

    public Task<IReadOnlyList<WorkshopTypeDto>> GetWorkshopTypesAsync(CancellationToken cancellationToken = default) =>
        GetOrLoadAsync<WorkshopTypeLookup, WorkshopTypeDto>("lookups:workshop-types",
            ct => _repository.GetWorkshopTypesAsync(ct), cancellationToken);

    public Task<IReadOnlyList<SpecializationDto>> GetSpecializationsAsync(CancellationToken cancellationToken = default) =>
        GetOrLoadAsync<CraftsmanSpecializationLookup, SpecializationDto>("lookups:specializations",
            ct => _repository.GetSpecializationsAsync(ct), cancellationToken);

    public Task<IReadOnlyList<ExperienceLevelDto>> GetExperienceLevelsAsync(CancellationToken cancellationToken = default) =>
        GetOrLoadAsync<ExperienceLevelLookup, ExperienceLevelDto>("lookups:experience-levels",
            ct => _repository.GetExperienceLevelsAsync(ct), cancellationToken);

    public Task<IReadOnlyList<ProductionSpecialtyDto>> GetProductionSpecialtiesAsync(CancellationToken cancellationToken = default) =>
        GetOrLoadAsync<ProductionSpecialtyLookup, ProductionSpecialtyDto>("lookups:production-specialties",
            ct => _repository.GetProductionSpecialtiesAsync(ct), cancellationToken);

    public Task<IReadOnlyList<FarmTypeDto>> GetFarmTypesAsync(CancellationToken cancellationToken = default) =>
        GetOrLoadAsync<FarmTypeLookup, FarmTypeDto>("lookups:farm-types",
            ct => _repository.GetFarmTypesAsync(ct), cancellationToken);

    public Task<IReadOnlyList<AvailabilitySeasonDto>> GetAvailabilitySeasonsAsync(CancellationToken cancellationToken = default) =>
        GetOrLoadAsync<AvailabilitySeasonLookup, AvailabilitySeasonDto>("lookups:availability-seasons",
            ct => _repository.GetAvailabilitySeasonsAsync(ct), cancellationToken);

    public Task<IReadOnlyList<FarmingMethodDto>> GetFarmingMethodsAsync(CancellationToken cancellationToken = default) =>
        GetOrLoadAsync<FarmingMethodLookup, FarmingMethodDto>("lookups:farming-methods",
            ct => _repository.GetFarmingMethodsAsync(ct), cancellationToken);

    public Task<IReadOnlyList<CompanyFieldDto>> GetCompanyFieldsAsync(CancellationToken cancellationToken = default) =>
        GetOrLoadAsync<CompanyFieldLookup, CompanyFieldDto>("lookups:company-fields",
            ct => _repository.GetCompanyFieldsAsync(ct), cancellationToken);

    public Task<IReadOnlyList<SupplierTypeDto>> GetSupplierTypesAsync(CancellationToken cancellationToken = default) =>
        GetOrLoadAsync<SupplierTypeLookup, SupplierTypeDto>("lookups:supplier-types",
            ct => _repository.GetSupplierTypesAsync(ct), cancellationToken);

    public Task<IReadOnlyList<TradeTypeDto>> GetTradeTypesAsync(CancellationToken cancellationToken = default) =>
        GetOrLoadAsync<TradeTypeLookup, TradeTypeDto>("lookups:trade-types",
            ct => _repository.GetTradeTypesAsync(ct), cancellationToken);

    public Task<IReadOnlyList<SaleTypeDto>> GetSaleTypesAsync(CancellationToken cancellationToken = default) =>
        GetOrLoadAsync<SaleTypeLookup, SaleTypeDto>("lookups:sale-types",
            ct => _repository.GetSaleTypesAsync(ct), cancellationToken);

    private async Task<IReadOnlyList<TDto>> GetOrLoadAsync<TEntity, TDto>(
        string cacheKey,
        Func<CancellationToken, Task<List<TEntity>>> loader,
        CancellationToken cancellationToken)
    {
        cacheKey = $"{cacheKey}:v{_cacheVersion.Version}";

        if (_cache.TryGetValue(cacheKey, out IReadOnlyList<TDto>? cached) && cached is not null)
            return cached;

        var entities = await loader(cancellationToken);
        var mapped = _mapper.Map<IReadOnlyList<TDto>>(entities);

        _cache.Set(cacheKey, mapped, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = CacheDuration,

            Size = 1
        });

        return mapped;
    }
}
