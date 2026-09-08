using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Domain.Entities;
using MarkatPlace.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging.Abstractions;
using Persistence.Configurations;
using Persistence.Data;
using Persistence.Repositories;
using Persistence.Services;
using ServicesAbstraction;
using Shared.Constants;
using Shared.Enums;
using Xunit;

namespace MarkatPlace.Tests;

public class AccountStateEnforcementTests
{
    private static ClaimsPrincipal Principal(params Claim[] claims) =>
        new(new ClaimsIdentity(claims, "TestScheme"));

    private static async Task<(int StatusCode, string Body, bool Continued)> RunAsync(
        ClaimsPrincipal user)
    {
        var context = new DefaultHttpContext();
        context.User = user;
        context.Request.Method = "GET";
        context.Request.Path = "/api/profile";
        context.Response.Body = new MemoryStream();

        var continued = false;

        var middleware = new AccountStatusMiddleware(
            _ =>
            {
                continued = true;
                return Task.CompletedTask;
            },
            NullLogger<AccountStatusMiddleware>.Instance);

        await middleware.InvokeAsync(context);

        context.Response.Body.Position = 0;

        var body = await new StreamReader(context.Response.Body, Encoding.UTF8).ReadToEndAsync();

        return (context.Response.StatusCode, body, continued);
    }

    private static Claim Status(UserAccountStatus status) =>
        new(AuthConstants.AccountStatusClaimType, ((int)status).ToString());

    [Fact]
    public async Task An_anonymous_request_passes_through_untouched()
    {
        var result = await RunAsync(new ClaimsPrincipal(new ClaimsIdentity()));

        Assert.True(result.Continued);
    }

    [Fact]
    public async Task An_active_account_passes_through()
    {
        var result = await RunAsync(Principal(
            new Claim(ClaimTypes.NameIdentifier, "user-1"),
            Status(UserAccountStatus.Active)));

        Assert.True(result.Continued);
    }

    [Theory]
    [InlineData(UserAccountStatus.Suspended)]
    [InlineData(UserAccountStatus.Blocked)]
    [InlineData(UserAccountStatus.Deactivated)]
    public async Task A_live_token_stops_working_the_moment_the_account_leaves_the_active_state(
        UserAccountStatus status)
    {
        var result = await RunAsync(Principal(
            new Claim(ClaimTypes.NameIdentifier, "user-1"),
            Status(status)));

        Assert.False(result.Continued);
        Assert.Equal(StatusCodes.Status403Forbidden, result.StatusCode);

        var payload = JsonDocument.Parse(result.Body).RootElement;

        Assert.False(payload.GetProperty("success").GetBoolean());
        Assert.True(ArabicText.IsArabic(payload.GetProperty("message").GetString()!));
    }

    [Fact]
    public async Task A_token_for_an_account_that_no_longer_exists_is_refused()
    {
        var result = await RunAsync(Principal(
            new Claim(ClaimTypes.NameIdentifier, "ghost"),
            new Claim(AuthConstants.AccountStatusClaimType, AuthConstants.AccountMissingClaimValue)));

        Assert.False(result.Continued);
        Assert.Equal(StatusCodes.Status401Unauthorized, result.StatusCode);
    }

    [Fact]
    public async Task A_nonsense_state_claim_is_refused_rather_than_trusted()
    {
        var result = await RunAsync(Principal(
            new Claim(ClaimTypes.NameIdentifier, "user-1"),
            new Claim(AuthConstants.AccountStatusClaimType, "99")));

        Assert.False(result.Continued);
        Assert.Equal(StatusCodes.Status401Unauthorized, result.StatusCode);
    }

    private static AppDbContext InMemoryContext() =>
        new(new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(warnings =>
                warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.InMemoryEventId
                    .TransactionIgnoredWarning))
            .Options);

    private static async Task<ClaimsPrincipal> TransformAsync(
        AppDbContext context, ClaimsPrincipal principal, string path = "/api/profile")
    {
        var accessor = new HttpContextAccessor
        {
            HttpContext = new DefaultHttpContext { Request = { Path = path } }
        };

        var transformation = new DatabaseRoleClaimsTransformation(
            context, new UserAccessStateCache(), accessor);

        return await transformation.TransformAsync(principal);
    }

    private static async Task<ApplicationUser> SeedAsync(
        AppDbContext context, UserAccountStatus status)
    {
        var user = new ApplicationUser
        {
            Id = Guid.NewGuid().ToString(),
            FirstName = "اسم",
            SecondName = "تاني",
            UserName = "user",
            Email = "user@example.com",
            Governorate = LocationConstants.Governorate,
            Center = LocationConstants.Centers[0],
            Status = status
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return user;
    }

    [Fact]
    public async Task The_account_state_is_read_from_the_database_and_stamped_onto_the_principal()
    {
        using var context = InMemoryContext();

        var user = await SeedAsync(context, UserAccountStatus.Suspended);

        var principal = await TransformAsync(
            context, Principal(new Claim(ClaimTypes.NameIdentifier, user.Id)));

        Assert.Equal(
            ((int)UserAccountStatus.Suspended).ToString(),
            principal.FindFirstValue(AuthConstants.AccountStatusClaimType));
    }

    [Fact]
    public async Task A_state_claim_carried_inside_the_token_is_discarded_not_trusted()
    {
        using var context = InMemoryContext();

        var user = await SeedAsync(context, UserAccountStatus.Blocked);

        var principal = await TransformAsync(context, Principal(
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(AuthConstants.AccountStatusClaimType, ((int)UserAccountStatus.Active).ToString())));

        var stamped = principal
            .FindAll(AuthConstants.AccountStatusClaimType)
            .Select(claim => claim.Value)
            .ToList();

        Assert.Equal([((int)UserAccountStatus.Blocked).ToString()], stamped);
    }

    [Fact]
    public async Task A_token_whose_account_row_is_gone_is_marked_missing()
    {
        using var context = InMemoryContext();

        var principal = await TransformAsync(
            context, Principal(new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())));

        Assert.Equal(
            AuthConstants.AccountMissingClaimValue,
            principal.FindFirstValue(AuthConstants.AccountStatusClaimType));
    }

    [Fact]
    public void The_state_gate_runs_between_authentication_and_authorization()
    {
        var program = File.ReadAllText(
            Path.Combine(RepositoryRoot.Path, "MarkatPlace", "Program.cs"));

        var authentication = program.IndexOf("app.UseAuthentication();", StringComparison.Ordinal);
        var gate = program.IndexOf("app.UseMiddleware<AccountStatusMiddleware>();", StringComparison.Ordinal);
        var authorization = program.IndexOf("app.UseAuthorization();", StringComparison.Ordinal);

        Assert.True(authentication >= 0, "UseAuthentication is missing from the pipeline.");
        Assert.True(gate > authentication,
            "AccountStatusMiddleware must run after UseAuthentication or the principal has no state claim.");
        Assert.True(authorization > gate,
            "AccountStatusMiddleware must run before UseAuthorization.");
    }

    [Fact]
    public void Every_administrator_status_change_drops_the_cached_access_state()
    {
        foreach (var file in new[]
                 {
                     Path.Combine("Core", "Services", "Admin", "AdminUserService.cs"),
                     Path.Combine("Core", "Services", "Admin", "AdminAccountService.cs")
                 })
        {
            var source = File.ReadAllText(Path.Combine(RepositoryRoot.Path, file));

            Assert.Contains("_accessState.Invalidate(", source);
        }
    }

    [Fact]
    public void An_administrator_cannot_stamp_an_account_as_closed_by_its_owner()
    {
        Assert.False(UserAccountCatalog.IsAdminAssignable(UserAccountStatus.Deactivated));

        Assert.All(
            UserAccountCatalog.Options,
            option => Assert.NotEqual(UserAccountStatus.Deactivated, option.Status));
    }

    [Fact]
    public void Every_account_state_has_an_Arabic_name()
    {
        foreach (var status in Enum.GetValues<UserAccountStatus>())
        {
            var name = UserAccountCatalog.GetStatusName(status);

            Assert.True(ArabicText.IsArabic(name), $"{status} has no Arabic name: \"{name}\"");
        }
    }

    private static IModel BuildModel()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer("Server=localhost;Database=ModelOnly;Trusted_Connection=True;")
            .Options;

        using var context = new AppDbContext(options);

        return context.GetService<IDesignTimeModel>().Model;
    }

    [Fact]
    public void Closing_an_account_reaches_every_module_that_records_an_owner()
    {
        var model = BuildModel();

        var moderated = model
            .GetEntityTypes()
            .Where(entityType =>
                !entityType.IsOwned() &&
                entityType.ClrType is { IsClass: true, IsAbstract: false } &&
                typeof(IModeratedListing).IsAssignableFrom(entityType.ClrType))
            .Select(entityType => entityType.ClrType)
            .Distinct()
            .ToList();

        var reached = model
            .GetEntityTypes()
            .Where(entityType =>
                !entityType.IsOwned() &&
                entityType.ClrType is { IsClass: true, IsAbstract: false } &&
                typeof(IModeratedListing).IsAssignableFrom(entityType.ClrType) &&
                AccountClosureRepository.OwnerProperties.Any(
                    name => entityType.FindProperty(name) is not null))
            .Select(entityType => entityType.ClrType)
            .Distinct()
            .ToList();

        Assert.True(reached.Count >= 45,
            $"only {reached.Count} listing modules would be hidden when an account is closed.");

        var missed = moderated.Except(reached).Select(type => type.Name).ToList();

        Assert.True(missed.Count == 0,
            "These moderated modules have no owner column, so closing an account leaves them visible: " +
            string.Join(", ", missed));
    }
}
