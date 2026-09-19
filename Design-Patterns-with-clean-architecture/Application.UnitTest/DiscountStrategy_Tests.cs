using Application.Abstraction.DesignPatterns.Strategy;
using Application.Common.Exceptions;
using Application.Implementation.DesignPatterns.Strategy;
using Application.Request.Strategy;
using NUnit.Framework;
using System.Collections.Generic;

namespace Application.UnitTest
{
    public class DiscountStrategy_Tests
    {
        [Test]
        public void RegularCustomer_PaysFullPrice()
        {
            var strategy = new RegularDiscountStrategy();

            var discount = strategy.Calculate(new DiscountCalculationInput { Subtotal = 100m });

            Assert.AreEqual(0m, discount);
            Assert.AreEqual(0m, strategy.DiscountPercent);
        }

        [Test]
        public void MemberCustomer_GetsTenPercentOff()
        {
            var strategy = new MemberDiscountStrategy();

            var discount = strategy.Calculate(new DiscountCalculationInput { Subtotal = 100m });

            Assert.AreEqual(10m, discount);
            Assert.AreEqual(10m, strategy.DiscountPercent);
        }

        [Test]
        public void PremiumCustomer_GetsTwentyPercentOff()
        {
            var strategy = new PremiumDiscountStrategy();

            var discount = strategy.Calculate(new DiscountCalculationInput { Subtotal = 50m });

            Assert.AreEqual(10m, discount);
            Assert.AreEqual(20m, strategy.DiscountPercent);
        }

        [Test]
        public void Calculator_UsesSelectedStrategy_AndReturnsAmountToPay()
        {
            var calculator = CreateCalculator();
            var request = CreateRequest("premium", quantity: 2, unitPrice: 25m);

            var quote = calculator.Quote(request);

            Assert.AreEqual("premium", quote.CustomerType);
            Assert.AreEqual(50m, quote.Subtotal);
            Assert.AreEqual(20m, quote.DiscountPercent);
            Assert.AreEqual(10m, quote.DiscountAmount);
            Assert.AreEqual(40m, quote.TotalToPay);
        }

        [Test]
        public void Resolver_WhenCustomerTypeUnknown_Throws()
        {
            var resolver = new DiscountStrategyResolver(new IDiscountStrategy[]
            {
                new RegularDiscountStrategy()
            });

            Assert.Throws<CustomHttpException>(() => resolver.Resolve("vip"));
        }

        private static DiscountCalculator CreateCalculator()
        {
            var resolver = new DiscountStrategyResolver(new IDiscountStrategy[]
            {
                new RegularDiscountStrategy(),
                new MemberDiscountStrategy(),
                new PremiumDiscountStrategy()
            });

            return new DiscountCalculator(resolver);
        }

        private static CalculateDiscountRequest CreateRequest(string customerType, int quantity, decimal unitPrice)
        {
            return new CalculateDiscountRequest
            {
                CustomerType = customerType,
                Items = new List<DiscountItemRequest>
                {
                    new DiscountItemRequest
                    {
                        Sku = "SKU-100",
                        ProductName = "Wireless Mouse",
                        Quantity = quantity,
                        UnitPrice = unitPrice
                    }
                }
            };
        }
    }
}
