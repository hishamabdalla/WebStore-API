using AutoMapper;
using Store.Core;
using Store.Core.Dtos.Orders;
using Store.Core.Entities;
using Store.Core.Entities.Order;
using Store.Core.Helper;
using Store.Core.Repositories.Interfaces;
using Store.Core.Services.Contract;
using Store.Core.Specifications.Orders;
using Store.Core.Specifications.Products;
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
        private readonly IPaymentService paymentService;
        private readonly IMapper mapper;

        public OrderService(IUnitOfWork unitOfWork, IBasketRepository basketRepository, IPaymentService paymentService, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.basketRepository = basketRepository;
            this.paymentService = paymentService;
            this.mapper = mapper;
        }

        public async Task<bool> CancelOrderAsync(string buyerEmail, int orderId)
        {
            var order = await GetOrderByIdForSpecificUser(buyerEmail, orderId);
            if (order == null || order.Status == OrderStatus.Cancelled) return false;

            order.Status = OrderStatus.Cancelled;
            unitOfWork.Repository<Order, int>().Update(order);

            return await unitOfWork.CompleteAsync() > 0;
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
            var Total=subTotal+deliveryMethod.Cost;
            if (!string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var spec = new OrderSpecificationWithPaymentIntentId(basket.PaymentIntentId);
               var ExistOrder=await unitOfWork.Repository<Order, int>().GetWithSpecAsync(spec);
                unitOfWork.Repository<Order,int>().Delete(ExistOrder);

            }
            var basketDto= await paymentService.SetPaymentIntentIdAsync(basketId);
            var order = new Order(buyerEmail, shippingAddress, deliveryMethod, orderItems, subTotal, basketDto.PaymentIntentId );

           await unitOfWork.Repository<Order,int>().AddAsync(order);
           var result= await unitOfWork.CompleteAsync();

            if(result<=0) return null;
            return order;
        }

        public async Task<IEnumerable<OrderToReturnDto>> GetAllOrdersAsync()
        {
            var spec = new OrderSpecifications();
            var orders = await unitOfWork.Repository<Order, int>().GetAllWithSpecAsync(spec);
            return  mapper.Map<IEnumerable<OrderToReturnDto>>(orders);
            
        }

        public async Task<Order?> GetOrderByIdForSpecificUser(string buyerEmail, int orderId)
        {
            var spec=new OrderSpecifications(buyerEmail, orderId);
            var order=await unitOfWork.Repository<Order, int>().GetWithSpecAsync(spec);

            if(order==null) return null;
            return order;
        }

        public async Task<IEnumerable<Order>?> GetOrdersForSpecificUser(string buyerEmail)
        {
            var spec = new OrderSpecifications(buyerEmail);
            var orders = await unitOfWork.Repository<Order, int>().GetAllWithSpecAsync(spec);

            if (orders == null) return null;
            return orders;
        }

    }
}
