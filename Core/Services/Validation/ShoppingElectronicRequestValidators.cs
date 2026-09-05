using FluentValidation;
using Shared.DTOs.OnlineShopping;
using Shared.Enums;

namespace Services.Validation;

public class CreateShoppingElectronicRequestValidator : AbstractValidator<CreateShoppingElectronicRequest>
{
    public CreateShoppingElectronicRequestValidator()
    {
        OnlineShoppingValidationRules.StoreName(this, x => x.StoreName);
        OnlineShoppingValidationRules.Contact(this, x => x.Phone, x => x.WhatsApp);

        RuleFor(x => x.Section)
            .IsInEnum().WithMessage("من فضلك اختار القسم صح.");

        OnlineShoppingValidationRules.OtherText(this, x => x.OtherSection,
            x => x.Section == ShoppingElectronicSection.Other,
            "Please specify the section when 'أخرى' is selected.");

        RuleFor(x => x.Brand)
            .NotEmpty().WithMessage("الماركة مطلوبة.")
            .MaximumLength(150);

        RuleFor(x => x.CompatibleWith)
            .IsInEnum().WithMessage("من فضلك اختار التوافق صح.");

        RuleFor(x => x.ProductCondition)
            .IsInEnum().WithMessage("من فضلك اختار حالة المنتج صح.");

        RuleFor(x => x.Warranty)
            .IsInEnum().WithMessage("من فضلك اختار الضمان صح.");

        OnlineShoppingValidationRules.Price(this, x => x.Price);
        OnlineShoppingValidationRules.RequiredImages(this, x => x.Images);
        OnlineShoppingValidationRules.Video(this, x => x.Video!);
        OnlineShoppingValidationRules.Advertisement(this, x => x.Title, x => x.Description);
    }
}

public class UpdateShoppingElectronicRequestValidator : AbstractValidator<UpdateShoppingElectronicRequest>
{
    public UpdateShoppingElectronicRequestValidator()
    {
        OnlineShoppingValidationRules.StoreName(this, x => x.StoreName);
        OnlineShoppingValidationRules.Contact(this, x => x.Phone, x => x.WhatsApp);

        RuleFor(x => x.Section)
            .IsInEnum().WithMessage("من فضلك اختار القسم صح.");

        OnlineShoppingValidationRules.OtherText(this, x => x.OtherSection,
            x => x.Section == ShoppingElectronicSection.Other,
            "Please specify the section when 'أخرى' is selected.");

        RuleFor(x => x.Brand)
            .NotEmpty().WithMessage("الماركة مطلوبة.")
            .MaximumLength(150);

        RuleFor(x => x.CompatibleWith)
            .IsInEnum().WithMessage("من فضلك اختار التوافق صح.");

        RuleFor(x => x.ProductCondition)
            .IsInEnum().WithMessage("من فضلك اختار حالة المنتج صح.");

        RuleFor(x => x.Warranty)
            .IsInEnum().WithMessage("من فضلك اختار الضمان صح.");

        OnlineShoppingValidationRules.Price(this, x => x.Price);
        OnlineShoppingValidationRules.OptionalImages(this, x => x.Images);
        OnlineShoppingValidationRules.Video(this, x => x.Video!);
        OnlineShoppingValidationRules.Advertisement(this, x => x.Title, x => x.Description);
    }
}
