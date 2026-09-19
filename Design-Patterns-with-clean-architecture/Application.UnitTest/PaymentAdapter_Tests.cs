using Application.Abstraction.DesignPatterns.Adapter;
using Application.Common.Exceptions;
using Application.Implementation.DesignPatterns.Adapter;
using Application.Implementation.DesignPatterns.Adapter.Adaptees;
using Application.Request.Adapter;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Application.UnitTest
{
    public class PaymentAdapter_Tests
    {
        [Test]
        public async Task StripeAdapter_ConvertsAmountToCents_AndMapsChargeId()
        {
            var adapter = new StripePaymentAdapter(new StripeGateway());
            var request = new ProcessPaymentRequest
            {
                Provider = "stripe",
                Amount = 25.50m,
                Currency = "USD",
                PaymentToken = "tok_visa"
            };

            var result = await adapter.ProcessAsync(request);

            Assert.AreEqual("stripe", result.Provider);
            Assert.AreEqual(25.50m, result.Amount);
            Assert.AreEqual("USD", result.Currency);
            Assert.AreEqual("succeeded", result.Status);
            StringAssert.StartsWith("ch_", result.TransactionId);
        }

        [Test]
        public void StripeAdapter_WhenTokenDeclined_Throws()
        {
            var adapter = new StripePaymentAdapter(new StripeGateway());
            var request = new ProcessPaymentRequest
            {
                Provider = "stripe",
                Amount = 10m,
                Currency = "USD",
                PaymentToken = "tok_declined"
            };

            Assert.ThrowsAsync<CustomHttpException>(async () => await adapter.ProcessAsync(request));
        }

        [Test]
        public async Task PayPalAdapter_UsesPayerEmail_AndMapsTransactionId()
        {
            var adapter = new PayPalPaymentAdapter(new PayPalGateway());
            var request = new ProcessPaymentRequest
            {
                Provider = "paypal",
                Amount = 40m,
                Currency = "usd",
                PayerEmail = "buyer@example.com"
            };

            var result = await adapter.ProcessAsync(request);

            Assert.AreEqual("paypal", result.Provider);
            Assert.AreEqual(40m, result.Amount);
            Assert.AreEqual("USD", result.Currency);
            Assert.AreEqual("Success", result.Status);
            StringAssert.StartsWith("PAYID-", result.TransactionId);
        }

        [Test]
        public void PayPalAdapter_WhenEmailMissing_Throws()
        {
            var adapter = new PayPalPaymentAdapter(new PayPalGateway());
            var request = new ProcessPaymentRequest
            {
                Provider = "paypal",
                Amount = 10m,
                Currency = "USD"
            };

            Assert.ThrowsAsync<CustomHttpException>(async () => await adapter.ProcessAsync(request));
        }

        [Test]
        public void Resolver_ReturnsMatchingAdapter()
        {
            var stripe = new StripePaymentAdapter(new StripeGateway());
            var paypal = new PayPalPaymentAdapter(new PayPalGateway());
            var resolver = new PaymentProcessorResolver(new IPaymentProcessor[] { stripe, paypal });

            Assert.AreSame(stripe, resolver.Resolve("STRIPE"));
            Assert.AreSame(paypal, resolver.Resolve("paypal"));
        }

        [Test]
        public void Resolver_WhenProviderUnknown_Throws()
        {
            var resolver = new PaymentProcessorResolver(new IPaymentProcessor[]
            {
                new StripePaymentAdapter(new StripeGateway())
            });

            Assert.Throws<CustomHttpException>(() => resolver.Resolve("applepay"));
        }
    }
}
