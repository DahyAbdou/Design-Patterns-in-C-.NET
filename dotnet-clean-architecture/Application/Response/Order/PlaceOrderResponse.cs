using System;
using System.Collections.Generic;

namespace Application.Response.Order
{
    public class PlaceOrderResponse
    {
        public Guid OrderId { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string? PaymentReference { get; set; }
        public string? TrackingNumber { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
    }

    public class OrderDto
    {
        public Guid OrderId { get; set; }
        public string CustomerEmail { get; set; } = string.Empty;
        public string ShippingAddress { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string? PaymentReference { get; set; }
        public string? TrackingNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
    }

    public class OrderItemDto
    {
        public string Sku { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
    }
}
