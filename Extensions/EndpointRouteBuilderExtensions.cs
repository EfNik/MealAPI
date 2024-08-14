using AutoMapper;
using MealAPI.DbContexts;
using MealAPI.EndpointFilters;
using MealAPI.EndpointHandlers;
using MealAPI.Entities;
using MealAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace MealAPI.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static void RegisterMealsEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var mealsEndpoints = endpointRouteBuilder.MapGroup("/meals")
            .RequireAuthorization();
        var mealsWithGuidEndpoints = mealsEndpoints.MapGroup("/{mealId:guid}")
            .RequireAuthorization();

        var mealsWithDuidIdEndpointAndLockedFilters = endpointRouteBuilder.MapGroup("/meals/{mealId:guid}")
            .RequireAuthorization("RequireAdminToBeChef")
            .AddEndpointFilter(new MealIsLockedFilter(new Guid("fd630a57-2352-4731-b25c-db9cc7601b16")))
            .AddEndpointFilter(new MealIsLockedFilter(new Guid("b512d7cf-b331-4b54-8dae-d1228d128e8d")));

        mealsEndpoints.MapGet("", MealHandlers.GetMealsAsync);
        mealsWithGuidEndpoints.MapGet("", MealHandlers.GetMealByIdAsync)
            .WithName("GetMeal")
            .WithOpenApi()
            .WithSummary("Get a meal by Id.")
            .WithDescription("Each meal has a unique Id. This Id is a GUID. You cat get a meal using this endpoint by providing its Id.");
        mealsEndpoints.MapGet("/{mealName}", MealHandlers.GetMealByNameAsync)
            .AllowAnonymous()
            .WithOpenApi(operation =>
            {
                operation.Deprecated = true;
                return operation;
            });
        mealsEndpoints.MapPost("", MealHandlers.CreateMealAsync)
            .RequireAuthorization("RequireAdminToBeChef")
            .AddEndpointFilter<ValidateAnnotationsFilter>()
            .ProducesValidationProblem(400)
            .Accepts<MealForCreationDto>(
                "application/json",
                "application/vnd.marvin.mealforcreation+json"); ;

        mealsWithDuidIdEndpointAndLockedFilters.MapPut("", MealHandlers.UpdateMealAsync);

        mealsWithDuidIdEndpointAndLockedFilters.MapDelete("", MealHandlers.DeleteMealAsync)
            .AddEndpointFilter<LogNotFoundResponseFilter>();


    }

    public static void RegisterIngredientsEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {

        var ingredientsEndpoints = endpointRouteBuilder.MapGroup("/meals/{mealId:guid}/ingredients")
            .RequireAuthorization();

        ingredientsEndpoints.MapGet("", IngredientsHandlers.GetIngredientsAsync);
        ingredientsEndpoints.MapPost("", () =>
        {
            throw new NotImplementedException();
        });
    }
}