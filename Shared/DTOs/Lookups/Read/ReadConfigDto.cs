using System.Text.Json.Serialization;

namespace Shared.DTOs.Lookups.Read;

public class ReadConfigDto
{
    public CategoryDto Category { get; set; } = default!;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public SubCategoryDto? SubCategory { get; set; }

    public bool RequiresSubCategory { get; set; }

    public string Module { get; set; } = default!;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ReadOperationDto? List { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ReadOperationDto? Details { get; set; }

    public IReadOnlyDictionary<string, ReadOperationDto> Operations { get; set; } =
        new Dictionary<string, ReadOperationDto>();
}
