using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class LawyerOfficeConfiguration : IEntityTypeConfiguration<LawyerOffice>
{
    public void Configure(EntityTypeBuilder<LawyerOffice> builder) =>
        DirectoryListingMapping.Apply<LawyerOffice, LawyerOfficeImage>(builder, "LawyerOffices");
}

public class LawyerOfficeImageConfiguration : IEntityTypeConfiguration<LawyerOfficeImage>
{
    public void Configure(EntityTypeBuilder<LawyerOfficeImage> builder)
    {
        DirectoryListingMapping.ApplyImage(builder, "LawyerOfficeImages");
        builder.HasOne(x => x.Listing).WithMany(x => x.Images).HasForeignKey(x => x.ListingId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasQueryFilter(x => !x.Listing.IsDeleted);
    }
}

public class EngineeringConsultancyOfficeConfiguration : IEntityTypeConfiguration<EngineeringConsultancyOffice>
{
    public void Configure(EntityTypeBuilder<EngineeringConsultancyOffice> builder) =>
        DirectoryListingMapping.Apply<EngineeringConsultancyOffice, EngineeringConsultancyOfficeImage>(builder, "EngineeringConsultancyOffices");
}

public class EngineeringConsultancyOfficeImageConfiguration : IEntityTypeConfiguration<EngineeringConsultancyOfficeImage>
{
    public void Configure(EntityTypeBuilder<EngineeringConsultancyOfficeImage> builder)
    {
        DirectoryListingMapping.ApplyImage(builder, "EngineeringConsultancyOfficeImages");
        builder.HasOne(x => x.Listing).WithMany(x => x.Images).HasForeignKey(x => x.ListingId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasQueryFilter(x => !x.Listing.IsDeleted);
    }
}

public class ContractorConfiguration : IEntityTypeConfiguration<Contractor>
{
    public void Configure(EntityTypeBuilder<Contractor> builder) =>
        DirectoryListingMapping.Apply<Contractor, ContractorImage>(builder, "Contractors");
}

public class ContractorImageConfiguration : IEntityTypeConfiguration<ContractorImage>
{
    public void Configure(EntityTypeBuilder<ContractorImage> builder)
    {
        DirectoryListingMapping.ApplyImage(builder, "ContractorImages");
        builder.HasOne(x => x.Listing).WithMany(x => x.Images).HasForeignKey(x => x.ListingId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasQueryFilter(x => !x.Listing.IsDeleted);
    }
}

public class IndustrialConsultancyOfficeConfiguration : IEntityTypeConfiguration<IndustrialConsultancyOffice>
{
    public void Configure(EntityTypeBuilder<IndustrialConsultancyOffice> builder) =>
        DirectoryListingMapping.Apply<IndustrialConsultancyOffice, IndustrialConsultancyOfficeImage>(builder, "IndustrialConsultancyOffices");
}

public class IndustrialConsultancyOfficeImageConfiguration : IEntityTypeConfiguration<IndustrialConsultancyOfficeImage>
{
    public void Configure(EntityTypeBuilder<IndustrialConsultancyOfficeImage> builder)
    {
        DirectoryListingMapping.ApplyImage(builder, "IndustrialConsultancyOfficeImages");
        builder.HasOne(x => x.Listing).WithMany(x => x.Images).HasForeignKey(x => x.ListingId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasQueryFilter(x => !x.Listing.IsDeleted);
    }
}

public class AccountantOfficeConfiguration : IEntityTypeConfiguration<AccountantOffice>
{
    public void Configure(EntityTypeBuilder<AccountantOffice> builder) =>
        DirectoryListingMapping.Apply<AccountantOffice, AccountantOfficeImage>(builder, "AccountantOffices");
}

public class AccountantOfficeImageConfiguration : IEntityTypeConfiguration<AccountantOfficeImage>
{
    public void Configure(EntityTypeBuilder<AccountantOfficeImage> builder)
    {
        DirectoryListingMapping.ApplyImage(builder, "AccountantOfficeImages");
        builder.HasOne(x => x.Listing).WithMany(x => x.Images).HasForeignKey(x => x.ListingId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasQueryFilter(x => !x.Listing.IsDeleted);
    }
}
