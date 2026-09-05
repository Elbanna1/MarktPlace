using FluentValidation;
using Shared.DTOs.HomeFurnishing;
using Shared.Enums;

namespace Services.Validation;

public class CreateBathroomSupplyRequestValidator
    : BathroomSupplyRequestValidatorBase<CreateBathroomSupplyRequest>
{
    public CreateBathroomSupplyRequestValidator() : base(imagesRequired: true)
    {
    }
}

public class UpdateBathroomSupplyRequestValidator
    : BathroomSupplyRequestValidatorBase<UpdateBathroomSupplyRequest>
{
    public UpdateBathroomSupplyRequestValidator() : base(imagesRequired: false)
    {
    }
}

public abstract class BathroomSupplyRequestValidatorBase<TRequest>
    : HomeFurnishingRequestValidator<TRequest>
    where TRequest : CreateBathroomSupplyRequest
{
    protected BathroomSupplyRequestValidatorBase(bool imagesRequired) : base(imagesRequired)
    {
        RuleFor(x => x.ProductType)
            .IsInEnum().WithMessage("من فضلك اختار نوع المنتج صح.");

        RequiredWhenOther(
            x => x.OtherProductType,
            x => x.ProductType == BathroomSupplyProductType.Other,
            "Please specify the product type when 'أخرى' is selected.");

        RuleFor(x => x.Material)
            .IsInEnum().WithMessage("من فضلك اختار الخامة صح.");

        RequiredWhenOther(
            x => x.OtherMaterial,
            x => x.Material == BathroomSupplyMaterial.Other,
            "Please specify the material when 'أخرى' is selected.");

        RuleFor(x => x.Colors)
            .NotEmpty().WithMessage("لازم تختار لون واحد على الأقل.")
            .Must(colors => colors.All(color => Enum.IsDefined(typeof(BathroomSupplyColor), color)))
            .WithMessage("فيه لون أو أكتر من اللي اخترتهم مش صحيح.");

        RequiredWhenOther(
            x => x.OtherColor,
            x => x.Colors.Contains(BathroomSupplyColor.Other),
            "Please specify the color when 'أخرى' is selected.");
    }
}
