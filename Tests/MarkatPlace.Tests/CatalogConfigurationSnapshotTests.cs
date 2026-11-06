using Services.AdPayments;
using Shared.Catalog;
using Shared.Constants;
using Shared.Enums;
using Xunit;

namespace MarkatPlace.Tests;

public class CatalogConfigurationSnapshotTests
{
    private static readonly DateTime Now = new(2026, 11, 6, 12, 0, 0, DateTimeKind.Utc);

    private const int HomeFurnishing = (int)CategoryType.HomeFurnishing;

    private const int FurnitureSub = (int)SubCategoryType.Furniture;

    private const int Charity = (int)CategoryType.Charity;

    private static int _ids;

    private static CatalogConfigurationEntry Payment(
        CatalogNodeType type, int id, bool enabled, decimal? price = null, DateTime? from = null,
        DateTime? until = null, DateTime? enabledAt = null, string? currency = null) =>
        new(++_ids, CatalogConfigurationKind.Payment, type, id, CatalogArea.None, enabled, null, price, currency,
            from, until, enabledAt ?? (enabled ? Now.AddDays(-1) : null), Now.AddDays(-2), null, null, null);

    private static CatalogConfigurationEntry Visibility(
        CatalogNodeType type, int id, CatalogArea area, bool visible, bool enabled = true,
        DateTime? from = null, DateTime? until = null) =>
        new(++_ids, CatalogConfigurationKind.Visibility, type, id, area, enabled, visible, null, null,
            from, until, null, Now.AddDays(-2), null, null, null);

    private static CatalogConfigurationSnapshot Snapshot(
        bool rootEnabled = false, decimal rootPrice = 0m, params CatalogConfigurationEntry[] entries) =>
        new(entries, new CatalogPaymentRoot(rootEnabled, rootPrice, "EGP", rootEnabled ? Now.AddDays(-10) : null),
            new Dictionary<int, int> { [FurnitureSub] = HomeFurnishing });

    private static CatalogContext Furniture => CatalogContext.ForModule(ListingModuleType.Furniture);

    [Fact]
    public void A_category_rule_is_inherited_by_its_sub_categories_and_modules()
    {
        var snapshot = Snapshot(false, 0m, Payment(CatalogNodeType.Category, HomeFurnishing, true, 75m));

        var module = snapshot.ResolvePayment(Furniture, Now);

        Assert.True(module.IsRequired);
        Assert.Equal(75m, module.Price);
        Assert.Equal(new CatalogNodeRef(CatalogNodeType.Category, HomeFurnishing), module.Source);
        Assert.True(snapshot.ResolvePayment(CatalogContext.ForSubCategory(FurnitureSub), Now).IsRequired);
        Assert.False(snapshot.ResolvePayment(CatalogContext.ForModule(ListingModuleType.Land), Now).IsRequired);
    }

    [Fact]
    public void A_sub_category_overrides_its_category_and_a_module_overrides_its_sub_category()
    {
        var snapshot = Snapshot(false, 0m,
            Payment(CatalogNodeType.Category, HomeFurnishing, true, 75m),
            Payment(CatalogNodeType.SubCategory, FurnitureSub, false));

        Assert.False(snapshot.ResolvePayment(Furniture, Now).IsRequired);

        var withModule = Snapshot(false, 0m,
            Payment(CatalogNodeType.Category, HomeFurnishing, true, 75m),
            Payment(CatalogNodeType.SubCategory, FurnitureSub, false),
            Payment(CatalogNodeType.Module, (int)ListingModuleType.Furniture, true, 20m));

        var module = withModule.ResolvePayment(Furniture, Now);

        Assert.True(module.IsRequired);
        Assert.Equal(20m, module.Price);
        Assert.Equal(new CatalogNodeRef(CatalogNodeType.Module, (int)ListingModuleType.Furniture), module.Source);
    }

    [Fact]
    public void Without_explicit_rules_everything_follows_the_global_switch_and_price()
    {
        var on = Snapshot(true, 150m).ResolvePayment(Furniture, Now);

        Assert.True(on.IsRequired);
        Assert.Equal(150m, on.Price);
        Assert.Null(on.Source);
        Assert.Null(on.PriceSource);

        Assert.False(Snapshot().ResolvePayment(Furniture, Now).IsRequired);
    }

    [Fact]
    public void A_rule_without_a_price_inherits_the_nearest_price_up_the_chain()
    {
        var snapshot = Snapshot(true, 150m,
            Payment(CatalogNodeType.Category, HomeFurnishing, true, 60m, currency: "USD"),
            Payment(CatalogNodeType.Module, (int)ListingModuleType.Furniture, true));

        var module = snapshot.ResolvePayment(Furniture, Now);

        Assert.Equal(60m, module.Price);
        Assert.Equal("USD", module.Currency);
        Assert.Equal(new CatalogNodeRef(CatalogNodeType.Category, HomeFurnishing), module.PriceSource);
    }

    [Fact]
    public void An_enabled_rule_whose_effective_price_is_zero_never_requires_payment()
    {
        var snapshot = Snapshot(false, 0m, Payment(CatalogNodeType.Category, HomeFurnishing, true));

        var module = snapshot.ResolvePayment(Furniture, Now);

        Assert.True(module.IsEnabled);
        Assert.False(module.IsRequired);
    }

    [Fact]
    public void A_future_activation_and_a_past_expiration_both_fall_back_to_the_parent()
    {
        var scheduled = Snapshot(false, 0m,
            Payment(CatalogNodeType.Category, HomeFurnishing, true, 75m, from: Now.AddDays(1)));

        Assert.False(scheduled.ResolvePayment(Furniture, Now).IsRequired);
        Assert.True(scheduled.ResolvePayment(Furniture, Now.AddDays(2)).IsRequired);

        var expiring = Snapshot(false, 0m,
            Payment(CatalogNodeType.Category, HomeFurnishing, true, 75m, until: Now.AddDays(1)));

        Assert.True(expiring.ResolvePayment(Furniture, Now).IsRequired);
        Assert.False(expiring.ResolvePayment(Furniture, Now.AddDays(1)).IsRequired);

        var disabledLater = Snapshot(true, 100m,
            Payment(CatalogNodeType.Category, HomeFurnishing, false, from: Now.AddHours(1)));

        Assert.True(disabledLater.ResolvePayment(Furniture, Now).IsRequired);
        Assert.False(disabledLater.ResolvePayment(Furniture, Now.AddHours(2)).IsRequired);
    }

    [Fact]
    public void The_rule_start_used_for_grandfathering_comes_from_the_deciding_rule()
    {
        var since = Now.AddDays(-3);
        var snapshot = Snapshot(false, 0m,
            Payment(CatalogNodeType.Category, HomeFurnishing, true, 75m, enabledAt: since));

        Assert.Equal(since, snapshot.ResolvePayment(Furniture, Now).RequiredSince);
        Assert.Equal(Now.AddDays(-10), Snapshot(true, 10m).ResolvePayment(Furniture, Now).RequiredSince);
    }

    [Theory]
    [InlineData(ListingModuleType.Rescue)]
    [InlineData(ListingModuleType.BloodRequest)]
    [InlineData(ListingModuleType.AskConsult)]
    public void The_charity_category_and_every_descendant_stay_free_whatever_is_configured(ListingModuleType module)
    {
        var snapshot = Snapshot(true, 100m,
            Payment(CatalogNodeType.Category, Charity, true, 50m),
            Payment(CatalogNodeType.Module, (int)module, true, 50m));

        var effective = snapshot.ResolvePayment(CatalogContext.ForModule(module), Now);

        Assert.True(effective.IsProtected);
        Assert.False(effective.IsRequired);
        Assert.False(AdPaymentCatalog.IsSubjectToPayment(module));
        Assert.True(snapshot.ResolvePayment(CatalogContext.ForCategory(Charity), Now).IsProtected);
        Assert.True(snapshot.ResolvePayment(
            CatalogContext.ForSubCategory((int)ListingModuleCatalog.SubCategoryOf(module)!.Value), Now).IsProtected);
    }

    [Fact]
    public void Visibility_defaults_to_shown_inherits_per_area_and_respects_child_overrides()
    {
        var snapshot = Snapshot(false, 0m,
            Visibility(CatalogNodeType.Category, HomeFurnishing, CatalogArea.HomePage, visible: false),
            Visibility(CatalogNodeType.Module, (int)ListingModuleType.Furniture, CatalogArea.HomePage, visible: true),
            Visibility(CatalogNodeType.Category, HomeFurnishing, CatalogArea.AdCreation, visible: true, enabled: false));

        Assert.False(snapshot.IsVisible(CatalogContext.ForCategory(HomeFurnishing), CatalogArea.HomePage, Now));
        Assert.False(snapshot.IsVisible(CatalogContext.ForSubCategory(FurnitureSub), CatalogArea.HomePage, Now));
        Assert.True(snapshot.IsVisible(Furniture, CatalogArea.HomePage, Now));
        Assert.True(snapshot.IsVisible(CatalogContext.ForCategory(HomeFurnishing), CatalogArea.Search, Now));
        Assert.False(snapshot.IsVisible(Furniture, CatalogArea.AdCreation, Now));

        var enabledOnly = snapshot.ResolveVisibility(Furniture, CatalogArea.AdCreation, Now);
        Assert.True(enabledOnly.IsVisible);
        Assert.False(enabledOnly.IsEnabled);
    }

    [Fact]
    public void Scheduled_visibility_applies_only_inside_its_window()
    {
        var snapshot = Snapshot(false, 0m,
            Visibility(CatalogNodeType.Category, HomeFurnishing, CatalogArea.CategoryListing, visible: false,
                from: Now.AddHours(1), until: Now.AddHours(3)));

        var context = CatalogContext.ForCategory(HomeFurnishing);

        Assert.True(snapshot.IsVisible(context, CatalogArea.CategoryListing, Now));
        Assert.False(snapshot.IsVisible(context, CatalogArea.CategoryListing, Now.AddHours(2)));
        Assert.True(snapshot.IsVisible(context, CatalogArea.CategoryListing, Now.AddHours(3)));
    }

    [Fact]
    public void Queue_rules_cover_each_car_sub_category_separately()
    {
        var snapshot = Snapshot(false, 0m,
            Payment(CatalogNodeType.SubCategory, (int)SubCategoryType.Taxi, true, 40m));

        var rules = AdPaymentGate.BuildQueueRules(snapshot, Now);

        var cars = Assert.Single(rules[ListingModuleType.Advertisement]);
        Assert.Equal(new int?[] { (int)SubCategoryType.Taxi }, cars.SubCategoryIds);
        Assert.False(rules.ContainsKey(ListingModuleType.Furniture));
    }

    [Fact]
    public void Queue_rules_never_include_the_charity_modules_even_when_everything_is_paid()
    {
        var rules = AdPaymentGate.BuildQueueRules(Snapshot(true, 10m), Now);

        Assert.DoesNotContain(ListingModuleType.Rescue, rules.Keys);
        Assert.DoesNotContain(ListingModuleType.BloodRequest, rules.Keys);
        Assert.DoesNotContain(ListingModuleType.AskConsult, rules.Keys);
        Assert.Contains(ListingModuleType.Furniture, rules.Keys);
    }
}
