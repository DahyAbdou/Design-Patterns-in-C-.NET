using FluentValidation;

namespace Application.Request.Adapter
{
    public class ProcessPaymentRequest
    {
        public string Provider { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "USD";
        public string PaymentToken { get; set; } = string.Empty;
        public string PayerEmail { get; set; } = string.Empty;
    }

    public class ProcessPaymentRequestValidator : AbstractValidator<ProcessPaymentRequest>
    {
        public ProcessPaymentRequestValidator()
        {
            RuleFor(x => x.Provider).NotEmpty();
            RuleFor(x => x.Amount).GreaterThan(0);
            RuleFor(x => x.Currency).NotEmpty();
        }
    }
}
