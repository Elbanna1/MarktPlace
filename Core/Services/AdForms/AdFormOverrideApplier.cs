using System.Text.Json;
using Domain.Entities;
using Shared.DTOs.Lookups.Forms;

namespace Services.AdForms;

public static class AdFormOverrideApplier
{
    public static IReadOnlyList<FormFieldDto> Apply(
        IReadOnlyList<FormFieldDto> catalogFields,
        IReadOnlyList<AdFormFieldOverride> overrides)
    {
        if (overrides.Count == 0)
            return catalogFields;

        var byName = overrides.ToDictionary(
            item => item.FieldName, StringComparer.OrdinalIgnoreCase);

        var merged = new List<FormFieldDto>(catalogFields.Count + overrides.Count);

        foreach (var field in catalogFields)
        {
            if (!byName.TryGetValue(field.Name, out var edit))
            {
                merged.Add(field);
                continue;
            }

            if (edit.IsHidden && !field.Required)
                continue;

            merged.Add(ApplyTo(field, edit));
        }

        foreach (var custom in overrides.Where(item => item.IsCustom))
        {
            if (catalogFields.Any(field =>
                    string.Equals(field.Name, custom.FieldName, StringComparison.OrdinalIgnoreCase)))
            {
                continue;
            }

            if (custom.IsHidden)
                continue;

            merged.Add(BuildCustom(custom));
        }

        return merged
            .OrderBy(field => field.Order)
            .ThenBy(field => field.Name, StringComparer.Ordinal)
            .ToList();
    }

    public static FormFieldDto ApplyTo(FormFieldDto field, AdFormFieldOverride edit)
    {
        var result = Clone(field);

        result.Label = string.IsNullOrWhiteSpace(edit.Label) ? field.Label : edit.Label;
        result.LabelEn = Coalesce(edit.LabelEn, field.LabelEn);
        result.Section = Coalesce(edit.Section, field.Section);
        result.Placeholder = Coalesce(edit.Placeholder, field.Placeholder);
        result.HelpText = Coalesce(edit.HelpText, field.HelpText);

        if (edit.SortOrder is { } order)
            result.Order = order;

        result.Required = field.Required || edit.IsRequired == true;

        result.MinLength = Stricter(field.MinLength, edit.MinLength, takeLarger: true);
        result.MaxLength = Stricter(field.MaxLength, edit.MaxLength, takeLarger: false);
        result.MinValue = Stricter(field.MinValue, edit.MinValue, takeLarger: true);
        result.MaxValue = Stricter(field.MaxValue, edit.MaxValue, takeLarger: false);
        result.MaxSelections = Stricter(field.MaxSelections, edit.MaxSelections, takeLarger: false);

        if (!string.IsNullOrWhiteSpace(edit.Pattern))
        {
            result.Pattern = edit.Pattern;
            result.PatternMessage = Coalesce(edit.PatternMessage, field.PatternMessage);
        }

        var options = ActiveOptions(edit);

        if (options.Count > 0)
        {
            result.Options = options;
            result.OptionsSource = null;
        }

        if (!string.IsNullOrWhiteSpace(edit.VisibleWhenField))
            result.VisibleWhen = BuildCondition(edit.VisibleWhenField, edit.VisibleWhenValues);

        if (!string.IsNullOrWhiteSpace(edit.RequiredWhenField))
            result.RequiredWhen = BuildCondition(edit.RequiredWhenField, edit.RequiredWhenValues);

        return result;
    }

    public static FormFieldDto BuildCustom(AdFormFieldOverride edit)
    {
        var field = new FormFieldDto
        {
            Name = edit.FieldName,
            Label = edit.Label ?? edit.FieldName,
            LabelEn = edit.LabelEn,
            Type = edit.Type ?? FormFieldTypes.Text,
            Required = edit.IsRequired == true,
            Visible = !edit.IsHidden,
            Order = edit.SortOrder ?? 0,
            Section = edit.Section,
            Placeholder = edit.Placeholder,
            HelpText = edit.HelpText,
            MinLength = edit.MinLength,
            MaxLength = edit.MaxLength,
            MinValue = edit.MinValue,
            MaxValue = edit.MaxValue,
            Pattern = edit.Pattern,
            PatternMessage = edit.PatternMessage,
            MaxSelections = edit.MaxSelections
        };

        var options = ActiveOptions(edit);

        if (options.Count > 0)
            field.Options = options;

        if (!string.IsNullOrWhiteSpace(edit.VisibleWhenField))
            field.VisibleWhen = BuildCondition(edit.VisibleWhenField, edit.VisibleWhenValues);

        if (!string.IsNullOrWhiteSpace(edit.RequiredWhenField))
            field.RequiredWhen = BuildCondition(edit.RequiredWhenField, edit.RequiredWhenValues);

        return field;
    }

    public static bool SupportsOptions(string? type) =>
        type is FormFieldTypes.Select or FormFieldTypes.MultiSelect or FormFieldTypes.Radio;

    public static List<object> ParseValues(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return new List<object>();

        try
        {
            using var document = JsonDocument.Parse(json);

            if (document.RootElement.ValueKind != JsonValueKind.Array)
                return new List<object>();

            return document.RootElement
                .EnumerateArray()
                .Select(ReadValue)
                .Where(value => value is not null)
                .Select(value => value!)
                .ToList();
        }
        catch (JsonException)
        {
            return new List<object>();
        }
    }

    public static string? SerializeValues(IReadOnlyList<string>? values) =>
        values is null || values.Count == 0
            ? null
            : JsonSerializer.Serialize(values);

    private static object? ReadValue(JsonElement element) =>
        element.ValueKind switch
        {
            JsonValueKind.Number when element.TryGetInt32(out var number) => number,
            JsonValueKind.Number => element.GetDecimal(),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.String => Typed(element.GetString()),
            _ => null
        };

    private static object? Typed(string? value)
    {
        if (value is null)
            return null;

        return int.TryParse(value, out var number) ? number : value;
    }

    private static List<FormFieldOptionDto> ActiveOptions(AdFormFieldOverride edit) =>
        edit.Options
            .Where(option => option.IsActive)
            .OrderBy(option => option.SortOrder)
            .ThenBy(option => option.Label, StringComparer.Ordinal)
            .Select(option => new FormFieldOptionDto(
                Typed(option.Value) ?? option.Value, option.Label, option.LabelEn, option.Group))
            .ToList();

    private static FormFieldConditionDto BuildCondition(string? field, string? valuesJson) =>
        new()
        {
            Field = field!,
            Values = ParseValues(valuesJson)
        };

    private static string? Coalesce(string? edited, string? original) =>
        string.IsNullOrWhiteSpace(edited) ? original : edited;

    private static int? Stricter(int? original, int? edited, bool takeLarger)
    {
        if (edited is not { } value)
            return original;

        if (original is not { } current)
            return value;

        return takeLarger ? Math.Max(current, value) : Math.Min(current, value);
    }

    private static decimal? Stricter(decimal? original, decimal? edited, bool takeLarger)
    {
        if (edited is not { } value)
            return original;

        if (original is not { } current)
            return value;

        return takeLarger ? Math.Max(current, value) : Math.Min(current, value);
    }

    private static FormFieldDto Clone(FormFieldDto field) =>
        new()
        {
            Name = field.Name,
            Label = field.Label,
            LabelEn = field.LabelEn,
            Type = field.Type,
            Required = field.Required,
            Visible = field.Visible,
            Order = field.Order,
            Section = field.Section,
            Placeholder = field.Placeholder,
            HelpText = field.HelpText,
            DefaultValue = field.DefaultValue,
            ReadOnly = field.ReadOnly,
            MinLength = field.MinLength,
            MaxLength = field.MaxLength,
            MinValue = field.MinValue,
            MaxValue = field.MaxValue,
            Pattern = field.Pattern,
            PatternMessage = field.PatternMessage,
            MaxSelections = field.MaxSelections,
            Searchable = field.Searchable,
            Grouped = field.Grouped,
            Options = field.Options,
            OptionsSource = field.OptionsSource,
            Multiple = field.Multiple,
            MaxFiles = field.MaxFiles,
            MinItems = field.MinItems,
            MaxSizeMb = field.MaxSizeMb,
            AllowedExtensions = field.AllowedExtensions,
            VisibleWhen = field.VisibleWhen,
            RequiredWhen = field.RequiredWhen
        };
}
