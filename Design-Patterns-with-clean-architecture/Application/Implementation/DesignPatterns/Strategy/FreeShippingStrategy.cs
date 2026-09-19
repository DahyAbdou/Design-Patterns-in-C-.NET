using Application.Abstraction.DesignPatterns.Strategy;
using Application.Common.Exceptions;
using System.Net;

namespace Application.Implementation.DesignPatterns.Strategy
{
    /// <summary>
    /// Concrete strategy: free shipping when the order subtotal meets the threshold.
    /// </summary>
    public class FreeShippingStrategy : IShippingCostStrategy
    {
        public const string MethodName = "free";
        public const decimal MinimumSubtotal = 200m;

        public string Method => MethodName;

        public decimal Calculate(ShippingCalculationInput input)
        {
            if (input.Subtotal < MinimumSubtotal)
                throw new CustomHttpException(
                    HttpStatusCode.BadRequest,
                    $"Free shipping requires a subtotal of at least {MinimumSubtotal:0}.");

            return 0m;
        }
    }
}
