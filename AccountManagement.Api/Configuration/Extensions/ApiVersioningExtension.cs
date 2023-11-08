using Asp.Versioning;

namespace AccountManagement.Configuration.Extensions;

public static class ApiVersioningExtension
{

    public static IServiceCollection AddApiVersioningConfigured(this IServiceCollection services)
    {

        var apiVersioningBuilder = services.AddApiVersioning(
            options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new QueryStringApiVersionReader("api-version"),
                    new HeaderApiVersionReader("X-Version"),
                    new MediaTypeApiVersionReader("x-api-version"));

            });

        apiVersioningBuilder.AddApiExplorer(
            options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

        return services;
    }
}