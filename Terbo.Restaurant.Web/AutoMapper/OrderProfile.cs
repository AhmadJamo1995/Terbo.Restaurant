using AutoMapper;
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
            CreateMap<Order , CreateUpdateOrderDto>().ReverseMap();
        }
    }
}
