using System.Text.Json;
using System.Text.RegularExpressions;
using MarkatPlace.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.Constants;
using Shared.DTOs.Auth;
using Shared.Settings;
using Xunit;

namespace MarkatPlace.Tests;

public class GoogleAuthConfigurationTests
{
    private const string GoogleSecretPrefix = "GOCSPX-";

    private static readonly string[] ConfigurationFiles =
    [
        "MarkatPlace/appsettings.json",
        "MarkatPlace/appsettings.Development.json",
        "MarkatPlace/appsettings.Production.json"
    ];

    private static JsonElement Read(string relativePath) =>
        JsonDocument.Parse(RepositoryRoot.ReadFile(relativePath)).RootElement;

    [Fact]
    public void No_source_file_carries_a_Google_client_secret()
    {
        var offenders = RepositoryRoot.SourceFiles()
            .Where(file => !file.EndsWith(nameof(GoogleAuthConfigurationTests) + ".cs", StringComparison.Ordinal))
            .Where(file => File.ReadAllText(file).Contains(GoogleSecretPrefix, StringComparison.Ordinal))
            .Select(RepositoryRoot.Relative)
            .ToList();

        Assert.True(offenders.Count == 0,
            "A Google client secret is committed in source. Supply it as the environment variable " +
            "GoogleAuth__ClientSecret instead:" + Environment.NewLine + string.Join(Environment.NewLine, offenders));
    }

    [Fact]
    public void No_committed_configuration_file_carries_a_Google_client_secret()
    {
        foreach (var file in ConfigurationFiles)
        {
            var text = RepositoryRoot.ReadFile(file);

            Assert.DoesNotContain(GoogleSecretPrefix, text, StringComparison.Ordinal);
            Assert.DoesNotContain("\"ClientSecret\":", text, StringComparison.Ordinal);
        }
    }

    [Fact]
    public void The_client_id_is_committed_because_the_browser_has_to_send_it_to_Google_anyway()
    {
        var google = Read("MarkatPlace/appsettings.json").GetProperty(GoogleAuthSettings.SectionName);

        var clientId = google.GetProperty("ClientId").GetString();

        Assert.False(string.IsNullOrWhiteSpace(clientId));
        Assert.EndsWith(".apps.googleusercontent.com", clientId, StringComparison.Ordinal);
    }

    [Fact]
    public void A_verified_e_mail_is_required_before_an_account_is_created_from_a_Google_identity()
    {
        var google = Read("MarkatPlace/appsettings.json").GetProperty(GoogleAuthSettings.SectionName);

        Assert.True(google.GetProperty("RequireVerifiedEmail").GetBoolean());
        Assert.True(new GoogleAuthSettings().RequireVerifiedEmail);
    }

    [Fact]
    public void An_empty_client_secret_is_read_as_absent_rather_than_overriding_a_configured_one()
    {
        var settings = new GoogleAuthSettings { ClientId = "id", ClientSecret = "   " };

        Assert.Null(settings.ClientSecret);
        Assert.True(settings.IsConfigured);
        Assert.False(settings.CanExchangeAuthorizationCode);
    }

    [Fact]
    public void The_published_configuration_model_has_no_place_to_leak_a_secret()
    {
        var names = typeof(GoogleAuthConfigDto)
            .GetProperties()
            .Select(property => property.Name)
            .ToList();

        Assert.Equal(2, names.Count);
        Assert.Contains("Enabled", names);
        Assert.Contains("ClientId", names);
        Assert.DoesNotContain(names, name => name.Contains("Secret", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void No_Google_credential_is_ever_written_to_the_log()
    {
        string[] files =
        [
            "Infastrucre/Presitance/Services/GoogleTokenValidator.cs",
            "Core/Services/AuthService.cs"
        ];

        string[] forbidden =
        [
            "idToken", "IdToken", "request.Code", "ClientSecret", "_settings.ClientId", "access_token"
        ];

        foreach (var file in files)
        {
            var source = RepositoryRoot.ReadFile(file);

            foreach (Match call in Regex.Matches(source, @"_logger\.Log\w+\("))
            {
                var arguments = CSharpSource.ReadArgumentList(source, call.Index + call.Length);

                var values = string.Join(
                    ',',
                    CSharpSource.SplitArguments(arguments).Where(argument => !IsPlainLiteral(argument)));

                foreach (var name in forbidden)
                {
                    Assert.False(values.Contains(name, StringComparison.Ordinal),
                        $"{file} logs '{name}'. Credentials and secrets must never reach the log: " +
                        $"{arguments.Trim()}");
                }
            }
        }
    }

    private static bool IsPlainLiteral(string argument)
    {
        var trimmed = argument.TrimStart();

        return trimmed.StartsWith('"') || trimmed.StartsWith("@\"", StringComparison.Ordinal);
    }

    [Fact]
    public void The_identity_token_is_verified_against_Google_rather_than_decoded_locally()
    {
        var source = RepositoryRoot.ReadFile("Infastrucre/Presitance/Services/GoogleTokenValidator.cs");

        Assert.Contains("GoogleJsonWebSignature.ValidateAsync", source, StringComparison.Ordinal);
        Assert.Contains("Audience = [_settings.ClientId!]", source, StringComparison.Ordinal);
        Assert.Contains("ExpirationTimeClockTolerance = TimeSpan.Zero", source, StringComparison.Ordinal);
    }

    [Fact]
    public void The_sign_in_endpoint_never_accepts_an_e_mail_as_proof_of_a_Google_identity()
    {
        var names = typeof(GoogleSignInRequest)
            .GetProperties()
            .Select(property => property.Name)
            .ToList();

        Assert.DoesNotContain("Email", names);
        Assert.DoesNotContain("Subject", names);
        Assert.Contains("IdToken", names);
    }

    [Fact]
    public void The_referral_code_travels_with_the_Google_credential()
    {
        var names = typeof(GoogleSignInRequest)
            .GetProperties()
            .Select(property => property.Name)
            .ToList();

        Assert.Contains("ReferralCode", names);
    }
}

public class CorsConfigurationTests
{
    private static CorsSettings Committed()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile(Path.Combine(RepositoryRoot.Path, "MarkatPlace", "appsettings.json"))
            .Build();

        return configuration.GetSection(CorsSettings.SectionName).Get<CorsSettings>() ?? new CorsSettings();
    }

    [Fact]
    public void The_application_no_longer_answers_every_origin_on_the_internet()
    {
        var program = RepositoryRoot.ReadFile("MarkatPlace/Program.cs");

        Assert.DoesNotContain("AllowAnyOrigin()", program, StringComparison.Ordinal);
        Assert.Contains("AddApiCors(", program, StringComparison.Ordinal);
    }

    [Fact]
    public void The_production_site_can_call_the_API()
    {
        var settings = Committed();

        Assert.False(settings.AllowAnyOrigin);
        Assert.Contains("https://shopiklopik.com", settings.AllowedOrigins);
        Assert.Contains("https://www.shopiklopik.com", settings.AllowedOrigins);
    }

    [Fact]
    public void A_locally_served_frontend_can_call_the_API()
    {
        Assert.Contains("http://localhost:5173", Committed().AllowedOrigins);
    }

    [Fact]
    public void Every_configured_origin_is_a_scheme_host_and_port_with_no_path()
    {
        foreach (var origin in Committed().AllowedOrigins)
        {
            Assert.True(Uri.TryCreate(origin, UriKind.Absolute, out var uri), $"'{origin}' is not absolute.");
            Assert.True(uri!.Scheme is "http" or "https", $"'{origin}' is not an http origin.");
            Assert.Equal("/", uri.AbsolutePath);
            Assert.DoesNotContain("?", origin, StringComparison.Ordinal);
        }
    }

    [Fact]
    public void A_trailing_slash_in_configuration_does_not_silently_break_the_allow_list()
    {
        var settings = new CorsSettings { AllowedOrigins = ["https://shopiklopik.com/"] };

        Assert.Equal(["https://shopiklopik.com"], settings.AllowedOrigins);
    }

    [Fact]
    public void A_blank_entry_is_dropped_rather_than_allowed()
    {
        var settings = new CorsSettings { AllowedOrigins = ["", "   ", "https://shopiklopik.com"] };

        Assert.Single(settings.AllowedOrigins);
    }

    [Fact]
    public void An_origin_that_is_not_a_URL_stops_start_up_instead_of_being_ignored()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                [$"{CorsSettings.SectionName}:AllowedOrigins:0"] = "shopiklopik.com"
            })
            .Build();

        var exception = Assert.Throws<InvalidOperationException>(() =>
            new ServiceCollection().AddApiCors(configuration, new StubEnvironment()));

        Assert.Contains("AllowedOrigins", exception.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void The_committed_allow_list_is_accepted_by_start_up()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile(Path.Combine(RepositoryRoot.Path, "MarkatPlace", "appsettings.json"))
            .Build();

        var services = new ServiceCollection().AddApiCors(configuration, new StubEnvironment());

        Assert.NotEmpty(services);
    }

    [Fact]
    public void Uploaded_files_keep_their_own_permissive_header_so_images_still_load_everywhere()
    {
        var program = RepositoryRoot.ReadFile("MarkatPlace/Program.cs");

        Assert.Contains("headers[\"Access-Control-Allow-Origin\"] = \"*\";", program, StringComparison.Ordinal);
    }

    private sealed class StubEnvironment : IHostEnvironment
    {
        public string EnvironmentName { get; set; } = Environments.Production;
        public string ApplicationName { get; set; } = "MarkatPlace";
        public string ContentRootPath { get; set; } = RepositoryRoot.Path;
        public Microsoft.Extensions.FileProviders.IFileProvider ContentRootFileProvider { get; set; } =
            new Microsoft.Extensions.FileProviders.NullFileProvider();
    }
}
