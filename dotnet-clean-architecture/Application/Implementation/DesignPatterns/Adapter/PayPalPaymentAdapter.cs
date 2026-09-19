using Application.Abstraction.DesignPatterns.Adapter;
using Application.Common.Exceptions;
using Application.Implementation.DesignPatterns.Adapter.Adaptees;
using Application.Request.Adapter;
using Application.Response.Adapter;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Implementation.DesignPatterns.Adapter
{
    /// <summary>
    /// Adapter: translates IPaymentProcessor calls into PayPalGateway's email-based API.
    /// </summary>
    public class PayPalPaymentAdapter : IPaymentProcessor
    {
        public const string ProviderName = "paypal";

        private readonly PayPalGateway _payPalGateway;

        public PayPalPaymentAdapter(PayPalGateway payPalGateway)
        {
            _payPalGateway = payPalGateway;
        }

        public string Provider => ProviderName;

        public Task<ProcessPaymentResponse> ProcessAsync(ProcessPaymentRequest request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(request.PayerEmail))
                throw new CustomHttpException(HttpStatusCode.BadRequest, "PayPal requires a payer email.");

            var transaction = _payPalGateway.SendMoney(request.Amount, request.Currency, request.PayerEmail);

            return Task.FromResult(new ProcessPaymentResponse
            {
                Provider = Provider,
                TransactionId = transaction.TransactionId,
                Amount = transaction.Amount,
                Currency = transaction.CurrencyCode,
                Status = transaction.Ack
            });
        }
    }
}
