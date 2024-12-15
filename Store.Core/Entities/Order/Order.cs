﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Entities.Order
{
    public class Order:BaseEntity<int>
    {
        public string BuyerEmail {  get; set; }
        public DateTimeOffset? OrderDate { get; set; }= DateTimeOffset.Now;
        public OrderStatus Status { get; set; }=OrderStatus.Pending;

        public Address ShippingAddress { get; set; }
        public int DeliveryMethodId {  get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }

        public ICollection<OrderItem> Items { get; set; }
        public decimal SubTotal {  get; set; }

        public decimal GetTotal()=>SubTotal+DeliveryMethod.Cost;

        public string PaymentInternId {  get; set; }

    }
}
