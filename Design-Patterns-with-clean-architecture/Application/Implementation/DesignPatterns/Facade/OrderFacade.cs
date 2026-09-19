using Application.Abstraction.DesignPatterns.Facade;
using Application.Common.Exceptions;
using Application.Request.Order;
using Application.Response.Order;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using static Domain.Enums.Enums;

namespace Application.Implementation.DesignPatterns.Facade
{
    /// <summary>
    /// Structural pattern: Facade.
    ///
    /// Intent: hide a set of collaborating services behind one use-case API.
    /// PlaceOrder coordinates stock, payment, persistence, shipping, and email.
    /// The HTTP client never talks to those subsystems, never knows their order,
    /// and never owns compensating actions (release stock / refund) on failure.
    ///
    /// Use when a workflow spans several services and you want one place to change it.
    /// Do not use it as a god class that absorbs unrelated features.
    /// </summary>
    public class OrderFacade : IOrderFacade
    {
        private readonly IInventoryService _inventoryService;
        private readonly IPaymentService _paymentService;
        private readonly IShippingService _shippingService;
        private readonly INotificationService _notificationService;
        private readonly IOrderStore _orderStore;

        public OrderFacade(
            IInventoryService inventoryService,
            IPaymentService paymentService,
            IShippingService shippingService,
            INotificationService notificationService,
            IOrderStore orderStore)
        {
            _inventoryService = inventoryService;
            _paymentService = paymentService;
            _shippingService = shippingService;
            _notificationService = notificationService;
            _orderStore = orderStore;
        }

        public async Task<PlaceOrderResponse> PlaceOrderAsync(PlaceOrderRequest request, CancellationToken cancellationToken = default)
        {
            var reservedItems = new List<OrderItem>();
            string? paymentReference = null;
            string? trackingNumber = null;
            Order? order = null;

            try
            {
                var pricedItems = await PriceAndValidateStockAsync(request.Items, cancellationToken);

                foreach (var item in pricedItems)
                {
                    await _inventoryService.ReserveAsync(item.Sku, item.Quantity, cancellationToken);
                    reservedItems.Add(item);
                }

                order = Order.Create(request.CustomerEmail, request.ShippingAddress, pricedItems);

                var receipt = await _paymentService.ChargeAsync(request.PaymentToken, order.TotalAmount, cancellationToken);
                paymentReference = receipt.PaymentReference;
                order.PaymentReference = paymentReference;
                order.Status = OrderStatusEnum.Paid;

                await _orderStore.SaveAsync(order, cancellationToken);

                var shipment = await _shippingService.ArrangeAsync(order.ShippingAddress, cancellationToken);
                trackingNumber = shipment.TrackingNumber;
                order.TrackingNumber = trackingNumber;
                order.Status = OrderStatusEnum.Shipped;

                await _orderStore.SaveAsync(order, cancellationToken);
                await _notificationService.NotifyOrderPlacedAsync(order.CustomerEmail, order.Id.ToString(), order.TrackingNumber, cancellationToken);

                order.Status = OrderStatusEnum.Completed;
                await _orderStore.SaveAsync(order, cancellationToken);

                return MapToPlaceOrderResponse(order);
            }
            catch
            {
                await CompensatePlaceOrderAsync(reservedItems, paymentReference, trackingNumber, order, cancellationToken);
                throw;
            }
        }

        public async Task<OrderDto> GetOrderAsync(Guid orderId, CancellationToken cancellationToken = default)
        {
            var order = await _orderStore.GetByIdAsync(orderId, cancellationToken);
            return MapToDto(order);
        }

        public async Task<OrderDto> CancelOrderAsync(CancelOrderRequest request, CancellationToken cancellationToken = default)
        {
            var order = await _orderStore.GetByIdAsync(request.OrderId, cancellationToken);

            if (order.Status == OrderStatusEnum.Cancelled)
                throw new CustomHttpException(HttpStatusCode.BadRequest, "Order is already cancelled.");

            if (!string.IsNullOrWhiteSpace(order.TrackingNumber))
                await _shippingService.CancelAsync(order.TrackingNumber, cancellationToken);

            if (!string.IsNullOrWhiteSpace(order.PaymentReference))
                await _paymentService.RefundAsync(order.PaymentReference, order.TotalAmount, cancellationToken);

            foreach (var item in order.Items)
                await _inventoryService.ReleaseAsync(item.Sku, item.Quantity, cancellationToken);

            order.Status = OrderStatusEnum.Cancelled;
            await _orderStore.SaveAsync(order, cancellationToken);
            await _notificationService.NotifyOrderCancelledAsync(order.CustomerEmail, order.Id.ToString(), cancellationToken);

            return MapToDto(order);
        }

        private async Task<List<OrderItem>> PriceAndValidateStockAsync(IEnumerable<PlaceOrderItemRequest> requestedItems, CancellationToken cancellationToken)
        {
            var pricedItems = new List<OrderItem>();

            foreach (var requested in requestedItems)
            {
                var stock = await _inventoryService.GetBySkuAsync(requested.Sku, cancellationToken);

                if (stock.AvailableQuantity < requested.Quantity)
                    throw new CustomHttpException(HttpStatusCode.BadRequest, $"Insufficient stock for '{requested.Sku}'.");

                pricedItems.Add(new OrderItem
                {
                    Sku = stock.Sku,
                    ProductName = stock.Name,
                    Quantity = requested.Quantity,
                    UnitPrice = stock.UnitPrice
                });
            }

            return pricedItems;
        }

        private async Task CompensatePlaceOrderAsync(
            IReadOnlyCollection<OrderItem> reservedItems,
            string? paymentReference,
            string? trackingNumber,
            Order? order,
            CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(trackingNumber))
                await _shippingService.CancelAsync(trackingNumber, cancellationToken);

            if (!string.IsNullOrWhiteSpace(paymentReference))
                await _paymentService.RefundAsync(paymentReference, reservedItems.Sum(item => item.LineTotal), cancellationToken);

            foreach (var item in reservedItems)
                await _inventoryService.ReleaseAsync(item.Sku, item.Quantity, cancellationToken);

            if (order != null)
            {
                order.Status = OrderStatusEnum.Failed;
                await _orderStore.SaveAsync(order, cancellationToken);
            }
        }

        private static PlaceOrderResponse MapToPlaceOrderResponse(Order order)
        {
            return new PlaceOrderResponse
            {
                OrderId = order.Id,
                Status = order.Status.ToString(),
                TotalAmount = order.TotalAmount,
                PaymentReference = order.PaymentReference,
                TrackingNumber = order.TrackingNumber,
                Items = order.Items.Select(MapItem).ToList()
            };
        }

        private static OrderDto MapToDto(Order order)
        {
            return new OrderDto
            {
                OrderId = order.Id,
                CustomerEmail = order.CustomerEmail,
                ShippingAddress = order.ShippingAddress,
                Status = order.Status.ToString(),
                TotalAmount = order.TotalAmount,
                PaymentReference = order.PaymentReference,
                TrackingNumber = order.TrackingNumber,
                CreatedAt = order.CreatedAt,
                Items = order.Items.Select(MapItem).ToList()
            };
        }

        private static OrderItemDto MapItem(OrderItem item)
        {
            return new OrderItemDto
            {
                Sku = item.Sku,
                ProductName = item.ProductName,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                LineTotal = item.LineTotal
            };
        }
    }
}
