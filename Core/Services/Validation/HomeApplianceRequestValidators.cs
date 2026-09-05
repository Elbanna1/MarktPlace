using FluentValidation;
using Shared.DTOs.HomeFurnishing;
using Shared.Enums;

namespace Services.Validation;

public class CreateHomeApplianceRequestValidator
    : HomeApplianceRequestValidatorBase<CreateHomeApplianceRequest>
{
    public CreateHomeApplianceRequestValidator() : base(imagesRequired: true)
    {
    }
}

public class UpdateHomeApplianceRequestValidator
    : HomeApplianceRequestValidatorBase<UpdateHomeApplianceRequest>
{
    public UpdateHomeApplianceRequestValidator() : base(imagesRequired: false)
    {
    }
}

public abstract class HomeApplianceRequestValidatorBase<TRequest>
    : HomeFurnishingRequestValidator<TRequest>
    where TRequest : CreateHomeApplianceRequest
{
    protected HomeApplianceRequestValidatorBase(bool imagesRequired) : base(imagesRequired)
    {
        RuleFor(x => x.DeviceType)
            .IsInEnum().WithMessage("من فضلك اختار نوع الجهاز صح.");

        RequiredWhenOther(
            x => x.OtherDeviceType,
            x => x.DeviceType == HomeApplianceDeviceType.Other,
            "Please specify the device type when 'أخرى' is selected.");

        RuleFor(x => x.Brand)
            .IsInEnum().WithMessage("من فضلك اختار الماركة صح.");

        RequiredWhenOther(
            x => x.OtherBrand,
            x => x.Brand == HomeApplianceBrand.Other,
            "Please specify the brand when 'أخرى' is selected.");

        RuleFor(x => x.Colors)
            .NotEmpty().WithMessage("لازم تختار لون واحد على الأقل.")
            .Must(colors => colors.All(color => Enum.IsDefined(typeof(HomeApplianceColor), color)))
            .WithMessage("فيه لون أو أكتر من اللي اخترتهم مش صحيح.");

        RequiredWhenOther(
            x => x.OtherColor,
            x => x.Colors.Contains(HomeApplianceColor.Other),
            "Please specify the color when 'أخرى' is selected.");

        RuleFor(x => x.Condition)
            .IsInEnum().WithMessage("من فضلك اختار الحالة صح.");

        RuleFor(x => x.Warranty)
            .IsInEnum().WithMessage("من فضلك اختار الضمان صح.");

        RequiredWhenOther(
            x => x.WarrantyDuration,
            x => x.Warranty == HomeApplianceWarranty.Available,
            "Please specify the warranty duration when a warranty is available.");

        RuleFor(x => x.PowerRating)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.PowerRating));
    }
}
