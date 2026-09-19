using FluentValidation;
using System.Collections.Generic;

namespace Application.Request.Order
{
    public class PlaceOrderRequest
    {
        public string CustomerEmail { get; set; } = string.Empty;
        public string ShippingAddress { get; set; } = string.Empty;
        public string PaymentToken { get; set; } = string.Empty;
        public List<PlaceOrderItemRequest> Items { get; set; } = new();
    }

    public class PlaceOrderItemRequest
    {
        public string Sku { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }

    public class PlaceOrderRequestValidator : AbstractValidator<PlaceOrderRequest>
    {
        public PlaceOrderRequestValidator()
        {
            RuleFor(x => x.CustomerEmail).NotEmpty().EmailAddress();
            RuleFor(x => x.ShippingAddress).NotEmpty();
            RuleFor(x => x.PaymentToken).NotEmpty();
            RuleFor(x => x.Items).NotEmpty();
            RuleForEach(x => x.Items).ChildRules(item =>
            {
                item.RuleFor(x => x.Sku).NotEmpty();
                item.RuleFor(x => x.Quantity).GreaterThan(0);
            });
        }
    }
}
