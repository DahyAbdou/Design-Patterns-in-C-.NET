using Application.Abstraction.DesignPatterns.Strategy;
using Application.Request.Strategy;
using Application.Response.Strategy;
using System.Linq;

namespace Application.Implementation.DesignPatterns.Strategy
{
    /// <summary>
    /// Context: totals the purchase, selects a discount strategy by customer type, and computes what to pay.
    /// Without Strategy this would be if/else on "regular" / "member" / "premium".
    /// Adding a new customer type means a new strategy class — this calculator does not change.
    /// </summary>
    public class DiscountCalculator : IDiscountCalculator
    {
        private readonly IDiscountStrategyResolver _strategyResolver;

        public DiscountCalculator(IDiscountStrategyResolver strategyResolver)
        {
            _strategyResolver = strategyResolver;
        }

        public DiscountQuoteResponse Quote(CalculateDiscountRequest request)
        {
            var items = request.Items.Select(item => new DiscountQuoteItemDto
            {
                Sku = item.Sku,
                ProductName = item.ProductName,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                LineTotal = item.Quantity * item.UnitPrice
            }).ToList();

            var input = new DiscountCalculationInput
            {
                Subtotal = items.Sum(item => item.LineTotal)
            };

            var strategy = _strategyResolver.Resolve(request.CustomerType);
            var discountAmount = strategy.Calculate(input);

            return new DiscountQuoteResponse
            {
                CustomerType = strategy.CustomerType,
                DiscountPercent = strategy.DiscountPercent,
                Subtotal = input.Subtotal,
                DiscountAmount = discountAmount,
                TotalToPay = input.Subtotal - discountAmount,
                Items = items
            };
        }
    }
}
