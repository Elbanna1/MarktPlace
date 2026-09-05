using FluentValidation;
using Shared.Constants;
using Shared.DTOs.LostFound;
using Shared.Enums;

namespace Services.Validation;

public class CreateLostFoundPostRequestValidator : AbstractValidator<CreateLostFoundPostRequest>
{
    public CreateLostFoundPostRequestValidator()
    {
        RuleFor(x => x.PostType)
            .IsInEnum().WithMessage("من فضلك اختار نوع المنشور (ضايع مني أو لقيت).");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("الاسم مطلوب.")
            .MaximumLength(150);

        RuleFor(x => x.ItemName)
            .NotEmpty().WithMessage("عنوان الإعلان مطلوب.")
            .MaximumLength(150);

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("الوصف مطلوب.")
            .MaximumLength(4000);

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("رقم الموبايل مطلوب.")
            .Matches(@"^01[0-2,5]{1}[0-9]{8}$")
            .WithMessage("من فضلك اكتب رقم موبايل مصري صحيح (مثال: 01012345678).");

        RuleFor(x => x.Governorate)
            .Must(LocationConstants.IsValidGovernorate)
            .When(x => !string.IsNullOrWhiteSpace(x.Governorate))
            .WithMessage($"المحافظة لازم تكون '{LocationConstants.Governorate}'.");

        RuleFor(x => x.Center)
            .NotEmpty().WithMessage("المركز مطلوب.")
            .Must(LocationConstants.IsValidCenter)
            .WithMessage($"المركز لازم يكون واحد من: {string.Join(", ", LocationConstants.Centers)}.");

        RuleFor(x => x.LostDate)
            .NotNull().WithMessage("تاريخ الضياع مطلوب في منشور «ضايع مني».")
            .When(x => x.PostType == PostType.Lost);

        RuleFor(x => x.FoundDate)
            .NotNull().WithMessage("تاريخ اللقطة مطلوب في منشور «لقيت».")
            .When(x => x.PostType == PostType.Found);
    }
}
