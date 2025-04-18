using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Template9;

public static class CompositionExtensions
{
    public static IServiceCollection RegisterDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var databaseOptions = configuration.GetSection(DatabaseOptions.SectionName).Get<DatabaseOptions>()
            ?? throw new InvalidOperationException($"Missing {DatabaseOptions.SectionName} section in configuration.");

        // var optionsAction = GetDbContextOptionsAction(databaseOptions);

        if (databaseOptions.UseDbContextPooling)
        {
            // services.AddDbContextPool<ProjectContext>(optionsAction);
        }
        else
        {
            // services.AddDbContext<ProjectContext>(optionsAction);
        }

        return services;
    }

    // private static Action<DbContextOptionsBuilder> GetDbContextOptionsAction(DatabaseOptions options)
    // {
    //     throw new NotImplementedException();
    // }

    // private static string GetConnectionString(DatabaseOptions options)
    // {
    //     throw new NotImplementedException();
    // }
}