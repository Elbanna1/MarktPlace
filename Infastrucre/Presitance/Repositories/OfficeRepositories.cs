using Domain.Entities;
using Persistence.Data;
using Shared.Constants;
using Shared.DTOs.Offices;

namespace Persistence.Repositories;

public sealed class LawyerOfficeRepository : DirectoryRepository<LawyerOffice, LawyerOfficeImage, LawyerOfficeFilterParams>
{
    public LawyerOfficeRepository(AppDbContext context) : base(context, OfficesCatalog.LawyerOffices) { }
}

public sealed class EngineeringConsultancyOfficeRepository : DirectoryRepository<EngineeringConsultancyOffice, EngineeringConsultancyOfficeImage, EngineeringConsultancyOfficeFilterParams>
{
    public EngineeringConsultancyOfficeRepository(AppDbContext context) : base(context, OfficesCatalog.EngineeringConsultancyOffices) { }
}

public sealed class ContractorRepository : DirectoryRepository<Contractor, ContractorImage, ContractorFilterParams>
{
    public ContractorRepository(AppDbContext context) : base(context, OfficesCatalog.Contractors) { }
}

public sealed class IndustrialConsultancyOfficeRepository : DirectoryRepository<IndustrialConsultancyOffice, IndustrialConsultancyOfficeImage, IndustrialConsultancyOfficeFilterParams>
{
    public IndustrialConsultancyOfficeRepository(AppDbContext context) : base(context, OfficesCatalog.IndustrialConsultancyOffices) { }
}

public sealed class AccountantOfficeRepository : DirectoryRepository<AccountantOffice, AccountantOfficeImage, AccountantOfficeFilterParams>
{
    public AccountantOfficeRepository(AppDbContext context) : base(context, OfficesCatalog.AccountantOffices) { }
}
