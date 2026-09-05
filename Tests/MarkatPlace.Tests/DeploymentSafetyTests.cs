using System.Text.RegularExpressions;
using Shared.Constants;
using Xunit;

namespace MarkatPlace.Tests;

public class DeploymentSafetyTests
{
    [Fact]
    public void The_committed_configuration_carries_no_JWT_signing_key()
    {
        var settings = RepositoryRoot.ReadFile("MarkatPlace/appsettings.json");

        Assert.DoesNotContain("\"SecretKey\"", settings);
        Assert.DoesNotContain("CHANGE-THIS-TO-A-LONG-RANDOM-SECRET", settings);
    }

    [Fact]
    public void The_production_configuration_carries_no_JWT_signing_key()
    {
        var settings = RepositoryRoot.ReadFile("MarkatPlace/appsettings.Production.json");

        Assert.DoesNotContain("\"SecretKey\"", settings);
    }

    [Fact]
    public void No_administrator_account_is_configured_for_production()
    {
        Assert.DoesNotContain("\"AdminUser\"", RepositoryRoot.ReadFile("MarkatPlace/appsettings.json"));
        Assert.DoesNotContain("\"AdminUser\": {",
            RepositoryRoot.ReadFile("MarkatPlace/appsettings.Production.json"));
    }

    [Fact]
    public void IIS_accepts_at_least_as_large_a_body_as_the_application_does()
    {
        var webConfig = RepositoryRoot.ReadFile("MarkatPlace/web.config");
        var match = Regex.Match(webConfig, @"maxAllowedContentLength=""(\d+)""");

        Assert.True(match.Success, "web.config declares no maxAllowedContentLength.");

        var iisLimit = long.Parse(match.Groups[1].Value);

        Assert.True(iisLimit >= FileUploadConstants.MaxRequestBodySizeBytes,
            $"IIS allows {iisLimit} bytes but the application accepts up to " +
            $"{FileUploadConstants.MaxRequestBodySizeBytes}. Requests between the two are rejected " +
            "by IIS with an HTML 404.13 page before any of our own validation runs.");
    }

    [Fact]
    public void Detailed_errors_and_stdout_logging_are_off_in_the_shipped_web_config()
    {
        var webConfig = Regex.Replace(
            RepositoryRoot.ReadFile("MarkatPlace/web.config"), "<!--.*?-->", string.Empty,
            RegexOptions.Singleline);

        Assert.DoesNotContain("ASPNETCORE_DETAILEDERRORS", webConfig);
        Assert.Contains("stdoutLogEnabled=\"false\"", webConfig);
    }

    [Fact]
    public void Every_accepted_upload_format_declares_a_content_type_and_at_least_one_extension()
    {
        var formats = ImageFormatCatalog.All
            .Select(f => (f.Name, f.CanonicalContentType, f.Extensions))
            .Concat(DocumentFormatCatalog.All
                .Select(f => (f.Name, f.CanonicalContentType, f.Extensions)))
            .Concat(VideoFormatCatalog.All
                .Select(f => (f.Name, f.CanonicalContentType, f.Extensions)));

        foreach (var (name, contentType, extensions) in formats)
        {
            Assert.False(string.IsNullOrWhiteSpace(contentType),
                $"{name} has no canonical content type.");
            Assert.NotEmpty(extensions);

            foreach (var extension in extensions)
                Assert.StartsWith(".", extension);
        }
    }
}
