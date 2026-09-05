using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Persistence.Configurations;

public class JobFieldLookupConfiguration : IEntityTypeConfiguration<JobFieldLookup>
{
    public void Configure(EntityTypeBuilder<JobFieldLookup> builder)
    {
        builder.ToTable("JobFields");

        builder.HasKey(j => j.Id);
        builder.Property(j => j.Id).ValueGeneratedNever();
        builder.Property(j => j.Group).IsRequired().HasMaxLength(100);
        builder.Property(j => j.GroupAr).IsRequired().HasMaxLength(100);
        builder.Property(j => j.Name).IsRequired().HasMaxLength(150);
        builder.Property(j => j.NameEn).IsRequired().HasMaxLength(150);

        builder.HasIndex(j => new { j.Group, j.Name }).IsUnique();

        builder.HasData(JobsCatalog.JobFields.Select(entry => new JobFieldLookup
        {
            Id = (int)entry.Value,
            Group = entry.Group,
            GroupAr = entry.GroupAr,
            Name = entry.Name,
            NameEn = entry.NameEn
        }));
    }
}

public class JobExperienceLevelLookupConfiguration : IEntityTypeConfiguration<JobExperienceLevelLookup>
{
    public void Configure(EntityTypeBuilder<JobExperienceLevelLookup> builder)
    {
        builder.ToTable("JobExperienceLevels");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();
        builder.Property(e => e.Name).IsRequired().HasMaxLength(100);
        builder.Property(e => e.NameEn).IsRequired().HasMaxLength(100);

        builder.HasIndex(e => e.Name).IsUnique();

        builder.HasData(JobsCatalog.ExperienceLevelNames.Select(entry => new JobExperienceLevelLookup
        {
            Id = (int)entry.Key,
            Name = entry.Value,
            NameEn = JobsCatalog.ExperienceLevelNamesEn[entry.Key]
        }));
    }
}

public class EducationLevelLookupConfiguration : IEntityTypeConfiguration<EducationLevelLookup>
{
    public void Configure(EntityTypeBuilder<EducationLevelLookup> builder)
    {
        builder.ToTable("EducationLevels");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();
        builder.Property(e => e.Name).IsRequired().HasMaxLength(100);
        builder.Property(e => e.NameEn).IsRequired().HasMaxLength(100);

        builder.HasIndex(e => e.Name).IsUnique();

        builder.HasData(JobsCatalog.EducationLevelNames.Select(entry => new EducationLevelLookup
        {
            Id = (int)entry.Key,
            Name = entry.Value,
            NameEn = JobsCatalog.EducationLevelNamesEn[entry.Key]
        }));
    }
}

public class WorkTypeLookupConfiguration : IEntityTypeConfiguration<WorkTypeLookup>
{
    public void Configure(EntityTypeBuilder<WorkTypeLookup> builder)
    {
        builder.ToTable("WorkTypes");

        builder.HasKey(w => w.Id);
        builder.Property(w => w.Id).ValueGeneratedNever();
        builder.Property(w => w.Name).IsRequired().HasMaxLength(100);
        builder.Property(w => w.NameEn).IsRequired().HasMaxLength(100);

        builder.HasIndex(w => w.Name).IsUnique();

        builder.HasData(JobsCatalog.WorkTypeNames.Select(entry => new WorkTypeLookup
        {
            Id = (int)entry.Key,
            Name = entry.Value,
            NameEn = JobsCatalog.WorkTypeNamesEn[entry.Key]
        }));
    }
}

public class SalaryTypeLookupConfiguration : IEntityTypeConfiguration<SalaryTypeLookup>
{
    public void Configure(EntityTypeBuilder<SalaryTypeLookup> builder)
    {
        builder.ToTable("SalaryTypes");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).ValueGeneratedNever();
        builder.Property(s => s.Name).IsRequired().HasMaxLength(100);
        builder.Property(s => s.NameEn).IsRequired().HasMaxLength(100);

        builder.HasIndex(s => s.Name).IsUnique();

        builder.HasData(JobsCatalog.SalaryTypeNames.Select(entry => new SalaryTypeLookup
        {
            Id = (int)entry.Key,
            Name = entry.Value,
            NameEn = JobsCatalog.SalaryTypeNamesEn[entry.Key]
        }));
    }
}
