using Domain.Entities;
using ServicesAbstraction;
using Shared.Catalog;
using Shared.Constants;
using Shared.DTOs.Admin;
using Shared.DTOs.Catalog;
using Shared.Enums;
using Shared.Exceptions;
using Shared.Responses;

namespace Services.Catalog;

public class CatalogConfigurationService : ICatalogConfigurationService
{
    public const int MaxTargetsPerRequest = 100;

    public const decimal MaxPaymentAmount = 1_000_000m;

    private static readonly IReadOnlyList<CatalogArea> Areas =
        Enum.GetValues<CatalogArea>().Where(area => area != CatalogArea.None).ToList();

    private static readonly IReadOnlyDictionary<CatalogArea, (string NameAr, string Description, bool Enforced)> AreaInfo =
        new Dictionary<CatalogArea, (string, string, bool)>
        {
            [CatalogArea.HomePage] = ("الصفحة الرئيسية",
                "أقسام الصفحة الرئيسية المربوطة بقسم (GET /api/home)", true),
            [CatalogArea.CategoryListing] = ("قائمة الأقسام الرئيسية",
                "GET /api/lookups/categories و /api/lookups/categories-tree", true),
            [CatalogArea.SubCategoryListing] = ("قائمة الأقسام الفرعية",
                "GET /api/lookups/subcategories/{categoryId} والأقسام الفرعية داخل categories-tree", true),
            [CatalogArea.AdCreation] = ("نموذج إضافة إعلان",
                "GET /api/lookups/create-ad-form وكل طلبات إنشاء الإعلانات", true),
            [CatalogArea.Browsing] = ("تصفح الإعلانات",
                "قوائم الإعلانات العامة لكل وحدة — الواجهة تقرأ القيمة من الإعدادات العامة", false),
            [CatalogArea.Search] = ("البحث والفلاتر",
                "البحث واقتراحات البحث والفلاتر — الواجهة تقرأ القيمة من الإعدادات العامة", false)
        };

    private readonly ICatalogConfigurationRepository _repository;
    private readonly ICatalogConfigurationProvider _provider;
    private readonly IAdPaymentRepository _payments;
    private readonly ILookupService _lookups;
    private readonly IAdminAuditService _audit;
    private readonly IUnitOfWork _unitOfWork;

    public CatalogConfigurationService(
        ICatalogConfigurationRepository repository,
        ICatalogConfigurationProvider provider,
        IAdPaymentRepository payments,
        ILookupService lookups,
        IAdminAuditService audit,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _provider = provider;
        _payments = payments;
        _lookups = lookups;
        _audit = audit;
        _unitOfWork = unitOfWork;
    }

    public async Task<CatalogHierarchyDto> GetHierarchyAsync(CancellationToken cancellationToken = default)
    {
        var snapshot = await _provider.GetSnapshotAsync(cancellationToken);
        var tree = await LoadTreeAsync(cancellationToken);
        var now = DateTime.UtcNow;

        return new CatalogHierarchyDto
        {
            GeneratedAt = now,
            GlobalPayment = new CatalogGlobalPaymentDto
            {
                PaymentEnabled = snapshot.PaymentRoot.IsEnabled,
                PaymentAmount = snapshot.PaymentRoot.Price,
                Currency = snapshot.PaymentRoot.Currency,
                RequiredSince = snapshot.PaymentRoot.EnabledAt,
                SettingsEndpoint = $"/{ApiVersions.AdminRoutePrefix}/ad-payment/settings"
            },
            Categories = tree.Select(category => BuildCategory(snapshot, category, now)).ToList()
        };
    }

    public async Task<CatalogNodeDto> GetNodeAsync(
        string nodeType, int nodeId, CancellationToken cancellationToken = default)
    {
        var node = new CatalogNodeRef(ParseNodeType(nodeType), nodeId);
        var snapshot = await _provider.GetSnapshotAsync(cancellationToken);
        var tree = await LoadTreeAsync(cancellationToken);

        return Describe(snapshot, tree, node, DateTime.UtcNow)
               ?? throw new NotFoundException("القسم أو الوحدة المطلوبة غير موجودة.");
    }

    public CatalogConfigurationOptionsDto GetOptions() => new()
    {
        NodeTypes = Enum.GetValues<CatalogNodeType>()
            .Select(type => new CatalogOptionDto
            {
                Key = type.ToString(),
                Value = (int)type,
                NameAr = type switch
                {
                    CatalogNodeType.Category => "قسم رئيسي",
                    CatalogNodeType.SubCategory => "قسم فرعي",
                    _ => "وحدة إعلانات"
                },
                EnforcedByBackend = true
            })
            .ToList(),
        Areas = Areas
            .Select(area => new CatalogOptionDto
            {
                Key = area.ToString(),
                Value = (int)area,
                NameAr = AreaInfo[area].NameAr,
                Description = AreaInfo[area].Description,
                EnforcedByBackend = AreaInfo[area].Enforced
            })
            .ToList(),
        Kinds = Enum.GetValues<CatalogConfigurationKind>()
            .Select(kind => new CatalogOptionDto
            {
                Key = kind.ToString(),
                Value = (int)kind,
                NameAr = kind == CatalogConfigurationKind.Payment ? "رسوم النشر" : "الظهور",
                EnforcedByBackend = true
            })
            .ToList(),
        InheritanceOrder = new[] { "Module", "SubCategory", "Category", "Global" },
        ConfigurationStatuses = new[]
        {
            CatalogConfigurationStatuses.Inherited, CatalogConfigurationStatuses.Active,
            CatalogConfigurationStatuses.Scheduled, CatalogConfigurationStatuses.Expired,
            CatalogConfigurationStatuses.Protected
        },
        ProtectedFreeCategoryIds = CatalogProtection.FreeCategoryIds.OrderBy(id => id).ToList(),
        DefaultCurrency = PaymentCatalog.DefaultCurrency,
        MaxPaymentAmount = MaxPaymentAmount,
        MaxTargetsPerRequest = MaxTargetsPerRequest,
        CacheSeconds = (int)CatalogConfigurationProvider.CacheDuration.TotalSeconds
    };

    public async Task<CatalogBulkResultDto> UpdatePaymentAsync(
        UpdateCatalogPaymentRequest request, string adminUserId, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var tree = await LoadTreeAsync(cancellationToken);
        var targets = ResolveTargets(request.Targets, tree);
        var now = DateTime.UtcNow;

        var from = NormalizeUtc(request.PaymentActivationDate, "تاريخ تفعيل الدفع");
        var until = NormalizeUtc(request.PaymentExpirationDate, "تاريخ انتهاء الدفع");
        ValidateWindow(from, until, now);

        var amount = request.PaymentAmount;

        if (amount is { } value)
        {
            if (value <= 0 || value > MaxPaymentAmount)
                throw new BadRequestException($"رسوم النشر لازم تكون أكبر من صفر ومتزيدش عن {MaxPaymentAmount:N0}.");

            if (decimal.Round(value, 2) != value)
                throw new BadRequestException("رسوم النشر مينفعش يكون فيها أكتر من رقمين عشريين.");
        }

        var currency = NormalizeCurrency(request.Currency);
        var snapshot = await _provider.GetSnapshotAsync(cancellationToken);

        if (request.PaymentEnabled)
        {
            var protectedTargets = targets.Where(target => snapshot.IsProtectedFree(ContextOf(target))).ToList();

            if (protectedTargets.Count > 0)
                throw new BadRequestException(
                    CatalogProtection.FreeCategoryMessage,
                    protectedTargets.Select(target => $"{target}: {NameOf(tree, target)}").ToList());

            var settings = await _payments.GetSettingsAsync(asNoTracking: true, cancellationToken);
            var active = await _payments.GetActiveMethodsAsync(cancellationToken);
            var allowed = ParseIds(settings?.PaymentMethodIds);

            if (!active.Any(method => allowed.Count == 0 || allowed.Contains(method.Id)))
                throw new BadRequestException("لازم تكون فيه طريقة دفع مفعلة واحدة على الأقل قبل تفعيل رسوم النشر.");

            var unpriced = targets
                .Where(target => amount is null &&
                                 snapshot.ResolvePayment(ContextOf(target), from ?? now, ignoring: target).Price <= 0)
                .ToList();

            if (unpriced.Count > 0)
                throw new BadRequestException(
                    "لازم تحدد رسوم النشر للأقسام دي، لأنها مش بتورث سعر أكبر من صفر.",
                    unpriced.Select(target => $"{target}: {NameOf(tree, target)}").ToList());
        }

        var rows = (await _repository.GetTrackedAsync(CatalogConfigurationKind.Payment, cancellationToken))
            .ToDictionary(row => new CatalogNodeRef(row.NodeType, row.NodeId));

        var audits = new List<(CatalogNodeRef Node, string? Before, string After)>();
        var created = 0;
        var updated = 0;

        foreach (var target in targets)
        {
            rows.TryGetValue(target, out var row);
            var before = row is null ? null : DescribePayment(row);

            if (row is null)
            {
                row = new CatalogConfiguration
                {
                    Kind = CatalogConfigurationKind.Payment,
                    NodeType = target.Type,
                    NodeId = target.Id,
                    Area = CatalogArea.None,
                    CreatedAt = now,
                    CreatedBy = adminUserId
                };

                _repository.Add(row);
                created++;
            }
            else
            {
                row.UpdatedAt = now;
                row.UpdatedBy = adminUserId;
                updated++;
            }

            var wasEnabled = row.IsEnabled && row.EnabledAt is not null;

            row.IsEnabled = request.PaymentEnabled;
            row.Price = amount;
            row.Currency = currency;
            row.ActiveFrom = from;
            row.ActiveUntil = until;

            if (!request.PaymentEnabled)
            {
                row.EnabledAt = null;
            }
            else if (!wasEnabled)
            {
                var parent = snapshot.ResolvePayment(ContextOf(target), from ?? now, ignoring: target);

                row.EnabledAt = parent.IsEnabled && parent.RequiredSince is { } inherited
                    ? inherited
                    : from ?? now;
            }

            audits.Add((target, before, DescribePayment(row)));
        }

        await SaveAsync(cancellationToken);

        foreach (var (node, before, after) in audits)
            await _audit.LogAsync(
                AdminAuditAction.UpdateCatalogPayment, AdminAuditCatalog.Targets.CatalogConfiguration, node.ToString(),
                $"تعديل رسوم النشر: {NameOf(tree, node)}",
                oldValue: before, newValue: after, adminUserId: adminUserId, cancellationToken: cancellationToken);

        return await ResultAsync(targets, tree, created, updated, 0, cancellationToken);
    }

    public async Task<CatalogBulkResultDto> UpdateVisibilityAsync(
        UpdateCatalogVisibilityRequest request, string adminUserId, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var tree = await LoadTreeAsync(cancellationToken);
        var targets = ResolveTargets(request.Targets, tree);
        var areas = ParseAreas(request.Areas, required: true);
        var now = DateTime.UtcNow;

        var from = NormalizeUtc(request.ActivationDate, "تاريخ بداية الظهور");
        var until = NormalizeUtc(request.ExpirationDate, "تاريخ نهاية الظهور");
        ValidateWindow(from, until, now);

        var rows = (await _repository.GetTrackedAsync(CatalogConfigurationKind.Visibility, cancellationToken))
            .ToDictionary(row => (new CatalogNodeRef(row.NodeType, row.NodeId), row.Area));

        var audits = new List<(CatalogNodeRef Node, CatalogArea Area, string? Before, string After)>();
        var created = 0;
        var updated = 0;

        foreach (var target in targets)
        {
            foreach (var area in areas)
            {
                rows.TryGetValue((target, area), out var row);
                var before = row is null ? null : DescribeVisibility(row);

                if (row is null)
                {
                    row = new CatalogConfiguration
                    {
                        Kind = CatalogConfigurationKind.Visibility,
                        NodeType = target.Type,
                        NodeId = target.Id,
                        Area = area,
                        CreatedAt = now,
                        CreatedBy = adminUserId
                    };

                    _repository.Add(row);
                    created++;
                }
                else
                {
                    row.UpdatedAt = now;
                    row.UpdatedBy = adminUserId;
                    updated++;
                }

                row.IsVisible = request.IsVisible;
                row.IsEnabled = request.IsEnabled;
                row.ActiveFrom = from;
                row.ActiveUntil = until;

                audits.Add((target, area, before, DescribeVisibility(row)));
            }
        }

        await SaveAsync(cancellationToken);

        foreach (var (node, area, before, after) in audits)
            await _audit.LogAsync(
                AdminAuditAction.UpdateCatalogVisibility, AdminAuditCatalog.Targets.CatalogConfiguration,
                $"{node}:{area}",
                $"تعديل ظهور {NameOf(tree, node)} في {AreaInfo[area].NameAr}",
                oldValue: before, newValue: after, adminUserId: adminUserId, cancellationToken: cancellationToken);

        return await ResultAsync(targets, tree, created, updated, 0, cancellationToken);
    }

    public async Task<CatalogBulkResultDto> ResetAsync(
        ResetCatalogConfigurationRequest request, string adminUserId, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var kind = ParseKind(request.Kind);
        var tree = await LoadTreeAsync(cancellationToken);
        var targets = ResolveTargets(request.Targets, tree);

        var areas = kind == CatalogConfigurationKind.Payment
            ? new[] { CatalogArea.None }
            : request.Areas is { Count: > 0 } ? ParseAreas(request.Areas, required: true) : Areas;

        var set = targets.ToHashSet();

        var doomed = (await _repository.GetTrackedAsync(kind, cancellationToken))
            .Where(row => set.Contains(new CatalogNodeRef(row.NodeType, row.NodeId)) && areas.Contains(row.Area))
            .ToList();

        foreach (var row in doomed)
            _repository.Remove(row);

        await SaveAsync(cancellationToken);

        foreach (var row in doomed)
        {
            var node = new CatalogNodeRef(row.NodeType, row.NodeId);

            await _audit.LogAsync(
                AdminAuditAction.ResetCatalogConfiguration, AdminAuditCatalog.Targets.CatalogConfiguration,
                kind == CatalogConfigurationKind.Payment ? node.ToString() : $"{node}:{row.Area}",
                kind == CatalogConfigurationKind.Payment
                    ? $"إرجاع رسوم النشر للوراثة: {NameOf(tree, node)}"
                    : $"إرجاع ظهور {NameOf(tree, node)} في {AreaInfo[row.Area].NameAr} للوراثة",
                oldValue: kind == CatalogConfigurationKind.Payment ? DescribePayment(row) : DescribeVisibility(row),
                newValue: "inherit", adminUserId: adminUserId, cancellationToken: cancellationToken);
        }

        return await ResultAsync(targets, tree, 0, 0, doomed.Count, cancellationToken);
    }

    public CatalogConfigurationKind ParseKind(string? kind)
    {
        if (!string.IsNullOrWhiteSpace(kind))
        {
            var text = kind.Trim();

            if (int.TryParse(text, out var number) && Enum.IsDefined((CatalogConfigurationKind)number))
                return (CatalogConfigurationKind)number;

            if (Enum.TryParse<CatalogConfigurationKind>(text, ignoreCase: true, out var parsed) && Enum.IsDefined(parsed))
                return parsed;
        }

        throw new BadRequestException("نوع الإعدادات لازم يكون Payment أو Visibility.");
    }

    public async Task<CatalogEffectiveConfigurationDto> GetEffectiveAsync(
        CatalogEffectiveQuery query, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);

        ListingModuleType? module = null;

        if (!string.IsNullOrWhiteSpace(query.Type))
            module = ParseModule(query.Type) ?? throw new BadRequestException("نوع الإعلان غير معروف.");

        module ??= query.SubCategoryId is { } subCategoryId ? ListingModuleCatalog.ModuleOf(subCategoryId) : null;

        if (module is null && query.SubCategoryId is null && query.CategoryId is null)
            throw new BadRequestException("حدد نوع الإعلان أو القسم أو القسم الفرعي.");

        var context = new CatalogContext(module, query.SubCategoryId, query.CategoryId);
        var snapshot = await _provider.GetSnapshotAsync(cancellationToken);
        var now = DateTime.UtcNow;
        var node = context.Module is { } m
            ? new CatalogNodeRef(CatalogNodeType.Module, (int)m)
            : context.SubCategoryId is { } s
                ? new CatalogNodeRef(CatalogNodeType.SubCategory, s)
                : new CatalogNodeRef(CatalogNodeType.Category, context.CategoryId!.Value);

        return new CatalogEffectiveConfigurationDto
        {
            EvaluatedAt = now,
            AdvertisementType = context.Module,
            CategoryId = snapshot.CategoryOf(context),
            SubCategoryId = CatalogConfigurationSnapshot.SubCategoryOf(context),
            Chain = snapshot.ChainOf(context).Select(entry => entry.ToString()).Append("Global").ToList(),
            Payment = EffectivePayment(snapshot, context, node, now),
            Visibility = VisibilityOf(snapshot, context, node, now)
        };
    }

    public async Task<IReadOnlyList<CatalogScheduledConfigurationDto>> GetScheduledAsync(
        CancellationToken cancellationToken = default)
    {
        var snapshot = await _provider.GetSnapshotAsync(cancellationToken);
        var tree = await LoadTreeAsync(cancellationToken);
        var now = DateTime.UtcNow;

        return snapshot.Entries
            .Where(entry => entry.ActiveFrom > now || entry.ActiveUntil > now)
            .OrderBy(entry => entry.ActiveFrom > now ? entry.ActiveFrom : entry.ActiveUntil)
            .ThenBy(entry => entry.Id)
            .Select(entry => new CatalogScheduledConfigurationDto
            {
                ConfigurationId = entry.Id,
                Key = entry.Node.ToString(),
                Kind = entry.Kind.ToString(),
                AreaKey = entry.Area == CatalogArea.None ? null : entry.Area.ToString(),
                Name = NameOf(tree, entry.Node),
                IsEnabled = entry.IsEnabled,
                IsVisible = entry.IsVisible,
                PaymentAmount = entry.Price,
                Currency = entry.Currency,
                ActivationDate = entry.ActiveFrom,
                ExpirationDate = entry.ActiveUntil,
                ConfigurationStatus = entry.StatusAt(now)
            })
            .ToList();
    }

    public Task<PaginatedResult<AdminAuditLogDto>> GetHistoryAsync(
        AdminAuditLogFilterParams filter, CancellationToken cancellationToken = default)
    {
        filter.TargetType = AdminAuditCatalog.Targets.CatalogConfiguration;

        return _audit.GetAsync(filter, cancellationToken);
    }

    public async Task<PublicCatalogConfigurationDto> GetPublicAsync(CancellationToken cancellationToken = default)
    {
        var snapshot = await _provider.GetSnapshotAsync(cancellationToken);
        var tree = await LoadTreeAsync(cancellationToken);
        var now = DateTime.UtcNow;

        return new PublicCatalogConfigurationDto
        {
            GeneratedAt = now,
            CacheSeconds = (int)CatalogConfigurationProvider.CacheDuration.TotalSeconds,
            Categories = tree
                .Select(category => PublicNode(
                    snapshot, new CatalogNodeRef(CatalogNodeType.Category, category.Id),
                    CatalogContext.ForCategory(category.Id), category.Name, category.NameAr, now,
                    category.SubCategories
                        .Select(sub => PublicNode(
                            snapshot, new CatalogNodeRef(CatalogNodeType.SubCategory, sub.Id),
                            new CatalogContext(null, sub.Id, category.Id), sub.Name, sub.NameAr, now,
                            ModulesOf(sub.Id)
                                .Select(module => PublicNode(
                                    snapshot, new CatalogNodeRef(CatalogNodeType.Module, (int)module),
                                    new CatalogContext(module, sub.Id, category.Id),
                                    module.ToString(), ListingModuleCatalog.NameOf(module), now,
                                    Array.Empty<PublicCatalogNodeDto>(), module))
                                .ToList()))
                        .ToList()))
                .ToList()
        };
    }

    private static PublicCatalogNodeDto PublicNode(
        CatalogConfigurationSnapshot snapshot, CatalogNodeRef node, CatalogContext context, string name, string nameAr,
        DateTime now, IReadOnlyList<PublicCatalogNodeDto> children, ListingModuleType? module = null)
    {
        var payment = snapshot.ResolvePayment(context, now);

        return new PublicCatalogNodeDto
        {
            Key = node.ToString(),
            NodeType = node.Type,
            NodeId = node.Id,
            ModuleKey = module?.ToString(),
            Name = name,
            NameAr = nameAr,
            Areas = Areas
                .Select(area =>
                {
                    var visibility = snapshot.ResolveVisibility(context, area, now);

                    return new PublicCatalogAreaDto
                    {
                        Area = area.ToString(),
                        IsVisible = visibility.IsVisible,
                        IsEnabled = visibility.IsEnabled
                    };
                })
                .ToList(),
            Payment = new PublicCatalogPaymentDto
            {
                PaymentRequired = payment.IsRequired,
                PaymentAmount = payment.IsRequired ? payment.Price : null,
                Currency = payment.IsRequired ? payment.Currency : null,
                IsFree = payment.IsProtected
            },
            Children = children
        };
    }

    private CatalogNodeDto BuildCategory(CatalogConfigurationSnapshot snapshot, TreeCategory category, DateTime now)
    {
        var node = new CatalogNodeRef(CatalogNodeType.Category, category.Id);
        var context = CatalogContext.ForCategory(category.Id);

        var dto = Node(snapshot, node, context, category.Name, category.NameAr, null, now);

        dto.Children = category.SubCategories
            .Select(sub =>
            {
                var subNode = new CatalogNodeRef(CatalogNodeType.SubCategory, sub.Id);
                var subContext = new CatalogContext(null, sub.Id, category.Id);
                var subDto = Node(snapshot, subNode, subContext, sub.Name, sub.NameAr, node, now);

                subDto.Children = ModulesOf(sub.Id)
                    .Select(module => Node(
                        snapshot,
                        new CatalogNodeRef(CatalogNodeType.Module, (int)module),
                        new CatalogContext(module, sub.Id, category.Id),
                        module.ToString(), ListingModuleCatalog.NameOf(module), subNode, now))
                    .ToList();

                return subDto;
            })
            .ToList();

        return dto;
    }

    private static CatalogNodeDto Node(
        CatalogConfigurationSnapshot snapshot, CatalogNodeRef node, CatalogContext context, string name,
        string nameAr, CatalogNodeRef? parent, DateTime now)
    {
        var explicitPayment = snapshot.Find(CatalogConfigurationKind.Payment, node);
        var module = node.Type == CatalogNodeType.Module ? (ListingModuleType?)node.Id : null;

        return new CatalogNodeDto
        {
            Key = node.ToString(),
            NodeType = node.Type,
            NodeTypeName = node.Type.ToString(),
            NodeId = node.Id,
            ModuleKey = module?.ToString(),
            Name = name,
            NameAr = nameAr,
            ParentKey = parent?.ToString(),
            CategoryId = snapshot.CategoryOf(context),
            SubCategoryId = CatalogConfigurationSnapshot.SubCategoryOf(context),
            IsProtectedFree = snapshot.IsProtectedFree(context),
            IsSubjectToPayment = module is not { } m || AdPaymentCatalog.IsSubjectToPayment(m),
            Payment = new CatalogPaymentConfigurationDto
            {
                Explicit = explicitPayment is null ? null : new CatalogPaymentRuleDto
                {
                    ConfigurationId = explicitPayment.Id,
                    PaymentEnabled = explicitPayment.IsEnabled,
                    PaymentAmount = explicitPayment.Price,
                    Currency = explicitPayment.Currency,
                    PaymentActivationDate = explicitPayment.ActiveFrom,
                    PaymentExpirationDate = explicitPayment.ActiveUntil,
                    RequiredSince = explicitPayment.EnabledAt,
                    ConfigurationStatus = explicitPayment.StatusAt(now),
                    CreatedAt = explicitPayment.CreatedAt,
                    UpdatedAt = explicitPayment.UpdatedAt,
                    UpdatedBy = explicitPayment.UpdatedBy ?? explicitPayment.CreatedBy
                },
                Effective = EffectivePayment(snapshot, context, node, now)
            },
            Visibility = VisibilityOf(snapshot, context, node, now)
        };
    }

    private static CatalogEffectivePaymentDto EffectivePayment(
        CatalogConfigurationSnapshot snapshot, CatalogContext context, CatalogNodeRef node, DateTime now)
    {
        var effective = snapshot.ResolvePayment(context, now);
        var source = effective.Source is { } found ? snapshot.Find(CatalogConfigurationKind.Payment, found) : null;

        return new CatalogEffectivePaymentDto
        {
            PaymentEnabled = effective.IsEnabled,
            PaymentRequired = effective.IsRequired,
            PaymentAmount = effective.Price,
            Currency = effective.Currency,
            RequiredSince = effective.IsRequired ? effective.RequiredSince : null,
            IsProtectedFree = effective.IsProtected,
            InheritanceMode = effective.IsProtected ? CatalogConfigurationStatuses.Protected
                : effective.Source is null ? "Global"
                : effective.Source == node ? "Explicit"
                : "Inherited",
            SourceKey = effective.Source?.ToString() ?? (effective.IsProtected ? null : "Global"),
            ParentConfigurationId = effective.Source is { } s && s != node ? source?.Id : null,
            PriceSourceKey = effective.PriceSource?.ToString() ?? "Global",
            ConfigurationStatus = effective.IsProtected
                ? CatalogConfigurationStatuses.Protected
                : snapshot.Find(CatalogConfigurationKind.Payment, node)?.StatusAt(now) ?? CatalogConfigurationStatuses.Inherited
        };
    }

    private static IReadOnlyList<CatalogAreaVisibilityDto> VisibilityOf(
        CatalogConfigurationSnapshot snapshot, CatalogContext context, CatalogNodeRef node, DateTime now) =>
        Areas
            .Select(area =>
            {
                var own = snapshot.Find(CatalogConfigurationKind.Visibility, node, area);
                var effective = snapshot.ResolveVisibility(context, area, now);
                var source = effective.Source is { } found
                    ? snapshot.Find(CatalogConfigurationKind.Visibility, found, area)
                    : null;

                return new CatalogAreaVisibilityDto
                {
                    Area = area,
                    AreaKey = area.ToString(),
                    AreaName = AreaInfo[area].NameAr,
                    Explicit = own is null ? null : new CatalogVisibilityRuleDto
                    {
                        ConfigurationId = own.Id,
                        IsVisible = own.IsVisible ?? true,
                        IsEnabled = own.IsEnabled,
                        ActivationDate = own.ActiveFrom,
                        ExpirationDate = own.ActiveUntil,
                        ConfigurationStatus = own.StatusAt(now),
                        UpdatedAt = own.UpdatedAt ?? own.CreatedAt,
                        UpdatedBy = own.UpdatedBy ?? own.CreatedBy
                    },
                    IsVisible = effective.IsVisible,
                    IsEnabled = effective.IsEnabled,
                    InheritanceMode = effective.Source is null ? "Default"
                        : effective.Source == node ? "Explicit"
                        : "Inherited",
                    SourceKey = effective.Source?.ToString(),
                    ParentConfigurationId = effective.Source is { } s && s != node ? source?.Id : null
                };
            })
            .ToList();

    private CatalogNodeDto? Describe(
        CatalogConfigurationSnapshot snapshot, IReadOnlyList<TreeCategory> tree, CatalogNodeRef node, DateTime now)
    {
        switch (node.Type)
        {
            case CatalogNodeType.Category:
            {
                var category = tree.FirstOrDefault(entry => entry.Id == node.Id);

                return category is null ? null : BuildCategory(snapshot, category, now);
            }

            case CatalogNodeType.SubCategory:
            {
                var category = tree.FirstOrDefault(entry => entry.SubCategories.Any(sub => sub.Id == node.Id));

                return category is null
                    ? null
                    : BuildCategory(snapshot, category, now).Children.First(child => child.NodeId == node.Id);
            }

            default:
            {
                if (!Enum.IsDefined((ListingModuleType)node.Id))
                    return null;

                var module = (ListingModuleType)node.Id;
                var parent = ParentOf(tree, module);

                var dto = Node(
                    snapshot, node,
                    new CatalogContext(module, parent?.SubCategoryId, parent?.CategoryId),
                    module.ToString(), ListingModuleCatalog.NameOf(module),
                    parent is { } p ? new CatalogNodeRef(CatalogNodeType.SubCategory, p.SubCategoryId) : null, now);

                return dto;
            }
        }
    }

    private static (int CategoryId, int SubCategoryId)? ParentOf(IReadOnlyList<TreeCategory> tree, ListingModuleType module)
    {
        foreach (var category in tree)
            foreach (var sub in category.SubCategories)
                if (ModulesOf(sub.Id).Contains(module))
                    return (category.Id, sub.Id);

        return null;
    }

    private async Task<CatalogBulkResultDto> ResultAsync(
        IReadOnlyList<CatalogNodeRef> targets, IReadOnlyList<TreeCategory> tree, int created, int updated, int removed,
        CancellationToken cancellationToken)
    {
        var snapshot = await _provider.GetSnapshotAsync(cancellationToken);
        var now = DateTime.UtcNow;

        return new CatalogBulkResultDto
        {
            Created = created,
            Updated = updated,
            Removed = removed,
            Items = targets
                .Select(target => Describe(snapshot, tree, target, now))
                .Where(item => item is not null)
                .Select(item => item!)
                .ToList()
        };
    }

    private async Task SaveAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _repository.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception) when (_unitOfWork.IsUniqueConstraintViolation(exception))
        {
            throw new ConflictException("الإعدادات دي اتعدلت من مسؤول تاني في نفس اللحظة. حدّث الصفحة وحاول تاني.");
        }
        finally
        {
            _provider.Invalidate();
        }
    }

    private static IReadOnlyList<CatalogNodeRef> ResolveTargets(
        IReadOnlyList<CatalogNodeTargetRequest>? requested, IReadOnlyList<TreeCategory> tree)
    {
        if (requested is null || requested.Count == 0)
            throw new BadRequestException("اختار قسم أو وحدة واحدة على الأقل.");

        if (requested.Count > MaxTargetsPerRequest)
            throw new BadRequestException($"أقصى عدد للأقسام في الطلب الواحد {MaxTargetsPerRequest}.");

        var targets = new List<CatalogNodeRef>();
        var unknown = new List<string>();

        foreach (var target in requested)
        {
            CatalogNodeType type;

            try
            {
                type = ParseNodeType(target.NodeType);
            }
            catch (BadRequestException)
            {
                unknown.Add($"{target.NodeType}:{target.NodeId}");
                continue;
            }

            var node = new CatalogNodeRef(type, target.NodeId);

            if (!Exists(tree, node))
                unknown.Add(node.ToString());
            else if (!targets.Contains(node))
                targets.Add(node);
        }

        if (unknown.Count > 0)
            throw new BadRequestException("الأقسام أو الوحدات دي غير موجودة.", unknown);

        return targets;
    }

    private static bool Exists(IReadOnlyList<TreeCategory> tree, CatalogNodeRef node) => node.Type switch
    {
        CatalogNodeType.Category => tree.Any(category => category.Id == node.Id),
        CatalogNodeType.SubCategory => tree.Any(category => category.SubCategories.Any(sub => sub.Id == node.Id)),
        CatalogNodeType.Module => Enum.IsDefined((ListingModuleType)node.Id),
        _ => false
    };

    private static CatalogNodeType ParseNodeType(string? value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            var text = value.Trim();

            if (int.TryParse(text, out var number) && Enum.IsDefined((CatalogNodeType)number))
                return (CatalogNodeType)number;

            if (Enum.TryParse<CatalogNodeType>(text, ignoreCase: true, out var parsed) && Enum.IsDefined(parsed))
                return parsed;
        }

        throw new BadRequestException("نوع العنصر لازم يكون Category أو SubCategory أو Module.");
    }

    private static ListingModuleType? ParseModule(string value)
    {
        var text = value.Trim();

        if (int.TryParse(text, out var number))
            return Enum.IsDefined((ListingModuleType)number) ? (ListingModuleType)number : null;

        return Enum.TryParse<ListingModuleType>(text, ignoreCase: true, out var parsed) && Enum.IsDefined(parsed)
            ? parsed
            : null;
    }

    private static IReadOnlyList<CatalogArea> ParseAreas(IReadOnlyList<string>? values, bool required)
    {
        if (values is null || values.Count == 0)
        {
            if (required)
                throw new BadRequestException("اختار مكان ظهور واحد على الأقل.");

            return Array.Empty<CatalogArea>();
        }

        var areas = new List<CatalogArea>();
        var unknown = new List<string>();

        foreach (var value in values)
        {
            var text = value?.Trim() ?? string.Empty;

            CatalogArea? area = int.TryParse(text, out var number) && Enum.IsDefined((CatalogArea)number)
                ? (CatalogArea)number
                : Enum.TryParse<CatalogArea>(text, ignoreCase: true, out var parsed) && Enum.IsDefined(parsed)
                    ? parsed
                    : null;

            if (area is null or CatalogArea.None)
                unknown.Add(text);
            else if (!areas.Contains(area.Value))
                areas.Add(area.Value);
        }

        if (unknown.Count > 0)
            throw new BadRequestException("أماكن الظهور دي غير معروفة.", unknown);

        return areas;
    }

    private static DateTime? NormalizeUtc(DateTime? value, string label) => value switch
    {
        null => null,
        { Kind: DateTimeKind.Utc } utc => utc,
        { Kind: DateTimeKind.Local } local => local.ToUniversalTime(),
        _ => throw new BadRequestException($"{label} لازم يكون بتوقيت UTC (مثال: 2026-12-01T00:00:00Z).")
    };

    private static void ValidateWindow(DateTime? from, DateTime? until, DateTime now)
    {
        if (from is { } start && until is { } end && start >= end)
            throw new BadRequestException("تاريخ البداية لازم يكون قبل تاريخ النهاية.");

        if (until is { } expiry && expiry <= now)
            throw new BadRequestException("تاريخ النهاية لازم يكون في المستقبل.");
    }

    private static string? NormalizeCurrency(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        var currency = value.Trim().ToUpperInvariant();

        if (currency.Length != 3 || !currency.All(char.IsAsciiLetterUpper))
            throw new BadRequestException("العملة لازم تكون كود من 3 حروف زي EGP.");

        return currency;
    }

    private static CatalogContext ContextOf(CatalogNodeRef node) => node.Type switch
    {
        CatalogNodeType.Category => CatalogContext.ForCategory(node.Id),
        CatalogNodeType.SubCategory => CatalogContext.ForSubCategory(node.Id),
        _ => CatalogContext.ForModule((ListingModuleType)node.Id)
    };

    private static IReadOnlyList<ListingModuleType> ModulesOf(int subCategoryId) =>
        ListingModuleCatalog.ModuleOf(subCategoryId) is { } module ? new[] { module } : Array.Empty<ListingModuleType>();

    private static string NameOf(IReadOnlyList<TreeCategory> tree, CatalogNodeRef node) => node.Type switch
    {
        CatalogNodeType.Category => tree.FirstOrDefault(category => category.Id == node.Id)?.NameAr ?? node.ToString(),
        CatalogNodeType.SubCategory => tree.SelectMany(category => category.SubCategories)
            .FirstOrDefault(sub => sub.Id == node.Id)?.NameAr ?? node.ToString(),
        _ => ListingModuleCatalog.NameOf((ListingModuleType)node.Id)
    };

    private static string DescribePayment(CatalogConfiguration row) =>
        $"enabled={row.IsEnabled}; amount={(row.Price is { } price ? price.ToString("0.##") : "inherit")}; " +
        $"currency={row.Currency ?? "inherit"}; from={row.ActiveFrom:O}; until={row.ActiveUntil:O}";

    private static string DescribeVisibility(CatalogConfiguration row) =>
        $"area={row.Area}; visible={row.IsVisible}; enabled={row.IsEnabled}; from={row.ActiveFrom:O}; until={row.ActiveUntil:O}";

    private static IReadOnlyList<int> ParseIds(string? value) =>
        string.IsNullOrWhiteSpace(value)
            ? Array.Empty<int>()
            : value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(part => int.TryParse(part, out var id) ? id : 0)
                .Where(id => id > 0)
                .ToList();

    private async Task<IReadOnlyList<TreeCategory>> LoadTreeAsync(CancellationToken cancellationToken)
    {
        var tree = await _lookups.GetCategoriesTreeAsync(cancellationToken);

        var categories = tree
            .OrderBy(category => category.SortOrder)
            .Select(category => new TreeCategory(
                category.Id, category.Name, category.NameAr,
                category.SubCategories
                    .OrderBy(sub => sub.SortOrder)
                    .Select(sub => new TreeSubCategory(sub.Id, sub.Name, sub.NameAr))
                    .ToList()))
            .ToList();

        foreach (var entry in CategoryCatalog.Categories)
        {
            if (categories.All(category => category.Id != (int)entry.Id))
                categories.Add(new TreeCategory((int)entry.Id, entry.Name, entry.NameAr, new List<TreeSubCategory>()));
        }

        foreach (var entry in CategoryCatalog.SubCategories)
        {
            if (categories.Any(category => category.SubCategories.Any(sub => sub.Id == (int)entry.Id)))
                continue;

            var parent = categories.FirstOrDefault(category => category.Id == (int)entry.Category);

            parent?.SubCategories.Add(new TreeSubCategory((int)entry.Id, entry.Name, entry.NameAr));
        }

        return categories;
    }

    private sealed record TreeCategory(int Id, string Name, string NameAr, List<TreeSubCategory> SubCategories);

    private sealed record TreeSubCategory(int Id, string Name, string NameAr);
}
