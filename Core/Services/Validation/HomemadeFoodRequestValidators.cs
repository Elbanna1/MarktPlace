using FluentValidation;
using Shared.DTOs.OnlineShopping;
using Shared.Enums;

namespace Services.Validation;

public class CreateHomemadeFoodRequestValidator : AbstractValidator<CreateHomemadeFoodRequest>
{
    public CreateHomemadeFoodRequestValidator()
    {
        OnlineShoppingValidationRules.StoreName(this, x => x.ProjectName, "اسم المشروع");
        OnlineShoppingValidationRules.Contact(this, x => x.Phone, x => x.WhatsApp);

        RuleFor(x => x.Section)
            .IsInEnum().WithMessage("من فضلك اختار القسم صح.");

        OnlineShoppingValidationRules.OtherText(this, x => x.OtherSection,
            x => x.Section == HomemadeFoodSection.Other,
            "Please specify the section when 'أخرى' is selected.");

        RuleFor(x => x.MinimumOrderQuantity)
            .GreaterThan(0).WithMessage("أقل كمية للطلب لازم تكون أكبر من صفر.");

        RuleFor(x => x.PreparationTime)
            .NotEmpty().WithMessage("وقت التحضير مطلوب.")
            .MaximumLength(100);

        RuleFor(x => x.DeliveryAreas)
            .NotEmpty().WithMessage("لازم تختار منطقة توصيل واحدة على الأقل لما التوصيل يكون متاح.")
            .Must(areas => areas.All(area => Enum.IsDefined(typeof(HomemadeFoodDeliveryArea), area)))
            .WithMessage("فيه منطقة توصيل أو أكتر من اللي اخترتهم مش صحيحة.")
            .When(x => x.DeliveryAvailable);

        RuleFor(x => x.DeliveryAreas)
            .Empty().WithMessage("مناطق التوصيل تتحدد بس لما التوصيل يكون متاح.")
            .When(x => !x.DeliveryAvailable);

        RuleFor(x => x.Ingredients).MaximumLength(2000);
        RuleFor(x => x.WeightOrSize).MaximumLength(150);
        RuleFor(x => x.StorageMethod).MaximumLength(500);
        RuleFor(x => x.AvailableOrderingHours).MaximumLength(200);
        RuleFor(x => x.AdditionalNotes).MaximumLength(2000);

        OnlineShoppingValidationRules.Price(this, x => x.Price);
        OnlineShoppingValidationRules.RequiredImages(this, x => x.Images);
        OnlineShoppingValidationRules.Video(this, x => x.Video!);
        OnlineShoppingValidationRules.Advertisement(this, x => x.Title, x => x.Description);
    }
}

public class UpdateHomemadeFoodRequestValidator : AbstractValidator<UpdateHomemadeFoodRequest>
{
    public UpdateHomemadeFoodRequestValidator()
    {
        OnlineShoppingValidationRules.StoreName(this, x => x.ProjectName, "اسم المشروع");
        OnlineShoppingValidationRules.Contact(this, x => x.Phone, x => x.WhatsApp);

        RuleFor(x => x.Section)
            .IsInEnum().WithMessage("من فضلك اختار القسم صح.");

        OnlineShoppingValidationRules.OtherText(this, x => x.OtherSection,
            x => x.Section == HomemadeFoodSection.Other,
            "Please specify the section when 'أخرى' is selected.");

        RuleFor(x => x.MinimumOrderQuantity)
            .GreaterThan(0).WithMessage("أقل كمية للطلب لازم تكون أكبر من صفر.");

        RuleFor(x => x.PreparationTime)
            .NotEmpty().WithMessage("وقت التحضير مطلوب.")
            .MaximumLength(100);

        RuleFor(x => x.DeliveryAreas)
            .NotEmpty().WithMessage("لازم تختار منطقة توصيل واحدة على الأقل لما التوصيل يكون متاح.")
            .Must(areas => areas.All(area => Enum.IsDefined(typeof(HomemadeFoodDeliveryArea), area)))
            .WithMessage("فيه منطقة توصيل أو أكتر من اللي اخترتهم مش صحيحة.")
            .When(x => x.DeliveryAvailable);

        RuleFor(x => x.DeliveryAreas)
            .Empty().WithMessage("مناطق التوصيل تتحدد بس لما التوصيل يكون متاح.")
            .When(x => !x.DeliveryAvailable);

        RuleFor(x => x.Ingredients).MaximumLength(2000);
        RuleFor(x => x.WeightOrSize).MaximumLength(150);
        RuleFor(x => x.StorageMethod).MaximumLength(500);
        RuleFor(x => x.AvailableOrderingHours).MaximumLength(200);
        RuleFor(x => x.AdditionalNotes).MaximumLength(2000);

        OnlineShoppingValidationRules.Price(this, x => x.Price);
        OnlineShoppingValidationRules.OptionalImages(this, x => x.Images);
        OnlineShoppingValidationRules.Video(this, x => x.Video!);
        OnlineShoppingValidationRules.Advertisement(this, x => x.Title, x => x.Description);
    }
}
