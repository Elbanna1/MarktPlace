using FluentValidation;
using Shared.DTOs.OnlineShopping;
using Shared.Enums;

namespace Services.Validation;

public class CreateHomeKitchenRequestValidator : AbstractValidator<CreateHomeKitchenRequest>
{
    public CreateHomeKitchenRequestValidator()
    {
        OnlineShoppingValidationRules.StoreName(this, x => x.StoreName);
        OnlineShoppingValidationRules.Contact(this, x => x.Phone, x => x.WhatsApp);

        RuleFor(x => x.Section)
            .IsInEnum().WithMessage("من فضلك اختار القسم صح.");

        OnlineShoppingValidationRules.OtherText(this, x => x.OtherSection,
            x => x.Section == HomeKitchenSection.Other,
            "Please specify the section when 'أخرى' is selected.");

        RuleFor(x => x.Material)
            .IsInEnum().WithMessage("من فضلك اختار الخامة صح.");

        OnlineShoppingValidationRules.OtherText(this, x => x.OtherMaterial,
            x => x.Material == HomeKitchenMaterial.Other,
            "Please specify the material when 'أخرى' is selected.");

        RuleFor(x => x.Colors)
            .NotEmpty().WithMessage("لازم تختار لون واحد على الأقل.")
            .Must(colors => colors.All(color => Enum.IsDefined(typeof(HomeKitchenColor), color)))
            .WithMessage("فيه لون أو أكتر من اللي اخترتهم مش صحيح.");

        OnlineShoppingValidationRules.OtherText(this, x => x.OtherColor,
            x => x.Colors.Contains(HomeKitchenColor.Other),
            "Please specify the color when 'أخرى' is selected.");

        OnlineShoppingValidationRules.Price(this, x => x.Price);
        OnlineShoppingValidationRules.RequiredImages(this, x => x.Images);
        OnlineShoppingValidationRules.Video(this, x => x.Video!);
        OnlineShoppingValidationRules.Advertisement(this, x => x.Title, x => x.Description);
    }
}

public class UpdateHomeKitchenRequestValidator : AbstractValidator<UpdateHomeKitchenRequest>
{
    public UpdateHomeKitchenRequestValidator()
    {
        OnlineShoppingValidationRules.StoreName(this, x => x.StoreName);
        OnlineShoppingValidationRules.Contact(this, x => x.Phone, x => x.WhatsApp);

        RuleFor(x => x.Section)
            .IsInEnum().WithMessage("من فضلك اختار القسم صح.");

        OnlineShoppingValidationRules.OtherText(this, x => x.OtherSection,
            x => x.Section == HomeKitchenSection.Other,
            "Please specify the section when 'أخرى' is selected.");

        RuleFor(x => x.Material)
            .IsInEnum().WithMessage("من فضلك اختار الخامة صح.");

        OnlineShoppingValidationRules.OtherText(this, x => x.OtherMaterial,
            x => x.Material == HomeKitchenMaterial.Other,
            "Please specify the material when 'أخرى' is selected.");

        RuleFor(x => x.Colors)
            .NotEmpty().WithMessage("لازم تختار لون واحد على الأقل.")
            .Must(colors => colors.All(color => Enum.IsDefined(typeof(HomeKitchenColor), color)))
            .WithMessage("فيه لون أو أكتر من اللي اخترتهم مش صحيح.");

        OnlineShoppingValidationRules.OtherText(this, x => x.OtherColor,
            x => x.Colors.Contains(HomeKitchenColor.Other),
            "Please specify the color when 'أخرى' is selected.");

        OnlineShoppingValidationRules.Price(this, x => x.Price);
        OnlineShoppingValidationRules.OptionalImages(this, x => x.Images);
        OnlineShoppingValidationRules.Video(this, x => x.Video!);
        OnlineShoppingValidationRules.Advertisement(this, x => x.Title, x => x.Description);
    }
}
