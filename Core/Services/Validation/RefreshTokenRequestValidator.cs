using FluentValidation;
using Shared.DTOs.Auth;

namespace Services.Validation;

public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenRequestValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("رمز التحديث مطلوب.");
    }
}
