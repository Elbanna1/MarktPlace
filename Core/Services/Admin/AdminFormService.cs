using Domain.Entities;
using Services.AdForms;
using Services.Lookups;
using ServicesAbstraction;
using Shared.DTOs.Admin;
using Shared.DTOs.Lookups.Forms;
using Shared.Exceptions;

namespace Services.Admin;

public class AdminFormService : IAdminFormService
{
    private readonly IAdminFormRepository _repository;
    private readonly CategorySelectionResolver _selectionResolver;

    private static readonly HashSet<string> SupportedTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        FormFieldTypes.Text, FormFieldTypes.TextArea, FormFieldTypes.Number,
        FormFieldTypes.Select, FormFieldTypes.MultiSelect, FormFieldTypes.Radio,
        FormFieldTypes.Checkbox, FormFieldTypes.Date,
        FormFieldTypes.Image, FormFieldTypes.Video, FormFieldTypes.File
    };

    public AdminFormService(IAdminFormRepository repository, CategorySelectionResolver selectionResolver)
    {
        _repository = repository;
        _selectionResolver = selectionResolver;
    }

    public async Task<AdminFormSchemaDto> GetFormAsync(
        int categoryId, int? subCategoryId, CancellationToken cancellationToken = default)
    {
        var (selection, schema) = await ResolveAsync(categoryId, subCategoryId, cancellationToken);

        var overrides = await _repository.GetOverridesAsync(categoryId, subCategoryId, cancellationToken);

        var overridesByName = overrides.ToDictionary(
            item => item.FieldName, StringComparer.OrdinalIgnoreCase);

        var fields = new List<AdminFormFieldDto>();

        foreach (var catalogField in schema.Fields)
        {
            overridesByName.TryGetValue(catalogField.Name, out var edit);

            var effective = edit is null
                ? catalogField
                : AdFormOverrideApplier.ApplyTo(catalogField, edit);

            fields.Add(new AdminFormFieldDto
            {
                FieldId = edit?.Id,
                Name = effective.Name,
                Label = effective.Label,
                LabelEn = effective.LabelEn,
                Type = effective.Type,
                Required = effective.Required,

                Visible = !(edit?.IsHidden ?? false),
                Order = effective.Order,
                Section = effective.Section,
                Placeholder = effective.Placeholder,
                HelpText = effective.HelpText,

                Origin = AdminFormFieldOrigin.Catalog,
                IsOverridden = edit is not null,
                IsPersisted = true,

                CanRelaxRequirement = !catalogField.Required,

                Options = MapOptions(effective, edit),
                OptionsSource = effective.OptionsSource,
                SupportsOptions = AdFormOverrideApplier.SupportsOptions(effective.Type),

                MinLength = effective.MinLength,
                MaxLength = effective.MaxLength,
                MinValue = effective.MinValue,
                MaxValue = effective.MaxValue,
                Pattern = effective.Pattern,
                PatternMessage = effective.PatternMessage,
                MaxSelections = effective.MaxSelections,
                VisibleWhen = effective.VisibleWhen,
                RequiredWhen = effective.RequiredWhen
            });
        }

        foreach (var custom in overrides.Where(item => item.IsCustom))
        {
            if (overridesByName.ContainsKey(custom.FieldName) &&
                schema.Fields.Any(field =>
                    string.Equals(field.Name, custom.FieldName, StringComparison.OrdinalIgnoreCase)))
            {
                continue;
            }

            fields.Add(MapCustom(custom));
        }

        return new AdminFormSchemaDto
        {
            CategoryId = categoryId,
            CategoryName = selection.Category.NameAr,
            SubCategoryId = subCategoryId,
            SubCategoryName = selection.SubCategory?.NameAr,
            Module = schema.Module,
            SubmitEndpoint = $"{schema.Submit.Method} {schema.Submit.Endpoint}",
            Fields = fields.OrderBy(field => field.Order).ThenBy(field => field.Name).ToList(),
            AvailableFieldTypes = SupportedTypes.OrderBy(type => type).ToList()
        };
    }

    public async Task<AdminFormFieldDto> CreateFieldAsync(
        int categoryId, int? subCategoryId, string adminUserId, CreateFormFieldRequest request,
        CancellationToken cancellationToken = default)
    {
        var (_, schema) = await ResolveAsync(categoryId, subCategoryId, cancellationToken);

        var name = request.Name.Trim();

        if (!SupportedTypes.Contains(request.Type))
            throw new BadRequestException(
                $"نوع الحقل '{request.Type}' غير مدعوم. الأنواع المتاحة: {string.Join(", ", SupportedTypes)}.");

        if (schema.Fields.Any(field => string.Equals(field.Name, name, StringComparison.OrdinalIgnoreCase)))
            throw new ConflictException(
                $"الحقل '{name}' موجود بالفعل في هذا النموذج. يمكنك تعديله بدلًا من إضافته.");

        var existing = await _repository.FindOverrideAsync(
            categoryId, subCategoryId, name, cancellationToken);

        if (existing is not null)
            throw new ConflictException($"الحقل '{name}' مضاف بالفعل إلى هذا النموذج.");

        var options = request.Options ?? Array.Empty<SaveFormOptionRequest>();

        if (options.Count > 0 && !AdFormOverrideApplier.SupportsOptions(request.Type))
            throw new BadRequestException("لا يمكن إضافة خيارات إلى حقل من هذا النوع.");

        var field = new AdFormFieldOverride
        {
            Id = Guid.NewGuid(),
            CategoryId = categoryId,
            SubCategoryId = subCategoryId,
            FieldName = name,
            IsCustom = true,
            Label = request.LabelAr.Trim(),
            LabelEn = Normalize(request.LabelEn),
            Type = request.Type.Trim(),
            Section = Normalize(request.Section),
            Placeholder = Normalize(request.Placeholder),
            HelpText = Normalize(request.HelpText),
            SortOrder = request.SortOrder,
            IsRequired = request.IsRequired,
            MinLength = request.MinLength,
            MaxLength = request.MaxLength,
            MinValue = request.MinValue,
            MaxValue = request.MaxValue,
            Pattern = Normalize(request.Pattern),
            PatternMessage = Normalize(request.PatternMessage),
            MaxSelections = request.MaxSelections,
            VisibleWhenField = Normalize(request.VisibleWhenField),
            VisibleWhenValues = AdFormOverrideApplier.SerializeValues(request.VisibleWhenValues),
            RequiredWhenField = Normalize(request.RequiredWhenField),
            RequiredWhenValues = AdFormOverrideApplier.SerializeValues(request.RequiredWhenValues),
            CreatedAt = DateTime.UtcNow,
            UpdatedBy = adminUserId
        };

        foreach (var option in options)
            field.Options.Add(BuildOption(field.Id, option));

        _repository.AddOverride(field);
        await _repository.SaveChangesAsync(cancellationToken);

        return MapCustom(field);
    }

    public async Task<AdminFormFieldDto> UpdateFieldAsync(
        Guid fieldId, string adminUserId, UpdateFormFieldRequest request,
        CancellationToken cancellationToken = default)
    {
        var field = await _repository.FindOverrideAsync(fieldId, cancellationToken)
            ?? throw new NotFoundException("الحقل غير موجود.");

        var catalogField = FindCatalogField(field);

        if (request.LabelAr is not null)
            field.Label = Normalize(request.LabelAr);

        if (request.LabelEn is not null)
            field.LabelEn = Normalize(request.LabelEn);

        if (request.IsRequired is { } isRequired)
        {
            if (!isRequired && catalogField is { Required: true })
                throw new BadRequestException(
                    $"الحقل '{field.FieldName}' إجباري في المنصة ولا يمكن جعله اختياريًا.");

            field.IsRequired = isRequired;
        }

        if (request.IsHidden is { } isHidden)
        {
            if (isHidden && catalogField is { Required: true })
                throw new BadRequestException(
                    $"الحقل '{field.FieldName}' إجباري في المنصة ولا يمكن إخفاؤه.");

            field.IsHidden = isHidden;
        }

        if (request.SortOrder is { } sortOrder)
            field.SortOrder = sortOrder;

        if (request.Section is not null)
            field.Section = Normalize(request.Section);

        if (request.Placeholder is not null)
            field.Placeholder = Normalize(request.Placeholder);

        if (request.HelpText is not null)
            field.HelpText = Normalize(request.HelpText);

        if (request.MinLength is not null) field.MinLength = request.MinLength;
        if (request.MaxLength is not null) field.MaxLength = request.MaxLength;
        if (request.MinValue is not null) field.MinValue = request.MinValue;
        if (request.MaxValue is not null) field.MaxValue = request.MaxValue;
        if (request.MaxSelections is not null) field.MaxSelections = request.MaxSelections;
        if (request.Pattern is not null) field.Pattern = Normalize(request.Pattern);
        if (request.PatternMessage is not null) field.PatternMessage = Normalize(request.PatternMessage);

        if (request.ClearConditions)
        {
            field.VisibleWhenField = null;
            field.VisibleWhenValues = null;
            field.RequiredWhenField = null;
            field.RequiredWhenValues = null;
        }
        else
        {
            if (request.VisibleWhenField is not null)
            {
                field.VisibleWhenField = Normalize(request.VisibleWhenField);
                field.VisibleWhenValues = AdFormOverrideApplier.SerializeValues(request.VisibleWhenValues);
            }

            if (request.RequiredWhenField is not null)
            {
                field.RequiredWhenField = Normalize(request.RequiredWhenField);
                field.RequiredWhenValues = AdFormOverrideApplier.SerializeValues(request.RequiredWhenValues);
            }
        }

        field.UpdatedAt = DateTime.UtcNow;
        field.UpdatedBy = adminUserId;

        await _repository.SaveChangesAsync(cancellationToken);

        return await ReadFieldAsync(field, cancellationToken);
    }

    public async Task DeleteFieldAsync(Guid fieldId, CancellationToken cancellationToken = default)
    {
        var field = await _repository.FindOverrideAsync(fieldId, cancellationToken)
            ?? throw new NotFoundException("الحقل غير موجود.");

        _repository.RemoveOverride(field);
        await _repository.SaveChangesAsync(cancellationToken);
    }

    public async Task ResetFieldAsync(
        int categoryId, int? subCategoryId, string fieldName, CancellationToken cancellationToken = default)
    {
        var field = await _repository.FindOverrideAsync(
            categoryId, subCategoryId, fieldName.Trim(), cancellationToken);

        if (field is null)
            return;

        _repository.RemoveOverride(field);
        await _repository.SaveChangesAsync(cancellationToken);
    }

    public async Task<AdminFormOptionDto> AddOptionAsync(
        Guid fieldId, string adminUserId, SaveFormOptionRequest request,
        CancellationToken cancellationToken = default)
    {
        var field = await _repository.FindOverrideAsync(fieldId, cancellationToken)
            ?? throw new NotFoundException("الحقل غير موجود.");

        var effectiveType = field.Type ?? FindCatalogField(field)?.Type;

        if (!AdFormOverrideApplier.SupportsOptions(effectiveType))
            throw new BadRequestException(
                "لا يمكن إضافة خيارات إلا إلى حقول من نوع select أو radio أو multiselect.");

        var value = request.Value.Trim();

        if (field.Options.Any(option =>
                string.Equals(option.Value, value, StringComparison.OrdinalIgnoreCase)))
        {
            throw new ConflictException($"الخيار '{value}' موجود بالفعل في هذا الحقل.");
        }

        var stored = BuildOption(field.Id, request);

        _repository.AddOption(stored);

        field.UpdatedAt = DateTime.UtcNow;
        field.UpdatedBy = adminUserId;

        await _repository.SaveChangesAsync(cancellationToken);

        return MapOption(stored);
    }

    public async Task<AdminFormOptionDto> UpdateOptionAsync(
        Guid optionId, SaveFormOptionRequest request, CancellationToken cancellationToken = default)
    {
        var option = await _repository.FindOptionAsync(optionId, cancellationToken)
            ?? throw new NotFoundException("الخيار غير موجود.");

        option.Value = request.Value.Trim();
        option.Label = request.Label.Trim();
        option.LabelEn = Normalize(request.LabelEn);
        option.Group = Normalize(request.Group);
        option.SortOrder = request.SortOrder;
        option.IsActive = request.IsActive;

        await _repository.SaveChangesAsync(cancellationToken);

        return MapOption(option);
    }

    public async Task DeleteOptionAsync(Guid optionId, CancellationToken cancellationToken = default)
    {
        var option = await _repository.FindOptionAsync(optionId, cancellationToken)
            ?? throw new NotFoundException("الخيار غير موجود.");

        _repository.RemoveOption(option);
        await _repository.SaveChangesAsync(cancellationToken);
    }

    private async Task<(CategorySelectionResolver.CategorySelection Selection, AdFormSchema Schema)> ResolveAsync(
        int categoryId, int? subCategoryId, CancellationToken cancellationToken)
    {
        var selection = await _selectionResolver.ResolveAsync(categoryId, subCategoryId, cancellationToken);

        if (selection.RequiresSubCategory)
            throw new BadRequestException("يجب اختيار قسم فرعي لعرض نموذج الإعلان.");

        var schema = AdFormSchemaCatalog.GetSchema(categoryId, subCategoryId)
            ?? throw new NotFoundException("لا يوجد نموذج إعلان لهذا القسم.");

        return (selection, schema);
    }

    private static FormFieldDto? FindCatalogField(AdFormFieldOverride field)
    {
        var schema = AdFormSchemaCatalog.GetSchema(field.CategoryId, field.SubCategoryId);

        return schema?.Fields.FirstOrDefault(item =>
            string.Equals(item.Name, field.FieldName, StringComparison.OrdinalIgnoreCase));
    }

    private async Task<AdminFormFieldDto> ReadFieldAsync(
        AdFormFieldOverride field, CancellationToken cancellationToken)
    {
        var form = await GetFormAsync(field.CategoryId, field.SubCategoryId, cancellationToken);

        return form.Fields.First(item =>
            string.Equals(item.Name, field.FieldName, StringComparison.OrdinalIgnoreCase));
    }

    private static AdminFormFieldDto MapCustom(AdFormFieldOverride field)
    {
        var rendered = AdFormOverrideApplier.BuildCustom(field);

        return new AdminFormFieldDto
        {
            FieldId = field.Id,
            Name = rendered.Name,
            Label = rendered.Label,
            LabelEn = rendered.LabelEn,
            Type = rendered.Type,
            Required = rendered.Required,
            Visible = !field.IsHidden,
            Order = rendered.Order,
            Section = rendered.Section,
            Placeholder = rendered.Placeholder,
            HelpText = rendered.HelpText,

            Origin = AdminFormFieldOrigin.Custom,
            IsOverridden = true,

            IsPersisted = false,
            CanRelaxRequirement = true,

            Options = field.Options
                .OrderBy(option => option.SortOrder)
                .ThenBy(option => option.Label, StringComparer.Ordinal)
                .Select(MapOption)
                .ToList(),
            SupportsOptions = AdFormOverrideApplier.SupportsOptions(rendered.Type),

            MinLength = rendered.MinLength,
            MaxLength = rendered.MaxLength,
            MinValue = rendered.MinValue,
            MaxValue = rendered.MaxValue,
            Pattern = rendered.Pattern,
            PatternMessage = rendered.PatternMessage,
            MaxSelections = rendered.MaxSelections,
            VisibleWhen = rendered.VisibleWhen,
            RequiredWhen = rendered.RequiredWhen
        };
    }

    private static IReadOnlyList<AdminFormOptionDto>? MapOptions(
        FormFieldDto effective, AdFormFieldOverride? edit)
    {
        if (edit is not null && edit.Options.Count > 0)
        {
            return edit.Options
                .OrderBy(option => option.SortOrder)
                .ThenBy(option => option.Label, StringComparer.Ordinal)
                .Select(MapOption)
                .ToList();
        }

        if (effective.Options is not { Count: > 0 })
            return null;

        return effective.Options
            .Select((option, index) => new AdminFormOptionDto
            {
                Id = null,
                Value = option.Value?.ToString() ?? string.Empty,
                Label = option.Label,
                LabelEn = option.LabelEn,
                Group = option.Group,
                SortOrder = index,
                IsActive = true,
                IsCustom = false
            })
            .ToList();
    }

    private static AdminFormOptionDto MapOption(AdFormFieldOption option) =>
        new()
        {
            Id = option.Id,
            Value = option.Value,
            Label = option.Label,
            LabelEn = option.LabelEn,
            Group = option.Group,
            SortOrder = option.SortOrder,
            IsActive = option.IsActive,
            IsCustom = true
        };

    private static AdFormFieldOption BuildOption(Guid fieldId, SaveFormOptionRequest request) =>
        new()
        {
            Id = Guid.NewGuid(),
            FieldOverrideId = fieldId,
            Value = request.Value.Trim(),
            Label = request.Label.Trim(),
            LabelEn = Normalize(request.LabelEn),
            Group = Normalize(request.Group),
            SortOrder = request.SortOrder,
            IsActive = request.IsActive
        };

    private static string? Normalize(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
