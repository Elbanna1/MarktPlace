using FluentValidation;
using Shared.DTOs.Antiques;
using Shared.Enums;

namespace Services.Validation;

public class UpdateDecorAntiqueRequestValidator : AbstractValidator<UpdateDecorAntiqueRequest>
{
    public UpdateDecorAntiqueRequestValidator()
    {
        AntiqueValidationRules.Seller(this, x => x.SellerName, x => x.Phone, x => x.WhatsApp);

        RuleFor(x => x.ItemName)
            .NotEmpty().WithMessage("اسم الحاجة مطلوب.")
            .MaximumLength(150);

        RuleFor(x => x.ItemType)
            .IsInEnum().WithMessage("من فضلك اختار نوع الحاجة صح.");

        RuleFor(x => x.OtherItemType)
            .NotEmpty().WithMessage("من فضلك اكتب نوع الحاجة.")
            .MaximumLength(150)
            .When(x => x.ItemType == DecorAntiqueItemType.Other);

        RuleFor(x => x.Material)
            .IsInEnum().WithMessage("من فضلك اختار الخامة صح.");

        RuleFor(x => x.OtherMaterial)
            .NotEmpty().WithMessage("من فضلك اكتب الخامة.")
            .MaximumLength(150)
            .When(x => x.Material == DecorAntiqueMaterial.Other);

        RuleFor(x => x.Condition)
            .IsInEnum().WithMessage("من فضلك اختار الحالة صح.");

        RuleFor(x => x.Originality)
            .IsInEnum().WithMessage("من فضلك اختار الأصلية صح.");

        AntiqueValidationRules.Measurement(this, x => x.Length, "Length", "الطول");
        AntiqueValidationRules.Measurement(this, x => x.Width, "Width", "العرض");
        AntiqueValidationRules.Measurement(this, x => x.Height, "Height", "الارتفاع");
        AntiqueValidationRules.Measurement(this, x => x.Weight, "Weight", "الوزن");

        AntiqueValidationRules.Price(this, x => x.Price);
        AntiqueValidationRules.Location(this, x => x.Center, x => x.Address, x => x.GoogleMaps);
        AntiqueValidationRules.Advertisement(this, x => x.Title, x => x.Description);
        AntiqueValidationRules.Images(this, x => x.Images, required: false);
        AntiqueValidationRules.Video(this, x => x.Video);
    }
}
