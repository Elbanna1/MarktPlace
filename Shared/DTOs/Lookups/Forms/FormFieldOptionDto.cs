using System.Text.Json.Serialization;

namespace Shared.DTOs.Lookups.Forms;

public class FormFieldOptionDto
{
    public FormFieldOptionDto() { }

    public FormFieldOptionDto(object value, string label, string? labelEn = null, string? group = null)
    {
        Value = value;
        Label = label;
        LabelEn = labelEn;
        Group = group;
    }

    public object Value { get; set; } = default!;

    public string Label { get; set; } = default!;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? LabelEn { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Group { get; set; }
}
