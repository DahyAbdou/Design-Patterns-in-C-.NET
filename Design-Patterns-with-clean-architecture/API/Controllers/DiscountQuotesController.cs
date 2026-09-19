using Application.Abstraction.DesignPatterns.Strategy;
using Application.Request.Strategy;
using Application.Response.Strategy;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/design-patterns/strategy/discounts")]
    [ApiController]
    public class DiscountQuotesController : ControllerBase
    {
        private readonly IDiscountCalculator _discountCalculator;

        public DiscountQuotesController(IDiscountCalculator discountCalculator)
        {
            _discountCalculator = discountCalculator;
        }

        [HttpPost]
        public ActionResult<DiscountQuoteResponse> Quote([FromBody] CalculateDiscountRequest request)
        {
            var quote = _discountCalculator.Quote(request);
            return Ok(quote);
        }
    }
}
