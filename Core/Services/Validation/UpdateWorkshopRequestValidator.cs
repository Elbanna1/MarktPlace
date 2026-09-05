using FluentValidation;
using Shared.Constants;
using Shared.DTOs.Workshops;
using Shared.Enums;

namespace Services.Validation;

public class UpdateWorkshopRequestValidator : AbstractValidator<UpdateWorkshopRequest>
{
    public UpdateWorkshopRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("اسم الورشة مطلوب.")
            .MaximumLength(150);

        RuleFor(x => x.WorkshopType)
            .IsInEnum().WithMessage("من فضلك اختار نوع الورشة صح.");

        RuleFor(x => x.OtherWorkshopType)
            .NotEmpty().WithMessage("من فضلك اكتب نوع الورشة لما تختار «أخرى».")
            .MaximumLength(150)
            .When(x => x.WorkshopType == WorkshopType.Other);

        RuleFor(x => x.Center)
            .NotEmpty().WithMessage("المركز مطلوب.")
            .Must(LocationConstants.IsValidCenter)
            .WithMessage($"المركز لازم يكون واحد من: {string.Join(", ", LocationConstants.Centers)}.");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("العنوان مطلوب.")
            .MaximumLength(300);

        RuleFor(x => x.GoogleMapsUrl)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrWhiteSpace(x.GoogleMapsUrl));

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("رقم الموبايل مطلوب.")
            .Matches(@"^01[0-2,5]{1}[0-9]{8}$")
            .WithMessage("من فضلك اكتب رقم موبايل مصري صحيح (مثال: 01012345678).");

        RuleFor(x => x.WhatsApp)
            .NotEmpty().WithMessage("رقم الواتساب مطلوب.")
            .Matches(@"^01[0-2,5]{1}[0-9]{8}$")
            .WithMessage("من فضلك اكتب رقم واتساب مصري صحيح (مثال: 01012345678).");

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("من فضلك اكتب بريد إلكتروني صحيح.")
            .MaximumLength(256)
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.AdTitle)
            .NotEmpty().WithMessage("عنوان الإعلان مطلوب.")
            .MaximumLength(150);

        RuleFor(x => x.AdDescription)
            .NotEmpty().WithMessage("وصف الإعلان مطلوب.")
            .MaximumLength(4000);
    }
}
