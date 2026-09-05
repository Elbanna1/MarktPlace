using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Admin;

public class AdminCategoryDto
{
    public int Id { get; set; }

    public string NameAr { get; set; } = default!;

    public string NameEn { get; set; } = default!;

    public string? Icon { get; set; }

    public bool IsActive { get; set; }

    public int SortOrder { get; set; }

    public int SubCategoriesCount { get; set; }

    public int AdsCount { get; set; }

    public bool IsSystem { get; set; }

    public IReadOnlyList<AdminSubCategoryDto> SubCategories { get; set; } =
        Array.Empty<AdminSubCategoryDto>();
}

public class AdminSubCategoryDto
{
    public int Id { get; set; }

    public int CategoryId { get; set; }

    public string CategoryNameAr { get; set; } = default!;

    public string NameAr { get; set; } = default!;

    public string NameEn { get; set; } = default!;

    public string? Icon { get; set; }

    public bool IsActive { get; set; }

    public int SortOrder { get; set; }

    public int AdsCount { get; set; }

    public bool IsSystem { get; set; }
}

public class CreateCategoryRequest
{
    [Required(ErrorMessage = "الاسم بالعربية مطلوب.")]
    [MaxLength(100, ErrorMessage = "لا يمكن أن يتجاوز الاسم 100 حرف.")]
    public string NameAr { get; set; } = default!;

    [Required(ErrorMessage = "الاسم بالإنجليزية مطلوب.")]
    [MaxLength(100, ErrorMessage = "لا يمكن أن يتجاوز الاسم 100 حرف.")]
    public string NameEn { get; set; } = default!;

    [MaxLength(500)]
    public string? Icon { get; set; }

    public bool IsActive { get; set; } = true;

    [Range(0, 9999, ErrorMessage = "الترتيب يجب أن يكون بين 0 و 9999.")]
    public int SortOrder { get; set; }
}

public class UpdateCategoryRequest : CreateCategoryRequest;

public class CreateSubCategoryRequest : CreateCategoryRequest;

public class UpdateSubCategoryRequest : CreateCategoryRequest
{
    public int? CategoryId { get; set; }
}

public class UpdateActiveStatusRequest
{
    [Required(ErrorMessage = "الحالة مطلوبة.")]
    public bool IsActive { get; set; }
}
