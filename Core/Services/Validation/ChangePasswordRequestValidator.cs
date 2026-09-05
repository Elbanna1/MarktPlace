using FluentValidation;
using Shared.DTOs.Profile;

namespace Services.Validation;

public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator()
    {
        RuleFor(x => x.OldPassword)
            .NotEmpty().WithMessage("كلمة السر القديمة مطلوبة.");

        RuleFor(x => x.NewPassword).StrongPassword();

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("تأكيد كلمة السر مطلوب.")
            .Equal(x => x.NewPassword).WithMessage("كلمة السر وتأكيدها مش زي بعض.");

        RuleFor(x => x)
            .Must(x => x.OldPassword != x.NewPassword)
            .WithMessage("كلمة السر الجديدة لازم تكون مختلفة عن القديمة.")
            .WithName(nameof(ChangePasswordRequest.NewPassword));
    }
}
