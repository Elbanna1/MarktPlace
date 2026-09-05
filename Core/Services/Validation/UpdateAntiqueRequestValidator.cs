using FluentValidation;
using Shared.DTOs.Antiques;
using Shared.Enums;

namespace Services.Validation;

public class UpdateAntiqueRequestValidator : AbstractValidator<UpdateAntiqueRequest>
{
    public UpdateAntiqueRequestValidator()
    {
        AntiqueValidationRules.Seller(this, x => x.SellerName, x => x.Phone, x => x.WhatsApp);

        RuleFor(x => x.AntiqueName)
            .NotEmpty().WithMessage("اسم التحفة مطلوب.")
            .MaximumLength(150);

        RuleFor(x => x.AntiqueType)
            .IsInEnum().WithMessage("من فضلك اختار نوع التحفة صح.");

        RuleFor(x => x.OtherType)
            .NotEmpty().WithMessage("من فضلك اكتب نوع التحفة.")
            .MaximumLength(150)
            .When(x => x.AntiqueType == AntiqueType.Other);

        AntiqueValidationRules.Year(this, x => x.ManufactureYear, "ManufactureYear");

        RuleFor(x => x.CountryOfOrigin)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.CountryOfOrigin));

        RuleFor(x => x.Manufacturer)
            .MaximumLength(150)
            .When(x => !string.IsNullOrWhiteSpace(x.Manufacturer));

        RuleFor(x => x.Material)
            .IsInEnum().WithMessage("من فضلك اختار الخامة صح.");

        RuleFor(x => x.OtherMaterial)
            .NotEmpty().WithMessage("من فضلك اكتب الخامة.")
            .MaximumLength(150)
            .When(x => x.Material == AntiqueMaterial.Other);

        RuleFor(x => x.Condition)
            .IsInEnum().WithMessage("من فضلك اختار الحالة صح.");

        RuleFor(x => x.WorkingStatus)
            .IsInEnum().WithMessage("من فضلك اختار حالة العمل صح.");

        RuleFor(x => x.Originality)
            .IsInEnum().WithMessage("من فضلك اختار الأصلية صح.");

        AntiqueValidationRules.Price(this, x => x.Price);
        AntiqueValidationRules.Location(this, x => x.Center, x => x.Address, x => x.GoogleMaps);
        AntiqueValidationRules.Advertisement(this, x => x.Title, x => x.Description);
        AntiqueValidationRules.Images(this, x => x.Images, required: false);
        AntiqueValidationRules.Video(this, x => x.Video);
    }
}
