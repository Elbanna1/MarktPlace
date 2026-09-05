using FluentValidation;
using Shared.Constants;
using Shared.DTOs.JobOpportunities;
using Shared.Enums;
using static Services.Validation.CreateJobRequestRequestValidator;

namespace Services.Validation;

public class CreateJobOpportunityRequestValidator : AbstractValidator<CreateJobOpportunityRequest>
{
    public CreateJobOpportunityRequestValidator()
    {
        RuleFor(x => x.EmployerName)
            .NotEmpty().WithMessage("اسم جهة العمل مطلوب.")
            .MaximumLength(150);

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("رقم الموبايل مطلوب.")
            .Matches(AdvertisementCatalog.EgyptianPhonePattern)
            .WithMessage("من فضلك اكتب رقم موبايل مصري صحيح (مثال: 01012345678).");

        RuleFor(x => x.WhatsApp)
            .NotEmpty().WithMessage("رقم الواتساب مطلوب.")
            .Matches(AdvertisementCatalog.EgyptianPhonePattern)
            .WithMessage("من فضلك اكتب رقم واتساب مصري صحيح (مثال: 01012345678).");

        RuleFor(x => x.JobTitle)
            .NotEmpty().WithMessage("المسمى الوظيفي مطلوب.")
            .MaximumLength(150);

        RuleFor(x => x.JobField)
            .IsInEnum().WithMessage("من فضلك اختار مجال الوظيفة صح.");

        RuleFor(x => x.OtherJobField)
            .NotEmpty().WithMessage("من فضلك اكتب مجال الوظيفة لما تختار «أخرى».")
            .MaximumLength(150)
            .When(x => x.JobField == JobField.Other);

        RuleFor(x => x.RequiredExperience)
            .IsInEnum().WithMessage("من فضلك اختار مستوى الخبرة المطلوب صح.");

        RuleFor(x => x.WorkType)
            .IsInEnum().WithMessage("من فضلك اختار نوع العمل صح.");

        RuleFor(x => x.SalaryType)
            .IsInEnum().WithMessage("من فضلك اختار نوع الراتب صح.");

        RuleFor(x => x.Salary)
            .NotNull().WithMessage("من فضلك اكتب الراتب لما تختار «تحديد الراتب».")
            .GreaterThan(0).WithMessage("الراتب لازم يكون أكبر من صفر.")
            .When(x => x.SalaryType == SalaryType.Specified);

        RuleFor(x => x.Center)
            .Must(LocationConstants.IsValidCenter)
            .WithMessage("المركز لازم يكون واحد من مراكز الفيوم.")
            .When(x => !string.IsNullOrWhiteSpace(x.Center));

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("العنوان مطلوب.")
            .MaximumLength(300);

        RuleFor(x => x.GoogleMaps)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrWhiteSpace(x.GoogleMaps));

        RuleFor(x => x.Logo!)
            .Must(file => file.Length <= ImageConstants.MaxFileSizeBytes)
            .WithMessage($"اللوجو ما ينفعش يزيد عن {ImageConstants.MaxFileSizeBytes / (1024 * 1024)} ميجابايت.")
            .Must(file => HasAllowedExtension(file.FileName, ImageConstants.AllowedExtensions))
            .WithMessage("اللوجو لازم يكون ملف JPG أو PNG أو WEBP.")
            .When(x => x.Logo is not null);

        RuleFor(x => x.Images)
            .Must(images => images.Count <= ImageConstants.MaxImagesPerItem)
            .WithMessage($"مسموح بـ {ImageConstants.MaxImagesPerItem} صور بحد أقصى.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("عنوان الإعلان مطلوب.")
            .MaximumLength(150);

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("وصف الوظيفة مطلوب.")
            .MaximumLength(4000);
    }
}
