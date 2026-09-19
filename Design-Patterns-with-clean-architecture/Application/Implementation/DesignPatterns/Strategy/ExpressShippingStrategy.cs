using Application.Abstraction.DesignPatterns.Strategy;
using System;

namespace Application.Implementation.DesignPatterns.Strategy
{
    /// <summary>
    /// Concrete strategy: weight-based express shipping with a minimum charge.
    /// </summary>
    public class ExpressShippingStrategy : IShippingCostStrategy
    {
        public const string MethodName = "express";
        public const decimal RatePerKg = 8m;
        public const decimal MinimumCharge = 25m;

        public string Method => MethodName;

        public decimal Calculate(ShippingCalculationInput input)
        {
            var weightCharge = input.TotalWeightKg * RatePerKg;
            return Math.Max(weightCharge, MinimumCharge);
        }
    }
}
