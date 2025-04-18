using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Template9.Design;

public class ProjectContextFactory : IDesignTimeDbContextFactory<ProjectContext>
{
    public ProjectContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationManager();
        configuration.AddJsonFile("appsettings.Design.json");

        var services = new ServiceCollection()
            .RegisterDbContext(configuration);

        var provider = services.BuildServiceProvider();
        return provider.GetRequiredService<ProjectContext>();
    }
}
