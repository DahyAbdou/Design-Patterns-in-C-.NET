namespace Application.Abstraction.DesignPatterns.Strategy
{
    public interface IDiscountStrategyResolver
    {
        IDiscountStrategy Resolve(string customerType);
    }
}
