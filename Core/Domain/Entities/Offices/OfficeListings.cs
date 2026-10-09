namespace Domain.Entities;

public class LawyerOffice : DirectoryListing<LawyerOfficeImage>;

public class LawyerOfficeImage : DirectoryListingImage
{
    public LawyerOffice Listing { get; set; } = default!;
}

public class EngineeringConsultancyOffice : DirectoryListing<EngineeringConsultancyOfficeImage>;

public class EngineeringConsultancyOfficeImage : DirectoryListingImage
{
    public EngineeringConsultancyOffice Listing { get; set; } = default!;
}

public class Contractor : DirectoryListing<ContractorImage>;

public class ContractorImage : DirectoryListingImage
{
    public Contractor Listing { get; set; } = default!;
}

public class IndustrialConsultancyOffice : DirectoryListing<IndustrialConsultancyOfficeImage>;

public class IndustrialConsultancyOfficeImage : DirectoryListingImage
{
    public IndustrialConsultancyOffice Listing { get; set; } = default!;
}

public class AccountantOffice : DirectoryListing<AccountantOfficeImage>;

public class AccountantOfficeImage : DirectoryListingImage
{
    public AccountantOffice Listing { get; set; } = default!;
}
