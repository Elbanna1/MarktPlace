using FluentValidation;

namespace Services.Validation;

internal static class BusinessValidationRules
{
    private const int MaxLength = 1000;

    public static void RequiredGoogleMaps<T>(
        AbstractValidator<T> validator, Func<T, string?> googleMaps)
    {
        validator.RuleFor(x => googleMaps(x))
            .NotEmpty().WithMessage("رابط الموقع على خرائط جوجل مطلوب.")
            .MaximumLength(MaxLength)
            .Must(BeAnHttpUrl).WithMessage("لينك خرائط جوجل لازم يبدأ بـ http أو https.")
            .OverridePropertyName("GoogleMaps");
    }

    private static bool BeAnHttpUrl(string? value) =>
        string.IsNullOrWhiteSpace(value) ||
        (Uri.TryCreate(value, UriKind.Absolute, out var uri) &&
         (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps));
}
