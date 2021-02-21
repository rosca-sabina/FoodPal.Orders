using Microsoft.AspNetCore.Mvc;

namespace FoodPal.Orders.API.Controllers
{
    /// <summary>
    /// Base controller class for FoodPal Orders API
    /// </summary>
    [ApiController]
	[Produces("application/json")]
	[Route("v{version:apiVersion}/[controller]")]
	public class ApiBaseController : ControllerBase
	{

	}
}
