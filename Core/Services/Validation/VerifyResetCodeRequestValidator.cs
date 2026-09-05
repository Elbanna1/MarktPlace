using FluentValidation;
using Shared.DTOs.Auth;

namespace Services.Validation;

public class VerifyResetCodeRequestValidator : AbstractValidator<VerifyResetCodeRequest>
{
    public VerifyResetCodeRequestValidator()
    {
        RuleFor(x => x.Otp)
            .NotEmpty().WithMessage("كود التحقق مطلوب.")
            .Matches("^[0-9]{6}$").WithMessage("كود التحقق لازم يكون 6 أرقام.");
    }
}
