using Application.Abstraction.DesignPatterns.Strategy;
using System;

namespace Application.Implementation.DesignPatterns.Strategy
{
    /// <summary>
    /// Concrete strategy: members get 10% off the purchase subtotal.
    /// </summary>
    public class MemberDiscountStrategy : IDiscountStrategy
    {
        public const string CustomerTypeName = "member";

        public string CustomerType => CustomerTypeName;
        public decimal DiscountPercent => 10m;

        public decimal Calculate(DiscountCalculationInput input)
        {
            return decimal.Round(input.Subtotal * DiscountPercent / 100m, 2, MidpointRounding.AwayFromZero);
        }
    }
}
