using Application.Abstraction.DesignPatterns.Strategy;
using System;

namespace Application.Implementation.DesignPatterns.Strategy
{
    /// <summary>
    /// Concrete strategy: premium customers get 20% off the purchase subtotal.
    /// </summary>
    public class PremiumDiscountStrategy : IDiscountStrategy
    {
        public const string CustomerTypeName = "premium";

        public string CustomerType => CustomerTypeName;
        public decimal DiscountPercent => 20m;

        public decimal Calculate(DiscountCalculationInput input)
        {
            return decimal.Round(input.Subtotal * DiscountPercent / 100m, 2, MidpointRounding.AwayFromZero);
        }
    }
}
