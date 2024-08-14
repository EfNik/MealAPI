using AutoMapper;
using MealAPI.DbContexts;
using MealAPI.Entities;
using MealAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace MealAPI.EndpointHandlers;

public static class IngredientsHandlers
{
    public static async Task<Results<NotFound, Ok<IEnumerable<IngredientDto>>>> GetIngredientsAsync(
        MealDbContext mealsDbContext,
        IMapper mapper,
        Guid mealId)
    {
        var mealEntity = await mealsDbContext.Meals.FirstOrDefaultAsync(d => d.Id == mealId);
        if (mealEntity == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(mapper.Map<IEnumerable<IngredientDto>>((await mealsDbContext.Meals
            .Include(d => d.Ingredients)
            .FirstOrDefaultAsync(d => d.Id == mealId))?.Ingredients));
    }
}