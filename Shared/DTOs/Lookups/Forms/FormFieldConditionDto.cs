namespace Shared.DTOs.Lookups.Forms;

public class FormFieldConditionDto
{
    public FormFieldConditionDto() { }

    public FormFieldConditionDto(string field, params object[] values)
    {
        Field = field;
        Values = values.ToList();
    }

    public string Field { get; set; } = default!;

    public List<object> Values { get; set; } = new();
}
