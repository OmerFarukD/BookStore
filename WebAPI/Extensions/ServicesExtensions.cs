using Microsoft.EntityFrameworkCore;
using Repositories.Abstracts;
using Repositories.EfCore;
using Services.Abstracts;
using Services.Concretes;

namespace WebAPI.Extensions;

public static class ServicesExtensions
{
    public static void ConfigureSqlContext(this IServiceCollection services,IConfiguration configuration) => 
        services.AddDbContext<RepositoryContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("SqlCon")));

    public static void ConfigureRepositoryManager(this IServiceCollection services) =>
        services.AddScoped<IRepositoryManager, RepositoryManager>();


    public static void ConfigureServiceManager(this IServiceCollection services) =>
        services.AddScoped<IServiceManager, ServiceManager>();
}