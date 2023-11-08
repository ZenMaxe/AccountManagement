using System.Text;
using System.Text.Json;
using AccountManagement.DAL;
using AccountManagement.Domain.Entities.Identity;
using AccountManagement.Domain.Responses;
using AccountManagement.Providers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace AccountManagement.Configuration.Extensions;

public static class IdentityExtension
{

    public static void ConfigureIdentity(this IServiceCollection serviceCollection)
    {
        var builder = serviceCollection.AddIdentityCore<AppUser>();


        builder.Services.Configure<IdentityOptions>(
            options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Tokens.PasswordResetTokenProvider = "NumericTokenProvider";
            });
        serviceCollection.Configure<DataProtectionTokenProviderOptions>(x => x.TokenLifespan = TimeSpan.FromHours(8));

        builder = new IdentityBuilder(builder.UserType, typeof(AppUser), serviceCollection);

        builder.AddEntityFrameworkStores<AppDbContext>()
               .AddDefaultTokenProviders()
               .AddTokenProvider<NumericTokenProvider<AppUser>>("NumericTokenProvider");

    }

    public static void ConfigureJwt(this IServiceCollection serviceCollection,
                                    IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtDetails");

        serviceCollection.AddAuthentication(
                              options =>
                              {
                                  options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                                  options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                              })
                         .AddJwtBearer(
                              options =>
                              {
                                  options.TokenValidationParameters = new TokenValidationParameters
                                  {
                                      ValidateIssuer = true,
                                      ValidateAudience = false,
                                      ValidateIssuerSigningKey = true,
                                      ValidIssuer = jwtSettings.GetSection("Issuer").Value,
                                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.GetSection("Key").Value!)),

                                  };
                                  options.Events = new JwtBearerEvents
                                  {
                                      OnChallenge = context =>
                                      {
                                          context.HandleResponse();
                                          context.Response.StatusCode = 401;
                                          context.Response.ContentType = "application/json";
                                          var result = ApiResult<string>.Failure(new List<string> { "Please Login Again" }, 401);
                                          var json = JsonSerializer.Serialize(result);

                                          return context.Response.WriteAsync(json);
                                      }
                                  };
                              });
    }
}