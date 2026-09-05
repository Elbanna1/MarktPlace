using FluentValidation;
using Shared.DTOs.HomeFurnishing;
using Shared.Enums;

namespace Services.Validation;

public class CreatePlantOrnamentRequestValidator
    : PlantOrnamentRequestValidatorBase<CreatePlantOrnamentRequest>
{
    public CreatePlantOrnamentRequestValidator() : base(imagesRequired: true)
    {
    }
}

public class UpdatePlantOrnamentRequestValidator
    : PlantOrnamentRequestValidatorBase<UpdatePlantOrnamentRequest>
{
    public UpdatePlantOrnamentRequestValidator() : base(imagesRequired: false)
    {
    }
}

public abstract class PlantOrnamentRequestValidatorBase<TRequest>
    : HomeFurnishingRequestValidator<TRequest>
    where TRequest : CreatePlantOrnamentRequest
{
    protected PlantOrnamentRequestValidatorBase(bool imagesRequired) : base(imagesRequired)
    {
        RuleFor(x => x.ProductType)
            .IsInEnum().WithMessage("من فضلك اختار نوع المنتج صح.");

        RequiredWhenOther(
            x => x.OtherProductType,
            x => x.ProductType == PlantOrnamentProductType.Other,
            "Please specify the product type when 'أخرى' is selected.");

        RuleFor(x => x.SuitableFor)
            .IsInEnum().WithMessage("من فضلك اختار «مناسب لـ» صح.");

        RuleFor(x => x.Height)
            .GreaterThan(0).WithMessage("الارتفاع لازم يكون أكبر من صفر.")
            .When(x => x.Height.HasValue);
    }
}
