using System.Linq.Expressions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Shared.Constants;

namespace Services.Validation;

internal static class ClothingValidationRules
{
    public static void StoreName<T>(AbstractValidator<T> validator, Expression<Func<T, string>> storeName) =>
        validator.RuleFor(storeName)
            .NotEmpty().WithMessage("اسم المحل مطلوب.")
            .MaximumLength(150);

    public static void Location<T>(
        AbstractValidator<T> validator,
        Expression<Func<T, string>> center,
        Expression<Func<T, string>> address,
        Expression<Func<T, string?>> googleMaps)
    {
        validator.RuleFor(center)
            .NotEmpty().WithMessage("المركز مطلوب.")
            .Must(LocationConstants.IsValidCenter)
            .WithMessage("المركز لازم يكون واحد من مراكز الفيوم.");

        validator.RuleFor(address)
            .NotEmpty().WithMessage("العنوان مطلوب.")
            .MaximumLength(300);

        var googleMapsAccessor = googleMaps.Compile();

        validator.RuleFor(googleMaps)
            .MaximumLength(1000)
            .When(request => !string.IsNullOrWhiteSpace(googleMapsAccessor(request)));
    }

    public static void Contact<T>(
        AbstractValidator<T> validator,
        Expression<Func<T, string>> phone,
        Expression<Func<T, string>> whatsApp,
        Expression<Func<T, string?>> email)
    {
        validator.RuleFor(phone)
            .NotEmpty().WithMessage("رقم الموبايل مطلوب.")
            .Matches(AdvertisementCatalog.EgyptianPhonePattern)
            .WithMessage("من فضلك اكتب رقم موبايل مصري صحيح (مثال: 01012345678).");

        validator.RuleFor(whatsApp)
            .NotEmpty().WithMessage("رقم الواتساب مطلوب.")
            .Matches(AdvertisementCatalog.EgyptianPhonePattern)
            .WithMessage("من فضلك اكتب رقم واتساب مصري صحيح (مثال: 01012345678).");

        var emailAccessor = email.Compile();

        validator.RuleFor(email)
            .EmailAddress().WithMessage("من فضلك اكتب بريد إلكتروني صحيح.")
            .MaximumLength(256)
            .When(request => !string.IsNullOrWhiteSpace(emailAccessor(request)));
    }

    public static void Video<T>(AbstractValidator<T> validator, Expression<Func<T, IFormFile>> video)
    {
        var videoAccessor = video.Compile();

        validator.RuleFor(video)
            .Must(file => file.Length <= FileUploadConstants.MaxVideoSizeBytes)
            .WithMessage(
                $"الفيديو ما ينفعش يزيد عن {FileUploadConstants.MaxVideoSizeBytes / (1024 * 1024)} ميجابايت.")
            .Must(file => HasAllowedExtension(file.FileName, VideoFormatCatalog.AllExtensions))
            .WithMessage($"الفيديو لازم يكون بصيغة: {VideoFormatCatalog.DisplayNames}.")
            .When(request => videoAccessor(request) is not null);
    }

    public static void Advertisement<T>(
        AbstractValidator<T> validator,
        Expression<Func<T, string>> title,
        Expression<Func<T, string>> description)
    {
        validator.RuleFor(title)
            .NotEmpty().WithMessage("عنوان الإعلان مطلوب.")
            .MaximumLength(150);

        validator.RuleFor(description)
            .NotEmpty().WithMessage("وصف الإعلان مطلوب.")
            .MaximumLength(4000);
    }

    private static bool HasAllowedExtension(string fileName, IReadOnlyCollection<string> allowedExtensions) =>
        allowedExtensions.Contains(Path.GetExtension(fileName), StringComparer.OrdinalIgnoreCase);
}
