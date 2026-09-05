using FluentValidation;
using Shared.Constants;
using Shared.DTOs.Charity;
using Shared.Enums;

namespace Services.Validation;

public class CreateRescueRequestValidator : AbstractValidator<CreateRescueRequest>
{
    public CreateRescueRequestValidator()
    {
        this.AddCharitySharedRules(locationRequired: true);
        this.AddRescueRules();
    }
}

public class UpdateRescueRequestValidator : AbstractValidator<UpdateRescueRequest>
{
    public UpdateRescueRequestValidator()
    {
        this.AddCharitySharedRules(locationRequired: true);
        this.AddRescueRules();
    }
}

internal static class RescueValidationRules
{
    public static void AddRescueRules<T>(this AbstractValidator<T> validator)
        where T : CreateRescueRequest
    {
        validator.RuleFor(x => x.RescuerName)
            .NotEmpty().WithMessage("اسم المستغيث مطلوب.")
            .MaximumLength(150);

        validator.RuleFor(x => x.Address)
            .NotEmpty().WithMessage("العنوان / مكان الاستغاثة مطلوب.")
            .MaximumLength(300);

        validator.RuleFor(x => x.Details)
            .NotEmpty().WithMessage("تفاصيل الاستغاثة مطلوبة.")
            .MaximumLength(4000);
    }
}

public class CreateBloodRequestRequestValidator : AbstractValidator<CreateBloodRequestRequest>
{
    public CreateBloodRequestRequestValidator()
    {
        this.AddCharitySharedRules(locationRequired: true);
        this.AddBloodRequestRules();
    }
}

public class UpdateBloodRequestRequestValidator : AbstractValidator<UpdateBloodRequestRequest>
{
    public UpdateBloodRequestRequestValidator()
    {
        this.AddCharitySharedRules(locationRequired: true);
        this.AddBloodRequestRules();
    }
}

internal static class BloodRequestValidationRules
{
    public static void AddBloodRequestRules<T>(this AbstractValidator<T> validator)
        where T : CreateBloodRequestRequest
    {
        validator.RuleFor(x => x.RequesterName)
            .NotEmpty().WithMessage("اسم طالب الدم مطلوب.")
            .MaximumLength(150);

        validator.RuleFor(x => x.BloodGroup)
            .NotNull().WithMessage("فصيلة الدم المطلوبة مطلوبة.")
            .IsInEnum().WithMessage(
                $"من فضلك اختار فصيلة دم صحيحة: {string.Join("، ", CharityCatalog.BloodGroups.Select(g => g.Name))}.");

        validator.AddCenterRule(x => x.Center, required: true);

        validator.RuleFor(x => x.HospitalName)
            .NotEmpty().WithMessage("اسم المستشفى / المكان مطلوب.")
            .MaximumLength(200);

        validator.RuleFor(x => x.Address)
            .NotEmpty().WithMessage("العنوان / مكان التبرع مطلوب.")
            .MaximumLength(300);

        validator.RuleFor(x => x.Details)
            .NotEmpty().WithMessage("تفاصيل الطلب مطلوبة.")
            .MaximumLength(4000);
    }
}

public class CreateAskConsultRequestValidator : AbstractValidator<CreateAskConsultRequest>
{
    public CreateAskConsultRequestValidator()
    {
        this.AddCharitySharedRules(locationRequired: false, acceptsLocation: false);
        this.AddAskConsultRules();
    }
}

public class UpdateAskConsultRequestValidator : AbstractValidator<UpdateAskConsultRequest>
{
    public UpdateAskConsultRequestValidator()
    {
        this.AddCharitySharedRules(locationRequired: false, acceptsLocation: false);
        this.AddAskConsultRules();
    }
}

internal static class AskConsultValidationRules
{
    public static void AddAskConsultRules<T>(this AbstractValidator<T> validator)
        where T : CreateAskConsultRequest
    {
        validator.RuleFor(x => x.Category)
            .NotNull().WithMessage("مجال السؤال مطلوب.")
            .IsInEnum().WithMessage("من فضلك اختار مجال سؤال صحيح.");

        validator.RuleFor(x => x.OtherCategory)
            .NotEmpty().WithMessage("من فضلك اكتب المجال عند اختيار 'أخرى'.")
            .MaximumLength(150)
            .When(x => x.Category == AskConsultCategory.Other);

        validator.RuleFor(x => x.AskerName)
            .NotEmpty().WithMessage("اسم صاحب السؤال مطلوب.")
            .MaximumLength(150);

        validator.RuleFor(x => x.Title)
            .NotEmpty().WithMessage("عنوان السؤال مطلوب.")
            .MaximumLength(150);

        validator.RuleFor(x => x.Question)
            .NotEmpty().WithMessage("السؤال / الاستشارة مطلوب.")
            .MaximumLength(4000);

        validator.RuleFor(x => x.Images)
            .Must(images => images.Count <= CharityCatalog.MaxAskConsultImages)
            .WithMessage($"يمكن رفع {CharityCatalog.MaxAskConsultImages} صور بحد أقصى.");
    }
}

public class CreateAskConsultCommentRequestValidator : AbstractValidator<CreateAskConsultCommentRequest>
{
    public CreateAskConsultCommentRequestValidator()
    {
        RuleFor(x => x.Comment)
            .NotEmpty().WithMessage("نص التعليق مطلوب.")
            .MaximumLength(CharityCatalog.MaxCommentLength);
    }
}
