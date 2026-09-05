using FluentValidation;
using Shared.Constants;
using Shared.DTOs.LostFound;

namespace Services.Validation;

public class UpdateLostFoundPostRequestValidator : AbstractValidator<UpdateLostFoundPostRequest>
{
    public UpdateLostFoundPostRequestValidator()
    {
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

        RuleFor(x => x.Center)
            .Must(LocationConstants.IsValidCenter)
            .When(x => !string.IsNullOrWhiteSpace(x.Center))
            .WithMessage($"المركز لازم يكون واحد من: {string.Join(", ", LocationConstants.Centers)}.");
    }
}
