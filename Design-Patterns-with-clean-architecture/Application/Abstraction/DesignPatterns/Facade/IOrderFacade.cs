using Application.Request.Order;
using Application.Response.Order;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Abstraction.DesignPatterns.Facade
{
    /// <summary>
    /// Facade: a single application API over inventory, payment, shipping, persistence, and notification.
    /// Controllers and other clients should depend on this contract, not on the subsystems.
    /// </summary>
    public interface IOrderFacade
    {
        Task<PlaceOrderResponse> PlaceOrderAsync(PlaceOrderRequest request, CancellationToken cancellationToken = default);
        Task<OrderDto> GetOrderAsync(Guid orderId, CancellationToken cancellationToken = default);
        Task<OrderDto> CancelOrderAsync(CancelOrderRequest request, CancellationToken cancellationToken = default);
    }
}
