using FluentValidation;
using Microsoft.AspNetCore.Http;
using Shared.Constants;

namespace Services.Validation;

internal static class AntiqueValidationRules
{
    public static bool HasAllowedExtension(string? fileName, IReadOnlyCollection<string> allowed)
    {
        var extension = Path.GetExtension(fileName);

        return string.IsNullOrWhiteSpace(extension) ||
               allowed.Contains(extension.ToLowerInvariant());
    }

    public static void Seller<T>(
        AbstractValidator<T> validator,
        Func<T, string> sellerName,
        Func<T, string> phone,
        Func<T, string?> whatsApp,
        string sellerNameMessage = "اسم البائع مطلوب.")
    {
        validator.RuleFor(x => sellerName(x))
            .NotEmpty().WithMessage(sellerNameMessage)
            .MaximumLength(150)
            .OverridePropertyName("SellerName");

        validator.RuleFor(x => phone(x))
            .NotEmpty().WithMessage("رقم الموبايل مطلوب.")
            .Matches(AdvertisementCatalog.EgyptianPhonePattern)
            .WithMessage(AdvertisementCatalog.EgyptianPhoneMessage)
            .OverridePropertyName("Phone");

        validator.RuleFor(x => whatsApp(x))
            .Matches(AdvertisementCatalog.EgyptianPhonePattern)
            .WithMessage(AdvertisementCatalog.EgyptianPhoneMessage)
            .When(x => !string.IsNullOrWhiteSpace(whatsApp(x)))
            .OverridePropertyName("WhatsApp");
    }

    public static void Location<T>(
        AbstractValidator<T> validator,
        Func<T, string> center,
        Func<T, string> address,
        Func<T, string?> googleMaps)
    {
        validator.RuleFor(x => center(x))
            .NotEmpty().WithMessage("المركز مطلوب.")
            .Must(LocationConstants.IsValidCenter)
            .WithMessage("المركز اللي اخترته مش من مراكز الفيوم.")
            .OverridePropertyName("Center");

        validator.RuleFor(x => address(x))
            .NotEmpty().WithMessage("العنوان مطلوب.")
            .MaximumLength(300)
            .OverridePropertyName("Address");

        validator.RuleFor(x => googleMaps(x))
            .MaximumLength(1000)
            .When(x => !string.IsNullOrWhiteSpace(googleMaps(x)))
            .OverridePropertyName("GoogleMaps");
    }

    public static void Price<T>(AbstractValidator<T> validator, Func<T, decimal> price)
    {
        validator.RuleFor(x => price(x))
            .GreaterThan(0).WithMessage("السعر لازم يكون أكبر من صفر.")
            .OverridePropertyName("Price");
    }

    public static void Advertisement<T>(
        AbstractValidator<T> validator, Func<T, string> title, Func<T, string> description)
    {
        validator.RuleFor(x => title(x))
            .NotEmpty().WithMessage("عنوان الإعلان مطلوب.")
            .MaximumLength(150)
            .OverridePropertyName("Title");

        validator.RuleFor(x => description(x))
            .NotEmpty().WithMessage("وصف الإعلان مطلوب.")
            .MaximumLength(4000)
            .OverridePropertyName("Description");
    }

    public static void Images<T>(
        AbstractValidator<T> validator, Func<T, List<IFormFile>> images, bool required)
    {
        if (required)
        {
            validator.RuleFor(x => images(x))
                .Must(files => files.Count > 0)
                .WithMessage("لازم ترفع صورة واحدة على الأقل.")
                .OverridePropertyName("Images");
        }

        validator.RuleFor(x => images(x))
            .Must(files => files.Count <= ImageConstants.MaxImagesPerItem)
            .WithMessage($"مسموح بـ {ImageConstants.MaxImagesPerItem} صور بحد أقصى.")
            .OverridePropertyName("Images");

        validator.RuleFor(x => images(x))
            .Must(files => files.All(file => file.Length <= ImageConstants.MaxFileSizeBytes))
            .WithMessage($"كل صورة ما ينفعش تزيد عن {ImageConstants.MaxFileSizeBytes / (1024 * 1024)} ميجابايت.")
            .Must(files => files.All(file => HasAllowedExtension(file.FileName, ImageConstants.AllowedExtensions)))
            .WithMessage($"الصور لازم تكون بصيغة: {ImageConstants.AllowedFormatNames}.")
            .OverridePropertyName("Images");
    }

    public static void Video<T>(AbstractValidator<T> validator, Func<T, IFormFile?> video)
    {
        validator.RuleFor(x => video(x)!)
            .Must(file => file.Length <= FileUploadConstants.MaxVideoSizeBytes)
            .WithMessage($"الفيديو ما ينفعش يزيد عن {FileUploadConstants.MaxVideoSizeBytes / (1024 * 1024)} ميجابايت.")
            .Must(file => HasAllowedExtension(file.FileName, VideoFormatCatalog.AllExtensions))
            .WithMessage($"الفيديو لازم يكون بصيغة: {VideoFormatCatalog.DisplayNames}.")
            .When(x => video(x) is not null)
            .OverridePropertyName("Video");
    }

    public static void Year<T>(AbstractValidator<T> validator, Func<T, int?> year, string propertyName)
    {
        validator.RuleFor(x => year(x)!.Value)
            .InclusiveBetween(AntiqueCatalog.MinYear, AntiqueCatalog.MaxYear)
            .WithMessage($"السنة لازم تكون بين {AntiqueCatalog.MinYear} و {AntiqueCatalog.MaxYear}.")
            .When(x => year(x) is not null)
            .OverridePropertyName(propertyName);
    }

    public static void Measurement<T>(
        AbstractValidator<T> validator, Func<T, decimal?> value, string propertyName, string label)
    {
        validator.RuleFor(x => value(x)!.Value)
            .GreaterThan(0).WithMessage($"{label} لازم يكون أكبر من صفر.")
            .When(x => value(x) is not null)
            .OverridePropertyName(propertyName);
    }
}
