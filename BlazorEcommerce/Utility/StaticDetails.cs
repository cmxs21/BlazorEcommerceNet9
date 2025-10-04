using BlazorEcommerce.Data;

namespace BlazorEcommerce.Utility
{
    public static class StaticDetails
    {
        public static string RoleAdmin = "Admin";
        public static string RoleCustomer = "Customer";

        public static string StatusPending = "Pending";
        public static string StatusApproved = "Approved";
        public static string StatusReadyForPickUp = "ReadyForPickUp";
        public static string StatusCompleted = "Completed";
        public static string StatusCancelled = "Cancelled";

        //Convert ShoppingCart to OrderDetail List 
        public static List<OrderDetail> ConvertShoppingCartToOrderDetails(List<ShoppingCart> shoppingCarts)
        {
            List<OrderDetail> orderDetails = new List<OrderDetail>();

            foreach (var cart in shoppingCarts)
            {
                OrderDetail orderDetail = new OrderDetail
                {
                    ProductId = cart.ProductId,
                    Count = cart.Quantity,
                    Price = (double)cart.Product.Price,
                    ProductName = cart.Product.Name
                };
                orderDetails.Add(orderDetail);
            }
            return orderDetails;
        }
    }
}
