using System.Net;
using System.Text.RegularExpressions;
using MarkatPlace.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Xunit;

namespace MarkatPlace.Tests;

public class ReverseProxyHostingTests
{
    private const string Loopback = "127.0.0.1";
    private const string PublicClient = "197.54.10.20";

    [Fact]
    public void All_three_forwarded_headers_are_honoured()
    {
        var options = Resolve(Configure());

        Assert.Equal(
            ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost,
            options.ForwardedHeaders);
    }

    [Fact]
    public void A_single_reverse_proxy_is_the_default_forward_limit()
    {
        Assert.Equal(1, Resolve(Configure()).ForwardLimit);
    }

    [Theory]
    [InlineData("127.0.0.1")]
    [InlineData("127.0.0.53")]
    [InlineData("::1")]
    [InlineData("::ffff:127.0.0.1")]
    public async Task Nginx_on_the_same_host_is_trusted_without_any_configuration(string proxyAddress)
    {
        var forwarded = await ForwardAsync(Resolve(Configure()), proxyAddress, PublicClient, "https");

        Assert.Equal("https", forwarded.Scheme);
        Assert.Equal(PublicClient, forwarded.RemoteIp);
    }

    [Fact]
    public async Task Https_at_Nginx_reaches_the_application_as_https()
    {
        var forwarded = await ForwardAsync(
            Resolve(Configure()), Loopback, PublicClient, "https", "markatplace.example");

        Assert.Equal("https", forwarded.Scheme);
        Assert.Equal("markatplace.example", forwarded.Host);
    }

    [Fact]
    public async Task An_unknown_caller_cannot_forge_the_scheme_the_host_or_the_client_ip()
    {
        var forwarded = await ForwardAsync(
            Resolve(Configure()), "203.0.113.9", "10.10.10.10", "https", "evil.example");

        Assert.Equal("http", forwarded.Scheme);
        Assert.Equal("kestrel.internal", forwarded.Host);
        Assert.Equal("203.0.113.9", forwarded.RemoteIp);
    }

    [Fact]
    public async Task A_proxy_on_another_host_is_trusted_once_it_is_listed()
    {
        var options = Resolve(Configure(("ForwardedHeaders:KnownProxies:0", "203.0.113.9")));

        var forwarded = await ForwardAsync(options, "203.0.113.9", PublicClient, "https");

        Assert.Equal("https", forwarded.Scheme);
        Assert.Equal(PublicClient, forwarded.RemoteIp);
    }

    [Fact]
    public async Task A_proxy_network_is_trusted_once_it_is_listed()
    {
        var options = Resolve(Configure(("ForwardedHeaders:KnownNetworks:0", "10.0.0.0/8")));

        var forwarded = await ForwardAsync(options, "10.4.2.1", PublicClient, "https");

        Assert.Equal("https", forwarded.Scheme);
        Assert.Equal(PublicClient, forwarded.RemoteIp);
    }

    [Fact]
    public async Task Configuring_a_proxy_does_not_stop_loopback_from_being_trusted()
    {
        var options = Resolve(Configure(("ForwardedHeaders:KnownProxies:0", "203.0.113.9")));

        var forwarded = await ForwardAsync(options, Loopback, PublicClient, "https");

        Assert.Equal("https", forwarded.Scheme);
    }

    [Fact]
    public async Task TrustAnyProxy_restores_unconditional_trust()
    {
        var options = Resolve(Configure(("ForwardedHeaders:TrustAnyProxy", "true")));

        Assert.Empty(options.KnownProxies);
        Assert.Empty(options.KnownIPNetworks);

        var forwarded = await ForwardAsync(options, "203.0.113.9", PublicClient, "https");

        Assert.Equal("https", forwarded.Scheme);
    }

    [Fact]
    public void An_unparseable_known_proxy_fails_start_up()
    {
        var exception = Assert.Throws<InvalidOperationException>(
            () => Resolve(Configure(("ForwardedHeaders:KnownProxies:0", "nginx.internal"))));

        Assert.Contains("nginx.internal", exception.Message);
    }

    [Fact]
    public void An_unparseable_known_network_fails_start_up()
    {
        var exception = Assert.Throws<InvalidOperationException>(
            () => Resolve(Configure(("ForwardedHeaders:KnownNetworks:0", "10.0.0.0/33"))));

        Assert.Contains("10.0.0.0/33", exception.Message);
    }

    [Fact]
    public async Task A_known_network_written_with_host_bits_still_matches()
    {
        var options = Resolve(Configure(("ForwardedHeaders:KnownNetworks:0", "10.0.0.1/8")));

        Assert.Equal("https", (await ForwardAsync(options, "10.4.2.1", PublicClient, "https")).Scheme);
    }

    [Fact]
    public void A_forward_limit_below_one_fails_start_up()
    {
        Assert.Throws<InvalidOperationException>(
            () => Resolve(Configure(("ForwardedHeaders:ForwardLimit", "0"))));
    }

    [Fact]
    public async Task The_shipped_production_configuration_trusts_Nginx_on_the_same_host_and_nobody_else()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile(System.IO.Path.Combine(RepositoryRoot.Path, "MarkatPlace", "appsettings.Production.json"))
            .Build();

        var options = Resolve(configuration);

        Assert.Equal(1, options.ForwardLimit);
        Assert.Equal("https", (await ForwardAsync(options, Loopback, PublicClient, "https")).Scheme);
        Assert.Equal("http", (await ForwardAsync(options, "203.0.113.9", PublicClient, "https")).Scheme);
    }

    [Fact]
    public void No_production_code_path_requires_IIS()
    {
        foreach (var file in ApplicationSourceFiles())
        {
            var source = File.ReadAllText(file);
            var name = RepositoryRoot.Relative(file);

            Assert.DoesNotContain("UseIISIntegration", source, StringComparison.Ordinal);
            Assert.DoesNotContain("UseIIS(", source, StringComparison.Ordinal);
            Assert.DoesNotContain("UseHttpSys", source, StringComparison.Ordinal);
            Assert.False(source.Contains("Microsoft.AspNetCore.Server.IIS", StringComparison.Ordinal),
                $"{name} takes a dependency on the IIS server. Production is Kestrel behind Nginx on Linux.");
        }
    }

    [Fact]
    public void IIS_request_limits_are_applied_only_where_IIS_can_exist()
    {
        var guarded = new Regex(
            @"OperatingSystem\.IsWindows\(\)[^{}]*\{[^{}]*IISServerOptions", RegexOptions.Singleline);

        foreach (var file in ApplicationSourceFiles())
        {
            var source = File.ReadAllText(file);

            if (!source.Contains("IISServerOptions", StringComparison.Ordinal))
                continue;

            Assert.True(guarded.IsMatch(source),
                $"{RepositoryRoot.Relative(file)} configures IISServerOptions outside a Windows guard. " +
                "Kestrel never reads it, and production is Kestrel behind Nginx on Linux.");
        }
    }

    [Fact]
    public void The_shipped_configuration_hard_codes_no_listening_address()
    {
        foreach (var file in new[] { "MarkatPlace/appsettings.json", "MarkatPlace/appsettings.Production.json" })
        {
            var settings = RepositoryRoot.ReadFile(file);

            Assert.False(settings.Contains("\"Kestrel\"", StringComparison.Ordinal),
                $"{file} pins a Kestrel endpoint. Endpoint configuration overrides ASPNETCORE_URLS, so the " +
                "hosting environment could no longer choose the address Nginx proxies to.");

            Assert.False(settings.Contains("\"urls\"", StringComparison.OrdinalIgnoreCase),
                $"{file} pins a listening address. It belongs in ASPNETCORE_URLS on the server.");
        }
    }

    private static IEnumerable<string> ApplicationSourceFiles() =>
        RepositoryRoot.SourceFiles().Where(file => !file.Contains("/Tests/", StringComparison.Ordinal));

    private static IConfiguration Configure(params (string Key, string Value)[] values) =>
        new ConfigurationBuilder()
            .AddInMemoryCollection(values.Select(v => new KeyValuePair<string, string?>(v.Key, v.Value)))
            .Build();

    private static ForwardedHeadersOptions Resolve(IConfiguration configuration)
    {
        var services = new ServiceCollection();
        services.AddReverseProxyForwardedHeaders(configuration);

        return services.BuildServiceProvider()
            .GetRequiredService<IOptions<ForwardedHeadersOptions>>()
            .Value;
    }

    private static async Task<(string Scheme, string? Host, string? RemoteIp)> ForwardAsync(
        ForwardedHeadersOptions options,
        string proxyAddress,
        string forwardedFor,
        string forwardedProto,
        string? forwardedHost = null)
    {
        var context = new DefaultHttpContext();
        context.Request.Scheme = "http";
        context.Request.Host = new HostString("kestrel.internal");
        context.Connection.RemoteIpAddress = IPAddress.Parse(proxyAddress);
        context.Request.Headers["X-Forwarded-For"] = forwardedFor;
        context.Request.Headers["X-Forwarded-Proto"] = forwardedProto;

        if (forwardedHost is not null)
            context.Request.Headers["X-Forwarded-Host"] = forwardedHost;

        var middleware = new ForwardedHeadersMiddleware(
            _ => Task.CompletedTask, NullLoggerFactory.Instance, Options.Create(options));

        await middleware.Invoke(context);

        return (context.Request.Scheme, context.Request.Host.Value, context.Connection.RemoteIpAddress?.ToString());
    }
}
