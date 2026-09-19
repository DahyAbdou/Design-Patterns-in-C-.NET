using Application.Common.Exceptions;
using Application.Abstraction.DesignPatterns.Facade;
using Application.Implementation.DesignPatterns.Facade;
using Application.Request.Order;
using Domain.Entities;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using static Domain.Enums.Enums;

namespace Application.UnitTest
{
    public class OrderFacade_Tests
    {
        private Mock<IInventoryService> _inventory = null!;
        private Mock<IPaymentService> _payment = null!;
        private Mock<IShippingService> _shipping = null!;
        private Mock<INotificationService> _notification = null!;
        private Mock<IOrderStore> _orderStore = null!;
        private OrderFacade _facade = null!;

        [SetUp]
        public void SetUp()
        {
            _inventory = new Mock<IInventoryService>();
            _payment = new Mock<IPaymentService>();
            _shipping = new Mock<IShippingService>();
            _notification = new Mock<INotificationService>();
            _orderStore = new Mock<IOrderStore>();
            _facade = new OrderFacade(
                _inventory.Object,
                _payment.Object,
                _shipping.Object,
                _notification.Object,
                _orderStore.Object);
        }

        [Test]
        public async Task PlaceOrder_WhenStockAndPaymentSucceed_CoordinatesAllSubsystems()
        {
            var request = CreateRequest();
            SetupAvailableStock("SKU-100", "Wireless Mouse", 25.00m, 10);
            _payment
                .Setup(p => p.ChargeAsync(request.PaymentToken, 50.00m, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PaymentReceipt { PaymentReference = "PAY-OK", Amount = 50.00m });
            _shipping
                .Setup(s => s.ArrangeAsync(request.ShippingAddress, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Shipment { TrackingNumber = "TRK-1", Carrier = "Demo Express" });

            var result = await _facade.PlaceOrderAsync(request);

            Assert.AreEqual("Completed", result.Status);
            Assert.AreEqual(50.00m, result.TotalAmount);
            Assert.AreEqual("PAY-OK", result.PaymentReference);
            Assert.AreEqual("TRK-1", result.TrackingNumber);
            _inventory.Verify(i => i.ReserveAsync("SKU-100", 2, It.IsAny<CancellationToken>()), Times.Once);
            _payment.Verify(p => p.ChargeAsync(request.PaymentToken, 50.00m, It.IsAny<CancellationToken>()), Times.Once);
            _shipping.Verify(s => s.ArrangeAsync(request.ShippingAddress, It.IsAny<CancellationToken>()), Times.Once);
            _notification.Verify(n => n.NotifyOrderPlacedAsync(request.CustomerEmail, It.IsAny<string>(), "TRK-1", It.IsAny<CancellationToken>()), Times.Once);
            _orderStore.Verify(s => s.SaveAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        }

        [Test]
        public void PlaceOrder_WhenOutOfStock_DoesNotChargePayment()
        {
            var request = CreateRequest();
            SetupAvailableStock("SKU-100", "Wireless Mouse", 25.00m, 1);

            Assert.ThrowsAsync<CustomHttpException>(async () => await _facade.PlaceOrderAsync(request));

            _inventory.Verify(i => i.ReserveAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
            _payment.Verify(p => p.ChargeAsync(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public void PlaceOrder_WhenPaymentFails_ReleasesReservedInventory()
        {
            var request = CreateRequest();
            SetupAvailableStock("SKU-100", "Wireless Mouse", 25.00m, 10);
            _payment
                .Setup(p => p.ChargeAsync(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new CustomHttpException(HttpStatusCode.BadRequest, "Payment was declined."));

            Assert.ThrowsAsync<CustomHttpException>(async () => await _facade.PlaceOrderAsync(request));

            _inventory.Verify(i => i.ReserveAsync("SKU-100", 2, It.IsAny<CancellationToken>()), Times.Once);
            _inventory.Verify(i => i.ReleaseAsync("SKU-100", 2, It.IsAny<CancellationToken>()), Times.Once);
            _shipping.Verify(s => s.ArrangeAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public void PlaceOrder_WhenShippingFails_RefundsPaymentAndReleasesStock()
        {
            var request = CreateRequest();
            SetupAvailableStock("SKU-100", "Wireless Mouse", 25.00m, 10);
            _payment
                .Setup(p => p.ChargeAsync(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PaymentReceipt { PaymentReference = "PAY-OK", Amount = 50.00m });
            _shipping
                .Setup(s => s.ArrangeAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new CustomHttpException(HttpStatusCode.BadRequest, "Carrier unavailable."));

            Assert.ThrowsAsync<CustomHttpException>(async () => await _facade.PlaceOrderAsync(request));

            _payment.Verify(p => p.RefundAsync("PAY-OK", 50.00m, It.IsAny<CancellationToken>()), Times.Once);
            _inventory.Verify(i => i.ReleaseAsync("SKU-100", 2, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task CancelOrder_WhenOrderExists_RefundsRestocksAndNotifies()
        {
            var order = Order.Create("buyer@example.com", "Riyadh", new List<OrderItem>
            {
                new OrderItem { Sku = "SKU-100", ProductName = "Wireless Mouse", Quantity = 2, UnitPrice = 25.00m }
            });
            order.PaymentReference = "PAY-OK";
            order.TrackingNumber = "TRK-1";
            order.Status = OrderStatusEnum.Completed;

            _orderStore
                .Setup(s => s.GetByIdAsync(order.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);

            var result = await _facade.CancelOrderAsync(new CancelOrderRequest { OrderId = order.Id });

            Assert.AreEqual("Cancelled", result.Status);
            _shipping.Verify(s => s.CancelAsync("TRK-1", It.IsAny<CancellationToken>()), Times.Once);
            _payment.Verify(p => p.RefundAsync("PAY-OK", 50.00m, It.IsAny<CancellationToken>()), Times.Once);
            _inventory.Verify(i => i.ReleaseAsync("SKU-100", 2, It.IsAny<CancellationToken>()), Times.Once);
            _notification.Verify(n => n.NotifyOrderCancelledAsync("buyer@example.com", order.Id.ToString(), It.IsAny<CancellationToken>()), Times.Once);
        }

        private void SetupAvailableStock(string sku, string name, decimal unitPrice, int quantity)
        {
            _inventory
                .Setup(i => i.GetBySkuAsync(sku, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new InventoryItem
                {
                    Sku = sku,
                    Name = name,
                    UnitPrice = unitPrice,
                    AvailableQuantity = quantity
                });
        }

        private static PlaceOrderRequest CreateRequest()
        {
            return new PlaceOrderRequest
            {
                CustomerEmail = "buyer@example.com",
                ShippingAddress = "Riyadh",
                PaymentToken = "tok_ok",
                Items = new List<PlaceOrderItemRequest>
                {
                    new PlaceOrderItemRequest { Sku = "SKU-100", Quantity = 2 }
                }
            };
        }
    }
}
