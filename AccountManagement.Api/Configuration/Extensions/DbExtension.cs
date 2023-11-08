using AccountManagement.DAL;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace AccountManagement.Configuration.Extensions;

public static class DbExtension
{
    public static IServiceCollection ConfigureDbContext(this IServiceCollection serviceCollection,
                                                         IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        serviceCollection.AddDbContext<AppDbContext>(
            k =>
            {
                k.UseSqlite($"Data Source={connectionString}");
                k.EnableSensitiveDataLogging();

            });


        return serviceCollection;
    }

    public static IServiceCollection ConfigureDataProtection(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddDataProtection()
               .PersistKeysToDbContext<AppDbContext>();



        return serviceCollection;
    }
}