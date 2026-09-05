using FluentValidation;
using Shared.Constants;
using Shared.DTOs.Profile;

namespace Services.Validation;

public class UpdateProfileRequestValidator : AbstractValidator<UpdateProfileRequest>
{
    public UpdateProfileRequestValidator()
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

        RuleFor(x => x.Center)
            .NotEmpty().WithMessage("المركز مطلوب.")
            .Must(LocationConstants.IsValidCenter)
            .WithMessage($"المركز لازم يكون واحد من: {string.Join(", ", LocationConstants.Centers)}.");
    }
}
