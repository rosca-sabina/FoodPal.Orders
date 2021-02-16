using FoodPal.Orders.DTOs;
using FoodPal.Orders.Enums;

namespace FoodPal.Orders.Exceptions
{
    public class FoodPalNotFoundException: BaseFoodPalException
    {
        public FoodPalNotFoundException() : base(null) { }

        public FoodPalNotFoundException(string entityIdentifier) : this(new ErrorInfoDTO() { InfoType = ErrorInfoType.Error, Message = entityIdentifier }) { }

        public FoodPalNotFoundException(ErrorInfoDTO errorInfoDto) : base(errorInfoDto) { }
    }
}
