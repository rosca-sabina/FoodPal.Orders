using FoodPal.Orders.DTOs;
using FoodPal.Orders.Enums;

namespace FoodPal.Orders.Mappers
{
    internal class OrderStatusProfile: AbstractProfile
    {
        public OrderStatusProfile()
        {
            CreateMap<OrderStatus, StatusDTO>()
                .ForMember(dest => dest.StatusId, options => options.MapFrom(o => (int)o))
                .ForMember(dest => dest.StatusName, options => options.MapFrom(o => o.ToString()));

            CreateMap<OrderItemStatus, StatusDTO>()
                .ForMember(dest => dest.StatusId, options => options.MapFrom(o => (int)o))
                .ForMember(dest => dest.StatusName, options => options.MapFrom(o => o.ToString()));
        }
    }
}
