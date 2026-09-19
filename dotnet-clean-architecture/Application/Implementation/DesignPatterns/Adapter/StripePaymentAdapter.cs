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
    /// Adapter: translates IPaymentProcessor calls into StripeGateway's cents-based API.
    /// </summary>
    public class StripePaymentAdapter : IPaymentProcessor
    {
        public const string ProviderName = "stripe";

        private readonly StripeGateway _stripeGateway;

        public StripePaymentAdapter(StripeGateway stripeGateway)
        {
            _stripeGateway = stripeGateway;
        }

        public string Provider => ProviderName;

        public Task<ProcessPaymentResponse> ProcessAsync(ProcessPaymentRequest request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(request.PaymentToken))
                throw new CustomHttpException(HttpStatusCode.BadRequest, "Stripe requires a payment token.");

            var amountInCents = ToCents(request.Amount);
            var charge = _stripeGateway.CreateCharge(amountInCents, request.Currency, request.PaymentToken);

            return Task.FromResult(new ProcessPaymentResponse
            {
                Provider = Provider,
                TransactionId = charge.Id,
                Amount = FromCents(charge.AmountInCents),
                Currency = charge.Currency.ToUpperInvariant(),
                Status = charge.Status
            });
        }

        private static int ToCents(decimal amount)
        {
            return (int)decimal.Round(amount * 100, 0, MidpointRounding.AwayFromZero);
        }

        private static decimal FromCents(int amountInCents)
        {
            return amountInCents / 100m;
        }
    }
}
