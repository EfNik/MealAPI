namespace MealAPI.EndpointFilters;

public class MealIsLockedFilter : IEndpointFilter
{
    private readonly Guid lockedMealId;

    public MealIsLockedFilter(Guid lockedMealId)
    {
        this.lockedMealId = lockedMealId;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next) 
    {
        Guid mealId;
        if(context.HttpContext.Request.Method == "PUT")
        {
            mealId = context.GetArgument<Guid>(2);
        }
        else if (context.HttpContext.Request.Method == "DELETE")
        {
            mealId = context.GetArgument<Guid>(1);
        }
        else
        {
            throw new NotSupportedException("This filter is not supported for this method.");
        }

        if (mealId == lockedMealId)
        {
            return TypedResults.Problem(new()
            {
                Status = 400,
                Title = "Meal is already perfect, no need to change it",
                Detail = "You cannot update or delete it"
            });
        }

        // Invoke the next filter
        var result = await next.Invoke(context);
        return result;
    }
}
