using System.Globalization;
using FluentValidation.Resources;

namespace Services.Validation;

public class EgyptianArabicLanguageManager : ILanguageManager
{
    private static readonly LanguageManager Fallback = new();

    private static readonly Dictionary<string, string> Messages = new(StringComparer.Ordinal);

    public bool Enabled { get; set; } = true;

    public CultureInfo? Culture { get; set; }

    public string GetString(string key, CultureInfo? culture = null) =>
        Messages.TryGetValue(key, out var message) ? message : Fallback.GetString(key, culture);

    private static void Add(string key, string message) => Messages[key] = message;

    static EgyptianArabicLanguageManager()
    {
        Add("NotNullValidator", "الحقل ده مطلوب.");
        Add("NotEmptyValidator", "الحقل ده مطلوب.");
        Add("EmailValidator", "من فضلك اكتب بريد إلكتروني صحيح.");

        Add("LengthValidator", "الحقل ده لازم يكون بين {MinLength} و {MaxLength} حرف.");
        Add("MinimumLengthValidator", "الحقل ده لازم يكون {MinLength} حرف على الأقل.");
        Add("MaximumLengthValidator", "الحقل ده ما ينفعش يزيد عن {MaxLength} حرف.");
        Add("ExactLengthValidator", "الحقل ده لازم يكون {MaxLength} حرف بالظبط.");

        Add("GreaterThanValidator", "القيمة لازم تكون أكبر من {ComparisonValue}.");
        Add("GreaterThanOrEqualValidator", "القيمة لازم تكون {ComparisonValue} أو أكتر.");
        Add("LessThanValidator", "القيمة لازم تكون أقل من {ComparisonValue}.");
        Add("LessThanOrEqualValidator", "القيمة لازم تكون {ComparisonValue} أو أقل.");
        Add("EqualValidator", "القيمة لازم تساوي {ComparisonValue}.");
        Add("NotEqualValidator", "القيمة ما ينفعش تساوي {ComparisonValue}.");
        Add("InclusiveBetweenValidator", "القيمة لازم تكون بين {From} و {To}.");
        Add("ExclusiveBetweenValidator", "القيمة لازم تكون بين {From} و {To} من غير الطرفين.");

        Add("RegularExpressionValidator", "القيمة اللي دخلتها مش بالشكل المطلوب.");
        Add("EnumValidator", "القيمة اللي دخلتها مش صحيحة.");
        Add("CreditCardValidator", "من فضلك اكتب رقم كارت صحيح.");
        Add("PredicateValidator", "القيمة اللي دخلتها مش صحيحة.");
        Add("AsyncPredicateValidator", "القيمة اللي دخلتها مش صحيحة.");
        Add("EmptyValidator", "الحقل ده لازم يفضل فاضي.");
        Add("NullValidator", "الحقل ده لازم يفضل فاضي.");
        Add("ScalePrecisionValidator",
            "الرقم ما ينفعش يزيد عن {ExpectedPrecision} خانة، منهم {ExpectedScale} بعد العلامة العشرية.");
    }
}
