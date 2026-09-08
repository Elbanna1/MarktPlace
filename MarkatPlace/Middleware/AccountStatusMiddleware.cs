using System.Security.Claims;
using System.Text.Json;
using Shared.Constants;
using Shared.Enums;
using Shared.Responses;

namespace MarkatPlace.Middleware;

public class AccountStatusMiddleware
{
    private static readonly JsonSerializerOptions SerializerOptions =
        new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    private readonly RequestDelegate _next;
    private readonly ILogger<AccountStatusMiddleware> _logger;

    public AccountStatusMiddleware(RequestDelegate next, ILogger<AccountStatusMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var user = context.User;

        if (user.Identity is not { IsAuthenticated: true })
        {
            await _next(context);
            return;
        }

        var claim = user.FindFirstValue(AuthConstants.AccountStatusClaimType);

        if (claim is null)
        {
            await _next(context);
            return;
        }

        if (claim == AuthConstants.AccountMissingClaimValue)
        {
            await RefuseAsync(
                context,
                StatusCodes.Status401Unauthorized,
                UserMessages.Account.UnknownAccount,
                "missing");

            return;
        }

        if (!int.TryParse(claim, out var value) ||
            !Enum.IsDefined(typeof(UserAccountStatus), value))
        {
            await RefuseAsync(
                context,
                StatusCodes.Status401Unauthorized,
                UserMessages.Account.UnknownAccount,
                claim);

            return;
        }

        var status = (UserAccountStatus)value;

        if (status == UserAccountStatus.Active)
        {
            await _next(context);
            return;
        }

        await RefuseAsync(
            context,
            StatusCodes.Status403Forbidden,
            status switch
            {
                UserAccountStatus.Blocked => UserMessages.Account.BlockedAccess,
                UserAccountStatus.Deactivated => UserMessages.Account.DeactivatedAccess,
                _ => UserMessages.Account.SuspendedAccess
            },
            status.ToString());
    }

    private async Task RefuseAsync(
        HttpContext context, int statusCode, string message, string state)
    {
        _logger.LogInformation(
            "Refused {Method} {Path} for account {UserId} in state {State}.",
            context.Request.Method,
            context.Request.Path,
            context.User.FindFirstValue(ClaimTypes.NameIdentifier),
            state);

        if (context.Response.HasStarted)
            return;

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(ApiResponse.Fail(message), SerializerOptions));
    }
}
