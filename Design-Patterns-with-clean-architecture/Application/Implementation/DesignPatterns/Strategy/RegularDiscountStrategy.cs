using Application.Abstraction.DesignPatterns.Strategy;

namespace Application.Implementation.DesignPatterns.Strategy
{
    /// <summary>
    /// Concrete strategy: regular customers pay full price.
    /// </summary>
    public class RegularDiscountStrategy : IDiscountStrategy
    {
        public const string CustomerTypeName = "regular";

        public string CustomerType => CustomerTypeName;
        public decimal DiscountPercent => 0m;

        public decimal Calculate(DiscountCalculationInput input)
        {
            return 0m;
        }
    }
}
