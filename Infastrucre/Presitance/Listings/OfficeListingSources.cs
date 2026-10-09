using Domain.Entities;
using Persistence.Data;
using Shared.Enums;

namespace Persistence.Listings;

public sealed class LawyerOfficeListingSource(AppDbContext context)
    : DirectoryListingSource<LawyerOffice, LawyerOfficeImage>(context)
{
    public override ListingModuleType Type => ListingModuleType.LawyerOffice;
}

public sealed class EngineeringConsultancyOfficeListingSource(AppDbContext context)
    : DirectoryListingSource<EngineeringConsultancyOffice, EngineeringConsultancyOfficeImage>(context)
{
    public override ListingModuleType Type => ListingModuleType.EngineeringConsultancyOffice;
}

public sealed class ContractorListingSource(AppDbContext context)
    : DirectoryListingSource<Contractor, ContractorImage>(context)
{
    public override ListingModuleType Type => ListingModuleType.Contractor;
}

public sealed class IndustrialConsultancyOfficeListingSource(AppDbContext context)
    : DirectoryListingSource<IndustrialConsultancyOffice, IndustrialConsultancyOfficeImage>(context)
{
    public override ListingModuleType Type => ListingModuleType.IndustrialConsultancyOffice;
}

public sealed class AccountantOfficeListingSource(AppDbContext context)
    : DirectoryListingSource<AccountantOffice, AccountantOfficeImage>(context)
{
    public override ListingModuleType Type => ListingModuleType.AccountantOffice;
}
