using Application.Abstraction.DesignPatterns.Facade;
using Application.Common.Exceptions;
using Domain.Entities;
using System;
using System.Collections.Concurrent;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Implementation.DesignPatterns.Facade
{
    public class InMemoryOrderStore : IOrderStore
    {
        private readonly ConcurrentDictionary<Guid, Order> _orders = new();

        public Task SaveAsync(Order order, CancellationToken cancellationToken = default)
        {
            _orders[order.Id] = order;
            return Task.CompletedTask;
        }

        public Task<Order> GetByIdAsync(Guid orderId, CancellationToken cancellationToken = default)
        {
            if (!_orders.TryGetValue(orderId, out var order))
                throw new CustomHttpException(HttpStatusCode.NotFound, $"Order '{orderId}' was not found.");

            return Task.FromResult(order);
        }
    }
}
