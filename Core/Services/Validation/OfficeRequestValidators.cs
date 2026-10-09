using FluentValidation;
using Shared.Constants;
using Shared.DTOs.Offices;

namespace Services.Validation;

public class CreateLawyerOfficeRequestValidator : AbstractValidator<CreateLawyerOfficeRequest>
{
    public CreateLawyerOfficeRequestValidator() =>
        this.AddDirectoryRules(OfficesCatalog.LawyerOffices, isCreate: true);
}

public class UpdateLawyerOfficeRequestValidator : AbstractValidator<UpdateLawyerOfficeRequest>
{
    public UpdateLawyerOfficeRequestValidator() =>
        this.AddDirectoryRules(OfficesCatalog.LawyerOffices, isCreate: false);
}

public class LawyerOfficeFilterParamsValidator : DirectoryFilterParamsValidator<LawyerOfficeFilterParams>;

public class CreateEngineeringConsultancyOfficeRequestValidator : AbstractValidator<CreateEngineeringConsultancyOfficeRequest>
{
    public CreateEngineeringConsultancyOfficeRequestValidator() =>
        this.AddDirectoryRules(OfficesCatalog.EngineeringConsultancyOffices, isCreate: true);
}

public class UpdateEngineeringConsultancyOfficeRequestValidator : AbstractValidator<UpdateEngineeringConsultancyOfficeRequest>
{
    public UpdateEngineeringConsultancyOfficeRequestValidator() =>
        this.AddDirectoryRules(OfficesCatalog.EngineeringConsultancyOffices, isCreate: false);
}

public class EngineeringConsultancyOfficeFilterParamsValidator : DirectoryFilterParamsValidator<EngineeringConsultancyOfficeFilterParams>;

public class CreateContractorRequestValidator : AbstractValidator<CreateContractorRequest>
{
    public CreateContractorRequestValidator() =>
        this.AddDirectoryRules(OfficesCatalog.Contractors, isCreate: true);
}

public class UpdateContractorRequestValidator : AbstractValidator<UpdateContractorRequest>
{
    public UpdateContractorRequestValidator() =>
        this.AddDirectoryRules(OfficesCatalog.Contractors, isCreate: false);
}

public class ContractorFilterParamsValidator : DirectoryFilterParamsValidator<ContractorFilterParams>;

public class CreateIndustrialConsultancyOfficeRequestValidator : AbstractValidator<CreateIndustrialConsultancyOfficeRequest>
{
    public CreateIndustrialConsultancyOfficeRequestValidator() =>
        this.AddDirectoryRules(OfficesCatalog.IndustrialConsultancyOffices, isCreate: true);
}

public class UpdateIndustrialConsultancyOfficeRequestValidator : AbstractValidator<UpdateIndustrialConsultancyOfficeRequest>
{
    public UpdateIndustrialConsultancyOfficeRequestValidator() =>
        this.AddDirectoryRules(OfficesCatalog.IndustrialConsultancyOffices, isCreate: false);
}

public class IndustrialConsultancyOfficeFilterParamsValidator : DirectoryFilterParamsValidator<IndustrialConsultancyOfficeFilterParams>;

public class CreateAccountantOfficeRequestValidator : AbstractValidator<CreateAccountantOfficeRequest>
{
    public CreateAccountantOfficeRequestValidator() =>
        this.AddDirectoryRules(OfficesCatalog.AccountantOffices, isCreate: true);
}

public class UpdateAccountantOfficeRequestValidator : AbstractValidator<UpdateAccountantOfficeRequest>
{
    public UpdateAccountantOfficeRequestValidator() =>
        this.AddDirectoryRules(OfficesCatalog.AccountantOffices, isCreate: false);
}

public class AccountantOfficeFilterParamsValidator : DirectoryFilterParamsValidator<AccountantOfficeFilterParams>;
