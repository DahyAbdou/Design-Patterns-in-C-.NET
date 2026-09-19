using System;
using System.Collections.Generic;
using System.Linq;
using static Domain.Enums.Enums;

namespace Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public string CustomerEmail { get; set; } = string.Empty;
        public string ShippingAddress { get; set; } = string.Empty;
        public OrderStatusEnum Status { get; set; }
        public decimal TotalAmount { get; set; }
        public string? PaymentReference { get; set; }
        public string? TrackingNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<OrderItem> Items { get; set; } = new();

        public static Order Create(string customerEmail, string shippingAddress, IEnumerable<OrderItem> items)
        {
            var orderItems = items.ToList();

            return new Order
            {
                Id = Guid.NewGuid(),
                CustomerEmail = customerEmail,
                ShippingAddress = shippingAddress,
                Status = OrderStatusEnum.Pending,
                Items = orderItems,
                TotalAmount = orderItems.Sum(item => item.LineTotal),
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
