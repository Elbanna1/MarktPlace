using FluentValidation;
using Shared.Constants;
using Shared.DTOs.Banners;

namespace Services.Validation;

internal static class BannerValidationRules
{
    public static bool BeAUsableLink(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return true;

        var value = url.Trim();

        if (value.StartsWith('/'))
            return true;

        return Uri.TryCreate(value, UriKind.Absolute, out var uri) &&
               (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
    }

    public static bool BeAValidWindow(DateTime? startDate, DateTime? endDate) =>
        startDate is not { } start || endDate is not { } end || end > start;

    public const string RedirectUrlMessage =
        "Redirect URL must be an absolute http(s) URL or start with '/'.";

    public const string WindowMessage = "The end date must be after the start date.";
}

public class CreateBannerRequestValidator : AbstractValidator<CreateBannerRequest>
{
    public CreateBannerRequestValidator()
    {
        RuleFor(x => x.Title)
            .MaximumLength(BannerCatalog.MaxTitleLength);

        RuleFor(x => x.Description)
            .MaximumLength(BannerCatalog.MaxDescriptionLength);

        RuleFor(x => x.RedirectUrl)
            .MaximumLength(BannerCatalog.MaxRedirectUrlLength)
            .Must(BannerValidationRules.BeAUsableLink)
            .WithMessage(BannerValidationRules.RedirectUrlMessage);

        RuleFor(x => x.DisplayOrder)
            .GreaterThanOrEqualTo(BannerCatalog.MinDisplayOrder)
            .LessThanOrEqualTo(BannerCatalog.MaxDisplayOrder);

        RuleFor(x => x.EndDate)
            .Must((request, endDate) =>
                BannerValidationRules.BeAValidWindow(request.StartDate, endDate))
            .WithMessage(BannerValidationRules.WindowMessage);

        RuleFor(x => x.Image)
            .NotNull().WithMessage("صورة البانر مطلوبة.");

        RuleFor(x => x.Image!.Length)
            .GreaterThan(0).WithMessage("صورة البانر فاضية.")
            .When(x => x.Image is not null);
    }
}

public class UpdateBannerRequestValidator : AbstractValidator<UpdateBannerRequest>
{
    public UpdateBannerRequestValidator()
    {
        RuleFor(x => x.Title)
            .MaximumLength(BannerCatalog.MaxTitleLength);

        RuleFor(x => x.Description)
            .MaximumLength(BannerCatalog.MaxDescriptionLength);

        RuleFor(x => x.RedirectUrl)
            .MaximumLength(BannerCatalog.MaxRedirectUrlLength)
            .Must(BannerValidationRules.BeAUsableLink)
            .WithMessage(BannerValidationRules.RedirectUrlMessage);

        RuleFor(x => x.DisplayOrder)
            .GreaterThanOrEqualTo(BannerCatalog.MinDisplayOrder)
            .LessThanOrEqualTo(BannerCatalog.MaxDisplayOrder);

        RuleFor(x => x.EndDate)
            .Must((request, endDate) =>
                BannerValidationRules.BeAValidWindow(request.StartDate, endDate))
            .WithMessage(BannerValidationRules.WindowMessage);

        RuleFor(x => x.Image!.Length)
            .GreaterThan(0).WithMessage("صورة البانر فاضية.")
            .When(x => x.Image is not null);
    }
}
