using FluentValidation;
using Shared.Constants;
using Shared.DTOs.JobRequests;
using Shared.Enums;
using static Services.Validation.CreateJobRequestRequestValidator;

namespace Services.Validation;

public class UpdateJobRequestRequestValidator : AbstractValidator<UpdateJobRequestRequest>
{
    public UpdateJobRequestRequestValidator()
    {
        RuleFor(x => x.ApplicantName)
            .NotEmpty().WithMessage("اسم المتقدم مطلوب.")
            .MaximumLength(150);

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("رقم الموبايل مطلوب.")
            .Matches(AdvertisementCatalog.EgyptianPhonePattern)
            .WithMessage("من فضلك اكتب رقم موبايل مصري صحيح (مثال: 01012345678).");

        RuleFor(x => x.WhatsApp)
            .NotEmpty().WithMessage("رقم الواتساب مطلوب.")
            .Matches(AdvertisementCatalog.EgyptianPhonePattern)
            .WithMessage("من فضلك اكتب رقم واتساب مصري صحيح (مثال: 01012345678).");

        RuleFor(x => x.JobField)
            .IsInEnum().WithMessage("من فضلك اختار مجال الوظيفة صح.");

        RuleFor(x => x.OtherJobField)
            .NotEmpty().WithMessage("من فضلك اكتب مجال الوظيفة لما تختار «أخرى».")
            .MaximumLength(150)
            .When(x => x.JobField == JobField.Other);

        RuleFor(x => x.Experience)
            .IsInEnum().WithMessage("من فضلك اختار مستوى الخبرة صح.");

        RuleFor(x => x.Education)
            .IsInEnum().WithMessage("من فضلك اختار المؤهل الدراسي صح.");

        RuleFor(x => x.Skills)
            .NotEmpty().WithMessage("المهارات مطلوبة.")
            .MaximumLength(1000);

        RuleFor(x => x.Center)
            .Must(LocationConstants.IsValidCenter)
            .WithMessage("المركز لازم يكون واحد من مراكز الفيوم.")
            .When(x => !string.IsNullOrWhiteSpace(x.Center));

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("العنوان مطلوب.")
            .MaximumLength(300);

        RuleFor(x => x.CvFile!)
            .Must(file => file.Length <= FileUploadConstants.MaxDocumentSizeBytes)
            .WithMessage($"ملف السيرة الذاتية ما ينفعش يزيد عن {FileUploadConstants.MaxDocumentSizeBytes / (1024 * 1024)} ميجابايت.")
            .Must(file => HasAllowedExtension(file.FileName, DocumentFormatCatalog.AllExtensions))
            .WithMessage($"ملف السيرة الذاتية لازم يكون بصيغة: {DocumentFormatCatalog.DisplayNames}.")
            .When(x => x.CvFile is not null);

        RuleFor(x => x.ProfileImage!)
            .Must(file => file.Length <= ImageConstants.MaxFileSizeBytes)
            .WithMessage($"صورة الحساب ما ينفعش تزيد عن {ImageConstants.MaxFileSizeBytes / (1024 * 1024)} ميجابايت.")
            .Must(file => HasAllowedExtension(file.FileName, ImageConstants.AllowedExtensions))
            .WithMessage("صورة الحساب لازم تكون ملف JPG أو PNG أو WEBP.")
            .When(x => x.ProfileImage is not null);

        RuleFor(x => x.IntroVideo!)
            .Must(file => file.Length <= FileUploadConstants.MaxVideoSizeBytes)
            .WithMessage($"الفيديو التعريفي ما ينفعش يزيد عن {FileUploadConstants.MaxVideoSizeBytes / (1024 * 1024)} ميجابايت.")
            .Must(file => HasAllowedExtension(file.FileName, VideoFormatCatalog.AllExtensions))
            .WithMessage($"الفيديو التعريفي لازم يكون بصيغة: {VideoFormatCatalog.DisplayNames}.")
            .When(x => x.IntroVideo is not null);

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("عنوان الإعلان مطلوب.")
            .MaximumLength(150);

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("وصف الإعلان مطلوب.")
            .MaximumLength(4000);
    }
}
