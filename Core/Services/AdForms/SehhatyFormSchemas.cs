using Shared.Constants;
using Shared.DTOs.Lookups.Forms;
using Shared.Enums;
using static Services.AdForms.AdFormFieldFactory;

namespace Services.AdForms;

internal static class SehhatyFormSchemas
{
    public static AdFormSchema? For(SubCategoryType subCategory) =>
        SehhatyCatalog.ModuleOf(subCategory) is { } module
            ? module.Type == ListingModuleType.Doctor ? Doctors(module) : DirectoryFormSchemas.Facility(module)
            : null;

    private static AdFormSchema Doctors(DirectoryModule module)
    {
        var fields = new List<FormFieldDto>
        {
            DirectoryFormSchemas.NameField(module),

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

        fields.AddRange(DirectoryFormSchemas.LocationFields());
        fields.AddRange(DirectoryFormSchemas.ContactFields());
        fields.AddRange(DirectoryFormSchemas.AdvertisementFields(module));

        return new AdFormSchema(module.Key, DirectoryFormSchemas.Submit(module), Array.Empty<string>(), DirectoryFormSchemas.Order(fields));
    }
}
