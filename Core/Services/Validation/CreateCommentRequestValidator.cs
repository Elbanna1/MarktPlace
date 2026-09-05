using FluentValidation;
using Shared.DTOs.LostFound;

namespace Services.Validation;

public class CreateCommentRequestValidator : AbstractValidator<CreateCommentRequest>
{
    public CreateCommentRequestValidator()
    {
        RuleFor(x => x.Comment)
            .NotEmpty().WithMessage("نص التعليق مطلوب.")
            .MaximumLength(1000);
    }
}
