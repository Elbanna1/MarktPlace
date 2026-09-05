using FluentValidation.Results;
using Services.AdForms;
using Services.Validation;
using Shared.DTOs.Lookups.Forms;
using Shared.DTOs.RealEstate;
using Shared.Enums;
using Xunit;

namespace MarkatPlace.Tests;

public class ApartmentOptionalFieldsTests
{
    private static readonly CreateApartmentRequestValidator CreateValidator = new();
    private static readonly UpdateApartmentRequestValidator UpdateValidator = new();

    private const int RealEstateCategoryId = 11;
    private const int ApartmentsSubCategoryId = (int)SubCategoryType.Apartments;

    public static readonly string[] TheSixOptionalFields =
        ["Features", "WhatsApp", "Email", "GoogleMaps", "District", "Notes"];

    private static void FillSix(CreateApartmentRequest request)
    {
        request.Features = [ApartmentFeature.Elevator, ApartmentFeature.Garage];
        request.WhatsApp = "01012345678";
        request.Email = "seller@example.com";
        request.GoogleMaps = "https://maps.google.com/?q=29.31,30.84";
        request.District = "قرية دمو";
        request.Notes = "المعاينة يومي الجمعة والسبت.";
    }

    private static void ClearSix(CreateApartmentRequest request)
    {
        request.Features = [];
        request.WhatsApp = null;
        request.Email = null;
        request.GoogleMaps = null;
        request.District = null;
        request.Notes = null;
    }

    private static void Clear(CreateApartmentRequest request, string field)
    {
        switch (field)
        {
            case "Features": request.Features = []; break;
            case "WhatsApp": request.WhatsApp = null; break;
            case "Email": request.Email = null; break;
            case "GoogleMaps": request.GoogleMaps = null; break;
            case "District": request.District = null; break;
            case "Notes": request.Notes = null; break;
            default: throw new ArgumentOutOfRangeException(nameof(field), field, null);
        }
    }

    private static void Blank(CreateApartmentRequest request, string field)
    {
        switch (field)
        {
            case "Features": request.Features = []; break;
            case "WhatsApp": request.WhatsApp = string.Empty; break;
            case "Email": request.Email = string.Empty; break;
            case "GoogleMaps": request.GoogleMaps = string.Empty; break;
            case "District": request.District = string.Empty; break;
            case "Notes": request.Notes = string.Empty; break;
            default: throw new ArgumentOutOfRangeException(nameof(field), field, null);
        }
    }

    private static CreateApartmentRequest Valid()
    {
        var request = new CreateApartmentRequest
        {
            Title = "شقة للبيع في مدينة الفيوم",
            Description = "شقة تشطيب سوبر لوكس، دور ثالث، أسانسير، قريبة من الخدمات.",
            AdvertiserName = "محمود السيد",
            ListingType = RealEstateListingType.Sale,
            ApartmentType = ApartmentType.Residential,
            OwnershipType = ApartmentOwnershipType.Freehold,
            Price = 1_400_000m,
            PricePerMeter = 12_000m,
            Area = 120m,
            RoomsCount = 3,
            BathroomsCount = 2,
            ReceptionPieces = ApartmentReceptionPieces.Two,
            FloorType = ApartmentFloorType.Repeated,
            FloorNumber = 3,
            TotalFloors = 8,
            ApartmentsPerFloor = 2,
            HasElevator = true,
            FurnishedStatus = ApartmentFurnishedStatus.No,
            FinishingType = ApartmentFinishingType.SuperLux,
            PropertyAge = ApartmentPropertyAge.New,
            Direction = ApartmentDirection.North,
            LegalStatus = ApartmentLegalStatus.Licensed,
            OwnershipDocument = ApartmentOwnershipDocument.FinalContract,
            PaymentMethod = ApartmentPaymentMethod.Cash,
            HasMaintenanceDeposit = false,
            MonthlyFees = 250m,
            Center = "الفيوم",
            Address = "شارع الحرية، بجوار مدرسة النصر",
            Phone = "01012345678",
        };

        FillSix(request);
        return request;
    }

    private static UpdateApartmentRequest ValidUpdate()
    {
        var source = Valid();
        var update = new UpdateApartmentRequest();

        foreach (var property in typeof(CreateApartmentRequest).GetProperties())
        {
            if (property.CanRead && property.CanWrite)
                property.SetValue(update, property.GetValue(source));
        }

        return update;
    }

    private static ValidationResult Validate(Action<CreateApartmentRequest>? change = null)
    {
        var request = Valid();
        change?.Invoke(request);
        return CreateValidator.Validate(request);
    }

    private static string Failures(ValidationResult result) =>
        string.Join(" | ", result.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}"));

    [Fact]
    public void An_apartment_that_supplies_every_field_is_still_accepted()
    {
        var result = Validate();
        Assert.True(result.IsValid, Failures(result));
    }

    [Fact]
    public void An_apartment_that_omits_all_six_optional_fields_is_accepted()
    {
        var result = Validate(ClearSix);
        Assert.True(result.IsValid, Failures(result));
    }

    [Theory]
    [InlineData("Features")]
    [InlineData("WhatsApp")]
    [InlineData("Email")]
    [InlineData("GoogleMaps")]
    [InlineData("District")]
    [InlineData("Notes")]
    public void An_apartment_that_omits_one_optional_field_is_accepted(string field)
    {
        var result = Validate(request => Clear(request, field));
        Assert.True(result.IsValid, $"'{field}' still blocks the listing: {Failures(result)}");
    }

    [Theory]
    [InlineData("Features")]
    [InlineData("WhatsApp")]
    [InlineData("Email")]
    [InlineData("GoogleMaps")]
    [InlineData("District")]
    [InlineData("Notes")]
    public void An_empty_value_is_treated_the_same_as_an_omitted_field(string field)
    {
        var result = Validate(request => Blank(request, field));
        Assert.True(result.IsValid, $"an empty '{field}' still blocks the listing: {Failures(result)}");
    }

    [Fact]
    public void An_edit_that_omits_all_six_optional_fields_is_accepted()
    {
        var update = ValidUpdate();
        ClearSix(update);

        var result = UpdateValidator.Validate(update);
        Assert.True(result.IsValid, Failures(result));
    }

    [Fact]
    public void An_edit_that_supplies_every_field_is_still_accepted()
    {
        var result = UpdateValidator.Validate(ValidUpdate());
        Assert.True(result.IsValid, Failures(result));
    }

    [Fact]
    public void A_supplied_WhatsApp_number_must_still_be_an_Egyptian_mobile()
    {
        var result = Validate(request => request.WhatsApp = "12345");
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateApartmentRequest.WhatsApp));
    }

    [Fact]
    public void A_supplied_email_must_still_look_like_an_email()
    {
        var result = Validate(request => request.Email = "not-an-email");
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateApartmentRequest.Email));
    }

    [Fact]
    public void A_supplied_maps_link_must_still_be_an_http_url()
    {
        var result = Validate(request => request.GoogleMaps = "javascript:alert(1)");
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateApartmentRequest.GoogleMaps));
    }

    [Fact]
    public void A_supplied_district_is_still_capped_in_length()
    {
        var result = Validate(request => request.District = new string('م', 151));
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateApartmentRequest.District));
    }

    [Fact]
    public void Supplied_notes_are_still_capped_in_length()
    {
        var result = Validate(request => request.Notes = new string('م', 4001));
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateApartmentRequest.Notes));
    }

    [Fact]
    public void Supplied_features_must_still_be_real_features()
    {
        var result = Validate(request => request.Features = [(ApartmentFeature)9999]);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName.StartsWith("Features"));
    }

    [Fact]
    public void Supplied_features_still_cannot_repeat()
    {
        var result = Validate(request =>
            request.Features = [ApartmentFeature.Elevator, ApartmentFeature.Elevator]);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateApartmentRequest.Features));
    }

    [Fact]
    public void Making_six_fields_optional_did_not_make_the_rest_of_the_form_optional()
    {
        Assert.False(Validate(request => request.Title = null!).IsValid);
        Assert.False(Validate(request => request.Description = null!).IsValid);
        Assert.False(Validate(request => request.AdvertiserName = null!).IsValid);
        Assert.False(Validate(request => request.Center = null!).IsValid);
        Assert.False(Validate(request => request.Address = null!).IsValid);
        Assert.False(Validate(request => request.Phone = null!).IsValid);
        Assert.False(Validate(request => request.ApartmentType = null).IsValid);
        Assert.False(Validate(request => request.Area = null).IsValid);
        Assert.False(Validate(request => request.RoomsCount = null).IsValid);
        Assert.False(Validate(request => request.LegalStatus = null).IsValid);
    }

    [Fact]
    public void Every_message_the_apartment_form_answers_with_is_Arabic()
    {
        var result = Validate(request =>
        {
            ClearSix(request);
            request.Title = null!;
            request.WhatsApp = "12345";
            request.Email = "nope";
            request.GoogleMaps = "javascript:alert(1)";
        });

        Assert.False(result.IsValid);
        foreach (var failure in result.Errors)
        {
            Assert.True(ArabicText.IsArabic(failure.ErrorMessage),
                $"'{failure.PropertyName}' answered in a language the seller cannot read: " +
                $"\"{failure.ErrorMessage}\"");
        }
    }

    private static FormFieldDto Field(SubCategoryType subCategory, string name)
    {
        var schema = AdFormSchemaCatalog.GetSchema(RealEstateCategoryId, (int)subCategory);
        Assert.NotNull(schema);

        var field = schema!.Fields.SingleOrDefault(f => f.Name == name);
        Assert.True(field is not null, $"{subCategory} no longer publishes a '{name}' field");
        return field!;
    }

    private static FormFieldDto Field(string name) =>
        Field(SubCategoryType.Apartments, name);

    [Theory]
    [InlineData("Features")]
    [InlineData("WhatsApp")]
    [InlineData("Email")]
    [InlineData("GoogleMaps")]
    [InlineData("District")]
    [InlineData("Notes")]
    public void The_published_apartment_form_marks_the_six_fields_optional(string name)
    {
        var field = Field(name);

        Assert.False(field.Required, $"the apartment form still publishes '{name}' as required");
        Assert.Null(field.RequiredWhen);
    }

    [Fact]
    public void The_published_apartment_form_still_marks_the_rest_required()
    {
        foreach (var name in new[] { "Title", "Description", "AdvertiserName", "Address", "Phone", "Area" })
            Assert.True(Field(name).Required, $"the apartment form stopped requiring '{name}'");
    }

    [Fact]
    public void The_shared_real_estate_rules_were_not_touched()
    {
        foreach (var subCategory in new[] { SubCategoryType.Lands, SubCategoryType.Shops })
        {
            foreach (var name in new[] { "Title", "Description", "AdvertiserName", "Address", "Phone" })
            {
                Assert.True(Field(subCategory, name).Required,
                    $"{subCategory} lost its required '{name}' — this change was meant to be apartments-only");
            }
        }
    }

    [Fact]
    public void Apartments_is_the_only_form_that_promotes_every_field_to_required()
    {
        var source = RepositoryRoot.ReadFile("Core/Services/AdForms/AdFormSchemaCatalog.cs");

        Assert.Equal(1, source.Split("everyFieldRequired: true").Length - 1);
        Assert.Equal(1, source.Split("ApartmentOptionalFields,").Length - 1);

        var declaration = source[source.IndexOf("HashSet<string> ApartmentOptionalFields")..];
        declaration = declaration[..declaration.IndexOf("];")];

        foreach (var name in TheSixOptionalFields)
            Assert.Contains($"\"{name}\"", declaration);
    }
}
