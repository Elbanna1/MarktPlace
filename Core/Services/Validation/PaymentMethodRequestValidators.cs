using FluentValidation;
using Shared.Constants;
using Shared.DTOs.Payments;
using Shared.Enums;

namespace Services.Validation;

public class CreatePaymentMethodRequestValidator : AbstractValidator<CreatePaymentMethodRequest>
{
    public CreatePaymentMethodRequestValidator()
    {
        ApplyRules(this);
    }

    internal static void ApplyRules<T>(AbstractValidator<T> validator) where T : CreatePaymentMethodRequest
    {
        validator.RuleFor(x => x.Name)
            .NotEmpty().WithMessage("اسم طريقة الدفع مطلوب.")
            .MaximumLength(100);

        validator.RuleFor(x => x.ArabicName)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.ArabicName));

        validator.RuleFor(x => x.Type)
            .IsInEnum().WithMessage("نوع طريقة الدفع مش صحيح.");

        validator.RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("رقم المحفظة مطلوب للمحفظة الإلكترونية.")
            .MaximumLength(20)
            .When(x => x.Type == PaymentMethodType.MobileWallet);

        validator.RuleFor(x => x.InstaPayIdentifier)
            .NotEmpty().WithMessage("عنوان إنستاباي مطلوب لطريقة إنستاباي.")
            .MaximumLength(100)
            .When(x => x.Type == PaymentMethodType.InstaPay);

        validator.RuleFor(x => x.BankName)
            .NotEmpty().WithMessage("اسم البنك مطلوب للحساب البنكي.")
            .MaximumLength(150)
            .When(x => x.Type == PaymentMethodType.BankAccount);

        validator.RuleFor(x => x.AccountHolderName)
            .NotEmpty().WithMessage("اسم صاحب الحساب مطلوب للحساب البنكي.")
            .MaximumLength(150)
            .When(x => x.Type == PaymentMethodType.BankAccount);

        validator.RuleFor(x => x.AccountNumber)
            .NotEmpty().WithMessage("رقم الحساب مطلوب للحساب البنكي.")
            .MaximumLength(50)
            .When(x => x.Type == PaymentMethodType.BankAccount);

        validator.RuleFor(x => x.Iban)
            .MaximumLength(50)
            .When(x => !string.IsNullOrWhiteSpace(x.Iban));

        validator.RuleFor(x => x.Instructions)
            .NotEmpty().WithMessage("التعليمات مطلوبة لما تختار نوع «أخرى».")
            .When(x => x.Type == PaymentMethodType.Other);

        validator.RuleFor(x => x.Instructions)
            .MaximumLength(PaymentCatalog.MaxNotesLength)
            .When(x => !string.IsNullOrWhiteSpace(x.Instructions));

        validator.RuleFor(x => x.DisplayOrder)
            .GreaterThanOrEqualTo(0);
    }
}

public class UpdatePaymentMethodRequestValidator : AbstractValidator<UpdatePaymentMethodRequest>
{
    public UpdatePaymentMethodRequestValidator()
    {
        CreatePaymentMethodRequestValidator.ApplyRules(this);
    }
}
