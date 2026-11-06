using Shared.Enums;

namespace Shared.DTOs.Catalog;

public class CatalogNodeTargetRequest
{
    public string? NodeType { get; set; }

    public int NodeId { get; set; }
}

public class UpdateCatalogPaymentRequest
{
    public List<CatalogNodeTargetRequest> Targets { get; set; } = new();

    public bool PaymentEnabled { get; set; }

    public decimal? PaymentAmount { get; set; }

    public string? Currency { get; set; }

    public DateTime? PaymentActivationDate { get; set; }

    public DateTime? PaymentExpirationDate { get; set; }
}

public class UpdateCatalogVisibilityRequest
{
    public List<CatalogNodeTargetRequest> Targets { get; set; } = new();

    public List<string> Areas { get; set; } = new();

    public bool IsVisible { get; set; } = true;

    public bool IsEnabled { get; set; } = true;

    public DateTime? ActivationDate { get; set; }

    public DateTime? ExpirationDate { get; set; }
}

public class ResetCatalogConfigurationRequest
{
    public List<CatalogNodeTargetRequest> Targets { get; set; } = new();

    public string? Kind { get; set; }

    public List<string>? Areas { get; set; }
}

public class CatalogEffectiveQuery
{
    public string? Type { get; set; }

    public int? CategoryId { get; set; }

    public int? SubCategoryId { get; set; }
}

public class CatalogPaymentRuleDto
{
    public int ConfigurationId { get; set; }

    public bool PaymentEnabled { get; set; }

    public decimal? PaymentAmount { get; set; }

    public string? Currency { get; set; }

    public DateTime? PaymentActivationDate { get; set; }

    public DateTime? PaymentExpirationDate { get; set; }

    public DateTime? RequiredSince { get; set; }

    public string ConfigurationStatus { get; set; } = default!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? UpdatedBy { get; set; }
}

public class CatalogEffectivePaymentDto
{
    public bool PaymentEnabled { get; set; }

    public bool PaymentRequired { get; set; }

    public decimal PaymentAmount { get; set; }

    public string Currency { get; set; } = default!;

    public DateTime? RequiredSince { get; set; }

    public bool IsProtectedFree { get; set; }

    public string InheritanceMode { get; set; } = default!;

    public string? SourceKey { get; set; }

    public int? ParentConfigurationId { get; set; }

    public string? PriceSourceKey { get; set; }

    public string ConfigurationStatus { get; set; } = default!;
}

public class CatalogPaymentConfigurationDto
{
    public CatalogPaymentRuleDto? Explicit { get; set; }

    public CatalogEffectivePaymentDto Effective { get; set; } = default!;
}

public class CatalogVisibilityRuleDto
{
    public int ConfigurationId { get; set; }

    public bool IsVisible { get; set; }

    public bool IsEnabled { get; set; }

    public DateTime? ActivationDate { get; set; }

    public DateTime? ExpirationDate { get; set; }

    public string ConfigurationStatus { get; set; } = default!;

    public DateTime? UpdatedAt { get; set; }

    public string? UpdatedBy { get; set; }
}

public class CatalogAreaVisibilityDto
{
    public CatalogArea Area { get; set; }

    public string AreaKey { get; set; } = default!;

    public string AreaName { get; set; } = default!;

    public CatalogVisibilityRuleDto? Explicit { get; set; }

    public bool IsVisible { get; set; }

    public bool IsEnabled { get; set; }

    public string InheritanceMode { get; set; } = default!;

    public string? SourceKey { get; set; }

    public int? ParentConfigurationId { get; set; }
}

public class CatalogNodeDto
{
    public string Key { get; set; } = default!;

    public CatalogNodeType NodeType { get; set; }

    public string NodeTypeName { get; set; } = default!;

    public int NodeId { get; set; }

    public string? ModuleKey { get; set; }

    public string Name { get; set; } = default!;

    public string NameAr { get; set; } = default!;

    public string? ParentKey { get; set; }

    public int? CategoryId { get; set; }

    public int? SubCategoryId { get; set; }

    public bool IsProtectedFree { get; set; }

    public bool IsSubjectToPayment { get; set; }

    public CatalogPaymentConfigurationDto Payment { get; set; } = default!;

    public IReadOnlyList<CatalogAreaVisibilityDto> Visibility { get; set; } = Array.Empty<CatalogAreaVisibilityDto>();

    public IReadOnlyList<CatalogNodeDto> Children { get; set; } = Array.Empty<CatalogNodeDto>();
}

public class CatalogGlobalPaymentDto
{
    public bool PaymentEnabled { get; set; }

    public decimal PaymentAmount { get; set; }

    public string Currency { get; set; } = default!;

    public DateTime? RequiredSince { get; set; }

    public string SettingsEndpoint { get; set; } = default!;
}

public class CatalogHierarchyDto
{
    public DateTime GeneratedAt { get; set; }

    public CatalogGlobalPaymentDto GlobalPayment { get; set; } = default!;

    public IReadOnlyList<CatalogNodeDto> Categories { get; set; } = Array.Empty<CatalogNodeDto>();
}

public class CatalogBulkResultDto
{
    public int Created { get; set; }

    public int Updated { get; set; }

    public int Removed { get; set; }

    public IReadOnlyList<CatalogNodeDto> Items { get; set; } = Array.Empty<CatalogNodeDto>();
}

public class CatalogOptionDto
{
    public string Key { get; set; } = default!;

    public int Value { get; set; }

    public string NameAr { get; set; } = default!;

    public string? Description { get; set; }

    public bool EnforcedByBackend { get; set; }
}

public class CatalogConfigurationOptionsDto
{
    public IReadOnlyList<CatalogOptionDto> NodeTypes { get; set; } = Array.Empty<CatalogOptionDto>();

    public IReadOnlyList<CatalogOptionDto> Areas { get; set; } = Array.Empty<CatalogOptionDto>();

    public IReadOnlyList<CatalogOptionDto> Kinds { get; set; } = Array.Empty<CatalogOptionDto>();

    public IReadOnlyList<string> InheritanceOrder { get; set; } = Array.Empty<string>();

    public IReadOnlyList<string> ConfigurationStatuses { get; set; } = Array.Empty<string>();

    public IReadOnlyList<int> ProtectedFreeCategoryIds { get; set; } = Array.Empty<int>();

    public string DefaultCurrency { get; set; } = default!;

    public decimal MaxPaymentAmount { get; set; }

    public int MaxTargetsPerRequest { get; set; }

    public int CacheSeconds { get; set; }
}

public class CatalogEffectiveConfigurationDto
{
    public DateTime EvaluatedAt { get; set; }

    public ListingModuleType? AdvertisementType { get; set; }

    public int? CategoryId { get; set; }

    public int? SubCategoryId { get; set; }

    public IReadOnlyList<string> Chain { get; set; } = Array.Empty<string>();

    public CatalogEffectivePaymentDto Payment { get; set; } = default!;

    public IReadOnlyList<CatalogAreaVisibilityDto> Visibility { get; set; } = Array.Empty<CatalogAreaVisibilityDto>();
}

public class CatalogScheduledConfigurationDto
{
    public int ConfigurationId { get; set; }

    public string Key { get; set; } = default!;

    public string Kind { get; set; } = default!;

    public string? AreaKey { get; set; }

    public string Name { get; set; } = default!;

    public bool IsEnabled { get; set; }

    public bool? IsVisible { get; set; }

    public decimal? PaymentAmount { get; set; }

    public string? Currency { get; set; }

    public DateTime? ActivationDate { get; set; }

    public DateTime? ExpirationDate { get; set; }

    public string ConfigurationStatus { get; set; } = default!;
}

public class PublicCatalogAreaDto
{
    public string Area { get; set; } = default!;

    public bool IsVisible { get; set; }

    public bool IsEnabled { get; set; }
}

public class PublicCatalogPaymentDto
{
    public bool PaymentRequired { get; set; }

    public decimal? PaymentAmount { get; set; }

    public string? Currency { get; set; }

    public bool IsFree { get; set; }
}

public class PublicCatalogNodeDto
{
    public string Key { get; set; } = default!;

    public CatalogNodeType NodeType { get; set; }

    public int NodeId { get; set; }

    public string? ModuleKey { get; set; }

    public string Name { get; set; } = default!;

    public string NameAr { get; set; } = default!;

    public IReadOnlyList<PublicCatalogAreaDto> Areas { get; set; } = Array.Empty<PublicCatalogAreaDto>();

    public PublicCatalogPaymentDto Payment { get; set; } = default!;

    public IReadOnlyList<PublicCatalogNodeDto> Children { get; set; } = Array.Empty<PublicCatalogNodeDto>();
}

public class PublicCatalogConfigurationDto
{
    public DateTime GeneratedAt { get; set; }

    public int CacheSeconds { get; set; }

    public IReadOnlyList<PublicCatalogNodeDto> Categories { get; set; } = Array.Empty<PublicCatalogNodeDto>();
}
