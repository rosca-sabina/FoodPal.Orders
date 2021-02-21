using FoodPal.Orders.DTOs;
using FoodPal.Orders.Enums;
using FoodPal.Orders.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodPal.Orders.API.Filters
{
    /// <summary>
    /// Custom filter used to handle API internal exceptions
    /// </summary>
    public class ExceptionFilter : IExceptionFilter, IActionFilter
	{
		private string _route;
		private Dictionary<string, object> _actionArgs;
		readonly ILogger<ExceptionFilter> _logger;

		/// <summary>
		/// Filter constructor
		/// </summary>
		/// <param name="logger"></param>
		public ExceptionFilter(ILogger<ExceptionFilter> logger) => _logger = logger;

		/// <summary>
		/// OnActionExecuted
		/// </summary>
		/// <param name="context"></param>
		public void OnActionExecuted(ActionExecutedContext context) { }

		/// <summary>
		/// OnActionExecuting
		/// </summary>
		/// <param name="context"></param>
		public void OnActionExecuting(ActionExecutingContext context)
		{
			_route = context.ActionDescriptor.AttributeRouteInfo.Template;
			_actionArgs = new Dictionary<string, object>(context.ActionArguments);
		}

		/// <summary>
		/// OnException
		/// </summary>
		/// <param name="context"></param>
		public void OnException(ExceptionContext context)
		{
			// defaults
			int defaultStatusCode = StatusCodes.Status500InternalServerError;

			object defaultValue = new ErrorInfoDTO()
			{
				InfoType = ErrorInfoType.Error,
				Message = "Internal server error",
				Details = string.Empty
			};

			if (IsFoodPalException(context.Exception))
			{
				context.Result = HandleFoodPalException(context.Exception as BaseFoodPalException, defaultValue, defaultStatusCode);
			}
			else
			{
				context.Result = new ObjectResult(defaultValue) { StatusCode = defaultStatusCode };
			}

			LogError(context);
		}

		private bool IsFoodPalException(Exception ex)
		{
			return ex is BaseFoodPalException;
		}

		private ObjectResult HandleFoodPalException(BaseFoodPalException ex, object defaultObjectValue, int defaultStatusCode)
		{
			object returnedValue = defaultObjectValue;
			int returnedStatusCode = defaultStatusCode;

			switch (ex)
			{
				case FoodPalNotFoundException foodPalNotFoundException:
					returnedStatusCode = StatusCodes.Status404NotFound;
					returnedValue = foodPalNotFoundException.ErrorInfo.Message;
					break;

				case FoodPalBadParamsException foodPalBadParametersException:
					returnedStatusCode = StatusCodes.Status400BadRequest;
					returnedValue = foodPalBadParametersException.ErrorInfo;
					break;

				default:
					break;
			}

			return new ObjectResult(returnedValue) { StatusCode = returnedStatusCode };
		}

		private void LogError(ExceptionContext context)
		{
			var defaultCode = StatusCodes.Status500InternalServerError;
			var message = JsonConvert.SerializeObject(context.Result);
			var exceptionMessage = FormatException(context.Exception);

			if (context.Result is ObjectResult)
			{
				var contextStatusCode = (context.Result as ObjectResult).StatusCode;
				defaultCode = contextStatusCode ?? defaultCode;
			}

			using (_logger.BeginScope("Exception Context Data"))
			{
				switch (defaultCode)
				{
					case StatusCodes.Status500InternalServerError:
						_logger.LogError(message);
						_logger.LogError(exceptionMessage);
						break;
					default:
						_logger.LogInformation(message);
						_logger.LogDebug(exceptionMessage);
						break;
				}
			}
		}

		private string FormatException(Exception exception, object returnedValue = null)
		{
			var loggedMsg = new StringBuilder();
			loggedMsg.Append($"Route: {this._route}");
			loggedMsg.Append($"{Environment.NewLine}");
			loggedMsg.Append($"Inputs");
			loggedMsg.Append($"{Environment.NewLine}");

			if (this._actionArgs != null)
			{
				foreach (var p in this._actionArgs)
				{
					loggedMsg.Append($"{p.Key} = ");
					loggedMsg.Append($"{JsonConvert.SerializeObject(p.Value)}");
					loggedMsg.Append($"{Environment.NewLine}");
				}
				if (exception != null)
				{
					loggedMsg.Append($"InnerException{Environment.NewLine}");
					loggedMsg.Append($"{JsonConvert.SerializeObject(exception)}");
				}
			}

			if (returnedValue != null)
			{
				loggedMsg.Append($"InnerException{Environment.NewLine}");
				loggedMsg.Append($"Response object");
				loggedMsg.Append($"{JsonConvert.SerializeObject(returnedValue)}");
			}

			return loggedMsg.ToString();
		}
	}
}
