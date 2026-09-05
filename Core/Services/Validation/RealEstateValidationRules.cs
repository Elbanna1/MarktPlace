using FluentValidation;
using Shared.Constants;
using Shared.DTOs.RealEstate;
using Shared.Enums;

namespace Services.Validation;

public static class RealEstateValidationRules
{
    public static void AddRealEstateSharedRules<T>(this AbstractValidator<T> validator)
        where T : CreateRealEstateRequestBase
    {
        validator.RuleFor(x => x.Title)
            .NotEmpty().WithMessage("عنوان الإعلان مطلوب.")
            .MaximumLength(150);

        validator.RuleFor(x => x.Description)
            .NotEmpty().WithMessage("وصف الإعلان مطلوب.")
            .MaximumLength(4000);

        validator.RuleFor(x => x.AdvertiserName)
            .NotEmpty().WithMessage("اسم المعلن مطلوب.")
            .MaximumLength(150);

        validator.RuleFor(x => x.ListingType)
            .IsInEnum().WithMessage("من فضلك اختار نوع إعلان صحيح (بيع / إيجار / بدل).");

        validator.RuleFor(x => x.Center)
            .NotEmpty().WithMessage("المركز مطلوب.")
            .Must(LocationConstants.IsValidCenter)
            .WithMessage($"المركز يجب أن يكون أحد: {string.Join("، ", LocationConstants.Centers)}.");

        validator.RuleFor(x => x.Project)
            .NotNull().WithMessage("اسم المشروع / الحي مطلوب لمركز الفيوم الجديدة.")
            .IsInEnum().WithMessage("من فضلك اختار مشروع صحيح.")
            .When(x => RealEstateCatalog.IsProjectCenter(x.Center));

        validator.RuleFor(x => x.Project)
            .Null().WithMessage("اسم المشروع / الحي متاح لمركز الفيوم الجديدة فقط.")
            .When(x => !RealEstateCatalog.IsProjectCenter(x.Center));

        validator.RuleFor(x => x.OtherProject)
            .NotEmpty().WithMessage("من فضلك اكتب اسم المشروع عند اختيار 'أخرى'.")
            .MaximumLength(150)
            .When(x => RealEstateCatalog.IsProjectCenter(x.Center) && x.Project == RealEstateProject.Other);

        validator.RuleFor(x => x.District).MaximumLength(150);

        validator.RuleFor(x => x.Address)
            .NotEmpty().WithMessage("العنوان التفصيلي مطلوب.")
            .MaximumLength(300);

        validator.RuleFor(x => x.GoogleMaps)
            .MaximumLength(1000)
            .Must(BeAnHttpUrl)
            .WithMessage("رابط خرائط جوجل يجب أن يكون رابط http(s) صحيح.")
            .When(x => !string.IsNullOrWhiteSpace(x.GoogleMaps));

        validator.RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("رقم الهاتف مطلوب.")
            .Matches(AdvertisementCatalog.EgyptianPhonePattern)
            .WithMessage(AdvertisementCatalog.EgyptianPhoneMessage)
            .MaximumLength(20);

        validator.RuleFor(x => x.WhatsApp)
            .Matches(AdvertisementCatalog.EgyptianPhonePattern)
            .WithMessage(AdvertisementCatalog.EgyptianPhoneMessage)
            .MaximumLength(20)
            .When(x => !string.IsNullOrWhiteSpace(x.WhatsApp));

        validator.RuleFor(x => x.Email)
            .EmailAddress().WithMessage("من فضلك اكتب بريد إلكتروني صحيح.")
            .MaximumLength(256)
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        validator.RuleFor(x => x.Notes).MaximumLength(4000);
    }

    public static void AddAvailabilityWindowRule<T>(
        this AbstractValidator<T> validator,
        Func<T, DateTime?> availableFrom,
        Func<T, DateTime?> availableTo)
    {
        validator.RuleFor(x => x)
            .Must(x => availableTo(x) is not { } to || availableFrom(x) is not { } from || to >= from)
            .WithMessage("تاريخ 'متاح حتى' يجب أن يكون بعد تاريخ 'متاح من'.")
            .WithName(nameof(CreateLandRequest.AvailableTo));
    }

    public static void AddLicenceWindowRule<T>(
        this AbstractValidator<T> validator,
        Func<T, DateTime?> issueDate,
        Func<T, DateTime?> expiryDate)
    {
        validator.RuleFor(x => x)
            .Must(x => expiryDate(x) is not { } expiry || issueDate(x) is not { } issue || expiry >= issue)
            .WithMessage("تاريخ انتهاء الرخصة يجب أن يكون بعد تاريخ إصدارها.")
            .WithName(nameof(CreateLandRequest.LicenseExpiryDate));
    }

    private static bool BeAnHttpUrl(string? value) =>
        string.IsNullOrWhiteSpace(value) ||
        (Uri.TryCreate(value, UriKind.Absolute, out var uri) &&
         (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps));
}
