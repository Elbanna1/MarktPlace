using FluentValidation;
using Shared.Constants;
using Shared.DTOs.Auth;

namespace Services.Validation;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("الاسم الأول مطلوب.")
            .MaximumLength(50)
            .Must(AccountNameRules.IsValidPersonName)
                .WithMessage(AccountNameRules.PersonNameMessage);

        RuleFor(x => x.SecondName)
            .NotEmpty().WithMessage("الاسم التاني مطلوب.")
            .MaximumLength(50)
            .Must(AccountNameRules.IsValidPersonName)
                .WithMessage(AccountNameRules.PersonNameMessage);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("البريد الإلكتروني مطلوب.")
            .EmailAddress().WithMessage("من فضلك اكتب بريد إلكتروني صحيح.");

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("اسم المستخدم مطلوب.")
            .Must(username => AccountNameRules.NormalizeWhitespace(username).Length >= 3)
                .WithMessage("اسم المستخدم لازم يكون 3 حروف على الأقل.")
            .MaximumLength(50)
            .Must(AccountNameRules.IsValidUsername)
                .WithMessage(AccountNameRules.UsernameMessage);

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("رقم الموبايل مطلوب.")
            .Matches(@"^01[0-2,5]{1}[0-9]{8}$")
            .WithMessage("من فضلك اكتب رقم موبايل مصري صحيح (مثال: 01012345678).");

        RuleFor(x => x.Governorate)
            .NotEmpty().WithMessage("المحافظة مطلوبة.")
            .Must(LocationConstants.IsValidGovernorate)
            .WithMessage($"المحافظة لازم تكون '{LocationConstants.Governorate}'.");

        RuleFor(x => x.Center)
            .NotEmpty().WithMessage("المركز مطلوب.")
            .Must(LocationConstants.IsValidCenter)
            .WithMessage($"المركز لازم يكون واحد من: {string.Join(", ", LocationConstants.Centers)}.");

        RuleFor(x => x.Password).StrongPassword();

        RuleFor(x => x.ReferralCode)
            .MaximumLength(ReferralCatalog.MaxCodeLength)
            .WithMessage($"كود الدعوة ما ينفعش يزيد عن {ReferralCatalog.MaxCodeLength} حرف.")
            .Matches("^[A-Za-z0-9]+$")
            .WithMessage("كود الدعوة يقبل حروف وأرقام بس.")
            .When(x => !string.IsNullOrWhiteSpace(x.ReferralCode));

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("تأكيد كلمة السر مطلوب.")
            .Equal(x => x.Password).WithMessage("كلمة السر وتأكيدها مش زي بعض.");
    }
}
