using AutoMapper;
using MealAPI.DbContexts;
using MealAPI.Entities;
using MealAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MealAPI.EndpointHandlers;

public static class MealHandlers
{
    public static async Task<Ok<IEnumerable<MealDto>>> GetMealsAsync(MealDbContext dbContext,
    IMapper mapper,
    ILogger<MealDto> logger,
    [FromQuery] string? name)
    {
        // Testing the logger
        logger.LogInformation("Getting all the meals!");

        return TypedResults.Ok(mapper.Map<IEnumerable<MealDto>>(await dbContext.Meals.
            Where(m => name == null || m.Name.Contains(name)).
            ToListAsync()));
    }

    public static async Task<Results<NotFound, Ok<MealDto>>> GetMealByIdAsync(MealDbContext mealsDbContext,
       IMapper mapper,
           Guid MealId)
    {
        var MealEntity = await mealsDbContext.Meals.FirstOrDefaultAsync(d => d.Id == MealId);
        if (MealEntity == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(mapper.Map<MealDto>(MealEntity));
    }

    public static async Task<Ok<MealDto>> GetMealByNameAsync(MealDbContext mealsDbContext,
            IMapper mapper,
            string MealName)
    {
        return TypedResults.Ok(mapper.Map<MealDto>(await mealsDbContext.Meals.FirstOrDefaultAsync(d => d.Name == MealName)));
    }

    public static async Task<CreatedAtRoute<MealDto>> CreateMealAsync(MealDbContext mealsDbContext,
    IMapper mapper,
    MealForCreationDto MealForCreationDto
    )
    {
        var MealEntity = mapper.Map<Meal>(MealForCreationDto);
        mealsDbContext.Add(MealEntity);
        await mealsDbContext.SaveChangesAsync();

        var MealToReturn = mapper.Map<MealDto>(MealEntity);

        return TypedResults.CreatedAtRoute(
            MealToReturn,
            "GetMeal",
            new
            {
                MealId = MealToReturn.Id
            });

    }

    public static async Task<Results<NotFound, NoContent>> UpdateMealAsync
        (MealDbContext mealsDbContext,
            IMapper mapper,
            Guid MealId,
            MealForUpdateDto MealForUpdateDto)
    {
        var MealEntity = await mealsDbContext.Meals.FirstOrDefaultAsync(d => d.Id == MealId);
        if (MealEntity == null)
        {
            return TypedResults.NotFound();
        }

        mapper.Map(MealForUpdateDto, MealEntity);
        await mealsDbContext.SaveChangesAsync();
        return TypedResults.NoContent();
    }

    public static async Task<Results<NotFound, NoContent>> DeleteMealAsync(MealDbContext mealsDbContext,
            Guid MealId)
    {
        var MealEntity = await mealsDbContext.Meals.FirstOrDefaultAsync(d => d.Id == MealId);
        if (MealEntity == null)
        {
            return TypedResults.NotFound();
        }

        mealsDbContext.Meals.Remove(MealEntity);
        await mealsDbContext.SaveChangesAsync();
        return TypedResults.NoContent();
    }

}
