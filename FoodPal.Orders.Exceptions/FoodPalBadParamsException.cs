using FoodPal.Orders.DTOs;
using FoodPal.Orders.Enums;
using System;
using System.Collections.Generic;

namespace FoodPal.Orders.Exceptions
{
    public class FoodPalBadParamsException: BaseFoodPalException
    {
		protected List<Exception> ParamExceptions;

		public FoodPalBadParamsException() : this(string.Empty) { }

		public FoodPalBadParamsException(string badParamsMessage) : this(new ErrorInfoDTO() { InfoType = ErrorInfoType.Error, Message = badParamsMessage }) { }

		public FoodPalBadParamsException(List<string> errors) : this(new ErrorInfoDTO() { InfoType = ErrorInfoType.Error, Message = string.Join("; ", errors) }) { }

		public FoodPalBadParamsException(ErrorInfoDTO errorInfoDto, List<Exception> paramExceptions = null) : base(errorInfoDto)
		{
			ParamExceptions = paramExceptions;
		}
	}
}
