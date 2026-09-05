using System.Text.Json.Serialization;
using Shared.DTOs.Lookups;
using Shared.Enums;

namespace Shared.DTOs.Home;

public class HomeConfigurationDto
{
    public IReadOnlyList<HomeSectionDto> Sections { get; set; } = Array.Empty<HomeSectionDto>();

    public IReadOnlyList<CategoryDto> Categories { get; set; } = Array.Empty<CategoryDto>();
}

public class HomeSectionDto
{
    public int Id { get; set; }

    public string Key { get; set; } = default!;

    public HomeSectionType Type { get; set; }

    public string TypeName { get; set; } = default!;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Title { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? TitleEn { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Subtitle { get; set; }

    public int SortOrder { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ImageUrl { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? LinkUrl { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? LinkText { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? CategoryId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? CategoryName { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? ItemCount { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public BannerLocation? BannerLocation { get; set; }
}
