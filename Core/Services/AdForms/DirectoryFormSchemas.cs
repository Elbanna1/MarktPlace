using Shared.Constants;
using Shared.DTOs.Lookups.Forms;
using static Services.AdForms.AdFormFieldFactory;

namespace Services.AdForms;

internal static class DirectoryFormSchemas
{
    public static AdFormSchema Facility(DirectoryModule module, bool locationBeforeContact = false)
    {
        var fields = new List<FormFieldDto> { NameField(module) };

        fields.AddRange(AdvertisementFields(module));

        if (locationBeforeContact)
        {
            fields.AddRange(LocationFields(module));
            fields.AddRange(ContactFields());
        }
        else
        {
            fields.AddRange(ContactFields());
            fields.AddRange(LocationFields(module));
        }

        return new AdFormSchema(module.Key, Submit(module), Array.Empty<string>(), Order(fields));
    }

    public static CreateAdFormSubmitDto Submit(DirectoryModule module) => new(module.PublicRoute);

    public static FormFieldDto NameField(DirectoryModule module) =>
        Text("Name", module.NameLabel, module.NameLabelEn, module.Section,
            required: true, maxLength: DirectoryListingRules.MaxNameLength, placeholder: module.NamePlaceholder);

    public static IEnumerable<FormFieldDto> LocationFields(DirectoryModule module)
    {
        var address = Address();
        address.Placeholder = module.AddressPlaceholder ?? address.Placeholder;

        var googleMaps = GoogleMapsUrl();
        googleMaps.Placeholder = module.GoogleMapsPlaceholder ?? googleMaps.Placeholder;

        return [address, googleMaps];
    }

    public static IEnumerable<FormFieldDto> ContactFields()
    {
        var whatsApp = PhoneNumber("WhatsApp", "رقم الواتساب", "WhatsApp number", required: false);
        whatsApp.HelpText = "اختياري.";

        var email = Email();
        email.HelpText = "اختياري.";

        return [PhoneNumber("Phone"), whatsApp, email];
    }

    public static IEnumerable<FormFieldDto> AdvertisementFields(DirectoryModule module)
    {
        var section = module.AdSection ?? SectionAd;

        var title = Title();
        title.Placeholder = module.TitlePlaceholder;
        title.Section = section;

        var description = TextArea("Description", "وصف الإعلان", "Advertisement description", section,
            required: true, maxLength: DirectoryListingRules.MaxDescriptionLength,
            placeholder: module.DescriptionPlaceholder);
        description.HelpText = module.DescriptionPlaceholder;

        var images = module.ImagesRequired ? RequiredImages() : Images();

        if (module.AdSection is not null)
            images.Section = module.AdSection;

        return [title, description, images];
    }

    public static IReadOnlyList<FormFieldDto> Order(List<FormFieldDto> fields)
    {
        for (var position = 0; position < fields.Count; position++)
            fields[position].Order = position + 1;

        return fields;
    }
}
