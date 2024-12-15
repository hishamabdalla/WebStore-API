using Store.Core;
using Store.Core.Entities;
using Store.Core.Entities.Order;
using Store.Core.Repositories.Interfaces;
using Store.Core.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IBasketRepository basketRepository;

        public OrderService(IUnitOfWork unitOfWork,IBasketRepository basketRepository)
        {
            this.unitOfWork = unitOfWork;
            this.basketRepository = basketRepository;
        }

      

        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
        {
            var basket=await basketRepository.GetBasketAsync(basketId);
            if(basket == null) return null;

            var orderItems=new List<OrderItem>();

            if(basket.BasketItems.Count() > 0)
            {
                foreach (var item in basket.BasketItems )
                {
                    var product=await unitOfWork.Repository<Product,int>().GetAsync(item.Id);

                    var ProductOrderedItem = new ProductItemOrder(product.Id,product.Name,product.PictureUrl);
                        
                    var orderItem = new OrderItem(ProductOrderedItem,product.Price,item.Quantity);

                    orderItems.Add(orderItem);
                }
            }

           var deliveryMethod = await unitOfWork.Repository<DeliveryMethod, int>().GetAsync(deliveryMethodId);

            var subTotal=orderItems.Sum(i=>i.Price*i.Quantity);
            var order = new Order(buyerEmail, shippingAddress, deliveryMethod, orderItems, subTotal, "");

           await unitOfWork.Repository<Order,int>().AddAsync(order);
           var result= await unitOfWork.CompleteAsync();

            if(result<=0) return null;
            return order;
        }

        public Task<Order?> GetOrderByIdForSpecificUser(string buyerEmail, int orderId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Order>?> GetOrderForSpecificUser(string buyerEmail)
        {
            throw new NotImplementedException();
        }
    }
}
