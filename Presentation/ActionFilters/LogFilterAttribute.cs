using Entities.LogModel;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using NLog.Fluent;
using Services.Abstracts;

namespace Presentation.ActionFilters;

public class LogFilterAttribute : ActionFilterAttribute
{
    private readonly ILoggerService _loggerService;

    public LogFilterAttribute(ILoggerService loggerService)
    {
        _loggerService = loggerService;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        _loggerService.LogInfo(Log("OnActionExecuting",context.RouteData));
    }

    private string Log(string modelName, RouteData routeData)
    {
        var logdetails = new LogDetails()
        {
            ModelName = modelName,
            Action = routeData.Values["action"],
            Controller = routeData.Values["controller"],
            CreateAt = DateTime.Now,
        };
        if (routeData.Values.Count>=3)
        {
            logdetails.Id = routeData.Values["Id"];
        }

        return logdetails.ToString();
    }
}