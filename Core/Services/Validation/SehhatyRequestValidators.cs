using FluentValidation;
using Shared.Constants;
using Shared.DTOs.Sehhaty;
using Shared.DTOs.Directory;
using Shared.Enums;

namespace Services.Validation;

public static class SehhatyValidationRules
{
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
        this.AddDirectoryRules(SehhatyCatalog.Doctors, isCreate: true);
        this.AddDoctorRules();
    }
}

public class UpdateDoctorRequestValidator : AbstractValidator<UpdateDoctorRequest>
{
    public UpdateDoctorRequestValidator()
    {
        this.AddDirectoryRules(SehhatyCatalog.Doctors, isCreate: false);
        this.AddDoctorRules();
    }
}

public class CreateMedicalLaboratoryRequestValidator : AbstractValidator<CreateMedicalLaboratoryRequest>
{
    public CreateMedicalLaboratoryRequestValidator() =>
        this.AddDirectoryRules(SehhatyCatalog.MedicalLaboratories, isCreate: true);
}

public class UpdateMedicalLaboratoryRequestValidator : AbstractValidator<UpdateMedicalLaboratoryRequest>
{
    public UpdateMedicalLaboratoryRequestValidator() =>
        this.AddDirectoryRules(SehhatyCatalog.MedicalLaboratories, isCreate: false);
}

public class CreateDentalLaboratoryRequestValidator : AbstractValidator<CreateDentalLaboratoryRequest>
{
    public CreateDentalLaboratoryRequestValidator() =>
        this.AddDirectoryRules(SehhatyCatalog.DentalLaboratories, isCreate: true);
}

public class UpdateDentalLaboratoryRequestValidator : AbstractValidator<UpdateDentalLaboratoryRequest>
{
    public UpdateDentalLaboratoryRequestValidator() =>
        this.AddDirectoryRules(SehhatyCatalog.DentalLaboratories, isCreate: false);
}

public class CreateRadiologyLaboratoryRequestValidator : AbstractValidator<CreateRadiologyLaboratoryRequest>
{
    public CreateRadiologyLaboratoryRequestValidator() =>
        this.AddDirectoryRules(SehhatyCatalog.RadiologyLaboratories, isCreate: true);
}

public class UpdateRadiologyLaboratoryRequestValidator : AbstractValidator<UpdateRadiologyLaboratoryRequest>
{
    public UpdateRadiologyLaboratoryRequestValidator() =>
        this.AddDirectoryRules(SehhatyCatalog.RadiologyLaboratories, isCreate: false);
}

public class CreateMedicalCenterRequestValidator : AbstractValidator<CreateMedicalCenterRequest>
{
    public CreateMedicalCenterRequestValidator() =>
        this.AddDirectoryRules(SehhatyCatalog.MedicalCenters, isCreate: true);
}

public class UpdateMedicalCenterRequestValidator : AbstractValidator<UpdateMedicalCenterRequest>
{
    public UpdateMedicalCenterRequestValidator() =>
        this.AddDirectoryRules(SehhatyCatalog.MedicalCenters, isCreate: false);
}

public class CreateHospitalRequestValidator : AbstractValidator<CreateHospitalRequest>
{
    public CreateHospitalRequestValidator() =>
        this.AddDirectoryRules(SehhatyCatalog.Hospitals, isCreate: true);
}

public class UpdateHospitalRequestValidator : AbstractValidator<UpdateHospitalRequest>
{
    public UpdateHospitalRequestValidator() =>
        this.AddDirectoryRules(SehhatyCatalog.Hospitals, isCreate: false);
}

public class DoctorFilterParamsValidator : DirectoryFilterParamsValidator<DoctorFilterParams>
{
    public DoctorFilterParamsValidator()
    {
        RuleFor(x => x.Specialty)
            .IsInEnum().WithMessage($"من فضلك اختار {SehhatyCatalog.SpecialtyLabel} صح.")
            .When(x => x.Specialty.HasValue);
    }
}

public class MedicalLaboratoryFilterParamsValidator : DirectoryFilterParamsValidator<MedicalLaboratoryFilterParams>;

public class DentalLaboratoryFilterParamsValidator : DirectoryFilterParamsValidator<DentalLaboratoryFilterParams>;

public class RadiologyLaboratoryFilterParamsValidator : DirectoryFilterParamsValidator<RadiologyLaboratoryFilterParams>;

public class MedicalCenterFilterParamsValidator : DirectoryFilterParamsValidator<MedicalCenterFilterParams>;

public class HospitalFilterParamsValidator : DirectoryFilterParamsValidator<HospitalFilterParams>;
