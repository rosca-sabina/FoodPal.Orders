using FoodPal.Orders.DTOs;
using FoodPal.Orders.Models;

namespace FoodPal.Orders.Mappers
{
    internal class OrderItemProfile: AbstractProfile
    {
        public OrderItemProfile()
        {
            CreateMap<NewOrderItemDTO, OrderItem>();
            CreateMap<OrderItem, OrderItemDTO>()
                .ForMember(dest => dest.OrderItemStatus, opt => opt.MapFrom(source => source.Status));
        }
    }
}
