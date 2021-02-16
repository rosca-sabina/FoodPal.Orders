using FoodPal.Orders.Enums;

namespace FoodPal.Orders.DTOs
{
    public class ErrorInfoDTO
    {
        public ErrorInfoType InfoType { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
    }
}
