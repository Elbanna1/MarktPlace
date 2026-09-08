using FluentValidation;
using Shared.Constants;
using Shared.DTOs.Admin;
using Shared.DTOs.Lookups.Forms;
using Shared.Enums;

namespace Services.Validation;

public class UpdateUserStatusRequestValidator : AbstractValidator<UpdateUserStatusRequest>
{
    public UpdateUserStatusRequestValidator()
    {
        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("حالة المستخدم غير صحيحة.")
            .Must(UserAccountCatalog.IsAdminAssignable).WithMessage("حالة المستخدم غير صحيحة.");

        RuleFor(x => x.Reason)
            .MaximumLength(500).WithMessage("لا يمكن أن يتجاوز السبب 500 حرف.");

        RuleFor(x => x.Reason)
            .NotEmpty().WithMessage("سبب الإيقاف أو الحظر مطلوب.")
            .When(x => x.Status != UserAccountStatus.Active);
    }
}

public class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
{
    public CreateCategoryRequestValidator() => ApplyRules(this);

    internal static void ApplyRules<T>(AbstractValidator<T> validator) where T : CreateCategoryRequest
    {
        validator.RuleFor(x => x.NameAr)
            .NotEmpty().WithMessage("الاسم بالعربية مطلوب.")
            .MaximumLength(100).WithMessage("لا يمكن أن يتجاوز الاسم 100 حرف.");

        validator.RuleFor(x => x.NameEn)
            .NotEmpty().WithMessage("الاسم بالإنجليزية مطلوب.")
            .MaximumLength(100).WithMessage("لا يمكن أن يتجاوز الاسم 100 حرف.");

        validator.RuleFor(x => x.Icon)
            .MaximumLength(500)
            .When(x => !string.IsNullOrWhiteSpace(x.Icon));

        validator.RuleFor(x => x.SortOrder)
            .InclusiveBetween(0, 9999).WithMessage("الترتيب يجب أن يكون بين 0 و 9999.");
    }
}

public class UpdateCategoryRequestValidator : AbstractValidator<UpdateCategoryRequest>
{
    public UpdateCategoryRequestValidator() => CreateCategoryRequestValidator.ApplyRules(this);
}

public class CreateSubCategoryRequestValidator : AbstractValidator<CreateSubCategoryRequest>
{
    public CreateSubCategoryRequestValidator() => CreateCategoryRequestValidator.ApplyRules(this);
}

public class UpdateSubCategoryRequestValidator : AbstractValidator<UpdateSubCategoryRequest>
{
    public UpdateSubCategoryRequestValidator()
    {
        CreateCategoryRequestValidator.ApplyRules(this);

        RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage("القسم المستهدف غير صحيح.")
            .When(x => x.CategoryId.HasValue);
    }
}

public class SaveLocationRequestValidator : AbstractValidator<SaveLocationRequest>
{
    public SaveLocationRequestValidator() => ApplyRules(this);

    internal static void ApplyRules<T>(AbstractValidator<T> validator) where T : SaveLocationRequest
    {
        validator.RuleFor(x => x.Name)
            .NotEmpty().WithMessage("الاسم مطلوب.")
            .MaximumLength(150).WithMessage("لا يمكن أن يتجاوز الاسم 150 حرفًا.");

        validator.RuleFor(x => x.SortOrder)
            .InclusiveBetween(0, 9999).WithMessage("الترتيب يجب أن يكون بين 0 و 9999.");
    }
}

public class CreateProjectRequestValidator : AbstractValidator<CreateProjectRequest>
{
    public CreateProjectRequestValidator()
    {
        SaveLocationRequestValidator.ApplyRules(this);

        RuleFor(x => x.CenterId)
            .GreaterThan(0).WithMessage("المركز مطلوب.");
    }
}

public class UpdateProjectRequestValidator : AbstractValidator<UpdateProjectRequest>
{
    public UpdateProjectRequestValidator()
    {
        SaveLocationRequestValidator.ApplyRules(this);

        RuleFor(x => x.CenterId)
            .GreaterThan(0).WithMessage("المركز المستهدف غير صحيح.")
            .When(x => x.CenterId.HasValue);
    }
}

public class UpdateHomeSectionRequestValidator : AbstractValidator<UpdateHomeSectionRequest>
{
    public UpdateHomeSectionRequestValidator()
    {
        RuleFor(x => x.Title).MaximumLength(150).WithMessage("لا يمكن أن يتجاوز العنوان 150 حرفًا.");
        RuleFor(x => x.TitleEn).MaximumLength(150).WithMessage("لا يمكن أن يتجاوز العنوان 150 حرفًا.");
        RuleFor(x => x.Subtitle).MaximumLength(500).WithMessage("لا يمكن أن يتجاوز الوصف 500 حرف.");
        RuleFor(x => x.LinkUrl).MaximumLength(1000);
        RuleFor(x => x.LinkText).MaximumLength(100);

        RuleFor(x => x.SortOrder)
            .InclusiveBetween(0, 9999).WithMessage("الترتيب يجب أن يكون بين 0 و 9999.");

        RuleFor(x => x.ItemCount)
            .InclusiveBetween(1, 50).WithMessage("عدد العناصر يجب أن يكون بين 1 و 50.")
            .When(x => x.ItemCount.HasValue);

        RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage("القسم المختار غير صحيح.")
            .When(x => x.CategoryId.HasValue);
    }
}

public class ReorderHomeSectionsRequestValidator : AbstractValidator<ReorderHomeSectionsRequest>
{
    public ReorderHomeSectionsRequestValidator()
    {
        RuleFor(x => x.SectionIds)
            .NotEmpty().WithMessage("ترتيب الأقسام مطلوب.");
    }
}

public class UpdateSettingsRequestValidator : AbstractValidator<UpdateSettingsRequest>
{
    public UpdateSettingsRequestValidator()
    {
        RuleFor(x => x.SiteName)
            .NotEmpty().WithMessage("اسم الموقع مطلوب.")
            .MaximumLength(150).WithMessage("لا يمكن أن يتجاوز اسم الموقع 150 حرفًا.");

        RuleFor(x => x.SiteNameEn).MaximumLength(150);
        RuleFor(x => x.Description).MaximumLength(1000);
        RuleFor(x => x.Address).MaximumLength(500);
        RuleFor(x => x.MaintenanceMessage).MaximumLength(1000);

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(30)
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));

        RuleFor(x => x.WhatsAppNumber)
            .MaximumLength(30)
            .When(x => !string.IsNullOrWhiteSpace(x.WhatsAppNumber));

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("البريد الإلكتروني غير صحيح.")
            .MaximumLength(256)
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        Url(x => x.FacebookUrl, "رابط فيسبوك غير صحيح.");
        Url(x => x.InstagramUrl, "رابط إنستجرام غير صحيح.");
        Url(x => x.TelegramUrl, "رابط تيليجرام غير صحيح.");
        Url(x => x.TwitterUrl, "الرابط غير صحيح.");
        Url(x => x.YouTubeUrl, "الرابط غير صحيح.");
        Url(x => x.TikTokUrl, "الرابط غير صحيح.");
        Url(x => x.LinkedInUrl, "الرابط غير صحيح.");
    }

    private void Url(
        System.Linq.Expressions.Expression<Func<UpdateSettingsRequest, string?>> selector, string message)
    {
        RuleFor(selector)
            .MaximumLength(500)
            .Must(value =>
                Uri.TryCreate(value, UriKind.Absolute, out var uri) &&
                (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
            .WithMessage(message)
            .When(x => !string.IsNullOrWhiteSpace(selector.Compile()(x)));
    }
}

public class CreateFormFieldRequestValidator : AbstractValidator<CreateFormFieldRequest>
{
    public CreateFormFieldRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("اسم الحقل مطلوب.")
            .MaximumLength(100).WithMessage("لا يمكن أن يتجاوز اسم الحقل 100 حرف.")
            .Matches("^[A-Za-z][A-Za-z0-9_]*$")
            .WithMessage("اسم الحقل يجب أن يبدأ بحرف ويحتوي على حروف وأرقام و _ فقط.");

        RuleFor(x => x.LabelAr)
            .NotEmpty().WithMessage("اسم الحقل بالعربية مطلوب.")
            .MaximumLength(200);

        RuleFor(x => x.LabelEn).MaximumLength(200);

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("نوع الحقل مطلوب.")
            .Must(AdminFormFieldRules.IsSupportedType)
            .WithMessage("نوع الحقل غير مدعوم.");

        RuleFor(x => x.Section).MaximumLength(150);
        RuleFor(x => x.Placeholder).MaximumLength(300);
        RuleFor(x => x.HelpText).MaximumLength(500);
        RuleFor(x => x.Pattern).MaximumLength(500);
        RuleFor(x => x.PatternMessage).MaximumLength(300);

        RuleFor(x => x.SortOrder)
            .InclusiveBetween(0, 9999).WithMessage("الترتيب يجب أن يكون بين 0 و 9999.");

        AdminFormFieldRules.ApplyBoundsRules(this,
            x => x.MinLength, x => x.MaxLength, x => x.MinValue, x => x.MaxValue, x => x.MaxSelections);

        RuleFor(x => x.VisibleWhenValues)
            .NotEmpty().WithMessage("يجب تحديد القيم التي يظهر عندها الحقل.")
            .When(x => !string.IsNullOrWhiteSpace(x.VisibleWhenField));

        RuleFor(x => x.VisibleWhenField)
            .NotEmpty().WithMessage("يجب تحديد الحقل الذي تعتمد عليه شرط الظهور.")
            .When(x => x.VisibleWhenValues is { Count: > 0 });

        RuleFor(x => x.RequiredWhenValues)
            .NotEmpty().WithMessage("يجب تحديد القيم التي يصبح عندها الحقل إجباريًا.")
            .When(x => !string.IsNullOrWhiteSpace(x.RequiredWhenField));

        RuleFor(x => x.RequiredWhenField)
            .NotEmpty().WithMessage("يجب تحديد الحقل الذي تعتمد عليه شرط الإجبارية.")
            .When(x => x.RequiredWhenValues is { Count: > 0 });

        RuleForEach(x => x.Options)
            .SetValidator(new SaveFormOptionRequestValidator())
            .When(x => x.Options is { Count: > 0 });
    }
}

public class UpdateFormFieldRequestValidator : AbstractValidator<UpdateFormFieldRequest>
{
    public UpdateFormFieldRequestValidator()
    {
        RuleFor(x => x.LabelAr).MaximumLength(200);
        RuleFor(x => x.LabelEn).MaximumLength(200);
        RuleFor(x => x.Section).MaximumLength(150);
        RuleFor(x => x.Placeholder).MaximumLength(300);
        RuleFor(x => x.HelpText).MaximumLength(500);
        RuleFor(x => x.Pattern).MaximumLength(500);
        RuleFor(x => x.PatternMessage).MaximumLength(300);

        RuleFor(x => x.SortOrder)
            .InclusiveBetween(0, 9999).WithMessage("الترتيب يجب أن يكون بين 0 و 9999.")
            .When(x => x.SortOrder.HasValue);

        AdminFormFieldRules.ApplyBoundsRules(this,
            x => x.MinLength, x => x.MaxLength, x => x.MinValue, x => x.MaxValue, x => x.MaxSelections);

        RuleFor(x => x.VisibleWhenValues)
            .NotEmpty().WithMessage("يجب تحديد القيم التي يظهر عندها الحقل.")
            .When(x => !x.ClearConditions && !string.IsNullOrWhiteSpace(x.VisibleWhenField));

        RuleFor(x => x.RequiredWhenValues)
            .NotEmpty().WithMessage("يجب تحديد القيم التي يصبح عندها الحقل إجباريًا.")
            .When(x => !x.ClearConditions && !string.IsNullOrWhiteSpace(x.RequiredWhenField));
    }
}

public class SaveFormOptionRequestValidator : AbstractValidator<SaveFormOptionRequest>
{
    public SaveFormOptionRequestValidator()
    {
        RuleFor(x => x.Value)
            .NotEmpty().WithMessage("قيمة الخيار مطلوبة.")
            .MaximumLength(200);

        RuleFor(x => x.Label)
            .NotEmpty().WithMessage("اسم الخيار بالعربية مطلوب.")
            .MaximumLength(200);

        RuleFor(x => x.LabelEn).MaximumLength(200);
        RuleFor(x => x.Group).MaximumLength(150);

        RuleFor(x => x.SortOrder)
            .InclusiveBetween(0, 9999).WithMessage("الترتيب يجب أن يكون بين 0 و 9999.");
    }
}

internal static class AdminFormFieldRules
{
    private static readonly HashSet<string> SupportedTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        FormFieldTypes.Text, FormFieldTypes.TextArea, FormFieldTypes.Number,
        FormFieldTypes.Select, FormFieldTypes.MultiSelect, FormFieldTypes.Radio,
        FormFieldTypes.Checkbox, FormFieldTypes.Date,
        FormFieldTypes.Image, FormFieldTypes.Video, FormFieldTypes.File
    };

    public static bool IsSupportedType(string? type) =>
        !string.IsNullOrWhiteSpace(type) && SupportedTypes.Contains(type);

    public static void ApplyBoundsRules<T>(
        AbstractValidator<T> validator,
        Func<T, int?> minLength, Func<T, int?> maxLength,
        Func<T, decimal?> minValue, Func<T, decimal?> maxValue,
        Func<T, int?> maxSelections)
    {
        validator.RuleFor(x => minLength(x))
            .GreaterThanOrEqualTo(0).WithMessage("أقل عدد للحروف لا يمكن أن يكون سالبًا.")
            .When(x => minLength(x).HasValue);

        validator.RuleFor(x => maxLength(x))
            .GreaterThan(0).WithMessage("أكبر عدد للحروف يجب أن يكون أكبر من صفر.")
            .When(x => maxLength(x).HasValue);

        validator.RuleFor(x => maxSelections(x))
            .GreaterThan(0).WithMessage("أكبر عدد للاختيارات يجب أن يكون أكبر من صفر.")
            .When(x => maxSelections(x).HasValue);

        validator.RuleFor(x => x)
            .Must(x => minLength(x)!.Value <= maxLength(x)!.Value)
            .WithMessage("أقل عدد للحروف لا يمكن أن يتجاوز أكبر عدد.")
            .When(x => minLength(x).HasValue && maxLength(x).HasValue);

        validator.RuleFor(x => x)
            .Must(x => minValue(x)!.Value <= maxValue(x)!.Value)
            .WithMessage("أقل قيمة لا يمكن أن تتجاوز أكبر قيمة.")
            .When(x => minValue(x).HasValue && maxValue(x).HasValue);
    }
}

public class RejectPaymentRequestValidator : AbstractValidator<RejectPaymentRequest>
{
    public RejectPaymentRequestValidator()
    {
        RuleFor(x => x.Reason)
            .NotEmpty().WithMessage("سبب الرفض مطلوب.")
            .MaximumLength(PaymentCatalog.MaxRejectReasonLength)
            .WithMessage("لا يمكن أن يتجاوز سبب الرفض 500 حرف.");
    }
}

public class RefundPaymentRequestValidator : AbstractValidator<RefundPaymentRequest>
{
    public RefundPaymentRequestValidator()
    {
        RuleFor(x => x.Reason)
            .NotEmpty().WithMessage("سبب الاسترداد مطلوب.")
            .MaximumLength(PaymentCatalog.MaxRejectReasonLength)
            .WithMessage("لا يمكن أن يتجاوز السبب 500 حرف.");
    }
}

public class AdminReportDecisionRequestValidator : AbstractValidator<AdminReportDecisionRequest>
{
    public AdminReportDecisionRequestValidator()
    {
        RuleFor(x => x.Note)
            .MaximumLength(1000).WithMessage("لا يمكن أن تتجاوز الملاحظات 1000 حرف.");
    }
}

public class AdminReportActionRequestValidator : AbstractValidator<AdminReportActionRequest>
{
    public AdminReportActionRequestValidator()
    {
        RuleFor(x => x.Note)
            .MaximumLength(1000).WithMessage("لا يمكن أن تتجاوز الملاحظات 1000 حرف.");

        RuleFor(x => x.Action)
            .IsInEnum().WithMessage("الإجراء غير صحيح.");
    }
}

public class ApproveListingRequestValidator : AbstractValidator<ApproveListingRequest>
{
    public ApproveListingRequestValidator()
    {
        RuleFor(x => x.Notes)
            .MaximumLength(ModerationCatalog.MaxNotesLength)
            .WithMessage("لا يمكن أن تتجاوز الملاحظات 500 حرف.");
    }
}

public class SuspendListingRequestValidator : AbstractValidator<SuspendListingRequest>
{
    public SuspendListingRequestValidator()
    {
        RuleFor(x => x.Notes)
            .MaximumLength(ModerationCatalog.MaxNotesLength)
            .WithMessage("لا يمكن أن تتجاوز الملاحظات 500 حرف.");
    }
}

public class RejectListingRequestValidator : AbstractValidator<RejectListingRequest>
{
    public RejectListingRequestValidator()
    {
        RuleFor(x => x.Reason)
            .IsInEnum().WithMessage("سبب الرفض غير صحيح.");

        RuleFor(x => x.Notes)
            .MaximumLength(ModerationCatalog.MaxNotesLength)
            .WithMessage("لا يمكن أن تتجاوز الملاحظات 500 حرف.");

        RuleFor(x => x.Notes)
            .NotEmpty().WithMessage("يجب كتابة سبب الرفض عند اختيار \"سبب آخر\".")
            .When(x => ModerationCatalog.RequiresNotes(x.Reason));
    }
}

public class UpdateActiveStatusRequestValidator : AbstractValidator<UpdateActiveStatusRequest>
{
    public UpdateActiveStatusRequestValidator()
    {
    }
}
