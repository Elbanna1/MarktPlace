using FluentValidation;
using Shared.Constants;
using Shared.DTOs.Animals;
using Shared.Enums;

namespace Services.Validation;

public class UpdateBeeRequestValidator : AbstractValidator<UpdateBeeRequest>
{
    public UpdateBeeRequestValidator()
    {
        RuleFor(x => x.SellerName)
            .NotEmpty().WithMessage("اسم البائع مطلوب.")
            .MaximumLength(150);

        RuleFor(x => x.AnimalType)
            .IsInEnum().WithMessage("من فضلك اختار النوع صح.");

        RuleFor(x => x.OtherType)
            .NotEmpty().WithMessage("من فضلك اكتب النوع.")
            .MaximumLength(150)
            .When(x => x.AnimalType == BeeType.Other);

        RuleFor(x => x.Purpose)
            .IsInEnum().WithMessage("من فضلك اختار الغرض صح.");

        RuleFor(x => x.HealthStatus)
            .IsInEnum().WithMessage("من فضلك اختار الحالة الصحية صح.");

        RuleFor(x => x.Production)
            .IsInEnum().WithMessage("من فضلك اختار الإنتاج صح.")
            .When(x => x.Production is not null);

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("الكمية لازم تكون أكبر من صفر.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("السعر لازم يكون أكبر من صفر.");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("العنوان مطلوب.")
            .MaximumLength(300);

        RuleFor(x => x.GoogleMaps)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrWhiteSpace(x.GoogleMaps));

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("رقم الموبايل مطلوب.")
            .Matches(AdvertisementCatalog.EgyptianPhonePattern)
            .WithMessage("من فضلك اكتب رقم موبايل مصري صحيح (مثال: 01012345678).");

        RuleFor(x => x.WhatsApp)
            .Matches(AdvertisementCatalog.EgyptianPhonePattern)
            .WithMessage("من فضلك اكتب رقم واتساب مصري صحيح (مثال: 01012345678).")
            .When(x => !string.IsNullOrWhiteSpace(x.WhatsApp));

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
