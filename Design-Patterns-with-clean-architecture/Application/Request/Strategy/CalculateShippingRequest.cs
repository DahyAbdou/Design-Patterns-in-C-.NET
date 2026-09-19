using FluentValidation;
using System.Collections.Generic;

namespace Application.Request.Strategy
{
    public class CalculateShippingRequest
    {
        public string ShippingMethod { get; set; } = string.Empty;
        public List<ShippingItemRequest> Items { get; set; } = new();
    }

    public class ShippingItemRequest
    {
        public string Sku { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal WeightKg { get; set; }
    }

    public class CalculateShippingRequestValidator : AbstractValidator<CalculateShippingRequest>
    {
        public CalculateShippingRequestValidator()
        {
            RuleFor(x => x.ShippingMethod).NotEmpty();
            RuleFor(x => x.Items).NotEmpty();
            RuleForEach(x => x.Items).ChildRules(item =>
            {
                item.RuleFor(x => x.Sku).NotEmpty();
                item.RuleFor(x => x.Quantity).GreaterThan(0);
                item.RuleFor(x => x.UnitPrice).GreaterThan(0);
                item.RuleFor(x => x.WeightKg).GreaterThanOrEqualTo(0);
            });
        }
    }
}
