using FluentValidation;
using Shared.Constants;
using Shared.DTOs.Payments;

namespace Services.Validation;

public class SubmitPaymentRequestValidator : AbstractValidator<SubmitPaymentRequest>
{
    public SubmitPaymentRequestValidator()
    {
        RuleFor(x => x.PaymentMethodId)
            .GreaterThan(0).WithMessage("طريقة الدفع مطلوبة.");

        RuleFor(x => x.Amount)
            .GreaterThanOrEqualTo(PaymentCatalog.MinAmount)
            .WithMessage($"المبلغ لازم يكون {PaymentCatalog.MinAmount} {PaymentCatalog.DefaultCurrency} على الأقل.")
            .LessThanOrEqualTo(PaymentCatalog.MaxAmount)
            .WithMessage($"المبلغ ما ينفعش يزيد عن {PaymentCatalog.MaxAmount} {PaymentCatalog.DefaultCurrency}.")
            .Must(amount => decimal.Round(amount, 2) == amount)
            .WithMessage("المبلغ ما ينفعش يزيد عن رقمين بعد العلامة العشرية.");

        RuleFor(x => x.Screenshot)
            .NotNull().WithMessage("صورة إيصال الدفع مطلوبة.");

        RuleFor(x => x.Screenshot!.Length)
            .GreaterThan(0).WithMessage("صورة إيصال الدفع فاضية.")
            .When(x => x.Screenshot is not null);

        RuleFor(x => x.Notes)
            .MaximumLength(PaymentCatalog.MaxNotesLength)
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));
    }
}
