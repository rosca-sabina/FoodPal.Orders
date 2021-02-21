using FoodPal.Orders.DTOs;
using FoodPal.Orders.Models;

namespace FoodPal.Orders.Mappers
{
    internal class OrderProfile: AbstractProfile
    {
        public OrderProfile()
        {
            CreateMap<NewOrderDTO, Order>();
            CreateMap<Order, OrderDTO>()
                .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(source => source.Status));
        }
    }
}
