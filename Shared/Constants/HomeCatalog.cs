using Shared.Enums;

namespace Shared.Constants;

public static class HomeCatalog
{
    public static readonly IReadOnlyDictionary<HomeSectionType, string> SectionTypeNames =
        new Dictionary<HomeSectionType, string>
        {
            [HomeSectionType.Hero] = "الواجهة الرئيسية",
            [HomeSectionType.Slider1] = "السلايدر الأول",
            [HomeSectionType.Slider2] = "السلايدر الثاني",
            [HomeSectionType.CategoryStrip] = "شريط الأقسام",
            [HomeSectionType.RecentlyViewed] = "شوهد مؤخرًا",
            [HomeSectionType.CategorySection] = "قسم إعلانات"
        };

    public static string GetSectionTypeName(HomeSectionType type) =>
        SectionTypeNames.TryGetValue(type, out var name) ? name : type.ToString();
}
