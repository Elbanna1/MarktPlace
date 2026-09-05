using FluentValidation;
using Shared.DTOs.Auth;

namespace Services.Validation;

public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
{
    public ResetPasswordRequestValidator()
    {
        RuleFor(x => x.NewPassword).StrongPassword();

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("تأكيد كلمة السر مطلوب.")
            .Equal(x => x.NewPassword).WithMessage("كلمة السر وتأكيدها مش زي بعض.");
    }
}
