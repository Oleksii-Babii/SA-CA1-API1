using BonsaiAPI.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BonsaiAPI.Tests;

public class BonsaiWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _dbName = $"TestDb_{Guid.NewGuid()}";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.UseSetting("TestDbName", _dbName);

        builder.ConfigureServices(services =>
        {
            // Remove the SQL Server BonsaiContext registration from Program.cs and
            // replace it with an InMemory provider so tests can run on any platform
            // (LocalDB is Windows-only and not available on Linux CI runners).
            var descriptorsToRemove = services
                .Where(d => d.ServiceType == typeof(DbContextOptions<BonsaiContext>)
                         || d.ServiceType == typeof(DbContextOptions)
                         || d.ServiceType == typeof(BonsaiContext))
                .ToList();

            foreach (var d in descriptorsToRemove)
            {
                services.Remove(d);
            }

            services.AddDbContext<BonsaiContext>(options =>
            {
                options.UseInMemoryDatabase(_dbName);
            });
        });
    }
}
