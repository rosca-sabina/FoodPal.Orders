using FoodPal.Orders.DTOs;
using FoodPal.Orders.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FoodPal.Orders.API.Controllers
{
    public class DeliveryDetailsController: ApiBaseController
    {
        private readonly IDeliveryDetailsService _deliveryDetailsService;

        public DeliveryDetailsController(IDeliveryDetailsService deliveryDetailsService)
        {
            _deliveryDetailsService = deliveryDetailsService;
        }

        [HttpGet]
        [Route("{orderId}")]
        [ProducesResponseType(typeof(DeliveryDetailsDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ErrorInfoDTO))]
        public async Task<ActionResult<DeliveryDetailsDTO>> GetOrderDeliveryDetails(int orderId)
        {
            var deliveryDetails = await _deliveryDetailsService.GetOrderDeliveryDetailsAsync(orderId);
            return Ok(deliveryDetails);
        }
    }
}
