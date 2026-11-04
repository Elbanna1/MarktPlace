using Services.AdPayments;
using Shared.Constants;
using Shared.Enums;
using Xunit;

namespace MarkatPlace.Tests;

public class AdPaymentCatalogTests
{
    [Theory]
    [InlineData(false, null, false, AdPaymentCatalog.Stages.NotRequired)]
    [InlineData(true, null, false, AdPaymentCatalog.Stages.AwaitingPayment)]
    [InlineData(true, PaymentStatus.Pending, false, AdPaymentCatalog.Stages.PaymentUnderReview)]
    [InlineData(false, PaymentStatus.Pending, false, AdPaymentCatalog.Stages.PaymentUnderReview)]
    [InlineData(true, PaymentStatus.Rejected, false, AdPaymentCatalog.Stages.PaymentRejected)]
    [InlineData(false, PaymentStatus.Rejected, false, AdPaymentCatalog.Stages.NotRequired)]
    [InlineData(false, PaymentStatus.Approved, true, AdPaymentCatalog.Stages.Paid)]
    [InlineData(true, PaymentStatus.Approved, false, AdPaymentCatalog.Stages.AwaitingPayment)]
    public void The_stage_is_resolved_from_requirement_latest_status_and_credit(
        bool required, PaymentStatus? latest, bool hasCredit, string expected)
    {
        Assert.Equal(expected, AdPaymentCatalog.ResolveStage(required, latest, hasCredit));
    }

    [Fact]
    public void Every_listing_module_is_subject_to_payment()
    {
        Assert.Equal(ListingModuleCatalog.All.Count, AdPaymentCatalog.SubjectModules.Count);
        Assert.All(ListingModuleCatalog.All, type => Assert.True(AdPaymentCatalog.IsSubjectToPayment(type)));
        Assert.False(AdPaymentCatalog.IsSubjectToPayment((ListingModuleType)9999));
    }

    [Fact]
    public void Every_sub_category_form_carries_a_payment_section_for_its_module()
    {
        foreach (var subCategory in CategoryCatalog.SubCategories)
        {
            var descriptor = AdPaymentFormDescriptor.For((int)subCategory.Id);

            Assert.NotNull(descriptor);
            Assert.Equal(ListingModuleCatalog.ModuleOf(subCategory.Id), descriptor!.AdvertisementType);
            Assert.True(descriptor.IsSubjectToPayment);
            Assert.Equal(AdPaymentCatalog.ConfigRoute, descriptor.ConfigEndpoint);
            Assert.Equal("multipart/form-data", descriptor.ContentType);

            var names = descriptor.Fields.Select(field => field.Name).ToList();
            Assert.Contains(AdPaymentCatalog.Fields.PaymentMethodId, names);
            Assert.Contains(AdPaymentCatalog.Fields.Screenshot, names);
            Assert.All(descriptor.Fields, field => Assert.Equal(AdPaymentCatalog.FormSection, field.Section));
        }

        Assert.Null(AdPaymentFormDescriptor.For((int?)null));
    }

    [Fact]
    public void Grandfathering_only_covers_listings_awaiting_their_first_approval()
    {
        var enabledAt = new DateTime(2026, 11, 1, 0, 0, 0, DateTimeKind.Utc);
        var before = enabledAt.AddDays(-1);
        var after = enabledAt.AddDays(1);

        Assert.True(AdPaymentRules.IsGrandfathered(enabledAt, ModerationStatus.Pending, before));
        Assert.True(AdPaymentRules.IsGrandfathered(enabledAt, ModerationStatus.Rejected, before));
        Assert.False(AdPaymentRules.IsGrandfathered(enabledAt, ModerationStatus.Approved, before));
        Assert.False(AdPaymentRules.IsGrandfathered(enabledAt, ModerationStatus.Pending, after));
        Assert.False(AdPaymentRules.IsGrandfathered(null, ModerationStatus.Pending, before));
    }

    [Fact]
    public void Payment_routes_are_built_from_the_module_name()
    {
        var id = Guid.Parse("11111111-2222-3333-4444-555555555555");

        Assert.Equal($"/api/v2/ads/Furniture/{id}/payment",
            AdPaymentCatalog.SubmitRouteOf(ListingModuleType.Furniture, id));
        Assert.Equal($"/api/v2/ads/Furniture/{id}/payment/resubmit",
            AdPaymentCatalog.ResubmitRouteOf(ListingModuleType.Furniture, id));
    }
}
