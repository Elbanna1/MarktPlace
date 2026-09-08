using FluentValidation;
using Shared.Constants;
using Shared.DTOs.Account;

namespace Services.Validation;

public class DeactivateAccountRequestValidator : AbstractValidator<DeactivateAccountRequest>
{
    public DeactivateAccountRequestValidator()
    {
        RuleFor(x => x.Confirm)
            .Equal(true).WithMessage(UserMessages.Account.ConfirmationRequired);

        RuleFor(x => x.Reason)
            .MaximumLength(DeactivateAccountRequest.MaxReasonLength)
            .WithMessage($"السبب لازم يكون {DeactivateAccountRequest.MaxReasonLength} حرف على الأكثر.");
    }
}
