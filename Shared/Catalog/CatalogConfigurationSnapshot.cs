using Shared.Constants;
using Shared.Enums;

namespace Shared.Catalog;

public readonly record struct CatalogNodeRef(CatalogNodeType Type, int Id)
{
    public override string ToString() => $"{Type}:{Id}";
}

public sealed record CatalogContext(ListingModuleType? Module, int? SubCategoryId, int? CategoryId)
{
    public static CatalogContext ForCategory(int categoryId) => new(null, null, categoryId);

    public static CatalogContext ForSubCategory(int subCategoryId) => new(null, subCategoryId, null);

    public static CatalogContext ForModule(ListingModuleType module, int? subCategoryId = null) =>
        new(module, subCategoryId, null);
}

public sealed record CatalogConfigurationEntry(
    int Id,
    CatalogConfigurationKind Kind,
    CatalogNodeType NodeType,
    int NodeId,
    CatalogArea Area,
    bool IsEnabled,
    bool? IsVisible,
    decimal? Price,
    string? Currency,
    DateTime? ActiveFrom,
    DateTime? ActiveUntil,
    DateTime? EnabledAt,
    DateTime CreatedAt,
    string? CreatedBy,
    DateTime? UpdatedAt,
    string? UpdatedBy)
{
    public CatalogNodeRef Node => new(NodeType, NodeId);

    public bool IsInEffect(DateTime utcNow) =>
        (ActiveFrom is null || ActiveFrom <= utcNow) && (ActiveUntil is null || utcNow < ActiveUntil);

    public string StatusAt(DateTime utcNow) =>
        ActiveFrom is { } from && from > utcNow ? CatalogConfigurationStatuses.Scheduled
        : ActiveUntil is { } until && until <= utcNow ? CatalogConfigurationStatuses.Expired
        : CatalogConfigurationStatuses.Active;
}

public sealed record CatalogPaymentRoot(bool IsEnabled, decimal Price, string Currency, DateTime? EnabledAt);

public sealed record EffectivePaymentConfiguration(
    bool IsEnabled,
    bool IsRequired,
    decimal Price,
    string Currency,
    DateTime? RequiredSince,
    bool IsProtected,
    CatalogNodeRef? Source,
    CatalogNodeRef? PriceSource);

public sealed record EffectiveVisibility(bool IsVisible, bool IsEnabled, CatalogNodeRef? Source);

public static class CatalogConfigurationStatuses
{
    public const string Inherited = "Inherited";

    public const string Active = "Active";

    public const string Scheduled = "Scheduled";

    public const string Expired = "Expired";

    public const string Protected = "Protected";
}

public sealed class CatalogConfigurationSnapshot
{
    private readonly IReadOnlyDictionary<(CatalogConfigurationKind, CatalogNodeType, int, CatalogArea), CatalogConfigurationEntry> _entries;
    private readonly IReadOnlyDictionary<int, int> _categoryOfSubCategory;

    public CatalogConfigurationSnapshot(
        IEnumerable<CatalogConfigurationEntry> entries,
        CatalogPaymentRoot paymentRoot,
        IReadOnlyDictionary<int, int> categoryOfSubCategory)
    {
        Entries = entries.ToList();
        PaymentRoot = paymentRoot;
        _categoryOfSubCategory = categoryOfSubCategory;
        _entries = Entries.ToDictionary(entry => (entry.Kind, entry.NodeType, entry.NodeId, entry.Area));
    }

    public IReadOnlyList<CatalogConfigurationEntry> Entries { get; }

    public IReadOnlyCollection<int> KnownSubCategoryIds => (IReadOnlyCollection<int>)_categoryOfSubCategory.Keys;

    public CatalogPaymentRoot PaymentRoot { get; }

    public CatalogConfigurationEntry? Find(
        CatalogConfigurationKind kind, CatalogNodeRef node, CatalogArea area = CatalogArea.None) =>
        _entries.GetValueOrDefault((kind, node.Type, node.Id, area));

    public int? CategoryOf(CatalogContext context)
    {
        if (context.CategoryId is { } categoryId)
            return categoryId;

        if (SubCategoryOf(context) is { } subCategoryId)
        {
            if (_categoryOfSubCategory.TryGetValue(subCategoryId, out var mapped))
                return mapped;

            if (ListingModuleCatalog.CategoryOf((SubCategoryType)subCategoryId) is { } fromCatalog)
                return (int)fromCatalog;
        }

        return context.Module is { } module && ListingModuleCatalog.CategoryOf(module) is { } category
            ? (int)category
            : null;
    }

    public static int? SubCategoryOf(CatalogContext context) =>
        context.SubCategoryId ??
        (context.Module is { } module && ListingModuleCatalog.SubCategoryOf(module) is { } sub ? (int)sub : null);

    public IReadOnlyList<CatalogNodeRef> ChainOf(CatalogContext context)
    {
        var chain = new List<CatalogNodeRef>(3);

        if (context.Module is { } module)
            chain.Add(new CatalogNodeRef(CatalogNodeType.Module, (int)module));

        if (SubCategoryOf(context) is { } subCategoryId)
            chain.Add(new CatalogNodeRef(CatalogNodeType.SubCategory, subCategoryId));

        if (CategoryOf(context) is { } categoryId)
            chain.Add(new CatalogNodeRef(CatalogNodeType.Category, categoryId));

        return chain;
    }

    public bool IsProtectedFree(CatalogContext context) =>
        CatalogProtection.IsFreeCategory(CategoryOf(context)) ||
        (context.Module is { } module && CatalogProtection.IsFreeModule(module));

    public EffectivePaymentConfiguration ResolvePayment(
        CatalogContext context, DateTime utcNow, CatalogNodeRef? ignoring = null)
    {
        var chain = ChainOf(context);

        decimal? price = null;
        string? currency = null;
        CatalogNodeRef? priceSource = null;
        CatalogConfigurationEntry? decider = null;

        foreach (var node in chain)
        {
            if (node == ignoring ||
                Find(CatalogConfigurationKind.Payment, node) is not { } entry ||
                !entry.IsInEffect(utcNow))
                continue;

            decider ??= entry;

            if (price is null && entry.Price is { } ownPrice)
            {
                price = ownPrice;
                priceSource = node;
            }

            currency ??= entry.Currency;
        }

        var resolvedPrice = price ?? PaymentRoot.Price;
        var resolvedCurrency = currency ?? PaymentRoot.Currency;

        if (IsProtectedFree(context))
            return new EffectivePaymentConfiguration(
                false, false, resolvedPrice, resolvedCurrency, null, true, null, priceSource);

        var enabled = decider?.IsEnabled ?? PaymentRoot.IsEnabled;

        var since = decider is null
            ? PaymentRoot.EnabledAt
            : decider.EnabledAt ?? decider.ActiveFrom;

        var subject = context.Module is not { } module || AdPaymentCatalog.IsSubjectToPayment(module);

        return new EffectivePaymentConfiguration(
            enabled,
            enabled && subject && resolvedPrice > 0,
            resolvedPrice,
            resolvedCurrency,
            since,
            false,
            decider?.Node,
            priceSource);
    }

    public EffectiveVisibility ResolveVisibility(CatalogContext context, CatalogArea area, DateTime utcNow)
    {
        foreach (var node in ChainOf(context))
        {
            if (Find(CatalogConfigurationKind.Visibility, node, area) is { } entry && entry.IsInEffect(utcNow))
                return new EffectiveVisibility(entry.IsVisible ?? true, entry.IsEnabled, node);
        }

        return new EffectiveVisibility(true, true, null);
    }

    public bool IsVisible(CatalogContext context, CatalogArea area, DateTime utcNow)
    {
        var visibility = ResolveVisibility(context, area, utcNow);

        return visibility.IsVisible && visibility.IsEnabled;
    }

    public bool AnyPaymentEnabled(DateTime utcNow) =>
        PaymentRoot.IsEnabled ||
        Entries.Any(entry => entry.Kind == CatalogConfigurationKind.Payment && entry.IsEnabled && entry.IsInEffect(utcNow));
}
