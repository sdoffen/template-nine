using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
#if (provider == "mssql")
using Microsoft.Data.SqlClient;
#elif (provider == "mysql")
using MySql.Data.MySqlClient;
#elif (provider == "postgres")
using Npgsql;
#endif

namespace Template9;

public static class CompositionExtensions
{
    public static IServiceCollection RegisterDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        // Call extension methods to register each interface implementation.

        var databaseOptions = configuration.GetSection(DatabaseOptions.SectionName).Get<DatabaseOptions>()
            ?? throw new InvalidOperationException($"Missing {DatabaseOptions.SectionName} section in configuration.");

        var optionsAction = GetDbContextOptionsAction(databaseOptions);

        services.AddDbContextFactory<ProjectContext>(optionsAction);

        if (databaseOptions.UseDbContextPooling)
        {
            services.AddDbContextPool<ProjectContext>(optionsAction);
        }
        else
        {
            services.AddDbContext<ProjectContext>(optionsAction);
        }

        return services;
    }

    private static Action<DbContextOptionsBuilder> GetDbContextOptionsAction(DatabaseOptions options)
    {
        return builder =>
        {
            if (options.AsNoTracking)
            {
                builder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            }

#if (provider == "mssql")
            builder.UseSqlServer(GetConnectionString(options), contextOptions =>
#elif (provider == "mysql")
            builder.UseMySQL(GetConnectionString(options), contextOptions =>
#elif (provider == "postgres")
            builder.UseNpgsql(GetConnectionString(options), contextOptions =>
#endif
            {
                if (options.UseMigrationsAssembly && !string.IsNullOrEmpty(options.MigrationsAssembly))
                {
                    contextOptions.MigrationsAssembly(options.MigrationsAssembly);
                }

                if (options.EnableRetryOnFailure)
                {
#if (provider != "postgres")
                    // Convert error codes from string to int
                    var errorCodes = options.ErrorCodesToAdd?.Select(code => int.Parse(code)).ToList() ?? null;

#endif
                    contextOptions.EnableRetryOnFailure(
                        options.MaxRetryCount,
                        TimeSpan.FromSeconds(options.MaxRetryDelaySeconds),
#if (provider == "postgres")
                        options.ErrorCodesToAdd
#elif (provider != "postgres")
                        errorCodes
#endif
                    );
                }
            });
        };
    }

    private static string GetConnectionString(DatabaseOptions options)
    {
#if (provider == "mssql")
        var builder = new SqlConnectionStringBuilder
        {
            DataSource = (options.Port > 0)
                ? $"{options.Host},{options.Port}"
                : options.Host,
            InitialCatalog = options.DatabaseName,
            UserID = options.Username,
        };
#elif (provider == "mysql")
        var builder = new MySqlConnectionStringBuilder
        {
            Server = options.Host,
            Port = (uint)options.Port,
            Database = options.DatabaseName,
            UserID = options.Username,
        };
#elif (provider == "postgres")
        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = options.Host,
            Port = options.Port,
            Database = options.DatabaseName,
            Username = options.Username,
        };
#endif

        if (!string.IsNullOrEmpty(options.Password))
        {
            builder.Password = options.Password;
        }
        else
        {
            // Handle the case where the password is not provided.
            throw new InvalidOperationException("Password is required for the connection string.");
        }

        return builder.ConnectionString;
    }
}