using FluentValidation;
using System.Collections.Generic;

namespace Application.Request.Strategy
{
    public class CalculateDiscountRequest
    {
        public string CustomerType { get; set; } = string.Empty;
        public List<DiscountItemRequest> Items { get; set; } = new();
    }

    public class DiscountItemRequest
    {
        public string Sku { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class CalculateDiscountRequestValidator : AbstractValidator<CalculateDiscountRequest>
    {
        public CalculateDiscountRequestValidator()
        {
            RuleFor(x => x.CustomerType).NotEmpty();
            RuleFor(x => x.Items).NotEmpty();
            RuleForEach(x => x.Items).ChildRules(item =>
            {
                item.RuleFor(x => x.Sku).NotEmpty();
                item.RuleFor(x => x.Quantity).GreaterThan(0);
                item.RuleFor(x => x.UnitPrice).GreaterThan(0);
            });
        }
    }
}
