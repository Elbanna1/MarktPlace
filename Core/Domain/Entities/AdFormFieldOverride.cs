namespace Domain.Entities;

public class AdFormFieldOverride
{
    public Guid Id { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; } = default!;

    public int? SubCategoryId { get; set; }
    public SubCategory? SubCategory { get; set; }

    public string FieldName { get; set; } = default!;

    public bool IsCustom { get; set; }

    public string? Label { get; set; }

    public string? LabelEn { get; set; }

    public string? Type { get; set; }

    public string? Section { get; set; }

    public string? Placeholder { get; set; }

    public string? HelpText { get; set; }

    public int? SortOrder { get; set; }

    public bool? IsRequired { get; set; }

    public bool IsHidden { get; set; }

    public int? MinLength { get; set; }

    public int? MaxLength { get; set; }

    public decimal? MinValue { get; set; }

    public decimal? MaxValue { get; set; }

    public string? Pattern { get; set; }

    public string? PatternMessage { get; set; }

    public int? MaxSelections { get; set; }

    public string? VisibleWhenField { get; set; }

    public string? VisibleWhenValues { get; set; }

    public string? RequiredWhenField { get; set; }

    public string? RequiredWhenValues { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public ICollection<AdFormFieldOption> Options { get; set; } = new List<AdFormFieldOption>();
}

public class AdFormFieldOption
{
    public Guid Id { get; set; }

    public Guid FieldOverrideId { get; set; }
    public AdFormFieldOverride FieldOverride { get; set; } = default!;

    public string Value { get; set; } = default!;

    public string Label { get; set; } = default!;

    public string? LabelEn { get; set; }

    public string? Group { get; set; }

    public int SortOrder { get; set; }

    public bool IsActive { get; set; } = true;
}
