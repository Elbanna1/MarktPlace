using System.ComponentModel.DataAnnotations;
using Shared.Enums;

namespace Shared.DTOs.Admin;

public class AdminHomeConfigurationDto
{
    public IReadOnlyList<AdminHomeSectionDto> Sections { get; set; } = Array.Empty<AdminHomeSectionDto>();

    public IReadOnlyList<AdminCategoryDto> Categories { get; set; } = Array.Empty<AdminCategoryDto>();
}

public class AdminHomeSectionDto
{
    public int Id { get; set; }

    public string Key { get; set; } = default!;

    public HomeSectionType Type { get; set; }

    public string TypeName { get; set; } = default!;

    public string? Title { get; set; }

    public string? TitleEn { get; set; }

    public string? Subtitle { get; set; }

    public bool IsVisible { get; set; }

    public int SortOrder { get; set; }

    public string? ImageUrl { get; set; }

    public string? LinkUrl { get; set; }

    public string? LinkText { get; set; }

    public int? CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public int? ItemCount { get; set; }

    public bool IsBannerSlider { get; set; }
}

public class UpdateHomeSectionRequest
{
    [MaxLength(150, ErrorMessage = "لا يمكن أن يتجاوز العنوان 150 حرفًا.")]
    public string? Title { get; set; }

    [MaxLength(150, ErrorMessage = "لا يمكن أن يتجاوز العنوان 150 حرفًا.")]
    public string? TitleEn { get; set; }

    [MaxLength(500, ErrorMessage = "لا يمكن أن يتجاوز الوصف 500 حرف.")]
    public string? Subtitle { get; set; }

    [Required(ErrorMessage = "حالة الظهور مطلوبة.")]
    public bool IsVisible { get; set; }

    [Range(0, 9999, ErrorMessage = "الترتيب يجب أن يكون بين 0 و 9999.")]
    public int SortOrder { get; set; }

    [MaxLength(1000)]
    public string? LinkUrl { get; set; }

    [MaxLength(100)]
    public string? LinkText { get; set; }

    public int? CategoryId { get; set; }

    [Range(1, 50, ErrorMessage = "عدد العناصر يجب أن يكون بين 1 و 50.")]
    public int? ItemCount { get; set; }
}

public class ReorderHomeSectionsRequest
{
    [Required(ErrorMessage = "ترتيب الأقسام مطلوب.")]
    [MinLength(1, ErrorMessage = "ترتيب الأقسام مطلوب.")]
    public IReadOnlyList<int> SectionIds { get; set; } = Array.Empty<int>();
}
