using System.Net;
using Microsoft.AspNetCore.HttpOverrides;

namespace MarkatPlace.Extensions;

public static class ForwardedHeadersExtensions
{
    public const string SectionName = "ForwardedHeaders";

    public static IServiceCollection AddReverseProxyForwardedHeaders(
        this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection(SectionName);

        var trustAnyProxy = section.GetValue("TrustAnyProxy", false);
        var forwardLimit = section.GetValue<int?>("ForwardLimit") ?? 1;

        if (forwardLimit < 1)
        {
            throw new InvalidOperationException(
                $"{SectionName}:ForwardLimit must be at least 1. It is the number of reverse proxies " +
                "between the client and this application: 1 for a single Nginx in front of Kestrel.");
        }

        var knownProxies = (section.GetSection("KnownProxies").Get<string[]>() ?? [])
            .Select(ParseProxy)
            .ToArray();

        var knownNetworks = (section.GetSection("KnownNetworks").Get<string[]>() ?? [])
            .Select(ParseNetwork)
            .ToArray();

        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor
                                     | Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto
                                     | Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedHost;

            options.ForwardLimit = forwardLimit;

            options.KnownProxies.Clear();
            options.KnownIPNetworks.Clear();

            if (trustAnyProxy)
                return;

            options.KnownProxies.Add(IPAddress.IPv6Loopback);
            options.KnownIPNetworks.Add(System.Net.IPNetwork.Parse("127.0.0.0/8"));

            foreach (var proxy in knownProxies)
                options.KnownProxies.Add(proxy);

            foreach (var network in knownNetworks)
                options.KnownIPNetworks.Add(network);
        });

        return services;
    }

    private static IPAddress ParseProxy(string value)
    {
        if (IPAddress.TryParse(value, out var address))
            return address;

        throw new InvalidOperationException(
            $"{SectionName}:KnownProxies contains '{value}', which is not an IP address. Use the address " +
            "the reverse proxy connects from, for example '10.0.0.5'. Loopback is always trusted and " +
            "does not need to be listed.");
    }

    private static System.Net.IPNetwork ParseNetwork(string value)
    {
        if (System.Net.IPNetwork.TryParse(value, out var network))
            return network;

        throw new InvalidOperationException(
            $"{SectionName}:KnownNetworks contains '{value}', which is not a CIDR network. Use " +
            "'address/prefix', for example '10.0.0.0/8'. Loopback is always trusted and does not " +
            "need to be listed.");
    }
}
