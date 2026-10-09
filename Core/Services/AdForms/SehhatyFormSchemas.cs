using Shared.Constants;
using Shared.DTOs.Lookups.Forms;
using Shared.Enums;
using static Services.AdForms.AdFormFieldFactory;

namespace Services.AdForms;

internal static class SehhatyFormSchemas
{
    public static AdFormSchema? For(SubCategoryType subCategory) =>
        SehhatyCatalog.ModuleOf(subCategory) is { } module
            ? module.Type == ListingModuleType.Doctor ? Doctors(module) : Facility(module)
            : null;

    private static AdFormSchema Doctors(SehhatyModule module)
    {
        var fields = new List<FormFieldDto>
        {
            NameField(module),

            new FormFieldDto
                {
                    Name = "Specialty",
                    Label = SehhatyCatalog.SpecialtyLabel,
                    LabelEn = "Specialty",
                    Type = FormFieldTypes.Select,
                    Required = true,
                    Section = module.Section,
                    Placeholder = "اختار تخصص الطبيب",
                    Options = SehhatyCatalog.Specialties
                        .Select(entry => new FormFieldOptionDto((int)entry.Id, entry.Name, entry.NameEn))
                        .ToList()
                }
                .AsSearchable(),

            Text("OtherSpecialty", SehhatyCatalog.OtherSpecialtyLabel, "Other specialty", module.Section,
                    required: false, maxLength: SehhatyCatalog.MaxOtherSpecialtyLength,
                    placeholder: "مثال: طب الأعشاب")
                .VisibleOnlyWhen("Specialty", (int)DoctorSpecialty.Other)
                .MandatoryWhen("Specialty", (int)DoctorSpecialty.Other)
        };

        fields.AddRange(LocationFields());
        fields.AddRange(ContactFields());
        fields.AddRange(AdvertisementFields(module));

        return new AdFormSchema(module.Key, Submit(module), Array.Empty<string>(), Order(fields));
    }

    private static AdFormSchema Facility(SehhatyModule module)
    {
        var fields = new List<FormFieldDto> { NameField(module) };

        fields.AddRange(AdvertisementFields(module));
        fields.AddRange(ContactFields());
        fields.AddRange(LocationFields());

        return new AdFormSchema(module.Key, Submit(module), Array.Empty<string>(), Order(fields));
    }

    private static CreateAdFormSubmitDto Submit(SehhatyModule module) => new(module.PublicRoute);

    private static FormFieldDto NameField(SehhatyModule module) =>
        Text("Name", module.NameLabel, module.NameLabelEn, module.Section,
            required: true, maxLength: SehhatyCatalog.MaxNameLength, placeholder: module.NamePlaceholder);

    private static IEnumerable<FormFieldDto> LocationFields() =>
    [
        Address(),
        GoogleMapsUrl()
    ];

    private static IEnumerable<FormFieldDto> ContactFields()
    {
        var whatsApp = PhoneNumber("WhatsApp", "رقم الواتساب", "WhatsApp number", required: false);
        whatsApp.HelpText = "اختياري.";

        var email = Email();
        email.HelpText = "اختياري.";

        return [PhoneNumber("Phone"), whatsApp, email];
    }

    private static IEnumerable<FormFieldDto> AdvertisementFields(SehhatyModule module)
    {
        var title = Title();
        title.Placeholder = module.TitlePlaceholder;

        var description = TextArea("Description", "وصف الإعلان", "Advertisement description", SectionAd,
            required: true, maxLength: SehhatyCatalog.MaxDescriptionLength,
            placeholder: module.DescriptionPlaceholder);
        description.HelpText = module.DescriptionPlaceholder;

        return [title, description, SehhatyCatalog.ImagesRequired ? RequiredImages() : Images()];
    }

    private static IReadOnlyList<FormFieldDto> Order(List<FormFieldDto> fields)
    {
        for (var position = 0; position < fields.Count; position++)
            fields[position].Order = position + 1;

        return fields;
    }
}
