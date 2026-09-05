using Shared.Enums;

namespace Shared.Constants;

public static class JobsCatalog
{
    public readonly record struct JobFieldEntry(
        JobField Value, string Group, string GroupAr, string Name, string NameEn);

    public const string GroupAdministration = "Administration & Business";
    public const string GroupTechnology = "Programming & Technology";
    public const string GroupEngineering = "Engineering";
    public const string GroupMedical = "Medicine & Health";
    public const string GroupEducation = "Education";
    public const string GroupCrafts = "Crafts & Trades";
    public const string GroupHospitality = "Restaurants & Hotels";
    public const string GroupAgriculture = "Agriculture";
    public const string GroupOther = "Other";

    private const string GroupAdministrationAr = "الإدارة والأعمال";
    private const string GroupTechnologyAr = "البرمجة والتكنولوجيا";
    private const string GroupEngineeringAr = "الهندسة";
    private const string GroupMedicalAr = "الطب والصحة";
    private const string GroupEducationAr = "التعليم";
    private const string GroupCraftsAr = "الحرف والمهن";
    private const string GroupHospitalityAr = "المطاعم والفنادق";
    private const string GroupAgricultureAr = "الزراعة";
    private const string GroupOtherAr = "أخرى";

    public static readonly IReadOnlyList<JobFieldEntry> JobFields = new List<JobFieldEntry>
    {
        new(JobField.Manager, GroupAdministration, GroupAdministrationAr, "مدير", "Manager"),
        new(JobField.Secretary, GroupAdministration, GroupAdministrationAr, "سكرتير", "Secretary"),
        new(JobField.AdministrativeEmployee, GroupAdministration, GroupAdministrationAr, "موظف إداري", "Administrative Employee"),
        new(JobField.CustomerService, GroupAdministration, GroupAdministrationAr, "خدمة عملاء", "Customer Service"),
        new(JobField.Sales, GroupAdministration, GroupAdministrationAr, "مبيعات", "Sales"),
        new(JobField.Accountant, GroupAdministration, GroupAdministrationAr, "محاسب", "Accountant"),
        new(JobField.HumanResources, GroupAdministration, GroupAdministrationAr, "موارد بشرية", "Human Resources"),

        new(JobField.Programmer, GroupTechnology, GroupTechnologyAr, "مبرمج", "Programmer"),
        new(JobField.WebDeveloper, GroupTechnology, GroupTechnologyAr, "مطور مواقع", "Web Developer"),
        new(JobField.ApplicationDeveloper, GroupTechnology, GroupTechnologyAr, "مطور تطبيقات", "Application Developer"),
        new(JobField.UiUxDesigner, GroupTechnology, GroupTechnologyAr, "مصمم UI/UX", "UI/UX Designer"),
        new(JobField.TechnicalSupport, GroupTechnology, GroupTechnologyAr, "دعم فني", "Technical Support"),
        new(JobField.Networks, GroupTechnology, GroupTechnologyAr, "شبكات", "Networks"),

        new(JobField.CivilEngineer, GroupEngineering, GroupEngineeringAr, "مهندس مدني", "Civil Engineer"),
        new(JobField.ArchitecturalEngineer, GroupEngineering, GroupEngineeringAr, "مهندس معماري", "Architectural Engineer"),
        new(JobField.ElectricalEngineer, GroupEngineering, GroupEngineeringAr, "مهندس كهرباء", "Electrical Engineer"),
        new(JobField.MechanicalEngineer, GroupEngineering, GroupEngineeringAr, "مهندس ميكانيكا", "Mechanical Engineer"),
        new(JobField.EngineeringTechnician, GroupEngineering, GroupEngineeringAr, "فني هندسي", "Engineering Technician"),

        new(JobField.Doctor, GroupMedical, GroupMedicalAr, "طبيب", "Doctor"),
        new(JobField.Nurse, GroupMedical, GroupMedicalAr, "ممرض", "Nurse"),
        new(JobField.Pharmacist, GroupMedical, GroupMedicalAr, "صيدلي", "Pharmacist"),
        new(JobField.MedicalAssistant, GroupMedical, GroupMedicalAr, "مساعد طبي", "Medical Assistant"),

        new(JobField.Teacher, GroupEducation, GroupEducationAr, "مدرس", "Teacher"),
        new(JobField.PrivateTutor, GroupEducation, GroupEducationAr, "مدرس خصوصي", "Private Tutor"),
        new(JobField.Lecturer, GroupEducation, GroupEducationAr, "محاضر", "Lecturer"),
        new(JobField.EducationAdministrator, GroupEducation, GroupEducationAr, "إداري تعليم", "Education Administrator"),

        new(JobField.Electrician, GroupCrafts, GroupCraftsAr, "كهربائي", "Electrician"),
        new(JobField.Plumber, GroupCrafts, GroupCraftsAr, "سباك", "Plumber"),
        new(JobField.Carpenter, GroupCrafts, GroupCraftsAr, "نجار", "Carpenter"),
        new(JobField.Blacksmith, GroupCrafts, GroupCraftsAr, "حداد", "Blacksmith"),
        new(JobField.AirConditioningTechnician, GroupCrafts, GroupCraftsAr, "فني تكييف", "Air Conditioning Technician"),
        new(JobField.Driver, GroupCrafts, GroupCraftsAr, "سائق", "Driver"),
        new(JobField.ProductionWorker, GroupCrafts, GroupCraftsAr, "عامل إنتاج", "Production Worker"),

        new(JobField.Chef, GroupHospitality, GroupHospitalityAr, "شيف", "Chef"),
        new(JobField.AssistantChef, GroupHospitality, GroupHospitalityAr, "مساعد شيف", "Assistant Chef"),
        new(JobField.Waiter, GroupHospitality, GroupHospitalityAr, "ويتر", "Waiter"),
        new(JobField.Cashier, GroupHospitality, GroupHospitalityAr, "كاشير", "Cashier"),
        new(JobField.RestaurantWorker, GroupHospitality, GroupHospitalityAr, "عامل مطعم", "Restaurant Worker"),

        new(JobField.AgriculturalEngineer, GroupAgriculture, GroupAgricultureAr, "مهندس زراعي", "Agricultural Engineer"),
        new(JobField.FarmWorker, GroupAgriculture, GroupAgricultureAr, "عامل مزرعة", "Farm Worker"),
        new(JobField.AgriculturalSupervisor, GroupAgriculture, GroupAgricultureAr, "مشرف زراعي", "Agricultural Supervisor"),

        new(JobField.Other, GroupOther, GroupOtherAr, "أخرى", "Other")
    };

    private static readonly IReadOnlyDictionary<JobField, JobFieldEntry> JobFieldsByValue =
        JobFields.ToDictionary(entry => entry.Value);

    public static string GetJobFieldName(JobField value) =>
        JobFieldsByValue.TryGetValue(value, out var entry) ? entry.Name : string.Empty;

    public static string GetJobFieldGroup(JobField value) =>
        JobFieldsByValue.TryGetValue(value, out var entry) ? entry.Group : string.Empty;

    public static string GetJobFieldGroupAr(JobField value) =>
        JobFieldsByValue.TryGetValue(value, out var entry) ? entry.GroupAr : string.Empty;

    public static readonly IReadOnlyDictionary<JobExperienceLevel, string> ExperienceLevelNames =
        new Dictionary<JobExperienceLevel, string>
        {
            [JobExperienceLevel.None] = "بدون خبرة",
            [JobExperienceLevel.LessThanOneYear] = "أقل من سنة",
            [JobExperienceLevel.OneToThreeYears] = "1 - 3 سنوات",
            [JobExperienceLevel.ThreeToFiveYears] = "3 - 5 سنوات",
            [JobExperienceLevel.MoreThanFiveYears] = "أكثر من 5 سنوات"
        };

    public static readonly IReadOnlyDictionary<JobExperienceLevel, string> ExperienceLevelNamesEn =
        new Dictionary<JobExperienceLevel, string>
        {
            [JobExperienceLevel.None] = "No experience",
            [JobExperienceLevel.LessThanOneYear] = "Less than a year",
            [JobExperienceLevel.OneToThreeYears] = "1 - 3 years",
            [JobExperienceLevel.ThreeToFiveYears] = "3 - 5 years",
            [JobExperienceLevel.MoreThanFiveYears] = "More than 5 years"
        };

    public static string GetExperienceLevelName(JobExperienceLevel value) =>
        ExperienceLevelNames.TryGetValue(value, out var name) ? name : string.Empty;

    public static readonly IReadOnlyDictionary<EducationLevel, string> EducationLevelNames =
        new Dictionary<EducationLevel, string>
        {
            [EducationLevel.None] = "بدون مؤهل",
            [EducationLevel.Diploma] = "دبلوم",
            [EducationLevel.HighSchool] = "ثانوية عامة",
            [EducationLevel.Institute] = "معهد",
            [EducationLevel.Bachelor] = "بكالوريوس",
            [EducationLevel.Master] = "ماجستير",
            [EducationLevel.Doctorate] = "دكتوراه",
            [EducationLevel.Other] = "أخرى"
        };

    public static readonly IReadOnlyDictionary<EducationLevel, string> EducationLevelNamesEn =
        new Dictionary<EducationLevel, string>
        {
            [EducationLevel.None] = "No qualification",
            [EducationLevel.Diploma] = "Diploma",
            [EducationLevel.HighSchool] = "High school",
            [EducationLevel.Institute] = "Institute",
            [EducationLevel.Bachelor] = "Bachelor",
            [EducationLevel.Master] = "Master",
            [EducationLevel.Doctorate] = "Doctorate",
            [EducationLevel.Other] = "Other"
        };

    public static string GetEducationLevelName(EducationLevel value) =>
        EducationLevelNames.TryGetValue(value, out var name) ? name : string.Empty;

    public static readonly IReadOnlyDictionary<WorkType, string> WorkTypeNames =
        new Dictionary<WorkType, string>
        {
            [WorkType.FullTime] = "دوام كامل",
            [WorkType.PartTime] = "دوام جزئي",
            [WorkType.Freelance] = "عمل حر",
            [WorkType.Internship] = "تدريب"
        };

    public static readonly IReadOnlyDictionary<WorkType, string> WorkTypeNamesEn =
        new Dictionary<WorkType, string>
        {
            [WorkType.FullTime] = "Full time",
            [WorkType.PartTime] = "Part time",
            [WorkType.Freelance] = "Freelance",
            [WorkType.Internship] = "Internship"
        };

    public static string GetWorkTypeName(WorkType value) =>
        WorkTypeNames.TryGetValue(value, out var name) ? name : string.Empty;

    public static readonly IReadOnlyDictionary<SalaryType, string> SalaryTypeNames =
        new Dictionary<SalaryType, string>
        {
            [SalaryType.Negotiable] = "قابل للتفاوض",
            [SalaryType.Specified] = "تحديد الراتب"
        };

    public static readonly IReadOnlyDictionary<SalaryType, string> SalaryTypeNamesEn =
        new Dictionary<SalaryType, string>
        {
            [SalaryType.Negotiable] = "Negotiable",
            [SalaryType.Specified] = "Specified"
        };

    public static string GetSalaryTypeName(SalaryType value) =>
        SalaryTypeNames.TryGetValue(value, out var name) ? name : string.Empty;
}
