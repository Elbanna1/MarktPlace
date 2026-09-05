using FluentValidation;
using Shared.Constants;
using Shared.DTOs.WholesaleTraders;
using Shared.Enums;

namespace Services.Validation;

public class CreateWholesaleTraderRequestValidator : AbstractValidator<CreateWholesaleTraderRequest>
{
    public CreateWholesaleTraderRequestValidator()
    {
        RuleFor(x => x.TraderName)
            .NotEmpty().WithMessage("اسم التاجر مطلوب.")
            .MaximumLength(150);

        RuleFor(x => x.TradeType)
            .IsInEnum().WithMessage("من فضلك اختار نوع التجارة صح.");

        RuleFor(x => x.OtherTradeType)
            .NotEmpty().WithMessage("من فضلك اكتب نوع التجارة لما تختار «أخرى».")
            .MaximumLength(150)
            .When(x => x.TradeType == WholesaleTradeType.Other);

        RuleFor(x => x.ProductsName)
            .NotEmpty().WithMessage("اسم المنتج مطلوب.")
            .MaximumLength(300);

        RuleFor(x => x.ProductDetails)
            .NotEmpty().WithMessage("تفاصيل المنتج مطلوبة.")
            .MaximumLength(4000);

        RuleFor(x => x.SaleType)
            .IsInEnum().WithMessage("من فضلك اختار نوع البيع صح.");

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
            .NotEmpty().WithMessage("لازم ترفع صورة واحدة على الأقل.");

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
