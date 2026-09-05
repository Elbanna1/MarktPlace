using Domain.Entities;
using Shared.Constants;
using Shared.Enums;

namespace Persistence.Configurations;

public class RealEstateListingTypeLookupConfiguration
    : RealEstateLookupConfiguration<RealEstateListingTypeLookup, RealEstateListingType>
{
    protected override string TableName => "RealEstateListingTypes";
    protected override IReadOnlyList<RealEstateLookupEntry<RealEstateListingType>> Entries =>
        RealEstateCatalog.ListingTypes;
}

public class RealEstateProjectLookupConfiguration
    : RealEstateLookupConfiguration<RealEstateProjectLookup, RealEstateProject>
{
    protected override string TableName => "RealEstateProjects";
    protected override IReadOnlyList<RealEstateLookupEntry<RealEstateProject>> Entries =>
        RealEstateCatalog.Projects;
}
