using FluentValidation;
using Shared.Constants;
using Shared.DTOs.Farms;
using Shared.Enums;

namespace Services.Validation;

public class UpdateFarmRequestValidator : AbstractValidator<UpdateFarmRequest>
{
    public UpdateFarmRequestValidator()
    {
        RuleFor(x => x.FarmName)
            .NotEmpty().WithMessage("اسم المزرعة مطلوب.")
            .MaximumLength(150);

        RuleFor(x => x.FarmType)
            .IsInEnum().WithMessage("من فضلك اختار نوع المزرعة صح.");

        RuleFor(x => x.OtherFarmType)
            .NotEmpty().WithMessage("من فضلك اكتب نوع المزرعة لما تختار «أخرى».")
            .MaximumLength(150)
            .When(x => x.FarmType == FarmType.Other);

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("العنوان مطلوب.")
            .MaximumLength(300);

        BusinessValidationRules.RequiredGoogleMaps(this, x => x.GoogleMaps);

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("رقم الموبايل مطلوب.")
            .Matches(AdvertisementCatalog.EgyptianPhonePattern)
            .WithMessage(AdvertisementCatalog.EgyptianPhoneMessage);

        RuleFor(x => x.WhatsApp)
            .NotEmpty().WithMessage("رقم الواتساب مطلوب.")
            .Matches(AdvertisementCatalog.EgyptianPhonePattern)
            .WithMessage(AdvertisementCatalog.EgyptianPhoneMessage);

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("من فضلك اكتب بريد إلكتروني صحيح.")
            .MaximumLength(256)
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.AreaInFeddan)
            .GreaterThan(0).WithMessage("المساحة بالفدان لازم تكون أكبر من صفر.")
            .When(x => x.AreaInFeddan.HasValue);

        RuleFor(x => x.AvailableQuantity)
            .MaximumLength(150)
            .When(x => !string.IsNullOrWhiteSpace(x.AvailableQuantity));

        RuleFor(x => x.AvailabilitySeason)
            .IsInEnum().WithMessage("من فضلك اختار موسم التوافر صح.")
            .When(x => x.AvailabilitySeason.HasValue);

        RuleFor(x => x.FarmingMethod)
            .IsInEnum().WithMessage("من فضلك اختار طريقة الزراعة صح.")
            .When(x => x.FarmingMethod.HasValue);

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("عنوان الإعلان مطلوب.")
            .MaximumLength(150);

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("وصف الإعلان مطلوب.")
            .MaximumLength(4000);

        RuleFor(x => x.Images)
            .Must(images => images == null || images.Count <= ImageConstants.MaxImagesPerItem)
            .WithMessage($"مسموح بـ {ImageConstants.MaxImagesPerItem} صور بحد أقصى.");
    }
}
