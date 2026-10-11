using FluentValidation;
using Shared.Constants;
using Shared.DTOs.GovernorateGuide;
using Shared.Enums;

namespace Services.Validation;

public static class GovernorateGuideValidationRules
{
    public static void AddCommercialShopRules<T>(this AbstractValidator<T> validator)
        where T : CreateCommercialShopRequest
    {
        validator.RuleFor(x => x.ActivityType)
            .IsInEnum().WithMessage($"من فضلك اختار {GovernorateGuideCatalog.ActivityTypeLabel} صح.")
            .When(x => x.ActivityType.HasValue);

        validator.RuleFor(x => x.OtherActivityType)
            .NotEmpty().WithMessage("من فضلك اكتب نوع النشاط عند اختيار 'أخرى'.")
            .When(x => x.ActivityType == ShopSuitableActivity.Other);

        validator.RuleFor(x => x.OtherActivityType)
            .MaximumLength(GovernorateGuideCatalog.MaxOtherActivityTypeLength)
            .WithMessage($"نوع النشاط لازم ميزيدش عن {GovernorateGuideCatalog.MaxOtherActivityTypeLength} حرف.")
            .When(x => x.ActivityType == ShopSuitableActivity.Other);
    }

    public static void AddGeneralServiceRules<T>(this AbstractValidator<T> validator)
        where T : CreateGeneralServiceRequest
    {
        validator.RuleFor(x => x.ServiceType)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage($"{GovernorateGuideCatalog.ServiceTypeLabel} مطلوب.")
            .IsInEnum().WithMessage("من فضلك اختار نوع خدمة صحيح (خدمات ثابتة / خدمات متنقلة).");

        validator.RuleFor(x => x.FixedServiceSubtype)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage($"{GovernorateGuideCatalog.FixedServiceSubtypeLabel} مطلوب.")
            .Must(subtype => GovernorateGuideCatalog.BelongsTo(subtype, GeneralServiceType.Fixed))
            .WithMessage("من فضلك اختار خدمة من قائمة الخدمات الثابتة.")
            .When(x => x.ServiceType == GeneralServiceType.Fixed);

        validator.RuleFor(x => x.MobileServiceSubtype)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage($"{GovernorateGuideCatalog.MobileServiceSubtypeLabel} مطلوب.")
            .Must(subtype => GovernorateGuideCatalog.BelongsTo(subtype, GeneralServiceType.Mobile))
            .WithMessage("من فضلك اختار خدمة من قائمة الخدمات المتنقلة.")
            .When(x => x.ServiceType == GeneralServiceType.Mobile);
    }
}

public class CreateCommercialShopRequestValidator : AbstractValidator<CreateCommercialShopRequest>
{
    public CreateCommercialShopRequestValidator()
    {
        this.AddDirectoryRules(GovernorateGuideCatalog.CommercialShops, isCreate: true);
        this.AddCommercialShopRules();
    }
}

public class UpdateCommercialShopRequestValidator : AbstractValidator<UpdateCommercialShopRequest>
{
    public UpdateCommercialShopRequestValidator()
    {
        this.AddDirectoryRules(GovernorateGuideCatalog.CommercialShops, isCreate: false);
        this.AddCommercialShopRules();
    }
}

public class CommercialShopFilterParamsValidator : DirectoryFilterParamsValidator<CommercialShopFilterParams>;

public class CreateGeneralServiceRequestValidator : AbstractValidator<CreateGeneralServiceRequest>
{
    public CreateGeneralServiceRequestValidator()
    {
        this.AddDirectoryRules(GovernorateGuideCatalog.GeneralServices, isCreate: true);
        this.AddGeneralServiceRules();
    }
}

public class UpdateGeneralServiceRequestValidator : AbstractValidator<UpdateGeneralServiceRequest>
{
    public UpdateGeneralServiceRequestValidator()
    {
        this.AddDirectoryRules(GovernorateGuideCatalog.GeneralServices, isCreate: false);
        this.AddGeneralServiceRules();
    }
}

public class GeneralServiceFilterParamsValidator : DirectoryFilterParamsValidator<GeneralServiceFilterParams>;

public class CreateResortHotelAmusementRequestValidator : AbstractValidator<CreateResortHotelAmusementRequest>
{
    public CreateResortHotelAmusementRequestValidator() =>
        this.AddDirectoryRules(GovernorateGuideCatalog.ResortsHotelsAndAmusement, isCreate: true);
}

public class UpdateResortHotelAmusementRequestValidator : AbstractValidator<UpdateResortHotelAmusementRequest>
{
    public UpdateResortHotelAmusementRequestValidator() =>
        this.AddDirectoryRules(GovernorateGuideCatalog.ResortsHotelsAndAmusement, isCreate: false);
}

public class ResortHotelAmusementFilterParamsValidator : DirectoryFilterParamsValidator<ResortHotelAmusementFilterParams>;
