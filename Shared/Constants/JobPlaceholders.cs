namespace Shared.Constants;

public static class JobPlaceholders
{
    public const string Skills = "مثال: Excel، English، Programming، Communication، Leadership، Team Management";

    public const string RequestTitle = "مثال: مهندس مدني خبرة 5 سنوات يبحث عن عمل";

    public static readonly IReadOnlyList<string> RequestTitleExamples = new List<string>
    {
        "مهندس مدني خبرة 5 سنوات يبحث عن عمل",
        "محاسب خبرة بالحسابات المالية",
        "فني كهرباء يبحث عن فرصة عمل"
    };

    public const string RequestDescription =
        "اذكر في الوصف:\n" +
        "• نبذة مختصرة\n" +
        "• الخبرات السابقة\n" +
        "• الشركات السابقة\n" +
        "• المهارات\n" +
        "• الشهادات\n" +
        "• الدورات التدريبية\n" +
        "• الوظيفة المطلوبة\n" +
        "• ملاحظات إضافية";

    public const string JobTitle = "مثال: مطلوب محاسب";

    public static readonly IReadOnlyList<string> JobTitleExamples = new List<string>
    {
        "مطلوب محاسب",
        "مطلوب سائق",
        "مطلوب مهندس"
    };

    public const string JobDescription =
        "اذكر في وصف الوظيفة:\n" +
        "• المهام والمسؤوليات\n" +
        "• المتطلبات\n" +
        "• ساعات العمل\n" +
        "• مكان العمل";

    public const string OpportunityTitle = "اكتب عنوانًا واضحًا ومختصرًا";
}
