using FluentValidation;
using Shared.Constants;
using Shared.DTOs.FruitVegetableMerchants;

namespace Services.Validation;

public class CreateFruitVegetableMerchantRequestValidator
    : AbstractValidator<CreateFruitVegetableMerchantRequest>
{
    public CreateFruitVegetableMerchantRequestValidator()
    {
        RuleFor(x => x.StallName)
            .NotEmpty().WithMessage("اسم المحل مطلوب.")
            .MaximumLength(150);

        RuleFor(x => x.MerchantName)
            .NotEmpty().WithMessage("اسم التاجر مطلوب.")
            .MaximumLength(150);

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("رقم الموبايل مطلوب.")
            .Matches(AdvertisementCatalog.EgyptianPhonePattern)
            .WithMessage("من فضلك اكتب رقم موبايل مصري صحيح (مثال: 01012345678).");

        RuleFor(x => x.WhatsApp)
            .Matches(AdvertisementCatalog.EgyptianPhonePattern)
            .WithMessage("من فضلك اكتب رقم واتساب مصري صحيح (مثال: 01012345678).")
            .When(x => !string.IsNullOrWhiteSpace(x.WhatsApp));

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("العنوان مطلوب.")
            .MaximumLength(300);

        RuleFor(x => x.GoogleMaps)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrWhiteSpace(x.GoogleMaps));

        RuleFor(x => x.ProductName)
            .NotEmpty().WithMessage("اسم المنتج مطلوب.")
            .MaximumLength(300);

        RuleFor(x => x.SaleType)
            .IsInEnum().WithMessage("من فضلك اختار نوع البيع صح.");

        RuleFor(x => x.ProductDetails)
            .NotEmpty().WithMessage("تفاصيل المنتج مطلوبة.")
            .MaximumLength(4000);

        RuleFor(x => x.Images)
            .Must(images => images.Count <= ImageConstants.MaxImagesPerItem)
            .WithMessage($"مسموح بـ {ImageConstants.MaxImagesPerItem} صور بحد أقصى.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("عنوان الإعلان مطلوب.")
            .MaximumLength(150);

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("وصف الإعلان مطلوب.")
            .MaximumLength(4000);
    }
}
