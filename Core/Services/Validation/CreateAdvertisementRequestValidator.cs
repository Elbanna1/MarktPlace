using FluentValidation;
using Shared.Constants;
using Shared.DTOs.Advertisements;

namespace Services.Validation;

public class CreateAdvertisementRequestValidator : AbstractValidator<CreateAdvertisementRequest>
{
    public CreateAdvertisementRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("العنوان مطلوب.")
            .MaximumLength(150);

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("الوصف مطلوب.")
            .MaximumLength(4000);

        RuleFor(x => x.Price)
            .NotNull().WithMessage("السعر مطلوب.")
            .GreaterThan(0).WithMessage("السعر لازم يكون أكبر من صفر.")
            .When(AdvertisementValidationRules.PricesBySalePrice);

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0).WithMessage("السعر ما ينفعش يكون بالسالب.")
            .When(x => !AdvertisementValidationRules.PricesBySalePrice(x) && x.Price.HasValue);

        RuleFor(x => x.ListingType)
            .IsInEnum().WithMessage("من فضلك اختار نوع إعلان صحيح.")
            .When(x => AdvertisementValidationRules.IsVehicle(x.SubCategoryId));

        RuleFor(x => x.SubCategoryId)
            .GreaterThan(0).WithMessage("من فضلك اختار قسم فرعي صحيح.");

        RuleFor(x => x.Center)
            .NotEmpty().WithMessage("المركز مطلوب.")
            .Must(LocationConstants.IsValidCenter)
            .WithMessage($"المركز لازم يكون واحد من: {string.Join(", ", LocationConstants.Centers)}.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("رقم الموبايل مطلوب.")
            .Matches(@"^01[0-2,5]{1}[0-9]{8}$")
            .WithMessage("من فضلك اكتب رقم موبايل مصري صحيح (مثال: 01012345678).");

        RuleFor(x => x.FeatureIds)
            .Must(ids => ids == null || ids.Count <= 100)
            .WithMessage("اخترت مميزات أكتر من المسموح.");

        RuleFor(x => x.Images)
            .Must(images => images is { Count: > 0 })
            .WithMessage("لازم ترفع صورة واحدة على الأقل.")
            .When(x => AdvertisementValidationRules.IsVehicle(x.SubCategoryId));

        this.AddVehicleRules();
        this.AddBusinessRules();
    }
}
