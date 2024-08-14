using AutoMapper;
using MealAPI.Entities;
using MealAPI.Models;

namespace MealAPI.Profiles;

public class MealProfile : Profile
{

    public MealProfile()
    {
        CreateMap<Meal, MealDto>();
        CreateMap<MealForCreationDto, Meal>();
        CreateMap<MealForUpdateDto, Meal>();
    }
}
