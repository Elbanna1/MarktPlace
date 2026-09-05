using FluentValidation;
using Shared.DTOs.OnlineShopping;
using Shared.Enums;

namespace Services.Validation;

public class CreateAccessoryRequestValidator : AbstractValidator<CreateAccessoryRequest>
{
    public CreateAccessoryRequestValidator()
    {
        OnlineShoppingValidationRules.StoreName(this, x => x.StoreName);
        OnlineShoppingValidationRules.Contact(this, x => x.Phone, x => x.WhatsApp);
        OnlineShoppingValidationRules.Logo(this, x => x.Logo!);

        RuleFor(x => x.AccessoryType)
            .IsInEnum().WithMessage("من فضلك اختار نوع الإكسسوار صح.");

        OnlineShoppingValidationRules.OtherText(this, x => x.OtherAccessoryType,
            x => x.AccessoryType == AccessoryType.Other,
            "Please specify the accessory type when 'أخرى' is selected.");

        RuleFor(x => x.Category)
            .IsInEnum().WithMessage("من فضلك اختار قسم صحيح.");

        RuleFor(x => x.Material)
            .IsInEnum().WithMessage("من فضلك اختار الخامة صح.");

        OnlineShoppingValidationRules.OtherText(this, x => x.OtherMaterial,
            x => x.Material == AccessoryMaterial.Other,
            "Please specify the material when 'أخرى' is selected.");

        RuleFor(x => x.Colors)
            .NotEmpty().WithMessage("لازم تختار لون واحد على الأقل.")
            .Must(colors => colors.All(color => Enum.IsDefined(typeof(AccessoryColor), color)))
            .WithMessage("فيه لون أو أكتر من اللي اخترتهم مش صحيح.");

        OnlineShoppingValidationRules.OtherText(this, x => x.OtherColor,
            x => x.Colors.Contains(AccessoryColor.Other),
            "Please specify the color when 'أخرى' is selected.");

        OnlineShoppingValidationRules.Price(this, x => x.Price);
        OnlineShoppingValidationRules.RequiredImages(this, x => x.Images);
        OnlineShoppingValidationRules.Video(this, x => x.Video!);
        OnlineShoppingValidationRules.Advertisement(this, x => x.Title, x => x.Description);
    }
}

public class UpdateAccessoryRequestValidator : AbstractValidator<UpdateAccessoryRequest>
{
    public UpdateAccessoryRequestValidator()
    {
        OnlineShoppingValidationRules.StoreName(this, x => x.StoreName);
        OnlineShoppingValidationRules.Contact(this, x => x.Phone, x => x.WhatsApp);
        OnlineShoppingValidationRules.Logo(this, x => x.Logo!);

        RuleFor(x => x.AccessoryType)
            .IsInEnum().WithMessage("من فضلك اختار نوع الإكسسوار صح.");

        OnlineShoppingValidationRules.OtherText(this, x => x.OtherAccessoryType,
            x => x.AccessoryType == AccessoryType.Other,
            "Please specify the accessory type when 'أخرى' is selected.");

        RuleFor(x => x.Category)
            .IsInEnum().WithMessage("من فضلك اختار قسم صحيح.");

        RuleFor(x => x.Material)
            .IsInEnum().WithMessage("من فضلك اختار الخامة صح.");

        OnlineShoppingValidationRules.OtherText(this, x => x.OtherMaterial,
            x => x.Material == AccessoryMaterial.Other,
            "Please specify the material when 'أخرى' is selected.");

        RuleFor(x => x.Colors)
            .NotEmpty().WithMessage("لازم تختار لون واحد على الأقل.")
            .Must(colors => colors.All(color => Enum.IsDefined(typeof(AccessoryColor), color)))
            .WithMessage("فيه لون أو أكتر من اللي اخترتهم مش صحيح.");

        OnlineShoppingValidationRules.OtherText(this, x => x.OtherColor,
            x => x.Colors.Contains(AccessoryColor.Other),
            "Please specify the color when 'أخرى' is selected.");

        OnlineShoppingValidationRules.Price(this, x => x.Price);
        OnlineShoppingValidationRules.OptionalImages(this, x => x.Images);
        OnlineShoppingValidationRules.Video(this, x => x.Video!);
        OnlineShoppingValidationRules.Advertisement(this, x => x.Title, x => x.Description);
    }
}
