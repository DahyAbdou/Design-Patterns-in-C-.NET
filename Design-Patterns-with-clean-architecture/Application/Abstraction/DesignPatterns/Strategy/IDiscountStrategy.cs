namespace Application.Abstraction.DesignPatterns.Strategy
{
    /// <summary>
    /// Strategy: discount algorithm for a purchase, selected by customer type.
    /// Replaces if/else on regular vs member vs premium.
    /// </summary>
    public interface IDiscountStrategy
    {
        string CustomerType { get; }
        decimal DiscountPercent { get; }
        decimal Calculate(DiscountCalculationInput input);
    }

    public class DiscountCalculationInput
    {
        public decimal Subtotal { get; init; }
    }
}
