using Application.Request.Strategy;
using Application.Response.Strategy;

namespace Application.Abstraction.DesignPatterns.Strategy
{
    /// <summary>
    /// Context: the Order shipping quote API. It picks a strategy and delegates the calculation.
    /// </summary>
    public interface IShippingCostCalculator
    {
        ShippingQuoteResponse Quote(CalculateShippingRequest request);
    }
}
