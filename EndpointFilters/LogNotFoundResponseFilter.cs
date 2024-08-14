using System.Net;

namespace MealAPI.EndpointFilters;

public class LogNotFoundResponseFilter : IEndpointFilter
{
    private readonly ILogger<LogNotFoundResponseFilter> logger;

    public LogNotFoundResponseFilter(ILogger<LogNotFoundResponseFilter> logger)
    {
        this.logger = logger;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var result = await next(context);
        var actualResult = (result is INestedHttpResult) ? ((INestedHttpResult)result).Result : (IResult)result;

        if ((actualResult as IStatusCodeHttpResult)?.StatusCode == (int)HttpStatusCode.NotFound)
        {
            logger.LogInformation($"Resource {context.HttpContext.Request.Path} was not found.");
        }

        return result;
    }
}
