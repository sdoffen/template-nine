using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Template9.Design;

public class ProjectDbContextFactory : IDesignTimeDbContextFactory<ProjectDbContext>
{
    public ProjectDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationManager();
        // Add configuration sources as needed, e.g. AWS Secrets or Azure Key Vault.
        // Order is important! The last configuration source will override any previous ones.
        configuration.AddJsonFile("appsettings.Design.json");

        var services = new ServiceCollection()
            .RegisterDbContext(configuration);

        var provider = services.BuildServiceProvider();
        return provider.GetRequiredService<ProjectDbContext>();
    }
}
