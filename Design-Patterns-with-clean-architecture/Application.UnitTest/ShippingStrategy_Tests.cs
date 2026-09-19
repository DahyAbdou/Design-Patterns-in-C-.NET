using Application.Abstraction.DesignPatterns.Strategy;
using Application.Common.Exceptions;
using Application.Implementation.DesignPatterns.Strategy;
using Application.Request.Strategy;
using NUnit.Framework;
using System.Collections.Generic;

namespace Application.UnitTest
{
    public class ShippingStrategy_Tests
    {
        [Test]
        public void StandardShipping_AlwaysChargesFlatRate()
        {
            var strategy = new StandardShippingStrategy();

            var cost = strategy.Calculate(new ShippingCalculationInput
            {
                Subtotal = 500m,
                TotalWeightKg = 10m,
                ItemCount = 3
            });

            Assert.AreEqual(15m, cost);
        }

        [Test]
        public void ExpressShipping_UsesWeightWhenAboveMinimum()
        {
            var strategy = new ExpressShippingStrategy();

            var cost = strategy.Calculate(new ShippingCalculationInput
            {
                Subtotal = 50m,
                TotalWeightKg = 4m,
                ItemCount = 1
            });

            Assert.AreEqual(32m, cost);
        }

        [Test]
        public void ExpressShipping_AppliesMinimumChargeWhenLight()
        {
            var strategy = new ExpressShippingStrategy();

            var cost = strategy.Calculate(new ShippingCalculationInput
            {
                Subtotal = 50m,
                TotalWeightKg = 1m,
                ItemCount = 1
            });

            Assert.AreEqual(25m, cost);
        }

        [Test]
        public void FreeShipping_WhenSubtotalMeetsThreshold_IsZero()
        {
            var strategy = new FreeShippingStrategy();

            var cost = strategy.Calculate(new ShippingCalculationInput
            {
                Subtotal = 200m,
                TotalWeightKg = 5m,
                ItemCount = 2
            });

            Assert.AreEqual(0m, cost);
        }

        [Test]
        public void FreeShipping_WhenSubtotalBelowThreshold_Throws()
        {
            var strategy = new FreeShippingStrategy();

            Assert.Throws<CustomHttpException>(() => strategy.Calculate(new ShippingCalculationInput
            {
                Subtotal = 199.99m,
                TotalWeightKg = 1m,
                ItemCount = 1
            }));
        }

        [Test]
        public void Calculator_UsesSelectedStrategy_AndAddsShippingToSubtotal()
        {
            var calculator = CreateCalculator();
            var request = CreateRequest("standard", quantity: 2, unitPrice: 25m, weightKg: 0.2m);

            var quote = calculator.Quote(request);

            Assert.AreEqual("standard", quote.ShippingMethod);
            Assert.AreEqual(50m, quote.Subtotal);
            Assert.AreEqual(15m, quote.ShippingCost);
            Assert.AreEqual(65m, quote.GrandTotal);
        }

        [Test]
        public void Resolver_WhenMethodUnknown_Throws()
        {
            var resolver = new ShippingCostStrategyResolver(new IShippingCostStrategy[]
            {
                new StandardShippingStrategy()
            });

            Assert.Throws<CustomHttpException>(() => resolver.Resolve("overnight"));
        }

        private static ShippingCostCalculator CreateCalculator()
        {
            var resolver = new ShippingCostStrategyResolver(new IShippingCostStrategy[]
            {
                new StandardShippingStrategy(),
                new ExpressShippingStrategy(),
                new FreeShippingStrategy()
            });

            return new ShippingCostCalculator(resolver);
        }

        private static CalculateShippingRequest CreateRequest(string method, int quantity, decimal unitPrice, decimal weightKg)
        {
            return new CalculateShippingRequest
            {
                ShippingMethod = method,
                Items = new List<ShippingItemRequest>
                {
                    new ShippingItemRequest
                    {
                        Sku = "SKU-100",
                        ProductName = "Wireless Mouse",
                        Quantity = quantity,
                        UnitPrice = unitPrice,
                        WeightKg = weightKg
                    }
                }
            };
        }
    }
}
