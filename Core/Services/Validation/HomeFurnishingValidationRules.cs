using FluentValidation;
using Shared.Constants;
using Shared.DTOs.HomeFurnishing;

namespace Services.Validation;

public abstract class HomeFurnishingRequestValidator<TRequest> : AbstractValidator<TRequest>
    where TRequest : CreateHomeFurnishingRequestBase
{
    protected HomeFurnishingRequestValidator(bool imagesRequired)
    {
        RuleFor(x => x.SellerName)
            .NotEmpty().WithMessage("اسم البائع مطلوب.")
            .MaximumLength(150);

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("رقم الموبايل مطلوب.")
            .Matches(AdvertisementCatalog.EgyptianPhonePattern)
            .WithMessage("من فضلك اكتب رقم موبايل مصري صحيح (مثال: 01012345678).");

        RuleFor(x => x.WhatsApp)
            .Matches(AdvertisementCatalog.EgyptianPhonePattern)
            .WithMessage("من فضلك اكتب رقم واتساب مصري صحيح (مثال: 01012345678).")
            .When(x => !string.IsNullOrWhiteSpace(x.WhatsApp));

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("من فضلك اكتب بريد إلكتروني صحيح.")
            .MaximumLength(256)
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.ProductName)
            .NotEmpty().WithMessage("اسم المنتج مطلوب.")
            .MaximumLength(150);

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("السعر لازم يكون أكبر من صفر.");

        RuleFor(x => x.Center)
            .NotEmpty().WithMessage("المركز مطلوب.")
            .Must(LocationConstants.IsValidCenter)
            .WithMessage("المركز لازم يكون واحد من مراكز الفيوم.");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("العنوان مطلوب.")
            .MaximumLength(300);

        RuleFor(x => x.GoogleMaps)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrWhiteSpace(x.GoogleMaps));

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("عنوان الإعلان مطلوب.")
            .MaximumLength(150);

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("وصف الإعلان مطلوب.")
            .MaximumLength(4000);

        if (imagesRequired)
        {
            RuleFor(x => x.Images)
                .NotEmpty().WithMessage("لازم ترفع صورة واحدة على الأقل.");
        }

        RuleFor(x => x.Images)
            .Must(images => images.Count <= ImageConstants.MaxImagesPerItem)
            .WithMessage($"مسموح بـ {ImageConstants.MaxImagesPerItem} صور بحد أقصى.");

        RuleFor(x => x.Video!)
            .Must(file => file.Length <= FileUploadConstants.MaxVideoSizeBytes)
            .WithMessage(
                $"الفيديو ما ينفعش يزيد عن {FileUploadConstants.MaxVideoSizeBytes / (1024 * 1024)} ميجابايت.")
            .Must(file => HasAllowedExtension(file.FileName, VideoFormatCatalog.AllExtensions))
            .WithMessage($"الفيديو لازم يكون بصيغة: {VideoFormatCatalog.DisplayNames}.")
            .When(x => x.Video is not null);
    }

    protected void RequiredWhenOther(
        System.Linq.Expressions.Expression<Func<TRequest, string?>> field,
        Func<TRequest, bool> isOther,
        string message)
    {
        RuleFor(field)
            .NotEmpty().WithMessage(message)
            .MaximumLength(150)
            .When(isOther);
    }

    private static bool HasAllowedExtension(string fileName, IReadOnlyCollection<string> allowedExtensions) =>
        allowedExtensions.Contains(Path.GetExtension(fileName), StringComparer.OrdinalIgnoreCase);
}
