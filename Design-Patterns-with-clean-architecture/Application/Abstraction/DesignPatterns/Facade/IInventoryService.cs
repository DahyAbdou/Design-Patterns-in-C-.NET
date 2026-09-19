using System.Threading;
using System.Threading.Tasks;

namespace Application.Abstraction.DesignPatterns.Facade
{
    public interface IInventoryService
    {
        Task<InventoryItem> GetBySkuAsync(string sku, CancellationToken cancellationToken = default);
        Task ReserveAsync(string sku, int quantity, CancellationToken cancellationToken = default);
        Task ReleaseAsync(string sku, int quantity, CancellationToken cancellationToken = default);
    }

    public class InventoryItem
    {
        public string Sku { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public decimal UnitPrice { get; init; }
        public int AvailableQuantity { get; init; }
    }
}
