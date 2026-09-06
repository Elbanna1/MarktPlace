using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Abstractions;
using Persistence.Data;
using Persistence.Data.Development;
using Xunit;

namespace MarkatPlace.Tests;

public class SeedDataTests
{
    private static readonly string[] OwnershipProperties =
    [
        "UserId", "OwnerId", "CreatedByUserId", "AuthorId",
        "ReferrerUserId", "ReferredUserId", "ApplicationUserId"
    ];

    private static readonly string[] ReferenceDataEntities =
    [
        nameof(Category), nameof(SubCategory), nameof(Governorate), nameof(Center),
        nameof(Feature), nameof(HomeSection), nameof(PlatformSetting),
        nameof(PaymentMethod), nameof(BannerPlacementSetting)
    ];

    private static IModel BuildModel()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer("Server=localhost;Database=ModelOnly;Trusted_Connection=True;")
            .Options;

        using var context = new AppDbContext(options);

        return context.GetService<IDesignTimeModel>().Model;
    }

    [Fact]
    public void No_entity_that_belongs_to_a_user_carries_HasData_rows()
    {
        var offenders = BuildModel().GetEntityTypes()
            .Where(entity => entity.GetSeedData().Any())
            .Where(entity =>
                entity.ClrType == typeof(ApplicationUser) ||
                entity.ClrType.Name.StartsWith("Identity", StringComparison.Ordinal) ||
                entity.GetProperties().Any(property =>
                    OwnershipProperties.Contains(property.Name, StringComparer.Ordinal)))
            .Select(entity => entity.ClrType.Name)
            .ToList();

        Assert.True(offenders.Count == 0,
            "These entity types are seeded with HasData even though their rows belong to a user, " +
            "which means a fresh Production database would come up with demo records: " +
            string.Join(", ", offenders));
    }

    [Theory]
    [InlineData("Advertisement")]
    [InlineData("Notification")]
    [InlineData("Referral")]
    [InlineData("ReferralLinkEvent")]
    [InlineData("LostFoundPost")]
    [InlineData("LostFoundComment")]
    [InlineData("Workshop")]
    [InlineData("Craftsman")]
    [InlineData("Payment")]
    [InlineData("BannerBooking")]
    [InlineData("AdvertisementView")]
    public void No_sample_row_is_seeded_for(string entityName)
    {
        var entity = BuildModel().GetEntityTypes()
            .SingleOrDefault(candidate => candidate.ClrType.Name == entityName);

        Assert.True(entity is not null, $"{entityName} is no longer a mapped entity; update this test.");

        Assert.Empty(entity!.GetSeedData());
    }

    [Fact]
    public void The_reference_data_the_application_needs_is_still_seeded()
    {
        var model = BuildModel();

        foreach (var name in ReferenceDataEntities)
        {
            var entity = model.GetEntityTypes()
                .SingleOrDefault(candidate => candidate.ClrType.Name == name);

            Assert.True(entity is not null, $"{name} is no longer a mapped entity; update this test.");

            Assert.True(entity!.GetSeedData().Any(),
                $"{name} carries no seed rows. It is reference data the application needs to " +
                "function (categories, locations, lookups, settings) and must not be removed " +
                "along with the demo data.");
        }
    }

    [Fact]
    public async Task The_demo_seeder_refuses_to_run_in_production()
    {
        var report = await DevelopmentDataSeeder.SeedAsync(
            BuildServices("Production"),
            new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?> { ["DemoData:Enabled"] = "true" })
                .Build(),
            NullLogger.Instance);

        Assert.True(report.Skipped);
        Assert.Contains("Development", report.SkipReason);
        Assert.Equal(0, report.UsersCreated);
        Assert.Equal(0, report.AdvertisementsCreated);
        Assert.Equal(0, report.WorkshopsCreated);
        Assert.Equal(0, report.CraftsmenCreated);
        Assert.Equal(0, report.PostsCreated);
    }

    [Fact]
    public async Task The_demo_seeder_refuses_to_run_in_staging()
    {
        var report = await DevelopmentDataSeeder.SeedAsync(
            BuildServices("Staging"), new ConfigurationBuilder().Build(), NullLogger.Instance);

        Assert.True(report.Skipped);
        Assert.Contains("Staging", report.SkipReason);
    }

    [Fact]
    public async Task Demo_data_is_off_unless_a_configuration_file_asks_for_it()
    {
        var report = await DevelopmentDataSeeder.SeedAsync(
            BuildServices("Development"), new ConfigurationBuilder().Build(), NullLogger.Instance);

        Assert.True(report.Skipped);
        Assert.Equal("DemoData:Enabled is false.", report.SkipReason);
    }

    [Fact]
    public void Demo_data_is_disabled_in_the_committed_and_production_configuration()
    {
        foreach (var file in new[] { "MarkatPlace/appsettings.json", "MarkatPlace/appsettings.Production.json" })
        {
            var settings = ReadJson(file);

            Assert.False(settings.GetValue<bool>("DemoData:Enabled"),
                $"{file} enables the demo data seeder.");
        }
    }

    [Fact]
    public void No_administrator_credentials_are_committed_outside_development()
    {
        foreach (var file in new[] { "MarkatPlace/appsettings.json", "MarkatPlace/appsettings.Production.json" })
        {
            var settings = ReadJson(file);

            Assert.Null(settings["AdminUser:UserName"]);
            Assert.Null(settings["AdminUser:Password"]);
        }
    }

    [Fact]
    public void Startup_only_seeds_demo_data_inside_a_development_check()
    {
        var program = RepositoryRoot.ReadFile("MarkatPlace/Program.cs");

        var guard = program.IndexOf("app.Environment.IsDevelopment()", StringComparison.Ordinal);
        var call = program.IndexOf("DevelopmentDataSeeder.SeedAsync", StringComparison.Ordinal);

        Assert.True(call > 0, "Program.cs no longer calls the demo seeder; update this test.");
        Assert.True(guard > 0 && guard < call,
            "DevelopmentDataSeeder.SeedAsync is called before any IsDevelopment() guard in Program.cs.");
    }

    [Fact]
    public void The_identity_seeder_carries_no_fallback_credentials()
    {
        var seeder = RepositoryRoot.ReadFile("Infastrucre/Presitance/Data/IdentityDataSeeder.cs");

        Assert.DoesNotContain("Admin@12345", seeder);
        Assert.Contains("skipping admin account seeding", seeder);
    }

    private static IConfigurationRoot ReadJson(string relativePath) =>
        new ConfigurationBuilder()
            .SetBasePath(RepositoryRoot.Path)
            .AddJsonFile(relativePath, optional: false)
            .Build();

    private static IServiceProvider BuildServices(string environmentName) =>
        new ServiceCollection()
            .AddSingleton<IHostEnvironment>(new StubEnvironment(environmentName))
            .BuildServiceProvider();

    private sealed class StubEnvironment : IHostEnvironment
    {
        public StubEnvironment(string environmentName) => EnvironmentName = environmentName;

        public string EnvironmentName { get; set; }
        public string ApplicationName { get; set; } = "MarkatPlace.Tests";
        public string ContentRootPath { get; set; } = AppContext.BaseDirectory;
        public IFileProvider ContentRootFileProvider { get; set; } = new NullFileProvider();
    }
}
