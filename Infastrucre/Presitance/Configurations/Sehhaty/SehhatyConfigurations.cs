using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Persistence.Configurations;

public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        DirectoryListingMapping.Apply<Doctor, DoctorImage>(builder, "Doctors");

        builder.Property(x => x.Specialty).HasConversion<int>();
        builder.Property(x => x.OtherSpecialty).HasMaxLength(SehhatyCatalog.MaxOtherSpecialtyLength);

        builder.HasIndex(x => new { x.ModerationStatus, x.Specialty, x.CreatedAt })
            .IncludeProperties(x => new { x.IsDeleted, x.ExpireAt });
    }
}

public class DoctorImageConfiguration : IEntityTypeConfiguration<DoctorImage>
{
    public void Configure(EntityTypeBuilder<DoctorImage> builder)
    {
        DirectoryListingMapping.ApplyImage(builder, "DoctorImages");
        builder.HasOne(x => x.Listing).WithMany(x => x.Images).HasForeignKey(x => x.ListingId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasQueryFilter(x => !x.Listing.IsDeleted);
    }
}

public class MedicalLaboratoryConfiguration : IEntityTypeConfiguration<MedicalLaboratory>
{
    public void Configure(EntityTypeBuilder<MedicalLaboratory> builder) =>
        DirectoryListingMapping.Apply<MedicalLaboratory, MedicalLaboratoryImage>(builder, "MedicalLaboratories");
}

public class MedicalLaboratoryImageConfiguration : IEntityTypeConfiguration<MedicalLaboratoryImage>
{
    public void Configure(EntityTypeBuilder<MedicalLaboratoryImage> builder)
    {
        DirectoryListingMapping.ApplyImage(builder, "MedicalLaboratoryImages");
        builder.HasOne(x => x.Listing).WithMany(x => x.Images).HasForeignKey(x => x.ListingId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasQueryFilter(x => !x.Listing.IsDeleted);
    }
}

public class DentalLaboratoryConfiguration : IEntityTypeConfiguration<DentalLaboratory>
{
    public void Configure(EntityTypeBuilder<DentalLaboratory> builder) =>
        DirectoryListingMapping.Apply<DentalLaboratory, DentalLaboratoryImage>(builder, "DentalLaboratories");
}

public class DentalLaboratoryImageConfiguration : IEntityTypeConfiguration<DentalLaboratoryImage>
{
    public void Configure(EntityTypeBuilder<DentalLaboratoryImage> builder)
    {
        DirectoryListingMapping.ApplyImage(builder, "DentalLaboratoryImages");
        builder.HasOne(x => x.Listing).WithMany(x => x.Images).HasForeignKey(x => x.ListingId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasQueryFilter(x => !x.Listing.IsDeleted);
    }
}

public class RadiologyLaboratoryConfiguration : IEntityTypeConfiguration<RadiologyLaboratory>
{
    public void Configure(EntityTypeBuilder<RadiologyLaboratory> builder) =>
        DirectoryListingMapping.Apply<RadiologyLaboratory, RadiologyLaboratoryImage>(builder, "RadiologyLaboratories");
}

public class RadiologyLaboratoryImageConfiguration : IEntityTypeConfiguration<RadiologyLaboratoryImage>
{
    public void Configure(EntityTypeBuilder<RadiologyLaboratoryImage> builder)
    {
        DirectoryListingMapping.ApplyImage(builder, "RadiologyLaboratoryImages");
        builder.HasOne(x => x.Listing).WithMany(x => x.Images).HasForeignKey(x => x.ListingId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasQueryFilter(x => !x.Listing.IsDeleted);
    }
}

public class MedicalCenterConfiguration : IEntityTypeConfiguration<MedicalCenter>
{
    public void Configure(EntityTypeBuilder<MedicalCenter> builder) =>
        DirectoryListingMapping.Apply<MedicalCenter, MedicalCenterImage>(builder, "MedicalCenters");
}

public class MedicalCenterImageConfiguration : IEntityTypeConfiguration<MedicalCenterImage>
{
    public void Configure(EntityTypeBuilder<MedicalCenterImage> builder)
    {
        DirectoryListingMapping.ApplyImage(builder, "MedicalCenterImages");
        builder.HasOne(x => x.Listing).WithMany(x => x.Images).HasForeignKey(x => x.ListingId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasQueryFilter(x => !x.Listing.IsDeleted);
    }
}

public class HospitalConfiguration : IEntityTypeConfiguration<Hospital>
{
    public void Configure(EntityTypeBuilder<Hospital> builder) =>
        DirectoryListingMapping.Apply<Hospital, HospitalImage>(builder, "Hospitals");
}

public class HospitalImageConfiguration : IEntityTypeConfiguration<HospitalImage>
{
    public void Configure(EntityTypeBuilder<HospitalImage> builder)
    {
        DirectoryListingMapping.ApplyImage(builder, "HospitalImages");
        builder.HasOne(x => x.Listing).WithMany(x => x.Images).HasForeignKey(x => x.ListingId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasQueryFilter(x => !x.Listing.IsDeleted);
    }
}
