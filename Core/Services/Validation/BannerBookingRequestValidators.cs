using FluentValidation;
using Services.BannerBookings;
using Shared.Constants;
using Shared.DTOs.BannerBookings;
using Shared.Enums;

namespace Services.Validation;

public class CreateBannerBookingRequestValidator : AbstractValidator<CreateBannerBookingRequest>
{
    public CreateBannerBookingRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("عنوان الإعلان مطلوب.")
            .MaximumLength(BannerBookingCatalog.MaxTitleLength);

        RuleFor(x => x.Description)
            .MaximumLength(BannerBookingCatalog.MaxDescriptionLength)
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.ButtonText)
            .NotEmpty().WithMessage("نص الزر مطلوب.")
            .MaximumLength(BannerBookingCatalog.MaxButtonTextLength);

        RuleFor(x => x.TargetUrl)
            .NotEmpty().WithMessage("رابط الإعلان مطلوب.")
            .MaximumLength(BannerBookingCatalog.MaxTargetUrlLength)
            .Must(BannerTargetUrl.IsValid)
            .WithMessage("رابط الإعلان غير صحيح. استخدم رابطًا خارجيًا يبدأ بـ https:// أو مسارًا داخل التطبيق يبدأ بـ /.");

        RuleFor(x => x.Location)
            .IsInEnum().WithMessage("مكان ظهور الإعلان غير صحيح.");

        RuleFor(x => x.SlotNumber)
            .GreaterThan(0).WithMessage("رقم المكان غير صحيح.")
            .When(x => x.SlotNumber.HasValue);

        RuleFor(x => x.CategoryId)
            .NotNull().WithMessage("يجب اختيار القسم.")
            .GreaterThan(0).WithMessage("القسم غير صحيح.")
            .When(x => x.Location == BannerLocation.SubCategoryBanner);

        RuleFor(x => x.SubCategoryId)
            .NotNull().WithMessage("يجب اختيار القسم الفرعي.")
            .GreaterThan(0).WithMessage("القسم الفرعي غير صحيح.")
            .When(x => x.Location == BannerLocation.SubCategoryBanner);

        RuleFor(x => x.AdvertiserName)
            .NotEmpty().WithMessage("اسم المعلن / الشركة مطلوب.")
            .MaximumLength(BannerBookingCatalog.MaxAdvertiserNameLength);

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("رقم الهاتف مطلوب.")
            .MaximumLength(BannerBookingCatalog.MaxPhoneLength);

        RuleFor(x => x.WhatsAppNumber)
            .MaximumLength(BannerBookingCatalog.MaxPhoneLength)
            .When(x => !string.IsNullOrWhiteSpace(x.WhatsAppNumber));

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("البريد الإلكتروني غير صحيح.")
            .MaximumLength(BannerBookingCatalog.MaxEmailLength)
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.PaymentMethodId)
            .GreaterThan(0).WithMessage("يجب اختيار طريقة الدفع.");

        RuleFor(x => x.PaymentProof)
            .NotNull().WithMessage("صورة إثبات الدفع مطلوبة.");

        RuleFor(x => x.PaymentProof!.Length)
            .GreaterThan(0).WithMessage("صورة إثبات الدفع فارغة.")
            .When(x => x.PaymentProof is not null);

        RuleFor(x => x.DesktopImage)
            .NotNull().WithMessage("صورة البانر — Desktop مطلوبة.");

        RuleFor(x => x.DesktopImage!.Length)
            .GreaterThan(0).WithMessage("صورة البانر — Desktop فارغة.")
            .When(x => x.DesktopImage is not null);

        RuleFor(x => x.MobileImage)
            .NotNull().WithMessage("صورة البانر — Mobile مطلوبة.");

        RuleFor(x => x.MobileImage!.Length)
            .GreaterThan(0).WithMessage("صورة البانر — Mobile فارغة.")
            .When(x => x.MobileImage is not null);

        RuleFor(x => x.ConfirmationAccepted)
            .Equal(true).WithMessage(BannerBookingCatalog.ConfirmationStatement);
    }
}

public class BannerImagePreviewRequestValidator : AbstractValidator<BannerImagePreviewRequest>
{
    public BannerImagePreviewRequestValidator()
    {
        RuleFor(x => x.Location)
            .IsInEnum().WithMessage("مكان ظهور الإعلان غير صحيح.");

        RuleFor(x => x)
            .Must(request => request.DesktopImage is not null || request.MobileImage is not null)
            .WithMessage("ارفع صورة Desktop أو Mobile للمعاينة.");
    }
}

public class RejectBannerBookingRequestValidator : AbstractValidator<RejectBannerBookingRequest>
{
    public RejectBannerBookingRequestValidator()
    {
        RuleFor(x => x.Reason)
            .IsInEnum().WithMessage("سبب الرفض غير صحيح.");

        RuleFor(x => x.Notes)
            .MaximumLength(BannerBookingCatalog.MaxRejectionNotesLength);

        RuleFor(x => x.Notes)
            .NotEmpty().WithMessage("يجب كتابة سبب الرفض عند اختيار 'سبب آخر'.")
            .When(x => x.Reason == BannerRejectionReason.Other);
    }
}

public class RejectBannerPaymentRequestValidator : AbstractValidator<RejectBannerPaymentRequest>
{
    public RejectBannerPaymentRequestValidator()
    {
        RuleFor(x => x.Notes)
            .MaximumLength(BannerBookingCatalog.MaxRejectionNotesLength);
    }
}

public class UpdateBannerPlacementRequestValidator : AbstractValidator<UpdateBannerPlacementRequest>
{
    public UpdateBannerPlacementRequestValidator()
    {
        RuleFor(x => x.Price)
            .InclusiveBetween(BannerBookingCatalog.MinPrice, BannerBookingCatalog.MaxPrice)
            .WithMessage($"السعر يجب أن يكون بين {BannerBookingCatalog.MinPrice} و {BannerBookingCatalog.MaxPrice}.")
            .Must(price => decimal.Round(price, 2) == price)
            .WithMessage("السعر يجب ألا يزيد عن رقمين عشريين.");

        RuleFor(x => x.MaxSlots)
            .InclusiveBetween(BannerBookingCatalog.MinSlots, BannerBookingCatalog.MaxSlots);

        RuleFor(x => x.DesktopWidth)
            .InclusiveBetween(BannerBookingCatalog.MinImageEdge, BannerBookingCatalog.MaxImageEdge);

        RuleFor(x => x.DesktopHeight)
            .InclusiveBetween(BannerBookingCatalog.MinImageEdge, BannerBookingCatalog.MaxImageEdge);

        RuleFor(x => x.MobileWidth)
            .InclusiveBetween(BannerBookingCatalog.MinImageEdge, BannerBookingCatalog.MaxImageEdge);

        RuleFor(x => x.MobileHeight)
            .InclusiveBetween(BannerBookingCatalog.MinImageEdge, BannerBookingCatalog.MaxImageEdge);

        RuleFor(x => x.MaxImageSizeMegabytes)
            .InclusiveBetween(
                BannerImageRules.ToMegabytes(BannerBookingCatalog.MinConfigurableImageSizeBytes),
                BannerImageRules.ToMegabytes(BannerBookingCatalog.MaxConfigurableImageSizeBytes));

        RuleForEach(x => x.AllowedFormats)
            .NotEmpty().WithMessage("الصيغة لا يمكن أن تكون فارغة.")
            .When(x => x.AllowedFormats is not null);

        RuleFor(x => x.DisplayOrder)
            .GreaterThanOrEqualTo(0);
    }
}
