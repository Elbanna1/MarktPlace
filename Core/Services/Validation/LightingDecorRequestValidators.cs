using FluentValidation;
using Shared.DTOs.HomeFurnishing;
using Shared.Enums;

namespace Services.Validation;

public class CreateLightingDecorRequestValidator
    : LightingDecorRequestValidatorBase<CreateLightingDecorRequest>
{
    public CreateLightingDecorRequestValidator() : base(imagesRequired: true)
    {
    }
}

public class UpdateLightingDecorRequestValidator
    : LightingDecorRequestValidatorBase<UpdateLightingDecorRequest>
{
    public UpdateLightingDecorRequestValidator() : base(imagesRequired: false)
    {
    }
}

public abstract class LightingDecorRequestValidatorBase<TRequest>
    : HomeFurnishingRequestValidator<TRequest>
    where TRequest : CreateLightingDecorRequest
{
    protected LightingDecorRequestValidatorBase(bool imagesRequired) : base(imagesRequired)
    {
        RuleFor(x => x.ProductType)
            .IsInEnum().WithMessage("من فضلك اختار نوع المنتج صح.");

        RequiredWhenOther(
            x => x.OtherProductType,
            x => x.ProductType == LightingDecorProductType.Other,
            "Please specify the product type when 'أخرى' is selected.");

        RuleFor(x => x.Material)
            .IsInEnum().WithMessage("من فضلك اختار الخامة صح.");

        RequiredWhenOther(
            x => x.OtherMaterial,
            x => x.Material == LightingDecorMaterial.Other,
            "Please specify the material when 'أخرى' is selected.");

        RuleFor(x => x.Colors)
            .NotEmpty().WithMessage("لازم تختار لون واحد على الأقل.")
            .Must(colors => colors.All(color => Enum.IsDefined(typeof(LightingDecorColor), color)))
            .WithMessage("فيه لون أو أكتر من اللي اخترتهم مش صحيح.");

        RequiredWhenOther(
            x => x.OtherColor,
            x => x.Colors.Contains(LightingDecorColor.Other),
            "Please specify the color when 'أخرى' is selected.");

        RuleFor(x => x.LightType)
            .NotNull().WithMessage("نوع الإضاءة مطلوب لمنتجات الإضاءة.")
            .IsInEnum().WithMessage("من فضلك اختار نوع الإضاءة صح.")
            .When(x => LightingDecorProductTypes.IsLighting(x.ProductType));
    }
}
