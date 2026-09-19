using Application.Abstraction.DesignPatterns.Facade;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Implementation.DesignPatterns.Facade
{
    public class ShippingService : IShippingService
    {
        public Task<Shipment> ArrangeAsync(string shippingAddress, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new Shipment
            {
                TrackingNumber = $"TRK-{Guid.NewGuid():N}"[..12].ToUpperInvariant(),
                Carrier = "Demo Express"
            });
        }

        public Task CancelAsync(string trackingNumber, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
