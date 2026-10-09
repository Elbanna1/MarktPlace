using FluentValidation;
using Shared.Constants;
using Shared.DTOs.Sehhaty;
using Shared.Enums;

namespace Services.Validation;

public static class SehhatyValidationRules
{
    public static void AddSehhatyRules<T>(
        this AbstractValidator<T> validator, SehhatyModule module, bool isCreate)
        where T : CreateSehhatyListingRequest
    {
        validator.RuleFor(x => x.Name)
            .NotEmpty().WithMessage(SehhatyCatalog.NameRequiredMessage(module))
            .MaximumLength(SehhatyCatalog.MaxNameLength);

        validator.RuleFor(x => x.Title)
            .NotEmpty().WithMessage("عنوان الإعلان مطلوب.")
            .MaximumLength(SehhatyCatalog.MaxTitleLength);

        validator.RuleFor(x => x.Description)
            .NotEmpty().WithMessage("وصف الإعلان مطلوب.")
            .MaximumLength(SehhatyCatalog.MaxDescriptionLength);

        validator.RuleFor(x => x.Address)
            .NotEmpty().WithMessage("العنوان مطلوب.")
            .MaximumLength(SehhatyCatalog.MaxAddressLength);

        validator.RuleFor(x => x.GoogleMapsUrl)
            .MaximumLength(SehhatyCatalog.MaxGoogleMapsUrlLength)
            .Must(SehhatyCatalog.IsHttpUrl).WithMessage(SehhatyCatalog.GoogleMapsUrlMessage)
            .When(x => !string.IsNullOrWhiteSpace(x.GoogleMapsUrl));

        validator.RuleFor(x => x.Phone)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("رقم الهاتف مطلوب.")
            .Matches(AdvertisementCatalog.EgyptianPhonePattern)
            .WithMessage(AdvertisementCatalog.EgyptianPhoneMessage)
            .MaximumLength(SehhatyCatalog.MaxPhoneLength);

        validator.RuleFor(x => x.WhatsApp)
            .Matches(AdvertisementCatalog.EgyptianPhonePattern)
            .WithMessage(AdvertisementCatalog.EgyptianPhoneMessage)
            .MaximumLength(SehhatyCatalog.MaxPhoneLength)
            .When(x => !string.IsNullOrWhiteSpace(x.WhatsApp));

        validator.RuleFor(x => x.Email)
            .EmailAddress().WithMessage(SehhatyCatalog.EmailMessage)
            .MaximumLength(SehhatyCatalog.MaxEmailLength)
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        if (isCreate && SehhatyCatalog.ImagesRequired)
        {
            validator.RuleFor(x => x.Images)
                .NotEmpty().WithMessage("لازم ترفع صورة واحدة على الأقل.");
        }

        validator.RuleFor(x => x.Images)
            .Must(images => images.Count <= ImageConstants.MaxImagesPerItem)
            .WithMessage($"مسموح بـ {ImageConstants.MaxImagesPerItem} صور بحد أقصى.");
    }

    public static void AddDoctorRules<T>(this AbstractValidator<T> validator)
        where T : CreateDoctorRequest
    {
        validator.RuleFor(x => x.Specialty)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage($"{SehhatyCatalog.SpecialtyLabel} مطلوب.")
            .IsInEnum().WithMessage($"من فضلك اختار {SehhatyCatalog.SpecialtyLabel} صح.");

        validator.RuleFor(x => x.OtherSpecialty)
            .NotEmpty().WithMessage(SehhatyCatalog.OtherSpecialtyRequiredMessage)
            .When(x => x.Specialty == DoctorSpecialty.Other);

        validator.RuleFor(x => x.OtherSpecialty)
            .MaximumLength(SehhatyCatalog.MaxOtherSpecialtyLength)
            .When(x => !string.IsNullOrWhiteSpace(x.OtherSpecialty));
    }
}

public class CreateDoctorRequestValidator : AbstractValidator<CreateDoctorRequest>
{
    public CreateDoctorRequestValidator()
    {
        this.AddSehhatyRules(SehhatyCatalog.Doctors, isCreate: true);
        this.AddDoctorRules();
    }
}

public class UpdateDoctorRequestValidator : AbstractValidator<UpdateDoctorRequest>
{
    public UpdateDoctorRequestValidator()
    {
        this.AddSehhatyRules(SehhatyCatalog.Doctors, isCreate: false);
        this.AddDoctorRules();
    }
}

public class CreateMedicalLaboratoryRequestValidator : AbstractValidator<CreateMedicalLaboratoryRequest>
{
    public CreateMedicalLaboratoryRequestValidator() =>
        this.AddSehhatyRules(SehhatyCatalog.MedicalLaboratories, isCreate: true);
}

public class UpdateMedicalLaboratoryRequestValidator : AbstractValidator<UpdateMedicalLaboratoryRequest>
{
    public UpdateMedicalLaboratoryRequestValidator() =>
        this.AddSehhatyRules(SehhatyCatalog.MedicalLaboratories, isCreate: false);
}

public class CreateDentalLaboratoryRequestValidator : AbstractValidator<CreateDentalLaboratoryRequest>
{
    public CreateDentalLaboratoryRequestValidator() =>
        this.AddSehhatyRules(SehhatyCatalog.DentalLaboratories, isCreate: true);
}

public class UpdateDentalLaboratoryRequestValidator : AbstractValidator<UpdateDentalLaboratoryRequest>
{
    public UpdateDentalLaboratoryRequestValidator() =>
        this.AddSehhatyRules(SehhatyCatalog.DentalLaboratories, isCreate: false);
}

public class CreateRadiologyLaboratoryRequestValidator : AbstractValidator<CreateRadiologyLaboratoryRequest>
{
    public CreateRadiologyLaboratoryRequestValidator() =>
        this.AddSehhatyRules(SehhatyCatalog.RadiologyLaboratories, isCreate: true);
}

public class UpdateRadiologyLaboratoryRequestValidator : AbstractValidator<UpdateRadiologyLaboratoryRequest>
{
    public UpdateRadiologyLaboratoryRequestValidator() =>
        this.AddSehhatyRules(SehhatyCatalog.RadiologyLaboratories, isCreate: false);
}

public class CreateMedicalCenterRequestValidator : AbstractValidator<CreateMedicalCenterRequest>
{
    public CreateMedicalCenterRequestValidator() =>
        this.AddSehhatyRules(SehhatyCatalog.MedicalCenters, isCreate: true);
}

public class UpdateMedicalCenterRequestValidator : AbstractValidator<UpdateMedicalCenterRequest>
{
    public UpdateMedicalCenterRequestValidator() =>
        this.AddSehhatyRules(SehhatyCatalog.MedicalCenters, isCreate: false);
}

public class CreateHospitalRequestValidator : AbstractValidator<CreateHospitalRequest>
{
    public CreateHospitalRequestValidator() =>
        this.AddSehhatyRules(SehhatyCatalog.Hospitals, isCreate: true);
}

public class UpdateHospitalRequestValidator : AbstractValidator<UpdateHospitalRequest>
{
    public UpdateHospitalRequestValidator() =>
        this.AddSehhatyRules(SehhatyCatalog.Hospitals, isCreate: false);
}

public class SehhatyFilterParamsValidator<T> : AbstractValidator<T>
    where T : SehhatyFilterParams
{
    public SehhatyFilterParamsValidator()
    {
        RuleFor(x => x.Search).MaximumLength(150);
        RuleFor(x => x.Name).MaximumLength(SehhatyCatalog.MaxNameLength);
        RuleFor(x => x.SortBy).IsInEnum().WithMessage("من فضلك اختار ترتيب صح.");
    }
}

public class DoctorFilterParamsValidator : SehhatyFilterParamsValidator<DoctorFilterParams>
{
    public DoctorFilterParamsValidator()
    {
        RuleFor(x => x.Specialty)
            .IsInEnum().WithMessage($"من فضلك اختار {SehhatyCatalog.SpecialtyLabel} صح.")
            .When(x => x.Specialty.HasValue);
    }
}

public class MedicalLaboratoryFilterParamsValidator : SehhatyFilterParamsValidator<MedicalLaboratoryFilterParams>;

public class DentalLaboratoryFilterParamsValidator : SehhatyFilterParamsValidator<DentalLaboratoryFilterParams>;

public class RadiologyLaboratoryFilterParamsValidator : SehhatyFilterParamsValidator<RadiologyLaboratoryFilterParams>;

public class MedicalCenterFilterParamsValidator : SehhatyFilterParamsValidator<MedicalCenterFilterParams>;

public class HospitalFilterParamsValidator : SehhatyFilterParamsValidator<HospitalFilterParams>;
