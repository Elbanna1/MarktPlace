using System.Text.Json.Serialization;
using Shared.DTOs.Lookups.Forms;

namespace Shared.DTOs.Lookups.Read;

public class ReadParameterDto
{
    public string Name { get; set; } = default!;

    public string Label { get; set; } = default!;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? LabelEn { get; set; }

    public string Type { get; set; } = ReadParameterTypes.String;

    public bool Required { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool Multiple { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? DefaultValue { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public decimal? MinValue { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public decimal? MaxValue { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<FormFieldOptionDto>? Options { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? OptionsSource { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? OptionsValue { get; set; }
}

public static class ReadParameterTypes
{
    public const string Integer = "int";
    public const string Decimal = "decimal";
    public const string String = "string";
    public const string Boolean = "bool";

    public const string Date = "date";

    public const string Guid = "guid";

    public const string Enum = "enum";
}

public static class ReadOptionsValues
{
    public const string Id = "id";

    public const string Name = "name";
}
