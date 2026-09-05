using System.Linq.Expressions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Shared.Constants;

namespace Services.Validation;

internal static class OnlineShoppingValidationRules
{
    public static void StoreName<T>(
        AbstractValidator<T> validator, Expression<Func<T, string>> storeName, string label = "اسم المحل") =>
        validator.RuleFor(storeName)
            .NotEmpty().WithMessage($"{label} مطلوب.")
            .MaximumLength(150);

    public static void Contact<T>(
        AbstractValidator<T> validator,
        Expression<Func<T, string>> phone,
        Expression<Func<T, string>> whatsApp)
    {
        validator.RuleFor(phone)
            .NotEmpty().WithMessage("رقم الموبايل مطلوب.")
            .Matches(AdvertisementCatalog.EgyptianPhonePattern)
            .WithMessage("من فضلك اكتب رقم موبايل مصري صحيح (مثال: 01012345678).");

        validator.RuleFor(whatsApp)
            .NotEmpty().WithMessage("رقم الواتساب مطلوب.")
            .Matches(AdvertisementCatalog.EgyptianPhonePattern)
            .WithMessage("من فضلك اكتب رقم واتساب مصري صحيح (مثال: 01012345678).");
    }

    public static void Price<T>(AbstractValidator<T> validator, Expression<Func<T, decimal>> price) =>
        validator.RuleFor(price)
            .GreaterThan(0).WithMessage("السعر لازم يكون أكبر من صفر.");

    public static void Logo<T>(AbstractValidator<T> validator, Expression<Func<T, IFormFile>> logo)
    {
        var accessor = logo.Compile();

        validator.RuleFor(logo)
            .Must(file => file.Length <= ImageConstants.MaxFileSizeBytes)
            .WithMessage(
                $"اللوجو ما ينفعش يزيد عن {ImageConstants.MaxFileSizeBytes / (1024 * 1024)} ميجابايت.")
            .Must(file => HasAllowedExtension(file.FileName, ImageConstants.AllowedExtensions))
            .WithMessage(
                $"اللوجو لازم يكون بصيغة: {string.Join(", ", ImageConstants.AllowedExtensions)}.")
            .When(request => accessor(request) is not null);
    }

    public static void RequiredImages<T>(
        AbstractValidator<T> validator, Expression<Func<T, List<IFormFile>>> images) =>
        validator.RuleFor(images)
            .NotEmpty().WithMessage("لازم ترفع صورة واحدة على الأقل.")
            .Must(list => list.Count <= ImageConstants.MaxImagesPerItem)
            .WithMessage($"مسموح بـ {ImageConstants.MaxImagesPerItem} صور بحد أقصى.");

    public static void OptionalImages<T>(
        AbstractValidator<T> validator, Expression<Func<T, List<IFormFile>>> images) =>
        validator.RuleFor(images)
            .Must(list => list.Count <= ImageConstants.MaxImagesPerItem)
            .WithMessage($"مسموح بـ {ImageConstants.MaxImagesPerItem} صور بحد أقصى.");

    public static void Video<T>(AbstractValidator<T> validator, Expression<Func<T, IFormFile>> video)
    {
        var accessor = video.Compile();

        validator.RuleFor(video)
            .Must(file => file.Length <= FileUploadConstants.MaxVideoSizeBytes)
            .WithMessage(
                $"الفيديو ما ينفعش يزيد عن {FileUploadConstants.MaxVideoSizeBytes / (1024 * 1024)} ميجابايت.")
            .Must(file => HasAllowedExtension(file.FileName, VideoFormatCatalog.AllExtensions))
            .WithMessage($"الفيديو لازم يكون بصيغة: {VideoFormatCatalog.DisplayNames}.")
            .When(request => accessor(request) is not null);
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

    public static void OtherText<T>(
        AbstractValidator<T> validator,
        Expression<Func<T, string?>> other,
        Func<T, bool> isOtherSelected,
        string message)
    {
        validator.RuleFor(other)
            .NotEmpty().WithMessage(message)
            .MaximumLength(150)
            .When(isOtherSelected);
    }

    private static bool HasAllowedExtension(string fileName, IReadOnlyCollection<string> allowedExtensions) =>
        allowedExtensions.Contains(Path.GetExtension(fileName), StringComparer.OrdinalIgnoreCase);
}
