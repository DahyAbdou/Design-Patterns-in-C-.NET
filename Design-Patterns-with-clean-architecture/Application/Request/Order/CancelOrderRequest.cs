using FluentValidation;
using System;

namespace Application.Request.Order
{
    public class CancelOrderRequest
    {
        public Guid OrderId { get; set; }
    }

    public class CancelOrderRequestValidator : AbstractValidator<CancelOrderRequest>
    {
        public CancelOrderRequestValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty();
        }
    }
}
