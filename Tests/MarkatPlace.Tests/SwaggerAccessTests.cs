using System.Text;
using System.Text.Json;
using MarkatPlace.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Shared.Settings;
using Xunit;

namespace MarkatPlace.Tests;

public class SwaggerAccessTests
{
    private const string Username = "swagger-test-user";
    private const string Password = "swagger-test-password";

    private sealed class TestEnvironment : IHostEnvironment
    {
        public string EnvironmentName { get; set; } = "Production";
        public string ApplicationName { get; set; } = "MarkatPlace";
        public string ContentRootPath { get; set; } = AppContext.BaseDirectory;
        public IFileProvider ContentRootFileProvider { get; set; } = new NullFileProvider();
    }

    private sealed record Result(int StatusCode, string? Challenge, string? Location, bool Continued);

    private static string Encode(string username, string password) =>
        Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));

    private static async Task<Result> RunAsync(
        string path,
        string? authorization = null,
        string environmentName = "Production",
        bool https = true,
        bool configured = true,
        string method = "GET")
    {
        var settings = new SwaggerAuthSettings();

        if (configured)
        {
            settings.Username = Username;
            settings.Password = Password;
        }

        var context = new DefaultHttpContext();
        context.Request.Method = method;
        context.Request.Path = path;
        context.Request.Scheme = https ? "https" : "http";
        context.Request.Host = new HostString("api.example.com");
        context.Response.Body = new MemoryStream();

        if (authorization is not null)
            context.Request.Headers.Authorization = authorization;

        var continued = false;

        var middleware = new SwaggerBasicAuthMiddleware(
            _ =>
            {
                continued = true;
                return Task.CompletedTask;
            },
            Options.Create(settings),
            new TestEnvironment { EnvironmentName = environmentName });

        await middleware.InvokeAsync(context);

        var challenge = context.Response.Headers.WWWAuthenticate.ToString();
        var location = context.Response.Headers.Location.ToString();

        return new Result(
            context.Response.StatusCode,
            challenge.Length == 0 ? null : challenge,
            location.Length == 0 ? null : location,
            continued);
    }

    public static TheoryData<string> SwaggerPaths() =>
    [
        "/swagger",
        "/swagger/",
        "/swagger/index.html",
        "/swagger/index.js",
        "/swagger/swagger-ui.css",
        "/swagger/swagger-ui-bundle.js",
        "/swagger/swagger-ui-standalone-preset.js",
        "/swagger/favicon-32x32.png",
        "/swagger/v1/swagger.json",
        "/swagger/v2/swagger.json",
        "/swagger/anything/at/all"
    ];

    [Theory]
    [MemberData(nameof(SwaggerPaths))]
    public async Task Every_swagger_path_is_refused_without_credentials(string path)
    {
        var result = await RunAsync(path);

        Assert.Equal(StatusCodes.Status401Unauthorized, result.StatusCode);
        Assert.Equal("Basic realm=\"Swagger\"", result.Challenge);
        Assert.False(result.Continued);
    }

    [Theory]
    [MemberData(nameof(SwaggerPaths))]
    public async Task Every_swagger_path_is_served_with_the_right_credentials(string path)
    {
        var result = await RunAsync(path, "Basic " + Encode(Username, Password));

        Assert.True(result.Continued);
        Assert.Null(result.Challenge);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
    }

    [Theory]
    [InlineData("Basic")]
    [InlineData("Basic ")]
    [InlineData("Bearer eyJhbGciOiJIUzI1NiJ9.e30.signature")]
    [InlineData("Basic not-base-64!!")]
    [InlineData("Basic dGhlcmUtaXMtbm8tY29sb24=")]
    public async Task A_malformed_authorisation_header_is_refused(string authorization)
    {
        var result = await RunAsync("/swagger", authorization);

        Assert.Equal(StatusCodes.Status401Unauthorized, result.StatusCode);
        Assert.False(result.Continued);
    }

    [Theory]
    [InlineData(Username, "wrong-password")]
    [InlineData("wrong-user", Password)]
    [InlineData("wrong-user", "wrong-password")]
    [InlineData(Username, Password + "x")]
    [InlineData("", "")]
    public async Task Invalid_credentials_are_refused(string username, string password)
    {
        var result = await RunAsync("/swagger", "Basic " + Encode(username, password));

        Assert.Equal(StatusCodes.Status401Unauthorized, result.StatusCode);
        Assert.Equal("Basic realm=\"Swagger\"", result.Challenge);
        Assert.False(result.Continued);
    }

    [Fact]
    public async Task An_unconfigured_swagger_refuses_every_login_outside_development()
    {
        var result = await RunAsync(
            "/swagger", "Basic " + Encode(Username, Password), configured: false);

        Assert.Equal(StatusCodes.Status401Unauthorized, result.StatusCode);
        Assert.False(result.Continued);
    }

    [Fact]
    public async Task Credentials_are_never_asked_for_over_plain_http()
    {
        var result = await RunAsync("/swagger/v1/swagger.json", https: false);

        Assert.Equal(StatusCodes.Status302Found, result.StatusCode);
        Assert.Equal("https://api.example.com/swagger/v1/swagger.json", result.Location);
        Assert.Null(result.Challenge);
        Assert.False(result.Continued);
    }

    [Fact]
    public async Task Development_keeps_serving_swagger_without_credentials()
    {
        var result = await RunAsync(
            "/swagger", environmentName: "Development", configured: false, https: false);

        Assert.True(result.Continued);
        Assert.Null(result.Challenge);
    }

    [Fact]
    public async Task Development_honours_credentials_when_they_are_configured()
    {
        var refused = await RunAsync("/swagger", environmentName: "Development", https: false);

        Assert.Equal(StatusCodes.Status401Unauthorized, refused.StatusCode);
        Assert.False(refused.Continued);

        var allowed = await RunAsync(
            "/swagger", "Basic " + Encode(Username, Password),
            environmentName: "Development", https: false);

        Assert.True(allowed.Continued);
    }

    public static TheoryData<string> ApiPaths() =>
    [
        "/",
        "/api/ads",
        "/api/auth/login",
        "/api/v2/admin/users",
        "/api/lookups/read-config/1",
        "/health",
        "/health/live",
        "/hubs/notifications",
        "/uploads/ads/photo.webp",
        "/swaggerish",
        "/api/swagger"
    ];

    [Theory]
    [MemberData(nameof(ApiPaths))]
    public async Task No_other_path_is_touched_by_the_swagger_gate(string path)
    {
        foreach (var method in new[] { "GET", "POST", "PUT", "DELETE", "OPTIONS" })
        {
            var anonymous = await RunAsync(path, method: method);

            Assert.True(anonymous.Continued, $"{method} {path} was intercepted.");
            Assert.Null(anonymous.Challenge);
            Assert.Equal(StatusCodes.Status200OK, anonymous.StatusCode);

            var withJwt = await RunAsync(
                path, "Bearer eyJhbGciOiJIUzI1NiJ9.e30.signature", method: method);

            Assert.True(withJwt.Continued, $"{method} {path} with a JWT was intercepted.");
            Assert.Null(withJwt.Challenge);
        }
    }

    [Theory]
    [MemberData(nameof(ApiPaths))]
    public async Task No_other_path_is_redirected_to_https_by_the_swagger_gate(string path)
    {
        var result = await RunAsync(path, https: false);

        Assert.True(result.Continued);
        Assert.Null(result.Location);
    }

    [Theory]
    [InlineData("MarkatPlace/appsettings.json")]
    [InlineData("MarkatPlace/appsettings.Development.json")]
    [InlineData("MarkatPlace/appsettings.Production.json")]
    public void No_committed_configuration_file_carries_swagger_credentials(string file)
    {
        using var document = JsonDocument.Parse(RepositoryRoot.ReadFile(file));

        Assert.False(
            document.RootElement.TryGetProperty(SwaggerAuthSettings.SectionName, out _),
            $"{file} defines a {SwaggerAuthSettings.SectionName} section. The user name and the " +
            "password must only ever come from the server environment.");
    }

    [Fact]
    public void The_settings_treat_a_blank_value_as_no_credential_at_all()
    {
        Assert.False(new SwaggerAuthSettings().IsConfigured);
        Assert.False(new SwaggerAuthSettings { Username = "  ", Password = "x" }.IsConfigured);
        Assert.False(new SwaggerAuthSettings { Username = "x", Password = "" }.IsConfigured);
        Assert.False(new SwaggerAuthSettings { Username = "x" }.IsConfigured);
        Assert.False(new SwaggerAuthSettings { Password = "x" }.IsConfigured);
        Assert.True(new SwaggerAuthSettings { Username = "x", Password = "y" }.IsConfigured);
    }
}
