﻿using System.Text;
using AspNetCoreRateLimit;
using Entities.Dtos;
using Entities.Models;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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
        services.AddSingleton<LogFilterAttribute>();
        services.AddScoped<ValidateMediaTypeAttribute>();
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

    public static void AddCustomMediaType(this IServiceCollection services)
    {
        services.Configure<MvcOptions>(config =>
        {
            var systemTextJsonOutputFormatter = config.OutputFormatters
                .OfType<SystemTextJsonOutputFormatter>()?.FirstOrDefault();
            if (systemTextJsonOutputFormatter is not null)
            {
                systemTextJsonOutputFormatter.SupportedMediaTypes.Add("application/vnd.aib.hateoas+json");
                systemTextJsonOutputFormatter.SupportedMediaTypes.Add("application/vnd.aib.apiroot+json");
            }

            var xmlOutputFormatter = config.OutputFormatters
                .OfType<XmlDataContractSerializerOutputFormatter>()?.FirstOrDefault();

            if (xmlOutputFormatter is not null)
            {
                xmlOutputFormatter.SupportedMediaTypes.Add("application/vnd.aib.hateoas+xml");
                xmlOutputFormatter.SupportedMediaTypes.Add("application/vnd.aib.apiroot+xml");
            }
        });
    }

    public static void ConfigureVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(opt =>
        {
            opt.ReportApiVersions = true;
            opt.AssumeDefaultVersionWhenUnspecified = true;
            opt.DefaultApiVersion = new ApiVersion(1,0);
            opt.ApiVersionReader = new HeaderApiVersionReader("api-version");
        });
    }

    public static void ConfigureResponseCaching(this IServiceCollection service) =>
        service.AddResponseCaching();

    public static void ConfigureHttpCacheHeaders(this IServiceCollection serviceCollection) =>
        serviceCollection.AddHttpCacheHeaders(opt =>
        {
            opt.MaxAge = 70;
            opt.CacheLocation = CacheLocation.Private;
        });

    public static void ConfigureRateLimitingOptions(this IServiceCollection services)
    {
        var ratelimtRules = new List<RateLimitRule>()
        {
            new RateLimitRule()
            {
                Endpoint = "*",
                Limit = 3,
                Period = "1m"
            }
        };
        services.Configure<IpRateLimitOptions>(opt =>
        {
            opt.GeneralRules = ratelimtRules;
        });
        services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
        services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
    }

    public static void ConfigureIdentity(this IServiceCollection service)
    {
        var builder = service.AddIdentity<User, IdentityRole>(opts =>
        {
            opts.Password.RequireDigit = true;
            opts.Password.RequireLowercase = false;
            opts.Password.RequireUppercase = false;
            opts.Password.RequireNonAlphanumeric = false;
            opts.Password.RequiredLength = 6;
            opts.User.RequireUniqueEmail = true;
        }).AddEntityFrameworkStores<RepositoryContext>()
            .AddDefaultTokenProviders();
    }

    public static void ConfigureJWT(this IServiceCollection service, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["secretKey"];
        var validIssuer = jwtSettings["validIssuer"];
        var validAudience = jwtSettings["validAudience"];
        service.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

        }).AddJwtBearer(opt =>
        {
            opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = validIssuer,
                ValidAudience = validAudience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
            };
        });

    }

    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(s =>
        {
            s.SwaggerDoc("v1", new OpenApiInfo()
            {
                Title = "Book Store App",
                Version = "v1"
            });
            s.SwaggerDoc("v2", new OpenApiInfo()
            {
                Title = "Book Store App",
                Version = "v2"
            });
            
            s.AddSecurityDefinition("Bearer",new OpenApiSecurityScheme()
            {
                In = ParameterLocation.Header,
                Description = "Place to add JWT with Bearer",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            s.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme()
                    {
                        Reference = new OpenApiReference()
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Name = "Bearer"
                    },
                    new List<string>()
                }
            });
        });
    }

    public static void RegisterRepositories(this IServiceCollection services)
    {
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
    }

    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IBookService, BookManager>();
        services.AddScoped<ICategoryService, CategoryManager>();
        services.AddScoped<IAuthenticationService, AuthenticationManager>();
    }
}