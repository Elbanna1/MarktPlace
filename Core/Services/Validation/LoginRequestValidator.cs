using FluentValidation;
using Shared.DTOs.Auth;

namespace Services.Validation;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("اسم المستخدم مطلوب.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("كلمة السر مطلوبة.");
    }
}
