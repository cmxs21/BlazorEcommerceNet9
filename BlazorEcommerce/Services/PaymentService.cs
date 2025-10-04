using BlazorEcommerce.Data;
using BlazorEcommerce.Repository.IRepository;
using BlazorEcommerce.Utility;
using Microsoft.AspNetCore.Components;
using Stripe.Checkout;

namespace BlazorEcommerce.Services
{
    public class PaymentService
    {
        private readonly NavigationManager _navigationManager;
        private readonly IOrderRepository _orderRepository;

        public PaymentService(NavigationManager navigationManager, IOrderRepository orderRepository)
        {
            _navigationManager = navigationManager;
            _orderRepository = orderRepository;
        }

        public Session CreateStripeCheckoutSession(OrderHeader orderHeader)
        {
            var domain = _navigationManager.BaseUri;
            var options = new SessionCreateOptions
            {
                SuccessUrl = domain + $"order/confirmation/{{CHECKOUT_SESSION_ID}}",
                CancelUrl = domain + "cart",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
            };

            foreach (var item in orderHeader.OrderDetails)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Price * 100), // Stripe expects the amount in cents
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.ProductName,
                        },
                    },
                    Quantity = item.Count,
                };
                options.LineItems.Add(sessionLineItem);
            }

            var service = new SessionService();
            Session session = service.Create(options);

            // Save the session ID and payment intent ID to the order header
            orderHeader.SessionId = session.Id;
            orderHeader.PaymentIntentId = session.PaymentIntentId;

            //_orderRepository.UpdateStatusAsync(orderHeader.Id, orderHeader.OrderStatus, session.PaymentIntentId).GetAwaiter().GetResult();
            return session;
        }

        public async Task<OrderHeader> CheckPaymentStatusAndUpdateOrderAsync(string sessionId)
        {
            var service = new SessionService();
            Session session = service.Get(sessionId);
            OrderHeader orderHeader = await _orderRepository.GetOrderBySessionIdAsync(sessionId);

            if (session.PaymentStatus.ToLower() == "paid")
            {
                await _orderRepository.UpdateStatusAsync(orderHeader.Id, StaticDetails.StatusApproved, session.PaymentIntentId);
            }

            return orderHeader;
        }
    }
}
