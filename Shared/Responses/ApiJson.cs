using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace Shared.Responses;

public static class ApiJson
{
    public static readonly JavaScriptEncoder Encoder = JavaScriptEncoder.Create(
        UnicodeRanges.BasicLatin,
        UnicodeRanges.Latin1Supplement,
        UnicodeRanges.Arabic,
        UnicodeRanges.ArabicSupplement,
        UnicodeRanges.ArabicExtendedA,
        UnicodeRanges.ArabicExtendedB,
        UnicodeRanges.ArabicPresentationFormsA,
        UnicodeRanges.ArabicPresentationFormsB);

    public static readonly JsonSerializerOptions SerializerOptions = Configure(new JsonSerializerOptions());

    public static JsonSerializerOptions Configure(JsonSerializerOptions options)
    {
        options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.Encoder = Encoder;
        options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

        return options;
    }
}
