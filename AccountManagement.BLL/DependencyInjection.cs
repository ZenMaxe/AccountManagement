using AccountManagement.BLL.Services;
using AccountManagement.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AccountManagement.BLL;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IAccountService, AccountService>();
        serviceCollection.AddTransient<IUserService, AccountService>();
        serviceCollection.AddScoped<IAuthService, AuthService>();
        serviceCollection.AddScoped<IJwtHandler, JwtHandler>();

        return serviceCollection;
    }
}