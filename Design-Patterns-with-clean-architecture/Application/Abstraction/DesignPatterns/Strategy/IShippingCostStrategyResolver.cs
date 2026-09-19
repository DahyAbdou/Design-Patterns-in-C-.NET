namespace Application.Abstraction.DesignPatterns.Strategy
{
    public interface IShippingCostStrategyResolver
    {
        IShippingCostStrategy Resolve(string method);
    }
}
