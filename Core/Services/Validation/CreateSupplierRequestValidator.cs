using FluentValidation;
using Shared.Constants;
using Shared.DTOs.Suppliers;
using Shared.Enums;

namespace Services.Validation;

public class CreateSupplierRequestValidator : AbstractValidator<CreateSupplierRequest>
{
    public CreateSupplierRequestValidator()
    {
        RuleFor(x => x.SupplierName)
            .NotEmpty().WithMessage("اسم المورد مطلوب.")
            .MaximumLength(150);

        RuleFor(x => x.SupplierType)
            .IsInEnum().WithMessage("من فضلك اختار نوع المورد صح.");

        RuleFor(x => x.OtherSupplierType)
            .NotEmpty().WithMessage("من فضلك اكتب نوع المورد لما تختار «أخرى».")
            .MaximumLength(150)
            .When(x => x.SupplierType == SupplierSpecialization.Other);

        RuleFor(x => x.SuppliedProduct)
            .NotEmpty().WithMessage("المنتج اللي بتوردّه مطلوب.")
            .MaximumLength(300);

        RuleFor(x => x.SupplyDetails)
            .NotEmpty().WithMessage("تفاصيل التوريد مطلوبة.")
            .MaximumLength(4000);

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("العنوان مطلوب.")
            .MaximumLength(300);

        RuleFor(x => x.GoogleMaps)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrWhiteSpace(x.GoogleMaps));

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("رقم الموبايل مطلوب.")
            .Matches(AdvertisementCatalog.EgyptianPhonePattern)
            .WithMessage("من فضلك اكتب رقم موبايل مصري صحيح (مثال: 01012345678).");

        RuleFor(x => x.WhatsApp)
            .NotEmpty().WithMessage("رقم الواتساب مطلوب.")
            .Matches(AdvertisementCatalog.EgyptianPhonePattern)
            .WithMessage("من فضلك اكتب رقم واتساب مصري صحيح (مثال: 01012345678).");

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("من فضلك اكتب بريد إلكتروني صحيح.")
            .MaximumLength(256)
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.Images)
            .Must(images => images.Count <= ImageConstants.MaxImagesPerItem)
            .WithMessage($"مسموح بـ {ImageConstants.MaxImagesPerItem} صور بحد أقصى.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("عنوان الإعلان مطلوب.")
            .MaximumLength(150);

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("وصف الإعلان مطلوب.")
            .MaximumLength(4000);
    }
}
