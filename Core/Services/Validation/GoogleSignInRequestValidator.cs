using FluentValidation;
using Shared.Constants;
using Shared.DTOs.Auth;

namespace Services.Validation;

public class GoogleSignInRequestValidator : AbstractValidator<GoogleSignInRequest>
{
    private const int MaxCredentialLength = 8192;

    public GoogleSignInRequestValidator()
    {
        RuleFor(x => x)
            .Must(request =>
                !string.IsNullOrWhiteSpace(request.IdToken) || !string.IsNullOrWhiteSpace(request.Code))
            .WithMessage(UserMessages.Auth.GoogleCredentialRequired);

        RuleFor(x => x.IdToken)
            .MaximumLength(MaxCredentialLength)
            .WithMessage(UserMessages.Auth.GoogleCredentialInvalid)
            .When(x => !string.IsNullOrWhiteSpace(x.IdToken));

        RuleFor(x => x.Code)
            .MaximumLength(MaxCredentialLength)
            .WithMessage(UserMessages.Auth.GoogleCredentialInvalid)
            .When(x => !string.IsNullOrWhiteSpace(x.Code));

        RuleFor(x => x.ReferralCode)
            .MaximumLength(ReferralCatalog.MaxCodeLength)
            .WithMessage($"كود الدعوة ما ينفعش يزيد عن {ReferralCatalog.MaxCodeLength} حرف.")
            .Matches("^[A-Za-z0-9]+$")
            .WithMessage("كود الدعوة يقبل حروف وأرقام بس.")
            .When(x => !string.IsNullOrWhiteSpace(x.ReferralCode));

        RuleFor(x => x.Center)
            .Must(LocationConstants.IsValidCenter)
            .WithMessage($"المركز لازم يكون واحد من: {string.Join(", ", LocationConstants.Centers)}.")
            .When(x => !string.IsNullOrWhiteSpace(x.Center));
    }
}
