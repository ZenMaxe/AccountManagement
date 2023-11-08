using Serilog;

namespace AccountManagement.Configuration.Extensions;

public static class SerilongExtension
{
    public static IServiceCollection ConfigureSerilog(this IServiceCollection serviceCollection,
                                                      IConfiguration configuration)
    {
        var logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(configuration:configuration)
                    .Enrich.FromLogContext()
                    .CreateLogger();

        serviceCollection.AddSerilog(logger);

        return serviceCollection;
    }
}