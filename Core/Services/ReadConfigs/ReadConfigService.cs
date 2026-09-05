using AutoMapper;
using Services.Lookups;
using ServicesAbstraction;
using Shared.DTOs.Lookups;
using Shared.DTOs.Lookups.Read;
using Shared.Exceptions;

namespace Services.ReadConfigs;

public class ReadConfigService : IReadConfigService
{
    private readonly CategorySelectionResolver _selectionResolver;
    private readonly IMapper _mapper;

    public ReadConfigService(CategorySelectionResolver selectionResolver, IMapper mapper)
    {
        _selectionResolver = selectionResolver;
        _mapper = mapper;
    }

    public async Task<ReadConfigDto> GetReadConfigAsync(
        int categoryId, int? subCategoryId = null, CancellationToken cancellationToken = default)
    {
        var selection = await _selectionResolver.ResolveAsync(categoryId, subCategoryId, cancellationToken);

        if (selection.RequiresSubCategory)
            return Build(
                ReadConfigCatalog.SubCategorySelectionSchema(categoryId),
                selection,
                requiresSubCategory: true);

        var schema = ReadConfigCatalog.GetSchema(categoryId, subCategoryId)
            ?? throw new BadRequestException(
                "مفيش إعدادات عرض متاحة للقسم اللي اخترته.");

        return Build(schema, selection, requiresSubCategory: false);
    }

    private ReadConfigDto Build(
        ReadConfigSchema schema,
        CategorySelectionResolver.CategorySelection selection,
        bool requiresSubCategory) =>
        new()
        {
            Category = _mapper.Map<CategoryDto>(selection.Category),
            SubCategory = selection.SubCategory is null
                ? null
                : _mapper.Map<SubCategoryDto>(selection.SubCategory),
            RequiresSubCategory = requiresSubCategory,
            Module = schema.Module,

            List = schema.Find(ReadOperationKeys.List),
            Details = schema.Find(ReadOperationKeys.Details),
            Operations = schema.ToMap()
        };
}
