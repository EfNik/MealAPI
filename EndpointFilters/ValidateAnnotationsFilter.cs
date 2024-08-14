using MealAPI.Models;
using MiniValidation;

namespace MealAPI.EndpointFilters;

public class ValidateAnnotationsFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var mealForCreationDto = context.GetArgument<MealForCreationDto>(2);

        if(!MiniValidator.TryValidate(mealForCreationDto, out var validationErrors))
        {
            return TypedResults.ValidationProblem(validationErrors);
        }

        return await next(context);
    }
}
