using FluentValidation;
using Shared.Constants;
using Shared.DTOs.Admin;
using Shared.Enums;

namespace Services.Validation;

public class ConfirmAdminRequestValidator : AbstractValidator<ConfirmAdminRequest>
{
    public ConfirmAdminRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("اختار المستخدم الأول.");

        RuleFor(x => x.Pages)
            .NotNull().WithMessage("قائمة الصفحات مطلوبة.");

        RuleForEach(x => x.Pages).SetValidator(new AdminPageAssignmentRequestValidator());
    }
}

public class UpdateAdminRequestValidator : AbstractValidator<UpdateAdminRequest>
{
    public UpdateAdminRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("الاسم الأول مطلوب.")
            .MaximumLength(50)
            .Must(AccountNameRules.IsValidPersonName).WithMessage(AccountNameRules.PersonNameMessage);

        RuleFor(x => x.SecondName)
            .NotEmpty().WithMessage("الاسم التاني مطلوب.")
            .MaximumLength(50)
            .Must(AccountNameRules.IsValidPersonName).WithMessage(AccountNameRules.PersonNameMessage);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("البريد الإلكتروني مطلوب.")
            .EmailAddress().WithMessage("من فضلك اكتب بريد إلكتروني صحيح.");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("رقم الموبايل مطلوب.")
            .Matches(@"^01[0-2,5]{1}[0-9]{8}$")
            .WithMessage("من فضلك اكتب رقم موبايل مصري صحيح (مثال: 01012345678).");

        RuleFor(x => x.Password!)
            .StrongPassword()
            .When(x => !string.IsNullOrWhiteSpace(x.Password));
    }
}

public class UpdateAdminStatusRequestValidator : AbstractValidator<UpdateAdminStatusRequest>
{
    public UpdateAdminStatusRequestValidator()
    {
        RuleFor(x => x.Reason)
            .MaximumLength(500).WithMessage("لا يمكن أن يتجاوز السبب 500 حرف.");

        RuleFor(x => x.Reason)
            .NotEmpty().WithMessage("سبب الإيقاف مطلوب.")
            .When(x => !x.IsActive);
    }
}

public class UpdateAdminPermissionsRequestValidator : AbstractValidator<UpdateAdminPermissionsRequest>
{
    public UpdateAdminPermissionsRequestValidator()
    {
        RuleFor(x => x.Pages)
            .NotNull().WithMessage("قائمة الصفحات مطلوبة.");

        RuleForEach(x => x.Pages).SetValidator(new AdminPageAssignmentRequestValidator());
    }
}

public class AdminPageAssignmentRequestValidator : AbstractValidator<AdminPageAssignmentRequest>
{
    public AdminPageAssignmentRequestValidator()
    {
        RuleFor(x => x.PageKey)
            .NotEmpty().WithMessage("مفتاح الصفحة مطلوب.")
            .Must(AdminPageCatalog.Exists).WithMessage("الصفحة دي مش موجودة في لوحة التحكم.");

        RuleFor(x => x.Permissions)
            .NotNull().WithMessage("قائمة الصلاحيات مطلوبة.");

        RuleFor(x => x)
            .Must(assignment => assignment.Permissions is null || assignment.Permissions.All(name =>
                Enum.TryParse<AdminPermission>(name?.Trim(), ignoreCase: true, out var permission) &&
                AdminPageCatalog.Allows(assignment.PageKey, permission)))
            .WithMessage("فيه صلاحية مش متاحة على الصفحة دي.")
            .When(x => AdminPageCatalog.Exists(x.PageKey));
    }
}
