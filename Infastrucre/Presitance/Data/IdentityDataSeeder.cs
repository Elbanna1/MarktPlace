using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shared.Constants;

namespace Persistence.Data;

public static class IdentityDataSeeder
{
    private const string AdminUserSection = "AdminUser";

    public static async Task SeedAsync(IServiceProvider services, IConfiguration configuration)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var environment = services.GetRequiredService<IHostEnvironment>();
        var logger = services.GetRequiredService<ILoggerFactory>()
            .CreateLogger("MarkatPlace.IdentitySeed");

        foreach (var role in AppRoles.All)
        {
            if (await roleManager.RoleExistsAsync(role))
                continue;

            var roleResult = await roleManager.CreateAsync(new IdentityRole(role));

            if (!roleResult.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Failed to create the {role} role: {Describe(roleResult)}");
            }

            logger.LogInformation("Created the {Role} role.", role);
        }

        var section = configuration.GetSection(AdminUserSection);
        var userName = section["UserName"];
        var email = section["Email"];
        var password = section["Password"];
        var phone = section["Phone"];

        ApplicationUser? admin = null;

        if (string.IsNullOrWhiteSpace(userName) ||
            string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password))
        {
            logger.LogInformation(
                "No bootstrap administrator configured for the {Environment} environment " +
                "({Section}:UserName / :Email / :Password); skipping admin account seeding.",
                environment.EnvironmentName, AdminUserSection);
        }
        else
        {
            admin = await userManager.FindByNameAsync(userName);

            if (admin is null)
            {
                admin = new ApplicationUser
                {
                    UserName = userName,
                    Email = email,
                    EmailConfirmed = true,
                    FirstName = "System",
                    SecondName = "Admin",
                    Governorate = LocationConstants.Governorate,
                    Center = LocationConstants.Centers[0],
                    PhoneNumber = string.IsNullOrWhiteSpace(phone) ? "01000000000" : phone,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(admin, password);

                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"Failed to seed the bootstrap administrator: {Describe(result)}");
                }

                logger.LogInformation("Created the bootstrap administrator account.");
            }

            await EnsureRoleAsync(userManager, admin, AppRoles.Admin, logger);
            await EnsureRoleAsync(userManager, admin, AppRoles.SuperAdmin, logger);
        }

        if (!await AnySuperAdminExistsAsync(userManager))
        {
            var existingAdmins = await userManager.GetUsersInRoleAsync(AppRoles.Admin);

            var founder = existingAdmins
                .OrderBy(user => user.CreatedAt)
                .ThenBy(user => user.Id, StringComparer.Ordinal)
                .FirstOrDefault();

            if (founder is not null)
            {
                await EnsureRoleAsync(userManager, founder, AppRoles.SuperAdmin, logger);

                logger.LogWarning(
                    "No {SuperAdmin} existed; promoted the oldest administrator ({UserName}) so the " +
                    "dashboard remains reachable and permissions can be granted. This runs once.",
                    AppRoles.SuperAdmin, founder.UserName);
            }
            else
            {
                logger.LogInformation(
                    "No administrators exist yet, so no {SuperAdmin} was promoted. Configure the " +
                    "{Section} section and restart to provision one.",
                    AppRoles.SuperAdmin, AdminUserSection);
            }
        }
    }

    private static async Task<bool> AnySuperAdminExistsAsync(UserManager<ApplicationUser> userManager) =>
        (await userManager.GetUsersInRoleAsync(AppRoles.SuperAdmin)).Count > 0;

    private static async Task EnsureRoleAsync(
        UserManager<ApplicationUser> userManager, ApplicationUser user, string role, ILogger logger)
    {
        if (await userManager.IsInRoleAsync(user, role))
            return;

        var assignment = await userManager.AddToRoleAsync(user, role);

        if (!assignment.Succeeded)
        {
            throw new InvalidOperationException(
                $"Failed to grant the {role} role to {user.UserName}: {Describe(assignment)}");
        }

        logger.LogInformation("Granted the {Role} role to {UserName}.", role, user.UserName);
    }

    private static string Describe(IdentityResult result) =>
        string.Join("; ", result.Errors.Select(error => error.Description));
}
