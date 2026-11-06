using System.Data.Common;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence.Data;
using ServicesAbstraction;
using Shared.Constants;
using Shared.Enums;
using Xunit;

namespace MarkatPlace.Tests;

public static class LocalDb
{
    public const string Server = @"(localdb)\MSSQLLocalDB";

    public static readonly bool IsAvailable = Probe();

    public static string ConnectionString(string database) =>
        $"Server={Server};Database={database};Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True";

    private static bool Probe()
    {
        if (!OperatingSystem.IsWindows())
            return false;

        try
        {
            using var connection = new SqlConnection(ConnectionString("master"));
            connection.Open();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}

public sealed class LocalDbFactAttribute : FactAttribute
{
    public LocalDbFactAttribute()
    {
        if (!LocalDb.IsAvailable)
            Skip = "SQL Server LocalDB is not available on this machine.";
    }
}

public sealed class LocalDbTheoryAttribute : TheoryAttribute
{
    public LocalDbTheoryAttribute()
    {
        if (!LocalDb.IsAvailable)
            Skip = "SQL Server LocalDB is not available on this machine.";
    }
}

[CollectionDefinition(Name)]
public class LifecycleApiCollection : ICollectionFixture<LifecycleApiFixture>
{
    public const string Name = "lifecycle-api";
}

public sealed class LifecycleApiFixture : IAsyncLifetime
{
    private const string OwnerPassword = "Lifecycle@12345";

    private static readonly IReadOnlyDictionary<string, string> Environment = new Dictionary<string, string>
    {
        ["ASPNETCORE_ENVIRONMENT"] = "Development",
        ["DemoData__Enabled"] = "false",
        ["RateLimiting__Enabled"] = "false"
    };

    private readonly string _database = "MarkatPlaceLifecycle_" + Guid.NewGuid().ToString("N");

    private WebApplicationFactory<Program>? _factory;

    public HttpClient Anonymous { get; private set; } = default!;

    public HttpClient Admin { get; private set; } = default!;

    public string AdminId { get; private set; } = default!;

    public async Task InitializeAsync()
    {
        if (!LocalDb.IsAvailable)
            return;

        var connectionString = LocalDb.ConnectionString(_database);

        await CreateDatabaseInConfiguredDirectoryAsync();

        var overrides = new Dictionary<string, string>(Environment)
        {
            ["ConnectionStrings__DefaultConnection"] = connectionString
        };

        var previous = overrides.Keys.ToDictionary(
            key => key, key => System.Environment.GetEnvironmentVariable(key));

        try
        {
            foreach (var (key, value) in overrides)
                System.Environment.SetEnvironmentVariable(key, value);

            RefuseAnythingButLocalDb();

            _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Development");

                builder.ConfigureTestServices(services =>
                {
                    var jobs = services
                        .Where(service => service.ServiceType == typeof(IHostedService))
                        .ToList();

                    foreach (var job in jobs)
                        services.Remove(job);
                });
            });

            Anonymous = _factory.CreateClient();
        }
        finally
        {
            foreach (var (key, value) in previous)
                System.Environment.SetEnvironmentVariable(key, value);
        }

        using var scope = _factory.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var target = context.Database.GetConnectionString() ?? string.Empty;

        if (!target.Contains(_database, StringComparison.Ordinal))
            throw new InvalidOperationException("The test host is not connected to its isolated database.");

        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var userName = configuration["AdminUser:UserName"]!;
        var password = configuration["AdminUser:Password"]!;

        Admin = await LoginAsync(userName, password);

        var users = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        AdminId = (await users.FindByNameAsync(userName))!.Id;
    }

    public async Task DisposeAsync()
    {
        if (_factory is null)
            return;

        await _factory.DisposeAsync();

        SqlConnection.ClearAllPools();

        await using var connection = new SqlConnection(LocalDb.ConnectionString("master"));
        await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        command.CommandText =
            $"ALTER DATABASE [{_database}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE [{_database}];";

        await command.ExecuteNonQueryAsync();
    }

    private async Task CreateDatabaseInConfiguredDirectoryAsync()
    {
        var directory = System.Environment.GetEnvironmentVariable("MARKATPLACE_TEST_DB_DIR");

        if (string.IsNullOrWhiteSpace(directory))
            return;

        Directory.CreateDirectory(directory);

        var data = Path.Combine(directory, _database + ".mdf").Replace("'", "''");
        var log = Path.Combine(directory, _database + "_log.ldf").Replace("'", "''");

        await using var connection = new SqlConnection(LocalDb.ConnectionString("master"));
        await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        command.CommandText =
            $"CREATE DATABASE [{_database}] ON (NAME = N'{_database}', FILENAME = N'{data}') " +
            $"LOG ON (NAME = N'{_database}_log', FILENAME = N'{log}');";

        await command.ExecuteNonQueryAsync();
    }

    public IServiceScope CreateScope() => _factory!.Services.CreateScope();

    public HubConnection ConnectNotifications(HttpClient client)
    {
        var token = client.DefaultRequestHeaders.Authorization?.Parameter;
        var server = _factory!.Server;

        return new HubConnectionBuilder()
            .WithUrl(new Uri(server.BaseAddress, "/hubs/notifications"), options =>
            {
                options.HttpMessageHandlerFactory = _ => server.CreateHandler();
                options.Transports = HttpTransportType.LongPolling;
                options.AccessTokenProvider = () => Task.FromResult(token);
            })
            .Build();
    }

    public async Task RunJobAsync(DateTime utcNow)
    {
        using var scope = CreateScope();

        await scope.ServiceProvider
            .GetRequiredService<IListingLifecycleProcessor>()
            .ProcessAsync(utcNow);
    }

    public async Task<(string Id, HttpClient Client)> CreateOwnerAsync()
    {
        using var scope = CreateScope();

        var users = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var unique = Guid.NewGuid().ToString("N")[..12];

        var user = new ApplicationUser
        {
            UserName = $"owner{unique}",
            Email = $"owner{unique}@lifecycle.test",
            PhoneNumber = "010" + Random.Shared.Next(10000000, 99999999),
            EmailConfirmed = true,
            PhoneNumberConfirmed = true
        };

        var model = scope.ServiceProvider.GetRequiredService<AppDbContext>().Model;

        foreach (var property in model.FindEntityType(typeof(ApplicationUser))!.GetProperties())
        {
            if (!property.IsNullable && property.ClrType == typeof(string) &&
                property.PropertyInfo?.GetValue(user) is null && !property.IsPrimaryKey())
            {
                property.PropertyInfo?.SetValue(user, "الفيوم");
            }
        }

        var created = await users.CreateAsync(user, OwnerPassword);

        Assert.True(created.Succeeded, string.Join(" | ", created.Errors.Select(e => e.Description)));

        return (user.Id, await LoginAsync(user.UserName, OwnerPassword));
    }

    public async Task<Guid> SeedAsync(
        ListingModuleType module, string ownerId, string title, int? subCategoryId = null)
    {
        using var scope = CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        return await ListingFactory.AddAsync(context, module, ownerId, title, subCategoryId);
    }

    public async Task SetWindowAsync(ListingModuleType module, Guid id, DateTime publishedAt, DateTime expireAt)
    {
        using var scope = CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var table = ListingFactory.TableOf(context, module);

        await context.Database.ExecuteSqlRawAsync(
            $"UPDATE [{table}] SET [PublishedAt] = {{0}}, [ExpireAt] = {{1}} WHERE [Id] = {{2}}",
            publishedAt, expireAt, id);
    }

    public async Task<ListingRow> ReadAsync(ListingModuleType module, Guid id)
    {
        using var scope = CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var table = ListingFactory.TableOf(context, module);

        await context.Database.OpenConnectionAsync();

        await using var command = context.Database.GetDbConnection().CreateCommand();
        command.CommandText =
            $"SELECT [PublishedAt], [ExpireAt], [FirstPublishedAt], [RepublishCount], [ModerationStatus], [CreatedAt] " +
            $"FROM [{table}] WHERE [Id] = @id";

        var parameter = command.CreateParameter();
        parameter.ParameterName = "@id";
        parameter.Value = id;
        command.Parameters.Add(parameter);

        await using var reader = await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
            return new ListingRow(0, null, null, null, 0, ModerationStatus.Pending, default);

        var row = new ListingRow(
            1,
            reader.IsDBNull(0) ? null : reader.GetDateTime(0),
            reader.IsDBNull(1) ? null : reader.GetDateTime(1),
            reader.IsDBNull(2) ? null : reader.GetDateTime(2),
            reader.GetInt32(3),
            (ModerationStatus)Convert.ToInt32(reader.GetValue(4)),
            reader.GetDateTime(5));

        await reader.CloseAsync();

        command.CommandText = $"SELECT COUNT(*) FROM [{table}] WHERE [Id] = @id";

        return row with { Count = Convert.ToInt32(await command.ExecuteScalarAsync()) };
    }

    public async Task<int> CountAdminAlertsAsync(Guid id)
    {
        using var scope = CreateScope();

        return await scope.ServiceProvider.GetRequiredService<AppDbContext>().Notifications
            .CountAsync(n => n.ReferenceId == id &&
                             n.UserId == AdminId &&
                             n.Type == NotificationType.AdminListingExpiring);
    }

    private async Task<HttpClient> LoginAsync(string userName, string password)
    {
        var response = await Anonymous.PostAsJsonAsync(
            "/api/auth/login", new { username = userName, password });

        var body = await response.Content.ReadAsStringAsync();

        Assert.True(response.IsSuccessStatusCode, $"Login failed: {(int)response.StatusCode} {body}");

        using var json = JsonDocument.Parse(body);

        var token = json.RootElement.GetProperty("data").GetProperty("token").GetString();

        var client = _factory!.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return client;
    }

    private static void RefuseAnythingButLocalDb()
    {
        var effective = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .Build()
            .GetConnectionString("DefaultConnection");

        if (effective is null || !effective.Contains(LocalDb.Server, StringComparison.Ordinal))
            throw new InvalidOperationException(
                "Refusing to boot the API: the effective connection string is not LocalDB.");
    }
}

public sealed record ListingRow(
    int Count,
    DateTime? PublishedAt,
    DateTime? ExpireAt,
    DateTime? FirstPublishedAt,
    int RepublishCount,
    ModerationStatus ModerationStatus,
    DateTime CreatedAt);

public static class ListingFactory
{
    public static string TableOf(AppDbContext context, ListingModuleType module) =>
        EntityOf(context, module).GetTableName()!;

    public static async Task<Guid> AddAsync(
        AppDbContext context, ListingModuleType module, string ownerId, string title, int? subCategoryId)
    {
        var entityType = EntityOf(context, module);
        var entity = Activator.CreateInstance(entityType.ClrType)!;
        var entry = context.Entry(entity);
        var id = Guid.NewGuid();

        foreach (var property in entityType.GetProperties())
        {
            if (property.IsPrimaryKey())
            {
                entry.Property(property.Name).CurrentValue = id;
                continue;
            }

            if (property.IsForeignKey() || property.IsNullable || property.IsShadowProperty())
                continue;

            if (property.Name is "ModerationStatus" or "CreatedAt" or "RepublishCount" or "Views" or "ViewCount" or "LikesCount" or "CommentsCount")
                continue;

            if (Sample(property) is { } value)
                entry.Property(property.Name).CurrentValue = value;
        }

        foreach (var foreignKey in entityType.GetForeignKeys())
        {
            var property = foreignKey.Properties.Single();

            if (foreignKey.PrincipalEntityType.ClrType == typeof(ApplicationUser))
            {
                if (property.Name is "UserId" or "OwnerId")
                    entry.Property(property.Name).CurrentValue = ownerId;

                continue;
            }

            if (!foreignKey.IsRequired)
                continue;

            entry.Property(property.Name).CurrentValue = property.Name switch
            {
                "CategoryId" => CategoryOf(module),
                "SubCategoryId" => SubCategoryOf(module, subCategoryId),
                _ => await FirstKeyOfAsync(context, foreignKey.PrincipalEntityType)
                     ?? throw new InvalidOperationException(
                         $"{foreignKey.PrincipalEntityType.ClrType.Name} has no seeded row.")
            };
        }

        foreach (var name in new[] { "Title", "Name", "ItemName" })
        {
            if (entityType.FindProperty(name) is { ClrType: var type } && type == typeof(string))
                entry.Property(name).CurrentValue = title;
        }

        entry.Property(nameof(IModeratedListing.ModerationStatus)).CurrentValue = ModerationStatus.Pending;

        if (entity is LostFoundPost post)
            post.PostType = module == ListingModuleType.FoundItem ? PostType.Found : PostType.Lost;

        if (entity is Advertisement advertisement)
            advertisement.Status = AdvertisementStatus.Active;

        context.Add(entity);

        await context.SaveChangesAsync();

        return id;
    }

    private static IEntityType EntityOf(AppDbContext context, ListingModuleType module) =>
        context.Model.FindEntityType(Persistence.Listings.ModerationModuleRegistry.EntityTypes[module])!;

    private static int CategoryOf(ListingModuleType module) =>
        (int?)ListingModuleCatalog.CategoryOf(module) ?? (int)CategoryType.Cars;

    private static int SubCategoryOf(ListingModuleType module, int? requested) =>
        requested ?? (int?)ListingModuleCatalog.SubCategoryOf(module) ?? (int)SubCategoryType.Private;

    private static object? Sample(IProperty property)
    {
        var type = Nullable.GetUnderlyingType(property.ClrType) ?? property.ClrType;

        if (type == typeof(string))
            return property.GetMaxLength() is { } length and < 11 ? "01012"[..Math.Min(5, length)] : "01012345678";

        if (type.IsEnum)
            return Enum.GetValues(type).GetValue(0);

        if (type == typeof(DateTime))
            return DateTime.UtcNow;

        if (type == typeof(decimal))
            return 100m;

        if (type == typeof(int))
            return 1;

        return null;
    }

    private static async Task<object?> FirstKeyOfAsync(AppDbContext context, IEntityType principal)
    {
        var key = principal.FindPrimaryKey()!.Properties.Single().GetColumnName();

        await context.Database.OpenConnectionAsync();

        await using DbCommand command = context.Database.GetDbConnection().CreateCommand();
        command.CommandText = $"SELECT TOP(1) [{key}] FROM [{principal.GetTableName()}] ORDER BY [{key}]";

        return await command.ExecuteScalarAsync();
    }
}
