using System.Text.Json.Serialization;

namespace Shared.DTOs.Lookups.Forms;

public class CreateAdFormDto
{
    public CategoryDto Category { get; set; } = default!;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public SubCategoryDto? SubCategory { get; set; }

    public IReadOnlyList<AdFormBreadcrumbItemDto> Breadcrumb { get; set; } =
        Array.Empty<AdFormBreadcrumbItemDto>();

    public CreateAdFormUploadLimitsDto Upload { get; set; } = new();

    public bool RequiresSubCategory { get; set; }

    public string Module { get; set; } = default!;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public CreateAdFormSubmitDto? Submit { get; set; }

    public CreateAdFormLookupsDto Lookups { get; set; } = new();

    public IReadOnlyList<FormFieldDto> Fields { get; set; } = Array.Empty<FormFieldDto>();
}
