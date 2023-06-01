﻿using Entities.Dtos;
using Microsoft.EntityFrameworkCore;
using Presentation.ActionFilters;
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

    public static void ConfigureLoggerService(this IServiceCollection services) => services.AddSingleton<ILoggerService,LoggerManager>();

    public static void ConfigureActionFilters(this IServiceCollection services)
    {
        services.AddScoped<ValidationFilterAttribute>();
        services.AddScoped<LogFilterAttribute>();
    }

    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().WithExposedHeaders("X-Pagination");
            });
        });
    }
    public static void ConfigureDataShaper(this IServiceCollection services)
    {
        services.AddScoped<IDataShaper<BookDto>, DataShaper<BookDto>>();
    }
}