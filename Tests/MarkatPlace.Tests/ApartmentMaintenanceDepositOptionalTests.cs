using System.Text.Json;
using Services.AdForms;
using Services.Validation;
using Shared.DTOs.Lookups.Forms;
using Shared.DTOs.RealEstate;
using Shared.Enums;
using Xunit;

namespace MarkatPlace.Tests;

public class ApartmentMaintenanceDepositOptionalTests
{
    private const string Deposit = "HasMaintenanceDeposit";
    private const string Elevator = "HasElevator";
    private const string DepositAmount = "MaintenanceDepositAmount";

    private const int RealEstateCategoryId = 11;
    private const int ApartmentsSubCategoryId = (int)SubCategoryType.Apartments;
    private const int CharityCategoryId = 12;
    private const int RescuesSubCategoryId = (int)SubCategoryType.Rescues;

    private const string AmountMissing = "من فضلك اكتب قيمة وديعة الصيانة.";
    private const string AmountNotPositive = "قيمة وديعة الصيانة يجب أن تكون أكبر من صفر.";

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

    private static string Failures(FluentValidation.Results.ValidationResult result) =>
        string.Join(" | ", result.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}"));

    private static IReadOnlyList<FormFieldDto> PublishedFields(int categoryId, int subCategoryId)
    {
        var schema = AdFormSchemaCatalog.GetSchema(categoryId, subCategoryId);
        Assert.NotNull(schema);
        return schema!.Fields;
    }

    private static FormFieldDto Published(string name)
    {
        var field = PublishedFields(RealEstateCategoryId, ApartmentsSubCategoryId)
            .SingleOrDefault(f => f.Name == name);

        Assert.True(field is not null, $"the apartment form no longer publishes a '{name}' field");
        return field!;
    }

    [Fact]
    public void Listing_type_one_is_Sale_which_is_what_the_published_conditions_key_on()
    {
        Assert.Equal(1, (int)RealEstateListingType.Sale);

        var visibleWhen = Published(Deposit).VisibleWhen;

        Assert.NotNull(visibleWhen);
        Assert.Equal("ListingType", visibleWhen!.Field);
        Assert.Equal<object>([(int)RealEstateListingType.Sale], visibleWhen.Values);
    }

    [Fact]
    public void A_sale_that_leaves_the_deposit_question_unanswered_is_accepted()
    {
        var request = Sale();
        request.HasMaintenanceDeposit = null;

        var result = CreateValidator.Validate(request);

        Assert.True(result.IsValid,
            $"a sale is still blocked when the deposit question is unanswered: {Failures(result)}");
    }

    [Fact]
    public void A_sale_with_an_unchecked_deposit_checkbox_is_accepted()
    {
        var request = Sale();
        request.HasMaintenanceDeposit = false;

        var result = CreateValidator.Validate(request);

        Assert.True(result.IsValid,
            $"a sale refuses an unchecked deposit checkbox: {Failures(result)}");
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData(false, false)]
    [InlineData(null, false)]
    [InlineData(false, null)]
    public void A_sale_that_leaves_both_checkboxes_off_is_accepted(bool? elevator, bool? deposit)
    {
        var request = Sale();
        request.HasElevator = elevator;
        request.HasMaintenanceDeposit = deposit;

        var result = CreateValidator.Validate(request);

        Assert.True(result.IsValid,
            $"a sale with elevator={elevator?.ToString() ?? "null"} and " +
            $"deposit={deposit?.ToString() ?? "null"} was rejected: {Failures(result)}");

        Assert.DoesNotContain(result.Errors,
            e => e.PropertyName == nameof(CreateApartmentRequest.HasElevator)
                 || e.PropertyName == nameof(CreateApartmentRequest.HasMaintenanceDeposit)
                 || e.PropertyName == nameof(CreateApartmentRequest.MaintenanceDepositAmount));
    }

    [Theory]
    [InlineData(null)]
    [InlineData(false)]
    public void An_unchecked_deposit_never_demands_an_amount(bool? deposit)
    {
        var request = Sale();
        request.HasMaintenanceDeposit = deposit;
        request.MaintenanceDepositAmount = null;

        var result = CreateValidator.Validate(request);

        Assert.True(result.IsValid, Failures(result));

        Assert.DoesNotContain(result.Errors, e => e.ErrorMessage == AmountMissing);
        Assert.DoesNotContain(result.Errors, e => e.ErrorMessage == AmountNotPositive);
    }

    [Fact]
    public void A_sale_that_declares_no_deposit_but_still_sends_an_amount_is_accepted()
    {
        var request = Sale();
        request.HasMaintenanceDeposit = false;
        request.MaintenanceDepositAmount = 5_000m;

        var result = CreateValidator.Validate(request);

        Assert.True(result.IsValid,
            $"an ignored amount should not block the listing: {Failures(result)}");
    }

    [Fact]
    public void A_declared_deposit_still_has_to_carry_an_amount()
    {
        var request = Sale();
        request.HasMaintenanceDeposit = true;
        request.MaintenanceDepositAmount = null;

        var result = CreateValidator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(CreateApartmentRequest.MaintenanceDepositAmount)
                 && e.ErrorMessage == AmountMissing);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-5000)]
    public void A_declared_deposit_still_refuses_an_amount_that_is_not_positive(decimal amount)
    {
        var request = Sale();
        request.HasMaintenanceDeposit = true;
        request.MaintenanceDepositAmount = amount;

        var result = CreateValidator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(CreateApartmentRequest.MaintenanceDepositAmount)
                 && e.ErrorMessage == AmountNotPositive);
    }

    [Fact]
    public void A_declared_deposit_with_a_real_amount_is_accepted()
    {
        var request = Sale();
        request.HasMaintenanceDeposit = true;
        request.MaintenanceDepositAmount = 15_000m;

        var result = CreateValidator.Validate(request);

        Assert.True(result.IsValid, Failures(result));
    }

    [Theory]
    [InlineData(null)]
    [InlineData(false)]
    [InlineData(true)]
    public void Rent_and_exchange_never_ask_about_the_deposit(bool? deposit)
    {
        foreach (var (listingType, request) in new[] { ("إيجار", Rent()), ("بدل", Exchange()) })
        {
            request.HasMaintenanceDeposit = deposit;
            request.MaintenanceDepositAmount = null;

            var result = CreateValidator.Validate(request);

            Assert.True(result.IsValid,
                $"'{listingType}' was blocked by the deposit question: {Failures(result)}");
        }
    }

    [Fact]
    public void An_edit_that_drops_the_deposit_answer_is_accepted()
    {
        var update = AsUpdate(Sale());
        update.HasMaintenanceDeposit = null;
        update.MaintenanceDepositAmount = null;

        var result = UpdateValidator.Validate(update);

        Assert.True(result.IsValid,
            $"editing a sale still demands a deposit answer: {Failures(result)}");
    }

    [Fact]
    public void A_sale_with_both_checkboxes_off_still_fails_on_a_genuinely_required_field()
    {
        var request = Sale();
        request.HasElevator = null;
        request.HasMaintenanceDeposit = null;
        request.Area = 0m;

        var result = CreateValidator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(CreateApartmentRequest.Area));
    }

    [Fact]
    public void The_published_deposit_field_is_never_required()
    {
        var field = Published(Deposit);

        Assert.False(field.Required);
        Assert.Null(field.RequiredWhen);
        Assert.Equal(FormFieldTypes.Checkbox, field.Type);
        Assert.Equal("يوجد وديعة صيانة", field.Label);
    }

    [Fact]
    public void The_published_deposit_amount_is_required_only_once_a_deposit_is_declared()
    {
        var field = Published(DepositAmount);

        Assert.False(field.Required);
        Assert.NotNull(field.RequiredWhen);
        Assert.Equal(Deposit, field.RequiredWhen!.Field);
        Assert.Equal<object>([true], field.RequiredWhen.Values);
    }

    [Fact]
    public void The_deposit_field_the_frontend_receives_carries_no_required_condition()
    {
        var json = JsonSerializer.Serialize(Published(Deposit),
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        using var document = JsonDocument.Parse(json);
        var root = document.RootElement;

        Assert.False(root.GetProperty("required").GetBoolean());
        Assert.False(root.TryGetProperty("requiredWhen", out _));
        Assert.True(root.TryGetProperty("visibleWhen", out _));
    }

    private static bool Matches(FormFieldConditionDto? condition, object? answer)
    {
        if (condition?.Values is not { Count: > 0 })
            return false;

        if (answer is null || (answer is string text && text.Length == 0))
            return false;

        return condition.Values.Any(value =>
            string.Equals(Loose(value), Loose(answer), StringComparison.OrdinalIgnoreCase));
    }

    private static string Loose(object? value) =>
        value switch
        {
            bool flag => flag ? "true" : "false",
            null => string.Empty,
            _ => Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty
        };

    private static bool IsVisible(FormFieldDto field, IReadOnlyDictionary<string, object?> answers) =>
        field.VisibleWhen is { } condition
            ? Matches(condition, answers.GetValueOrDefault(condition.Field))
            : field.Visible;

    private static bool IsRequired(FormFieldDto field, IReadOnlyDictionary<string, object?> answers)
    {
        if (!IsVisible(field, answers))
            return false;

        if (field.Required)
            return true;

        return field.RequiredWhen is { } condition
               && Matches(condition, answers.GetValueOrDefault(condition.Field));
    }

    private static Dictionary<string, object?> SaleAnswers() =>
        new() { ["ListingType"] = (int)RealEstateListingType.Sale };

    [Theory]
    [InlineData(Elevator)]
    [InlineData(Deposit)]
    public void The_frontend_rule_does_not_make_either_checkbox_required_for_a_sale(string name)
    {
        var required = IsRequired(Published(name), SaleAnswers());

        Assert.False(required,
            $"with ListingType=Sale the published schema still makes '{name}' required, which is " +
            "exactly what makes the form answer 'يجب تفعيل هذا الخيار'");
    }

    [Fact]
    public void No_checkbox_on_the_apartment_sale_form_is_required()
    {
        var answers = SaleAnswers();

        var offenders = PublishedFields(RealEstateCategoryId, ApartmentsSubCategoryId)
            .Where(field => field.Type == FormFieldTypes.Checkbox && IsRequired(field, answers))
            .Select(field => field.Name)
            .ToList();

        Assert.True(offenders.Count == 0,
            "these apartment checkboxes would force 'يجب تفعيل هذا الخيار' on a sale: " +
            string.Join(", ", offenders));
    }

    [Fact]
    public void A_genuinely_required_consent_checkbox_elsewhere_is_still_required()
    {
        var consent = PublishedFields(CharityCategoryId, RescuesSubCategoryId)
            .Single(field => field.Name == "IsResponsibilityAccepted");

        Assert.Equal(FormFieldTypes.Checkbox, consent.Type);
        Assert.True(IsRequired(consent, new Dictionary<string, object?>()),
            "the responsibility declaration stopped being a required checkbox");
    }

    [Fact]
    public void Neither_checkbox_exists_on_the_other_real_estate_forms()
    {
        foreach (var subCategory in new[] { SubCategoryType.Lands, SubCategoryType.Shops })
        {
            var fields = PublishedFields(RealEstateCategoryId, (int)subCategory);

            Assert.DoesNotContain(fields,
                field => field.Name is Elevator or Deposit or DepositAmount);
        }
    }
}
