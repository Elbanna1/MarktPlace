using System.Text.Json.Serialization;

namespace Shared.DTOs.Lookups.Forms;

public class AdFormBreadcrumbItemDto
{
    public string Level { get; set; } = default!;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Id { get; set; }

    public string Name { get; set; } = default!;

    public string NameAr { get; set; } = default!;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Icon { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Path { get; set; }

    public bool IsCurrent { get; set; }
}
