using FluentValidation;
using Shared.DTOs.Auth;

namespace Services.Validation;

public class ForgotPasswordRequestValidator : AbstractValidator<ForgotPasswordRequest>
{
    public ForgotPasswordRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("البريد الإلكتروني مطلوب.")
            .EmailAddress().WithMessage("من فضلك اكتب بريد إلكتروني صحيح.");
    }
}
