using AutoMapper;
using MealAPI.Entities;
using MealAPI.Models;

namespace MealAPI.Profiles;

public class IngredientProfile : Profile
{
    public IngredientProfile() 
    {
        CreateMap<Ingredient, IngredientDto>()
            .ForMember(
            m => m.MealId,
            o => o.MapFrom(s => s.Meals.First().Id));
    }
}