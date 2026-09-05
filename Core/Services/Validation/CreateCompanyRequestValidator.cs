using FluentValidation;
using Shared.Constants;
using Shared.DTOs.Companies;
using Shared.Enums;

namespace Services.Validation;

public class CreateCompanyRequestValidator : AbstractValidator<CreateCompanyRequest>
{
    public CreateCompanyRequestValidator()
    {
        RuleFor(x => x.CompanyName)
            .NotEmpty().WithMessage("اسم الشركة مطلوب.")
            .MaximumLength(150);

        RuleFor(x => x.CompanyField)
            .IsInEnum().WithMessage("من فضلك اختار مجال الشركة صح.");

        RuleFor(x => x.OtherCompanyField)
            .NotEmpty().WithMessage("من فضلك اكتب مجال الشركة لما تختار «أخرى».")
            .MaximumLength(150)
            .When(x => x.CompanyField == CompanyField.Other);

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("العنوان مطلوب.")
            .MaximumLength(300);

        BusinessValidationRules.RequiredGoogleMaps(this, x => x.GoogleMaps);

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("رقم الموبايل مطلوب.")
            .Matches(AdvertisementCatalog.EgyptianPhonePattern)
            .WithMessage(AdvertisementCatalog.EgyptianPhoneMessage);

        RuleFor(x => x.WhatsApp)
            .NotEmpty().WithMessage("رقم الواتساب مطلوب.")
            .Matches(AdvertisementCatalog.EgyptianPhonePattern)
            .WithMessage(AdvertisementCatalog.EgyptianPhoneMessage);

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("من فضلك اكتب بريد إلكتروني صحيح.")
            .MaximumLength(256)
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.Website)
            .MaximumLength(1000)
            .Must(BeAnHttpUrl).WithMessage("لينك الموقع لازم يبدأ بـ http أو https.")
            .When(x => !string.IsNullOrWhiteSpace(x.Website));

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("عنوان الإعلان مطلوب.")
            .MaximumLength(150);

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("وصف الإعلان مطلوب.")
            .MaximumLength(4000);

        RuleFor(x => x.Images)
            .NotEmpty().WithMessage("لازم ترفع صورة واحدة على الأقل.");

        RuleFor(x => x.Images)
            .Must(images => images == null || images.Count <= ImageConstants.MaxImagesPerItem)
            .WithMessage($"مسموح بـ {ImageConstants.MaxImagesPerItem} صور بحد أقصى.");
    }

    private static bool BeAnHttpUrl(string? value) =>
        string.IsNullOrWhiteSpace(value) ||
        (Uri.TryCreate(value, UriKind.Absolute, out var uri) &&
         (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps));
}
