using FluentValidation;
using Shared.DTOs.OnlineShopping;
using Shared.Enums;

namespace Services.Validation;

public class CreateGiftToyRequestValidator : AbstractValidator<CreateGiftToyRequest>
{
    public CreateGiftToyRequestValidator()
    {
        OnlineShoppingValidationRules.StoreName(this, x => x.StoreName);
        OnlineShoppingValidationRules.Contact(this, x => x.Phone, x => x.WhatsApp);

        RuleFor(x => x.GiftType)
            .IsInEnum().WithMessage("من فضلك اختار نوع الهدية صح.");

        OnlineShoppingValidationRules.OtherText(this, x => x.OtherGiftType,
            x => x.GiftType == GiftToyType.Other,
            "Please specify the type when 'أخرى' is selected.");

        RuleFor(x => x.SuitableFor)
            .IsInEnum().WithMessage("من فضلك اختار «مناسب لـ» صح.");

        OnlineShoppingValidationRules.Price(this, x => x.Price);
        OnlineShoppingValidationRules.RequiredImages(this, x => x.Images);
        OnlineShoppingValidationRules.Video(this, x => x.Video!);
        OnlineShoppingValidationRules.Advertisement(this, x => x.Title, x => x.Description);
    }
}

public class UpdateGiftToyRequestValidator : AbstractValidator<UpdateGiftToyRequest>
{
    public UpdateGiftToyRequestValidator()
    {
        OnlineShoppingValidationRules.StoreName(this, x => x.StoreName);
        OnlineShoppingValidationRules.Contact(this, x => x.Phone, x => x.WhatsApp);

        RuleFor(x => x.GiftType)
            .IsInEnum().WithMessage("من فضلك اختار نوع الهدية صح.");

        OnlineShoppingValidationRules.OtherText(this, x => x.OtherGiftType,
            x => x.GiftType == GiftToyType.Other,
            "Please specify the type when 'أخرى' is selected.");

        RuleFor(x => x.SuitableFor)
            .IsInEnum().WithMessage("من فضلك اختار «مناسب لـ» صح.");

        OnlineShoppingValidationRules.Price(this, x => x.Price);
        OnlineShoppingValidationRules.OptionalImages(this, x => x.Images);
        OnlineShoppingValidationRules.Video(this, x => x.Video!);
        OnlineShoppingValidationRules.Advertisement(this, x => x.Title, x => x.Description);
    }
}
