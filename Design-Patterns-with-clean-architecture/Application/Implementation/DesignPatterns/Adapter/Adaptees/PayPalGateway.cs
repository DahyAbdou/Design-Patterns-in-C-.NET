using Application.Common.Exceptions;
using System;
using System.Net;

namespace Application.Implementation.DesignPatterns.Adapter.Adaptees
{
    /// <summary>
    /// Adaptee: a third-party SDK you cannot change.
    /// It takes a decimal amount and a payer email, not a card token.
    /// </summary>
    public class PayPalGateway
    {
        public PayPalTransaction SendMoney(decimal amount, string currencyCode, string payerEmail)
        {
            if (string.IsNullOrWhiteSpace(payerEmail))
                throw new CustomHttpException(HttpStatusCode.BadRequest, "PayPal requires a payer email.");

            if (string.Equals(payerEmail, "fail@paypal.com", StringComparison.OrdinalIgnoreCase))
                throw new CustomHttpException(HttpStatusCode.BadRequest, "PayPal rejected the payment.");

            return new PayPalTransaction
            {
                TransactionId = $"PAYID-{Guid.NewGuid():N}"[..20].ToUpperInvariant(),
                Amount = amount,
                CurrencyCode = currencyCode.ToUpperInvariant(),
                Ack = "Success"
            };
        }
    }

    public class PayPalTransaction
    {
        public string TransactionId { get; init; } = string.Empty;
        public decimal Amount { get; init; }
        public string CurrencyCode { get; init; } = string.Empty;
        public string Ack { get; init; } = string.Empty;
    }
}
