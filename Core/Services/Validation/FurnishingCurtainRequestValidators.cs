using FluentValidation;
using Shared.DTOs.HomeFurnishing;
using Shared.Enums;

namespace Services.Validation;

public class CreateFurnishingCurtainRequestValidator
    : FurnishingCurtainRequestValidatorBase<CreateFurnishingCurtainRequest>
{
    public CreateFurnishingCurtainRequestValidator() : base(imagesRequired: true)
    {
    }
}

public class UpdateFurnishingCurtainRequestValidator
    : FurnishingCurtainRequestValidatorBase<UpdateFurnishingCurtainRequest>
{
    public UpdateFurnishingCurtainRequestValidator() : base(imagesRequired: false)
    {
    }
}

public abstract class FurnishingCurtainRequestValidatorBase<TRequest>
    : HomeFurnishingRequestValidator<TRequest>
    where TRequest : CreateFurnishingCurtainRequest
{
    protected FurnishingCurtainRequestValidatorBase(bool imagesRequired) : base(imagesRequired)
    {
        RuleFor(x => x.ProductType)
            .IsInEnum().WithMessage("من فضلك اختار نوع المنتج صح.");

        RequiredWhenOther(
            x => x.OtherProductType,
            x => x.ProductType == FurnishingCurtainProductType.Other,
            "Please specify the product type when 'أخرى' is selected.");

        RuleFor(x => x.Size)
            .IsInEnum().WithMessage("من فضلك اختار المقاس صح.");

        RequiredWhenOther(
            x => x.OtherSize,
            x => x.Size == FurnishingCurtainSize.Other,
            "Please specify the size when 'أخرى' is selected.");

        RuleFor(x => x.Material)
            .IsInEnum().WithMessage("من فضلك اختار الخامة صح.");

        RequiredWhenOther(
            x => x.OtherMaterial,
            x => x.Material == FurnishingCurtainMaterial.Other,
            "Please specify the material when 'أخرى' is selected.");

        RuleFor(x => x.Colors)
            .NotEmpty().WithMessage("لازم تختار لون واحد على الأقل.")
            .Must(colors => colors.All(color => Enum.IsDefined(typeof(FurnishingCurtainColor), color)))
            .WithMessage("فيه لون أو أكتر من اللي اخترتهم مش صحيح.");

        RequiredWhenOther(
            x => x.OtherColor,
            x => x.Colors.Contains(FurnishingCurtainColor.Other),
            "Please specify the color when 'أخرى' is selected.");
    }
}
