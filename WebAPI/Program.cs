using Microsoft.AspNetCore.Mvc;
using NLog;
using Presentation.ActionFilters;
using Services.Abstracts;
using Services.Concretes;
using WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);
LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(),"/nlog.config"));
// Add services to the container.

builder.Services.AddControllers(config =>
    {
        config.RespectBrowserAcceptHeader = true;
        config.ReturnHttpNotAcceptable = true;
    })
    .AddXmlDataContractSerializerFormatters()
    .AddCustomCsvFormatter()
    .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);
    //.AddNewtonsoftJson();


builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureLoggerService();
builder.Services.ConfigureActionFilters();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.ConfigureCors();
builder.Services.AddScoped<ValidationFilterAttribute>();
builder.Services.ConfigureDataShaper();
builder.Services.AddCustomMediaType();
builder.Services.AddScoped<IBookLinks, BookLinks>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILoggerService>();
app.ConfigureExceptionHandler(logger);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsProduction())
{
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthorization();

app.MapControllers();

app.Run();