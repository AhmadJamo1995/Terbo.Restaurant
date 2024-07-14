using AutoMapper;
using Dtos.Meal;
using Entities;

namespace Terbo.Restaurant.Web.AutoMapper
{
    public class MealProfile : Profile
    {
        public MealProfile()
        {
            CreateMap<Meal, MealDto>();
            CreateMap<Meal , MealDetailsDto>();
            CreateMap<CreateUpdateMealDto, Meal>();

            CreateMap<Meal, CreateUpdateMealDto>()
                .ForMember(
                        createUpdateMealDto => createUpdateMealDto.IngredientIds,
                    opts => opts
                        .MapFrom(meal => meal.Ingredients.Select(ingredient => ingredient.Id)));
        }
    }
}
