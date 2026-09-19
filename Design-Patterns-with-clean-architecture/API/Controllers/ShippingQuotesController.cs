using Application.Abstraction.DesignPatterns.Strategy;
using Application.Request.Strategy;
using Application.Response.Strategy;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/design-patterns/strategy/shipping-quotes")]
    [ApiController]
    public class ShippingQuotesController : ControllerBase
    {
        private readonly IShippingCostCalculator _shippingCostCalculator;

        public ShippingQuotesController(IShippingCostCalculator shippingCostCalculator)
        {
            _shippingCostCalculator = shippingCostCalculator;
        }

        [HttpPost]
        public ActionResult<ShippingQuoteResponse> Quote([FromBody] CalculateShippingRequest request)
        {
            var quote = _shippingCostCalculator.Quote(request);
            return Ok(quote);
        }
    }
}
