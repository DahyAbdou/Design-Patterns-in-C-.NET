using System.Collections.Generic;

namespace Application.Response.Strategy
{
    public class DiscountQuoteResponse
    {
        public string CustomerType { get; set; } = string.Empty;
        public decimal DiscountPercent { get; set; }
        public decimal Subtotal { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TotalToPay { get; set; }
        public List<DiscountQuoteItemDto> Items { get; set; } = new();
    }

    public class DiscountQuoteItemDto
    {
        public string Sku { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
    }
}
