using FluentValidation;
using Shared.DTOs.HomeFurnishing;
using Shared.Enums;

namespace Services.Validation;

public class CreateFurnitureRequestValidator : FurnitureRequestValidatorBase<CreateFurnitureRequest>
{
    public CreateFurnitureRequestValidator() : base(imagesRequired: true)
    {
    }
}

public class UpdateFurnitureRequestValidator : FurnitureRequestValidatorBase<UpdateFurnitureRequest>
{
    public UpdateFurnitureRequestValidator() : base(imagesRequired: false)
    {
    }
}

public abstract class FurnitureRequestValidatorBase<TRequest> : HomeFurnishingRequestValidator<TRequest>
    where TRequest : CreateFurnitureRequest
{
    protected FurnitureRequestValidatorBase(bool imagesRequired) : base(imagesRequired)
    {
        RuleFor(x => x.FurnitureType)
            .IsInEnum().WithMessage("من فضلك اختار نوع الأثاث صح.");

        RequiredWhenOther(
            x => x.OtherFurnitureType,
            x => x.FurnitureType == FurnitureType.Other,
            "Please specify the furniture type when 'أخرى' is selected.");

        RuleFor(x => x.Material)
            .IsInEnum().WithMessage("من فضلك اختار الخامة صح.");

        RequiredWhenOther(
            x => x.OtherMaterial,
            x => x.Material == FurnitureMaterial.Other,
            "Please specify the material when 'أخرى' is selected.");

        RuleFor(x => x.Colors)
            .NotEmpty().WithMessage("لازم تختار لون واحد على الأقل.")
            .Must(colors => colors.All(color => Enum.IsDefined(typeof(FurnitureColor), color)))
            .WithMessage("فيه لون أو أكتر من اللي اخترتهم مش صحيح.");

        RequiredWhenOther(
            x => x.OtherColor,
            x => x.Colors.Contains(FurnitureColor.Other),
            "Please specify the color when 'أخرى' is selected.");

        RuleFor(x => x.Condition)
            .IsInEnum().WithMessage("من فضلك اختار الحالة صح.");

        RuleFor(x => x.Length)
            .GreaterThan(0).WithMessage("الطول مطلوب ولازم يكون أكبر من صفر.");

        RuleFor(x => x.Width)
            .GreaterThan(0).WithMessage("العرض مطلوب ولازم يكون أكبر من صفر.");

        RuleFor(x => x.Height)
            .GreaterThan(0).WithMessage("الارتفاع مطلوب ولازم يكون أكبر من صفر.");
    }
}
