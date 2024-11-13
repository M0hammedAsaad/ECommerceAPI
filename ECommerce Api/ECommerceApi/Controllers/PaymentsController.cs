using ECommerce.InfraStructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly PayPalService _payPalService;

        public PaymentsController(PayPalService payPalService)
        {
            _payPalService = payPalService;
        }

        //[HttpPost("Create")]
        //public async Task<IActionResult> CreateAsync(decimal amount)
        //{
        //    if (amount <= 0)
        //        return BadRequest("Amount must be more than 0 !");


        //}
    }
}
