using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace MarkatPlace.Tests;

internal static class ApiRouteIndex
{
    private static readonly Regex Placeholder = new(@"\{[^}]*\}", RegexOptions.Compiled);

    public static IReadOnlySet<string> Routes { get; } = Build();

    public static bool Exists(string httpMethod, string endpoint) =>
        Routes.Contains(Key(httpMethod, endpoint));

    public static string Key(string httpMethod, string template) =>
        $"{httpMethod.ToUpperInvariant()} {Normalize(template)}";

    private static string Normalize(string template) =>
        Placeholder.Replace(template.Trim().TrimStart('~').Trim('/'), "{}").ToLowerInvariant();

    private static IReadOnlySet<string> Build()
    {
        var routes = new HashSet<string>(StringComparer.Ordinal);

        var controllers = typeof(Presentation.Controllers.LookupsController).Assembly
            .GetTypes()
            .Where(type => type is { IsClass: true, IsAbstract: false } &&
                           typeof(ControllerBase).IsAssignableFrom(type));

        foreach (var controller in controllers)
        {
            var prefixes = controller.GetCustomAttributes<RouteAttribute>(inherit: true)
                .Select(route => route.Template)
                .DefaultIfEmpty(string.Empty)
                .ToList();

            var actions = controller.GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .Where(method => !method.IsSpecialName);

            foreach (var action in actions)
            {
                foreach (var verb in action.GetCustomAttributes<HttpMethodAttribute>(inherit: true))
                {
                    foreach (var prefix in prefixes)
                    {
                        var template = verb.Template is null
                            ? prefix
                            : verb.Template.StartsWith('~') || verb.Template.StartsWith('/')
                                ? verb.Template
                                : $"{prefix}/{verb.Template}";

                        foreach (var method in verb.HttpMethods)
                        {
                            foreach (var variant in OptionalVariants(template))
                                routes.Add(Key(method, variant));
                        }
                    }
                }
            }
        }

        return routes;
    }

    private static IEnumerable<string> OptionalVariants(string template)
    {
        yield return template;

        var trimmed = template.TrimEnd('/');
        var last = trimmed.LastIndexOf('/');

        if (last >= 0 && Regex.IsMatch(trimmed[(last + 1)..], @"^\{[^}]*\?\}$"))
            yield return trimmed[..last];
    }
}
