using Application.Abstraction.DesignPatterns.Adapter;
using Application.Request.Adapter;
using Application.Response.Adapter;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/design-patterns/adapter/payments")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentProcessorResolver _paymentProcessorResolver;

        public PaymentsController(IPaymentProcessorResolver paymentProcessorResolver)
        {
            _paymentProcessorResolver = paymentProcessorResolver;
        }

        [HttpPost]
        public async Task<ActionResult<ProcessPaymentResponse>> Process([FromBody] ProcessPaymentRequest request, CancellationToken cancellationToken)
        {
            var processor = _paymentProcessorResolver.Resolve(request.Provider);
            var result = await processor.ProcessAsync(request, cancellationToken);
            return Ok(result);
        }
    }
}
