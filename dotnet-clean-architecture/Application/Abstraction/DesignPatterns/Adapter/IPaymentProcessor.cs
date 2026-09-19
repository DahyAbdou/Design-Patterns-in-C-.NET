using Application.Request.Adapter;
using Application.Response.Adapter;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Abstraction.DesignPatterns.Adapter
{
    /// <summary>
    /// Target: the interface the application already depends on.
    /// Stripe and PayPal SDKs do not look like this; adapters make them look like this.
    /// </summary>
    public interface IPaymentProcessor
    {
        string Provider { get; }
        Task<ProcessPaymentResponse> ProcessAsync(ProcessPaymentRequest request, CancellationToken cancellationToken = default);
    }
}
