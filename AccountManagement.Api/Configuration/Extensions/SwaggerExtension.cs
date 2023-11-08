using Asp.Versioning.ApiExplorer;
using Microsoft.OpenApi.Models;

namespace AccountManagement.Configuration.Extensions;

public  static class SwaggerExtension
{
    public static IServiceCollection ConfigureSwagger(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSwaggerGen(
            c =>
            {
                c.AddSecurityDefinition(
                    "Bearer",
                    new OpenApiSecurityScheme
                    {
                        Description = @"JWT Authorization Header Using Bearer Scheme.
                                        Enter 'Bearer' [space] and you're Token
                                        Example: 'Bearer JwtToken'",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer"
                    });

                c.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer",
                                },
                                Scheme = "0auth2",
                                Name = "Bearer",
                                In = ParameterLocation.Header
                            },
                            new List<string>()
                        }
                    });
                var provider = serviceCollection.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

                foreach (var description in provider.ApiVersionDescriptions)
                {
                    c.SwaggerDoc(
                        description.GroupName,
                        new OpenApiInfo
                        {
                            Title = $"AccountManagement Project {description.ApiVersion}",
                            Version = description.ApiVersion.ToString(),
                            Description = "This is the API documentation for different API versions."
                        });
                }
            });

        return serviceCollection;
    }

    public static IApplicationBuilder ConfigureSwaggerUi(this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
    {
        app.UseSwagger();
        app.UseSwaggerUI(
            options =>
            {
                foreach (var description in provider.ApiVersionDescriptions.Select(x => x.GroupName))
                {
                    options.SwaggerEndpoint(
                        $"/swagger/{description}/swagger.json",
                        $"AccountManagement Project {description.ToUpperInvariant()}");
                }
            });

        return app;
    }
}