using Shared.Enums;

namespace Domain.Entities;

public class HomeSection
{
    public int Id { get; set; }

    public string Key { get; set; } = default!;

    public HomeSectionType Type { get; set; }

    public string? Title { get; set; }

    public string? TitleEn { get; set; }

    public string? Subtitle { get; set; }

    public bool IsVisible { get; set; } = true;

    public int SortOrder { get; set; }

    public string? ImageUrl { get; set; }

    public string? ImagePath { get; set; }

    public string? LinkUrl { get; set; }

    public string? LinkText { get; set; }

    public int? CategoryId { get; set; }
    public Category? Category { get; set; }

    public int? ItemCount { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}
