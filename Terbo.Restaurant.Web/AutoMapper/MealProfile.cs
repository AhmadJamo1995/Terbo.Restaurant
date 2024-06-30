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
            CreateMap<Meal , CreateUpdateMealDto>().ReverseMap();
        }
    }
}
