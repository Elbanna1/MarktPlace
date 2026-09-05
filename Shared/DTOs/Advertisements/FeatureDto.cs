using System.Text.Json.Serialization;

namespace Shared.DTOs.Advertisements;

public class FeatureDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Group { get; set; }
}
