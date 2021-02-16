using FoodPal.Orders.DTOs;
using FoodPal.Orders.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FoodPal.Orders.API.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class OrdersController: ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ErrorInfoDTO))]
        public async Task<ActionResult<string>> CreateOrder(NewOrderDTO newOrder)
        {
            var requestId = await _orderService.CreateAsync(newOrder);
            return Accepted(requestId);
        }
    }
}
