using Shared.DTOs.Directory;
using Shared.Enums;

namespace Shared.DTOs.Offices;

public class CreateLawyerOfficeRequest : CreateDirectoryListingRequest;

public class UpdateLawyerOfficeRequest : CreateLawyerOfficeRequest, IDirectoryGalleryEdit
{
    public List<Guid> RemoveImageIds { get; set; } = new();
}

public class LawyerOfficeDto : DirectoryListingDto
{
    public override ListingModuleType StatsListingType => ListingModuleType.LawyerOffice;
}

public class LawyerOfficeFilterParams : DirectoryFilterParams;

public class CreateEngineeringConsultancyOfficeRequest : CreateDirectoryListingRequest;

public class UpdateEngineeringConsultancyOfficeRequest : CreateEngineeringConsultancyOfficeRequest, IDirectoryGalleryEdit
{
    public List<Guid> RemoveImageIds { get; set; } = new();
}

public class EngineeringConsultancyOfficeDto : DirectoryListingDto
{
    public override ListingModuleType StatsListingType => ListingModuleType.EngineeringConsultancyOffice;
}

public class EngineeringConsultancyOfficeFilterParams : DirectoryFilterParams;

public class CreateContractorRequest : CreateDirectoryListingRequest;

public class UpdateContractorRequest : CreateContractorRequest, IDirectoryGalleryEdit
{
    public List<Guid> RemoveImageIds { get; set; } = new();
}

public class ContractorDto : DirectoryListingDto
{
    public override ListingModuleType StatsListingType => ListingModuleType.Contractor;
}

public class ContractorFilterParams : DirectoryFilterParams;

public class CreateIndustrialConsultancyOfficeRequest : CreateDirectoryListingRequest;

public class UpdateIndustrialConsultancyOfficeRequest : CreateIndustrialConsultancyOfficeRequest, IDirectoryGalleryEdit
{
    public List<Guid> RemoveImageIds { get; set; } = new();
}

public class IndustrialConsultancyOfficeDto : DirectoryListingDto
{
    public override ListingModuleType StatsListingType => ListingModuleType.IndustrialConsultancyOffice;
}

public class IndustrialConsultancyOfficeFilterParams : DirectoryFilterParams;

public class CreateAccountantOfficeRequest : CreateDirectoryListingRequest;

public class UpdateAccountantOfficeRequest : CreateAccountantOfficeRequest, IDirectoryGalleryEdit
{
    public List<Guid> RemoveImageIds { get; set; } = new();
}

public class AccountantOfficeDto : DirectoryListingDto
{
    public override ListingModuleType StatsListingType => ListingModuleType.AccountantOffice;
}

public class AccountantOfficeFilterParams : DirectoryFilterParams;
