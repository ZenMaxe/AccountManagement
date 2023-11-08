using AccountManagement.DAL.Repositories;
using AccountManagement.DAL.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;

namespace AccountManagement.DAL;

public static class DependencyInjection
{
    public static IServiceCollection AddRepositories(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        return serviceCollection;
    }

    public static IServiceCollection AddUnitOfWork(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

        return serviceCollection;
    }
}