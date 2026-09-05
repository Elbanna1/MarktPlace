using FluentValidation;
using Shared.DTOs.Antiques;
using Shared.Enums;

namespace Services.Validation;

public class UpdateCoinStampRequestValidator : AbstractValidator<UpdateCoinStampRequest>
{
    public UpdateCoinStampRequestValidator()
    {
        AntiqueValidationRules.Seller(this, x => x.SellerName, x => x.Phone, x => x.WhatsApp);

        RuleFor(x => x.ItemName)
            .NotEmpty().WithMessage("اسم الحاجة مطلوب.")
            .MaximumLength(150);

        RuleFor(x => x.ItemType)
            .IsInEnum().WithMessage("من فضلك اختار نوع الحاجة صح.");

        RuleFor(x => x.OtherType)
            .NotEmpty().WithMessage("من فضلك اكتب نوع الحاجة.")
            .MaximumLength(150)
            .When(x => x.ItemType == CoinStampItemType.Other);

        RuleFor(x => x.Country)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.Country));

        AntiqueValidationRules.Year(this, x => x.IssueYear, "IssueYear");

        RuleFor(x => x.Denomination)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.Denomination));

        RuleFor(x => x.Metal)
            .IsInEnum().WithMessage("من فضلك اختار المعدن صح.");

        RuleFor(x => x.OtherMetal)
            .NotEmpty().WithMessage("من فضلك اكتب المعدن.")
            .MaximumLength(150)
            .When(x => x.Metal == CoinStampMetal.Other);

        RuleFor(x => x.Condition)
            .IsInEnum().WithMessage("من فضلك اختار الحالة صح.");

        AntiqueValidationRules.Price(this, x => x.Price);
        AntiqueValidationRules.Location(this, x => x.Center, x => x.Address, x => x.GoogleMaps);
        AntiqueValidationRules.Advertisement(this, x => x.Title, x => x.Description);
        AntiqueValidationRules.Images(this, x => x.Images, required: false);
        AntiqueValidationRules.Video(this, x => x.Video);
    }
}
