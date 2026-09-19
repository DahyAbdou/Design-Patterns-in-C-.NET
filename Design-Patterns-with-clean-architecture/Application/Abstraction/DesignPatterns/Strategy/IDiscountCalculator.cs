using Application.Request.Strategy;
using Application.Response.Strategy;

namespace Application.Abstraction.DesignPatterns.Strategy
{
    /// <summary>
    /// Context: applies the customer-type discount strategy to a purchase and returns the amount to pay.
    /// </summary>
    public interface IDiscountCalculator
    {
        DiscountQuoteResponse Quote(CalculateDiscountRequest request);
    }
}
