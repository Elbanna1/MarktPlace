using System.Text.Json.Serialization;

namespace Shared.DTOs.Lookups.Read;

public class ReadOperationDto
{
    public string Key { get; set; } = default!;

    public string Label { get; set; } = default!;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? LabelEn { get; set; }

    public string Endpoint { get; set; } = default!;

    public string Method { get; } = "GET";

    public bool RequiresAuthentication { get; set; }

    public bool Paginated { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<ReadParameterDto>? RouteParameters { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<ReadParameterDto>? QueryParameters { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyDictionary<string, object>? Query { get; set; }
}
