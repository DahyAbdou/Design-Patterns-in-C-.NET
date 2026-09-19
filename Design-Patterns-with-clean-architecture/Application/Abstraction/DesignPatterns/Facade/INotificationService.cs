using System.Threading;
using System.Threading.Tasks;

namespace Application.Abstraction.DesignPatterns.Facade
{
    public interface INotificationService
    {
        Task NotifyOrderPlacedAsync(string customerEmail, string orderId, string trackingNumber, CancellationToken cancellationToken = default);
        Task NotifyOrderCancelledAsync(string customerEmail, string orderId, CancellationToken cancellationToken = default);
    }
}
