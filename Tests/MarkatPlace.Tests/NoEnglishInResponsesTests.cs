using System.Text;
using System.Text.RegularExpressions;
using Xunit;

namespace MarkatPlace.Tests;

public class NoEnglishInResponsesTests
{
    private static readonly string[] CallSites =
    [
        "ApiResponse.Ok(", "ApiResponse.Fail(",
        "new NotFoundException(", "new BadRequestException(", "new ConflictException(",
        "new ForbiddenException(", "new UnauthorizedException(", "new PaymentRequiredException(",
        ".WithMessage(",
    ];

    private static readonly Regex AllowedLatin = new(
        @"^(px|cm|mm|km|kg|EGP|MB|KB|GB|PDF|DOC|DOCX|MP4|MOV|JPG|JPEG|PNG|WEBP|GIF|BMP|TIFF|SVG|HEIF|AVIF|ICO|WhatsApp|Google|Maps|Spam|Organic|GPS|URL|SMS|OTP)$",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    public static IEnumerable<object[]> SourceFiles()
    {
        foreach (var file in RepositoryRoot.SourceFiles())
        {
            if (file.Contains("/Migrations/", StringComparison.Ordinal))
                continue;
            if (file.Contains("/Tests/", StringComparison.Ordinal))
                continue;

            yield return [file];
        }
    }

    [Theory]
    [MemberData(nameof(SourceFiles))]
    public void No_customer_facing_message_is_written_in_English(string file)
    {
        var source = File.ReadAllText(file);
        var offenders = new StringBuilder();

        foreach (var callSite in CallSites)
        {
            var index = 0;

            while ((index = source.IndexOf(callSite, index, StringComparison.Ordinal)) >= 0)
            {
                var callStart = index;
                index += callSite.Length;

                var argumentList = CSharpSource.ReadArgumentList(source, index);

                foreach (var argument in CSharpSource.SplitArguments(argumentList))
                {
                    if (ArabicText.IsArabic(argument))
                        continue;

                    var sentence = CSharpSource.StringLiterals(argument)
                        .FirstOrDefault(literal => literal.Trim().Length >= 4 && HasEnglishWords(literal));

                    if (sentence is null)
                        continue;

                    var line = source.Take(callStart).Count(character => character == '\n') + 1;
                    offenders.AppendLine(
                        $"  {RepositoryRoot.Relative(file)}:{line}  {callSite}\"{sentence}\"");
                }
            }
        }

        Assert.True(offenders.Length == 0,
            "A customer-facing message is not in Egyptian Arabic. Move the wording into " +
            $"Shared/Constants/UserMessages.cs and reference it:{Environment.NewLine}{offenders}");
    }

    private static bool HasEnglishWords(string literal) =>
        Regex.Matches(literal, "[A-Za-z]{2,}")
            .Select(match => match.Value)
            .Any(word => !AllowedLatin.IsMatch(word));

    [Fact]
    public void The_scanner_reads_an_interpolated_message_as_one_Arabic_sentence()
    {
        const string code =
            """.WithMessage($"المركز لازم يكون واحد من: {string.Join(", ", Centers)}.");""";

        var arguments = CSharpSource.ReadArgumentList(code, code.IndexOf('(') + 1);

        Assert.True(ArabicText.IsArabic(arguments));
        Assert.Single(CSharpSource.SplitArguments(arguments));
    }

    [Fact]
    public void The_scanner_still_reports_a_wholly_English_message()
    {
        const string code = """ApiResponse.Ok(result, "Land listing deleted successfully.");""";

        var arguments = CSharpSource.ReadArgumentList(code, code.IndexOf('(') + 1);
        var parts = CSharpSource.SplitArguments(arguments).ToList();

        Assert.Equal(2, parts.Count);
        Assert.False(ArabicText.IsArabic(parts[1]));
        Assert.Contains(CSharpSource.StringLiterals(parts[1]), HasEnglishWords);
    }

    [Fact]
    public void The_scanner_ignores_code_inside_an_interpolation_hole()
    {
        const string code = """new BadRequestException($"الصيغ المدعومة: {string.Join(", ", All)}.");""";

        var arguments = CSharpSource.ReadArgumentList(code, code.IndexOf('(') + 1);
        var literals = CSharpSource.StringLiterals(arguments).ToList();

        Assert.Single(literals);
        Assert.DoesNotContain(literals, HasEnglishWords);
    }
}
