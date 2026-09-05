using FluentValidation;
using Shared.DTOs.HomeFurnishing;
using Shared.Enums;

namespace Services.Validation;

public class CreateKitchenToolRequestValidator
    : KitchenToolRequestValidatorBase<CreateKitchenToolRequest>
{
    public CreateKitchenToolRequestValidator() : base(imagesRequired: true)
    {
    }
}

public class UpdateKitchenToolRequestValidator
    : KitchenToolRequestValidatorBase<UpdateKitchenToolRequest>
{
    public UpdateKitchenToolRequestValidator() : base(imagesRequired: false)
    {
    }
}

public abstract class KitchenToolRequestValidatorBase<TRequest>
    : HomeFurnishingRequestValidator<TRequest>
    where TRequest : CreateKitchenToolRequest
{
    protected KitchenToolRequestValidatorBase(bool imagesRequired) : base(imagesRequired)
    {
        RuleFor(x => x.ProductType)
            .IsInEnum().WithMessage("من فضلك اختار نوع المنتج صح.");

        RequiredWhenOther(
            x => x.OtherProductType,
            x => x.ProductType == KitchenToolProductType.Other,
            "Please specify the product type when 'أخرى' is selected.");

        RuleFor(x => x.Material)
            .IsInEnum().WithMessage("من فضلك اختار الخامة صح.");

        RequiredWhenOther(
            x => x.OtherMaterial,
            x => x.Material == KitchenToolMaterial.Other,
            "Please specify the material when 'أخرى' is selected.");

        RuleFor(x => x.Colors)
            .NotEmpty().WithMessage("لازم تختار لون واحد على الأقل.")
            .Must(colors => colors.All(color => Enum.IsDefined(typeof(KitchenToolColor), color)))
            .WithMessage("فيه لون أو أكتر من اللي اخترتهم مش صحيح.");

        RequiredWhenOther(
            x => x.OtherColor,
            x => x.Colors.Contains(KitchenToolColor.Other),
            "Please specify the color when 'أخرى' is selected.");
    }
}
