using FluentValidation;

namespace Services.Validation;

public static class ValidationExtensions
{
    public static IRuleBuilderOptions<T, string> StrongPassword<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("كلمة السر مطلوبة.")
            .MinimumLength(8).WithMessage("كلمة السر لازم تكون 8 حروف على الأقل.")
            .Matches("[A-Z]").WithMessage("كلمة السر لازم يكون فيها حرف إنجليزي كبير على الأقل.")
            .Matches("[a-z]").WithMessage("كلمة السر لازم يكون فيها حرف إنجليزي صغير على الأقل.")
            .Matches("[0-9]").WithMessage("كلمة السر لازم يكون فيها رقم واحد على الأقل.")
            .Matches("[^a-zA-Z0-9]").WithMessage("كلمة السر لازم يكون فيها رمز خاص على الأقل.");
    }
}
