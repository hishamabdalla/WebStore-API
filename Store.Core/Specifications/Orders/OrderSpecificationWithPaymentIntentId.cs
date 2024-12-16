using Store.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Specifications.Orders
{
    public class OrderSpecificationWithPaymentIntentId:BaseSpecifications<Order,int>
    {
        public OrderSpecificationWithPaymentIntentId(string paymentIntentId):base(o=>o.PaymentInternId==paymentIntentId) 
        {
            Include.Add(o => o.DeliveryMethod);
            Include.Add(o => o.Items);

        }
    }
}
