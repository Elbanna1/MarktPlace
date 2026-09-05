using Shared.Constants;
using Shared.DTOs.Lookups.Forms;
using Shared.Enums;
using static Services.AdForms.AdFormFieldFactory;

namespace Services.AdForms;

internal static class CharityFormSchemas
{
    private const string SectionRescue = "بيانات الاستغاثة";
    private const string SectionBlood = "بيانات طلب الدم";
    private const string SectionQuestion = "بيانات السؤال";
    private const string SectionLocation = "الموقع";
    private const string SectionContact = "التواصل";
    private const string SectionImages = "الصور";
    private const string SectionResponsibility = "المسؤولية والموافقة";

    private static readonly CreateAdFormSubmitDto RescuesSubmit = new("/api/rescues");
    private static readonly CreateAdFormSubmitDto BloodRequestsSubmit = new("/api/blood-requests");
    private static readonly CreateAdFormSubmitDto AskConsultsSubmit = new("/api/ask-consults");

    public static AdFormSchema? For(SubCategoryType subCategory) =>
        subCategory switch
        {
            SubCategoryType.Rescues => Rescues(),
            SubCategoryType.BloodRequests => BloodRequests(),
            SubCategoryType.AskConsults => AskConsults(),
            _ => null
        };

    private static AdFormSchema Rescues()
    {
        var fields = new List<FormFieldDto>
        {
            Text("RescuerName", "اسم المستغيث", "Rescuer name", SectionRescue,
                required: true, maxLength: 150, placeholder: "مثال: أحمد محمود"),

            PhoneNumber("Phone"),

            Text("Address", "العنوان / مكان الاستغاثة", "Address", SectionRescue,
                required: true, maxLength: 300,
                placeholder: "مثال: طريق الفيوم - القاهرة أمام بوابة الرسوم"),

            LocationPicker(required: true,
                confirmation: "تم تحديد موقع الاستغاثة"),

            TextArea("Details", "تفاصيل الاستغاثة", "Details", SectionRescue,
                required: true, maxLength: 4000,
                placeholder: "مثال: سيارة متعطلة وتحتاج ونش، والمستغيث موجود بجوار المكان المحدد."),

            CharityImages("صورة الاستغاثة", "Rescue image"),

            Responsibility(CharityCatalog.RescueResponsibilityDeclaration)
        };

        return new AdFormSchema("rescues", RescuesSubmit, Array.Empty<string>(), Order(fields));
    }

    private static AdFormSchema BloodRequests()
    {
        var fields = new List<FormFieldDto>
        {
            Text("RequesterName", "اسم طالب الدم", "Requester name", SectionBlood,
                required: true, maxLength: 150),

            PhoneNumber("Phone"),

            EnumOptions("BloodGroup", "فصيلة الدم المطلوبة", "Blood group", SectionBlood,
                CharityCatalog.BloodGroups.Select(entry => ((int)entry.Id, entry.Name, entry.NameEn)),
                required: true),

            Governorate(),
            Center(),

            Text("HospitalName", "اسم المستشفى / المكان", "Hospital name", SectionBlood,
                required: true, maxLength: 200),

            Text("Address", "العنوان / مكان التبرع", "Address", SectionBlood,
                required: true, maxLength: 300),

            LocationPicker(required: true, confirmation: "تم تحديد مكان التبرع"),

            TextArea("Details", "تفاصيل الطلب", "Details", SectionBlood,
                required: true, maxLength: 4000),

            CharityImages("صورة أو مستند", "Image or document"),

            Responsibility(CharityCatalog.BloodRequestResponsibilityDeclaration)
        };

        return new AdFormSchema("bloodRequests", BloodRequestsSubmit,
            new[] { AdFormLookupKeys.Governorates, AdFormLookupKeys.Centers }, Order(fields));
    }

    private static AdFormSchema AskConsults()
    {
        var fields = new List<FormFieldDto>
        {
            EnumOptions("Category", "مجال السؤال", "Question field", SectionQuestion,
                CharityCatalog.AskConsultCategories
                    .Select(entry => ((int)entry.Id, entry.Name, entry.NameEn)),
                required: true),

            Text("OtherCategory", "اكتب المجال", "Other field", SectionQuestion,
                    required: false, maxLength: 150)
                .VisibleOnlyWhen("Category", (int)AskConsultCategory.Other)
                .MandatoryWhen("Category", (int)AskConsultCategory.Other),

            Text("AskerName", "اسم صاحب السؤال", "Asker name", SectionQuestion,
                required: true, maxLength: 150),

            Text("Title", "عنوان السؤال", "Question title", SectionQuestion,
                required: true, maxLength: 150),

            TextArea("Question", "السؤال / الاستشارة", "Question", SectionQuestion,
                required: true, maxLength: 4000),

            PhoneNumber("Phone"),

            CharityImages("الصور", "Images",
                maxFiles: CharityCatalog.MaxAskConsultImages, multiple: true),

            Responsibility(CharityCatalog.AskConsultResponsibilityDeclaration)
        };

        return new AdFormSchema("askConsults", AskConsultsSubmit, Array.Empty<string>(), Order(fields));
    }

    private static FormFieldDto LocationPicker(bool required, string confirmation) =>
        new()
        {
            Name = "Location",
            Label = "تحديد الموقع",
            LabelEn = "Pick location",
            Type = FormFieldTypes.Location,
            Required = required,
            Section = SectionLocation,
            WritesFields = new List<string> { "Latitude", "Longitude" },
            HelpText = required
                ? $"افتح الخريطة وحدد الموقع. بعد التحديد: ✓ {confirmation}"
                : $"اختياري — يمكنك فتح الخريطة وتحديد الموقع. بعد التحديد: ✓ {confirmation}"
        };

    private static FormFieldDto Responsibility(string declaration) =>
        new()
        {
            Name = "IsResponsibilityAccepted",
            Label = declaration,
            LabelEn = "I accept responsibility for this post",
            Type = FormFieldTypes.Checkbox,
            Required = true,
            Section = SectionResponsibility,
            DefaultValue = false,
            HelpText = CharityCatalog.ResponsibilityRequiredMessage
        };

    private static FormFieldDto CharityImages(
        string label, string labelEn, int maxFiles = CharityCatalog.MaxCharityImages, bool multiple = true) =>
        new()
        {
            Name = "Images",
            Label = label,
            LabelEn = labelEn,
            Type = FormFieldTypes.Image,
            Required = false,
            Section = SectionImages,
            Multiple = multiple,
            MaxFiles = maxFiles,
            MaxSizeMb = (int)(ImageConstants.MaxFileSizeBytes / (1024 * 1024)),
            AllowedExtensions = ImageConstants.AllowedExtensions
                .Select(extension => extension.TrimStart('.')).ToList(),
            HelpText = "اختياري."
        };

    private static FormFieldDto Governorate()
    {
        var field = AdFormFieldFactory.Governorate();
        field.Section = SectionLocation;
        field.Required = true;
        return field;
    }

    private static FormFieldDto Center()
    {
        var field = AdFormFieldFactory.Center();
        field.Section = SectionLocation;
        field.Required = true;
        return field;
    }

    private static FormFieldDto EnumOptions(
        string name, string label, string labelEn, string section,
        IEnumerable<(int Value, string Name, string NameEn)> options, bool required) =>
        new()
        {
            Name = name,
            Label = label,
            LabelEn = labelEn,
            Type = FormFieldTypes.Select,
            Required = required,
            Section = section,
            Options = options
                .Select(option => new FormFieldOptionDto(option.Value, option.Name, option.NameEn))
                .ToList()
        };

    private static IReadOnlyList<FormFieldDto> Order(List<FormFieldDto> fields)
    {
        for (var position = 0; position < fields.Count; position++)
            fields[position].Order = position + 1;

        return fields;
    }
}
