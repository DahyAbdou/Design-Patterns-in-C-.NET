using System.Threading;
using System.Threading.Tasks;

namespace Application.Abstraction.DesignPatterns.Facade
{
    public interface IPaymentService
    {
        Task<PaymentReceipt> ChargeAsync(string paymentToken, decimal amount, CancellationToken cancellationToken = default);
        Task RefundAsync(string paymentReference, decimal amount, CancellationToken cancellationToken = default);
    }

    public class PaymentReceipt
    {
        public string PaymentReference { get; init; } = string.Empty;
        public decimal Amount { get; init; }
    }
}
