using FluentValidation;
using Shared.DTOs.Antiques;
using Shared.Enums;

namespace Services.Validation;

public class CreateHandmadeRequestValidator : AbstractValidator<CreateHandmadeRequest>
{
    public CreateHandmadeRequestValidator()
    {
        AntiqueValidationRules.Seller(this, x => x.SellerName, x => x.Phone, x => x.WhatsApp);

        RuleFor(x => x.ProductName)
            .NotEmpty().WithMessage("اسم المنتج مطلوب.")
            .MaximumLength(150);

        RuleFor(x => x.HandmadeType)
            .IsInEnum().WithMessage("من فضلك اختار نوع الشغل اليدوي صح.");

        RuleFor(x => x.OtherType)
            .NotEmpty().WithMessage("من فضلك اكتب نوع الشغل اليدوي.")
            .MaximumLength(150)
            .When(x => x.HandmadeType == HandmadeType.Other);

        RuleFor(x => x.Material)
            .NotEmpty().WithMessage("الخامة مطلوبة.")
            .MaximumLength(250);

        RuleFor(x => x.ProductionTime)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.ProductionTime));

        RuleFor(x => x.Size)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.Size));

        RuleFor(x => x.Colors)
            .Must(colors => colors.Count > 0).WithMessage("لازم تختار لون واحد على الأقل.")
            .Must(colors => colors.All(color => Enum.IsDefined(typeof(HandmadeColor), color)))
            .WithMessage("واحد من الألوان اللي اخترتها مش صحيح.");

        RuleFor(x => x.OtherColor)
            .NotEmpty().WithMessage("من فضلك اكتب اللون.")
            .MaximumLength(150)
            .When(x => x.Colors.Contains(HandmadeColor.Other));

        AntiqueValidationRules.Price(this, x => x.Price);
        AntiqueValidationRules.Location(this, x => x.Center, x => x.Address, x => x.GoogleMaps);
        AntiqueValidationRules.Advertisement(this, x => x.Title, x => x.Description);
        AntiqueValidationRules.Images(this, x => x.Images, required: true);
        AntiqueValidationRules.Video(this, x => x.Video);
    }
}
