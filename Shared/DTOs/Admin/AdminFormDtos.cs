using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Shared.DTOs.Lookups.Forms;

namespace Shared.DTOs.Admin;

public enum AdminFormFieldOrigin
{
    Catalog = 1,

    Custom = 2
}

public class AdminFormSchemaDto
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = default!;

    public int? SubCategoryId { get; set; }

    public string? SubCategoryName { get; set; }

    public string Module { get; set; } = default!;

    public string? SubmitEndpoint { get; set; }

    public IReadOnlyList<AdminFormFieldDto> Fields { get; set; } = Array.Empty<AdminFormFieldDto>();

    public IReadOnlyList<string> AvailableFieldTypes { get; set; } = Array.Empty<string>();
}

public class AdminFormFieldDto
{
    public Guid? FieldId { get; set; }

    public string Name { get; set; } = default!;

    public string Label { get; set; } = default!;

    public string? LabelEn { get; set; }

    public string Type { get; set; } = default!;

    public bool Required { get; set; }

    public bool Visible { get; set; }

    public int Order { get; set; }

    public string? Section { get; set; }

    public string? Placeholder { get; set; }

    public string? HelpText { get; set; }

    public AdminFormFieldOrigin Origin { get; set; }

    public bool IsOverridden { get; set; }

    public bool IsPersisted { get; set; }

    public bool CanRelaxRequirement { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AdminFormOptionDto>? Options { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? OptionsSource { get; set; }

    public bool SupportsOptions { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MinLength { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MaxLength { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public decimal? MinValue { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public decimal? MaxValue { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Pattern { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? PatternMessage { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MaxSelections { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public FormFieldConditionDto? VisibleWhen { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public FormFieldConditionDto? RequiredWhen { get; set; }
}

public class AdminFormOptionDto
{
    public Guid? Id { get; set; }

    public string Value { get; set; } = default!;

    public string Label { get; set; } = default!;

    public string? LabelEn { get; set; }

    public string? Group { get; set; }

    public int SortOrder { get; set; }

    public bool IsActive { get; set; }

    public bool IsCustom { get; set; }
}

public class CreateFormFieldRequest
{
    [Required(ErrorMessage = "اسم الحقل مطلوب.")]
    [MaxLength(100, ErrorMessage = "لا يمكن أن يتجاوز اسم الحقل 100 حرف.")]
    [RegularExpression("^[A-Za-z][A-Za-z0-9_]*$",
        ErrorMessage = "اسم الحقل يجب أن يبدأ بحرف ويحتوي على حروف وأرقام و _ فقط.")]
    public string Name { get; set; } = default!;

    [Required(ErrorMessage = "اسم الحقل بالعربية مطلوب.")]
    [MaxLength(200, ErrorMessage = "لا يمكن أن يتجاوز الاسم 200 حرف.")]
    public string LabelAr { get; set; } = default!;

    [MaxLength(200)]
    public string? LabelEn { get; set; }

    [Required(ErrorMessage = "نوع الحقل مطلوب.")]
    [MaxLength(30)]
    public string Type { get; set; } = default!;

    public bool IsRequired { get; set; }

    [Range(0, 9999, ErrorMessage = "الترتيب يجب أن يكون بين 0 و 9999.")]
    public int SortOrder { get; set; }

    [MaxLength(150)]
    public string? Section { get; set; }

    [MaxLength(300)]
    public string? Placeholder { get; set; }

    [MaxLength(500)]
    public string? HelpText { get; set; }

    public int? MinLength { get; set; }

    public int? MaxLength { get; set; }

    public decimal? MinValue { get; set; }

    public decimal? MaxValue { get; set; }

    [MaxLength(500)]
    public string? Pattern { get; set; }

    [MaxLength(300)]
    public string? PatternMessage { get; set; }

    public int? MaxSelections { get; set; }

    [MaxLength(100)]
    public string? VisibleWhenField { get; set; }

    public IReadOnlyList<string>? VisibleWhenValues { get; set; }

    [MaxLength(100)]
    public string? RequiredWhenField { get; set; }

    public IReadOnlyList<string>? RequiredWhenValues { get; set; }

    public IReadOnlyList<SaveFormOptionRequest>? Options { get; set; }
}

public class UpdateFormFieldRequest
{
    [MaxLength(200)]
    public string? LabelAr { get; set; }

    [MaxLength(200)]
    public string? LabelEn { get; set; }

    public bool? IsRequired { get; set; }

    public bool? IsHidden { get; set; }

    [Range(0, 9999, ErrorMessage = "الترتيب يجب أن يكون بين 0 و 9999.")]
    public int? SortOrder { get; set; }

    [MaxLength(150)]
    public string? Section { get; set; }

    [MaxLength(300)]
    public string? Placeholder { get; set; }

    [MaxLength(500)]
    public string? HelpText { get; set; }

    public int? MinLength { get; set; }

    public int? MaxLength { get; set; }

    public decimal? MinValue { get; set; }

    public decimal? MaxValue { get; set; }

    [MaxLength(500)]
    public string? Pattern { get; set; }

    [MaxLength(300)]
    public string? PatternMessage { get; set; }

    public int? MaxSelections { get; set; }

    [MaxLength(100)]
    public string? VisibleWhenField { get; set; }

    public IReadOnlyList<string>? VisibleWhenValues { get; set; }

    [MaxLength(100)]
    public string? RequiredWhenField { get; set; }

    public IReadOnlyList<string>? RequiredWhenValues { get; set; }

    public bool ClearConditions { get; set; }
}

public class SaveFormOptionRequest
{
    [Required(ErrorMessage = "قيمة الخيار مطلوبة.")]
    [MaxLength(200)]
    public string Value { get; set; } = default!;

    [Required(ErrorMessage = "اسم الخيار بالعربية مطلوب.")]
    [MaxLength(200)]
    public string Label { get; set; } = default!;

    [MaxLength(200)]
    public string? LabelEn { get; set; }

    [MaxLength(150)]
    public string? Group { get; set; }

    [Range(0, 9999, ErrorMessage = "الترتيب يجب أن يكون بين 0 و 9999.")]
    public int SortOrder { get; set; }

    public bool IsActive { get; set; } = true;
}
