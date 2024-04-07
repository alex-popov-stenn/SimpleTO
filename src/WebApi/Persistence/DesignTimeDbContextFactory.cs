using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Design;

namespace WebApi.Persistence;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
public sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        var services = new ServiceCollection();
        services.AddPersistence<ApplicationDbContext>(configuration.GetConnectionString("DbConnectionString")!);
        var provider = services.BuildServiceProvider();
        return provider.GetRequiredService<ApplicationDbContext>();
    }
}