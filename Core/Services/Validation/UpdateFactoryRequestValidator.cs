using FluentValidation;
using Shared.Constants;
using Shared.DTOs.Factories;
using Shared.Enums;

namespace Services.Validation;

public class UpdateFactoryRequestValidator : AbstractValidator<UpdateFactoryRequest>
{
    public UpdateFactoryRequestValidator()
    {
        RuleFor(x => x.FactoryName)
            .NotEmpty().WithMessage("اسم المصنع مطلوب.")
            .MaximumLength(150);

        RuleFor(x => x.ProductionSpecialty)
            .IsInEnum().WithMessage("من فضلك اختار تخصص الإنتاج صح.");

        RuleFor(x => x.OtherSpecialty)
            .NotEmpty().WithMessage("من فضلك اكتب تخصص الإنتاج لما تختار «أخرى».")
            .MaximumLength(150)
            .When(x => x.ProductionSpecialty == ProductionSpecialty.Other);

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
