namespace Application.Abstraction.DesignPatterns.Adapter
{
    public interface IPaymentProcessorResolver
    {
        IPaymentProcessor Resolve(string provider);
    }
}
