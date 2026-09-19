using System.Threading;
using System.Threading.Tasks;

namespace Application.Abstraction.DesignPatterns.Facade
{
    public interface IShippingService
    {
        Task<Shipment> ArrangeAsync(string shippingAddress, CancellationToken cancellationToken = default);
        Task CancelAsync(string trackingNumber, CancellationToken cancellationToken = default);
    }

    public class Shipment
    {
        public string TrackingNumber { get; init; } = string.Empty;
        public string Carrier { get; init; } = string.Empty;
    }
}
