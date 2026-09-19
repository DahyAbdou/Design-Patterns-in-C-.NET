using Application.Abstraction.DesignPatterns.Facade;
using Application.Request.Order;
using Application.Response.Order;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/design-patterns/facade/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderFacade _orderFacade;

        public OrdersController(IOrderFacade orderFacade)
        {
            _orderFacade = orderFacade;
        }

        [HttpPost]
        public async Task<ActionResult<PlaceOrderResponse>> PlaceOrder([FromBody] PlaceOrderRequest request, CancellationToken cancellationToken)
        {
            var order = await _orderFacade.PlaceOrderAsync(request, cancellationToken);
            return Ok(order);
        }

        [HttpGet("{orderId:guid}")]
        public async Task<ActionResult<OrderDto>> GetOrder(Guid orderId, CancellationToken cancellationToken)
        {
            var order = await _orderFacade.GetOrderAsync(orderId, cancellationToken);
            return Ok(order);
        }

        [HttpPost("{orderId:guid}/cancel")]
        public async Task<ActionResult<OrderDto>> CancelOrder(Guid orderId, CancellationToken cancellationToken)
        {
            var order = await _orderFacade.CancelOrderAsync(new CancelOrderRequest { OrderId = orderId }, cancellationToken);
            return Ok(order);
        }
    }
}
