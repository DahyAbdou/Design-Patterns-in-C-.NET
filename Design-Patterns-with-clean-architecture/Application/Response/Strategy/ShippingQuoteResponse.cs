using System.Collections.Generic;

namespace Application.Response.Strategy
{
    public class ShippingQuoteResponse
    {
        public string ShippingMethod { get; set; } = string.Empty;
        public decimal Subtotal { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal TotalWeightKg { get; set; }
        public List<ShippingQuoteItemDto> Items { get; set; } = new();
    }

    public class ShippingQuoteItemDto
    {
        public string Sku { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
        public decimal WeightKg { get; set; }
    }
}
