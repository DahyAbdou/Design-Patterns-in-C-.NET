using Application.Abstraction.DesignPatterns.Strategy;

namespace Application.Implementation.DesignPatterns.Strategy
{
    /// <summary>
    /// Concrete strategy: flat shipping rate for a standard order.
    /// </summary>
    public class StandardShippingStrategy : IShippingCostStrategy
    {
        public const string MethodName = "standard";
        public const decimal FlatRate = 15m;

        public string Method => MethodName;

        public decimal Calculate(ShippingCalculationInput input)
        {
            return FlatRate;
        }
    }
}
