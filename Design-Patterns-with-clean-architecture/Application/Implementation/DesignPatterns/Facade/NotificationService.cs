using Application.Abstraction.DesignPatterns.Facade;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Implementation.DesignPatterns.Facade
{
    /// <summary>
    /// Stand-in for email/SMS. A real implementation would live in Infrastructure.
    /// </summary>
    public class NotificationService : INotificationService
    {
        public Task NotifyOrderPlacedAsync(string customerEmail, string orderId, string trackingNumber, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task NotifyOrderCancelledAsync(string customerEmail, string orderId, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
