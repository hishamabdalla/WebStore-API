using Microsoft.Extensions.Configuration;
using Store.Core;
using Store.Core.Entities;
using Store.Core.Entities.Order;
using Store.Core.Repositories.Interfaces;
using Store.Core.Services.Contract;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product= Store.Core.Entities.Product;
namespace Store.Service.Services.Payments
{
    public class PaymentService : IPaymentService
    {
        private readonly IBasketRepository basketRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IConfiguration configuration;

        public PaymentService(IBasketRepository basketRepository,IUnitOfWork unitOfWork,IConfiguration configuration)
        {
            this.basketRepository = basketRepository;
            this.unitOfWork = unitOfWork;
            this.configuration = configuration;
        }
        public async Task<UserBasket> SetPaymentIntentIdAsync(string basketId)
        {
            StripeConfiguration.ApiKey = configuration["Stripe:SecretKey "];
            var basket= await basketRepository.GetBasketAsync(basketId);
            if (basket == null) return null;



            var shippingPrice = 0m;

            if (basket.DeliveryMethodId.HasValue)
            {
              var deliveryMethod= await unitOfWork.Repository<DeliveryMethod, int>().GetAsync(basket.DeliveryMethodId.Value);
                shippingPrice = deliveryMethod.Cost;
            }

            if (basket.BasketItems.Count() > 0)
            {
                foreach (var item in basket.BasketItems)
                {
                   var product=await unitOfWork.Repository<Product, int>().GetAsync(item.Id);

                    if (item.Price != product.Price)
                    {
                        item.Price = product.Price;
                    }
                }
            }

            var subTotal=basket.BasketItems.Sum(i=>i.Price*i.Quantity);

            var service = new PaymentIntentService();


            PaymentIntent paymentIntent;
            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount= (long)(subTotal*100+shippingPrice*100),
                    PaymentMethodTypes=new List<string>() { "cart"},
                    Currency="usd"
                };
                paymentIntent = await service.CreateAsync(options);
                basket.PaymentIntentId=paymentIntent.Id;
                basket.ClientSecret= paymentIntent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)(subTotal * 100 + shippingPrice * 100),
                   
                };
                paymentIntent = await service.UpdateAsync(basket.PaymentIntentId,options);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }

          basket= await  basketRepository.SetBasketAsync(basket);
            if (basket is null) return null;
            return basket;
        }
    }
}
