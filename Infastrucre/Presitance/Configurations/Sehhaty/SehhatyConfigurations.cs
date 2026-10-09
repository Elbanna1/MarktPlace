using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Persistence.Configurations;

internal static class SehhatyListingMapping
{
    public static void Apply<TListing, TImage>(EntityTypeBuilder<TListing> builder, string table)
        where TListing : SehhatyListing<TImage>
        where TImage : SehhatyListingImage
    {
        builder.ToTable(table);
        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(SehhatyCatalog.MaxNameLength);
        builder.Property(x => x.Title).IsRequired().HasMaxLength(SehhatyCatalog.MaxTitleLength);
        builder.Property(x => x.Description).IsRequired().HasMaxLength(SehhatyCatalog.MaxDescriptionLength);
        builder.Property(x => x.Address).IsRequired().HasMaxLength(SehhatyCatalog.MaxAddressLength);
        builder.Property(x => x.GoogleMapsUrl).HasMaxLength(SehhatyCatalog.MaxGoogleMapsUrlLength);
        builder.Property(x => x.Phone).IsRequired().HasMaxLength(SehhatyCatalog.MaxPhoneLength);
        builder.Property(x => x.WhatsApp).HasMaxLength(SehhatyCatalog.MaxPhoneLength);
        builder.Property(x => x.Email).HasMaxLength(SehhatyCatalog.MaxEmailLength);

        builder.Property(x => x.ModerationNotes).HasMaxLength(2000);
        builder.Property(x => x.ModeratedBy).HasMaxLength(450);

        builder.HasQueryFilter(QueryFilterNames.SoftDelete, x => !x.IsDeleted);

        builder.HasIndex(x => new { x.UserId, x.CreatedAt });
        builder.HasIndex(x => x.IsDeleted);

        builder.HasIndex(x => new { x.ModerationStatus, x.Name }, $"IX_{table}_Advertisers")
            .IncludeProperties(x => new { x.IsDeleted, x.ExpireAt });

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }

    public static void ApplyImage<TImage>(EntityTypeBuilder<TImage> builder, string table)
        where TImage : SehhatyListingImage
    {
        builder.ToTable(table);
        builder.HasKey(x => x.Id);

        builder.Property(x => x.FileName).IsRequired().HasMaxLength(260);
        builder.Property(x => x.ImagePath).IsRequired().HasMaxLength(500);
        builder.Property(x => x.ImageUrl).IsRequired().HasMaxLength(1000);

        builder.HasIndex(x => new { x.ListingId, x.SortOrder });
    }
}

public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        SehhatyListingMapping.Apply<Doctor, DoctorImage>(builder, "Doctors");

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
        SehhatyListingMapping.ApplyImage(builder, "DoctorImages");
        builder.HasOne(x => x.Listing).WithMany(x => x.Images).HasForeignKey(x => x.ListingId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasQueryFilter(x => !x.Listing.IsDeleted);
    }
}

public class MedicalLaboratoryConfiguration : IEntityTypeConfiguration<MedicalLaboratory>
{
    public void Configure(EntityTypeBuilder<MedicalLaboratory> builder) =>
        SehhatyListingMapping.Apply<MedicalLaboratory, MedicalLaboratoryImage>(builder, "MedicalLaboratories");
}

public class MedicalLaboratoryImageConfiguration : IEntityTypeConfiguration<MedicalLaboratoryImage>
{
    public void Configure(EntityTypeBuilder<MedicalLaboratoryImage> builder)
    {
        SehhatyListingMapping.ApplyImage(builder, "MedicalLaboratoryImages");
        builder.HasOne(x => x.Listing).WithMany(x => x.Images).HasForeignKey(x => x.ListingId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasQueryFilter(x => !x.Listing.IsDeleted);
    }
}

public class DentalLaboratoryConfiguration : IEntityTypeConfiguration<DentalLaboratory>
{
    public void Configure(EntityTypeBuilder<DentalLaboratory> builder) =>
        SehhatyListingMapping.Apply<DentalLaboratory, DentalLaboratoryImage>(builder, "DentalLaboratories");
}

public class DentalLaboratoryImageConfiguration : IEntityTypeConfiguration<DentalLaboratoryImage>
{
    public void Configure(EntityTypeBuilder<DentalLaboratoryImage> builder)
    {
        SehhatyListingMapping.ApplyImage(builder, "DentalLaboratoryImages");
        builder.HasOne(x => x.Listing).WithMany(x => x.Images).HasForeignKey(x => x.ListingId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasQueryFilter(x => !x.Listing.IsDeleted);
    }
}

public class RadiologyLaboratoryConfiguration : IEntityTypeConfiguration<RadiologyLaboratory>
{
    public void Configure(EntityTypeBuilder<RadiologyLaboratory> builder) =>
        SehhatyListingMapping.Apply<RadiologyLaboratory, RadiologyLaboratoryImage>(builder, "RadiologyLaboratories");
}

public class RadiologyLaboratoryImageConfiguration : IEntityTypeConfiguration<RadiologyLaboratoryImage>
{
    public void Configure(EntityTypeBuilder<RadiologyLaboratoryImage> builder)
    {
        SehhatyListingMapping.ApplyImage(builder, "RadiologyLaboratoryImages");
        builder.HasOne(x => x.Listing).WithMany(x => x.Images).HasForeignKey(x => x.ListingId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasQueryFilter(x => !x.Listing.IsDeleted);
    }
}

public class MedicalCenterConfiguration : IEntityTypeConfiguration<MedicalCenter>
{
    public void Configure(EntityTypeBuilder<MedicalCenter> builder) =>
        SehhatyListingMapping.Apply<MedicalCenter, MedicalCenterImage>(builder, "MedicalCenters");
}

public class MedicalCenterImageConfiguration : IEntityTypeConfiguration<MedicalCenterImage>
{
    public void Configure(EntityTypeBuilder<MedicalCenterImage> builder)
    {
        SehhatyListingMapping.ApplyImage(builder, "MedicalCenterImages");
        builder.HasOne(x => x.Listing).WithMany(x => x.Images).HasForeignKey(x => x.ListingId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasQueryFilter(x => !x.Listing.IsDeleted);
    }
}

public class HospitalConfiguration : IEntityTypeConfiguration<Hospital>
{
    public void Configure(EntityTypeBuilder<Hospital> builder) =>
        SehhatyListingMapping.Apply<Hospital, HospitalImage>(builder, "Hospitals");
}

public class HospitalImageConfiguration : IEntityTypeConfiguration<HospitalImage>
{
    public void Configure(EntityTypeBuilder<HospitalImage> builder)
    {
        SehhatyListingMapping.ApplyImage(builder, "HospitalImages");
        builder.HasOne(x => x.Listing).WithMany(x => x.Images).HasForeignKey(x => x.ListingId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasQueryFilter(x => !x.Listing.IsDeleted);
    }
}
