using AutoMapper;
using Services.Lookups;
using ServicesAbstraction;
using Shared.DTOs.Lookups;
using Shared.DTOs.FruitVegetableMerchants;
using Shared.DTOs.JobOpportunities;
using Shared.DTOs.JobRequests;
using Shared.DTOs.WholesaleTraders;
using Shared.DTOs.Suppliers;
using Shared.Constants;
using Shared.DTOs.Lookups.Forms;
using Shared.Exceptions;

namespace Services.AdForms;

public class CreateAdFormService : ICreateAdFormService
{
    private readonly CategorySelectionResolver _selectionResolver;
    private readonly ILookupService _lookupService;
    private readonly IAdminFormRepository _formOverrides;
    private readonly IMapper _mapper;

    public CreateAdFormService(
        CategorySelectionResolver selectionResolver,
        ILookupService lookupService,
        IAdminFormRepository formOverrides,
        IMapper mapper)
    {
        _selectionResolver = selectionResolver;
        _lookupService = lookupService;
        _formOverrides = formOverrides;
        _mapper = mapper;
    }

    public async Task<CreateAdFormDto> GetCreateAdFormAsync(
        int categoryId, int? subCategoryId = null, CancellationToken cancellationToken = default)
    {
        var selection = await _selectionResolver.ResolveAsync(categoryId, subCategoryId, cancellationToken);

        if (selection.RequiresSubCategory)
            return BuildSubCategorySelectionForm(selection.Category, selection.SubCategories);

        var schema = AdFormSchemaCatalog.GetSchema(categoryId, subCategoryId)
            ?? throw new BadRequestException(
                "مفيش نموذج إضافة إعلان متاح للقسم اللي اخترته.");

        var overrides = await _formOverrides.GetOverridesAsync(
            categoryId, subCategoryId, cancellationToken);

        var fields = AdFormOverrideApplier.Apply(schema.Fields, overrides);

        return new CreateAdFormDto
        {
            Category = _mapper.Map<CategoryDto>(selection.Category),
            SubCategory = selection.SubCategory is null
                ? null
                : _mapper.Map<SubCategoryDto>(selection.SubCategory),
            RequiresSubCategory = false,
            Module = schema.Module,
            Submit = schema.Submit,
            Lookups = await LoadLookupsAsync(schema.RequiredLookups, subCategoryId, cancellationToken),
            Fields = fields
        };
    }

    private CreateAdFormDto BuildSubCategorySelectionForm(
        Domain.Entities.Category category, IReadOnlyList<Domain.Entities.SubCategory> subCategories) =>
        new()
        {
            Category = _mapper.Map<CategoryDto>(category),
            RequiresSubCategory = true,
            Module = "lookups",
            Lookups = new CreateAdFormLookupsDto
            {
                SubCategories = _mapper.Map<IReadOnlyList<CategoryTreeSubCategoryDto>>(subCategories)
            },
            Fields = AdFormSchemaCatalog.SubCategorySelectionFields()
        };

    private async Task<CreateAdFormLookupsDto> LoadLookupsAsync(
        IReadOnlyCollection<string> requiredLookups, int? subCategoryId,
        CancellationToken cancellationToken)
    {
        var lookups = new CreateAdFormLookupsDto();

        foreach (var key in requiredLookups)
        {
            switch (key)
            {
                case AdFormLookupKeys.ListingTypes:
                    lookups.ListingTypes = await _lookupService.GetListingTypesAsync(cancellationToken);
                    break;
                case AdFormLookupKeys.Features:
                    lookups.Features = await _lookupService.GetFeaturesAsync(
                        subCategoryId, cancellationToken);
                    break;
                case AdFormLookupKeys.Governorates:
                    lookups.Governorates = await _lookupService.GetGovernoratesAsync(cancellationToken);
                    break;
                case AdFormLookupKeys.Centers:
                    lookups.Centers = await _lookupService.GetCentersAsync(cancellationToken: cancellationToken);
                    break;
                case AdFormLookupKeys.WorkshopTypes:
                    lookups.WorkshopTypes = await _lookupService.GetWorkshopTypesAsync(cancellationToken);
                    break;
                case AdFormLookupKeys.Specializations:
                    lookups.Specializations = await _lookupService.GetSpecializationsAsync(cancellationToken);
                    break;
                case AdFormLookupKeys.ExperienceLevels:
                    lookups.ExperienceLevels = await _lookupService.GetExperienceLevelsAsync(cancellationToken);
                    break;

                case AdFormLookupKeys.ProductionSpecialties:
                    lookups.ProductionSpecialties = await _lookupService.GetProductionSpecialtiesAsync(cancellationToken);
                    break;
                case AdFormLookupKeys.FarmTypes:
                    lookups.FarmTypes = await _lookupService.GetFarmTypesAsync(cancellationToken);
                    break;
                case AdFormLookupKeys.Seasons:
                    lookups.Seasons = await _lookupService.GetAvailabilitySeasonsAsync(cancellationToken);
                    break;
                case AdFormLookupKeys.FarmingMethods:
                    lookups.FarmingMethods = await _lookupService.GetFarmingMethodsAsync(cancellationToken);
                    break;
                case AdFormLookupKeys.CompanyFields:
                    lookups.CompanyFields = await _lookupService.GetCompanyFieldsAsync(cancellationToken);
                    break;

                case AdFormLookupKeys.SupplierTypes:
                    lookups.SupplierTypes = SupplierCatalog.Specializations
                        .Select(entry => new SupplierSpecializationDto
                        {
                            Id = (int)entry.Value,
                            Name = entry.Name,
                            Group = entry.Group,
                            GroupAr = entry.GroupAr
                        })
                        .ToList();
                    break;
                case AdFormLookupKeys.TradeTypes:
                    lookups.TradeTypes = WholesaleTraderCatalog.TradeTypes
                        .Select(entry => new WholesaleTradeTypeDto
                        {
                            Id = (int)entry.Value,
                            Name = entry.Name
                        })
                        .ToList();
                    break;
                case AdFormLookupKeys.WholesaleSaleTypes:
                    lookups.WholesaleSaleTypes = WholesaleTraderCatalog.SaleTypeNames
                        .Select(entry => new WholesaleSaleTypeDto
                        {
                            Id = (int)entry.Key,
                            Name = entry.Value
                        })
                        .ToList();
                    break;
                case AdFormLookupKeys.MerchantSaleTypes:
                    lookups.MerchantSaleTypes = FruitVegetableMerchantCatalog.SaleTypeNames
                        .Select(entry => new MerchantSaleTypeDto
                        {
                            Id = (int)entry.Key,
                            Name = entry.Value
                        })
                        .ToList();
                    break;

                default:
                    _ = CarFormLookups.TryApply(lookups, key) ||
                        JobFormLookups.TryApply(lookups, key) ||
                        AnimalFormLookups.TryApply(lookups, key) ||
                        AntiqueFormLookups.TryApply(lookups, key) ||
                        ClothingFormLookups.TryApply(lookups, key) ||
                        OnlineShoppingFormLookups.TryApply(lookups, key) ||
                        HomeFurnishingFormLookups.TryApply(lookups, key) ||
                        RealEstateFormLookups.TryApply(lookups, key);
                    break;
            }
        }

        return lookups;
    }
}
