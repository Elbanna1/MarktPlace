using Shared.DTOs.Lookups.Forms;

namespace Services.AdForms;

public sealed record AdFormSchema(
    string Module,
    CreateAdFormSubmitDto Submit,
    IReadOnlyCollection<string> RequiredLookups,
    IReadOnlyList<FormFieldDto> Fields);
