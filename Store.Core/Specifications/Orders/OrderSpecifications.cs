using Store.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Specifications.Orders
{
    public class OrderSpecifications:BaseSpecifications<Order,int>
    {

        public OrderSpecifications()
        {
            

        }

        public OrderSpecifications(string buyerEmail,int orderId)
            :base(o=>o.BuyerEmail==buyerEmail && o.Id==orderId)
        {
            Include.Add(o => o.DeliveryMethod);
            Include.Add(o => o.Items);
        }
        public OrderSpecifications(string buyerEmail)
           : base(o => o.BuyerEmail == buyerEmail )
        {
            Include.Add(o => o.DeliveryMethod);
            Include.Add(o => o.Items);
        }
    }
}
