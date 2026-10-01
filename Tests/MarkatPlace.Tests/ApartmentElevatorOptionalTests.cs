using System.Text.Json;
using Domain.Entities;
using FluentValidation.Results;
using Services.AdForms;
using Services.Validation;
using Shared.DTOs.Lookups.Forms;
using Shared.DTOs.RealEstate;
using Shared.Enums;
using Xunit;

namespace MarkatPlace.Tests;

public class ApartmentElevatorOptionalTests
{
    private const string Elevator = "HasElevator";

    private const int RealEstateCategoryId = 11;
    private const int ApartmentsSubCategoryId = (int)SubCategoryType.Apartments;

    private static readonly CreateApartmentRequestValidator CreateValidator = new();
    private static readonly UpdateApartmentRequestValidator UpdateValidator = new();

    private static CreateApartmentRequest Sale()
    {
        var request = Shared();

        request.ListingType = RealEstateListingType.Sale;
        request.Price = 1_400_000m;
        request.PricePerMeter = 12_000m;
        request.PaymentMethod = ApartmentPaymentMethod.Cash;
        request.MonthlyFees = 250m;

        return request;
    }

    private static CreateApartmentRequest Rent()
    {
        var request = Shared();

        request.ListingType = RealEstateListingType.Rent;
        request.RentType = ApartmentRentType.Monthly;
        request.RentValue = 6_000m;
        request.SecurityDeposit = 12_000m;
        request.RentDownPayment = 6_000m;
        request.MinimumRentPeriod = 6;
        request.AvailableFrom = new DateTime(2026, 11, 1);
        request.AvailableTo = new DateTime(2027, 11, 1);
        request.OwnerConditions = "بدون حيوانات، السكن للعائلات فقط.";
        request.RentInclusions = [ApartmentRentInclusion.Water, ApartmentRentInclusion.Electricity];

        return request;
    }

    private static CreateApartmentRequest Exchange()
    {
        var request = Shared();

        request.ListingType = RealEstateListingType.Exchange;
        request.ExchangeWith = ApartmentExchangeWith.Apartment;
        request.AcceptsDifferencePayment = false;
        request.ExchangeDetails = "بدل بشقة أكبر في نفس المنطقة.";

        return request;
    }

    private static CreateApartmentRequest Shared() =>
        new()
        {
            Title = "شقة في مدينة الفيوم",
            Description = "شقة تشطيب سوبر لوكس، دور ثالث، قريبة من الخدمات.",
            AdvertiserName = "محمود السيد",
            ApartmentType = ApartmentType.Residential,
            OwnershipType = ApartmentOwnershipType.Freehold,
            Area = 120m,
            RoomsCount = 3,
            BathroomsCount = 2,
            ReceptionPieces = ApartmentReceptionPieces.Two,
            FloorType = ApartmentFloorType.Repeated,
            FloorNumber = 3,
            TotalFloors = 8,
            ApartmentsPerFloor = 2,
            FurnishedStatus = ApartmentFurnishedStatus.No,
            FinishingType = ApartmentFinishingType.SuperLux,
            PropertyAge = ApartmentPropertyAge.New,
            Direction = ApartmentDirection.North,
            LegalStatus = ApartmentLegalStatus.Licensed,
            OwnershipDocument = ApartmentOwnershipDocument.FinalContract,
            Center = "الفيوم",
            Address = "شارع الحرية، بجوار مدرسة النصر",
            Phone = "01012345678"
        };

    private static UpdateApartmentRequest AsUpdate(CreateApartmentRequest source)
    {
        var update = new UpdateApartmentRequest();

        foreach (var property in typeof(CreateApartmentRequest).GetProperties())
        {
            if (property.CanRead && property.CanWrite)
                property.SetValue(update, property.GetValue(source));
        }

        return update;
    }

    private static string Failures(ValidationResult result) =>
        string.Join(" | ", result.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}"));

    public static TheoryData<string, CreateApartmentRequest> EveryListingType() =>
        new()
        {
            { "بيع", Sale() },
            { "إيجار", Rent() },
            { "بدل", Exchange() }
        };

    [Theory]
    [MemberData(nameof(EveryListingType))]
    public void An_unanswered_elevator_question_is_accepted_for_every_listing_type(
        string listingType, CreateApartmentRequest request)
    {
        request.HasElevator = null;

        var result = CreateValidator.Validate(request);

        Assert.True(result.IsValid,
            $"'{listingType}' still blocks a listing that left the elevator unanswered: {Failures(result)}");
    }

    [Theory]
    [MemberData(nameof(EveryListingType))]
    public void An_unchecked_elevator_checkbox_is_accepted_for_every_listing_type(
        string listingType, CreateApartmentRequest request)
    {
        request.HasElevator = false;

        var result = CreateValidator.Validate(request);

        Assert.True(result.IsValid,
            $"'{listingType}' refuses an unchecked elevator checkbox: {Failures(result)}");
    }

    [Theory]
    [MemberData(nameof(EveryListingType))]
    public void A_checked_elevator_checkbox_is_still_accepted_for_every_listing_type(
        string listingType, CreateApartmentRequest request)
    {
        request.HasElevator = true;

        var result = CreateValidator.Validate(request);

        Assert.True(result.IsValid,
            $"'{listingType}' refuses a checked elevator checkbox: {Failures(result)}");
    }

    [Theory]
    [MemberData(nameof(EveryListingType))]
    public void An_edit_that_leaves_the_elevator_unanswered_is_accepted(
        string listingType, CreateApartmentRequest request)
    {
        var update = AsUpdate(request);
        update.HasElevator = null;

        var result = UpdateValidator.Validate(update);

        Assert.True(result.IsValid,
            $"editing a '{listingType}' listing still demands an elevator answer: {Failures(result)}");
    }

    [Fact]
    public void No_validator_message_mentions_the_elevator()
    {
        foreach (var request in new[] { Sale(), Rent(), Exchange() })
        {
            request.HasElevator = null;

            var result = CreateValidator.Validate(request);

            Assert.DoesNotContain(result.Errors,
                e => e.PropertyName == nameof(CreateApartmentRequest.HasElevator));
        }
    }

    private static FormFieldDto PublishedElevator()
    {
        var schema = AdFormSchemaCatalog.GetSchema(RealEstateCategoryId, ApartmentsSubCategoryId);
        Assert.NotNull(schema);

        var field = schema!.Fields.SingleOrDefault(f => f.Name == Elevator);
        Assert.True(field is not null, "the apartment form no longer publishes a 'HasElevator' field");
        return field!;
    }

    [Fact]
    public void The_published_elevator_field_is_never_required()
    {
        var field = PublishedElevator();

        Assert.False(field.Required);
        Assert.Null(field.RequiredWhen);
    }

    [Fact]
    public void The_published_elevator_field_stays_visible_as_a_checkbox()
    {
        var field = PublishedElevator();

        Assert.True(field.Visible);
        Assert.Null(field.VisibleWhen);
        Assert.Equal(FormFieldTypes.Checkbox, field.Type);
        Assert.Equal("يوجد أسانسير", field.Label);
    }

    [Fact]
    public void The_published_elevator_field_preselects_nothing()
    {
        Assert.Null(PublishedElevator().DefaultValue);
    }

    [Fact]
    public void The_elevator_field_the_frontend_receives_carries_no_required_flag()
    {
        var json = JsonSerializer.Serialize(PublishedElevator(),
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        using var document = JsonDocument.Parse(json);
        var root = document.RootElement;

        Assert.False(root.GetProperty("required").GetBoolean());
        Assert.False(root.TryGetProperty("requiredWhen", out _));
        Assert.False(root.TryGetProperty("visibleWhen", out _));
        Assert.False(root.TryGetProperty("defaultValue", out _));
    }

    [Fact]
    public void An_admin_override_cannot_require_the_elevator_again()
    {
        var merged = AdFormOverrideApplier.Apply(
            [PublishedElevator()],
            [
                new AdFormFieldOverride
                {
                    Id = Guid.NewGuid(),
                    CategoryId = RealEstateCategoryId,
                    SubCategoryId = ApartmentsSubCategoryId,
                    FieldName = Elevator,
                    IsRequired = true
                }
            ]);

        var field = Assert.Single(merged);

        Assert.False(field.Required, "a stored admin override can still force the elevator answer");
        Assert.Null(field.RequiredWhen);
    }

    [Fact]
    public void An_admin_override_cannot_attach_a_required_condition_to_the_elevator()
    {
        var merged = AdFormOverrideApplier.Apply(
            [PublishedElevator()],
            [
                new AdFormFieldOverride
                {
                    Id = Guid.NewGuid(),
                    CategoryId = RealEstateCategoryId,
                    SubCategoryId = ApartmentsSubCategoryId,
                    FieldName = Elevator,
                    RequiredWhenField = "ListingType",
                    RequiredWhenValues = "[1]"
                }
            ]);

        Assert.Null(Assert.Single(merged).RequiredWhen);
    }

    [Fact]
    public void An_admin_override_can_still_relabel_the_elevator()
    {
        var merged = AdFormOverrideApplier.Apply(
            [PublishedElevator()],
            [
                new AdFormFieldOverride
                {
                    Id = Guid.NewGuid(),
                    CategoryId = RealEstateCategoryId,
                    SubCategoryId = ApartmentsSubCategoryId,
                    FieldName = Elevator,
                    Label = "يوجد مصعد"
                }
            ]);

        Assert.Equal("يوجد مصعد", Assert.Single(merged).Label);
    }

    [Fact]
    public void The_elevator_is_the_only_field_the_platform_pins_to_optional()
    {
        Assert.True(AdFormSchemaCatalog.IsAlwaysOptional(Elevator));
        Assert.True(AdFormSchemaCatalog.IsAlwaysOptional("haselevator"));

        Assert.False(AdFormSchemaCatalog.IsAlwaysOptional("HasMaintenanceDeposit"));
        Assert.False(AdFormSchemaCatalog.IsAlwaysOptional("Area"));
        Assert.False(AdFormSchemaCatalog.IsAlwaysOptional(null));
    }

    [Fact]
    public void Making_the_elevator_optional_left_the_rest_of_the_apartment_form_alone()
    {
        var schema = AdFormSchemaCatalog.GetSchema(RealEstateCategoryId, ApartmentsSubCategoryId);
        Assert.NotNull(schema);

        foreach (var name in new[] { "Title", "Description", "AdvertiserName", "Address", "Phone", "Area" })
        {
            var field = schema!.Fields.Single(f => f.Name == name);
            Assert.True(field.Required, $"the apartment form stopped requiring '{name}'");
        }

        var deposit = schema!.Fields.Single(f => f.Name == "HasMaintenanceDeposit");

        Assert.False(deposit.Required);
        Assert.NotNull(deposit.VisibleWhen);
        Assert.Equal("ListingType", deposit.VisibleWhen!.Field);
    }

    [Fact]
    public void A_sale_still_refuses_to_lose_a_genuinely_required_answer()
    {
        var request = Sale();
        request.HasElevator = null;
        request.LegalStatus = null;

        var result = CreateValidator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(CreateApartmentRequest.LegalStatus));
    }
}
