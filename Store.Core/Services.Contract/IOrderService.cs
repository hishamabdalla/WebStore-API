using Store.Core.Dtos.Orders;
using Store.Core.Entities.Order;
using Store.Core.Helper;
using Store.Core.Specifications.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Services.Contract
{
    public interface IOrderService
    {
        /// <summary>
        /// Creates a new order for a specific user.
        /// </summary>
        /// <param name="buyerEmail">The email of the user placing the order.</param>
        /// <param name="basketId">The ID of the basket associated with the order.</param>
        /// <param name="deliveryMethodId">The delivery method chosen for the order.</param>
        /// <param name="shippingAddress">The shipping address for the order.</param>
        /// <returns>The created order or null if creation fails.</returns>
        Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress);

        /// <summary>
        /// Retrieves all orders for a specific user.
        /// </summary>
        /// <param name="buyerEmail">The email of the user whose orders are being retrieved.</param>
        /// <returns>A collection of orders or null if no orders are found.</returns>
        Task<IEnumerable<Order>?> GetOrdersForSpecificUser(string buyerEmail);

        /// <summary>
        /// Retrieves a specific order by its ID for a given user.
        /// </summary>
        /// <param name="buyerEmail">The email of the user.</param>
        /// <param name="orderId">The ID of the order.</param>
        /// <returns>The order or null if it doesn't exist.</returns>
        Task<Order?> GetOrderByIdForSpecificUser(string buyerEmail, int orderId);

        /// <summary>
        /// Cancels an order for a specific user.
        /// </summary>
        /// <param name="buyerEmail">The email of the user.</param>
        /// <param name="orderId">The ID of the order to cancel.</param>
        /// <returns>True if cancellation is successful, false otherwise.</returns>
        Task<bool> CancelOrderAsync(string buyerEmail, int orderId);



        /// <summary>
        /// Retrieves all available orders (admin use).
        /// </summary>
        /// <returns>A collection of all orders.</returns>
        Task<IEnumerable<OrderToReturnDto>> GetAllOrdersAsync();


    }
}



