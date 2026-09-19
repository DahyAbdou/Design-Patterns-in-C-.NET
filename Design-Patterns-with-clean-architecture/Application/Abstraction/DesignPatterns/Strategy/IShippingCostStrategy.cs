namespace Application.Abstraction.DesignPatterns.Strategy
{
    /// <summary>
    /// Strategy: one algorithm for calculating shipping on an order quote.
    /// Swap the implementation to change the rule without changing the calculator.
    /// </summary>
    public interface IShippingCostStrategy
    {
        string Method { get; }
        decimal Calculate(ShippingCalculationInput input);
    }

    public class ShippingCalculationInput
    {
        public decimal Subtotal { get; init; }
        public decimal TotalWeightKg { get; init; }
        public int ItemCount { get; init; }
    }
}
