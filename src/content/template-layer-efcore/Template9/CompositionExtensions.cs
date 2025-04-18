using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
// using Npgsql;
// using MySql.Data.MySqlClient;

namespace Template9;

public static class CompositionExtensions
{
    public static IServiceCollection RegisterDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        // Call extension methods to register each interface implementation.

        var databaseOptions = configuration.GetSection(DatabaseOptions.SectionName).Get<DatabaseOptions>()
            ?? throw new InvalidOperationException($"Missing {DatabaseOptions.SectionName} section in configuration.");

        var optionsAction = GetDbContextOptionsAction(databaseOptions);

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

            // builder.UseNpgsql(GetConnectionString(options), contextOptions =>
            // builder.UseMySQL(GetConnectionString(options), contextOptions =>
            builder.UseSqlServer(GetConnectionString(options), contextOptions =>
            {
                if (options.UseMigrationsAssembly && !string.IsNullOrEmpty(options.MigrationsAssembly))
                {
                    contextOptions.MigrationsAssembly(options.MigrationsAssembly);
                }

                if (options.EnableRetryOnFailure)
                {
                    // var errorCodes = options.ErrorCodesToAdd;
                    var errorCodes = options.ErrorCodesToAdd?.Select(code => int.Parse(code)).ToList() ?? null;

                    contextOptions.EnableRetryOnFailure(
                        options.MaxRetryCount,
                        TimeSpan.FromSeconds(options.MaxRetryDelaySeconds),
                        errorCodes
                    );
                }
            });
        };
    }

    private static string GetConnectionString(DatabaseOptions options)
    {
        // var builder = new NpgsqlConnectionStringBuilder
        // {
        //     Host = options.Host,
        //     Port = options.Port,
        //     Database = options.DatabaseName,
        //     Username = options.Username,
        // };

        // var builder = new MySqlConnectionStringBuilder
        // {
        //     Server = options.Host,
        //     Port = (uint)options.Port,
        //     Database = options.DatabaseName,
        //     UserID = options.Username,
        // };

        var builder = new SqlConnectionStringBuilder
        {
            DataSource = (options.Port > 0)
                ? $"{options.Host},{options.Port}"
                : options.Host,
            InitialCatalog = options.DatabaseName,
            UserID = options.Username,
        };

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