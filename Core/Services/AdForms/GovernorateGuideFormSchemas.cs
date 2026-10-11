using Shared.Constants;
using Shared.DTOs.Lookups.Forms;
using Shared.Enums;
using static Services.AdForms.AdFormFieldFactory;

namespace Services.AdForms;

internal static class GovernorateGuideFormSchemas
{
    public static AdFormSchema? For(SubCategoryType subCategory) => subCategory switch
    {
        SubCategoryType.CommercialShops => CommercialShops(GovernorateGuideCatalog.CommercialShops),
        SubCategoryType.GeneralServices => GeneralServices(GovernorateGuideCatalog.GeneralServices),
        SubCategoryType.ResortsHotelsAndAmusement =>
            DirectoryFormSchemas.Facility(GovernorateGuideCatalog.ResortsHotelsAndAmusement, locationBeforeContact: true),
        _ => null
    };

    private static AdFormSchema CommercialShops(DirectoryModule module)
    {
        var fields = new List<FormFieldDto>
        {
            DirectoryFormSchemas.NameField(module),

            new FormFieldDto
                {
                    Name = "ActivityType",
                    Label = GovernorateGuideCatalog.ActivityTypeLabel,
                    LabelEn = "Activity type",
                    Type = FormFieldTypes.Select,
                    Required = false,
                    Section = module.Section,
                    Placeholder = "اختار نوع النشاط",
                    Options = ShopCatalog.SuitableActivities
                        .Select(entry => new FormFieldOptionDto((int)entry.Value, entry.Name, entry.NameEn))
                        .ToList()
                }
                .AsSearchable(),

            Text("OtherActivityType", GovernorateGuideCatalog.OtherActivityTypeLabel, "Other activity type",
                    module.Section, required: false, maxLength: GovernorateGuideCatalog.MaxOtherActivityTypeLength,
                    placeholder: "مثال: أدوات صحية")
                .VisibleOnlyWhen("ActivityType", (int)ShopSuitableActivity.Other)
                .MandatoryWhen("ActivityType", (int)ShopSuitableActivity.Other)
        };

        fields.AddRange(DirectoryFormSchemas.AdvertisementFields(module));
        fields.AddRange(DirectoryFormSchemas.LocationFields(module));
        fields.AddRange(DirectoryFormSchemas.ContactFields());

        return new AdFormSchema(module.Key, DirectoryFormSchemas.Submit(module), Array.Empty<string>(),
            DirectoryFormSchemas.Order(fields));
    }

    private static AdFormSchema GeneralServices(DirectoryModule module)
    {
        var fields = new List<FormFieldDto>
        {
            DirectoryFormSchemas.NameField(module),

            new FormFieldDto
            {
                Name = "ServiceType",
                Label = GovernorateGuideCatalog.ServiceTypeLabel,
                LabelEn = "Service type",
                Type = FormFieldTypes.Select,
                Required = true,
                Section = module.Section,
                Placeholder = "اختار نوع الخدمة",
                Options = GovernorateGuideCatalog.ServiceTypes
                    .Select(entry => new FormFieldOptionDto((int)entry.Id, entry.Name, entry.NameEn))
                    .ToList()
            },

            SubtypeField(module, GeneralServiceType.Fixed, GovernorateGuideCatalog.FixedServiceSubtypeField,
                GovernorateGuideCatalog.FixedServiceSubtypeLabel, "Fixed service"),

            SubtypeField(module, GeneralServiceType.Mobile, GovernorateGuideCatalog.MobileServiceSubtypeField,
                GovernorateGuideCatalog.MobileServiceSubtypeLabel, "Mobile service")
        };

        fields.AddRange(DirectoryFormSchemas.AdvertisementFields(module));
        fields.AddRange(DirectoryFormSchemas.LocationFields(module));
        fields.AddRange(DirectoryFormSchemas.ContactFields());

        return new AdFormSchema(module.Key, DirectoryFormSchemas.Submit(module), Array.Empty<string>(),
            DirectoryFormSchemas.Order(fields));
    }

    private static FormFieldDto SubtypeField(
        DirectoryModule module, GeneralServiceType type, string name, string label, string labelEn) =>
        new FormFieldDto
            {
                Name = name,
                Label = label,
                LabelEn = labelEn,
                Type = FormFieldTypes.Select,
                Required = false,
                Section = module.Section,
                Placeholder = "اختار الخدمة",
                Options = GovernorateGuideCatalog.SubtypesOf(type)
                    .Select(entry => new FormFieldOptionDto((int)entry.Id, entry.Name, entry.NameEn))
                    .ToList()
            }
            .VisibleOnlyWhen("ServiceType", (int)type)
            .MandatoryWhen("ServiceType", (int)type);
}
