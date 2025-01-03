﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Entities.Order
{
    public class Order:BaseEntity<int>
    {
        public Order()
        {
            
        }
        public Order(string buyerEmail, Address shippingAddress,  DeliveryMethod deliveryMethod, ICollection<OrderItem> items, decimal subTotal, string paymentInternId)
        {
            BuyerEmail = buyerEmail;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            Items = items;
            SubTotal = subTotal;
            PaymentInternId = paymentInternId;
            Total= SubTotal + DeliveryMethod.Cost;
        }

        public string BuyerEmail {  get; set; }
        public DateTimeOffset? OrderDate { get; set; }= DateTimeOffset.Now;
        public OrderStatus Status { get; set; }=OrderStatus.Pending;

        public Address ShippingAddress { get; set; }
        public int DeliveryMethodId {  get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }

        public ICollection<OrderItem> Items { get; set; }
        public decimal SubTotal {  get; set; }
        [NotMapped]
        public decimal Total {  get; set; }

        public string PaymentInternId {  get; set; }

    }
}
