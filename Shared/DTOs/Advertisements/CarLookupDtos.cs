using System.Text.Json.Serialization;

namespace Shared.DTOs.Advertisements;

public class CarLookupItemDto
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Group { get; set; }
}

public class CarNameOptionDto
{
    public string Value { get; set; } = default!;

    public string Name { get; set; } = default!;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Group { get; set; }
}

public class CarFeatureOptionDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Group { get; set; } = default!;
}
