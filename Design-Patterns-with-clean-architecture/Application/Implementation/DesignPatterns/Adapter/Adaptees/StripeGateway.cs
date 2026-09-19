using Application.Common.Exceptions;
using System;
using System.Net;

namespace Application.Implementation.DesignPatterns.Adapter.Adaptees
{
    /// <summary>
    /// Adaptee: a third-party SDK you cannot change.
    /// It charges in cents and returns a Stripe-shaped result.
    /// </summary>
    public class StripeGateway
    {
        public StripeCharge CreateCharge(int amountInCents, string currency, string stripeToken)
        {
            if (amountInCents <= 0)
                throw new CustomHttpException(HttpStatusCode.BadRequest, "Stripe amount must be greater than zero.");

            if (string.Equals(stripeToken, "tok_declined", StringComparison.OrdinalIgnoreCase))
                throw new CustomHttpException(HttpStatusCode.BadRequest, "Stripe declined the card.");

            return new StripeCharge
            {
                Id = $"ch_{Guid.NewGuid():N}"[..18],
                AmountInCents = amountInCents,
                Currency = currency.ToLowerInvariant(),
                Status = "succeeded"
            };
        }
    }

    public class StripeCharge
    {
        public string Id { get; init; } = string.Empty;
        public int AmountInCents { get; init; }
        public string Currency { get; init; } = string.Empty;
        public string Status { get; init; } = string.Empty;
    }
}
