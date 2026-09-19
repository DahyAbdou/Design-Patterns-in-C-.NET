using Application.Abstraction.DesignPatterns.Facade;
using Application.Common.Exceptions;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Implementation.DesignPatterns.Facade
{
    /// <summary>
    /// Simulated payment gateway. Send payment token "DECLINED" to exercise the failure path.
    /// </summary>
    public class PaymentService : IPaymentService
    {
        public Task<PaymentReceipt> ChargeAsync(string paymentToken, decimal amount, CancellationToken cancellationToken = default)
        {
            if (string.Equals(paymentToken, "DECLINED", StringComparison.OrdinalIgnoreCase))
                throw new CustomHttpException(HttpStatusCode.BadRequest, "Payment was declined.");

            return Task.FromResult(new PaymentReceipt
            {
                PaymentReference = $"PAY-{Guid.NewGuid():N}"[..16].ToUpperInvariant(),
                Amount = amount
            });
        }

        public Task RefundAsync(string paymentReference, decimal amount, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(paymentReference))
                throw new CustomHttpException(HttpStatusCode.BadRequest, "Payment reference is required to refund.");

            return Task.CompletedTask;
        }
    }
}
