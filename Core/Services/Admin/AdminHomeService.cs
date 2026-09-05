using Domain.Entities;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Admin;
using Shared.DTOs.Advertisements;
using Shared.Enums;
using Shared.Exceptions;

namespace Services.Admin;

public class AdminHomeService : IAdminHomeService
{
    private readonly IAdminHomeRepository _repository;
    private readonly IAdminCatalogService _catalog;
    private readonly IFileService _fileService;
    private readonly IAdminAuditService _audit;

    public AdminHomeService(
        IAdminHomeRepository repository,
        IAdminCatalogService catalog,
        IFileService fileService,
        IAdminAuditService audit)
    {
        _repository = repository;
        _catalog = catalog;
        _fileService = fileService;
        _audit = audit;
    }

    private static string Describe(HomeSection section) =>
        $"{section.Title ?? section.Key} — {(section.IsVisible ? "ظاهر" : "مخفي")} — " +
        $"الترتيب: {section.SortOrder}";

    public async Task<AdminHomeConfigurationDto> GetAsync(CancellationToken cancellationToken = default)
    {
        var sections = await _repository.GetSectionsAsync(cancellationToken);
        var categories = await _catalog.GetCategoriesAsync(cancellationToken);

        return new AdminHomeConfigurationDto
        {
            Sections = sections.Select(Map).ToList(),
            Categories = categories
        };
    }

    public async Task<AdminHomeSectionDto> UpdateSectionAsync(
        int id, string adminUserId, UpdateHomeSectionRequest request,
        CancellationToken cancellationToken = default)
    {
        var section = await _repository.FindSectionAsync(id, cancellationToken)
            ?? throw new NotFoundException("القسم غير موجود.");

        var before = Describe(section);

        section.Title = Normalize(request.Title);
        section.TitleEn = Normalize(request.TitleEn);
        section.Subtitle = Normalize(request.Subtitle);
        section.IsVisible = request.IsVisible;
        section.SortOrder = request.SortOrder;

        if (section.Type == HomeSectionType.Hero)
        {
            section.LinkUrl = Normalize(request.LinkUrl);
            section.LinkText = Normalize(request.LinkText);
        }

        if (section.Type == HomeSectionType.CategorySection)
        {
            if (request.CategoryId is { } categoryId)
            {
                if (!await _repository.CategoryExistsAsync(categoryId, cancellationToken))
                    throw new NotFoundException("القسم المختار غير موجود.");

                section.CategoryId = categoryId;
            }

            section.ItemCount = request.ItemCount;
        }

        section.UpdatedAt = DateTime.UtcNow;

        await _repository.SaveChangesAsync(cancellationToken);

        await _audit.LogAsync(
            AdminAuditAction.UpdateHomeSection, AdminAuditCatalog.Targets.HomeSection,
            id.ToString(), $"تعديل قسم في الصفحة الرئيسية: {section.Key}",
            oldValue: before, newValue: Describe(section),
            adminUserId: adminUserId, cancellationToken: cancellationToken);

        var refreshed = await _repository.FindSectionAsync(id, cancellationToken);

        return Map(refreshed ?? section);
    }

    public async Task<AdminHomeSectionDto> UpdateSectionImageAsync(
        int id, string adminUserId, UploadImageModel? image,
        CancellationToken cancellationToken = default)
    {
        var section = await _repository.FindSectionAsync(id, cancellationToken)
            ?? throw new NotFoundException("القسم غير موجود.");

        if (section.Type != HomeSectionType.Hero)
            throw new BadRequestException("لا يمكن رفع صورة إلا لقسم Hero.");

        if (image is null)
            throw new BadRequestException("الصورة مطلوبة.");

        var stored = await _fileService.SaveAsync(image, ImageConstants.HomeFolder, cancellationToken);

        var previousPath = section.ImagePath;

        section.ImageUrl = stored.Url;
        section.ImagePath = stored.RelativePath;
        section.UpdatedAt = DateTime.UtcNow;

        try
        {
            await _repository.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            _fileService.Delete(stored.RelativePath);
            throw;
        }

        if (!string.IsNullOrWhiteSpace(previousPath))
            _fileService.Delete(previousPath);

        return Map(section);
    }

    public async Task<AdminHomeSectionDto> DeleteSectionImageAsync(
        int id, string adminUserId, CancellationToken cancellationToken = default)
    {
        var section = await _repository.FindSectionAsync(id, cancellationToken)
            ?? throw new NotFoundException("القسم غير موجود.");

        var previousPath = section.ImagePath;

        section.ImageUrl = null;
        section.ImagePath = null;
        section.UpdatedAt = DateTime.UtcNow;

        await _repository.SaveChangesAsync(cancellationToken);

        if (!string.IsNullOrWhiteSpace(previousPath))
            _fileService.Delete(previousPath);

        return Map(section);
    }

    public async Task<IReadOnlyList<AdminHomeSectionDto>> ReorderAsync(
        ReorderHomeSectionsRequest request, CancellationToken cancellationToken = default)
    {
        var sections = await _repository.GetSectionsForUpdateAsync(cancellationToken);

        var requested = request.SectionIds.ToList();

        var previousOrder = string.Join(", ", sections.Select(item => item.Id));

        if (requested.Count != sections.Count ||
            requested.Distinct().Count() != requested.Count ||
            requested.Except(sections.Select(section => section.Id)).Any())
        {
            throw new BadRequestException("يجب إرسال جميع أقسام الصفحة الرئيسية مرة واحدة بدون تكرار.");
        }

        var utcNow = DateTime.UtcNow;

        for (var position = 0; position < requested.Count; position++)
        {
            var section = sections.First(item => item.Id == requested[position]);

            section.SortOrder = position + 1;
            section.UpdatedAt = utcNow;
        }

        await _repository.SaveChangesAsync(cancellationToken);

        await _audit.LogAsync(
            AdminAuditAction.ReorderHomeSections, AdminAuditCatalog.Targets.HomeSection, null,
            "تغيير ترتيب أقسام الصفحة الرئيسية",
            oldValue: previousOrder, newValue: string.Join(", ", requested),
            cancellationToken: cancellationToken);

        var refreshed = await _repository.GetSectionsAsync(cancellationToken);

        return refreshed.Select(Map).ToList();
    }

    private static string? Normalize(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static AdminHomeSectionDto Map(HomeSection section) =>
        new()
        {
            Id = section.Id,
            Key = section.Key,
            Type = section.Type,
            TypeName = HomeCatalog.GetSectionTypeName(section.Type),
            Title = section.Title,
            TitleEn = section.TitleEn,
            Subtitle = section.Subtitle,
            IsVisible = section.IsVisible,
            SortOrder = section.SortOrder,
            ImageUrl = section.ImageUrl,
            LinkUrl = section.LinkUrl,
            LinkText = section.LinkText,
            CategoryId = section.CategoryId,
            CategoryName = section.Category?.NameAr,
            ItemCount = section.ItemCount,

            IsBannerSlider = section.Type is HomeSectionType.Slider1 or HomeSectionType.Slider2
        };
}
