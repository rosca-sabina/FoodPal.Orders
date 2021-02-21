using FoodPal.Orders.DTOs;
using FoodPal.Orders.Models;

namespace FoodPal.Orders.Mappers
{
    internal class DeliveryDetailsProfile: AbstractProfile
    {
        public DeliveryDetailsProfile()
        {
            CreateMap<DeliveryDetailsDTO, DeliveryDetails>().ReverseMap();
        }
    }
}
