using Shared.Constants;
using Shared.DTOs.JobOpportunities;
using Shared.DTOs.JobRequests;
using Shared.DTOs.Lookups.Forms;

namespace Services.AdForms;

internal static class JobFormLookups
{
    public static IReadOnlyList<JobFieldDto> JobFieldOptions { get; } =
        JobsCatalog.JobFields
            .Select(entry => new JobFieldDto
            {
                Id = (int)entry.Value,
                Group = entry.Group,
                GroupAr = entry.GroupAr,
                Name = entry.Name,
                NameEn = entry.NameEn
            })
            .ToList();

    public static IReadOnlyList<JobExperienceLevelDto> ExperienceLevelOptions { get; } =
        JobsCatalog.ExperienceLevelNames
            .Select(entry => new JobExperienceLevelDto
            {
                Id = (int)entry.Key,
                Name = entry.Value,
                NameEn = JobsCatalog.ExperienceLevelNamesEn[entry.Key]
            })
            .ToList();

    public static IReadOnlyList<EducationLevelDto> EducationLevelOptions { get; } =
        JobsCatalog.EducationLevelNames
            .Select(entry => new EducationLevelDto
            {
                Id = (int)entry.Key,
                Name = entry.Value,
                NameEn = JobsCatalog.EducationLevelNamesEn[entry.Key]
            })
            .ToList();

    public static IReadOnlyList<WorkTypeDto> WorkTypeOptions { get; } =
        JobsCatalog.WorkTypeNames
            .Select(entry => new WorkTypeDto
            {
                Id = (int)entry.Key,
                Name = entry.Value,
                NameEn = JobsCatalog.WorkTypeNamesEn[entry.Key]
            })
            .ToList();

    public static IReadOnlyList<SalaryTypeDto> SalaryTypeOptions { get; } =
        JobsCatalog.SalaryTypeNames
            .Select(entry => new SalaryTypeDto
            {
                Id = (int)entry.Key,
                Name = entry.Value,
                NameEn = JobsCatalog.SalaryTypeNamesEn[entry.Key]
            })
            .ToList();

    public static bool TryApply(CreateAdFormLookupsDto lookups, string key)
    {
        switch (key)
        {
            case AdFormLookupKeys.JobFields:
                lookups.JobFields = JobFieldOptions;
                return true;
            case AdFormLookupKeys.JobExperienceLevels:
                lookups.JobExperienceLevels = ExperienceLevelOptions;
                return true;
            case AdFormLookupKeys.EducationLevels:
                lookups.EducationLevels = EducationLevelOptions;
                return true;
            case AdFormLookupKeys.WorkTypes:
                lookups.WorkTypes = WorkTypeOptions;
                return true;
            case AdFormLookupKeys.SalaryTypes:
                lookups.SalaryTypes = SalaryTypeOptions;
                return true;
            default:
                return false;
        }
    }
}
