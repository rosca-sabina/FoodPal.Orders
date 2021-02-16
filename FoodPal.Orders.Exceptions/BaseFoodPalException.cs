using FoodPal.Orders.DTOs;
using System;

namespace FoodPal.Orders.Exceptions
{
    public class BaseFoodPalException: Exception
    {
        public ErrorInfoDTO ErrorInfo { get; private set; }
        public BaseFoodPalException(ErrorInfoDTO errorInfoDto) : base()
        {
            ErrorInfo = errorInfoDto;
        }
    }
}
