using AutoMapper;
using Dtos.Meal;
using Dtos.Order;
using Entities;

namespace Terbo.Restaurant.Web.AutoMapper
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDto>();
            CreateMap<Order, OrderDetailsDto>();
            CreateMap<CreateUpdateOrderDto, Order>();


            CreateMap<Order, CreateUpdateOrderDto>()
                .ForMember(
                        createUpdateOrderDto => createUpdateOrderDto.MealIds,
                    opts => opts
                        .MapFrom(order => order.Meals.Select(meal => meal.Id)));
        }
    }
}
