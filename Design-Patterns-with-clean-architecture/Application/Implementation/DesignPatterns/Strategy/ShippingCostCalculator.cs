using Application.Abstraction.DesignPatterns.Strategy;
using Application.Request.Strategy;
using Application.Response.Strategy;
using System.Linq;

namespace Application.Implementation.DesignPatterns.Strategy
{
    /// <summary>
    /// Context: builds an order quote, selects a shipping strategy, and delegates the cost calculation.
    /// Adding a new shipping method means adding a strategy class — this calculator does not change.
    /// </summary>
    public class ShippingCostCalculator : IShippingCostCalculator
    {
        private readonly IShippingCostStrategyResolver _strategyResolver;

        public ShippingCostCalculator(IShippingCostStrategyResolver strategyResolver)
        {
            _strategyResolver = strategyResolver;
        }

        public ShippingQuoteResponse Quote(CalculateShippingRequest request)
        {
            var items = request.Items.Select(item => new ShippingQuoteItemDto
            {
                Sku = item.Sku,
                ProductName = item.ProductName,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                LineTotal = item.Quantity * item.UnitPrice,
                WeightKg = item.WeightKg
            }).ToList();

            var input = new ShippingCalculationInput
            {
                Subtotal = items.Sum(item => item.LineTotal),
                TotalWeightKg = items.Sum(item => item.WeightKg * item.Quantity),
                ItemCount = items.Sum(item => item.Quantity)
            };

            var strategy = _strategyResolver.Resolve(request.ShippingMethod);
            var shippingCost = strategy.Calculate(input);

            return new ShippingQuoteResponse
            {
                ShippingMethod = strategy.Method,
                Subtotal = input.Subtotal,
                ShippingCost = shippingCost,
                GrandTotal = input.Subtotal + shippingCost,
                TotalWeightKg = input.TotalWeightKg,
                Items = items
            };
        }
    }
}
