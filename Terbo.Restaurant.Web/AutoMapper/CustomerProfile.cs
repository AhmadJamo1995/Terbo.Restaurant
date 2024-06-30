using AutoMapper;
using Dtos.Customer;
using Entities;

namespace Terbo.Restaurant.Web.AutoMapper
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<Customer, CustomerDto>();
            CreateMap<Customer, CustomerDetailsDto>();
            CreateMap<Customer , CreateUpdateCustomerDto>().ReverseMap();
        }
    }
}
