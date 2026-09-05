using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Persistence.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var connectionString =
            Environment.GetEnvironmentVariable("MARKATPLACE_CONNECTION")
            ?? "Server=.;Database=MarkatPlaceDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true";

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer(connectionString, sql =>
            sql.MigrationsAssembly(typeof(AppDbContextFactory).Assembly.FullName));

        return new AppDbContext(optionsBuilder.Options);
    }
}
