using FluentValidation.Results;
using Services.Validation;
using Shared.DTOs.RealEstate;
using Shared.Enums;
using Xunit;

namespace MarkatPlace.Tests;

public class LandValidationTests
{
    private static readonly CreateLandRequestValidator Validator = new();

    private static CreateLandRequest Valid() => new()
    {
        Title = "أرض سكنية مميزة للبيع في سنورس",
        Description = "أرض على شارع رئيسي، مرافق متكاملة، قريبة من الخدمات.",
        AdvertiserName = "محمد عبد الرحمن",
        ListingType = RealEstateListingType.Sale,
        LandType = LandType.Residential,
        AreaUnit = LandAreaUnit.SquareMeter,
        Area = 500m,
        PricePerMeter = 4500m,
        TotalPrice = 2_250_000m,
        Length = 25m,
        Width = 20m,
        FacadeLength = 20m,
        FacadesCount = LandFacadesCount.One,
        Direction = LandDirection.North,
        StreetWidth = 12m,
        RoadType = LandRoadType.Asphalt,
        LegalStatus = LandLegalStatus.Unlicensed,
        OwnershipDocument = LandOwnershipDocument.FinalContract,
        Center = "سنورس",
        Address = "شارع الجمهورية، بجوار مسجد النور",
        Phone = "01012345678",
        WhatsApp = "01012345678",
        Negotiable = true,
    };

    private static ValidationResult Validate(Action<CreateLandRequest>? change = null)
    {
        var request = Valid();
        change?.Invoke(request);
        return Validator.Validate(request);
    }

    private static void AssertMessagesAreArabic(ValidationResult result)
    {
        foreach (var failure in result.Errors)
        {
            Assert.True(ArabicText.IsArabic(failure.ErrorMessage),
                $"'{failure.PropertyName}' answered in a language the seller cannot read: " +
                $"\"{failure.ErrorMessage}\"");
        }
    }

    [Fact]
    public void A_listing_a_real_seller_would_post_is_accepted()
    {
        var result = Validate();

        Assert.True(result.IsValid,
            "a complete, ordinary أراضي listing was rejected: " +
            string.Join(" | ", result.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}")));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void A_listing_with_no_title_is_rejected_in_Arabic(string title)
    {
        var result = Validate(request => request.Title = title);

        Assert.False(result.IsValid);
        AssertMessagesAreArabic(result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-500)]
    public void An_area_that_is_not_a_positive_number_is_rejected(decimal area)
    {
        var result = Validate(request => request.Area = area);

        Assert.False(result.IsValid);
        AssertMessagesAreArabic(result);
    }

    [Theory]
    [InlineData("12345")]
    [InlineData("01712345678")]
    [InlineData("0101234567")]
    public void A_contact_number_that_is_not_an_Egyptian_mobile_is_rejected(string phone)
    {
        var result = Validate(request => request.Phone = phone);

        Assert.False(result.IsValid);
        AssertMessagesAreArabic(result);
    }

    [Theory]
    [InlineData("01012345678")]
    [InlineData("01112345678")]
    [InlineData("01212345678")]
    [InlineData("01512345678")]
    public void Every_Egyptian_operator_is_accepted_as_a_contact_number(string phone)
    {
        var result = Validate(request => request.Phone = phone);

        Assert.True(result.IsValid,
            string.Join(" | ", result.Errors.Select(e => e.ErrorMessage)));
    }

    [Fact]
    public void A_center_outside_Fayoum_is_rejected()
    {
        var result = Validate(request => request.Center = "القاهرة");

        Assert.False(result.IsValid);
        AssertMessagesAreArabic(result);
    }

    [Fact]
    public void Choosing_Other_as_the_land_type_makes_the_written_name_mandatory()
    {
        var missing = Validate(request =>
        {
            request.LandType = LandType.Other;
            request.OtherLandType = null;
        });
        Assert.False(missing.IsValid);
        AssertMessagesAreArabic(missing);

        var supplied = Validate(request =>
        {
            request.LandType = LandType.Other;
            request.OtherLandType = "أرض مقابر";
        });
        Assert.True(supplied.IsValid,
            string.Join(" | ", supplied.Errors.Select(e => e.ErrorMessage)));
    }

    [Fact]
    public void A_rental_listing_does_not_have_to_carry_a_sale_price()
    {
        var result = Validate(request =>
        {
            request.ListingType = RealEstateListingType.Rent;
            request.TotalPrice = null;
            request.PricePerMeter = null;
            request.RentType = LandRentType.Yearly;
            request.RentValue = 60_000m;
        });

        Assert.True(result.IsValid,
            string.Join(" | ", result.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}")));
    }

    [Fact]
    public void A_rental_listing_must_say_what_the_rent_is()
    {
        var result = Validate(request =>
        {
            request.ListingType = RealEstateListingType.Rent;
            request.TotalPrice = null;
            request.PricePerMeter = null;
            request.RentType = null;
            request.RentValue = null;
        });

        Assert.False(result.IsValid);
        AssertMessagesAreArabic(result);
    }

    [Fact]
    public void No_failure_ever_names_a_property_instead_of_the_field_the_user_saw()
    {
        var result = Validate(request =>
        {
            request.Title = string.Empty;
            request.Area = 0;
            request.Phone = "nope";
            request.Center = "الإسكندرية";
            request.LandType = LandType.Other;
        });

        Assert.False(result.IsValid);
        AssertMessagesAreArabic(result);

        foreach (var failure in result.Errors)
        {
            foreach (var identifier in new[]
                     {
                         "LandType", "AreaUnit", "TotalPrice", "FacadeLength", "OwnershipDocument",
                         "is required", "must be", "Invalid",
                     })
            {
                Assert.DoesNotContain(identifier, failure.ErrorMessage, StringComparison.Ordinal);
            }
        }
    }

    [Fact]
    public void The_update_rules_accept_exactly_what_the_create_rules_accept()
    {
        var create = Valid();
        var update = new UpdateLandRequest();

        foreach (var property in typeof(CreateLandRequest).GetProperties()
                     .Where(p => p.CanRead && p.CanWrite))
        {
            var target = typeof(UpdateLandRequest).GetProperty(property.Name);
            if (target?.CanWrite == true)
                target.SetValue(update, property.GetValue(create));
        }

        var result = new UpdateLandRequestValidator().Validate(update);

        Assert.True(result.IsValid,
            string.Join(" | ", result.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}")));
    }
}
