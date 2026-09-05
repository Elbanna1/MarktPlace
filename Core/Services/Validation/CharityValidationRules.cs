using FluentValidation;
using Shared.Constants;
using Shared.DTOs.Charity;

namespace Services.Validation;

public static class CharityValidationRules
{
    public static void AddCharitySharedRules<T>(
        this AbstractValidator<T> validator, bool locationRequired, bool acceptsLocation = true)
        where T : CreateCharityRequestBase
    {
        validator.RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("رقم الهاتف مطلوب.")
            .Matches(AdvertisementCatalog.EgyptianPhonePattern)
            .WithMessage(AdvertisementCatalog.EgyptianPhoneMessage)
            .MaximumLength(20);

        if (!acceptsLocation)
        {
        }
        else if (locationRequired)
        {
            validator.RuleFor(x => x.Latitude)
                .NotNull().WithMessage("من فضلك حدد الموقع على الخريطة.");

            validator.RuleFor(x => x.Longitude)
                .NotNull().WithMessage("من فضلك حدد الموقع على الخريطة.");

            validator.RuleFor(x => x)
                .Must(x => CharityCatalog.IsRealLocation(x.Latitude, x.Longitude))
                .WithMessage("إحداثيات الموقع غير صالحة. من فضلك حدد الموقع على الخريطة.")
                .WithName(nameof(CreateCharityRequestBase.Latitude))
                .When(x => x.Latitude.HasValue && x.Longitude.HasValue);
        }
        else
        {
            validator.RuleFor(x => x)
                .Must(x => x.Latitude.HasValue == x.Longitude.HasValue)
                .WithMessage("من فضلك ابعت خط العرض وخط الطول معًا.")
                .WithName(nameof(CreateCharityRequestBase.Latitude));

            validator.RuleFor(x => x)
                .Must(x => CharityCatalog.IsRealLocation(x.Latitude, x.Longitude))
                .WithMessage("إحداثيات الموقع غير صالحة.")
                .WithName(nameof(CreateCharityRequestBase.Latitude))
                .When(x => x.Latitude.HasValue || x.Longitude.HasValue);
        }

        validator.RuleFor(x => x.IsResponsibilityAccepted)
            .Equal(true).WithMessage(CharityCatalog.ResponsibilityRequiredMessage);

        validator.RuleFor(x => x.Images)
            .Must(images => images.Count <= CharityCatalog.MaxCharityImages)
            .WithMessage($"يمكن رفع {CharityCatalog.MaxCharityImages} صور بحد أقصى.");
    }

    public static void AddCenterRule<T>(
        this AbstractValidator<T> validator, Func<T, string?> center, bool required)
    {
        if (required)
        {
            validator.RuleFor(x => center(x))
                .NotEmpty().WithMessage("المركز مطلوب.")
                .WithName("Center");
        }

        validator.RuleFor(x => center(x))
            .Must(value => LocationConstants.IsValidCenter(value))
            .WithMessage($"المركز يجب أن يكون أحد: {string.Join("، ", LocationConstants.Centers)}.")
            .WithName("Center")
            .When(x => !string.IsNullOrWhiteSpace(center(x)));
    }
}
