using Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Abstraction.DesignPatterns.Facade
{
    public interface IOrderStore
    {
        Task SaveAsync(Order order, CancellationToken cancellationToken = default);
        Task<Order> GetByIdAsync(Guid orderId, CancellationToken cancellationToken = default);
    }
}
