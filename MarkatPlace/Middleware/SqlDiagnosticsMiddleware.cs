using System.Diagnostics;
using Persistence.Data;

namespace MarkatPlace.Middleware;

public sealed class SqlDiagnosticsMiddleware
{
    private readonly RequestDelegate _next;

    public SqlDiagnosticsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, RequestSqlCounter counter)
    {
        var stopwatch = Stopwatch.StartNew();

        context.Response.OnStarting(() =>
        {
            stopwatch.Stop();
            var headers = context.Response.Headers;
            headers["X-Sql-Count"] = counter.Count.ToString();
            headers["X-Sql-Ms"] = counter.ElapsedMilliseconds.ToString("F2");
            headers["X-Elapsed-Ms"] = stopwatch.Elapsed.TotalMilliseconds.ToString("F2");
            return Task.CompletedTask;
        });

        await _next(context);
    }
}
