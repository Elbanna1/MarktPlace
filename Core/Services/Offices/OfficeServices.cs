using AutoMapper;
using Domain.Entities;
using Services.Directory;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Offices;

namespace Services.Offices;

public sealed class LawyerOfficeService
    : DirectoryListingService<LawyerOffice, LawyerOfficeImage, CreateLawyerOfficeRequest, UpdateLawyerOfficeRequest, LawyerOfficeDto, LawyerOfficeFilterParams>,
        ILawyerOfficeService
{
    public LawyerOfficeService(
        IDirectoryRepository<LawyerOffice, LawyerOfficeImage, LawyerOfficeFilterParams> repository,
        IFileService fileService, IMapper mapper, INotificationService notifications)
        : base(repository, fileService, mapper, notifications)
    {
    }

    protected override DirectoryModule Module => OfficesCatalog.LawyerOffices;
}

public sealed class EngineeringConsultancyOfficeService
    : DirectoryListingService<EngineeringConsultancyOffice, EngineeringConsultancyOfficeImage, CreateEngineeringConsultancyOfficeRequest, UpdateEngineeringConsultancyOfficeRequest, EngineeringConsultancyOfficeDto, EngineeringConsultancyOfficeFilterParams>,
        IEngineeringConsultancyOfficeService
{
    public EngineeringConsultancyOfficeService(
        IDirectoryRepository<EngineeringConsultancyOffice, EngineeringConsultancyOfficeImage, EngineeringConsultancyOfficeFilterParams> repository,
        IFileService fileService, IMapper mapper, INotificationService notifications)
        : base(repository, fileService, mapper, notifications)
    {
    }

    protected override DirectoryModule Module => OfficesCatalog.EngineeringConsultancyOffices;
}

public sealed class ContractorService
    : DirectoryListingService<Contractor, ContractorImage, CreateContractorRequest, UpdateContractorRequest, ContractorDto, ContractorFilterParams>,
        IContractorService
{
    public ContractorService(
        IDirectoryRepository<Contractor, ContractorImage, ContractorFilterParams> repository,
        IFileService fileService, IMapper mapper, INotificationService notifications)
        : base(repository, fileService, mapper, notifications)
    {
    }

    protected override DirectoryModule Module => OfficesCatalog.Contractors;
}

public sealed class IndustrialConsultancyOfficeService
    : DirectoryListingService<IndustrialConsultancyOffice, IndustrialConsultancyOfficeImage, CreateIndustrialConsultancyOfficeRequest, UpdateIndustrialConsultancyOfficeRequest, IndustrialConsultancyOfficeDto, IndustrialConsultancyOfficeFilterParams>,
        IIndustrialConsultancyOfficeService
{
    public IndustrialConsultancyOfficeService(
        IDirectoryRepository<IndustrialConsultancyOffice, IndustrialConsultancyOfficeImage, IndustrialConsultancyOfficeFilterParams> repository,
        IFileService fileService, IMapper mapper, INotificationService notifications)
        : base(repository, fileService, mapper, notifications)
    {
    }

    protected override DirectoryModule Module => OfficesCatalog.IndustrialConsultancyOffices;
}

public sealed class AccountantOfficeService
    : DirectoryListingService<AccountantOffice, AccountantOfficeImage, CreateAccountantOfficeRequest, UpdateAccountantOfficeRequest, AccountantOfficeDto, AccountantOfficeFilterParams>,
        IAccountantOfficeService
{
    public AccountantOfficeService(
        IDirectoryRepository<AccountantOffice, AccountantOfficeImage, AccountantOfficeFilterParams> repository,
        IFileService fileService, IMapper mapper, INotificationService notifications)
        : base(repository, fileService, mapper, notifications)
    {
    }

    protected override DirectoryModule Module => OfficesCatalog.AccountantOffices;
}
