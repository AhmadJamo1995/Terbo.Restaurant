using AutoMapper;
using Dtos.Ingredient;
using Entities;

namespace Terbo.Restaurant.Web.AutoMapper
{
    public class IngredientProfile : Profile
    {
        public IngredientProfile()
        {
            CreateMap<Ingredient , IngredientDto>().ReverseMap();
        }
    }
}
