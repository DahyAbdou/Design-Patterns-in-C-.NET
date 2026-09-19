using Application.Abstraction.DesignPatterns.Facade;
using Application.Common.Exceptions;
using System.Collections.Concurrent;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Implementation.DesignPatterns.Facade
{
    /// <summary>
    /// In-memory catalog used so the Facade demo can run without a database.
    /// Replace with a repository-backed implementation when you persist products.
    /// </summary>
    public class InventoryService : IInventoryService
    {
        private readonly ConcurrentDictionary<string, ProductStock> _catalog = new()
        {
            ["SKU-100"] = new ProductStock("SKU-100", "Wireless Mouse", 25.00m, 10),
            ["SKU-200"] = new ProductStock("SKU-200", "USB-C Hub", 45.00m, 5),
            ["SKU-300"] = new ProductStock("SKU-300", "Laptop Stand", 60.00m, 0)
        };

        public Task<InventoryItem> GetBySkuAsync(string sku, CancellationToken cancellationToken = default)
        {
            if (!_catalog.TryGetValue(sku, out var stock))
                throw new CustomHttpException(HttpStatusCode.NotFound, $"Product '{sku}' was not found.");

            return Task.FromResult(new InventoryItem
            {
                Sku = stock.Sku,
                Name = stock.Name,
                UnitPrice = stock.UnitPrice,
                AvailableQuantity = stock.AvailableQuantity
            });
        }

        public Task ReserveAsync(string sku, int quantity, CancellationToken cancellationToken = default)
        {
            var item = GetRequired(sku);

            lock (item)
            {
                if (item.AvailableQuantity < quantity)
                    throw new CustomHttpException(HttpStatusCode.BadRequest, $"Insufficient stock for '{sku}'.");

                item.AvailableQuantity -= quantity;
            }

            return Task.CompletedTask;
        }

        public Task ReleaseAsync(string sku, int quantity, CancellationToken cancellationToken = default)
        {
            var item = GetRequired(sku);

            lock (item)
            {
                item.AvailableQuantity += quantity;
            }

            return Task.CompletedTask;
        }

        private ProductStock GetRequired(string sku)
        {
            if (!_catalog.TryGetValue(sku, out var stock))
                throw new CustomHttpException(HttpStatusCode.NotFound, $"Product '{sku}' was not found.");

            return stock;
        }

        private sealed class ProductStock
        {
            public ProductStock(string sku, string name, decimal unitPrice, int availableQuantity)
            {
                Sku = sku;
                Name = name;
                UnitPrice = unitPrice;
                AvailableQuantity = availableQuantity;
            }

            public string Sku { get; }
            public string Name { get; }
            public decimal UnitPrice { get; }
            public int AvailableQuantity { get; set; }
        }
    }
}
