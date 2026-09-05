using Shared.Constants;
using Shared.DTOs.JobOpportunities;
using Shared.DTOs.JobRequests;
using Shared.Enums;

namespace Services.JobModules;

public static class JobModuleExamples
{
    private static readonly Guid JobRequestId = new("66666666-6666-6666-6666-666666666666");
    private static readonly Guid JobOpportunityId = new("77777777-7777-7777-7777-777777777777");
    private static readonly Guid ImageId = new("88888888-8888-8888-8888-888888888888");
    private static readonly Guid OwnerId = new("99999999-9999-9999-9999-999999999999");

    private static readonly DateTime CreatedAt = new(2026, 7, 20, 9, 30, 0, DateTimeKind.Utc);

    private const string Center = "سنورس";

    public static JobRequestDetailsDto JobRequestDetails() => new()
    {
        Id = JobRequestId,
        OwnerId = OwnerId.ToString(),
        ApplicantName = "أحمد محمود عبد العزيز",
        Phone = "01012345678",
        WhatsApp = "01087654321",
        JobField = JobField.CivilEngineer,
        JobFieldName = JobsCatalog.GetJobFieldName(JobField.CivilEngineer),
        JobFieldGroup = JobsCatalog.GetJobFieldGroup(JobField.CivilEngineer),
        JobFieldGroupAr = JobsCatalog.GetJobFieldGroupAr(JobField.CivilEngineer),
        OtherJobField = null,
        Experience = JobExperienceLevel.MoreThanFiveYears,
        ExperienceName = JobsCatalog.GetExperienceLevelName(JobExperienceLevel.MoreThanFiveYears),
        Education = EducationLevel.Bachelor,
        EducationName = JobsCatalog.GetEducationLevelName(EducationLevel.Bachelor),
        Skills = "Excel، AutoCAD، English، Communication، Team Management",
        Governorate = LocationConstants.Governorate,
        Center = Center,
        Address = "شارع الجمهورية، بجوار مدرسة سنورس الثانوية",
        ProfileImageUrl = "https://api.example.com/uploads/job-requests/sample.jpg",
        CvFileUrl = "https://api.example.com/uploads/job-requests-cv/sample.pdf",
        CvFileName = "ahmed-cv.pdf",
        IntroVideoUrl = "https://api.example.com/uploads/job-requests-video/sample.mp4",
        Title = "مهندس مدني خبرة 5 سنوات يبحث عن عمل",
        Description =
            "نبذة مختصرة: مهندس مدني خريج كلية الهندسة جامعة الفيوم.\n" +
            "الخبرات السابقة: 6 سنوات في الإشراف على المشروعات السكنية والخرسانة المسلحة.\n" +
            "الشركات السابقة: شركة النيل للمقاولات، مكتب الهندسة الحديثة.\n" +
            "المهارات: AutoCAD، Revit، إدارة المواقع، حصر الكميات.\n" +
            "الشهادات: عضوية نقابة المهندسين.\n" +
            "الدورات التدريبية: إدارة المشروعات PMP، السلامة والصحة المهنية.\n" +
            "الوظيفة المطلوبة: مهندس موقع أو مهندس تنفيذ داخل محافظة الفيوم.\n" +
            "ملاحظات إضافية: مستعد للعمل فورًا، ولدي رخصة قيادة سارية.",
        CreatedAt = CreatedAt,
        UpdatedAt = null
    };

    public static JobRequestListItemDto JobRequestListItem()
    {
        var details = JobRequestDetails();

        return new JobRequestListItemDto
        {
            Id = details.Id,
            ApplicantName = details.ApplicantName,
            Phone = details.Phone,
            WhatsApp = details.WhatsApp,
            JobField = details.JobField,
            JobFieldName = details.JobFieldName,
            JobFieldGroup = details.JobFieldGroup,
            JobFieldGroupAr = details.JobFieldGroupAr,
            OtherJobField = details.OtherJobField,
            Experience = details.Experience,
            ExperienceName = details.ExperienceName,
            Education = details.Education,
            EducationName = details.EducationName,
            Skills = details.Skills,
            Governorate = details.Governorate,
            Center = details.Center,
            ProfileImageUrl = details.ProfileImageUrl,
            CvFileUrl = details.CvFileUrl,
            IntroVideoUrl = details.IntroVideoUrl,
            HasIntroVideo = details.IntroVideoUrl is not null,
            Title = details.Title,
            Description = details.Description,
            CreatedAt = details.CreatedAt
        };
    }

    public static JobOpportunityDetailsDto JobOpportunityDetails() => new()
    {
        Id = JobOpportunityId,
        OwnerId = OwnerId.ToString(),
        EmployerName = "شركة النيل للمقاولات",
        Phone = "01112345678",
        WhatsApp = "01187654321",
        JobTitle = "مطلوب محاسب",
        JobField = JobField.Accountant,
        JobFieldName = JobsCatalog.GetJobFieldName(JobField.Accountant),
        JobFieldGroup = JobsCatalog.GetJobFieldGroup(JobField.Accountant),
        JobFieldGroupAr = JobsCatalog.GetJobFieldGroupAr(JobField.Accountant),
        OtherJobField = null,
        RequiredExperience = JobExperienceLevel.OneToThreeYears,
        RequiredExperienceName = JobsCatalog.GetExperienceLevelName(JobExperienceLevel.OneToThreeYears),
        WorkType = WorkType.FullTime,
        WorkTypeName = JobsCatalog.GetWorkTypeName(WorkType.FullTime),
        SalaryType = SalaryType.Specified,
        SalaryTypeName = JobsCatalog.GetSalaryTypeName(SalaryType.Specified),
        Salary = 8000m,
        Governorate = LocationConstants.Governorate,
        Center = Center,
        Address = "شارع النصر، المبنى الإداري، الدور الثالث",
        GoogleMaps = "https://maps.app.goo.gl/example",
        LogoUrl = "https://api.example.com/uploads/job-opportunities/logo.png",
        Title = "مطلوب محاسب لشركة مقاولات بالفيوم",
        Description =
            "المهام والمسؤوليات: إعداد القيود اليومية، متابعة الحسابات المالية، إعداد التقارير الشهرية.\n" +
            "المتطلبات: بكالوريوس تجارة، خبرة من سنة إلى 3 سنوات، إجادة Excel وبرامج المحاسبة.\n" +
            "ساعات العمل: من 9 صباحًا حتى 5 مساءً، الجمعة إجازة.\n" +
            "مكان العمل: مركز سنورس، محافظة الفيوم.",
        Images =
        [
            new JobOpportunityImageDto
            {
                Id = ImageId,
                Url = "https://api.example.com/uploads/job-opportunities/sample.jpg",
                IsPrimary = true
            }
        ],
        CreatedAt = CreatedAt,
        UpdatedAt = null
    };

    public static JobOpportunityListItemDto JobOpportunityListItem()
    {
        var details = JobOpportunityDetails();

        return new JobOpportunityListItemDto
        {
            Id = details.Id,
            EmployerName = details.EmployerName,
            Phone = details.Phone,
            WhatsApp = details.WhatsApp,
            JobTitle = details.JobTitle,
            JobField = details.JobField,
            JobFieldName = details.JobFieldName,
            JobFieldGroup = details.JobFieldGroup,
            JobFieldGroupAr = details.JobFieldGroupAr,
            OtherJobField = details.OtherJobField,
            RequiredExperience = details.RequiredExperience,
            RequiredExperienceName = details.RequiredExperienceName,
            WorkType = details.WorkType,
            WorkTypeName = details.WorkTypeName,
            SalaryType = details.SalaryType,
            SalaryTypeName = details.SalaryTypeName,
            Salary = details.Salary,
            Governorate = details.Governorate,
            Center = details.Center,
            LogoUrl = details.LogoUrl,
            Title = details.Title,
            Description = details.Description,
            PrimaryImageUrl = details.Images[0].Url,
            Images = details.Images,
            CreatedAt = details.CreatedAt
        };
    }

    public static IReadOnlyList<JobFieldDto> JobFields() =>
        JobsCatalog.JobFields
            .Where(entry => entry.Value is JobField.Manager
                or JobField.Programmer
                or JobField.CivilEngineer
                or JobField.Doctor
                or JobField.Teacher
                or JobField.Electrician
                or JobField.Chef
                or JobField.AgriculturalEngineer
                or JobField.Other)
            .Select(entry => new JobFieldDto
            {
                Id = (int)entry.Value,
                Group = entry.Group,
                GroupAr = entry.GroupAr,
                Name = entry.Name,
                NameEn = entry.NameEn
            })
            .ToList();

    public static IReadOnlyList<JobExperienceLevelDto> ExperienceLevels() =>
        JobsCatalog.ExperienceLevelNames
            .Select(entry => new JobExperienceLevelDto
            {
                Id = (int)entry.Key,
                Name = entry.Value,
                NameEn = JobsCatalog.ExperienceLevelNamesEn[entry.Key]
            })
            .ToList();

    public static IReadOnlyList<EducationLevelDto> EducationLevels() =>
        JobsCatalog.EducationLevelNames
            .Select(entry => new EducationLevelDto
            {
                Id = (int)entry.Key,
                Name = entry.Value,
                NameEn = JobsCatalog.EducationLevelNamesEn[entry.Key]
            })
            .ToList();

    public static IReadOnlyList<WorkTypeDto> WorkTypes() =>
        JobsCatalog.WorkTypeNames
            .Select(entry => new WorkTypeDto
            {
                Id = (int)entry.Key,
                Name = entry.Value,
                NameEn = JobsCatalog.WorkTypeNamesEn[entry.Key]
            })
            .ToList();

    public static IReadOnlyList<SalaryTypeDto> SalaryTypes() =>
        JobsCatalog.SalaryTypeNames
            .Select(entry => new SalaryTypeDto
            {
                Id = (int)entry.Key,
                Name = entry.Value,
                NameEn = JobsCatalog.SalaryTypeNamesEn[entry.Key]
            })
            .ToList();
}
